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

using Hybrid.Numerics;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents an API specific command list implementation.
/// </summary>
public interface ICommandListImpl : IDeviceResource
{
    /// <summary>
    /// Prepares the command list for recording.
    /// </summary>
    public void Begin();

    /// <summary>
    /// Compiles the command list for execution.
    /// </summary>
    public void End();

    public void SetPipeline(IPipelineImpl pipeline);

    public void SetDescriptorSet(uint index, IDescriptorSetImpl descriptors);

    public void SetRenderTargets(ITextureViewImpl?[] renderTargets, ITextureViewImpl? depthStencil);

    public void SetVertexBuffer(IBufferImpl buffer, uint index, uint offset);

    public void SetIndexBuffer(IBufferImpl buffer, IndexFormat format, uint offset);

    public void SetViewport(ref readonly Viewport viewport, uint index);

    public void SetScissor(ref readonly Rectangle bounds, uint index);

    public void ClearRenderTarget(ITextureViewImpl renderTarget, Color color);

    public void ClearDepthStencil(ITextureViewImpl depthStencil, float depth, byte stencil);

    public void Draw(uint vertexCount, uint instanceCount, uint vertexStart, uint instanceStart);

    public void DrawIndexed(uint indexCount, uint instanceCount, uint indexStart, int baseVertex, uint instanceStart);

    public void Dispatch(uint threadGroupWidth, uint threadGroupHeight, uint threadGroupDepth);

    /// <summary>
    /// Maps a GPU <paramref name="resource"/> to CPU memory.
    /// </summary>
    /// <remarks>
    /// Due to limitations with DirectX 11, this operation
    /// will execute on the immediate context.
    /// </remarks>
    /// <param name="resource">The resource to map.</param>
    /// <param name="mode">The mapping mode to use.</param>
    /// <param name="subresource">The subresource to map.</param>
    /// <param name="doNotWait">Determines whether the operation should block.</param>
    /// <returns>A handle to the mapped resource.</returns>
    public MappedResource MapResource(IGpuResource resource, MapMode mode, uint subresource, bool doNotWait);

    /// <summary>
    /// Unmaps a GPU <paramref name="resource"/> from CPU memory.
    /// </summary>
    /// <remarks>
    /// Due to limitations with DirectX 11, this operation
    /// will execute on the immediate context.
    /// </remarks>
    /// <param name="resource">The resource to unmap.</param>
    public void UnmapResource(MappedResource resource);

    public void WriteResource(IGpuResource resource, uint subresource, DataBox data);

    public void WriteResource(IGpuResource resource, uint subresource, ResourceRegion region, DataBox data);

    public void CopyTexture(ITextureImpl source, uint sourceSubresource, ITextureImpl destination, uint destinationSubresource, TextureFormat format);
}
