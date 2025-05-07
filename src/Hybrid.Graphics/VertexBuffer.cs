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

namespace Hybrid.Graphics;

/// <summary>
/// Represents a vertex buffer.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="capacity">The capacity of the buffer in elements.</param>
/// <param name="strideInBytes">The stride of the buffer in bytes.</param>
/// <param name="access">The access of the buffer.</param>
public class VertexBuffer(
    GraphicsDevice graphicsDevice,
    int capacity, int strideInBytes,
    ResourceAccess access = ResourceAccess.Default)
    : Buffer(graphicsDevice, BufferUsage.VertexBuffer,
        strideInBytes * capacity, strideInBytes, access)
{
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
}

/// <summary>
/// Represents a typed vertex buffer.
/// </summary>
/// <typeparam name="TVertex">The type of vertex data in the buffer.</typeparam>
public class VertexBuffer<TVertex> : VertexBuffer
    where TVertex : unmanaged, IVertex
{
    /// <summary>
    /// Constructs a vertex buffer.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="capacity">The capacity of the buffer in elements.</param>
    /// <param name="access">The access of the buffer.</param>
    public VertexBuffer(GraphicsDevice graphicsDevice, int capacity,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, capacity, Unsafe.SizeOf<TVertex>(), access) { }

    /// <summary>
    /// Constructs a vertex buffer with initial data.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="initialData">The initial data of the buffer.</param>
    /// <param name="access">The access of the buffer.</param>
    public VertexBuffer(GraphicsDevice graphicsDevice, Span<TVertex> initialData,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, initialData.Length, Unsafe.SizeOf<TVertex>(), access)
    {
        var commandQueue = graphicsDevice.GraphicsQueue;
        var commandList = commandQueue.Allocate();

        Write(commandList, initialData);

        commandQueue.Execute(commandList);
    }

    public void Write(CommandList commandList, ReadOnlySpan<TVertex> vertices, int offset = 0)
    {
        Debug.Assert(vertices.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, vertices, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, TVertex[] vertices, int offset = 0)
    {
        Debug.Assert(vertices.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, vertices, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, ref TVertex data, int offset)
    {
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, ref data, StrideInBytes * offset);
    }

    /// <summary>
    /// Implicitly gets the vertex buffer view from the <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The vertex buffer view.</param>
    public static implicit operator VertexBufferView(VertexBuffer<TVertex> buffer) => new(buffer, TVertex.Layout);
}
