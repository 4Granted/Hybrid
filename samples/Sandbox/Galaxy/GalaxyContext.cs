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

using Hybrid.Input;
using Hybrid.Numerics;
using Hybrid.Physics;
using Hybrid.Services;

namespace Sandbox.Galaxy
{
    internal sealed class GalaxyContext
    {
        /// <summary>
        /// Gets the octree.
        /// </summary>
        public Octree<StarSystem> Octree => octree;

        /// <summary>
        /// Gets the mouse world position.
        /// </summary>
        public Vector3? MousePosition { get; private set; }

        /// <summary>
        /// Gets the selected star system.
        /// </summary>
        public StarSystem? SelectedSystem { get; private set; }

        private readonly RenderContext renderContext;
        private readonly Octree<StarSystem> octree;

        internal GalaxyContext(IServiceScope services)
        {
            renderContext = services.GetRequiredService<RenderContext>();

            octree = new Octree<StarSystem>(Vector3.Zero);
        }

        public void Update(InputContext input)
        {
            var ray = renderContext.Camera.RayCast(input.MousePosition);

            if (input.IsDown(MouseButton.Left))
            {
                SelectedSystem = octree.RayCast(in ray, out var result) ? result.Hit : null;
            }
        }
    }
}
