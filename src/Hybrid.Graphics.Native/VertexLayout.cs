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
/// Represents a description of a vertex layout.
/// </summary>
public readonly struct VertexLayout : IDistinct<VertexLayout>
{
    /// <summary>
    /// The elements of the layout.
    /// </summary>
    public readonly VertexElement[] Elements;

    /// <summary>
    /// The stride of the layout in bytes.
    /// </summary>
    public readonly uint StrideInBytes;

    /// <summary>
    /// The instance offset of the layout.
    /// </summary>
    public readonly uint InstanceOffset;

    public VertexLayout(params VertexElement[] elements)
    {
        Elements = elements;

        uint stride = 0;

        foreach (var element in elements)
        {
            var elementSize = VertexElement.GetSizeInBytes(element.Format);
            var elementOffset = element.Offset;

            if (elementOffset != 0)
            {
                stride += elementSize + elementOffset;
            }
            else
            {
                stride += elementSize;
            }
        }

        StrideInBytes = stride;
        InstanceOffset = 0;
    }

    /// <inheritdoc/>
    public readonly bool Equals(VertexLayout other)
        => other.StrideInBytes == StrideInBytes
        && other.InstanceOffset == InstanceOffset;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is VertexLayout other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        Elements, StrideInBytes, InstanceOffset);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(VertexLayout left, VertexLayout right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(VertexLayout left, VertexLayout right) => !left.Equals(right);
}
