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
using System.Diagnostics;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11GraphicsBackend : IGraphicsBackend
{
    private const int ERROR_NOT_FOUND = unchecked((int)0x887A0002);

    public GraphicsApi Api => GraphicsApi.DirectX11;

    private Dx11GraphicsDevice? graphicsDevice;

    public unsafe IGraphicsDeviceImpl? CreateDevice(QueryAdapter queryAdapter)
    {
        Debug.Assert(queryAdapter != null);

        if (graphicsDevice == null)
        {
            using var dxgi = DXGI.GetApi(null!, forceDxvk: false);
            using var d3d11 = D3D11.GetApi(null!, forceDxvk: false);
            using var d3dc = D3DCompiler.GetApi();

            SilkMarshal.ThrowHResult(dxgi.CreateDXGIFactory2(0, out ComPtr<IDXGIFactory4> factory));

            var adapterIndex = 0u;

            ComPtr<IDXGIAdapter1> adapter = default;
            Dx11GraphicsAdapter? candidate = null;

            while (true)
            {
                var result = factory.EnumAdapters1(adapterIndex, ref adapter);

                if (result == ERROR_NOT_FOUND)
                    break;

                if (result < 0 || adapter.Handle == null)
                    return null;

                var desc = default(AdapterDesc1);

                adapter.GetDesc1(ref desc);

                candidate = new Dx11GraphicsAdapter(
                    new string(desc.Description), adapterIndex,
                    (DeviceVendor)desc.VendorId, DeviceMode.Dedicated);

                if (queryAdapter?.Invoke(candidate) ?? true)
                    break;

                adapter.Release();

                adapterIndex++;
            }

            factory.Release();

            if (candidate == null)
                return null;

            graphicsDevice = new Dx11GraphicsDevice(this, candidate);
        }

        return graphicsDevice;
    }
}
