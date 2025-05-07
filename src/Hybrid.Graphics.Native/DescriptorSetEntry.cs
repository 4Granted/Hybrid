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
/// Represents a description of a <see cref="DescriptorSetDescription"/> entry.
/// </summary>
public struct DescriptorSetEntry : IDistinct<DescriptorSetEntry>
{
    /// <summary>
    /// The resource of the descriptor set entry.
    /// </summary>
    public IGpuResource Resource;

    /// <summary>
    /// The index of the descriptor set entry.
    /// </summary>
    public uint Index;

    /// <summary>
    /// The size of the descriptor set entry in bytes.
    /// </summary>
    public uint SizeInBytes;

    /// <summary>
    /// The offset of the descriptor set entry in bytes.
    /// </summary>
    public uint OffsetInBytes;

    /// <inheritdoc/>
    public readonly bool Equals(DescriptorSetEntry other)
        => other.Resource.Id == Resource.Id
        && other.SizeInBytes == SizeInBytes
        && other.OffsetInBytes == OffsetInBytes;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DescriptorSetEntry other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        Resource, SizeInBytes, OffsetInBytes);

    /// <inheritdoc/>
    public static bool operator ==(DescriptorSetEntry left, DescriptorSetEntry right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(DescriptorSetEntry left, DescriptorSetEntry right) => !left.Equals(right);
}
