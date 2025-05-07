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
/// Represents a vector with two components; X and Y.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Vector2 : IVector<Vector2>
{
    /// <summary>
    /// The size of the vector in bytes.
    /// </summary>
    public const int SizeInBytes = 8;

    /// <summary>
    /// A vector with all components set to zero.
    /// </summary>
    public static readonly Vector2 Zero = new(0f);

    /// <summary>
    /// A vector with all components set to one.
    /// </summary>
    public static readonly Vector2 One = new(1f);

    /// <summary>
    /// A vector with the X component set to
    /// one and the Y component set to zero.
    /// </summary>
    public static readonly Vector2 UnitX = new(1f, 0f);

    /// <summary>
    /// A vector with the X component set to
    /// zero and the Y component set to one.
    /// </summary>
    public static readonly Vector2 UnitY = new(0f, 1f);

    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public float X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public float Y;

    /// <inheritdoc/>
    public readonly Vector2 Normalized
    {
        get
        {
            var vector = this;

            vector.Normalize();

            return vector;
        }
    }

    /// <summary>
    /// Gets the YX pair of the vector.
    /// </summary>
    public readonly Vector2 Yx => new(Y, X);

    /// <inheritdoc/>
    public readonly float Length => MathF.Sqrt(LengthSquared);

    /// <inheritdoc/>
    public readonly float LengthSquared => X * X + Y * Y;

    /// <summary>
    /// Constructs a vector.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    public Vector2(float x, float y) => (X, Y) = (x, y);

    /// <summary>
    /// Constructs a vector with two components
    /// from a vector with four components.
    /// </summary>
    /// <param name="vector">The four-dimensional vector.</param>
    public Vector2(Vector4 vector) => (X, Y) = (vector.X, vector.Y);

    /// <summary>
    /// Constructs a vector with two components
    /// from a vector with three components.
    /// </summary>
    /// <param name="vector">The three-dimensional vector.</param>
    public Vector2(Vector3 vector) => (X, Y) = (vector.X, vector.Y);

    /// <summary>
    /// Constructs a vector with a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components.</param>
    public Vector2(float value) => (X, Y) = (value, value);

    /// <inheritdoc/>
    public void Normalize()
    {
        var scale = 1f / Length;

        X *= scale;
        Y *= scale;
    }

    /// <inheritdoc/>
    public readonly bool Equals(Vector2 other)
        => other.X == X && other.Y == Y;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector2 other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    public readonly override string ToString() => $"<{X}, {Y}>";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Add(ref readonly Vector2 left, ref readonly Vector2 right)
        => new(left.X + right.X, left.Y + right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Add(ref readonly Vector2 left, float right)
        => new(left.X + right, left.Y + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Subtract(ref readonly Vector2 left, ref readonly Vector2 right)
        => new(left.X - right.X, left.Y - right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Subtract(ref readonly Vector2 left, float right)
        => new(left.X - right, left.Y - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(ref readonly Vector2 left, ref readonly Vector2 right)
        => new(left.X / right.X, left.Y / right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Divide(ref readonly Vector2 left, float right)
        => new(left.X / right, left.Y / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(ref readonly Vector2 left, ref readonly Vector2 right)
        => new(left.X * right.X, left.Y * right.Y);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Multiply(ref readonly Vector2 left, float right)
        => new(left.X * right, left.Y * right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Transform(ref readonly Vector2 vector, ref readonly Matrix4x4 matrix)
        => new(vector.X * matrix.M11 + vector.Y * matrix.M21 + matrix.M31 + matrix.M41,
            vector.X * matrix.M12 + vector.Y * matrix.M22 + matrix.M32 + matrix.M42);

    /// <inheritdoc/>
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Vector2 left, Vector2 right) => !left.Equals(right);

    /// <inheritdoc/>
    public static Vector2 operator +(Vector2 vector) => new(+vector.X, +vector.Y);

    /// <inheritdoc/>
    public static Vector2 operator -(Vector2 vector) => new(-vector.X, -vector.Y);

    /// <inheritdoc cref="Add(ref readonly Vector2, ref readonly Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 left, Vector2 right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly Vector2, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator +(Vector2 left, float right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly Vector2, ref readonly Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 left, Vector2 right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Vector2, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator -(Vector2 left, float right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly Vector2, ref readonly Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 left, Vector2 right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly Vector2, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator /(Vector2 left, float right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly Vector2, ref readonly Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, Vector2 right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Vector2, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 operator *(Vector2 left, float right) => Multiply(in left, right);

    /// <inheritdoc cref="Transform(ref readonly Vector2, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector2 left, Matrix4x4 right) => Transform(in left, in right);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// four components into a vector with two components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector4 vector) => vector.Xy;

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// three components into a vector with two components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector3 vector) => vector.Xy;
}
