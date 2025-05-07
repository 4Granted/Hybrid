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
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Services;
using SampleFramework;
using Sandbox.Galaxy;

namespace Sandbox
{
    internal sealed class SandboxGame : Sample
    {
        private RenderContext renderContext = default!;
        //private RenderGraph renderGraph = default!;

        protected override void OnInitialize()
        {
            Window.Title = "Ephemeris";

            var effectCache = new EffectCache(GraphicsDevice);

            Services.AddSingleton(effectCache);

            renderContext = new RenderContext(GraphicsDevice, SwapChain);
            //renderGraph = new RenderGraph(GraphicsDevice);

            Services.AddSingleton(renderContext);

            Services.AddSingleton(new DebugRenderer(GraphicsDevice));

            Services.AddSingleton<RenderSettings>();
            Services.AddSingleton<SkyboxSettings>();
            Services.AddSingleton<GenerationSettings>();
            Services.AddSingleton<SimulationSettings>();

            PushLayer<DebugLayer>();
            PushLayer<WorldLayer>();
        }

        /*private static readonly ResourceName TestResource = "";

        private struct TestData
        {
        }*/

        protected override void OnRender(GameTime time)
        {
            /*renderGraph.AddRenderPass((RenderPassBuilder builder, ref TestData data) =>
            {
                builder.WriteTexture(TestResource);
            }, (RenderPassContext context, in TestData data) =>
            {

            });

            renderGraph.Execute();*/

            renderContext.Execute(time);
        }

        protected override void OnResize(Area size)
        {
            renderContext.Resize(size);
        }
    }
}
