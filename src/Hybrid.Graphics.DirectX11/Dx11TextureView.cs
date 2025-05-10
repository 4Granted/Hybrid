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

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11TextureView : Dx11NativeResource, ITextureViewImpl
{
    public TextureViewDescription Description { get; }

    internal ComPtr<ID3D11RenderTargetView> DxRtv;
    internal ComPtr<ID3D11DepthStencilView> DxDsv;
    internal ComPtr<ID3D11ShaderResourceView> DxSrv;
    internal ComPtr<ID3D11UnorderedAccessView> DxUav;

    private readonly WeakReference reference;
    private readonly Lock commandListLock = new();

    internal Dx11TextureView(
        Dx11GraphicsDevice graphicsDevice,
        ref TextureViewDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        CommonExtensions.AsOrThrow(description.Texture, out Dx11Texture texture);

        var usage = texture.Description.Usage;

        if ((usage & TextureUsage.RenderTarget) != 0)
        {
            DxRtv = CreateRtv(texture);
        }

        if ((usage & TextureUsage.DepthStencil) != 0)
        {
            DxDsv = CreateDsv(texture);
        }

        if ((usage & TextureUsage.SampleBuffer) != 0)
        {
            DxSrv = CreateSrv(texture);
        }

        if ((usage & TextureUsage.ColorBuffer) != 0)
        {
            DxUav = CreateUav(texture);
        }

        reference = new WeakReference(this);
    }

    protected override void Dispose(bool disposing)
    {
        var usage = Description.Texture.Description.Usage;

        if ((usage & TextureUsage.RenderTarget) != 0)
        {
            ReleaseCom(ref DxRtv);
        }

        if ((usage & TextureUsage.DepthStencil) != 0)
        {
            ReleaseCom(ref DxDsv);
        }

        if ((usage & TextureUsage.SampleBuffer) != 0)
        {
            ReleaseCom(ref DxSrv);
        }

        if ((usage & TextureUsage.ColorBuffer) != 0)
        {
            ReleaseCom(ref DxUav);
        }
    }

    private ComPtr<ID3D11RenderTargetView> CreateRtv(Dx11Texture texture)
    {
        var description = new RenderTargetViewDesc
        {
            Format = Dx11Utilities.GetFormat(texture.Description.Format, false),
        };

        var usage = texture.Description.Usage;
        var samples = texture.Description.Samples;

        var arrayStart = Description.ArrayStart;
        var arrayCount = Description.ArrayCount;
        var mipStart = Description.MipStart;

        if (arrayCount > 1 || (usage & TextureUsage.CubeMap) != 0)
        {
            if (samples == TextureSamples.X1)
            {
                description.ViewDimension = RtvDimension.Texture2Darray;
                description.Texture2DArray = new Tex2DArrayRtv
                {
                    ArraySize = 1,
                    FirstArraySlice = arrayStart,
                    MipSlice = mipStart,
                };
            }
            else
            {
                description.ViewDimension = RtvDimension.Texture2Dmsarray;
                description.Texture2DMSArray = new Tex2DmsArrayRtv
                {
                    ArraySize = 1,
                    FirstArraySlice = arrayStart,
                };
            }
        }
        else
        {
            if (samples == TextureSamples.X1)
            {
                description.ViewDimension = RtvDimension.Texture2D;
                description.Texture2D.MipSlice = mipStart;
            }
            else
            {
                description.ViewDimension = RtvDimension.Texture2Dms;
            }
        }

        ComPtr<ID3D11RenderTargetView> rtv = default;

        SilkMarshal.ThrowHResult(GraphicsDevice.NativeDevice.CreateRenderTargetView(
            texture.NativeResource, ref description, ref rtv));

        return rtv;
    }

    private ComPtr<ID3D11DepthStencilView> CreateDsv(Dx11Texture texture)
    {
        var description = new DepthStencilViewDesc
        {
            Format = Dx11Utilities.GetDepthFormat(texture.Description.Format),
        };

        var samples = texture.Description.Samples;

        var arrayStart = Description.ArrayStart;
        var arrayCount = Description.ArrayCount;
        var mipStart = Description.MipStart;

        if (arrayCount == 1)
        {
            if (samples == TextureSamples.X1)
            {
                description.ViewDimension = DsvDimension.Texture2D;
                description.Texture2D.MipSlice = mipStart;
            }
            else
            {
                description.ViewDimension = DsvDimension.Texture2Dms;
            }
        }
        else
        {
            if (samples == TextureSamples.X1)
            {
                description.ViewDimension = DsvDimension.Texture2Darray;
                description.Texture2DArray.FirstArraySlice = arrayStart;
                description.Texture2DArray.ArraySize = 1;
                description.Texture2DArray.MipSlice = mipStart;
            }
            else
            {
                description.ViewDimension = DsvDimension.Texture2Dmsarray;
                description.Texture2DMSArray.FirstArraySlice = arrayStart;
                description.Texture2DMSArray.ArraySize = 1;
            }
        }

        ComPtr<ID3D11DepthStencilView> dsv = default;

        SilkMarshal.ThrowHResult(GraphicsDevice.NativeDevice.CreateDepthStencilView(
            texture.NativeResource, ref description, ref dsv));

        return dsv;
    }

    private ComPtr<ID3D11ShaderResourceView> CreateSrv(Dx11Texture texture)
    {
        var usage = texture.Description.Usage;

        var isDepth = (usage & TextureUsage.DepthStencil) != 0;

        var description = new ShaderResourceViewDesc
        {
            Format = Dx11Utilities.GetViewFormat(Dx11Utilities.GetFormat(texture.Description.Format, isDepth)),
        };

        var dimension = texture.Description.Dimension;

        var arrayCount = Description.ArrayCount;
        var arrayStart = Description.ArrayStart;
        var mipCount = Description.MipCount;
        var mipStart = Description.MipStart;

        if ((usage & TextureUsage.CubeMap) != 0)
        {
            if (arrayCount > 1)
            {
                description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexturecubearray;
                description.TextureCubeArray.NumCubes = arrayCount;
                description.TextureCubeArray.First2DArrayFace = arrayStart;
                description.TextureCubeArray.MipLevels = mipCount;
                description.TextureCubeArray.MostDetailedMip = mipStart;
            }
            else
            {
                description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexturecube;
                description.TextureCube.MipLevels = mipCount;
                description.TextureCube.MostDetailedMip = mipStart;
            }
        }
        else if (texture.Description.Depth == 1)
        {
            if (arrayCount > 1)
            {
                if (dimension == TextureDimension.Texture1D)
                {
                    description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexture1Darray;
                    description.Texture1DArray.ArraySize = arrayCount;
                    description.Texture1DArray.FirstArraySlice = arrayStart;
                    description.Texture1DArray.MipLevels = mipCount;
                    description.Texture1DArray.MostDetailedMip = mipStart;
                }
                else
                {
                    description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexture2Darray;
                    description.Texture2DArray.ArraySize = arrayCount;
                    description.Texture2DArray.FirstArraySlice = arrayStart;
                    description.Texture2DArray.MipLevels = mipCount;
                    description.Texture2DArray.MostDetailedMip = mipStart;
                }
            }
            else
            {
                if (dimension == TextureDimension.Texture1D)
                {
                    description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexture1D;
                    description.Texture1D.MipLevels = mipCount;
                    description.Texture1D.MostDetailedMip = mipStart;
                }
                else
                {
                    description.ViewDimension = texture.Description.Samples == TextureSamples.X1
                        ? D3DSrvDimension.D3D11SrvDimensionTexture2D
                        : D3DSrvDimension.D3D11SrvDimensionTexture2Dms;
                    description.Texture2D.MipLevels = mipCount;
                    description.Texture2D.MostDetailedMip = mipStart;
                }
            }
        }
        else
        {
            description.ViewDimension = D3DSrvDimension.D3D11SrvDimensionTexture3D;
            description.Texture3D.MipLevels = mipCount;
            description.Texture3D.MostDetailedMip = mipStart;
        }

        ComPtr<ID3D11ShaderResourceView> srv = default;

        SilkMarshal.ThrowHResult(GraphicsDevice.NativeDevice.CreateShaderResourceView(
            texture.NativeResource, ref description, ref srv));

        return srv;
    }

    private ComPtr<ID3D11UnorderedAccessView> CreateUav(Dx11Texture texture)
    {
        var description = new UnorderedAccessViewDesc
        {
            Format = Dx11Utilities.GetViewFormat(texture.NativeFormat),
        };

        var dimension = texture.Description.Dimension;
        var usage = texture.Description.Usage;

        var arrayStart = Description.ArrayStart;
        var arrayCount = Description.ArrayCount;
        var mipStart = Description.MipStart;

        if ((usage & TextureUsage.CubeMap) != 0)
        {
            throw new NotSupportedException();
        }
        else if (texture.Description.Depth == 1)
        {
            if (texture.Description.ArraySize == 1)
            {
                if (dimension == TextureDimension.Texture1D)
                {
                    description.ViewDimension = UavDimension.Texture1D;
                    description.Texture1D.MipSlice = mipStart;
                }
                else
                {
                    description.ViewDimension = UavDimension.Texture2D;
                    description.Texture2D.MipSlice = mipStart;
                }
            }
            else
            {
                if (dimension == TextureDimension.Texture1D)
                {
                    description.ViewDimension = UavDimension.Texture1Darray;
                    description.Texture1DArray.FirstArraySlice = arrayStart;
                    description.Texture1DArray.ArraySize = arrayCount;
                    description.Texture1DArray.MipSlice = mipStart;
                }
                else
                {
                    description.ViewDimension = UavDimension.Texture2Darray;
                    description.Texture2DArray.FirstArraySlice = arrayStart;
                    description.Texture2DArray.ArraySize = arrayCount;
                    description.Texture3D.MipSlice = mipStart;
                }
            }
        }
        else
        {
            description.ViewDimension = UavDimension.Texture3D;
            description.Texture3D.FirstWSlice = 0;
            description.Texture3D.WSize = texture.Description.Depth;
            description.Texture3D.MipSlice = mipStart;
        }

        ComPtr<ID3D11UnorderedAccessView> uav = default;

        SilkMarshal.ThrowHResult(GraphicsDevice.NativeDevice.CreateUnorderedAccessView(
            texture.NativeResource, ref description, ref uav));

        return uav;
    }
}
