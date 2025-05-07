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
/// Represents a description of a depth state.
/// </summary>
public struct DepthStencilState : IDistinct<DepthStencilState>
{
    /// <summary>
    /// The default depth stencil state.
    /// </summary>
    public static readonly DepthStencilState Default = new()
    {
        StencilFront = StencilBehavior.Default,
        StencilBack = StencilBehavior.Default,
        DepthComparison = ComparisonFunction.LessEqual,
        StencilReadMask = byte.MaxValue,
        StencilWriteMask = byte.MaxValue,
        DepthEnabled = false,
        DepthWriteEnabled = false,
        StencilEnabled = false,
    };

    /// <summary>
    /// The read depth stencil state.
    /// </summary>
    public static readonly DepthStencilState DepthRead = Default with
    {
        DepthEnabled = true,
    };

    /// <summary>
    /// The read-write depth stencil state.
    /// </summary>
    public static readonly DepthStencilState DepthReadWrite = DepthRead with
    {
        DepthWriteEnabled = true,
    };

    /// <summary>
    /// The stencil behavior for front faces.
    /// </summary>
    public StencilBehavior StencilFront;

    /// <summary>
    /// The stencil behavior for back faces.
    /// </summary>
    public StencilBehavior StencilBack;

    /// <summary>
    /// The depth comparison function.
    /// </summary>
    public ComparisonFunction DepthComparison;

    /// <summary>
    /// The stencil read mask.
    /// </summary>
    public byte StencilReadMask;

    /// <summary>
    /// The stencil write mask.
    /// </summary>
    public byte StencilWriteMask;

    /// <summary>
    /// Whether depth operations are enabled.
    /// </summary>
    public bool DepthEnabled;

    /// <summary>
    /// Whether depth write operations are enabled.
    /// </summary>
    public bool DepthWriteEnabled;

    /// <summary>
    /// Whether stencil operations are enabled.
    /// </summary>
    public bool StencilEnabled;

    /// <inheritdoc/>
    public readonly bool Equals(DepthStencilState other)
        => other.DepthComparison == DepthComparison
        && other.StencilFront == StencilFront
        && other.StencilBack == StencilBack
        && other.StencilReadMask == StencilReadMask
        && other.StencilWriteMask == StencilWriteMask
        && other.DepthEnabled == DepthEnabled
        && other.DepthWriteEnabled == DepthWriteEnabled
        && other.StencilEnabled == StencilEnabled;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DepthStencilState other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        DepthComparison, StencilFront, StencilBack, StencilReadMask,
        StencilWriteMask, DepthEnabled, DepthWriteEnabled, StencilEnabled);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(DepthStencilState left, DepthStencilState right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(DepthStencilState left, DepthStencilState right) => !left.Equals(right);
}
