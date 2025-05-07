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

namespace Sandbox
{
    internal sealed class DebugRenderer : DeviceResource
    {
        private readonly VertexBuffer<VertexPositionColor> vertexBuffer;
        private readonly IndexBuffer<ushort> indexBuffer;
        private readonly DynamicHeap<VertexPositionColor> vertexHeap;
        private readonly DynamicHeap<ushort> indexHeap;
        private readonly EffectInstance effectInstance;

        internal DebugRenderer(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            const int InitialSize = 64;

            vertexBuffer = new VertexBuffer<VertexPositionColor>(graphicsDevice, InitialSize);
            vertexHeap = new DynamicHeap<VertexPositionColor>(InitialSize);

            indexBuffer = new IndexBuffer<ushort>(graphicsDevice, InitialSize);
            indexHeap = new DynamicHeap<ushort>(InitialSize);

            var source = File.ReadAllText("Debug.hlsl");

            var effect = new Effect(GraphicsDevice,
                new VertexShader(GraphicsDevice, source),
                new PixelShader(GraphicsDevice, source));

            effect.AddParameter(SpatialEffect.PerViewParameter);
            effect.AddParameter(SpatialEffect.PerDrawParameter);

            effectInstance = new EffectInstance(effect)
            {
                RasterizerState = RasterizerState.DefaultDepth,
                BlendState = BlendState.Disabled,
                DepthStencilState = DepthStencilState.DepthReadWrite,
                Topology = PrimitiveTopology.LineList,
            };
        }

        public void Flush(CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView)
        {
            if (vertexHeap.Count <= 0 || indexHeap.Count <= 0)
                return;

            if (vertexHeap.Count > vertexBuffer.Capacity)
                vertexBuffer.Resize(vertexHeap.Count);

            vertexBuffer.Write(commandList, vertexHeap.Span);

            if (indexHeap.Count > indexBuffer.Capacity)
                indexBuffer.Resize(indexHeap.Count);

            indexBuffer.Write(commandList, indexHeap.Span);

            effectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);

            commandList.SetVertexBuffer(vertexBuffer);
            commandList.SetIndexBuffer(indexBuffer);

            effectInstance.Apply(commandList);

            commandList.DrawIndexed(indexHeap.Count);
            //commandList.Draw(vertexHeap.Count);

            vertexHeap.Clear();
            indexHeap.Clear();
        }

        public void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            vertexHeap.Allocate() = new VertexPositionColor(start, color);
            vertexHeap.Allocate() = new VertexPositionColor(end, color);

            var count = indexHeap.Count;

            indexHeap.Allocate() = (ushort)count;
            indexHeap.Allocate() = (ushort)(count + 1);
        }

        public void DrawLine(Vector3 start, Vector3 end)
            => DrawLine(start, end, Color.White);

        public void DrawQuad(Vector3 minimum, Vector3 maximum, Color color)
        {
            var xMin = minimum.X;
            var yMin = minimum.Y;
            var zMin = minimum.Z;

            var xMax = maximum.X;
            var yMax = maximum.Y;
            var zMax = maximum.Z;

            var a = new Vector3(xMin, yMin, zMin);
            var b = new Vector3(xMax, yMin, zMin);
            var c = new Vector3(xMax, yMin, zMax);
            var d = new Vector3(xMin, yMin, zMax);

            DrawLine(a, b, color);
            DrawLine(b, c, color);
            DrawLine(c, d, color);
            DrawLine(d, a, color);
        }

        public void DrawQuad(Vector3 minimum, Vector3 maximum)
            => DrawQuad(minimum, maximum, Color.White);

        public void DrawFlatQuad(Vector3 minimum, Vector3 maximum, Color color)
        {
            var xMin = minimum.X;
            var yMin = minimum.Y;
            var zMin = minimum.Z;

            var xMax = maximum.X;
            var yMax = maximum.Y;

            var a = new Vector3(xMin, yMin, zMin);
            var b = new Vector3(xMax, yMin, zMin);
            var c = new Vector3(xMax, yMax, zMin);
            var d = new Vector3(xMin, yMax, zMin);

            DrawLine(a, b, color);
            DrawLine(b, c, color);
            DrawLine(c, d, color);
            DrawLine(d, a, color);
        }

        public void DrawBox(Box box, Color color)
        {
            var min = box.Minimum;
            var max = box.Maximum;

            var p1 = new Vector3(max.X, min.Y, min.Z);
            var p2 = new Vector3(min.X, min.Y, max.Z);
            var p3 = new Vector3(max.X, min.Y, max.Z);

            var p4 = new Vector3(min.X, max.Y, min.Z);
            var p5 = new Vector3(min.X, max.Y, max.Z);
            var p6 = new Vector3(max.X, max.Y, min.Z);

            DrawLine(min, p1, color);
            DrawLine(min, p2, color);
            DrawLine(min, p4, color);

            DrawLine(p6, max, color);
            DrawLine(p5, max, color);
            DrawLine(p3, max, color);

            DrawLine(p4, p6, color);
            DrawLine(p4, p5, color);

            DrawLine(p1, p3, color);
            DrawLine(p2, p3, color);

            DrawLine(p1, p6, color);
            DrawLine(p2, p5, color);
        }

        public void DrawRay(Ray ray, Color color, float length = 1f)
        {
            var origin = ray.Origin;
            var direction = (origin + ray.Direction) * length;

            DrawLine(origin, direction, color);
        }
    }
}
