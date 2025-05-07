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
/// Represents a constants buffer.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="strideInBytes">The stride of the buffer in bytes.</param>
/// <param name="access">The access of the buffer.</param>
public class ConstantsBuffer(GraphicsDevice graphicsDevice, int strideInBytes,
        ResourceAccess access = ResourceAccess.Default)
    : Buffer(graphicsDevice, BufferUsage.ConstantsBuffer, strideInBytes, strideInBytes, access);

/// <summary>
/// Represents a typed constants buffer.
/// </summary>
/// <typeparam name="TData">The type of data in the buffer.</typeparam>
/// <param name="graphicsDevice">The graphics device of the buffer.</param>
/// <param name="access">The access of the buffer.</param>
public class ConstantsBuffer<TData>(GraphicsDevice graphicsDevice,
        ResourceAccess access = ResourceAccess.Default)
    : ConstantsBuffer(graphicsDevice, Marshal.SizeOf<TData>(), access)
    where TData : unmanaged
{
    /// <summary>
    /// Writes the <paramref name="data"/> to the buffer.
    /// </summary>
    /// <param name="data">The data to write.</param>
    public void Write(CommandList commandList, ref TData data)
        => WriteUnsafe(commandList, ref data);
}
