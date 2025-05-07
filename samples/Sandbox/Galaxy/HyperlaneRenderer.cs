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

using Hybrid;
using Hybrid.Collections;
using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Services;

namespace Sandbox.Galaxy
{
    internal sealed class HyperlaneRenderer : DeviceResource
    {
        private readonly VertexBuffer<VertexPositionColor> vertexBuffer;
        private readonly IndexBuffer<ushort> indexBuffer;
        private readonly EffectInstance effectInstance;
        private readonly DynamicHeap<VertexPositionColor> vertexHeap;
        private readonly DynamicHeap<ushort> indexHeap;
        private bool isDirty;

        internal HyperlaneRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            const int InitialSize = 64;

            vertexBuffer = new VertexBuffer<VertexPositionColor>(GraphicsDevice, InitialSize);
            vertexHeap = new DynamicHeap<VertexPositionColor>(InitialSize);

            indexBuffer = new IndexBuffer<ushort>(graphicsDevice, InitialSize);
            indexHeap = new DynamicHeap<ushort>(InitialSize);

            var source = File.ReadAllText("Hyperlane.hlsl");

            var effect = new Effect(GraphicsDevice,
                new VertexShader(GraphicsDevice, source),
                new PixelShader(GraphicsDevice, source));

            effect.AddParameter(SpatialEffect.PerViewParameter);
            effect.AddParameter(SpatialEffect.PerDrawParameter);

            effectInstance = new EffectInstance(effect)
            {
                RasterizerState = RasterizerState.DefaultDepth,
                BlendState = BlendState.Disabled,
                DepthStencilState = DepthStencilState.Default,
                Topology = PrimitiveTopology.LineList,
            };
        }

        public void Draw(CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView)
        {
            if (isDirty)
            {
                isDirty = false;

                if (vertexHeap.Count > vertexBuffer.Capacity)
                    vertexBuffer.Resize(vertexHeap.Count);

                if (vertexHeap.Count > 0)
                    vertexBuffer.Write(commandList, vertexHeap.Span);

                if (indexHeap.Count > indexBuffer.Capacity)
                    indexBuffer.Resize(indexHeap.Count);

                if (indexHeap.Count > 0)
                    indexBuffer.Write(commandList, indexHeap.Span);
            }

            if (indexHeap.Count <= 0)
                return;

            effectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);

            commandList.SetVertexBuffer(vertexBuffer);
            commandList.SetIndexBuffer(indexBuffer);

            effectInstance.Apply(commandList);

            commandList.DrawIndexed(indexHeap.Count);
        }

        public void Generate(IReadOnlyList<Vector3> positions)
        {
            vertexHeap.Clear();

            var count = positions.Count;
            var points = new List<Vector2>();

            for (int i = 0; i < count; i++)
            {
                var position = positions[i];

                vertexHeap.Allocate() = new VertexPositionColor(position, Color.White);

                points.Add(position.Xz);
            }

            indexHeap.Clear();

            var hyperlanes = GenerateHyperlane(points);
            var sorted = MST.Kruskal(hyperlanes, points.Count);

            count = sorted.Count;

            for (int i = 0; i < count; i++)
            {
                var edge = sorted[i];

                indexHeap.Allocate() = (ushort)edge.Source;
                indexHeap.Allocate() = (ushort)edge.Destination;
            }

            isDirty = true;
        }

        private static List<Hyperlane> GenerateHyperlane(List<Vector2> points)
        {
            var hyperlanes = new List<Hyperlane>();

            var n = points.Count;

            // Create all pairwise edges
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var dx = points[i].X - points[j].X;
                    var dy = points[i].Y - points[j].Y;

                    var distance = MathF.Sqrt(dx * dx + dy * dy);

                    // Add edge i -> j (and j -> i in an undirected graph, 
                    // but typically one entry is enough for undirected MST)
                    hyperlanes.Add(new Hyperlane(i, j, distance));
                }
            }

            return hyperlanes;
        }
    }
}
