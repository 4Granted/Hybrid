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

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Textures;
using SkiaSharp;

namespace Hybrid.Skia;

/// <summary>
/// Represents a Skia surface.
/// </summary>
public class SkiaSurface : DeviceResource, ISkiaSurface
{
    /// <inheritdoc/>
    public Texture2D Image { get; }

    /// <inheritdoc/>
    public int Width { get; private set; }

    /// <inheritdoc/>
    public int Height { get; private set; }

    private SKSurface? currentSurface;

    public SkiaSurface(
        GraphicsDevice graphicsDevice,
        int width, int height)
        : base(graphicsDevice)
    {
        Image = new RenderTarget2D(GraphicsDevice,
            TextureFormat.Rgba8UNorm, width, height);
        Width = Math.Max(width, 1);
        Height = Math.Max(height, 1);
    }

    /// <inheritdoc/>
    public virtual SKCanvas Acquire()
    {
        currentSurface ??= CreateSurface();

        return currentSurface.Canvas;
    }

    /// <inheritdoc/>
    public virtual void Flush()
    {
        if (currentSurface == null)
            return;

        var commandQueue = GraphicsDevice.GraphicsQueue;
        var commandList = commandQueue.Allocate();

        using var image = currentSurface.Snapshot();

        Image.WriteUnsafe(commandList, image.PeekPixels().GetPixelSpan());

        commandQueue.Execute(commandList);
    }

    /// <inheritdoc/>
    public virtual void Resize(int width, int height)
    {
        if (Width == width && Height == height)
            return;

        Width = width = Math.Max(width, 1);
        Height = height = Math.Max(height, 1);

        Utilities.Dispose(ref currentSurface);

        Image.Resize(width, height);
    }

    /// <summary>
    /// Creates the Skia surface.
    /// </summary>
    /// <returns>The surface.</returns>
    protected virtual SKSurface CreateSurface()
    {
        var info = new SKImageInfo(Width, Height,
            SKColorType.Rgba8888, SKAlphaType.Premul);

        return SKSurface.Create(info, info.RowBytes);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            Image.Dispose();

            Utilities.Dispose(ref currentSurface);
        }
    }
}
