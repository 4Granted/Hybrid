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
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Graphics.Textures;
using Hybrid.Input;
using Hybrid.Numerics;
using Hybrid.Physics;
using Hybrid.Rendering;
using Hybrid.Rendering.Effects;
using Hybrid.Services;
using Sandbox.Galaxy;
using Silk.NET.Input;
using System.Runtime.InteropServices;

namespace Sandbox
{
    internal sealed class WorldLayer : Layer
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct CompositeData
        {
            public float GammaInverse;
        }

        public static readonly ConstantsParameter<CompositeData> CompositeParameter
            = EffectParameter.CreateConstants<CompositeData>("PerFrame", ShaderStage.Pixel);

        public static readonly EffectParameter<Texture2D> SourceParameter
            = EffectParameter.CreateTexture("Source", ShaderStage.Pixel);

        private Area Size => renderContext.ScreenSize;

        private readonly InputContext localInput = new();

        private GraphicsDevice graphicsDevice = default!;
        private RenderContext renderContext = default!;
        private InputContext globalInput = default!;
        private RenderSettings renderSettings = default!;
        private GalaxyContext galaxyContext = default!;
        private DebugRenderer debugRenderer = default!;
        //private SandboxCamera camera = default!;
        private Effect outputEffect = default!;
        private EffectInstance outputEffectInstance = default!;
        private ConstantsBuffer<SpatialEffect.PerView> perViewConstants = default!;
        private ConstantsBuffer<SpatialEffect.PerDraw> perDrawConstants = default!;
        private ConstantsBuffer<CompositeData> compositeConstants = default!;
        private SkyboxRenderer skyboxRenderer = default!;
        private GalaxyRenderer galaxyRenderer = default!;

        protected override void OnAttach(IServiceScope services)
        {
            graphicsDevice = services.GetRequiredService<GraphicsDevice>();
            renderContext = services.GetRequiredService<RenderContext>();
            debugRenderer = services.GetRequiredService<DebugRenderer>();
            globalInput = services.GetRequiredService<InputContext>();
            renderSettings = services.GetRequiredService<RenderSettings>();

            galaxyContext = new GalaxyContext(services);

            services.AddSingleton(galaxyContext);

            perViewConstants = new ConstantsBuffer<SpatialEffect.PerView>(graphicsDevice);
            perDrawConstants = new ConstantsBuffer<SpatialEffect.PerDraw>(graphicsDevice);
            compositeConstants = new ConstantsBuffer<CompositeData>(graphicsDevice);

            var source = File.ReadAllText("Output.hlsl");

            outputEffect = new Effect(graphicsDevice,
                new VertexShader(graphicsDevice, source),
                new PixelShader(graphicsDevice, source));

            outputEffect.AddParameter(CompositeParameter);
            outputEffect.AddParameter(SourceParameter);

            outputEffectInstance = new EffectInstance(outputEffect)
            {
                RasterizerState = RasterizerState.Default with
                {
                    WindingMode = WindingMode.CounterClockwise,
                },
                BlendState = BlendState.Disabled,
            };

            var galaxySettings = new SimulationSettings();

            skyboxRenderer = new SkyboxRenderer(graphicsDevice, services);
            galaxyRenderer = new GalaxyRenderer(graphicsDevice, services);

            renderContext.Compositor.AddLayer(OnRender);
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            localInput.ClearState();

            foreach (var e in globalInput.Events)
            {
                localInput.AddEvent(e);
            }

            renderContext.Camera.Update(gameTime, localInput);

            galaxyContext.Update(localInput);
        }

        private void OnRender(CommandList commandList)
        {
            var renderTarget = renderContext.Compositor.GetRenderTarget(
                format: TextureFormat.Rgba16Float,
                samples: renderSettings.Samples);
            var copyTarget = renderContext.Compositor.GetRenderTarget(
                format: TextureFormat.Rgba16Float,
                samples: TextureSamples.X1);
            var depthStencil = renderContext.Compositor.GetDepthStencil(
                format: TextureFormat.D16UNorm,
                samples: renderSettings.Samples);

            commandList.ClearRenderTarget(renderTarget, in renderSettings.Background);
            commandList.ClearDepthStencil(depthStencil, 1f);

            commandList.SetRenderTarget(renderTarget, depthStencil);

            UpdateConstants(commandList);

            // Skybox
            skyboxRenderer.Draw(commandList, perViewConstants);

            // Render
            {
                galaxyRenderer.Draw(commandList, perViewConstants, perDrawConstants);

                commandList.CopyTexture(renderTarget, copyTarget);
            }

            // Debug
            {
                commandList.SetRenderTarget(copyTarget, depthStencil);

                debugRenderer.Flush(commandList, perViewConstants);
            }

            // Present
            {
                var compositeData = new CompositeData
                {
                    GammaInverse = 1f / renderSettings.Gamma,
                };

                compositeConstants.Write(commandList, ref compositeData);

                commandList.SetRenderTarget(renderContext.Compositor.Surface);

                outputEffectInstance.Parameters.SetParameter(CompositeParameter, compositeConstants);
                outputEffectInstance.Parameters.SetParameter(SourceParameter, copyTarget);

                outputEffectInstance.Apply(commandList);

                commandList.Draw(3);
            }
        }

        private void UpdateConstants(CommandList commandList)
        {
            var camera = renderContext.Camera;

            var view = camera.ViewMatrix;
            var projection = camera.ProjectionMatrix;
            var viewProjection = view * projection;

            var perViewData = new SpatialEffect.PerView
            {
                View = view,
                ViewInverse = view.Inverted,
                Projection = projection,
                ProjectionInverse = projection.Inverted,
                ViewProjection = viewProjection,
                ViewProjectionInverse = viewProjection.Inverted,
                CameraPosition = camera.Position,
                Resolution = Size,
                DeltaTime = renderContext.Compositor.DeltaTime,
            };

            perViewConstants.Write(commandList, ref perViewData);

            var perDrawData = new SpatialEffect.PerDraw
            {
                World = Matrix4.Identity,
                WorldInverse = Matrix4.Identity,
            };

            perDrawConstants.Write(commandList, ref perDrawData);
        }
    }
}
