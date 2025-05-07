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

using Hybrid.Numerics;
using Hybrid.Rendering;

namespace Sandbox
{
    internal sealed class PerspectiveCamera : Camera
    {
        public override Matrix4 ViewMatrix
        {
            get
            {
                var camera = Position;
                var focus = Position + front;

                return Matrix4.CreateLookAt(in camera, in focus, in up);
            }
        }
        public override Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(fieldOfView, AspectRatio, 0.01f, 100f);
        public Vector3 Front => front;
        public Vector3 Up => up;
        public Vector3 Right => right;
        public float AspectRatio { get; set; }
        public float FieldOfView
        {
            get => Numeric.RadiansToDegrees(fieldOfView);
            set
            {
                var angle = Numeric.Clamp(value, 1f, 90f);

                fieldOfView = Numeric.DegreesToRadians(angle);
            }
        }
        public float Pitch
        {
            get => Numeric.RadiansToDegrees(pitch);
            set
            {
                var angle = Numeric.Clamp(value, -89f, 89f);

                pitch = Numeric.DegreesToRadians(angle);

                CalculateVectors();
            }
        }
        public float Yaw
        {
            get => Numeric.RadiansToDegrees(yaw);
            set
            {
                yaw = Numeric.DegreesToRadians(value);

                CalculateVectors();
            }
        }

        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private float fieldOfView = Numeric.PiOver2;
        private float pitch;
        private float yaw = -Numeric.PiOver2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="aspectRatio"></param>
        public PerspectiveCamera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }

        private void CalculateVectors()
        {
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

            front.Normalize();

            right = Vector3.Normalize(Vector3.Cross(in front, in Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(in right, in front));
        }
    }
}
