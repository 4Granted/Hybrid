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
/// Defines the format of a <see cref="VertexElement"/>.
/// </summary>
public enum VertexElementFormat
{
    /// <summary>
    /// None.
    /// </summary>
    None,

    /// <summary>
    /// A vertex element with four 8-bit normalized integers.
    /// </summary>
    Byte4Norm,

    /// <summary>
    /// A vertex element with a single 32-bit unsigned integer.
    /// </summary>
    UInt1,

    /// <summary>
    /// A vertex element with two 32-bit unsigned integers.
    /// </summary>
    UInt2,

    /// <summary>
    /// A vertex element with three 32-bit unsigned integers.
    /// </summary>
    UInt3,

    /// <summary>
    /// A vertex element with four 32-bit unsigned integers.
    /// </summary>
    UInt4,

    /// <summary>
    /// A vertex element with a single 32-bit integer.
    /// </summary>
    Int1,

    /// <summary>
    /// A vertex element with two 32-bit integers.
    /// </summary>
    Int2,

    /// <summary>
    /// A vertex element with three 32-bit integers.
    /// </summary>
    Int3,

    /// <summary>
    /// A vertex element with four 32-bit integers.
    /// </summary>
    Int4,

    /// <summary>
    /// A vertex element with a single 32-bit floating point number.
    /// </summary>
    Float1,

    /// <summary>
    /// A vertex element with two 32-bit floating point numbers.
    /// </summary>
    Float2,

    /// <summary>
    /// A vertex element with three 32-bit floating point numbers.
    /// </summary>
    Float3,

    /// <summary>
    /// A vertex element with four 32-bit floating point numbers.
    /// </summary>
    Float4,
}
