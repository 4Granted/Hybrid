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
/// Represents a texture resource.
/// </summary>
public class Texture : GpuResource, ITexture
{
    /// <summary>
    /// Gets the native texture.
    /// </summary>
    public ITextureImpl Impl => impl ?? throw new GraphicsException();

    /// <summary>
    /// Gets the dimension of the texture.
    /// </summary>
    public TextureDimension Dimension => Description.Dimension;

    /// <summary>
    /// Gets the usage of the texture.
    /// </summary>
    public TextureUsage Usage => Description.Usage;

    /// <summary>
    /// Gets the format of the texture.
    /// </summary>
    public TextureFormat Format => Description.Format;

    /// <summary>
    /// Gets the samples of the texture.
    /// </summary>
    public TextureSamples Samples => Description.Samples;

    /// <summary>
    /// Gets the width of the texture.
    /// </summary>
    public int Width => (int)Description.Width;

    /// <summary>
    /// Gets the height of the texture.
    /// </summary>
    public int Height => (int)Description.Height;

    /// <summary>
    /// Gets the depth of the texture.
    /// </summary>
    public int Depth => (int)Description.Depth;

    /// <summary>
    /// Gets the array size of the texture.
    /// </summary>
    public int ArraySize => (int)Description.ArraySize;

    /// <summary>
    /// Gets the amount of mip levels of the texture.
    /// </summary>
    public int MipLevels => (int)Description.MipLevels;

    /// <summary>
    /// Gets the description of the texture.
    /// </summary>
    protected TextureDescription Description => description;

    private ITextureImpl? impl;
    private TextureDescription description;

    /// <summary>
    /// Constructs a texture.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the texture.</param>
    /// <param name="dimension">The dimension of the texture.</param>
    /// <param name="usage">The usage of the texture</param>
    /// <param name="format">The format of the texture.</param>
    /// <param name="samples">The sample count of the texture.</param>
    /// <param name="size">The size of the texture.</param>
    /// <param name="arraySize">The array size of the texture.</param>
    /// <param name="mipLevels">The amount of mip levels in the texture.</param>
    /// <param name="access">The access of the resource.</param>
    public Texture(GraphicsDevice graphicsDevice,
        TextureDimension dimension, TextureUsage usage,
        TextureFormat format, TextureSamples samples,
        int width, int height, int depth, int arraySize, int mipLevels,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, access)
    {
        description = new TextureDescription
        {
            Dimension = dimension,
            Usage = usage,
            Format = format,
            Samples = samples,
            Width = (uint)width,
            Height = (uint)height,
            Depth = (uint)depth,
            ArraySize = (uint)arraySize,
            MipLevels = (uint)mipLevels,
        };

        impl = CreateTexture(ref description);
    }

    /// <summary>
    /// Initializes the texture with the provided <paramref name="description"/>.
    /// </summary>
    /// <param name="description">The description to use.</param>
    public void InitializeUnsafe(ref TextureDescription description)
    {
        // Ensure the previous texture is disposed of
        CommonExtensions.Dispose(ref impl);

        this.description = description;

        impl = CreateTexture(ref description);
    }

    /// <summary>
    /// Resizes the texture.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="depth">The depth of the texture.</param>
    public void ResizeUnsafe(int width, int height, int depth)
    {
        var description = Description with
        {
            Width = (uint)width,
            Height = (uint)height,
            Depth = (uint)depth
        };

        InitializeUnsafe(ref description);
    }

    /// <summary>
    /// Writes pixel data to the texture.
    /// </summary>
    /// <param name="commandList">The command list.</param>
    /// <param name="data">The pixel data.</param>
    /// <param name="arraySlice">The array slice.</param>
    /// <param name="mipSlice">The mip slice.</param>
    /// <param name="region">The region.</param>
    public unsafe void WriteUnsafe(CommandList commandList, DataBox data, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
    {
        var subresource = (uint)GetSubresource(arraySlice, mipSlice);

        if (Access == ResourceAccess.Default)
        {
            if (region.HasValue)
            {
                var textureRegion = region.Value;

                commandList.Impl.WriteResource(Impl, subresource, textureRegion, data);
            }
            else
            {
                commandList.Impl.WriteResource(Impl, subresource, data);
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Writes pixel data to the texture.
    /// </summary>
    /// <typeparam name="TPixel">The pixel type.</typeparam>
    /// <param name="commandList">The command list.</param>
    /// <param name="data">The pixel data.</param>
    /// <param name="arraySlice">The array slice.</param>
    /// <param name="mipSlice">The mip slice.</param>
    /// <param name="region">The region.</param>
    public unsafe void WriteUnsafe<TPixel>(CommandList commandList, Span<TPixel> data, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
        where TPixel : unmanaged
    {
        TextureUtilities.GetPitch(Format, Width, Height, out var rowPitch, out var depthPitch);

        fixed (void* source = data)
        {
            var box = new DataBox((nint)source, (uint)rowPitch, (uint)depthPitch);

            WriteUnsafe(commandList, box, arraySlice, mipSlice, region);
        }
    }

    /// <summary>
    /// Writes pixel data to the texture using a <paramref name="bitmap"/>.
    /// </summary>
    /// <typeparam name="TPixel">The pixel type.</typeparam>
    /// <param name="commandList">The command list.</param>
    /// <param name="bitmap">The bitmap.</param>
    /// <param name="arraySlice">The array slice.</param>
    /// <param name="mipSlice">The mip slice.</param>
    /// <param name="region">The region.</param>
    public unsafe void WriteUnsafe<TPixel>(CommandList commandList, Bitmap<TPixel> bitmap, int arraySlice = 0, int mipSlice = 0, ResourceRegion? region = null)
        where TPixel : unmanaged => WriteUnsafe(commandList, bitmap.Data.Span, arraySlice, mipSlice, region);

    /// <summary>
    /// Gets the subresource index of the texture.
    /// </summary>
    /// <param name="arraySlice">The array slice.</param>
    /// <param name="mipSlice">The mip slice.</param>
    /// <returns>The subresource index.</returns>
    public int GetSubresource(int arraySlice, int mipSlice)
        => arraySlice * MipLevels + mipSlice;

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            CommonExtensions.Dispose(ref impl);
        }
    }

    private ITextureImpl CreateTexture(ref TextureDescription description)
        => Factory.CreateTexture(ref description);
}
