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
/// Represents a vector with four components; X, Y, Z, and W.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Vector4 : IVector<Vector4>
{
    /// <summary>
    /// The size of the vector in bytes.
    /// </summary>
    public const int SizeInBytes = 16;

    /// <summary>
    /// A vector with all components set to zero.
    /// </summary>
    public static readonly Vector4 Zero = new(0f);

    /// <summary>
    /// A vector with all components set to one.
    /// </summary>
    public static readonly Vector4 One = new(1f);

    /// <summary>
    /// A vector with the Y, Z, and W components set
    /// to zero and the X component set to one.
    /// </summary>
    public static readonly Vector4 UnitX = new(1f, 0f, 0f, 0f);

    /// <summary>
    /// A vector with the X, Z, and W components set
    /// to zero and the Y component set to one.
    /// </summary>
    public static readonly Vector4 UnitY = new(0f, 1f, 0f, 0f);

    /// <summary>
    /// A vector with the X, Y, and W components set
    /// to zero and the Z component set to one.
    /// </summary>
    public static readonly Vector4 UnitZ = new(0f, 0f, 1f, 0f);

    /// <summary>
    /// A vector with the X, Y, and Z components set
    /// to zero and the W component set to one.
    /// </summary>
    public static readonly Vector4 UnitW = new(0f, 0f, 0f, 1f);

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

    /// <summary>
    /// The W component of the vector.
    /// </summary>
    public float W;

    /// <inheritdoc/>
    public readonly Vector4 Normalized
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
    public readonly Vector3 Xyz => new(X, Y, Z);

    /// <summary>
    /// Gets the ZYX pair of the vector.
    /// </summary>
    public readonly Vector3 Zyx => new(Z, Y, X);

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
    public readonly float LengthSquared => X * X + Y * Y + Z * Z + W * W;

    /// <summary>
    /// Constructs a vector.
    /// </summary>
    /// <param name="x">The X component of the vector.</param>
    /// <param name="y">The Y component of the vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    /// <param name="w">The W component of the vector.</param>
    public Vector4(float x, float y, float z, float w)
        => (X, Y, Z, W) = (x, y, z, w);

    /// <summary>
    /// Constructs a vector with four components
    /// from a vector with three components.
    /// </summary>
    /// <param name="vector">The two-dimensional vector.</param>
    /// <param name="w">The W component of the vector.</param>
    public Vector4(Vector3 vector, float w) => (X, Y, Z, W) = (vector.X, vector.Y, vector.Z, w);

    /// <summary>
    /// Constructs a vector with four components
    /// from a vector with three components.
    /// </summary>
    /// <param name="vector">The two-dimensional vector.</param>
    public Vector4(Vector3 vector) => (X, Y, Z, W) = (vector.X, vector.Y, vector.Z, 0f);

    /// <summary>
    /// Constructs a vector with four components
    /// from a vector with two components.
    /// </summary>
    /// <param name="vector">The four-dimensional vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    /// <param name="w">The W component of the vector.</param>
    public Vector4(Vector2 vector, float z, float w) => (X, Y, Z, W) = (vector.X, vector.Y, z, w);

    /// <summary>
    /// Constructs a vector with four components
    /// from a vector with two components.
    /// </summary>
    /// <param name="vector">The four-dimensional vector.</param>
    /// <param name="z">The Z component of the vector.</param>
    public Vector4(Vector2 vector, float z) => (X, Y, Z, W) = (vector.X, vector.Y, z, 0f);

    /// <summary>
    /// Constructs a vector with four components
    /// from a vector with two components.
    /// </summary>
    /// <param name="vector">The two-dimensional vector.</param>
    public Vector4(Vector2 vector) => (X, Y, Z, W) = (vector.X, vector.Y, 0f, 0f);

    /// <summary>
    /// Constructs a vector with a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components.</param>
    public Vector4(float value) => (X, Y, Z, W) = (value, value, value, value);

    /// <inheritdoc/>
    public void Normalize()
    {
        var scale = 1f / Length;

        X *= scale;
        Y *= scale;
        Z *= scale;
        W *= scale;
    }

    /// <inheritdoc/>
    public readonly bool Equals(Vector4 other)
        => other.X == X && other.Y == Y
        && other.Z == Z && other.W == W;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector4 other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    /// <inheritdoc/>
    public readonly override string ToString() => $"<{X}, {Y}, {Z}, {W}>";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Add(ref readonly Vector4 left, ref readonly Vector4 right)
        => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Add(ref readonly Vector4 left, float right)
        => new(left.X + right, left.Y + right, left.Z + right, left.W + right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Subtract(ref readonly Vector4 left, ref readonly Vector4 right)
        => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Subtract(ref readonly Vector4 left, float right)
        => new(left.X - right, left.Y - right, left.Z - right, left.W - right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Divide(ref readonly Vector4 left, ref readonly Vector4 right)
        => new(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Divide(ref readonly Vector4 left, float right)
        => new(left.X / right, left.Y / right, left.Z / right, left.W / right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Multiply(ref readonly Vector4 left, ref readonly Vector4 right)
        => new(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Multiply(ref readonly Vector4 left, float right)
        => new(left.X * right, left.Y * right, left.Z * right, left.W * right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Transform(ref readonly Vector4 vector, ref readonly Matrix4x4 matrix)
        => new(vector.X * matrix.M11 + vector.Y * matrix.M21 + vector.Z * matrix.M31 + vector.W * matrix.M41,
            vector.X * matrix.M12 + vector.Y * matrix.M22 + vector.Z * matrix.M32 + vector.W * matrix.M42,
            vector.X * matrix.M13 + vector.Y * matrix.M23 + vector.Z * matrix.M33 + vector.W * matrix.M43,
            vector.X * matrix.M14 + vector.Y * matrix.M24 + vector.Z * matrix.M34 + vector.W * matrix.M44);

    /// <inheritdoc/>
    public static bool operator ==(Vector4 left, Vector4 right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Vector4 left, Vector4 right) => !left.Equals(right);

    /// <inheritdoc/>
    public static Vector4 operator +(Vector4 vector)
        => new(+vector.X, +vector.Y, +vector.Z, +vector.W);

    /// <inheritdoc/>
    public static Vector4 operator -(Vector4 vector)
        => new(-vector.X, -vector.Y, -vector.Z, -vector.W);

    /// <inheritdoc cref="Add(ref readonly Vector4, ref readonly Vector4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 left, Vector4 right) => Add(in left, in right);

    /// <inheritdoc cref="Add(ref readonly Vector4, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator +(Vector4 left, float right) => Add(in left, right);

    /// <inheritdoc cref="Subtract(ref readonly Vector4, ref readonly Vector4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 left, Vector4 right) => Subtract(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Vector4, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator -(Vector4 left, float right) => Subtract(in left, right);

    /// <inheritdoc cref="Divide(ref readonly Vector4, ref readonly Vector4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, Vector4 right) => Divide(in left, in right);

    /// <inheritdoc cref="Divide(ref readonly Vector4, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator /(Vector4 left, float right) => Divide(in left, right);

    /// <inheritdoc cref="Multiply(ref readonly Vector4, ref readonly Vector4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, Vector4 right) => Multiply(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Vector4, float)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 operator *(Vector4 left, float right) => Multiply(in left, right);

    /// <inheritdoc cref="Transform(ref readonly Vector4, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 operator *(Vector4 left, Matrix4x4 right) => Transform(in left, in right);

    /// <summary>
    /// Implicitly converts the <paramref name="color"/> with
    /// four components into a vector with four components.
    /// </summary>
    /// <param name="color">The color.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Color color)
        => new(color.R, color.G, color.B, color.A);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// three components into a vector with four components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Vector3 vector) => new(vector);

    /// <summary>
    /// Implicitly converts the <paramref name="vector"/> with
    /// two components into a vector with four components.
    /// </summary>
    /// <param name="vector">The vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector4(Vector2 vector) => new(vector);
}
