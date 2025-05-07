// Hybrid - A versatile framework for application development.
// Copyright (C) 2024  Fielding Baran
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY- without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Numerics;
using System.Runtime.InteropServices;

namespace Sandbox
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct VertexPosition(Vector3 position) : IVertex
    {
        private static readonly VertexGroup Group = new(VertexElement.POSITION3);

        public readonly Vector3 Position = position;

        public readonly VertexGroup GetGroup() => Group;

        public static implicit operator VertexPosition(Vector3 position) => new(position);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct VertexPositionColor(Vector3 position, Color color) : IVertex
    {
        private static readonly VertexGroup Group = new(
            VertexElement.POSITION3, VertexElement.COLOR);

        public readonly Vector3 Position = position;
        public readonly Color Color = color;

        public readonly VertexGroup GetGroup() => Group;
    }
}
