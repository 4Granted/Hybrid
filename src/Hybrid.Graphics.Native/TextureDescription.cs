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
/// Represents a description of a <see cref="ITextureImpl"/>.
/// </summary>
public struct TextureDescription : IDistinct<TextureDescription>
{
    /// <summary>
    /// The dimension of the texture.
    /// </summary>
    public TextureDimension Dimension;

    /// <summary>
    /// The usage of the texture.
    /// </summary>
    public TextureUsage Usage;

    /// <summary>
    /// The format of the texture.
    /// </summary>
    public TextureFormat Format;

    /// <summary>
    /// The samples of the texture.
    /// </summary>
    public TextureSamples Samples;

    /// <summary>
    /// The width of the texture.
    /// </summary>
    public uint Width;

    /// <summary>
    /// The height of the texture.
    /// </summary>
    public uint Height;

    /// <summary>
    /// The depth of the texture.
    /// </summary>
    public uint Depth;

    /// <summary>
    /// The array size of the texture.
    /// </summary>
    public uint ArraySize;

    /// <summary>
    /// The amount of mip levels of the texture.
    /// </summary>
    public uint MipLevels;

    /// <inheritdoc/>
    public readonly bool Equals(TextureDescription other)
        => other.Dimension == Dimension && other.Usage == Usage
        && other.Format == Format && other.Samples == Samples
        && other.Width == Width && other.Height == Height
        && other.Depth == Depth && other.ArraySize == ArraySize
        && other.MipLevels == MipLevels;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is TextureDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() =>
        HashCode.Combine(Dimension, Usage, Format, Samples, Width,
        HashCode.Combine(Height, Depth, ArraySize, MipLevels));

    /// <inheritdoc/>
    public static bool operator ==(TextureDescription left, TextureDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(TextureDescription left, TextureDescription right) => !left.Equals(right);
}
