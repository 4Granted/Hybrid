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
/// Represents a matrix with four rows and four columns.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Matrix4x4 : IMatrix<Matrix4x4>
{
    /// <summary>
    /// An identity matrix.
    /// </summary>
    public static readonly Matrix4x4 Identity = new(
        Vector4.UnitX, Vector4.UnitY,
        Vector4.UnitZ, Vector4.UnitW);

    /// <summary>
    /// The size of the matrix in bytes.
    /// </summary>
    public static readonly int SizeInBytes = Vector4.SizeInBytes * 4;

    /// <summary>
    /// The first row of the matrix.
    /// </summary>
    public Vector4 Row1;

    /// <summary>
    /// The second row of the matrix.
    /// </summary>
    public Vector4 Row2;

    /// <summary>
    /// The third row of the matrix.
    /// </summary>
    public Vector4 Row3;

    /// <summary>
    /// The fourth row of the matrix.
    /// </summary>
    public Vector4 Row4;

    /// <summary>
    /// Gets or sets the first column of the matrix.
    /// </summary>
    public Vector4 Column1
    {
        readonly get => new(Row1.X, Row2.X, Row3.X, Row4.X);
        set
        {
            Row1.X = value.X;
            Row2.X = value.Y;
            Row3.X = value.Z;
            Row4.X = value.W;
        }
    }

    /// <summary>
    /// Gets or sets the second column of the matrix.
    /// </summary>
    public Vector4 Column2
    {
        readonly get => new(Row1.Y, Row2.Y, Row3.Y, Row4.Y);
        set
        {
            Row1.Y = value.X;
            Row2.Y = value.Y;
            Row3.Y = value.Z;
            Row4.Y = value.W;
        }
    }

    /// <summary>
    /// Gets or sets the third column of the matrix.
    /// </summary>
    public Vector4 Column3
    {
        readonly get => new(Row1.Z, Row2.Z, Row3.Z, Row4.Z);
        set
        {
            Row1.Z = value.X;
            Row2.Z = value.Y;
            Row3.Z = value.Z;
            Row4.Z = value.W;
        }
    }

    /// <summary>
    /// Gets or sets the fourth column of the matrix.
    /// </summary>
    public Vector4 Column4
    {
        readonly get => new(Row1.W, Row2.W, Row3.W, Row4.W);
        set
        {
            Row1.W = value.X;
            Row2.W = value.Y;
            Row3.W = value.Z;
            Row4.W = value.W;
        }
    }

    /// <summary>
    /// Gets or sets the values of the matrix along the diagonal.
    /// </summary>
    public Vector4 Diagonal
    {
        readonly get => new(Row1.X, Row2.Y, Row3.Z, Row4.W);
        set
        {
            Row1.X = value.X;
            Row2.Y = value.Y;
            Row3.Z = value.Z;
            Row4.W = value.W;
        }
    }

    /// <summary>
    /// Gets or sets the first row, first column of the matrix.
    /// </summary>
    public float M11
    {
        readonly get => Row1.X;
        set => Row1.X = value;
    }

    /// <summary>
    /// Gets or sets the first row, second column of the matrix.
    /// </summary>
    public float M12
    {
        readonly get => Row1.Y;
        set => Row1.Y = value;
    }

    /// <summary>
    /// Gets or sets the first row, third column of the matrix.
    /// </summary>
    public float M13
    {
        readonly get => Row1.Z;
        set => Row1.Z = value;
    }

    /// <summary>
    /// Gets or sets the first row, fourth column of the matrix.
    /// </summary>
    public float M14
    {
        readonly get => Row1.W;
        set => Row1.W = value;
    }

    /// <summary>
    /// Gets or sets the second row, first column of the matrix.
    /// </summary>
    public float M21
    {
        readonly get => Row2.X;
        set => Row2.X = value;
    }

    /// <summary>
    /// Gets or sets the second row, second column of the matrix.
    /// </summary>
    public float M22
    {
        readonly get => Row2.Y;
        set => Row2.Y = value;
    }

    /// <summary>
    /// Gets or sets the second row, third column of the matrix.
    /// </summary>
    public float M23
    {
        readonly get => Row2.Z;
        set => Row2.Z = value;
    }

    /// <summary>
    /// Gets or sets the second row, fourth column of the matrix.
    /// </summary>
    public float M24
    {
        readonly get => Row2.W;
        set => Row2.W = value;
    }

    /// <summary>
    /// Gets or sets the third row, first column of the matrix.
    /// </summary>
    public float M31
    {
        readonly get => Row3.X;
        set => Row3.X = value;
    }

    /// <summary>
    /// Gets or sets the third row, second column of the matrix.
    /// </summary>
    public float M32
    {
        readonly get => Row3.Y;
        set => Row3.Y = value;
    }

    /// <summary>
    /// Gets or sets the third row, third column of the matrix.
    /// </summary>
    public float M33
    {
        readonly get => Row3.Z;
        set => Row3.Z = value;
    }

    /// <summary>
    /// Gets or sets the third row, fourth column of the matrix.
    /// </summary>
    public float M34
    {
        readonly get => Row3.W;
        set => Row3.W = value;
    }

    /// <summary>
    /// Gets or sets the fourth row, first column of the matrix.
    /// </summary>
    public float M41
    {
        readonly get => Row4.X;
        set => Row4.X = value;
    }

    /// <summary>
    /// Gets or sets the fourth row, second column of the matrix.
    /// </summary>
    public float M42
    {
        readonly get => Row4.Y;
        set => Row4.Y = value;
    }

    /// <summary>
    /// Gets or sets the fourth row, third column of the matrix.
    /// </summary>
    public float M43
    {
        readonly get => Row4.Z;
        set => Row4.Z = value;
    }

    /// <summary>
    /// Gets or sets the fourth row, fourth column of the matrix.
    /// </summary>
    public float M44
    {
        readonly get => Row4.W;
        set => Row4.W = value;
    }

    /// <summary>
    /// Constructs a matrix.
    /// </summary>
    /// <param name="row1">The first row of the matrix.</param>
    /// <param name="row2">The second row of the matrix.</param>
    /// <param name="row3">The third row of the matrix.</param>
    /// <param name="row4">The fourth row of the matrix.</param>
    public Matrix4x4(
        Vector4 row1, Vector4 row2,
        Vector4 row3, Vector4 row4)
        => (Row1, Row2, Row3, Row4) = (row1, row2, row3, row4);

    /// <summary>
    /// Constructs a matrix.
    /// </summary>
    /// <param name="m11">The first row, first column of the matrix.</param>
    /// <param name="m12">The first row, second column of the matrix.</param>
    /// <param name="m13">The first row, third column of the matrix.</param>
    /// <param name="m14">The first row, fouth column of the matrix.</param>
    /// <param name="m21">The second row, first column of the matrix.</param>
    /// <param name="m22">The second row, second column of the matrix</param>
    /// <param name="m23">The second row, third column of the matrix.</param>
    /// <param name="m24">The second row, fouth column of the matrix.</param>
    /// <param name="m31">The third row, first column of the matrix.</param>
    /// <param name="m32">The third row, second column of the matrix</param>
    /// <param name="m33">The third row, third column of the matrix.</param>
    /// <param name="m34">The third row, fouth column of the matrix.</param>
    /// <param name="m41">The fouth row, first column of the matrix.</param>
    /// <param name="m42">The fouth row, second column of the matrix</param>
    /// <param name="m43">The fouth row, third column of the matrix.</param>
    /// <param name="m44">The fouth row, fouth column of the matrix.</param>
    public Matrix4x4(
        float m11, float m12, float m13, float m14,
        float m21, float m22, float m23, float m24,
        float m31, float m32, float m33, float m34,
        float m41, float m42, float m43, float m44)
    {
        Row1 = new(m11, m12, m13, m14);
        Row2 = new(m21, m22, m23, m24);
        Row3 = new(m31, m32, m33, m34);
        Row4 = new(m41, m42, m43, m44);
    }

    /// <inheritdoc/>
    public readonly bool Equals(Matrix4x4 other)
        => other.Row1 == Row1 && other.Row2 == Row2
        && other.Row3 == Row3 && other.Row4 == Row4;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector2 other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Row1, Row2, Row3, Row4);

    /// <inheritdoc/>
    public readonly override string ToString() => $"[{Row1}, {Row2}, {Row3}, {Row4}]";

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Add(ref readonly Matrix4x4 left, ref readonly Matrix4x4 right)
        => new(left.Row1 + right.Row1, left.Row2 + right.Row2, left.Row3 + right.Row3, left.Row4 + right.Row4);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Subtract(ref readonly Matrix4x4 left, ref readonly Matrix4x4 right)
        => new(left.Row1 - right.Row1, left.Row2 - right.Row2, left.Row3 - right.Row3, left.Row4 - right.Row4);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Multiply(ref readonly Matrix4x4 left, ref readonly Matrix4x4 right)
    {
        var m11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41;
        var m12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42;
        var m13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43;
        var m14 = left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44;
        var m21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41;
        var m22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42;
        var m23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43;
        var m24 = left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44;
        var m31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41;
        var m32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42;
        var m33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43;
        var m34 = left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44;
        var m41 = left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41;
        var m42 = left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42;
        var m43 = left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43;
        var m44 = left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44;

        return new(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 Transpose(ref readonly Matrix4x4 matrix)
        => new(matrix.M11, matrix.M21, matrix.M31, matrix.M41,
            matrix.M12, matrix.M22, matrix.M32, matrix.M42,
            matrix.M13, matrix.M23, matrix.M33, matrix.M43,
            matrix.M14, matrix.M24, matrix.M34, matrix.M44);

    /// <summary>
    /// Creates a translation matrix from the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The translation vector.</param>
    /// <returns>The translation matrix.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateTranslation(ref readonly Vector3 vector)
        => new(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, vector.X, vector.Y, vector.Z, 1f);

    /// <summary>
    /// Creates a scale matrix from the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The scale vector.</param>
    /// <returns>The scale matrix.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 CreateScale(ref readonly Vector3 vector)
        => new (vector.X, 0f, 0f, 0f, 0f, vector.Y, 0f, 0f, 0f, 0f, vector.Z, 0f, 0f, 0f, 0f, 1f);

    /// <inheritdoc/>
    public static bool operator ==(Matrix4x4 left, Matrix4x4 right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Matrix4x4 left, Matrix4x4 right) => !left.Equals(right);

    /// <inheritdoc cref="Add(ref readonly Matrix4x4, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right) => Add(in left, in right);

    /// <inheritdoc cref="Subtract(ref readonly Matrix4x4, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right) => Subtract(in left, in right);

    /// <inheritdoc cref="Multiply(ref readonly Matrix4x4, ref readonly Matrix4x4)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right) => Multiply(in left, in right);
}
