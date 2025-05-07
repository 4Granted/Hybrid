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
using Silk.NET.Core.Native;
using Silk.NET.Direct3D11;
using System.Diagnostics;
using System.Text;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11StateCache : Dx11DeviceResource
{
    private readonly Dictionary<RasterizerState, ComPtr<ID3D11RasterizerState>> rasterizerStates = [];
    private readonly Dictionary<BlendState, ComPtr<ID3D11BlendState>> blendStates = [];
    private readonly Dictionary<DepthStencilState, ComPtr<ID3D11DepthStencilState>> depthStencilStates = [];
    private readonly Dictionary<VertexDeclaration, ComPtr<ID3D11InputLayout>> inputLayouts = [];
    private readonly object stateLock = new();

    internal Dx11StateCache(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice) { }

    public void GetResources(
        ref RasterizerState rasterizerDescription,
        ref BlendState blendDescription,
        ref DepthStencilState depthStencilDescription,
        ref VertexDeclaration vertexLayout,
        Dx11Shader vertexShader,
        out ComPtr<ID3D11RasterizerState> rasterizerState,
        out ComPtr<ID3D11BlendState> blendState,
        out ComPtr<ID3D11DepthStencilState> depthStencilState,
        out ComPtr<ID3D11InputLayout> inputLayout)
    {
        lock (stateLock)
        {
            rasterizerState = GetRasterizerState(ref rasterizerDescription);
            blendState = GetBlendState(ref blendDescription);
            depthStencilState = GetDepthStencilState(ref depthStencilDescription);
            inputLayout = vertexLayout != default
                    ? GetInputLayout(ref vertexLayout, vertexShader)
                    : default;
        }
    }

    private ComPtr<ID3D11RasterizerState> GetRasterizerState(ref RasterizerState description)
    {
        Debug.Assert(Monitor.IsEntered(stateLock));

        if (!rasterizerStates.TryGetValue(description, out var rasterizerState))
        {
            rasterizerState = CreateRasterizerState(ref description);

            rasterizerStates.Add(description, rasterizerState);
        }

        return rasterizerState;
    }

    private ComPtr<ID3D11RasterizerState> CreateRasterizerState(ref RasterizerState description)
    {
        ComPtr<ID3D11RasterizerState> rasterizerState = default;

        var rasterizerDescription = new RasterizerDesc
        {
            FillMode = description.FillMode switch
            {
                Native.FillMode.Solid => Silk.NET.Direct3D11.FillMode.Solid,
                Native.FillMode.Wireframe => Silk.NET.Direct3D11.FillMode.Wireframe,
                _ => Silk.NET.Direct3D11.FillMode.None,
            },
            CullMode = description.CullMode switch
            {
                Native.CullMode.Front => Silk.NET.Direct3D11.CullMode.Front,
                Native.CullMode.Back => Silk.NET.Direct3D11.CullMode.Back,
                Native.CullMode.None or _ => Silk.NET.Direct3D11.CullMode.None,
            },
            DepthBias = 0,
            DepthBiasClamp = 0.0f,
            SlopeScaledDepthBias = 0.0f,
            DepthClipEnable = description.DepthEnabled,
            ScissorEnable = description.ScissorEnabled,
            FrontCounterClockwise = description.WindingMode == WindingMode.CounterClockwise,
            MultisampleEnable = true,
            AntialiasedLineEnable = true,
        };

        SilkMarshal.ThrowHResult(NativeDevice.CreateRasterizerState(ref rasterizerDescription, ref rasterizerState));

        return rasterizerState;
    }

    private ComPtr<ID3D11BlendState> GetBlendState(ref BlendState description)
    {
        Debug.Assert(Monitor.IsEntered(stateLock));

        if (!blendStates.TryGetValue(description, out var blendState))
        {
            blendState = CreateBlendState(ref description);

            blendStates.Add(description, blendState);
        }

        return blendState;
    }

    private ComPtr<ID3D11BlendState> CreateBlendState(ref BlendState description)
    {
        static RenderTargetBlendDesc CreateDescription(BlendTarget target)
        {
            return new RenderTargetBlendDesc
            {
                BlendOp = Dx11Utilities.GetBlendFunction(target.ColorFunction),
                BlendOpAlpha = Dx11Utilities.GetBlendFunction(target.AlphaFunction),
                SrcBlend = Dx11Utilities.GetBlendFactor(target.SourceColorFactor),
                SrcBlendAlpha = Dx11Utilities.GetBlendFactor(target.SourceAlphaFactor),
                DestBlend = Dx11Utilities.GetBlendFactor(target.DestinationColorFactor),
                DestBlendAlpha = Dx11Utilities.GetBlendFactor(target.DestinationAlphaFactor),
                RenderTargetWriteMask = (byte)Dx11Utilities.GetColorChannels(target.ColorWriteMask),
                BlendEnable = target.BlendEnabled,
            };
        }

        ComPtr<ID3D11BlendState> blendState = default;

        var blendStateDescription = new BlendDesc
        {
            RenderTarget = new BlendDesc.RenderTargetBuffer
            {
                Element0 = CreateDescription(description.Target0),
                Element1 = CreateDescription(description.Target1),
                Element2 = CreateDescription(description.Target2),
                Element3 = CreateDescription(description.Target3),
                Element4 = CreateDescription(description.Target4),
                Element5 = CreateDescription(description.Target5),
                Element6 = CreateDescription(description.Target6),
                Element7 = CreateDescription(description.Target7),
            },
            AlphaToCoverageEnable = description.AlphaToConvergeEnabled,
            IndependentBlendEnable = description.IndependentBlendEnabled,
        };

        SilkMarshal.ThrowHResult(NativeDevice.CreateBlendState(ref blendStateDescription, ref blendState));

        return blendState;
    }

    private ComPtr<ID3D11DepthStencilState> GetDepthStencilState(ref DepthStencilState description)
    {
        Debug.Assert(Monitor.IsEntered(stateLock));

        if (!depthStencilStates.TryGetValue(description, out var depthStencilState))
        {
            depthStencilState = CreateDepthStencilState(ref description);

            depthStencilStates.Add(description, depthStencilState);
        }

        return depthStencilState;
    }

    private ComPtr<ID3D11DepthStencilState> CreateDepthStencilState(ref DepthStencilState description)
    {
        ComPtr<ID3D11DepthStencilState> depthStencilState = default;

        var depthStencilStateDescription = new DepthStencilDesc
        {
            DepthFunc = Dx11Utilities.GetComparison(description.DepthComparison),
            FrontFace = CreateDepthStencilBehavior(description.StencilFront),
            BackFace = CreateDepthStencilBehavior(description.StencilFront),
            StencilReadMask = description.StencilReadMask,
            StencilWriteMask = description.StencilWriteMask,
            DepthEnable = description.DepthEnabled,
            DepthWriteMask = description.DepthWriteEnabled ? DepthWriteMask.All : DepthWriteMask.Zero,
            StencilEnable = description.StencilEnabled,
        };

        SilkMarshal.ThrowHResult(NativeDevice.CreateDepthStencilState(ref depthStencilStateDescription, ref depthStencilState));

        return depthStencilState;
    }

    private ComPtr<ID3D11InputLayout> GetInputLayout(ref VertexDeclaration layout, Dx11Shader vertexShader)
    {
        Debug.Assert(Monitor.IsEntered(stateLock));

        if (!inputLayouts.TryGetValue(layout, out var inputLayout))
        {
            inputLayout = CreateInputLayout(ref layout, vertexShader);

            inputLayouts.Add(layout, inputLayout);
        }

        return inputLayout;
    }

    private unsafe ComPtr<ID3D11InputLayout> CreateInputLayout(ref VertexDeclaration layout, Dx11Shader vertexShader)
    {
        ComPtr<ID3D11InputLayout> inputLayout = default;

        var groups = layout.Layouts;

        var elementCount = 0;
        var elementIndex = 0;

        foreach (var group in groups)
        {
            elementCount += group.Elements.Length;
        }

        var inputs = new InputElementDesc[elementCount];

        for (int slot = 0; slot < groups.Length; slot++)
        {
            var group = groups[slot];
            var elements = group.Elements;
            var type = group.InstanceOffset == 0
                ? InputClassification.PerVertexData
                : InputClassification.PerInstanceData;
            var offset = 0u;

            foreach (var element in elements)
            {
                var format = element.Format;
                var semantic = Encoding.UTF8.GetBytes(element.Semantic);
                var elementOffset = element.Offset != 0 ? element.Offset : offset;

                fixed (byte* semanticBytes = semantic)
                {
                    inputs[elementIndex++] = new InputElementDesc
                    {
                        Format = Dx11Utilities.GetFormat(format),
                        SemanticName = semanticBytes,
                        SemanticIndex = 0,
                        InputSlotClass = type,
                        InputSlot = (uint)slot,
                        AlignedByteOffset = elementOffset,
                        InstanceDataStepRate = group.InstanceOffset,
                    };
                }

                offset += VertexElement.GetSizeInBytes(format);
            }
        }

        var blob = vertexShader.NativeBlob;

        fixed (InputElementDesc* descs = inputs)
        {
            SilkMarshal.ThrowHResult(NativeDevice.CreateInputLayout(descs, (uint)inputs.Length, blob.GetBufferPointer(), blob.GetBufferSize(), ref inputLayout));
        }

        return inputLayout;
    }

    protected override void Dispose(bool disposing)
    {
        inputLayouts.DisposeValues();
        depthStencilStates.DisposeValues();
        blendStates.DisposeValues();
        rasterizerStates.DisposeValues();
    }

    private static DepthStencilopDesc CreateDepthStencilBehavior(StencilBehavior description)
    {
        return new DepthStencilopDesc
        {
            StencilFunc = Dx11Utilities.GetComparison(description.StencilFunction),
            StencilPassOp = Dx11Utilities.GetStencilOperation(description.Pass),
            StencilFailOp = Dx11Utilities.GetStencilOperation(description.Fail),
            StencilDepthFailOp = Dx11Utilities.GetStencilOperation(description.DepthFail),
        };
    }
}
