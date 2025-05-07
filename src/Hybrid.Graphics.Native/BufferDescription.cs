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

/// <summary>
/// Represents a description of an <see cref="IBufferImpl"/>.
/// </summary>
public struct BufferDescription : IDistinct<BufferDescription>
{
    /// <summary>
    /// The usage of the buffer.
    /// </summary>
    public BufferUsage Usage;

    /// <summary>
    /// The size of the buffer in bytes.
    /// </summary>
    public uint SizeInBytes;

    /// <summary>
    /// The stride of the buffer in bytes.
    /// </summary>
    public uint StrideInBytes;

    /// <inheritdoc/>
    public readonly bool Equals(BufferDescription other)
        => other.Usage == Usage
        && other.SizeInBytes == SizeInBytes
        && other.StrideInBytes == StrideInBytes;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is BufferDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Usage, SizeInBytes, StrideInBytes);

    /// <inheritdoc/>
    public static bool operator ==(BufferDescription left, BufferDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(BufferDescription left, BufferDescription right) => !left.Equals(right);
}
