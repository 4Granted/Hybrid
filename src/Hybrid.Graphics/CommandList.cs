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

using Hybrid.Graphics.Native;
using Hybrid.Graphics.Textures;
using Hybrid.Numerics;
using System.Buffers;
using System.Diagnostics;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a command list.
/// </summary>
public sealed class CommandList : DeviceResource, ICommandList
{
    /// <inheritdoc/>
    public CommandQueue CommandQueue { get; }

    /// <summary>
    /// Gets the native command list.
    /// </summary>
    public ICommandListImpl Impl { get; }

    /// <inheritdoc/>
    public IReadOnlyList<Viewport> Viewports => viewports;

    /// <inheritdoc/>
    public Viewport Viewport => viewports[0];

    /// <inheritdoc/>
    public IReadOnlyList<Rectangle> Scissors => scissors;

    /// <inheritdoc/>
    public CommandListType Type { get; }

    private readonly Viewport[] viewports = new Viewport[8];
    private readonly Rectangle[] scissors = new Rectangle[8];

    private GraphicsPipeline? pipelineCache = null;
    private VertexLayout[] vertexLayoutCache = new VertexLayout[1];
    private IndexBufferView indexBufferCache = default;
    private int vertexBufferCount;

    internal CommandList(
        GraphicsDevice graphicsDevice,
        CommandQueue commandQueue,
        CommandListType type)
        : base(graphicsDevice)
    {
        CommandQueue = commandQueue;
        Impl = Factory.CreateCommandList();
        Type = type;
    }

    /// <inheritdoc/>
    public void SetRenderTarget(IRenderTarget? renderTarget, DepthStencil? depthStencil)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A render target and depth stencil can only be set during a graphics pass");

        var renderTargets = new Span<IRenderTarget?>(ref renderTarget);

        SetRenderTargets(renderTargets, depthStencil);
    }

    /// <inheritdoc/>
    public void SetRenderTarget(IRenderTarget? renderTarget)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A render target can only be set during a graphics pass");

        var renderTargets = new Span<IRenderTarget?>(ref renderTarget);

        SetRenderTargets(renderTargets, null);
    }

    /// <inheritdoc/>
    public void SetPipeline(GraphicsPipeline? pipeline)
    {
        if (pipeline == pipelineCache ||
            pipeline == null)
            return;

        pipelineCache = pipeline;

        pipeline.Initialize(vertexLayoutCache);

        Impl.SetPipeline(pipeline.Impl);
    }

    /// <inheritdoc/>
    public void SetPipeline(ComputePipeline pipeline)
    {
        Debug.Assert(Type == CommandListType.Compute, "A compute pipeline can only be set during a compute pass");

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void SetDescriptorSet(int index, IDescriptorSet descriptors)
    {
        Impl.SetDescriptorSet((uint)index, descriptors.Impl);
    }

    /// <inheritdoc/>
    public void SetVertexBuffer(VertexBufferView view)
    {
        Debug.Assert(view.Index >= 0, "A vertex buffer index must be equal to or greater than zero");
        Debug.Assert(view.Offset >= 0, "A vertex buffer offset must be equal to or greater than zero");

        CommonExtensions.EnsureSize(ref vertexLayoutCache, view.Index);

        var buffer = view.Buffer.Impl;

        Impl.SetVertexBuffer(buffer, (uint)view.Index, (uint)view.Offset);

        vertexLayoutCache[view.Index] = view.Layout;

        vertexBufferCount = Math.Max(view.Index + 1, vertexBufferCount);
    }

    /// <inheritdoc/>
    public void SetIndexBuffer(IndexBufferView view)
    {
        Debug.Assert(view.Offset >= 0, "An index buffer offset must be equal to or greater than zero");

        if (indexBufferCache != view)
        {
            var buffer = view.Buffer.Impl;

            indexBufferCache = view;

            Impl.SetIndexBuffer(buffer, view.Buffer.Format, (uint)view.Offset);
        }
    }

    /// <inheritdoc/>
    public void SetViewport(Viewport viewport, int index = 0)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A viewport can only be set during a graphics pass");
        Debug.Assert(index >= 0 && index < 8, "A viewport index must be equal to or greater than zero");

        SetViewport(ref viewport, index);
    }

    /// <inheritdoc/>
    public void SetScissor(Rectangle bounds, int index = 0)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A scissor can only be set during a graphics pass");
        Debug.Assert(index >= 0 && index < 8, "A scissor rectangle index must be equal to or greater than zero");

        SetScissor(ref bounds, index);
    }

    /// <inheritdoc/>
    public void ClearRenderTarget(IRenderTarget renderTarget, ref readonly Color color)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A render target can only be cleared during a graphics pass");

        Impl.ClearRenderTarget(renderTarget.Impl.View, color);
    }

    /// <inheritdoc/>
    public void ClearDepthStencil(DepthStencil depthStencil, float depth, byte stencil)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A depth stencil can only be cleared during a graphics pass");

        Impl.ClearDepthStencil(depthStencil.Impl.View, depth, stencil);
    }

    /// <inheritdoc/>
    public void ClearDepthStencil(DepthStencil depthStencil, float depth)
    {
        Debug.Assert(Type == CommandListType.Graphics, "A depth stencil can only be cleared during a graphics pass");

        Impl.ClearDepthStencil(depthStencil.Impl.View, depth, 0);
    }

    /// <inheritdoc/>
    public void Draw(int vertexCount, int instanceCount = 1, int vertexStart = 0, int instanceStart = 0)
    {
        Debug.Assert(pipelineCache != null, "The pipeline cannot be null when drawing geometry");
        Debug.Assert(vertexCount > 0, "The amount of vertices to draw must be greater than zero");
        Debug.Assert(instanceCount >= 1, "The amount of instances to draw must be greater than or equal to one");
        Debug.Assert(vertexStart >= 0, "The vertex offset must be greater than or equal to zero");
        Debug.Assert(instanceStart >= 0, "The instance offset must be greater than or equal to zero");

        Impl.Draw((uint)vertexCount, (uint)instanceCount, (uint)vertexStart, (uint)instanceStart);

        GraphicsDevice.Metrics.PrimitiveCount += GetPrimitiveCount(pipelineCache.Topology, vertexCount);
        GraphicsDevice.Metrics.DrawCalls++;
    }

    /// <inheritdoc/>
    public void DrawIndexed(int indexCount, int instanceCount = 1, int indexStart = 0, int baseVertex = 0, int instanceStart = 0)
    {
        Debug.Assert(pipelineCache != null, "The pipeline cannot be null when drawing geometry");
        Debug.Assert(indexCount > 0, "The amount of indices to draw must be greater than zero");
        Debug.Assert(instanceCount >= 1, "The amount of instances to draw must be greater than or equal to one");
        Debug.Assert(indexStart >= 0, "The index offset must be greater than or equal to zero");
        Debug.Assert(instanceStart >= 0, "The instance offset must be greater than or equal to zero");

        Impl.DrawIndexed((uint)indexCount, (uint)instanceCount, (uint)indexStart, baseVertex, (uint)instanceStart);

        GraphicsDevice.Metrics.PrimitiveCount += GetPrimitiveCount(pipelineCache.Topology, indexCount);
        GraphicsDevice.Metrics.DrawCalls++;
    }

    /// <inheritdoc/>
    public void CopyTexture(Texture source, Texture destination)
    {
        Debug.Assert(source != null, "The source texture cannot be null");
        Debug.Assert(destination != null, "The destination texture cannot be null");

        Impl.CopyTexture(source.Impl, 0, destination.Impl, 0, destination.Impl.Description.Format);
    }

    internal override void Reset()
    {
        pipelineCache = default;

        Array.Clear(viewports);
        Array.Clear(scissors);
        Array.Clear(vertexLayoutCache);

        indexBufferCache = default;

        vertexBufferCount = 0;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            Impl?.Dispose();
        }
    }

    private void SetRenderTargets(Span<IRenderTarget?> renderTargets, DepthStencil? depthStencil)
    {
        const int RenderTargetCount = 8;

        var textures = ArrayPool<ITextureViewImpl?>.Shared.Rent(RenderTargetCount);

        for (int i = 0; i < RenderTargetCount; i++)
        {
            if (i >= renderTargets.Length)
                continue;

            var renderTarget = renderTargets[i];

            if (renderTarget == null)
                continue;

            textures[i] = renderTarget.Impl.View;
        }

        Impl.SetRenderTargets(textures, depthStencil?.Impl.View);

        SetViewportsAndScissors(textures);

        ArrayPool<ITextureViewImpl?>.Shared.Return(textures);
    }

    private void SetViewportsAndScissors(ITextureViewImpl?[] renderTargets)
    {
        for (int i = 0; i < renderTargets.Length; i++)
        {
            var renderTarget = renderTargets[i];

            if (renderTarget == null)
                continue;

            var description = renderTarget.Description.Texture.Description;

            var viewport = new Viewport
            {
                X = 0, Y = 0,
                Width = (int)description.Width,
                Height = (int)description.Height,
                MinimumDepth = 0.0f,
                MaximumDepth = 1.0f,
            };

            SetViewport(ref viewport, i);

            var scissor = (Rectangle)viewport;

            SetScissor(ref scissor, i);
        }
    }

    private void SetViewport(ref Viewport viewport, int i)
    {
        Impl.SetViewport(ref viewport, (uint)i);

        viewports[i] = viewport;
    }

    private void SetScissor(ref Rectangle bounds, int i)
    {
        Impl.SetScissor(ref bounds, (uint)i);

        scissors[i] = bounds;
    }

    private static int GetPrimitiveCount(PrimitiveTopology topology, int count) => topology switch
    {
        PrimitiveTopology.TriangleList => count * 3,
        PrimitiveTopology.TriangleStrip => count + 2,
        PrimitiveTopology.LineList => count * 2,
        PrimitiveTopology.LineStrip => count + 2,
        PrimitiveTopology.PointList or _ => count,
    };
}
