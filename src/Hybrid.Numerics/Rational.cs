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
/// Represents a rational number.
/// </summary>
[DebuggerDisplay("(N = {Numerator}, D = {Denominator})")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Rational : INumeric<Rational>
{
    /// <summary>
    /// The data structure in bytes.
    /// </summary>
    public const int SizeInBytes = 8;

    /// <summary>
    /// The numerator of the rational number.
    /// </summary>
    public readonly int Numerator;

    /// <summary>
    /// The denominator of the rational number.
    /// </summary>
    public readonly int Denominator;

    /// <summary>
    /// The ratio of the rational number.
    /// </summary>
    public readonly float Ratio => Numerator / Denominator;

    /// <summary>
    /// Constructs a rational number.
    /// </summary>
    /// <param name="numerator">The numerator of the rational number.</param>
    /// <param name="denominator">The denominator of the rational number.</param>
    public Rational(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    /// <summary>
    /// Deconstructs the rational number.
    /// </summary>
    /// <param name="numerator">The numerator of the rational number.</param>
    /// <param name="denominator">The denominator of the rational number.</param>
    public readonly void Deconstruct(
        out int numerator, out int denominator)
    {
        numerator = Numerator;
        denominator = Denominator;
    }

    /// <inheritdoc/>
    public readonly bool Equals(Rational other)
        => other.Numerator == Numerator
        && other.Denominator == Denominator;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Rational other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Numerator, Denominator);

    /// <inheritdoc/>
    public readonly override string ToString() => $"{Numerator}/{Denominator}";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rational left, Rational right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rational left, Rational right) => !left.Equals(right);

    /// <summary>
    /// Implicitly gets the ratio of the rational number.
    /// </summary>
    /// <param name="rational">The rational number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Rational rational) => rational.Ratio;
}
