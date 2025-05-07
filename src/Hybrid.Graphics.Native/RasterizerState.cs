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
/// Represents a description of a rasterizer state.
/// </summary>
public struct RasterizerState : IDistinct<RasterizerState>
{
    /// <summary>
    /// The default rasterizer state.
    /// </summary>
    public static readonly RasterizerState Default = new()
    {
        FillMode = FillMode.Solid,
        CullMode = CullMode.None,
        WindingMode = WindingMode.Clockwise,
        DepthEnabled = false,
        ScissorEnabled = false,
    };

    /// <summary>
    /// The default rasterizer state with depth enabled.
    /// </summary>
    public static readonly RasterizerState DefaultDepth = Default with
    {
        DepthEnabled = true,
    };

    /// <summary>
    /// The default rasterizer state.
    /// </summary>
    public static readonly RasterizerState Wireframe = Default with
    {
        FillMode = FillMode.Wireframe,
    };

    /// <summary>
    /// The fill mode.
    /// </summary>
    public FillMode FillMode;

    /// <summary>
    /// The cull mode.
    /// </summary>
    public CullMode CullMode;

    /// <summary>
    /// The winding mode.
    /// </summary>
    public WindingMode WindingMode;

    /// <summary>
    /// Whether depth testing is enabled.
    /// </summary>
    public bool DepthEnabled;

    /// <summary>
    /// Whether scissor rectangles are enabled.
    /// </summary>
    public bool ScissorEnabled;

    /// <inheritdoc/>
    public readonly bool Equals(RasterizerState other)
        => other.FillMode == FillMode
        && other.CullMode == CullMode
        && other.WindingMode == WindingMode
        && other.DepthEnabled == DepthEnabled
        && other.ScissorEnabled == ScissorEnabled;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is RasterizerState other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        FillMode, CullMode, WindingMode, DepthEnabled, ScissorEnabled);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RasterizerState left, RasterizerState right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RasterizerState left, RasterizerState right) => !left.Equals(right);
}
