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

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11Pipeline : Dx11DeviceResource, IPipelineImpl
{
    public PipelineDescription Description { get; }

    // Note: These resources are handled by the Dx11StateCache
    internal ComPtr<ID3D11RasterizerState> RasterizerState;
    internal ComPtr<ID3D11BlendState> BlendState;
    internal ComPtr<ID3D11DepthStencilState> DepthStencilState;
    internal ComPtr<ID3D11InputLayout> InputLayout;
    internal ComPtr<ID3D11VertexShader> VertexShader;
    internal ComPtr<ID3D11GeometryShader> GeometryShader;
    internal ComPtr<ID3D11PixelShader> PixelShader;
    internal Dx11DescriptorLayout[] DescriptorLayouts;
    internal D3DPrimitiveTopology PrimitiveTopology;
    internal Color BlendFactor;
    internal uint[] VertexStrides;
    internal uint StencilReference;

    internal Dx11Pipeline(
        Dx11GraphicsDevice graphicsDevice,
        Dx11StateCache resourceCache,
        ref PipelineDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        // Create the vertex shader
        CommonExtensions.AsOrThrow(in description.VertexShader, out Dx11Shader vertexShader);

        SilkMarshal.ThrowHResult(vertexShader.NativeResource.QueryInterface(out VertexShader));

        // Create the pixel shader
        if (description.GeometryShader is { } gs)
        {
            CommonExtensions.AsOrThrow(in gs, out Dx11Shader geometryShader);

            SilkMarshal.ThrowHResult(geometryShader.NativeResource.QueryInterface(out GeometryShader));
        }

        // Create the pixel shader
        CommonExtensions.AsOrThrow(in description.PixelShader, out Dx11Shader pixelShader);

        SilkMarshal.ThrowHResult(pixelShader.NativeResource.QueryInterface(out PixelShader));

        resourceCache.GetResources(
               ref description.RasterizerState,
               ref description.BlendState,
               ref description.DepthStencilState,
               ref description.VertexLayout,
               vertexShader,
               out RasterizerState,
               out BlendState,
               out DepthStencilState,
               out InputLayout);

        CommonExtensions.AsOrThrow(in description.DescriptorLayouts, out DescriptorLayouts);

        PrimitiveTopology = Dx11Utilities.GetPrimitiveTopology(description.Topology);
        BlendFactor = description.BlendFactor;

        if (description.VertexLayout is { } layout && layout != default)
        {
            var vertexBindings = layout.Layouts.Length;

            VertexStrides = new uint[vertexBindings];

            for (var i = 0;i < vertexBindings; i++)
            {
                VertexStrides[i] = layout.Layouts[i].StrideInBytes;
            }
        }
        else
        {
            VertexStrides = [];
        }
    }
}
