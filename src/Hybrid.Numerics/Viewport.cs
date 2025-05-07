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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hybrid.Numerics;

/// <summary>
/// Represents a viewport.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential)]
public struct Viewport : INumeric<Viewport>
{
    /// <summary>
    /// The size of the data structure in bytes.
    /// </summary>
    public const int SizeInBytes = 24;

    /// <summary>
    /// The X coordinate of the viewport.
    /// </summary>
    public int X;

    /// <summary>
    /// The Y coordinate of the viewport.
    /// </summary>
    public int Y;

    /// <summary>
    /// The width of the viewport.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height of the viewport.
    /// </summary>
    public int Height;

    /// <summary>
    /// The minimum depth of the viewport.
    /// </summary>
    public float MinimumDepth;

    /// <summary>
    /// The maximum depth of the viewport.
    /// </summary>
    public float MaximumDepth;

    /// <summary>
    /// Constructs a viewport.
    /// </summary>
    /// <param name="x">The X coordinate of the viewport.</param>
    /// <param name="y">The Y coordinate of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    /// <param name="minimumDepth">The minimum depth of the viewport.</param>
    /// <param name="maximumDepth">The maximum depth of the viewport.</param>
    public Viewport(int x, int y,
        int width, int height,
        float minimumDepth,
        float maximumDepth)
    {
        X = x; Y = y;
        Width = width;
        Height = height;
        MinimumDepth = minimumDepth;
        MaximumDepth = maximumDepth;
    }

    /// <summary>
    /// Constructs a viewport.
    /// </summary>
    /// <param name="x">The X coordinate of the viewport.</param>
    /// <param name="y">The Y coordinate of the viewport.</param>
    /// <param name="width">The width of the viewport.</param>
    /// <param name="height">The height of the viewport.</param>
    public Viewport(int x, int y,
        int width, int height)
    {
        X = x; Y = y;
        Width = width;
        Height = height;
        MinimumDepth = 0f;
        MaximumDepth = 1f;
    }

    /// <summary>
    /// Deconstructs the two-dimensional size.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public readonly void Deconstruct(
        out int x, out int y,
        out int width, out int height,
        out float minimumDepth,
        out float maximumDepth)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
        minimumDepth = MinimumDepth;
        maximumDepth = MaximumDepth;
    }

    /// <summary>
    /// Deconstructs the two-dimensional size.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public readonly void Deconstruct(
        out int width, out int height)
    {
        width = Width;
        height = Height;
    }

    /// <inheritdoc/>
    public readonly bool Equals(Viewport other)
        => other.X == X && other.Y == Y
        && other.Width == Width
        && other.Height == Height
        && other.MinimumDepth == MinimumDepth
        && other.MaximumDepth == MaximumDepth;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Viewport other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        X, Y, Width, Height, MinimumDepth, MaximumDepth);

    /// <inheritdoc/>
    public readonly override string ToString() => $"[{X},{Y},{Width},{Height}]";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Viewport left, Viewport right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Viewport left, Viewport right) => !left.Equals(right);

    /// <summary>
    /// Implicitly converts a viewport to a rectangle.
    /// </summary>
    /// <param name="viewport">The viewport to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator RectangleF(Viewport viewport)
        => new(viewport.X, viewport.Y, viewport.Width, viewport.Height);

    /// <summary>
    /// Implicitly converts a viewport to a rectangle.
    /// </summary>
    /// <param name="viewport">The viewport to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rectangle(Viewport viewport)
        => new(viewport.X, viewport.Y, viewport.Width, viewport.Height);
}
