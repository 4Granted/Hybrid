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

using System.Numerics;

namespace Hybrid.Numerics;

/// <summary>
/// Represents a contract that requires a numeric
/// to implement vector properties.
/// </summary>
/// <typeparam name="TSelf">The vector type.</typeparam>
public interface IVector<TSelf> : INumeric<TSelf>,
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>
    where TSelf : IVector<TSelf>
{
    /// <summary>
    /// Gets the normalized vector.
    /// </summary>
    public TSelf Normalized { get; }

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    public float Length { get; }

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    public float LengthSquared { get; }

    /// <summary>
    /// Normalizes the vector to unit length.
    /// </summary>
    public void Normalize();

    /// <summary>
    /// Adds the <paramref name="right"/> vector to the <paramref name="left"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand vector.</param>
    /// <returns>The sum of the vectors.</returns>
    public static abstract TSelf Add(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Adds the <paramref name="right"/> number to the <paramref name="left"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The sum of the vector and number.</returns>
    public static abstract TSelf Add(ref readonly TSelf left, float right);

    /// <summary>
    /// Subtracts the <paramref name="right"/> vector from the <paramref name="left"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand vector.</param>
    /// <returns>The difference of the vectors.</returns>
    public static abstract TSelf Subtract(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Subtracts the <paramref name="right"/> number from the <paramref name="left"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The difference of the vector and number.</returns>
    public static abstract TSelf Subtract(ref readonly TSelf left, float right);

    /// <summary>
    /// Divides the <paramref name="left"/> vector by the <paramref name="right"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand vector.</param>
    /// <returns>The quotient of the vectors.</returns>
    public static abstract TSelf Divide(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Divides the <paramref name="left"/> vector by the <paramref name="right"/> number.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The quotient of the vector and number.</returns>
    public static abstract TSelf Divide(ref readonly TSelf left, float right);

    /// <summary>
    /// Multiplies the <paramref name="left"/> vector by the <paramref name="right"/> vector.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand vector.</param>
    /// <returns>The product of the vectors.</returns>
    public static abstract TSelf Multiply(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Multiplies the <paramref name="left"/> vector by the <paramref name="right"/> number.
    /// </summary>
    /// <param name="left">The left-hand vector.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The product of the vector and number.</returns>
    public static abstract TSelf Multiply(ref readonly TSelf left, float right);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static abstract TSelf Transform(ref readonly TSelf vector, ref readonly Matrix4x4 matrix);

    /// <summary>
    /// Gets the positive identity of the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>The vector.</returns>
    public static abstract TSelf operator +(TSelf vector);

    /// <summary>
    /// Gets the negative identity of the <paramref name="vector"/>.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>The vector.</returns>
    public static abstract TSelf operator -(TSelf vector);
}
