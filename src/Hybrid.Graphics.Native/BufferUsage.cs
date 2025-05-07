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

namespace Hybrid.Graphics.Native;

/// <summary>
/// Defines the usage of a <see cref="IBufferImpl"/>.
/// </summary>
[Flags]
public enum BufferUsage
{
    /// <summary>
    /// A constants buffer.
    /// </summary>
    ConstantsBuffer = 1 << 0,

    /// <summary>
    /// A structured read-only buffer.
    /// </summary>
    StructuredBuffer = 1 << 1,

    /// <summary>
    /// A structured read-write buffer.
    /// </summary>
    ComputeBuffer = 1 << 2,

    /// <summary>
    /// A vertex buffer.
    /// </summary>
    VertexBuffer = 1 << 3,

    /// <summary>
    /// An index buffer.
    /// </summary>
    IndexBuffer = 1 << 4,

    /// <summary>
    /// An indirect argument buffer.
    /// </summary>
    IndirectBuffer = 1 << 5,

    /// <summary>
    /// A copy buffer.
    /// </summary>
    CopyBuffer = 1 << 6,

    /// <summary>
    /// A dynamic buffer.
    /// </summary>
    Dynamic = 1 << 7,
}
