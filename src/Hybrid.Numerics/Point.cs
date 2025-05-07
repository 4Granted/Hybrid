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
/// Represents a point with two components; X and Y.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Point : INumeric<Point>
{
    /// <summary>
    /// The size of the vector in bytes.
    /// </summary>
    public const int SizeInBytes = 8;

    /// <summary>
    /// A vector with all components set to zero.
    /// </summary>
    public static readonly Point Zero = new(0);

    /// <summary>
    /// A vector with all components set to one.
    /// </summary>
    public static readonly Point One = new(1);

    /// <summary>
    /// A vector with the X component set to
    /// one and the Y component set to zero.
    /// </summary>
    public static readonly Point UnitX = new(1, 0);

    /// <summary>
    /// A vector with the X component set to
    /// zero and the Y component set to one.
    /// </summary>
    public static readonly Point UnitY = new(0, 1);

    /// <summary>
    /// The X component of the point.
    /// </summary>
    public int X;

    /// <summary>
    /// The Y component of the point.
    /// </summary>
    public int Y;

    /// <summary>
    /// Constructs a point.
    /// </summary>
    /// <param name="x">The X component of the point.</param>
    /// <param name="y">The Y component of the point.</param>
    public Point(int x, int y) => (X, Y) = (x, y);

    /// <summary>
    /// Constructs a point with a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components.</param>
    public Point(int value) => (X, Y) = (value, value);

    /// <inheritdoc/>
    public readonly bool Equals(Point other)
        => other.X == X && other.Y == Y;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Point other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    public readonly override string ToString() => $"<{X}, {Y}>";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Add(ref readonly Point left, ref readonly Point right)
        => new(left.X + right.X, left.Y + right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Add(ref readonly Point left, int right)
        => new(left.X + right, left.Y + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Subtract(ref readonly Point left, ref readonly Point right)
        => new(left.X - right.X, left.Y - right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Subtract(ref readonly Point left, int right)
        => new(left.X - right, left.Y - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Divide(ref readonly Point left, ref readonly Point right)
        => new(left.X / right.X, left.Y / right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Divide(ref readonly Point left, int right)
        => new(left.X / right, left.Y / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Multiply(ref readonly Point left, ref readonly Point right)
        => new(left.X * right.X, left.Y * right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point Multiply(ref readonly Point left, int right)
        => new(left.X * right, left.Y * right);

    /// <inheritdoc/>
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Point left, Point right) => !left.Equals(right);

    /// <inheritdoc/>
    public static Point operator +(Point vector) => new(+vector.X, +vector.Y);

    /// <inheritdoc/>
    public static Point operator -(Point vector) => new(-vector.X, -vector.Y);

    /// <inheritdoc cref="Add(ref readonly Point, ref readonly Point)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator +(Point left, Point right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly Point, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator +(Point left, int right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly Point, ref readonly Point)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator -(Point left, Point right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Point, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator -(Point left, int right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly Point, ref readonly Point)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator /(Point left, Point right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly Point, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator /(Point left, int right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly Point, ref readonly Point)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator *(Point left, Point right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Point, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Point operator *(Point left, int right) => Multiply(in left, right);
}
