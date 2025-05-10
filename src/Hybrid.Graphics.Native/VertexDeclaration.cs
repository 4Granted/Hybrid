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

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents a description of a vertex declaration.
/// </summary>
public readonly struct VertexDeclaration : IDistinct<VertexDeclaration>
{
    /// <summary>
    /// An empty vertex declaration.
    /// </summary>
    public static readonly VertexDeclaration None = new();

    /// <summary>
    /// The layouts of the declaration.
    /// </summary>
    public readonly ImmutableArray<VertexLayout> Layouts;

    public VertexDeclaration(params Span<VertexLayout> layouts) => Layouts = [.. layouts];

    public VertexDeclaration(params VertexElement[] elements)
        => Layouts = [new VertexLayout(elements)];

    /// <inheritdoc/>
    public readonly bool Equals(VertexDeclaration other)
        => other.Layouts == Layouts;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is VertexDeclaration other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        base.GetHashCode(), Layouts);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(VertexDeclaration left, VertexDeclaration right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(VertexDeclaration left, VertexDeclaration right) => !left.Equals(right);
}
