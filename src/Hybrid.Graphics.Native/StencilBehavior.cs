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
/// Represents a description of stencil behavior.
/// </summary>
public struct StencilBehavior : IDistinct<StencilBehavior>
{
    /// <summary>
    /// The default stencil behavior.
    /// </summary>
    public static readonly StencilBehavior Default = new()
    {
        StencilFunction = ComparisonFunction.Always,
        Pass = StencilOperation.Keep,
        Fail = StencilOperation.Keep,
        DepthFail = StencilOperation.Keep,
    };

    /// <summary>
    /// The keep stencil behavior.
    /// </summary>
    public static readonly StencilBehavior Keep = new()
    {
        StencilFunction = ComparisonFunction.Equal,
        Pass = StencilOperation.Keep,
        Fail = StencilOperation.Keep,
        DepthFail = StencilOperation.Keep,
    };

    /// <summary>
    /// The increment stencil behavior.
    /// </summary>
    public static readonly StencilBehavior Increment = Keep with
    {
        Pass = StencilOperation.Increment,
    };

    /// <summary>
    /// The decrement stencil behavior.
    /// </summary>
    public static readonly StencilBehavior Decrement = Keep with
    {
        Pass = StencilOperation.Decrement,
    };

    /// <summary>
    /// The stencil comparison function.
    /// </summary>
    public ComparisonFunction StencilFunction;

    /// <summary>
    /// The stencil success operation.
    /// </summary>
    public StencilOperation Pass;

    /// <summary>
    /// The stencil fail operation.
    /// </summary>
    public StencilOperation Fail;

    /// <summary>
    /// The stencil depth fail operation.
    /// </summary>
    public StencilOperation DepthFail;

    /// <inheritdoc/>
    public readonly bool Equals(StencilBehavior other)
        => other.StencilFunction == StencilFunction
        && other.Pass == Pass
        && other.Fail == Fail
        && other.DepthFail == DepthFail;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is StencilBehavior other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        StencilFunction, Pass, Fail, DepthFail);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(StencilBehavior left, StencilBehavior right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(StencilBehavior left, StencilBehavior right) => !left.Equals(right);
}
