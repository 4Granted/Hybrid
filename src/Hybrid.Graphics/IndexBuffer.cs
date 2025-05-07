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
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics;

/// <summary>
/// Represents an index buffer.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="format">The format of the buffer elements.</param>
/// <param name="capacity">The capacity of the buffer in elements.</param>
/// <param name="access">The access of the buffer.</param>
public class IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format,
    int capacity, ResourceAccess access = ResourceAccess.Default)
    : Buffer(graphicsDevice, BufferUsage.IndexBuffer,
        GetStrideInBytes(format) * capacity,
        GetStrideInBytes(format), access)
{
    /// <summary>
    /// Gets the format of the buffer elements.
    /// </summary>
    public IndexFormat Format { get; } = format;

    /// <summary>
    /// Gets the capacity of the buffer in elements.
    /// </summary>
    public int Capacity { get; private set; } = capacity;

    /// <summary>
    /// Resizes the capacity of the buffer.
    /// </summary>
    /// <param name="capacity">The new capacity of the buffer.</param>
    public void Resize(int capacity)
    {
        Capacity = capacity;

        var description = Description with
        {
            SizeInBytes = (uint)(StrideInBytes * capacity),
        };

        InitializeUnsafe(ref description);
    }

    /// <summary>
    /// Implicitly gets the index buffer view from the <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The index buffer view.</param>
    public static implicit operator IndexBufferView(IndexBuffer buffer) => new(buffer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    public static int GetStrideInBytes(IndexFormat format) => format switch
    {
        IndexFormat.UInt16 => sizeof(ushort),
        IndexFormat.UInt32 => sizeof(uint),
        _ => 0,
    };
}

/// <summary>
/// Represents a typed index buffer.
/// </summary>
/// <typeparam name="TIndex">The type of index data in the buffer.</typeparam>
public class IndexBuffer<TIndex> : IndexBuffer
    where TIndex : unmanaged, IUnsignedNumber<TIndex>
{
    /// <summary>
    /// The format of the index data.
    /// </summary>
    public static readonly IndexFormat IndexFormat = GetFormat();

    /// <summary>
    /// Constructs an index buffer.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="capacity">The capacity of the buffer in elements.</param>
    /// <param name="access">The access of the buffer.</param>
    public IndexBuffer(GraphicsDevice graphicsDevice, int capacity,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, IndexFormat, capacity, access) { }

    /// <summary>
    /// Constructs a index buffer with initial data.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="initialData">The initial data of the buffer.</param>
    /// <param name="access">The access of the buffer.</param>
    public IndexBuffer(GraphicsDevice graphicsDevice, TIndex[] initialData,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, IndexFormat, initialData.Length, access)
    {
        var commandQueue = graphicsDevice.GraphicsQueue;
        var commandList = commandQueue.Allocate();

        Write(commandList, initialData);

        commandQueue.Execute(commandList);
    }

    public void Write(CommandList commandList, ReadOnlySpan<TIndex> indices, int offset = 0)
    {
        Debug.Assert(indices.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, indices, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, TIndex[] indices, int offset = 0)
    {
        Debug.Assert(indices.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, indices, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, ref TIndex data, int offset)
    {
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, ref data, StrideInBytes * offset);
    }

    private static IndexFormat GetFormat()
    {
        var type = typeof(TIndex);

        if (type.IsAssignableTo(typeof(ushort)))
        {
            return IndexFormat.UInt16;
        }
        else if (type.IsAssignableTo(typeof(uint)))
        {
            return IndexFormat.UInt32;
        }

        throw new InvalidOperationException();
    }
}
