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
using Hybrid.Services;

namespace Sandbox.Galaxy
{
    internal sealed class GalaxyRenderer : DeviceResource
    {
        private readonly StarRenderer starRenderer;
        private readonly SimulationRenderer simulationRenderer;

        internal GalaxyRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            starRenderer = new StarRenderer(graphicsDevice, services);
            simulationRenderer = new SimulationRenderer(graphicsDevice, services);
        }

        public void Draw(CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView,
            ConstantsBuffer<SpatialEffect.PerDraw> perDraw)
        {
            // Simulation
            simulationRenderer.Draw(commandList, perView, perDraw);

            // Stars and hyperlanes
            starRenderer.Draw(commandList, perView, perDraw);
        }
    }
}
