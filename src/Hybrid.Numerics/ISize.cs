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
/// to implement size properties.
/// </summary>
/// <typeparam name="TSelf">The size type.</typeparam>
public interface ISize<TSelf> : INumeric<TSelf>,
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>
    where TSelf : ISize<TSelf>
{
    /// <summary>
    /// Adds the <paramref name="right"/> size to the <paramref name="left"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand size.</param>
    /// <returns>The sum of the sizes.</returns>
    public static abstract TSelf Add(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Adds the <paramref name="right"/> number to the <paramref name="left"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The sum of the size and number.</returns>
    public static abstract TSelf Add(ref readonly TSelf left, float right);

    /// <summary>
    /// Subtracts the <paramref name="right"/> size from the <paramref name="left"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand size.</param>
    /// <returns>The difference of the sizes.</returns>
    public static abstract TSelf Subtract(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Subtracts the <paramref name="right"/> number from the <paramref name="left"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The difference of the size and number.</returns>
    public static abstract TSelf Subtract(ref readonly TSelf left, float right);

    /// <summary>
    /// Divides the <paramref name="left"/> size by the <paramref name="right"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand size.</param>
    /// <returns>The quotient of the sizes.</returns>
    public static abstract TSelf Divide(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Divides the <paramref name="left"/> size by the <paramref name="right"/> number.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The quotient of the size and number.</returns>
    public static abstract TSelf Divide(ref readonly TSelf left, float right);

    /// <summary>
    /// Multiplies the <paramref name="left"/> size by the <paramref name="right"/> size.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand size.</param>
    /// <returns>The product of the sizes.</returns>
    public static abstract TSelf Multiply(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Multiplies the <paramref name="left"/> size by the <paramref name="right"/> number.
    /// </summary>
    /// <param name="left">The left-hand size.</param>
    /// <param name="right">The right-hand number.</param>
    /// <returns>The product of the size and number.</returns>
    public static abstract TSelf Multiply(ref readonly TSelf left, float right);
}
