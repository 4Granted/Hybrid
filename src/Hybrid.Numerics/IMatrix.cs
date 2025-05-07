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
/// to implement matrix properties.
/// </summary>
/// <typeparam name="TSelf">The matrix type.</typeparam>
public interface IMatrix<TSelf> : INumeric<TSelf>,
    IAdditionOperators<TSelf, TSelf, TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>
    where TSelf : IMatrix<TSelf>
{
    /// <summary>
    /// Adds the <paramref name="right"/> matrix to the <paramref name="left"/> matrix.
    /// </summary>
    /// <param name="left">The left-hand matrix.</param>
    /// <param name="right">The right-hand matrix.</param>
    /// <returns>The sum of the matrices.</returns>
    public static abstract TSelf Add(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Subtracts the <paramref name="right"/> matrix from the <paramref name="left"/> matrix.
    /// </summary>
    /// <param name="left">The left-hand matrix.</param>
    /// <param name="right">The right-hand matrix.</param>
    /// <returns>The difference of the matrices.</returns>
    public static abstract TSelf Subtract(ref readonly TSelf left, ref readonly TSelf right);

    /// <summary>
    /// Multiplies the <paramref name="left"/> matrix by the <paramref name="right"/> matrix.
    /// </summary>
    /// <param name="left">The left-hand matrix.</param>
    /// <param name="right">The right-hand matrix.</param>
    /// <returns>The product of the matrices.</returns>
    public static abstract TSelf Multiply(ref readonly TSelf left, ref readonly TSelf right);
}
