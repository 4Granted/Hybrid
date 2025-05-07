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

namespace Hybrid.Graphics.Textures;

/// <summary>
/// Represents a two-dimensional texture.
/// </summary>
public class Texture2D : Texture, ITexture2D
{
    /// <summary>
    /// Constructs a two-dimensional texture.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the texture.</param>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="arraySize">The amount of array layers in the texture.</param>
    /// <param name="mipLevels">The amount of the mip levels in the texture.</param>
    /// <param name="usage">The usage of the texture.</param>
    /// <param name="samples">The amount of samples used by the texture.</param>
    /// <param name="access">The resource access of the texture.</param>
    public Texture2D(GraphicsDevice graphicsDevice,
        TextureFormat format, int width, int height,
        int arraySize = 1, int mipLevels = 1,
        TextureUsage usage = TextureUsage.SampleBuffer,
        TextureSamples samples = TextureSamples.X1,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, TextureDimension.Texture2D, usage,
              format, samples, width, height, 1,
              arraySize, mipLevels, access) { }

    /// <summary>
    /// Resizes the texture.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    public void Resize(int width, int height)
    {
        var description = Description with
        {
            Width = (uint)width,
            Height = (uint)height,
        };

        InitializeUnsafe(ref description);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="graphicsDevice"></param>
    /// <param name="format"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Texture2D Create(
        GraphicsDevice graphicsDevice,
        TextureFormat format, byte[] data,
        int width, int height)
    {
        var texture = new Texture2D(graphicsDevice, format, width, height);

        if (data.Length > 0)
        {
            var bitmap = new Bitmap<byte>(data, width, height);

            using var context = graphicsDevice.CreateContext(
                out var commandList);

            texture.WriteUnsafe(commandList, bitmap);
        }

        return texture;
    }
}

/// <summary>
/// Represents a typed two-dimensional texture.
/// </summary>
/// <typeparam name="TPixel">The type of pixel data in the texture.</typeparam>
public class Texture2D<TPixel> : Texture2D
    where TPixel : unmanaged
{
    /// <summary>
    /// The pixel format of the texture.
    /// </summary>
    public static readonly TextureFormat PixelFormat = TextureFormat.Rgba8UNorm; // TODO

    /// <summary>
    /// Constructs a two-dimensional texture.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the texture.</param>
    /// <param name="bitmap">The pixels of the texture.</param>
    /// <param name="format">The format of the texture.</param>
    public Texture2D(GraphicsDevice graphicsDevice,
        TextureFormat format, Bitmap<TPixel> bitmap)
        : base(graphicsDevice, format, bitmap.Width, bitmap.Height)
    {
        var commandQueue = graphicsDevice.GraphicsQueue;
        var commandList = commandQueue.Allocate();

        WriteUnsafe(commandList, bitmap);

        commandQueue.Execute(commandList);
    }

    /// <summary>
    /// Constructs a two-dimensional texture.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the texture.</param>
    /// <param name="bitmap">The pixels of the texture.</param>
    public Texture2D(GraphicsDevice graphicsDevice, Bitmap<TPixel> bitmap)
        : this(graphicsDevice, PixelFormat, bitmap) { }
}
