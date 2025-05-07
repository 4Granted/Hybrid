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
using Silk.NET.DXGI;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11GraphicsDevice : Dx11Resource, IGraphicsDeviceImpl
{
    public Dx11GraphicsBackend Backend { get; }
    IGraphicsBackend IGraphicsDeviceImpl.Backend => Backend;

    public Dx11GraphicsFactory Factory { get; }
    IGraphicsFactoryImpl IGraphicsDeviceImpl.Factory => Factory;

    public Dx11GraphicsAdapter Adapter { get; }
    IGraphicsAdapterImpl IGraphicsDeviceImpl.Adapter => Adapter;

    internal readonly DXGI DXGI;
    internal readonly D3D11 D3D11;
    internal readonly D3DCompiler D3DC;
    internal ComPtr<IDXGIFactory4> NativeFactory;
    internal ComPtr<ID3D11Device5> NativeDevice;
    internal ComPtr<ID3D11DeviceContext4> NativeImmediateContext;

    internal unsafe Dx11GraphicsDevice(
        Dx11GraphicsBackend backend,
        Dx11GraphicsAdapter adapter)
    {
        Backend = backend;
        Adapter = adapter;

        DXGI = DXGI.GetApi(null!, forceDxvk: false);
        D3D11 = D3D11.GetApi(null!, forceDxvk: false);
        D3DC = D3DCompiler.GetApi();

        SilkMarshal.ThrowHResult(DXGI.CreateDXGIFactory2(0, out NativeFactory));
        SilkMarshal.ThrowHResult(D3D11.CreateDevice(default(ComPtr<IDXGIAdapter>), D3DDriverType.Hardware, Software: default, (uint)CreateDeviceFlag.Debug, null, 0, D3D11.SdkVersion, ref NativeDevice, null, ref NativeImmediateContext));

        Factory = new Dx11GraphicsFactory(this);
    }

    protected override void Dispose(bool disposing)
    {
        Factory.Dispose();

        ReleaseCom(ref NativeImmediateContext);
        ReleaseCom(ref NativeDevice);
        ReleaseCom(ref NativeFactory);

        D3DC.Dispose();
        D3D11.Dispose();
        DXGI.Dispose();
    }
}
