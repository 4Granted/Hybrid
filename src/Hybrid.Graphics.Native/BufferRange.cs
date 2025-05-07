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

namespace Hybrid.Graphics.Native;

public readonly struct BufferRange(uint offset, uint length) : IDistinct<BufferRange>
{
    /// <summary>
    /// Gets the offset of the buffer range.
    /// </summary>
    public readonly uint Offset = offset;

    /// <summary>
    /// Gets the length of the buffer range.
    /// </summary>
    public readonly uint Length = length;

    /// <inheritdoc/>
    public readonly bool Equals(BufferRange other)
        => other.Offset == Offset
        && other.Length == Length;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is BufferRange other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Offset, Length);

    /// <inheritdoc/>
    public static bool operator ==(BufferRange left, BufferRange right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(BufferRange left, BufferRange right) => !left.Equals(right);

    public static implicit operator BufferRange(Range range)
        => new((uint)range.Start.Value, (uint)(range.End.Value - range.Start.Value));
}
