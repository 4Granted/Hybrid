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
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using SampleFramework;
using System.Runtime.InteropServices;

namespace Triangle
{
    internal sealed class TriangleSample : Sample
    {
        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Vertex(Vector3 position, Color color) : IVertex
        {
            private static readonly VertexGroup Group = new(
                VertexElement.POSITION3, VertexElement.COLOR);

            // The position of the vertex
            public readonly Vector3 Position = position;

            // The color of the vertex
            public readonly Color Color = color;

            // Used to automatically set vertex groups in the graphics pipeline
            public VertexGroup GetGroup() => Group;
        }

        private GraphicsPipeline graphicsPipeline;
        private VertexBuffer<Vertex> vertexBuffer = default!;

        protected override void OnInitialize()
        {
            var source = File.ReadAllText("Simple.hlsl");

            // Create a vertex shader
            var vertexShader = new VertexShader(GraphicsDevice, source);

            // Create a pixel shader
            var pixelShader = new PixelShader(GraphicsDevice, source);

            // Create a graphics pipeline
            graphicsPipeline = GraphicsPipeline.Default with
            {
                VertexShader = vertexShader,
                PixelShader = pixelShader,
            };

            // Creates an array of the vertices
            var vertices = new Vertex[3]
            {
                new(new Vector3(-0.5f, -0.5f, 0.0f), Color.Red),
                new(new Vector3( 0.5f, -0.5f, 0.0f), Color.Green),
                new(new Vector3( 0.0f,  0.5f, 0.0f), Color.Blue),
            };

            // Create a vertex buffer with the initial data
            vertexBuffer = new VertexBuffer<Vertex>(GraphicsDevice, vertices);
        }

        protected override void OnRender(GameTime time)
        {
            // Get the graphics command queue
            var commandQueue = GraphicsDevice.GraphicsQueue;

            // Allocate a command list via the queue
            var commandList = commandQueue.Allocate();

            // Fill the swapchain buffer with a provided color
            commandList.ClearRenderTarget(SwapChain, in Color.Black);

            // Set the render target of the command list to the window swapchain
            commandList.SetRenderTarget(SwapChain);

            // We set the vertex buffer before the pipeline
            //  in order to take advantage of automatically
            //  extracting the vertex layout - however, this
            //  is only possible when using a typed buffer.
            commandList.SetVertexBuffer(vertexBuffer);

            // Set the graphics pipeline of the command list
            commandList.SetPipeline(graphicsPipeline);

            // Draw the vertices
            commandList.Draw(3);

            // Submit the command list to the GPU
            commandQueue.Execute(commandList);
        }
    }
}
