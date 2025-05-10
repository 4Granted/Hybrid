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

namespace RippleSample;

/// <summary>
/// This sample demonstrates how to create a ripple effect shader in Hybrid.
/// </summary>
internal sealed class RippleSample : Sample
{
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Vertex(Vector3 position, Vector2 texCoord) : IVertex
    {
        // Used to automatically set vertex groups in the graphics pipeline
        public static VertexLayout Layout => layout;

        private static readonly VertexLayout layout = new(
            VertexElement.POSITION3, VertexElement.TEXCOORD);

        // The position of the vertex
        public readonly Vector3 Position = position;

        // The texture coordinate of the vertex
        public readonly Vector2 TexCoord = texCoord;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Constants
    {
        public int Width;
        public int Height;
        public float Time;
        public float Frequency;
        public float Amplitude;
        public float Speed;
    }

    private GraphicsPipeline? graphicsPipeline;
    private VertexBuffer<Vertex> vertexBuffer = default!;
    private IndexBuffer<ushort> indexBuffer = default!;
    private ConstantsBuffer<Constants> constantsBuffer = default!;
    private DescriptorSet descriptorSet = default!;
    private float totalTime;

    protected override void OnInitialize()
    {
        var source = File.ReadAllText("Ripple.hlsl");

        // Create a vertex shader
        var vertexShader = new VertexShader(GraphicsDevice, source);

        // Create a pixel shader
        var pixelShader = new PixelShader(GraphicsDevice, source);

        // Create a descriptor layout with the constants buffer slot
        var descriptorLayout = DescriptorLayoutBuilder.Create()
            .AddDescriptor(DescriptorType.ConstantResource, ShaderStage.Pixel)
            .Build(GraphicsDevice);

        // Create a graphics pipeline
        graphicsPipeline = new GraphicsPipeline(GraphicsDevice,
            vertexShader: vertexShader, pixelShader: pixelShader,
            descriptorLayouts: [descriptorLayout]);

        // Creates an array of the vertices
        var vertices = new Vertex[4]
        {
            new(new Vector3(-1.0f,  1.0f, 0.0f), Vector2.Zero),
            new(new Vector3( 1.0f,  1.0f, 0.0f), Vector2.UnitX),
            new(new Vector3(-1.0f, -1.0f, 0.0f), Vector2.UnitY),
            new(new Vector3( 1.0f, -1.0f, 0.0f), Vector2.One),
        };

        // Create a vertex buffer with the initial data
        vertexBuffer = new VertexBuffer<Vertex>(GraphicsDevice, vertices);

        // Creates an array of the indices
        var indices = new ushort[6]
        {
            0, 1, 2,
            2, 1, 3,
        };

        // Create an index buffer with the initial data
        indexBuffer = new IndexBuffer<ushort>(GraphicsDevice, indices);

        // Create the constants buffer
        constantsBuffer = new ConstantsBuffer<Constants>(GraphicsDevice);

        // Create the descriptor set that will bind the constants buffer
        descriptorSet = new DescriptorSet(GraphicsDevice, descriptorLayout);

        // Bind the constants buffer to the descriptor set slot
        descriptorSet.SetConstants(0, constantsBuffer);
    }

    protected override void OnRender(GameTime time)
    {
        // Get the graphics command queue
        var commandQueue = GraphicsDevice.GraphicsQueue;

        // Allocate a command list via the queue
        var commandList = commandQueue.Allocate();

        // Add the delta time to the totla time
        totalTime += time.DeltaTime;

        // Create the constants data
        var constants = new Constants
        {
            Width = SwapChain.Width,
            Height = SwapChain.Height,
            Time = totalTime,
            Frequency = 20.0f,
            Amplitude = 0.08f,
            Speed = 5.0f
        };

        // Write the constants data to the buffer
        constantsBuffer.Write(commandList, ref constants);

        // Fill the swapchain buffer with a provided color
        commandList.ClearRenderTarget(SwapChain, in Color.Black);

        // Set the render target of the command list to the window swapchain
        commandList.SetRenderTarget(SwapChain);

        // We set the vertex buffer before the pipeline
        //  in order to take advantage of automatically
        //  extracting the vertex layout - however, this
        //  is only possible when using a typed buffer.
        commandList.SetVertexBuffer(vertexBuffer);

        // Set the index buffer of the command list
        commandList.SetIndexBuffer(indexBuffer);

        // Set the descriptor set of the command list
        commandList.SetDescriptorSet(0, descriptorSet);

        // Set the graphics pipeline of the command list
        commandList.SetPipeline(graphicsPipeline);

        // Draw the indexed vertices
        commandList.DrawIndexed(6);

        // Submit the command list to the GPU
        commandQueue.Execute(commandList);
    }
}
