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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hybrid.Numerics;

/// <summary>
/// Represents a size with two components; width and height.
/// The size is backed by two 32-bit floating point numbers.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct SizeF : ISize<SizeF, float>
{
    /// <summary>
    /// The size of the size in bytes.
    /// </summary>
    public const int SizeInBytes = 8;

    /// <summary>
    /// A size with both width and height set to zero.
    /// </summary>
    public static readonly SizeF Zero = new(0f);

    /// <summary>
    /// A size with both width and height set to one.
    /// </summary>
    public static readonly SizeF One = new(1f);

    /// <summary>
    /// The width of the size.
    /// </summary>
    public float Width;

    /// <summary>
    /// The height of the size.
    /// </summary>
    public float Height;

    /// <summary>
    /// Constructs a size.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public SizeF(float width, float height) => (Width, Height) = (width, height);

    /// <summary>
    /// Constructs a size with a single value for both width and height.
    /// </summary>
    /// <param name="value">The value of both width and height.</param>
    public SizeF(float value) => (Width, Height) = (value, value);

    /// <inheritdoc/>
    public readonly bool Equals(SizeF other)
        => other.Width == Width
        && other.Height == Height;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is SizeF other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <inheritdoc/>
    public readonly override string ToString() => $"{Width},{Height}";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Add(ref readonly SizeF left, ref readonly SizeF right)
        => new(left.Width + right.Width, left.Height + right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Add(ref readonly SizeF left, float right)
        => new(left.Width + right, left.Height + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Subtract(ref readonly SizeF left, ref readonly SizeF right)
        => new(left.Width - right.Width, left.Height - right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Subtract(ref readonly SizeF left, float right)
        => new(left.Width - right, left.Height - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Divide(ref readonly SizeF left, ref readonly SizeF right)
        => new(left.Width / right.Width, left.Height / right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Divide(ref readonly SizeF left, float right)
        => new(left.Width / right, left.Height / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Multiply(ref readonly SizeF left, ref readonly SizeF right)
        => new(left.Width * right.Width, left.Height * right.Height);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF Multiply(ref readonly SizeF left, float right)
        => new(left.Width * right, left.Height * right);

    /// <inheritdoc/>
    public static bool operator ==(SizeF left, SizeF right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(SizeF left, SizeF right) => !left.Equals(right);

    /// <inheritdoc cref="Add(ref readonly SizeF, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator +(SizeF left, SizeF right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly SizeF, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator +(SizeF left, float right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly SizeF, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator -(SizeF left, SizeF right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly SizeF, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator -(SizeF left, float right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly SizeF, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator /(SizeF left, SizeF right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly SizeF, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator /(SizeF left, float right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly SizeF, ref readonly SizeF)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator *(SizeF left, SizeF right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly SizeF, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SizeF operator *(SizeF left, float right) => Multiply(in left, right);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> into a size.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator SizeF(Vector2 vector) => new(vector.X, vector.Y);
}
