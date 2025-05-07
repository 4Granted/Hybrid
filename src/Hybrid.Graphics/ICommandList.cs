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

using Hybrid.Graphics.Textures;
using Hybrid.Numerics;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a command list contract.
/// </summary>
public interface ICommandList
{
    /// <summary>
    /// Gets the command queue of the command list.
    /// </summary>
    public CommandQueue CommandQueue { get; }

    /// <summary>
    /// Gets the viewports of the command list.
    /// </summary>
    public IReadOnlyList<Viewport> Viewports { get; }

    /// <summary>
    /// Gets the viewport of the command list.
    /// </summary>
    public Viewport Viewport { get; }

    /// <summary>
    /// Gets the scissors of the command list.
    /// </summary>
    public IReadOnlyList<Rectangle> Scissors { get; }

    /// <summary>
    /// Gets the type of the command list.
    /// </summary>
    public CommandListType Type { get; }

    /// <summary>
    /// Sets the render target of the command list.
    /// </summary>
    /// <param name="renderTarget">The render target.</param>
    /// <param name="depthStencil">The depth stencil.</param>
    public void SetRenderTarget(IRenderTarget? renderTarget, DepthStencil? depthStencil);

    /// <summary>
    /// Sets the render target of the command list.
    /// </summary>
    /// <param name="renderTarget">The render target.</param>
    public void SetRenderTarget(IRenderTarget? renderTarget);

    /// <summary>
    /// Sets the pipeline of the command list.
    /// </summary>
    /// <param name="pipeline">The pipeline.</param>
    public void SetPipeline(GraphicsPipeline pipeline);

    /// <summary>
    /// Sets the compute pipeline of the command list.
    /// </summary>
    /// <param name="pipeline">The compute pipeline.</param>
    public void SetPipeline(ComputePipeline pipeline);

    /// <summary>
    /// Sets the descriptor set of the command list.
    /// </summary>
    /// <param name="index">The index of the descriptor to bind.</param>
    /// <param name="descriptors">The descriptor set to bind.</param>
    public void SetDescriptorSet(int index, IDescriptorSet descriptors);

    /// <summary>
    /// Sets the vertex buffer of the command list.
    /// </summary>
    /// <param name="view">The view of the vertex buffer.</param>
    public void SetVertexBuffer(VertexBufferView view);

    /// <summary>
    /// Sets the index buffer of the command list.
    /// </summary>
    /// <param name="view">The view of the index buffer.</param>
    public void SetIndexBuffer(IndexBufferView view);

    /// <summary>
    /// Sets the viewport of the command list.
    /// </summary>
    /// <param name="viewport">The viewport to bind to the context.</param>
    /// <param name="index">The index of the viewport to set.</param>
    public void SetViewport(Viewport viewport, int index = 0);

    /// <summary>
    /// Sets the scissor rectangle of the command list.
    /// </summary>
    /// <param name="bounds">The scissor bounds.</param>
    /// <param name="index">The scissor index.</param>
    public void SetScissor(Rectangle bounds, int index = 0);

    /// <summary>
    /// Clears the <paramref name="renderTarget"/> to
    /// the provided <paramref name="color"/> value.
    /// </summary>
    /// <param name="renderTarget">The render target.</param>
    /// <param name="color">The color value.</param>
    public void ClearRenderTarget(IRenderTarget renderTarget, ref readonly Color color);

    /// <summary>
    /// Clears the <paramref name="depthStencil"/>
    /// with the provided <paramref name="depth"/>
    /// and <paramref name="stencil"/> values.
    /// </summary>
    /// <param name="depthStencil">The depth stencil.</param>
    /// <param name="depth">The depth value.</param>
    /// <param name="stencil">The stencil value.</param>
    public void ClearDepthStencil(DepthStencil depthStencil, float depth, byte stencil);

    /// <summary>
    /// Clears the <paramref name="depthStencil"/> to
    /// the provided <paramref name="depth"/> value.
    /// </summary>
    /// <param name="depthStencil">The depth stencil.</param>
    /// <param name="depth">The depth value.</param>
    public void ClearDepthStencil(DepthStencil depthStencil, float depth);

    /// <summary>
    /// Draws geometry to the bound render target.
    /// </summary>
    /// <param name="vertexCount">The amount of vertices.</param>
    /// <param name="instanceCount">The amount of instances.</param>
    /// <param name="vertexStart">The vertex to start at.</param>
    /// <param name="instanceStart">The instance to start at.</param>
    public void Draw(int vertexCount, int instanceCount = 1, int vertexStart = 0, int instanceStart = 0);

    /// <summary>
    /// Draws indexed geometry to the bound render target.
    /// </summary>
    /// <param name="indexCount">The amount of indices.</param>
    /// <param name="instanceCount">The amount of instances.</param>
    /// <param name="indexStart">The index to start at.</param>
    /// <param name="baseVertex">The vertex to start at.</param>
    /// <param name="instanceStart">The instance to start at.</param>
    public void DrawIndexed(int indexCount, int instanceCount = 1, int indexStart = 0, int baseVertex = 0, int instanceStart = 0);

    /// <summary>
    /// Copies the <paramref name="source"/> texture to
    /// the <paramref name="destination"/> texture.
    /// </summary>
    /// <param name="source">The source texture.</param>
    /// <param name="destination">The destination texture.</param>
    public void CopyTexture(Texture source, Texture destination);
}
