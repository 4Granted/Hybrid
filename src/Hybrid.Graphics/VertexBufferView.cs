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
using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a view into a <see cref="VertexBuffer"/>.
/// </summary>
/// <param name="buffer">The buffer of the view.</param>
/// <param name="layout">The layout of the view.</param>
/// <param name="index">The index of the view.</param>
/// <param name="offset">The offset of the view.</param>
public readonly struct VertexBufferView(
    VertexBuffer buffer, VertexLayout layout, int index = 0, int offset = 0)
    : IDistinct<VertexBufferView>, IDisposable
{
    /// <summary>
    /// The buffer of the view.
    /// </summary>
    public readonly VertexBuffer Buffer = buffer;

    /// <summary>
    /// The layout of the view.
    /// </summary>
    public readonly VertexLayout Layout = layout;

    /// <summary>
    /// The index of the view.
    /// </summary>
    public readonly int Index = index;

    /// <summary>
    /// The offset of the view.
    /// </summary>
    public readonly int Offset = offset;

    /// <inheritdoc/>
    public readonly void Dispose() => Buffer?.Dispose();

    /// <inheritdoc/>
    public readonly bool Equals(VertexBufferView other)
        => other.Buffer?.Id == other.Buffer?.Id
        && other.Index == Index
        && other.Offset == Offset;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is VertexBufferView other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Buffer, Index, Offset);

    /// <inheritdoc/>
    public static bool operator ==(VertexBufferView left, VertexBufferView right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(VertexBufferView left, VertexBufferView right) => !left.Equals(right);
}
