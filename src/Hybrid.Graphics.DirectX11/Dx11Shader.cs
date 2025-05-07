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
using Silk.NET.Direct3D.Compilers;
using Silk.NET.Direct3D11;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11Shader : Dx11DeviceResource, IShaderImpl
{
    public IShaderBytecodeImpl Bytecode { get; }

    public ShaderDescription Description { get; }

    internal ComPtr<ID3D11DeviceChild> NativeResource;
    internal ComPtr<ID3D10Blob> NativeBlob;

    internal unsafe Dx11Shader(
        Dx11GraphicsDevice graphicsDevice,
        Dx11ShaderBytecode bytecode,
        ref ShaderDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        Bytecode = bytecode;

        NativeBlob = bytecode.Blob;

        switch (bytecode.Stage)
        {
            case ShaderStage.Vertex:
                {
                    ComPtr<ID3D11VertexShader> vertexShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateVertexShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref vertexShader));

                    SilkMarshal.ThrowHResult(vertexShader.QueryInterface(out NativeResource));
                }
                break;
            case ShaderStage.Hull:
                {
                    ComPtr<ID3D11HullShader> hullShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateHullShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref hullShader));

                    SilkMarshal.ThrowHResult(hullShader.QueryInterface(out NativeResource));
                }
                break;
            case ShaderStage.Domain:
                {
                    ComPtr<ID3D11DomainShader> domainShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateDomainShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref domainShader));

                    SilkMarshal.ThrowHResult(domainShader.QueryInterface(out NativeResource));
                }
                break;
            case ShaderStage.Geometry:
                {
                    ComPtr<ID3D11GeometryShader> geometryShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateGeometryShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref geometryShader));

                    SilkMarshal.ThrowHResult(geometryShader.QueryInterface(out NativeResource));
                }
                break;
            case ShaderStage.Pixel:
                {
                    ComPtr<ID3D11PixelShader> pixelShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreatePixelShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref pixelShader));

                    SilkMarshal.ThrowHResult(pixelShader.QueryInterface(out NativeResource));
                }
                break;
            case ShaderStage.Compute:
                {
                    ComPtr<ID3D11ComputeShader> computeShader = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateComputeShader(
                        NativeBlob.GetBufferPointer(),
                        NativeBlob.GetBufferSize(),
                        ref Unsafe.NullRef<ID3D11ClassLinkage>(),
                        ref computeShader));

                    SilkMarshal.ThrowHResult(computeShader.QueryInterface(out NativeResource));
                }
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    protected override void Dispose(bool disposing)
    {
        ReleaseCom(ref NativeBlob);
        ReleaseCom(ref NativeResource);
    }
}
