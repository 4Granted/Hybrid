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
/// Represents a vector with three components; X, Y, and Z.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Vector3 : IVector<Vector3>
{
    /// <summary>
    /// The size of the vector in bytes.
    /// </summary>
    public const int SizeInBytes = 12;

    /// <summary>
    /// A vector with all components set to zero.
    /// </summary>
    public static readonly Vector3 Zero = new(0f);

    /// <summary>
    /// A vector with all components set to one.
    /// </summary>
    public static readonly Vector3 One = new(1f);

    /// <summary>
    /// A vector with the Y and Z components set
    /// to zero and the X component set to one.
    /// </summary>
    public static readonly Vector3 UnitX = new(1f, 0f, 0f);

    /// <summary>
    /// A vector with the X and Z components set
    /// to zero and the Y component set to one.
    /// </summary>
    public static readonly Vector3 UnitY = new(0f, 1f, 0f);

    /// <summary>
    /// A vector with the X and Y components set
    /// to zero and the Z component set to one.
    /// </summary>
    public static readonly Vector3 UnitZ = new(0f, 0f, 1f);

    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public float X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public float Y;

    /// <summary>
    /// The Z component of the vector.
    /// </summary>
    public float Z;

    /// <inheritdoc/>
    public readonly Vector3 Normalized
    {
        get
        {
            var vector = this;

            vector.Normalize();

            return vector;
        }
    }

    /// <summary>
    /// Gets the XY pair of the vector.
    /// </summary>
    public readonly Vector2 Xy => new(X, Y);

    /// <summary>
    /// Gets the XZ pair of the vector.
    /// </summary>
    public readonly Vector2 Xz => new(X, Z);

    /// <inheritdoc/>
    public readonly float Length => MathF.Sqrt(LengthSquared);

    /// <inheritdoc/>
    public readonly float LengthSquared => X * X + Y * Y + Z * Z;

    /// <summary>
    /// Constructs a vector.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    public Vector3(float x, float y, float z) => (X, Y, Z) = (x, y, z);

    /// <summary>
    /// Constructs a vector with three components
    /// from a vector with four components.
    /// </summary>
    /// <param name="vector">The two-dimensional vector.</param>
    public Vector3(Vector4 vector) => (X, Y, Z) = (vector.X, vector.Y, vector.Z);

    /// <summary>
    /// Constructs a vector with three components
    /// from a vector with two components.
    /// </summary>
    /// <param name="vector">The four-dimensional vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    public Vector3(Vector2 vector, float z) => (X, Y, Z) = (vector.X, vector.Y, z);

    /// <summary>
    /// Constructs a vector with three components
    /// from a vector with two components.
    /// </summary>
    /// <param name="vector">The two-dimensional vector.</param>
    public Vector3(Vector2 vector) => (X, Y, Z) = (vector.X, vector.Y, 0f);

    /// <summary>
    /// Constructs a vector with a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components.</param>
    public Vector3(float value) => (X, Y, Z) = (value, value, value);

    /// <inheritdoc/>
    public void Normalize()
    {
        var scale = 1f / Length;

        X *= scale;
        Y *= scale;
        Z *= scale;
    }

    /// <inheritdoc/>
    public readonly bool Equals(Vector3 other)
        => other.X == X && other.Y == Y && other.Z == Z;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector3 other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z);

    /// <inheritdoc/>
    public readonly override string ToString() => $"<{X}, {Y}, {Z}>";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Add(ref readonly Vector3 left, ref readonly Vector3 right)
        => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Add(ref readonly Vector3 left, float right)
        => new(left.X + right, left.Y + right, left.Z + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Subtract(ref readonly Vector3 left, ref readonly Vector3 right)
        => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Subtract(ref readonly Vector3 left, float right)
        => new(left.X - right, left.Y - right, left.Z - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Divide(ref readonly Vector3 left, ref readonly Vector3 right)
        => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Divide(ref readonly Vector3 left, float right)
        => new(left.X / right, left.Y / right, left.Z / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Multiply(ref readonly Vector3 left, ref readonly Vector3 right)
        => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Multiply(ref readonly Vector3 left, float right)
        => new(left.X * right, left.Y * right, left.Z * right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Transform(ref readonly Vector3 vector, ref readonly Matrix4x4 matrix)
        => new(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + matrix.M41,
            vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + matrix.M42,
            vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + matrix.M43);

    /// <inheritdoc/>
    public static bool operator ==(Vector3 left, Vector3 right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Vector3 left, Vector3 right) => !left.Equals(right);

    /// <inheritdoc/>
    public static Vector3 operator +(Vector3 vector) => new(+vector.X, +vector.Y, +vector.Z);

    /// <inheritdoc/>
    public static Vector3 operator -(Vector3 vector) => new(-vector.X, -vector.Y, -vector.Z);

    /// <inheritdoc cref="Add(ref readonly Vector3, ref readonly Vector3)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 left, Vector3 right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly Vector3, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator +(Vector3 left, float right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly Vector3, ref readonly Vector3)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 left, Vector3 right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Vector3, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator -(Vector3 left, float right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly Vector3, ref readonly Vector3)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, Vector3 right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly Vector3, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator /(Vector3 left, float right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly Vector3, ref readonly Vector3)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Vector3 right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Vector3, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, float right) => Multiply(in left, right);

    /// <inheritdoc cref="Transform(ref readonly Vector3, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector3 left, Matrix4x4 right) => Transform(in left, in right);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// four components into a vector with two components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector4 vector) => vector.Xy;

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// three components into a vector with two components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector2 vector) => new(vector);
}
