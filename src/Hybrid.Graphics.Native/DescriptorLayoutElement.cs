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
/// Represents a description of an <see cref="DescriptorLayoutDescription"/> element.
/// </summary>
public struct DescriptorLayoutElement : IDistinct<DescriptorLayoutElement>
{
    /// <summary>
    /// The type of the descriptor layout element.
    /// </summary>
    public DescriptorType Type;

    /// <summary>
    /// The stages of the pipeline the descriptor layout element is used.
    /// </summary>
    public ShaderStage Stage;

    /// <summary>
    /// The index of the descriptor layout element.
    /// </summary>
    public uint Index;

    /// <inheritdoc/>
    public readonly bool Equals(DescriptorLayoutElement other)
        => other.Type == Type && other.Index == Index;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DescriptorLayoutElement other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Type, Index);

    /// <inheritdoc/>
    public static bool operator ==(DescriptorLayoutElement left, DescriptorLayoutElement right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(DescriptorLayoutElement left, DescriptorLayoutElement right) => !left.Equals(right);
}
