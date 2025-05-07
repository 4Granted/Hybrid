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

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MemoryBuffer = System.Buffer;

namespace Hybrid.Graphics;

/// <summary>
/// Represents raw pixel data in CPU memory.
/// </summary>
/// <typeparam name="TPixel">The type of the pixel.</typeparam>
public sealed class Bitmap<TPixel>
    where TPixel : unmanaged
{
    /// <summary>
    /// Gets or sets the pixel at the position.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The pixel at the position.</returns>
    public TPixel this[int x, int y]
    {
        get => Data.Span[GetIndex(x, y)];
        set => Data.Span[GetIndex(x, y)] = value;
    }

    /// <summary>
    /// Gets or sets the pixel at the <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index of the pixel.</param>
    /// <returns>The pixel at the <paramref name="index"/>.</returns>
    public TPixel this[int index]
    {
        get => Data.Span[index];
        set => Data.Span[index] = value;
    }

    /// <summary>
    /// Gets the data of the bitmap.
    /// </summary>
    public Memory<TPixel> Data { get; }

    /// <summary>
    /// Gets the width of the bitmap.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the bitmap.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the size of a pixel in bytes.
    /// </summary>
    public int PixelSizeInBytes { get; }

    /// <summary>
    /// Gets the size of the bitmap in bytes.
    /// </summary>
    public int SizeInBytes => Width * Height * PixelSizeInBytes;

    private GCHandle handle;

    /// <summary>
    /// Constructs a bitmap.
    /// </summary>
    /// <param name="data">The data of the bitmap.</param>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    /// <param name="pixelSizeInBytes">The size of a pixel in bytes.</param>
    public Bitmap(TPixel[] data, int width, int height, int pixelSizeInBytes = -1)
    {
        var pixelSize = pixelSizeInBytes >= 0
            ? pixelSizeInBytes : Unsafe.SizeOf<TPixel>();
        var size = width * height * pixelSize;

        ArgumentOutOfRangeException.ThrowIfNotEqual(data.Length, size, nameof(data));

        Data = data;
        Width = width;
        Height = height;
        PixelSizeInBytes = pixelSize;
    }

    /// <summary>
    /// Constructs a bitmap.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    /// <param name="pixelSizeInBytes">The size of a pixel in bytes.</param>
    public Bitmap(int width, int height, int pixelSizeInBytes = -1)
    {
        var pixelSize = pixelSizeInBytes >= 0
            ? pixelSizeInBytes : Unsafe.SizeOf<TPixel>();
        var size = width * height * pixelSize;

        Data = new TPixel[size];
        Width = width;
        Height = height;
        PixelSizeInBytes = pixelSize;
    }

    /// <summary>
    /// Constructs a bitmap.
    /// </summary>
    /// <param name="width">The width of the bitmap.</param>
    /// <param name="height">The height of the bitmap.</param>
    public Bitmap(int width, int height)
    {
        Data = new TPixel[width * height];
        Width = width;
        Height = height;
        PixelSizeInBytes = Unsafe.SizeOf<TPixel>();
    }

    /// <summary>
    /// Copies the <paramref name="other"/> bitmap data to this bitmap.
    /// </summary>
    /// <param name="other">The bitmap to copy.</param>
    public unsafe void CopyFrom(Bitmap<TPixel> other)
    {
        var source = (byte*)other.LockBits();
        var destination = (byte*)LockBits();

        MemoryBuffer.MemoryCopy(source, destination, other.Data.Span.Length, Data.Span.Length);

        UnlockBits();
    }

    /// <summary>
    /// Copies the <paramref name="data"/> to the bitmap.
    /// </summary>
    /// <param name="data">The data to move.</param>
    public unsafe void CopyFrom(Span<TPixel> data)
    {
        Debug.Assert(data.Length == SizeInBytes);

        var destination = (byte*)LockBits();

        fixed (TPixel* source = data)
        {
            MemoryBuffer.MemoryCopy(source, destination, Data.Length, data.Length);
        }

        UnlockBits();
    }

    /// <summary>
    /// Copies the bitmap to the <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">The destination of the data.</param>
    public unsafe void CopyTo(TPixel[] destination)
    {
        Debug.Assert(destination.Length == Width * Height);

        var source = (TPixel*)LockBits();
        var length = Data.Length;

        fixed (TPixel* ptr = destination)
        {
            MemoryBuffer.MemoryCopy(source, ptr, length, length);
        }

        UnlockBits();
    }

    /// <summary>
    /// Pins the bitmap to memory for writing.
    /// </summary>
    /// <returns>The pointer to the memory.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the bitmp is already pinned.</exception>
    public nint LockBits()
    {
        if (handle.IsAllocated)
            throw new InvalidOperationException();

        handle = GCHandle.Alloc(Data, GCHandleType.Pinned);

        return handle.AddrOfPinnedObject();
    }

    /// <summary>
    /// Unpins the bitmap from memory.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the bitmp isn't already pinned.</exception>
    public void UnlockBits()
    {
        if (!handle.IsAllocated)
            throw new InvalidOperationException();

        handle.Free();
    }

    /// <summary>
    /// Gets the index of the two-dimensional position.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The index of the coordinate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetIndex(int x, int y) => y * Width + x;

    /// <summary>
    /// Gets the offset to the <paramref name="row"/>.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <returns>The offset in pixels.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetRowOffset(int row) => Width * row;
}
