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

using Hybrid.Framework;
using Hybrid.Input;
using Hybrid.Numerics;
using Hybrid.Rendering;

namespace Sandbox
{
    internal sealed class SandboxCamera : Camera
    {
        /// <inheritdoc/>
        public override Matrix4 ViewMatrix
        {
            get
            {
                CalculateVectors();

                var target = focus + offset;

                return Matrix4.CreateLookAt(in eye, in target, in Vector3.UnitY);
            }
        }

        /// <inheritdoc/>
        public override Matrix4 ProjectionMatrix => Matrix4.CreatePerspectiveFieldOfView(fieldOfView, AspectRatio, 0.01f, 100f);

        /// <summary>
        /// Gets or sets the position bounds of the camera.
        /// </summary>
        public Circle PositionBounds { get; set; }

        /// <inheritdoc/>
        public override Vector3 Position
        {
            get => offset;
            set => offset = value;
        }

        /// <summary>
        /// Gets or sets the eye of the camera.
        /// </summary>
        public Vector3 Eye
        {
            get => eye;
            set => eye = value;
        }

        /// <summary>
        /// Gets the focus of the camera.
        /// </summary>
        public Vector3 Focus => focus;

        /// <summary>
        /// Gets the offset of the camera.
        /// </summary>
        public Vector3 Offset => offset;

        /// <summary>
        /// Gets the front-facing vector of the camera.
        /// </summary>
        public Vector3 Front => front;

        /// <summary>
        /// Gets the up-facing vector of the camera.
        /// </summary>
        public Vector3 Up => up;

        /// <summary>
        /// Gets the right-facing vector of the camera.
        /// </summary>
        public Vector3 Right => right;

        /// <summary>
        /// Gets or sets the aspect ratio of the camera.
        /// </summary>
        public float AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the field of view of the camera.
        /// </summary>
        public float FieldOfView
        {
            get => Numeric.RadiansToDegrees(fieldOfView);
            set
            {
                var angle = Numeric.Clamp(value, 1f, 90f);

                fieldOfView = Numeric.DegreesToRadians(angle);
            }
        }

        /// <summary>
        /// Gets or sets the pitch of the camera.
        /// </summary>
        public float Pitch
        {
            get => Numeric.RadiansToDegrees(pitch);
            set
            {
                //var angle = Numeric.Clamp(value, -89f, 89f);
                var angle = Numeric.Clamp(value, 10f, 80f);

                pitch = Numeric.DegreesToRadians(angle);
            }
        }

        /// <summary>
        /// Gets or sets the yaw of the camera.
        /// </summary>
        public float Yaw
        {
            get => Numeric.RadiansToDegrees(yaw);
            set => yaw = Numeric.DegreesToRadians(value);
        }

        /// <summary>
        /// Gets or sets the zoom of the camera.
        /// </summary>
        public float Zoom
        {
            get => currentZoom;
            set => currentZoom = Numeric.Clamp(value, 1f, 50f);
        }

        private Vector3 targetPosition = Vector3.Zero;
        private Vector3 eye = Vector3.Zero;
        private Vector3 focus = Vector3.Zero;
        private Vector3 offset = Vector3.Zero;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private Vector2 lastPosition = Vector2.Zero;
        private float fieldOfView = Numeric.PiOver2;
        private float pitch;
        private float yaw = -Numeric.PiOver2;
        private float currentZoom = 5f;
        private float targetZoom = 5f;

        internal SandboxCamera(float aspectRatio)
            => AspectRatio = aspectRatio;

        public void Update(GameTime time, InputContext input)
        {
            const float panningFactor = 5f;
            const float panningSpeed = 5f;
            const float rotationSpeed = 0.2f;
            const float zoomFactor = 0.5f;
            const float zoomSpeed = 3f;

            var deltaTime = time.DeltaTime;

            // Rotate
            var mousePosition = input.MousePosition;
            var mouseDelta = lastPosition - mousePosition;

            if (input.IsDown(MouseButton.Right))
            {
                Yaw += mouseDelta.X * rotationSpeed;
                Pitch -= mouseDelta.Y * rotationSpeed;
            }

            lastPosition = mousePosition;

            // Zoom
            var scroll = input.MouseScroll;

            if (MathF.Abs(scroll) > 0.01f)
            {
                var amount = scroll * zoomFactor;

                targetZoom = Numeric.Clamp(targetZoom - amount, 1f, 50f);
            }

            Zoom = float.Lerp(currentZoom, targetZoom, deltaTime * zoomSpeed);

            // Movement
            if (input.IsDown(KeyCode.W))
            {
                //targetPosition += Front * panningFactor * deltaTime;

                EnsureBounds(Front * panningFactor * deltaTime);
            }

            if (input.IsDown(KeyCode.S))
            {
                //targetPosition -= Front * panningFactor * deltaTime;

                EnsureBounds(-(Front * panningFactor * deltaTime));
            }

            if (input.IsDown(KeyCode.A))
            {
                //targetPosition -= Right * panningFactor * deltaTime;

                EnsureBounds(-(Right * panningFactor * deltaTime));
            }

            if (input.IsDown(KeyCode.D))
            {
                //targetPosition += Right * panningFactor * deltaTime;

                EnsureBounds(Right * panningFactor * deltaTime);
            }

            Position = Vector3.Lerp(in offset, in targetPosition, deltaTime * panningSpeed);
        }

        private void EnsureBounds(Vector3 velocity)
        {
            targetPosition += velocity;

            var x = targetPosition.X;
            var z = targetPosition.Z;

            // circle center and radius
            var cx = PositionBounds.Center.X;
            var cy = PositionBounds.Center.Y;
            var r = PositionBounds.Radius;

            // compute offset and distance
            var dx = x - cx;
            var dy = z - cy;
            var dist = MathF.Sqrt(dx * dx + dy * dy);

            // clamp if outside the circle
            if (dist > r)
            {
                // normalize the direction
                var nx = dx / dist;
                var ny = dy / dist;

                // move camera back onto the circle boundary
                x = cx + nx * r;
                z = cy + ny * r;
            }

            // update camera position
            targetPosition.X = x;
            targetPosition.Z = z;
        }

        private void CalculateVectors()
        {
            var cs = MathF.Cos(pitch) * MathF.Sin(yaw);
            var cc = MathF.Cos(pitch) * MathF.Cos(yaw);

            eye.X = focus.X + currentZoom * cs;
            eye.Y = focus.Y + currentZoom * MathF.Sin(pitch);
            eye.Z = focus.Z + currentZoom * cc;

            eye += offset;

            front.X = -cs;
            front.Z = -cc;

            front.Normalize();

            right = Vector3.Normalize(Vector3.Cross(in front, in Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(in right, in front));
        }
    }
}
