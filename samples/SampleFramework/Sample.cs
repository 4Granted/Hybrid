// Hybrid - A versatile framework for application development.
// Copyright (C) 2024  Fielding Baran
//
// Licensed under the Apache License, Version 2.0 (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Hybrid;
using Hybrid.Graphics;
using Silk.NET.Windowing;

namespace SampleFramework;

/// <summary>
/// Represents a sample application.
/// </summary>
public abstract class Sample : Resource
{
    /// <summary>
    /// Gets the graphics device of the sample.
    /// </summary>
    protected GraphicsDevice GraphicsDevice => graphicsDevice ?? throw new InvalidOperationException();

    /// <summary>
    /// Gets the swap chain of the sample.
    /// </summary>
    protected SwapChain SwapChain => swapChain ?? throw new InvalidOperationException();

    private IWindow? window;
    private GraphicsDevice? graphicsDevice;
    private SwapChain? swapChain;
    private int frameCount;

    /// <summary>
    /// Runs the sample application.
    /// </summary>
    public void Start()
    {
        var options = WindowOptions.Default with
        {
            Title = "Sample",
            Size = new(800, 600),
            API = GraphicsAPI.None,
            ShouldSwapAutomatically = false,
            IsContextControlDisabled = true,
        };

        window = Window.Create(options);

        window.Load += () =>
        {
            graphicsDevice = GraphicsDevice.Create(GraphicsOptions.Default);

            var nativeHandle = window.Native switch
            {
                { DXHandle: nint handle } => handle,
                { Glfw: nint handle } => handle,
                { X11: (nint handle, nuint _) } => handle,
                _ => throw new NotSupportedException(),
            };

            swapChain = new SwapChain(graphicsDevice, nativeHandle, window.Size.X, window.Size.Y);

            OnInitialize();
        };

        window.Update += time =>
        {
            OnUpdate(new GameTime((float)time, frameCount));
        };

        window.Render += time =>
        {
            OnRender(new GameTime((float)time, frameCount++));

            SwapChain.Present();
        };

        window.FramebufferResize += size =>
        {
            SwapChain.Resize(size.X, size.Y);

            OnResize(size.X, size.Y);
        };

        window.Run();
    }

    /// <summary>
    /// A method invoked upon the sample being initialized.
    /// </summary>
    protected virtual void OnInitialize() { }

    /// <summary>
    /// A method invoked upon the sample being updated.
    /// </summary>
    /// <param name="time">The game time.</param>
    protected virtual void OnUpdate(GameTime time) { }

    /// <summary>
    /// A method invoked upon the sample being rendered.
    /// </summary>
    /// <param name="time">The game time.</param>
    protected virtual void OnRender(GameTime time) { }

    /// <summary>
    /// A method invoked upon the sample being resized.
    /// </summary>
    /// <param name="width">The new width.</param>
    /// <param name="height">The new height.</param>
    protected virtual void OnResize(int width, int height) { }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            graphicsDevice?.Dispose();
            window?.Dispose();
        }
    }
}
