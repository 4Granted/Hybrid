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
/// Represents a description of a descriptor group.
/// </summary>
public struct DescriptorLayoutDescription : IDistinct<DescriptorLayoutDescription>
{
    /// <summary>
    /// A descriptor group with no elements.
    /// </summary>
    public static readonly DescriptorLayoutDescription None = default;

    /// <summary>
    /// The elements of the descriptor group.
    /// </summary>
    public DescriptorLayoutElement[] Elements;

    /// <summary>
    /// Constructs a descriptor group description.
    /// </summary>
    /// <param name="elements">The elements of the descriptor group.</param>
    public DescriptorLayoutDescription(
        params DescriptorLayoutElement[] elements)
        => Elements = elements;

    /// <inheritdoc/>
    public readonly bool Equals(DescriptorLayoutDescription other)
        => Utilities.CompareDistinct(in other.Elements, in Elements);

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DescriptorLayoutDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        base.GetHashCode(), Elements);

    /// <inheritdoc/>
    public static bool operator ==(DescriptorLayoutDescription left, DescriptorLayoutDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(DescriptorLayoutDescription left, DescriptorLayoutDescription right) => !left.Equals(right);
}
