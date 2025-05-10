// Hybrid - A versatile framework for application development.
// Copyright (C) 2024  Fielding Baran
//
// Licensed under the Apache License, Version 2.0 (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using SampleFramework;
using System.Runtime.InteropServices;

namespace TriangleSample;

/// <summary>
/// This sample demonstrates how to draw a triangle using Hybrid.
/// </summary>
internal sealed class TriangleSample : Sample
{
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Vertex(Vector3 position, Color color) : IVertex
    {
        // Used to automatically set vertex groups in the graphics pipeline
        public static VertexLayout Layout => layout;

        private static readonly VertexLayout layout = new(
            VertexElement.POSITION3, VertexElement.COLOR);

        // The position of the vertex
        public readonly Vector3 Position = position;

        // The color of the vertex
        public readonly Color Color = color;
    }

    private GraphicsPipeline pipeline = default!;
    private VertexBuffer<Vertex> vertexBuffer = default!;

    protected override void OnInitialize()
    {
        var source = File.ReadAllText("Simple.hlsl");

        // Create a vertex shader
        var vertexShader = new VertexShader(GraphicsDevice, source);

        // Create a pixel shader
        var pixelShader = new PixelShader(GraphicsDevice, source);

        // Create a graphics pipeline
        pipeline = new GraphicsPipeline(GraphicsDevice,
            vertexShader: vertexShader, pixelShader: pixelShader);

        // Creates an array of the vertices
        Span<Vertex> vertices =
        [
            new(new Vector3(-0.5f, -0.5f, 0.0f), new Color(1f, 0f, 0f)),
            new(new Vector3( 0.5f, -0.5f, 0.0f), new Color(0f, 1f, 0f)),
            new(new Vector3( 0.0f,  0.5f, 0.0f), new Color(0f, 0f, 1f)),
        ];

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

        // Set the vertex buffer of the command list
        commandList.SetVertexBuffer(vertexBuffer);

        // Set the graphics pipeline of the command list
        commandList.SetPipeline(pipeline);

        // Draw the vertices
        commandList.Draw(3);

        // Submit the command list to the GPU
        commandQueue.Execute(commandList);
    }
}
