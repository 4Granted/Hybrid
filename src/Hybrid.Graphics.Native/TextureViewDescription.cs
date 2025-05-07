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
/// Represents a description of a <see cref="ITextureViewImpl"/>.
/// </summary>
public struct TextureViewDescription : IDistinct<TextureViewDescription>
{
    /// <summary>
    /// The target of the texture view.
    /// </summary>
    public ITextureImpl Texture;

    /// <summary>
    /// The start of the array layer.
    /// </summary>
    public uint ArrayStart;

    /// <summary>
    /// The amount of array layers.
    /// </summary>
    public uint ArrayCount;

    /// <summary>
    /// The start of the mip level.
    /// </summary>
    public uint MipStart;

    /// <summary>
    /// The amount of mip levels.
    /// </summary>
    public uint MipCount;

    /// <inheritdoc/>
    public readonly bool Equals(TextureViewDescription other)
        => other.Texture.Id == Texture.Id
        && other.ArrayStart == ArrayStart
        && other.ArrayCount == ArrayCount
        && other.MipStart == MipStart
        && other.MipCount == MipCount;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is TextureViewDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        Texture, ArrayStart, ArrayCount, MipStart, MipCount);

    /// <inheritdoc/>
    public static bool operator ==(TextureViewDescription left, TextureViewDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(TextureViewDescription left, TextureViewDescription right) => !left.Equals(right);
}
