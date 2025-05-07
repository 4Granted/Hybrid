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

namespace Sandbox.Galaxy
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct GalaxyParticle : IVertex
    {
        private static readonly VertexGroup Group = new(
            new VertexElement(VertexElementFormat.Float1, "THETAZ"),
            new VertexElement(VertexElementFormat.Float1, "THETAV"),
            new VertexElement(VertexElementFormat.Float1, "TILTANGLE"),
            new VertexElement(VertexElementFormat.Float1, "AVAL"),
            new VertexElement(VertexElementFormat.Float1, "BVAL"),
            new VertexElement(VertexElementFormat.Float1, "TEMP"),
            new VertexElement(VertexElementFormat.Float1, "MAG"),
            new VertexElement(VertexElementFormat.Int1, "TYPE"),
            VertexElement.COLOR);

        public float Theta0;
        public float VelTheta;
        public float TiltAngle;
        public float A;
        public float B;
        public float Temp;
        public float Mag;
        public int Type;
        public Color Color;

        public readonly VertexGroup GetGroup() => Group;
    }
}
