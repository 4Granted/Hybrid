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
/// Represents a description of a blend state.
/// </summary>
public struct BlendState : IDistinct<BlendState>
{
    /// <summary>
    /// The default blend state.
    /// </summary>
    public static readonly BlendState Default = new()
    {
        Target0 = BlendTarget.Default,
        Target1 = BlendTarget.Disabled,
        Target2 = BlendTarget.Disabled,
        Target3 = BlendTarget.Disabled,
        Target4 = BlendTarget.Disabled,
        Target5 = BlendTarget.Disabled,
        Target6 = BlendTarget.Disabled,
        Target7 = BlendTarget.Disabled,
        AlphaToConvergeEnabled = false,
        IndependentBlendEnabled = false,
    };

    /// <summary>
    /// The disabled blend state.
    /// </summary>
    public static readonly BlendState Disabled = Default with
    {
        Target0 = BlendTarget.Disabled,
    };

    /// <summary>
    /// The additive blend state.
    /// </summary>
    public static readonly BlendState Additive = Default with
    {
        Target0 = BlendTarget.Additive,
    };

    /// <summary>
    /// Tthe alpha blend state.
    /// </summary>
    public static readonly BlendState Alpha = Default with
    {
        Target0 = BlendTarget.Alpha,
    };

    /// <summary>
    /// The alpha blend state.
    /// </summary>
    public static readonly BlendState NonPremultiplied = Default with
    {
        Target0 = BlendTarget.NonPremultiplied,
        IndependentBlendEnabled = true,
    };

    /// <summary>
    /// The opaque blend state.
    /// </summary>
    public static readonly BlendState Opaque = Default with
    {
        Target0 = BlendTarget.Opaque,
    };

    /// <summary>
    /// The first blend target description.
    /// </summary>
    public BlendTarget Target0;

    /// <summary>
    /// The second blend target description.
    /// </summary>
    public BlendTarget Target1;

    /// <summary>
    /// The third blend target description.
    /// </summary>
    public BlendTarget Target2;

    /// <summary>
    /// The fourth blend target description.
    /// </summary>
    public BlendTarget Target3;

    /// <summary>
    /// The fifth blend target description.
    /// </summary>
    public BlendTarget Target4;

    /// <summary>
    /// The sixth blend target description.
    /// </summary>
    public BlendTarget Target5;

    /// <summary>
    /// The seventh blend target description.
    /// </summary>
    public BlendTarget Target6;

    /// <summary>
    /// The eighth blend target description.
    /// </summary>
    public BlendTarget Target7;

    /// <summary>
    /// Whether alpha-to-converge is enabled.
    /// </summary>
    public bool AlphaToConvergeEnabled;

    /// <summary>
    /// Whether indenpendent blend is enabled.
    /// </summary>
    public bool IndependentBlendEnabled;

    /// <inheritdoc/>
    public readonly bool Equals(BlendState other)
        => other.Target0 == Target0 && other.Target1 == Target1
        && other.Target2 == Target2 && other.Target3 == Target3
        && other.Target4 == Target4 && other.Target5 == Target5
        && other.Target6 == Target6 && other.Target7 == Target7
        && other.AlphaToConvergeEnabled == AlphaToConvergeEnabled
        && other.IndependentBlendEnabled == IndependentBlendEnabled;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is BlendState other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        Target0, Target1, Target2, Target3, Target4, Target5, Target6, Target7);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BlendState left, BlendState right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BlendState left, BlendState right) => !left.Equals(right);
}
