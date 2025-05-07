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
/// Represents a description of a blend target.
/// </summary>
public struct BlendTarget : IDistinct<BlendTarget>
{
    /// <summary>
    /// The default blend target.
    /// </summary>
    public static readonly BlendTarget Default = new()
    {
        ColorFunction = BlendFunction.Add,
        AlphaFunction = BlendFunction.Add,
        SourceColorFactor = BlendFactor.One,
        SourceAlphaFactor = BlendFactor.One,
        DestinationColorFactor = BlendFactor.Zero,
        DestinationAlphaFactor = BlendFactor.Zero,
        ColorWriteMask = ColorChannels.RGB,
        BlendEnabled = true,
    };

    /// <summary>
    /// The disabled blend target.
    /// </summary>
    public static readonly BlendTarget Disabled = Default with
    {
        BlendEnabled = false,
    };

    /// <summary>
    /// The additive blend target.
    /// </summary>
    public static readonly BlendTarget Additive = Default with
    {
        SourceColorFactor = BlendFactor.SourceAlpha,
        SourceAlphaFactor = BlendFactor.SourceAlpha,
        DestinationColorFactor = BlendFactor.One,
        DestinationAlphaFactor = BlendFactor.One,
        ColorWriteMask = ColorChannels.All,
    };

    /// <summary>
    /// The alpha blend target.
    /// </summary>
    public static readonly BlendTarget Alpha = Default with
    {
        DestinationColorFactor = BlendFactor.InverseSourceAlpha,
        DestinationAlphaFactor = BlendFactor.InverseSourceAlpha,
        ColorWriteMask = ColorChannels.All,
    };

    /// <summary>
    /// The alpha blend target.
    /// </summary>
    public static readonly BlendTarget NonPremultiplied = Default with
    {
        SourceColorFactor = BlendFactor.SourceAlpha,
        SourceAlphaFactor = BlendFactor.SourceAlpha,
        DestinationColorFactor = BlendFactor.InverseSourceAlpha,
        DestinationAlphaFactor = BlendFactor.InverseSourceAlpha,
    };

    /// <summary>
    /// The opaque blend target.
    /// </summary>
    public static readonly BlendTarget Opaque = Default with
    {
        BlendEnabled = true,
    };

    /// <summary>
    /// The color function.
    /// </summary>
    public BlendFunction ColorFunction;

    /// <summary>
    /// The alpha function.
    /// </summary>
    public BlendFunction AlphaFunction;

    /// <summary>
    /// The source color factor.
    /// </summary>
    public BlendFactor SourceColorFactor;

    /// <summary>
    /// The source alpha factor.
    /// </summary>
    public BlendFactor SourceAlphaFactor;

    /// <summary>
    /// The destination color factor.
    /// </summary>
    public BlendFactor DestinationColorFactor;

    /// <summary>
    /// The destination alpha factor.
    /// </summary>
    public BlendFactor DestinationAlphaFactor;

    /// <summary>
    /// The color write mask.
    /// </summary>
    public ColorChannels ColorWriteMask;

    /// <summary>
    /// Whether blending is enabled.
    /// </summary>
    public bool BlendEnabled;

    /// <inheritdoc/>
    public readonly bool Equals(BlendTarget other)
        => other.ColorFunction == ColorFunction
        && other.AlphaFunction == ColorFunction
        && other.SourceColorFactor == SourceColorFactor
        && other.SourceAlphaFactor == SourceAlphaFactor
        && other.DestinationColorFactor == DestinationColorFactor
        && other.DestinationAlphaFactor == DestinationAlphaFactor
        && other.ColorWriteMask == ColorWriteMask
        && other.BlendEnabled == BlendEnabled;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is BlendTarget other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        ColorFunction, AlphaFunction, SourceColorFactor, SourceAlphaFactor,
        DestinationColorFactor, DestinationAlphaFactor, ColorWriteMask, BlendEnabled);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BlendTarget left, BlendTarget right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BlendTarget left, BlendTarget right) => !left.Equals(right);
}
