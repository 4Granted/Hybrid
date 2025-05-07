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
/// Represents a normalized color with four
/// components; red, green, blue and alpha.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public readonly struct Color : INumeric<Color>
{
    /// <summary>
    /// A color with all components set to zero.
    /// </summary>
    public static readonly Color Zero = new(0f, 0f, 0f, 0f);

    /// <summary>
    /// A color with all components set to one.
    /// </summary>
    public static readonly Color One = new(1f, 1f, 1f, 1f);

    /// <summary>
    /// A transparent color.
    /// </summary>
    public static readonly Color Transparent = new(0, 0, 0, 0);

    /// <summary>
    /// A white color.
    /// </summary>
    public static readonly Color White = new(255, 255, 255);

    /// <summary>
    /// A black color.
    /// </summary>
    public static readonly Color Black = new(0, 0, 0);

    /// <summary>
    /// The red component of the color.
    /// </summary>
    public readonly float R;

    /// <summary>
    /// The green component of the color.
    /// </summary>
    public readonly float G;

    /// <summary>
    /// The blue component of the color.
    /// </summary>
    public readonly float B;

    /// <summary>
    /// The alpha component of the color.
    /// </summary>
    public readonly float A;

    /// <summary>
    /// Constructs a color.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <param name="a">The alpha component of the color.</param>
    public Color(float r, float g, float b, float a)
        => (R, G, B, A) = (r, g, b, a);

    /// <summary>
    /// Constructs a color with full alpha.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public Color(float r, float g, float b)
        => (R, G, B, A) = (r, g, b, 1f);

    /// <summary>
    /// Constructs a color.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    /// <param name="a">The alpha component of the color.</param>
    public Color(byte r, byte g, byte b, byte a)
    {
        R = (byte)(r / byte.MaxValue);
        G = (byte)(g / byte.MaxValue);
        B = (byte)(b / byte.MaxValue);
        A = (byte)(a / byte.MaxValue);
    }

    /// <summary>
    /// Constructs a color with full alpha.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    public Color(byte r, byte g, byte b)
    {
        R = (byte)(r / byte.MaxValue);
        G = (byte)(g / byte.MaxValue);
        B = (byte)(b / byte.MaxValue);
        A = 1f;                      
    }

    /// <inheritdoc/>
    public readonly bool Equals(Color other)
        => other.R == R && other.G == G
        && other.B == B && other.A == A;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Color other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(R, G, B, A);

    /// <inheritdoc/>
    public readonly override string ToString() => $"({R}, {G}, {B}, {A})";

    /// <summary>
    /// Multiplies the <paramref name="left"/> color by the <paramref name="right"/> color.
    /// </summary>
    /// <param name="left">The left-hand color.</param>
    /// <param name="right">The right-hand color.</param>
    /// <returns>The product of the colors.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Multiply(ref readonly Color left, ref readonly Color right)
        => new(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);

    /// <summary>
    /// Multiplies the <paramref name="left"/> color by the <paramref name="right"/> value.
    /// </summary>
    /// <param name="left">The left-hand color.</param>
    /// <param name="right">The right-hand value.</param>
    /// <returns>The product of the color and value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Multiply(ref readonly Color left, float right)
        => new(left.R * right, left.G * right, left.B * right, left.A * right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Min(ref readonly Color left, ref readonly Color right)
        => new(left.R < right.R ? left.R : right.R,
               left.G < right.G ? left.G : right.G,
               left.B < right.B ? left.B : right.B,
               left.A < right.A ? left.A : right.A);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color Max(ref readonly Color left, ref readonly Color right)
        => new(left.R > right.R ? left.R : right.R,
               left.G > right.G ? left.G : right.G,
               left.B > right.B ? left.B : right.B,
               left.A > right.A ? left.A : right.A);

    /// <summary>
    /// Unpacks a color from the unsigned 32-bit <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The unpacked color.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color UnpackColor(uint value)
        => new((value & 255) / 255f,
               (value >> 8 & 255) / 255f,
               (value >> 16 & 255) / 255f,
               (value >> 24 & 255) / 255f);

    /// <summary>
    /// Packs the <paramref name="color"/> into an unsigned 32-bit integer.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>The packed color.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint PackColor(ref readonly Color color)
    {
        var min = Min(in One, in color);
        var scaledColor = Max(in Zero, in min) * White;

        return (uint)scaledColor.R | (uint)scaledColor.G << 8 | (uint)scaledColor.B << 16 | (uint)scaledColor.A << 24;
    }

    /// <inheritdoc/>
    public static bool operator ==(Color left, Color right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Color left, Color right) => !left.Equals(right);

    /// <inheritdoc cref="Multiply(ref readonly Color, ref readonly Color)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color left, Color right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Color, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color operator *(Color left, float right) => Multiply(in left, right);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// four components into a color with four components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(Vector4 vector)
        => new(vector.X, vector.Y, vector.Z, vector.W);

    /// <summary>
    /// Implicitly unpacks a color from the unsigned 32-bit <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Color(uint value) => UnpackColor(value);

    /// <summary>
    /// Implicitly packs the <paramref name="color"/> into an unsigned 32-bit integer.
    /// </summary>
    /// <param name="color">The color.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator uint(Color color) => PackColor(in color);
}
