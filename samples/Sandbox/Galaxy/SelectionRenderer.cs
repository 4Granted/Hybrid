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
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Rendering.Geometry;
using Hybrid.Services;

namespace Sandbox.Galaxy
{
    internal sealed class SelectionRenderer : DeviceResource
    {
        private readonly GalaxyContext galaxyContext;
        private readonly EffectInstance effectInstance;
        private readonly Mesh discMesh;

        public SelectionRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            galaxyContext = services.GetRequiredService<GalaxyContext>();

            var source = File.ReadAllText("Selection.hlsl");

            var effect = new Effect(graphicsDevice,
                new VertexShader(graphicsDevice, source),
                new PixelShader(graphicsDevice, source));

            effect.AddParameter(SpatialEffect.PerViewParameter);
            effect.AddParameter(SpatialEffect.PerDrawParameter);

            effectInstance = new EffectInstance(effect)
            {
                RasterizerState = RasterizerState.DefaultDepth,
                BlendState = BlendState.Default,
                DepthStencilState = DepthStencilState.DepthReadWrite,
                Topology = PrimitiveTopology.TriangleList,
            };

            discMesh = Mesh.Create(graphicsDevice, Primitives.GenerateDisc(0.15f));
        }

        public void Draw(CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView,
            ConstantsBuffer<SpatialEffect.PerDraw> perDraw)
        {
            if (galaxyContext.SelectedSystem is { Position: var position })
            {
                var perDrawData = new SpatialEffect.PerDraw
                {
                    World = Matrix4.CreateTranslation(in position),
                };

                perDraw.Write(commandList, ref perDrawData);

                effectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);
                effectInstance.Parameters.SetParameter(SpatialEffect.PerDrawParameter, perDraw);

                discMesh.Draw(commandList, effectInstance);
            }
        }
    }
}
