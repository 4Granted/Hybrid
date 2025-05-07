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
using Hybrid.Graphics;
using Hybrid.Graphics.Composition;
using Hybrid.Numerics;
using Hybrid.Rendering;

namespace Sandbox
{
    internal sealed class RenderContext : DeviceResource
    {
        /// <summary>
        /// Gets the compositor of the render context.
        /// </summary>
        public Compositor Compositor { get; }

        /// <summary>
        /// Gets the camera of the render context.
        /// </summary>
        public SandboxCamera Camera { get; }

        /// <summary>
        /// Gets the screen size of the render context.
        /// </summary>
        public Area ScreenSize { get; private set; }

        internal RenderContext(GraphicsDevice graphicsDevice, IRenderTarget2D outputTarget)
            : base(graphicsDevice)
        {
            Compositor = new Compositor(graphicsDevice, outputTarget);

            var size = outputTarget.Size;

            Camera = new SandboxCamera(size.Width / size.Height)
            {
                PositionBounds = new Circle
                {
                    Center = Vector2.Zero,
                    Radius = 20f,
                },
                Zoom = 25f,
            };

            ScreenSize = size;
        }

        internal void Execute(GameTime gameTime)
        {
            Compositor.Execute(gameTime.DeltaTime);
        }

        internal void Resize(Area size)
        {
            ScreenSize = size;

            Camera.AspectRatio = size.Width / size.Height;

            Compositor.Resize(size);
        }
    }
}
