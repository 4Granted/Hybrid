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
using Hybrid.Numerics;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using NativeViewport = Silk.NET.Direct3D11.Viewport;
using NativeBox = Silk.NET.Maths.Box2D<int>;
using NativePoint = Silk.NET.Maths.Vector2D<int>;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11CommandList : Dx11DeviceResource, ICommandListImpl
{
    [Flags]
    private enum CommandListFlags
    {
        None = 0,
        Recording = 1 << 0,
        Compiled = 1 << 1,
        ViewportsChanged = 1 << 2,
        ScissorsChanged = 1 << 3,
        BuffersChanged = 1 << 4,
    }

    internal ComPtr<ID3D11DeviceContext4> NativeImmediateContext;
    internal ComPtr<ID3D11DeviceContext4> NativeDeferredContext;
    internal ComPtr<ID3D11CommandList> NativeCommandList;

    private readonly Lock immediateLock = new();
    private readonly Lock mapLock = new();

    private CommandListFlags flags = CommandListFlags.None;
    private Dx11Pipeline? cachedPipeline;
    private ComPtr<ID3D11RasterizerState> cachedRasterizerState;
    private ComPtr<ID3D11BlendState> cachedBlendState;
    private ComPtr<ID3D11DepthStencilState> cachedDepthStencilState;
    private IBufferImpl[] cachedVertexBuffers = new IBufferImpl[1];
    private NativeViewport[] cachedViewports = new NativeViewport[1];
    private NativeBox[] cachedScissors = new NativeBox[1];
    private Color cachedBlendFactor = default;
    private uint[] cachedVertexStrides = new uint[1];
    private uint[] cachedVertexOffsets = new uint[1];
    private uint cachedVertexCount;
    private uint cachedIndexBuffer;
    private uint cachedIndexOffset;
    private bool stencilEnabled;

    internal Dx11CommandList(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
    {
        NativeImmediateContext = graphicsDevice.NativeImmediateContext;

        graphicsDevice.NativeDevice.CreateDeferredContext(0, ref NativeDeferredContext);
    }

    public void Begin()
    {
        flags |= CommandListFlags.Recording;

        ReleaseCom(ref NativeCommandList);

        NativeDeferredContext.ClearState();

        ClearState();
    }

    public void End()
    {
        if ((flags & CommandListFlags.Recording) == 0)
            throw new InvalidOperationException();

        flags &= ~CommandListFlags.Recording;
        flags |= CommandListFlags.Compiled;

        NativeDeferredContext.FinishCommandList(false, ref NativeCommandList);

        ClearState();
    }

    public unsafe void SetRenderTargets(ITextureViewImpl?[] renderTargets, ITextureViewImpl? depthStencil)
    {
        const int RenderTargetCount = 8;

        var rtvs = stackalloc ID3D11RenderTargetView*[RenderTargetCount];

        ID3D11DepthStencilView* dsv = null;

        var rtv = rtvs;

        for (int i = 0; i < RenderTargetCount; i++)
        {
            var renderTarget = renderTargets[i];

            if (renderTarget is Dx11TextureView dxRtv)
            {
                *(rtv + i) = dxRtv.DxRtv;
            }
        }

        if (depthStencil is Dx11TextureView dxDsv)
        {
            dsv = dxDsv.DxDsv;
        }

        NativeDeferredContext.OMSetRenderTargets((uint)RenderTargetCount, rtvs, dsv);
    }

    public unsafe void SetPipeline(IPipelineImpl pipeline)
    {
        if (pipeline.Description != cachedPipeline?.Description)
        {
            Utilities.AsOrThrow(in pipeline, out Dx11Pipeline dxPipeline);

            cachedPipeline = dxPipeline;

            var blendFactor = dxPipeline.BlendFactor;

            if (dxPipeline.RasterizerState.Handle != cachedRasterizerState)
            {
                cachedRasterizerState = dxPipeline.RasterizerState;

                NativeDeferredContext.RSSetState(cachedRasterizerState);
            }

            if (dxPipeline.BlendState.Handle != cachedBlendState || blendFactor != cachedBlendFactor)
            {
                cachedBlendState = dxPipeline.BlendState;
                cachedBlendFactor = blendFactor;

                NativeDeferredContext.OMSetBlendState(cachedBlendState, (float*)&blendFactor, 0xFFFFFFFF);
            }

            if (dxPipeline.DepthStencilState.Handle != cachedDepthStencilState)
            {
                cachedDepthStencilState = dxPipeline.DepthStencilState;

                stencilEnabled = dxPipeline.Description.DepthStencilState.StencilEnabled;

                NativeDeferredContext.OMSetDepthStencilState(cachedDepthStencilState, 0);
            }

            if (dxPipeline.VertexStrides.Length > 0)
            {
                NativeDeferredContext.IASetInputLayout(dxPipeline.InputLayout);
            }
            else
            {
                NativeDeferredContext.IASetInputLayout(ref SilkMarshal.NullRef<ID3D11InputLayout>());
            }

            NativeDeferredContext.VSSetShader(dxPipeline.VertexShader, ref Unsafe.NullRef<ComPtr<ID3D11ClassInstance>>(), 0);
            NativeDeferredContext.GSSetShader(dxPipeline.GeometryShader, ref Unsafe.NullRef<ComPtr<ID3D11ClassInstance>>(), 0);
            NativeDeferredContext.PSSetShader(dxPipeline.PixelShader, ref Unsafe.NullRef<ComPtr<ID3D11ClassInstance>>(), 0);

            NativeDeferredContext.IASetPrimitiveTopology(dxPipeline.PrimitiveTopology);

            var vertexStrides = dxPipeline.VertexStrides;

            if (!Utilities.CompareArray(in cachedVertexStrides, in vertexStrides))
            {
                var length = vertexStrides.Length;

                Utilities.EnsureSize(ref cachedVertexBuffers, length);
                Utilities.EnsureSize(ref cachedVertexStrides, length);
                Utilities.EnsureSize(ref cachedVertexOffsets, length);

                vertexStrides.CopyTo(cachedVertexStrides, 0);
            }
        }
    }

    public void SetDescriptorSet(uint index, IDescriptorSetImpl descriptors)
    {
        Utilities.AsOrThrow(in descriptors, out Dx11DescriptorSet dxDescriptors);
        Utilities.AsOrThrow(descriptors.Layout, out Dx11DescriptorLayout dxLayout);

        var elements = dxLayout.Elements;
        var entries = dxDescriptors.Entries;
        var resources = dxDescriptors.Resources;

        var constantsOffset = GetOffset(DescriptorType.ConstantResource, index);
        var graphicsOffset = GetOffset(DescriptorType.GraphicsResource, index);
        var computeOffset = GetOffset(DescriptorType.ComputeResource, index);
        var samplerOffset = GetOffset(DescriptorType.SamplerResource, index);

        for (int i = 0; i < entries.Length; i++)
        {
            var entry = entries[i];
            var resource = resources[i];
            var element = elements[entry.Index];

            if (resource == null)
                continue;

            switch (element.Type)
            {
                case DescriptorType.ConstantResource:
                    SetConstantsBuffer(element.Stage, constantsOffset + element.Index, resource);
                    break;
                case DescriptorType.GraphicsResource:
                    SetShaderResource(element.Stage, graphicsOffset + element.Index, resource);
                    break;
                case DescriptorType.ComputeResource:
                    SetUnorderedAccess(element.Stage, computeOffset + element.Index, resource);
                    break;
                case DescriptorType.SamplerResource:
                    SetSampler(samplerOffset + element.Index, resource);
                    break;
            }
        }
    }

    public void SetVertexBuffer(IBufferImpl buffer, uint index, uint offset)
    {
        Utilities.AsOrThrow(in buffer, out Dx11Buffer dxBuffer);

        if (buffer?.Id != cachedVertexBuffers[index]?.Id || offset != cachedVertexOffsets[index])
        {
            cachedVertexBuffers[index] = dxBuffer;
            cachedVertexOffsets[index] = offset;

            cachedVertexCount = Math.Max(index + 1, cachedVertexCount);

            flags |= CommandListFlags.BuffersChanged;
        }
    }

    public void SetIndexBuffer(IBufferImpl buffer, IndexFormat format, uint offset)
    {
        if (buffer.Id != cachedIndexBuffer || offset != cachedIndexOffset)
        {
            Utilities.AsOrThrow(in buffer, out Dx11Buffer dxBuffer);

            cachedIndexBuffer = buffer.Id;
            cachedIndexOffset = offset;

            var dxFormat = format switch
            {
                IndexFormat.UInt16 => Format.FormatR16Uint,
                IndexFormat.UInt32 or _ => Format.FormatR32Uint
            };

            NativeDeferredContext.IASetIndexBuffer(dxBuffer.NativeBuffer, dxFormat, offset);
        }
    }

    public void SetViewport(ref readonly Numerics.Viewport viewport, uint index)
    {
        Utilities.EnsureSize(ref cachedViewports, (int)index + 1);

        cachedViewports[index] = new NativeViewport
        {
            TopLeftX = viewport.X,
            TopLeftY = viewport.Y,
            Width = viewport.Width,
            Height = viewport.Height,
            MinDepth = viewport.MinimumDepth,
            MaxDepth = viewport.MaximumDepth,
        };

        flags |= CommandListFlags.ViewportsChanged;
    }

    public void SetScissor(ref readonly Rectangle bounds, uint index)
    {
        Utilities.EnsureSize(ref cachedScissors, (int)index + 1);

        cachedScissors[index] = new NativeBox
        {
            Min = new NativePoint
            {
                X = bounds.X,
                Y = bounds.Y,
            },
            Max = new NativePoint
            {
                X = bounds.X + bounds.Width,
                Y = bounds.Y + bounds.Height,
            },
        };

        flags |= CommandListFlags.ScissorsChanged;
    }

    public unsafe void ClearRenderTarget(ITextureViewImpl renderTarget, Color color)
    {
        Utilities.AsOrThrow(in renderTarget, out Dx11TextureView dxView);

        NativeDeferredContext.ClearRenderTargetView(dxView.DxRtv, (float*)&color);
    }

    public void ClearDepthStencil(ITextureViewImpl depthStencil, float depth, byte stencil)
    {
        var flags = ClearFlag.Depth;

        if (stencilEnabled)
            flags |= ClearFlag.Stencil;

        Utilities.AsOrThrow(in depthStencil, out Dx11TextureView dxView);

        NativeDeferredContext.ClearDepthStencilView(dxView.DxDsv, (uint)flags, depth, stencil);
    }

    public void Draw(uint vertexCount, uint instanceCount, uint vertexStart, uint instanceStart)
    {
        PrepareState();

        if (instanceCount == 1 && instanceStart == 0)
        {
            NativeDeferredContext.Draw(vertexCount, vertexStart);
        }
        else
        {
            NativeDeferredContext.DrawInstanced(vertexCount, instanceCount, vertexStart, instanceStart);
        }
    }

    public void DrawIndexed(uint indexCount, uint instanceCount, uint indexStart, int baseVertex, uint instanceStart)
    {
        PrepareState();

        if (instanceCount == 1 && instanceStart == 0)
        {
            NativeDeferredContext.DrawIndexed(indexCount, indexStart, baseVertex);
        }
        else
        {
            NativeDeferredContext.DrawIndexedInstanced(indexCount, instanceCount, indexStart, baseVertex, instanceStart);
        }
    }

    public void Dispatch(uint threadGroupWidth, uint threadGroupHeight, uint threadGroupDepth)
    {

    }

    public unsafe MappedResource MapResource(IGpuResource resource, MapMode mode, uint subresource, bool doNotWait)
    {
        lock (mapLock)
        {
            _ = resource ?? throw new ArgumentNullException(nameof(resource));

            var type = Dx11Utilities.GetMapMode(mode);
            var flags = doNotWait ? (uint)MapFlag.DONotWait : 0;

            lock (immediateLock)
            {
                Utilities.AsOrThrow(in resource, out Dx11NativeResource dx);

                MappedSubresource mapped = default;

                SilkMarshal.ThrowHResult(NativeImmediateContext.Map(dx.NativeResource, subresource, type, flags, ref mapped));

                var box = new DataBox
                {
                    DataPointer = (nint)mapped.PData,
                    RowPitch = mapped.RowPitch,
                    SlicePitch = mapped.DepthPitch,
                };

                return new MappedResource(resource, box, subresource);
            }
        }
    }

    public void UnmapResource(MappedResource resource)
    {
        lock (mapLock)
        {
            lock (immediateLock)
            {
                Utilities.AsOrThrow(in resource.Resource, out Dx11NativeResource dx);

                NativeImmediateContext.Unmap(dx.NativeResource, resource.Subresource);
            }
        }
    }

    public unsafe void WriteResource(IGpuResource resource, uint subresource, DataBox data)
    {
        Utilities.AsOrThrow(in resource, out Dx11NativeResource dx);

        NativeDeferredContext.UpdateSubresource1(dx.NativeResource, 0, ref SilkMarshal.NullRef<Silk.NET.Direct3D11.Box>(), data.DataPointer.ToPointer(), data.RowPitch, data.SlicePitch, 0);
    }

    public unsafe void WriteResource(IGpuResource resource, uint subresource, ResourceRegion region, DataBox data)
    {
        Utilities.AsOrThrow(in resource, out Dx11NativeResource dx);

        NativeDeferredContext.UpdateSubresource1(dx.NativeResource, subresource, ref region.AsDx(), data.DataPointer.ToPointer(), data.RowPitch, data.SlicePitch, 0);
    }

    public void CopyTexture(ITextureImpl source, uint sourceSubresource, ITextureImpl destination, uint destinationSubresource, TextureFormat format)
    {
        Utilities.AsOrThrow(in source, out Dx11Texture dxs);
        Utilities.AsOrThrow(in destination, out Dx11Texture dxd);

        NativeDeferredContext.ResolveSubresource(dxd.NativeResource, destinationSubresource, dxs.NativeResource, sourceSubresource, dxd.NativeFormat);
    }

    internal void Complete()
    {
        flags = CommandListFlags.None;

        ReleaseCom(ref NativeCommandList);
    }

    internal void Reset()
    {
        if ((flags & CommandListFlags.Compiled) != 0)
        {
            ReleaseCom(ref NativeCommandList);
        }
        else if ((flags & CommandListFlags.Recording) != 0)
        {
            NativeDeferredContext.ClearState();
            NativeDeferredContext.FinishCommandList(false, ref NativeCommandList);

            ReleaseCom(ref NativeCommandList);
        }

        ClearState();
    }

    protected override void Dispose(bool disposing)
    {
        ReleaseCom(ref NativeCommandList);
        ReleaseCom(ref NativeDeferredContext);
    }

    private unsafe void PrepareState()
    {
        if ((flags & CommandListFlags.ViewportsChanged) != 0)
        {
            NativeDeferredContext.RSSetViewports((uint)cachedViewports.Length, cachedViewports);

            flags &= ~CommandListFlags.ViewportsChanged;
        }

        if ((flags & CommandListFlags.ScissorsChanged) != 0)
        {
            var scissorCount = (uint)cachedScissors.Length;

            if (scissorCount > 0)
            {
                NativeDeferredContext.RSSetScissorRects(scissorCount, cachedScissors);
            }

            flags &= ~CommandListFlags.ScissorsChanged;
        }

        if ((flags & CommandListFlags.BuffersChanged) != 0)
        {
            var vertexBuffers = stackalloc ID3D11Buffer*[(int)cachedVertexCount];

            var rtv = vertexBuffers;

            for (int i = 0; i < cachedVertexCount; i++)
            {
                var vertexBuffer = cachedVertexBuffers[i];

                if (vertexBuffer is Dx11Buffer dxBuffer)
                {
                    *(rtv + i) = dxBuffer.NativeBuffer;
                }
            }

            NativeDeferredContext.IASetVertexBuffers(0, cachedVertexCount,
                vertexBuffers, cachedVertexStrides, cachedVertexOffsets);

            flags &= ~CommandListFlags.BuffersChanged;
        }
    }

    private void ClearState()
    {
        cachedPipeline = default;
        cachedRasterizerState = default;
        cachedBlendState = default;
        cachedDepthStencilState = default;

        Array.Clear(cachedViewports);
        Array.Clear(cachedScissors);
        Array.Clear(cachedVertexBuffers);
        Array.Clear(cachedVertexStrides);
        Array.Clear(cachedVertexOffsets);

        cachedVertexCount = 0;
        cachedIndexBuffer = 0;
        cachedIndexOffset = 0;
    }

    private uint GetOffset(DescriptorType type, uint index)
    {
        if (cachedPipeline == null)
            return 0;

        var layouts = cachedPipeline.DescriptorLayouts;

        uint offset = 0;

        for (var i = 0; i < index; i++)
        {
            var layout = layouts[i];

            offset += type switch
            {
                DescriptorType.ConstantResource => layout.Constants,
                DescriptorType.GraphicsResource => layout.Graphics,
                DescriptorType.ComputeResource => layout.Compute,
                DescriptorType.SamplerResource => layout.Samplers,
                _ => throw new InvalidOperationException()
            };
        }

        return offset;
    }

    private void SetConstantsBuffer(ShaderStage stage, uint index, Dx11NativeResource resource)
    {
        Utilities.AsOrThrow(in resource, out Dx11Buffer dxBuffer);

        var buffer = dxBuffer.NativeBuffer;

        if ((stage & ShaderStage.Vertex) != 0)
        {
            NativeDeferredContext.VSSetConstantBuffers(index, 1, ref buffer);
        }

        if ((stage & ShaderStage.Geometry) != 0)
        {
            NativeDeferredContext.GSSetConstantBuffers(index, 1, ref buffer);
        }

        if ((stage & ShaderStage.Pixel) != 0)
        {
            NativeDeferredContext.PSSetConstantBuffers(index, 1, ref buffer);
        }
    }

    private void SetShaderResource(ShaderStage stage, uint index, Dx11NativeResource resource)
    {
        var srv = resource switch
        {
            Dx11TextureView textureView => textureView.DxSrv,
            Dx11Buffer buffer => buffer.GetShaderResourceView(new BufferRange(0, buffer.SizeInBytes)),
            _ => throw new InvalidOperationException(),
        };

        if (stage.HasFlag(ShaderStage.Vertex))
        {
            NativeDeferredContext.VSSetShaderResources(index, 1, ref srv);
        }

        if (stage.HasFlag(ShaderStage.Geometry))
        {
            NativeDeferredContext.GSSetShaderResources(index, 1, ref srv);
        }

        if (stage.HasFlag(ShaderStage.Pixel))
        {
            NativeDeferredContext.PSSetShaderResources(index, 1, ref srv);
        }
    }

    private void SetUnorderedAccess(ShaderStage stage, uint index, Dx11NativeResource resource)
    {

    }

    private void SetSampler(uint index, Dx11NativeResource resource)
    {
        Utilities.AsOrThrow(in resource, out Dx11Sampler dxSampler);

        NativeDeferredContext.PSSetSamplers(index, 1, ref dxSampler.NativeSampler);
    }
}
