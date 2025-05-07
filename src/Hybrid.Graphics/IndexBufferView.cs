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

namespace Hybrid.Graphics;

/// <summary>
/// Represents a view into a <see cref="IndexBuffer"/>.
/// </summary>
/// <param name="buffer">The buffer of the view.</param>
/// <param name="offset">The offset of the view.</param>
public readonly struct IndexBufferView(
    IndexBuffer buffer, int offset = 0)
    : IDistinct<IndexBufferView>, IDisposable
{
    /// <summary>
    /// The buffer of the view.
    /// </summary>
    public readonly IndexBuffer Buffer = buffer;

    /// <summary>
    /// The offset of the view.
    /// </summary>
    public readonly int Offset = offset;

    /// <inheritdoc/>
    public readonly void Dispose() => Buffer?.Dispose();

    /// <inheritdoc/>
    public readonly bool Equals(IndexBufferView other)
        => other.Buffer?.Id == Buffer?.Id
        && other.Offset == Offset;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is IndexBufferView other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Buffer, Offset);

    /// <inheritdoc/>
    public static bool operator ==(IndexBufferView left, IndexBufferView right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(IndexBufferView left, IndexBufferView right) => !left.Equals(right);
}
