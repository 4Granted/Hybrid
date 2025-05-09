﻿// Hybrid - A versatile framework for application development.
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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hybrid.Numerics;

/// <summary>
/// Represents a size with two components; width and height.
/// The size is backed by two 32-bit integers.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Size : ISize<Size, int>
{
    /// <summary>
    /// The size of the size in bytes.
    /// </summary>
    public const int SizeInBytes = 8;

    /// <summary>
    /// A size with both width and height set to zero.
    /// </summary>
    public static readonly Size Zero = new(0);

    /// <summary>
    /// A size with both width and height set to one.
    /// </summary>
    public static readonly Size One = new(1);

    /// <summary>
    /// The width of the size.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height of the size.
    /// </summary>
    public int Height;

    /// <summary>
    /// Constructs a size.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public Size(int width, int height) => (Width, Height) = (width, height);

    /// <summary>
    /// Constructs a size with a single value for both width and height.
    /// </summary>
    /// <param name="value">The value of both width and height.</param>
    public Size(int value) => (Width, Height) = (value, value);

    /// <inheritdoc/>
    public readonly bool Equals(Size other)
        => other.Width == Width
        && other.Height == Height;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Size other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <inheritdoc/>
    public readonly override string ToString() => $"{Width},{Height}";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Add(ref readonly Size left, ref readonly Size right)
        => new(left.Width + right.Width, left.Height + right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Add(ref readonly Size left, int right)
        => new(left.Width + right, left.Height + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Subtract(ref readonly Size left, ref readonly Size right)
        => new(left.Width - right.Width, left.Height - right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Subtract(ref readonly Size left, int right)
        => new(left.Width - right, left.Height - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Divide(ref readonly Size left, ref readonly Size right)
        => new(left.Width / right.Width, left.Height / right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Divide(ref readonly Size left, int right)
        => new(left.Width / right, left.Height / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Multiply(ref readonly Size left, ref readonly Size right)
        => new(left.Width * right.Width, left.Height * right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size Multiply(ref readonly Size left, int right)
        => new(left.Width * right, left.Height * right);

    /// <inheritdoc/>
    public static bool operator ==(Size left, Size right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Size left, Size right) => !left.Equals(right);

    /// <inheritdoc cref="Add(ref readonly SizeF, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator +(Size left, Size right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly Size, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator +(Size left, int right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly Size, ref readonly Size)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator -(Size left, Size right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Size, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator -(Size left, int right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly Size, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator /(Size left, Size right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly SizeF, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator /(Size left, int right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly Size, ref readonly Size)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator *(Size left, Size right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Size, int)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Size operator *(Size left, int right) => Multiply(in left, right);

    /// <summary>
    /// Implicitly converts the <paramref name="point"/> into a size.
    /// </summary>
    /// <param name="point">The point.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Size(Point point) => new(point.X, point.Y);
}
