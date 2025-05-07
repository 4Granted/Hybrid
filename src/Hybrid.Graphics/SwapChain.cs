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

using Hybrid.Graphics.Native;
using Hybrid.Graphics.Textures;
using System.Diagnostics;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a swapchain.
/// </summary>
public sealed class SwapChain : DeviceResource, ISwapChain
{
    /// <summary>
    /// Gets the native swapchain.
    /// </summary>
    public ISwapChainImpl Impl { get; }
    ITextureImpl ITexture.Impl => Impl.Texture;

    /// <inheritdoc/>
    public int Width => Impl.Width;

    /// <inheritdoc/>
    public int Height => Impl.Height;

    /// <summary>
    /// An event invoked upon the swapchain surface being resized.
    /// </summary>
    public event Action<int, int>? OnResize;

    /// <summary>
    /// Constructs a swapchain.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the swapchain.</param>
    /// <param name="handle">The handle of the swapchain.</param>
    /// <param name="format">The format of the swapchain.</param>
    /// <param name="width">The initial width of the swapchain.</param>
    /// <param name="height">The initial height of the swapchain.</param>
    public SwapChain(
        GraphicsDevice graphicsDevice,
        nint handle, TextureFormat format,
        int width, int height)
        : base(graphicsDevice)
    {
        Debug.Assert(handle != nint.Zero);

        var description = new SwapChainDescription
        {
            Handle = handle,
            Format = format,
            Width = (uint)width,
            Height = (uint)height,
        };

        Impl = Factory.CreateSwapChain(ref description);
    }

    /// <summary>
    /// Constructs a swapchain.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the swapchain.</param>
    /// <param name="handle">The handle of the swapchain.</param>
    /// <param name="width">The initial width of the swapchain.</param>
    /// <param name="height">The initial height of the swapchain.</param>
    public SwapChain(GraphicsDevice graphicsDevice, nint handle, int width, int height)
        : this(graphicsDevice, handle, TextureFormat.Rgba8UNorm, width, height) { }

    /// <summary>
    /// Constructs a swapchain.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the swapchain.</param>
    /// <param name="handle">The handle of the swapchain.</param>
    /// <param name="format">The format of the swapchain.</param>
    public SwapChain(GraphicsDevice graphicsDevice, nint handle, TextureFormat format)
        : this(graphicsDevice, handle, format, 0, 0) { }

    /// <summary>
    /// Constructs a swapchain.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the swapchain.</param>
    /// <param name="handle">The handle of the swapchain.</param>
    public SwapChain(GraphicsDevice graphicsDevice, nint handle)
        : this(graphicsDevice, handle, TextureFormat.Rgba8UNorm) { }

    /// <inheritdoc/>
    public bool Present() => Impl.Present();

    /// <inheritdoc/>
    public void Resize(int width, int height)
    {
        width = int.Max(width, 1);
        height = int.Max(height, 1);

        Impl.Resize(width, height);

        OnResize?.Invoke(width, height);

        GraphicsDevice.Reset();
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            Impl?.Dispose();
        }
    }
}
