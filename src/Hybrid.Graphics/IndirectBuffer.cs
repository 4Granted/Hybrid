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
using System.Runtime.InteropServices;

namespace Hybrid.Graphics;

/// <summary>
/// Represents an indirect buffer.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="strideInBytes">The stride of the buffer in bytes.</param>
/// <param name="access">The access of the buffer.</param>
public class IndirectBuffer(GraphicsDevice graphicsDevice, int strideInBytes,
        ResourceAccess access = ResourceAccess.Default)
    : Buffer(graphicsDevice, BufferUsage.IndirectBuffer, strideInBytes, strideInBytes, access)
{
    /// <summary>
    /// Represents an indirect draw argument buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Draw
    {
        /// <summary>
        /// The amount of vertices to draw.
        /// </summary>
        public uint VertexCount;

        /// <summary>
        /// The amount of instances to draw.
        /// </summary>
        public uint InstanceCount;

        /// <summary>
        /// The offset to the start vertex.
        /// </summary>
        public uint VertexStart;

        /// <summary>
        /// The offset ot the start instance.
        /// </summary>
        public uint InstanceStart;
    }

    /// <summary>
    /// Represents an indirect draw indexed argument buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DrawIndexed
    {
        /// <summary>
        /// The amount of indices to draw.
        /// </summary>
        public uint IndexCount;
        
        /// <summary>
        /// The amount of instances to draw.
        /// </summary>
        public uint InstanceCount;

        /// <summary>
        /// The offset to the start index.
        /// </summary>
        public uint IndexStart;

        /// <summary>
        /// The offset to the start vertex.
        /// </summary>
        public int BaseVertex;

        /// <summary>
        /// The offset ot the start instance.
        /// </summary>
        public uint InstanceStart;
    }

    /// <summary>
    /// Represents an indirect dispatch argument buffer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Dispatch
    {
        /// <summary>
        /// The thread group width.
        /// </summary>
        public uint ThreadGroupWidth;

        /// <summary>
        /// The thread group height.
        /// </summary>
        public uint ThreadGroupHeight;

        /// <summary>
        /// The thread group depth.
        /// </summary>
        public uint ThreadGroupDepth;
    }
}

/// <summary>
/// Represents a typed indirect buffer.
/// </summary>
/// <typeparam name="TArguments">The type of argument data in the buffer.</typeparam>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="access">The access of the buffer.</param>
public class IndirectBuffer<TArguments>(GraphicsDevice graphicsDevice,
        ResourceAccess access = ResourceAccess.Default)
    : IndirectBuffer(graphicsDevice, Marshal.SizeOf<TArguments>(), access)
    where TArguments : unmanaged;
