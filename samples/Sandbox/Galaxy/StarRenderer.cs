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

using Hybrid.Collections;
using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Rendering.Geometry;
using Hybrid.Services;
using System.Runtime.InteropServices;

namespace Sandbox.Galaxy
{
    internal sealed class StarRenderer : DeviceResource
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Instance
        {
            public Matrix4 World;
            public Matrix4 WorldInverse;
            public Color Color;
        }

        private static readonly StructuredParameter<Instance> InstanceParameter
            = EffectParameter.CreateStructured<Instance>("PerInstance", ShaderStage.Vertex);

        private readonly GalaxyContext galaxyContext;
        private readonly GenerationSettings settings;
        private readonly DebugRenderer debugRenderer;
        private readonly SelectionRenderer selectionRenderer;
        private readonly HyperlaneRenderer hyperlaneRenderer;
        private readonly StructuredBuffer<Instance> instanceBuffer;
        private readonly DynamicHeap<Instance> instanceHeap;
        private readonly Mesh sphereMesh;
        private readonly EffectInstance effectInstance;
        private Vector3 origin = Vector3.Zero;

        internal StarRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            galaxyContext = services.GetRequiredService<GalaxyContext>();
            settings = services.GetRequiredService<GenerationSettings>();
            debugRenderer = services.GetRequiredService<DebugRenderer>();

            selectionRenderer = new SelectionRenderer(graphicsDevice, services);
            hyperlaneRenderer = new HyperlaneRenderer(graphicsDevice, services);

            instanceHeap = new DynamicHeap<Instance>(settings.Samples, 16);
            instanceBuffer = new StructuredBuffer<Instance>(graphicsDevice, 4);

            sphereMesh = Mesh.Create(graphicsDevice, Primitives.GenerateSphere(1f, 6));

            var source = File.ReadAllText("Star.hlsl");

            var effect = new Effect(graphicsDevice,
                new VertexShader(graphicsDevice, source),
                new PixelShader(graphicsDevice, source));

            effect.AddParameter(SpatialEffect.PerViewParameter);
            effect.AddParameter(InstanceParameter);

            effectInstance = new EffectInstance(effect)
            {
                RasterizerState = RasterizerState.DefaultDepth,
                BlendState = BlendState.Default,
                DepthStencilState = DepthStencilState.DepthReadWrite,
                Topology = PrimitiveTopology.TriangleList,
            };
        }

        public void Draw(CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView,
            ConstantsBuffer<SpatialEffect.PerDraw> perDraw)
        {
            if (settings.EnableOctree)
            {
                galaxyContext.Octree.ForeachNode(bounds
                    => debugRenderer.DrawBox(bounds, Color.Magenta));

                galaxyContext.Octree.ForeachLeaf(bounds
                    => debugRenderer.DrawBox(bounds, Color.Yellow));
            }

            // Selection
            selectionRenderer.Draw(commandList, perView, perDraw);

            // Hyperlanes
            if (settings.EnableHyperlanes)
            {
                hyperlaneRenderer.Draw(commandList, perView);
            }

            Generate();

            // Stars
            if (instanceHeap.Count > 0)
            {
                if (instanceHeap.Count > instanceBuffer.Capacity)
                    instanceBuffer.Resize(instanceHeap.Count);

                instanceBuffer.Write(commandList, instanceHeap.Span);

                effectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);
                effectInstance.Parameters.SetParameter(InstanceParameter, instanceBuffer);

                sphereMesh.Draw(commandList, effectInstance, instanceHeap.Count);
            }
        }

        private static readonly Vector3 scale = new(0.1f);

        private void Generate()
        {
            if (settings.HasChanged)
            {
                settings.HasChanged = false;

                //starSystems.Clear();
                instanceHeap.Clear();
                galaxyContext.Octree.Clear();

                var random = settings.Random;

                var innerRadius = settings.InnerRadius;
                var outerRadius = settings.OuterRadius;

                var size = outerRadius * 1.5f;

                origin = new Vector3(size / 2f, 0f, size / 2f);

                var points = size > 0f ? PoissonDisc.Sample3(size, size, 1f, 30, random)
                        .OrderBy(point => Vector3.DistanceSquared(in point, in origin))
                        .Where(point => IsValid(point, origin, innerRadius, outerRadius * 0.75f))
                        .Select(point => point - origin)
                        .Take(settings.Samples).ToList() : [];

                for (int i = 0; i < points.Count; i++)
                {
                    var position = new Vector3(points[i].X, 0f, points[i].Z);
                    var bounds = Box.FromExtent(in position, in scale);

                    galaxyContext.Octree.Insert(in bounds, new StarSystem
                    {
                        Position = position,
                    });

                    ref var instance = ref instanceHeap.Allocate();

                    instance.World = new Transform
                    {
                        Origin = Vector3.Zero,
                        Position = position,
                        Scale = new Vector3(random.Gaussian(0.05f, 0.02f)),
                        Rotation = Quaternion.Identity,
                    };

                    instance.Color = BlackBody.FromTemperature(
                        random.Uniform(4000f, 10000f));
                }

                hyperlaneRenderer.Generate(points);
            }
        }

        private static bool IsValid(
            Vector3 point, Vector3 center,
            float innerRadius, float outerRadius)
        {
            var dx = point.X - center.X;
            var dz = point.Z - center.Z;

            var distSq = dx * dx + dz * dz;

            var innerSq = innerRadius * innerRadius;
            var outerSq = outerRadius * outerRadius;

            return distSq >= innerSq && distSq <= outerSq;
        }
    }
}
