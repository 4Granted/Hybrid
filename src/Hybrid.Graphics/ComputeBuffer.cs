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
/// Represents a compute buffer.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="capacity">The capacity of the buffer in elements.</param>
/// <param name="strideInBytes">The stride of the buffer in bytes.</param>
/// <param name="access">The access of the buffer.</param>
public class ComputeBuffer(
    GraphicsDevice graphicsDevice,
    int capacity, int strideInBytes,
    ResourceAccess access = ResourceAccess.Default)
    : Buffer(graphicsDevice, BufferUsage.ComputeBuffer,
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
/// Represents a typed compute buffer.
/// </summary>
/// <typeparam name="TElement">The type of element data in the buffer.</typeparam>
public class ComputeBuffer<TElement> : ComputeBuffer
    where TElement : unmanaged
{
    /// <summary>
    /// Constructs a compute buffer.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the buffer.</param>
    /// <param name="capacity">The capacity of the buffer in elements.</param>
    /// <param name="access">The access of the buffer.</param>
    public ComputeBuffer(GraphicsDevice graphicsDevice, int capacity,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, capacity, Unsafe.SizeOf<TElement>(), access) { }

    public void Write(CommandList commandList, ReadOnlySpan<TElement> elements, int offset = 0)
    {
        Debug.Assert(elements.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, elements, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, TElement[] elements, int offset = 0)
    {
        Debug.Assert(elements.Length > 0);
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, elements, StrideInBytes * offset);
    }

    public void Write(CommandList commandList, ref TElement element, int offset)
    {
        Debug.Assert(offset >= 0);

        WriteUnsafe(commandList, ref element, StrideInBytes * offset);
    }
}
