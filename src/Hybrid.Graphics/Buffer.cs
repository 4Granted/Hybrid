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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hybrid.Graphics;

/// <summary>
/// Represents an API independent buffer resource.
/// </summary>
public class Buffer : GpuResource, IBuffer
{
    /// <summary>
    /// Gets the native buffer.
    /// </summary>
    public IBufferImpl Impl => impl ?? throw new GraphicsException();

    /// <summary>
    /// Gets the usage of the buffer.
    /// </summary>
    public BufferUsage Usage { get; }

    /// <summary>
    /// Gets the size of the buffer in bytes.
    /// </summary>
    public int SizeInBytes { get; private set; }

    /// <summary>
    /// Gets the stride of the buffer in bytes.
    /// </summary>
    public int StrideInBytes { get; private set; }

    /// <summary>
    /// Gets the description of the buffer.
    /// </summary>
    protected BufferDescription Description => description;

    private IBufferImpl? impl;
    private BufferDescription description;

    /// <summary>
    /// Constructs a buffer.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="usage">The usage of the buffer.</param>
    /// <param name="sizeInBytes">The size of the buffer in bytes.</param>
    /// <param name="strideInBytes">The stride of the buffer in bytes.</param>
    /// <param name="access">The access of the buffer.</param>
    public Buffer(GraphicsDevice graphicsDevice,
        BufferUsage usage, int sizeInBytes, int strideInBytes,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, access)
    {
        Debug.Assert(sizeInBytes > 0, "The size of a buffer must be greater than zero");
        Debug.Assert(strideInBytes > 0, "The stride of a buffer must be greater than zero");

        if ((usage & BufferUsage.ConstantsBuffer) != 0)
        {
            var alignedSize = (sizeInBytes >> 4) << 4;

            if (alignedSize < sizeInBytes)
                alignedSize += 16;

            sizeInBytes = alignedSize;
        }

        SizeInBytes = sizeInBytes;
        StrideInBytes = strideInBytes;

        if ((access & ResourceAccess.Dynamic) != 0)
            usage |= BufferUsage.Dynamic;

        description = new BufferDescription
        {
            Usage = Usage = usage,
            SizeInBytes = (uint)sizeInBytes,
            StrideInBytes = (uint)strideInBytes,
        };

        impl = CreateBuffer(ref description);
    }

    /// <summary>
    /// Initializes the buffer with the provided <paramref name="description"/>.
    /// </summary>
    /// <param name="description">The description to use.</param>
    public void InitializeUnsafe(ref BufferDescription description)
    {
        // Ensure the previous buffer is disposed of
        Utilities.Dispose(ref impl);

        this.description = description;

        SizeInBytes = (int)description.SizeInBytes;

        impl = CreateBuffer(ref description);
    }

    /// <summary>
    /// Writes the <paramref name="data"/> to the buffer.
    /// </summary>
    /// <typeparam name="TData">The type of data to write.</typeparam>
    /// <param name="commandList">The command list to execute on.</param>
    /// <param name="data">The data to write to the buffer.</param>
    /// <param name="offsetInBytes">The offset of the write in bytes.</param>
    /// <remarks>
    /// Extreme care should be taken when writing to
    /// a buffer in an unsafe manner, as the underlying
    /// type of data present isn't taken into account.
    /// </remarks>
    public unsafe void WriteUnsafe<TData>(
        CommandList commandList,
        ReadOnlySpan<TData> data,
        int offsetInBytes = 0)
        where TData : unmanaged
    {
        var sizeInBytes = StrideInBytes * data.Length;

        ArgumentOutOfRangeException.ThrowIfGreaterThan(sizeInBytes, SizeInBytes, nameof(data));

        var cli = commandList.Impl;

        fixed (void* pointer = data)
        {
            if (Access == ResourceAccess.Default)
            {
                var box = new DataBox((nint)pointer, 0, 0);

                if ((Usage & BufferUsage.ConstantsBuffer) != 0)
                {
                    cli.WriteResource(Impl, 0, box);
                }
                else
                {
                    var region = new ResourceRegion(offsetInBytes, 0, 0, offsetInBytes + sizeInBytes, 1, 1);

                    cli.WriteResource(Impl, 0, region, box);
                }
            }
            else
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(offsetInBytes, 0, nameof(offsetInBytes));

                var mode = Usage == BufferUsage.CopyBuffer ? MapMode.Write : MapMode.WriteDiscard;

                var mappedResource = cli.MapResource(Impl, mode, 0, false);

                Unsafe.CopyBlockUnaligned((void*)mappedResource.Data.DataPointer, pointer, (uint)sizeInBytes);

                cli.UnmapResource(mappedResource);
            }
        }
    }

    /// <summary>
    /// Writes the <paramref name="data"/> to the buffer.
    /// </summary>
    /// <typeparam name="TData">The type of data to write.</typeparam>
    /// <param name="commandList">The command list to execute on.</param>
    /// <param name="data">The data to write to the buffer.</param>
    /// <param name="offsetInBytes">The offset of the write in bytes.</param>
    /// <remarks>
    /// Extreme care should be taken when writing to
    /// a buffer in an unsafe manner, as the underlying
    /// type of data present isn't taken into account.
    /// </remarks>
    public void WriteUnsafe<TData>(
        CommandList commandList,
        TData[] data,
        int offsetInBytes = 0)
        where TData : unmanaged
    {
        var span = (ReadOnlySpan<TData>)data.AsSpan();

        WriteUnsafe(commandList, span, offsetInBytes);
    }

    /// <summary>
    /// Writes the <paramref name="data"/> to the buffer.
    /// </summary>
    /// <typeparam name="TData">The type of data to write.</typeparam>
    /// <param name="commandList">The command list to execute on.</param>
    /// <param name="data">The data to write to the buffer.</param>
    /// <param name="offsetInBytes">The offset of the write in bytes.</param>
    /// <remarks>
    /// Extreme care should be taken when writing to
    /// a buffer in an unsafe manner, as the underlying
    /// type of data present isn't taken into account.
    /// </remarks>
    public void WriteUnsafe<TData>(
        CommandList commandList,
        ref TData data,
        int offsetInBytes = 0)
        where TData : unmanaged
    {
        var span = MemoryMarshal.CreateReadOnlySpan(ref data, 1);

        WriteUnsafe(commandList, span, offsetInBytes);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            Utilities.Dispose(ref impl);
        }
    }

    private IBufferImpl CreateBuffer(ref BufferDescription description)
        => Factory.CreateBuffer(ref description);
}
