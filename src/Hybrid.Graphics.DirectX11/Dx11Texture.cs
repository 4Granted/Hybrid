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

internal sealed class Dx11Texture : Dx11NativeResource, ITextureImpl
{
    public ITextureViewImpl View
    {
        get
        {
            lock (textureViewLock)
            {
                var description = new TextureViewDescription
                {
                    Texture = this,
                    ArrayCount = Description.ArraySize,
                    ArrayStart = 0,
                    MipCount = Description.MipLevels,
                    MipStart = 0,
                };

                return textureView ??= new Dx11TextureView(GraphicsDevice, ref description);
            }
        }
    }
    public TextureDescription Description { get; }

    internal readonly Format NativeFormat;

    private readonly Lock textureViewLock = new();
    private Dx11TextureView? textureView;

    internal unsafe Dx11Texture(
        Dx11GraphicsDevice graphicsDevice,
        ref TextureDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        /*var sizeInBytes = 0;
        var pixelSizeInBytes = (int)TextureUtilities.GetSizeInBytes(Format);

        for (int i = 0; i < MipLevels; i++)
        {
            sizeInBytes += pixelSizeInBytes * ((int)Width >> i) * ((int)Height >> i);
        }

        SizeInBytes = (uint)sizeInBytes;*/

        var isDepth = (description.Usage & TextureUsage.DepthStencil) != 0;

        NativeFormat = Dx11Utilities.GetFormat(description.Format, isDepth);

        var typelessFormat = Dx11Utilities.GetTypelessFormat(NativeFormat);

        var resourceUsage = Usage.Default;
        var bindFlags = BindFlag.None;
        var cpuAccessFlags = CpuAccessFlag.None;
        var miscFlags = ResourceMiscFlag.None;
        var arraySize = description.ArraySize;

        if ((description.Usage & TextureUsage.RenderTarget) != 0)
            bindFlags |= BindFlag.RenderTarget;

        if ((description.Usage & TextureUsage.DepthStencil) != 0)
            bindFlags |= BindFlag.DepthStencil;

        if ((description.Usage & TextureUsage.SampleBuffer) != 0)
            bindFlags |= BindFlag.ShaderResource;

        if ((description.Usage & TextureUsage.ColorBuffer) != 0)
            bindFlags |= BindFlag.UnorderedAccess;

        if ((description.Usage & TextureUsage.CopyBuffer) != 0)
        {
            cpuAccessFlags |= CpuAccessFlag.Read | CpuAccessFlag.Write;
            resourceUsage = Usage.Staging;
        }

        if ((description.Usage & TextureUsage.GenerateMipmaps) != 0)
        {
            bindFlags |= BindFlag.RenderTarget | BindFlag.ShaderResource;
            miscFlags |= ResourceMiscFlag.GenerateMips;
        }

        if ((description.Usage & TextureUsage.CubeMap) != 0)
        {
            miscFlags |= ResourceMiscFlag.Texturecube;

            arraySize *= 6;
        }

        switch (description.Dimension)
        {
            case TextureDimension.Texture1D:
                {
                    var desc = new Texture1DDesc
                    {
                        Format = typelessFormat,
                        Usage = resourceUsage,
                        BindFlags = (uint)bindFlags,
                        CPUAccessFlags = (uint)cpuAccessFlags,
                        MiscFlags = (uint)miscFlags,
                        Width = description.Width,
                        ArraySize = arraySize,
                        MipLevels = description.MipLevels,
                    };

                    ComPtr<ID3D11Texture1D> tex = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateTexture1D(ref desc, null, ref tex));
                    SilkMarshal.ThrowHResult(tex.QueryInterface(out NativeResource));

                    tex.Dispose();
                }
                break;
            case TextureDimension.Texture2D:
                {
                    var desc = new Texture2DDesc
                    {
                        Format = typelessFormat,
                        Usage = resourceUsage,
                        BindFlags = (uint)bindFlags,
                        CPUAccessFlags = (uint)cpuAccessFlags,
                        MiscFlags = (uint)miscFlags,
                        SampleDesc = new SampleDesc(description.Samples switch
                        {
                            TextureSamples.X1 => 1,
                            TextureSamples.X2 => 2,
                            TextureSamples.X4 => 4,
                            TextureSamples.X8 => 8,
                            TextureSamples.X16 => 16,
                            _ => throw new InvalidOperationException(),
                        }, 0),
                        Width = description.Width,
                        Height = description.Height,
                        ArraySize = arraySize,
                        MipLevels = description.MipLevels,
                    };

                    ComPtr<ID3D11Texture2D> tex = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateTexture2D(ref desc, null, ref tex));
                    SilkMarshal.ThrowHResult(tex.QueryInterface(out NativeResource));

                    tex.Dispose();
                }
                break;
            case TextureDimension.Texture3D:
                {
                    var desc = new Texture3DDesc
                    {
                        Format = typelessFormat,
                        Usage = resourceUsage,
                        BindFlags = (uint)bindFlags,
                        CPUAccessFlags = (uint)cpuAccessFlags,
                        MiscFlags = (uint)miscFlags,
                        Width = description.Width,
                        Height = description.Height,
                        Depth = description.Depth,
                        MipLevels = description.MipLevels,
                    };

                    ComPtr<ID3D11Texture3D> tex = default;

                    SilkMarshal.ThrowHResult(NativeDevice.CreateTexture3D(ref desc, null, ref tex));
                    SilkMarshal.ThrowHResult(tex.QueryInterface(out NativeResource));

                    tex.Dispose();
                }
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    internal Dx11Texture(
        Dx11GraphicsDevice graphicsDevice,
        ComPtr<ID3D11Texture2D> source)
        : base(graphicsDevice)
    {
        Texture2DDesc desc = default;

        source.GetDesc(ref desc);

        Description = new TextureDescription
        {
            Dimension = TextureDimension.Texture2D,
            Usage = TextureUsage.RenderTarget,
            Format = TextureFormat.Rgba8UNorm, // TODO
            Samples = TextureSamples.X1, // TODO
            Width = desc.Width,
            Height = desc.Height,
            Depth = 1,
            ArraySize = desc.ArraySize,
            MipLevels = desc.MipLevels,
        };

        SilkMarshal.ThrowHResult(source.QueryInterface(out NativeResource));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            CommonExtensions.Dispose(ref textureView);
        }

        ReleaseCom(ref NativeResource);
    }
}
