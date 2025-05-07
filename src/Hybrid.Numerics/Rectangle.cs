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
using System.Runtime.InteropServices;

namespace Hybrid.Numerics;

/// <summary>
/// Represents a rectangle.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("{ToString()}")]
public struct Rectangle : IRectangle<Rectangle>
{
    /// <summary>
    /// The size of the vector in bytes.
    /// </summary>
    public const int SizeInBytes = Vector2.SizeInBytes * 2;

    /// <summary>
    /// A rectangle with all components set to zero.
    /// </summary>
    public static readonly Rectangle Zero = new(Vector2.Zero, Vector2.Zero);

    /// <summary>
    /// The minimum of the rectangle.
    /// </summary>
    public Vector2 Minimum;

    /// <summary>
    /// The maximum of the rectangle.
    /// </summary>
    public Vector2 Maximum;

    /// <inheritdoc/>
    public readonly Vector2 Position => Minimum;

    /// <inheritdoc/>
    public readonly Size Size => Maximum - Minimum;

    /// <inheritdoc/>
    public readonly Vector2 Center => (Minimum + Maximum) / 2f;

    /// <summary>
    /// Gets or sets the X coordinate of the rectangle.
    /// </summary>
    public float X
    {
        readonly get => Minimum.X;
        set => Minimum.X = value;
    }

    /// <summary>
    /// Gets or sets the Y coordinate of the rectangle.
    /// </summary>
    public float Y
    {
        readonly get => Minimum.Y;
        set => Minimum.Y = value;
    }

    /// <summary>
    /// Gets or sets the width of the rectangle.
    /// </summary>
    public float Width
    {
        readonly get => Maximum.X - Minimum.X;
        set => Maximum.X = X + value;
    }

    /// <summary>
    /// Gets or sets the height of the rectangle.
    /// </summary>
    public float Height
    {
        readonly get => Maximum.Y - Minimum.Y;
        set => Maximum.Y = Y + value;
    }

    /// <summary>
    /// Constructs a rectangle.
    /// </summary>
    /// <param name="minimum">The minimum of the rectangle.</param>
    /// <param name="maximum">The maximum of the rectangle.</param>
    public Rectangle(
        Vector2 minimum,
        Vector2 maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    /// <summary>
    /// Constructs a rectangle.
    /// </summary>
    /// <param name="position">The position of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    public Rectangle(
        Vector2 position,
        Size size)
    {
        Minimum = position;
        Maximum = new(position.X + size.Width, position.Y + size.Height);
    }

    /// <summary>
    /// Constructs a rectangle.
    /// </summary>
    /// <param name="x">The X coordinate of the rectangle.</param>
    /// <param name="y">The Y coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    public Rectangle(
        float x, float y,
        float width,
        float height)
    {
        Minimum = new(x, y);
        Maximum = new(x + width, y + height);
    }

    /// <inheritdoc/>
    public readonly bool Equals(Rectangle other)
        => other.Minimum == Minimum;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Color other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Minimum, Maximum);

    /// <inheritdoc/>
    public readonly override string ToString() => $"{Minimum},{Maximum}";

    /// <inheritdoc/>
    public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(right);
}
