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
/// Represents a description of an <see cref="ISamplerImpl"/>.
/// </summary>
public struct SamplerDescription : IDistinct<SamplerDescription>
{
    /// <summary>
    /// The default sampler description.
    /// </summary>
    public static readonly SamplerDescription Default = new()
    {
        AddressU = AddressMode.Clamp,
        AddressV = AddressMode.Clamp,
        AddressW = AddressMode.Clamp,
        Border = BorderColor.TransparentBlack,
        Filter = TextureFilter.LinearLinearLinear,
        Comparison = null,
        MaximumAnisotropy = 16,
        MinimumLod = uint.MinValue,
        MaximumLod = uint.MaxValue,
        LodBias = 0,
    };

    /// <summary>
    /// The point sampler description.
    /// </summary>
    public static readonly SamplerDescription Point = Default with
    {
        Filter = TextureFilter.PointPointPoint,
    };

    /// <summary>
    /// The linear sampler description.
    /// </summary>
    public static readonly SamplerDescription Linear = Default with
    {
        Filter = TextureFilter.LinearLinearLinear,
    };

    /// <summary>
    /// The anisotropic sampler description.
    /// </summary>
    public static readonly SamplerDescription Anisotropic = Default with
    {
        Filter = TextureFilter.Anisotropic,
    };

    /// <summary>
    /// The address mode of the U coordinate.
    /// </summary>
    public AddressMode AddressU;

    /// <summary>
    /// The address mode of the V coordinate.
    /// </summary>
    public AddressMode AddressV;

    /// <summary>
    /// The address mode of the W coordinate.
    /// </summary>
    public AddressMode AddressW;

    /// <summary>
    /// The border color when <see cref="AddressMode.Border"/> is used.
    /// </summary>
    public BorderColor Border;

    /// <summary>
    /// The filter of the sampler.
    /// </summary>
    public TextureFilter Filter;

    /// <summary>
    /// The comparison operator used when sampling.
    /// </summary>
    public ComparisonFunction? Comparison;

    /// <summary>
    /// The maximum anisotropy when <see cref="TextureFilter.Anisotropic"/> is used.
    /// </summary>
    public uint MaximumAnisotropy;

    /// <summary>
    /// The minimum level of detail.
    /// </summary>
    public uint MinimumLod;

    /// <summary>
    /// The maxium level of detail.
    /// </summary>
    public uint MaximumLod;

    /// <summary>
    /// The level of detail bias.
    /// </summary>
    public int LodBias;

    /// <inheritdoc/>
    public readonly bool Equals(SamplerDescription other)
        => other.AddressU == AddressU && other.AddressV == AddressV
        && other.AddressW == AddressW && other.Border == Border
        && other.Filter == Filter && other.Comparison == Comparison
        && other.MaximumAnisotropy == MaximumAnisotropy && other.MinimumLod == MinimumLod
        && other.MaximumLod == MaximumLod && other.LodBias == LodBias;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is SamplerDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() =>
        HashCode.Combine(AddressU, AddressV, AddressW, Border, Filter, Comparison,
        HashCode.Combine(MaximumAnisotropy, MinimumLod, MaximumLod, LodBias));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SamplerDescription left, SamplerDescription right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SamplerDescription left, SamplerDescription right) => !left.Equals(right);
}
