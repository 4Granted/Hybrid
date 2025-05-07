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

using Hybrid.Graphics.Native;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using System.Runtime.InteropServices;

namespace Sandbox
{
    internal static class SpatialEffect
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PerView
        {
            public Matrix4 View;
            public Matrix4 ViewInverse;
            public Matrix4 Projection;
            public Matrix4 ProjectionInverse;
            public Matrix4 ViewProjection;
            public Matrix4 ViewProjectionInverse;
            public Vector3 CameraPosition;
            public Area Resolution;
            public float DeltaTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PerDraw
        {
            public Matrix4 World;
            public Matrix4 WorldInverse;
        }

        public static readonly ConstantsParameter<PerView> PerViewParameter
            = EffectParameter.CreateConstants<PerView>("PerView", ShaderStage.All);

        public static readonly ConstantsParameter<PerDraw> PerDrawParameter
            = EffectParameter.CreateConstants<PerDraw>("PerDraw", ShaderStage.All);

        public static readonly StructuredParameter<PerDraw> PerInstanceParameter
            = EffectParameter.CreateStructured<PerDraw>("PerDraw", ShaderStage.Vertex);
    }
}
