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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents a description of an vertex element.
/// </summary>
public struct VertexElement : IDistinct<VertexElement>
{
    /// <summary>
    /// Represents the common color vertex element.
    /// </summary>
    public static readonly VertexElement COLOR = new(
        VertexElementFormat.Float4, VertexSemantics.COLOR);

    /// <summary>
    /// Represents the common position vertex element.
    /// </summary>
    public static readonly VertexElement POSITION3 = new(
        VertexElementFormat.Float3, VertexSemantics.POSITION);

    /// <summary>
    /// Represents the common position vertex element.
    /// </summary>
    public static readonly VertexElement POSITION2 = new(
        VertexElementFormat.Float2, VertexSemantics.POSITION);

    /// <summary>
    /// Represents the common normal vertex element.
    /// </summary>
    public static readonly VertexElement NORMAL = new(
        VertexElementFormat.Float3, VertexSemantics.NORMAL);

    /// <summary>
    /// Represents the common tangent vertex element.
    /// </summary>
    public static readonly VertexElement TANGENT = new(
        VertexElementFormat.Float3, VertexSemantics.TANGENT);

    /// <summary>
    /// Represents the common bitangent vertex element.
    /// </summary>
    public static readonly VertexElement BITANGENT = new(
        VertexElementFormat.Float3, VertexSemantics.BITANGENT);

    /// <summary>
    /// Represents the common texture coordinate vertex element.
    /// </summary>
    public static readonly VertexElement TEXCOORD = new(
        VertexElementFormat.Float2, VertexSemantics.TEXCOORD);

    /// <summary>
    /// The format of the vertex element.
    /// </summary>
    public VertexElementFormat Format;

    /// <summary>
    /// The name of the vertex element.
    /// </summary>
    public string Name;

    /// <summary>
    /// The semantic of the vertex element.
    /// </summary>
    public string Semantic;

    /// <summary>
    /// The offset of the vertex element.
    /// </summary>
    public uint Offset;

    public VertexElement(
        VertexElementFormat format, string name,
        string semantic, uint offset)
    {
        Format = format;
        Name = name;
        Semantic = semantic;
        Offset = offset;
    }

    public VertexElement(VertexElementFormat format, string name, string semantic)
    {
        Format = format;
        Name = name;
        Semantic = semantic;
        Offset = 0;
    }

    public VertexElement(VertexElementFormat format, string semantic)
    {
        Format = format;
        Name = semantic;
        Semantic = semantic;
        Offset = 0;
    }

    /// <inheritdoc/>
    public readonly bool Equals(VertexElement other)
        => other.Format == Format && other.Name == Name
        && other.Semantic == Semantic && other.Offset == Offset;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is VertexElement other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        Format, Name, Semantic, Offset);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(VertexElement left, VertexElement right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(VertexElement left, VertexElement right) => !left.Equals(right);

    public static uint GetSizeInBytes(VertexElementFormat format) => format switch
    {
        VertexElementFormat.Byte4Norm or
        VertexElementFormat.UInt1 or
        VertexElementFormat.Int1 or
        VertexElementFormat.Float1 => 4,

        VertexElementFormat.UInt2 or
        VertexElementFormat.Int2 or
        VertexElementFormat.Float2 => 8,

        VertexElementFormat.UInt3 or
        VertexElementFormat.Int3 or
        VertexElementFormat.Float3 => 12,

        VertexElementFormat.UInt4 or
        VertexElementFormat.Int4 or
        VertexElementFormat.Float4 => 16,

        _ => 0,
    };
}
