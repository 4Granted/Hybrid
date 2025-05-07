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
using Silk.NET.DXGI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11SwapChain : Dx11DeviceResource, ISwapChainImpl
{
    private const int FRAME_COUNT = 2;

    public ITextureImpl Texture => RenderTarget
        ?? throw new InvalidOperationException();
    public SwapChainDescription Description { get; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    internal ComPtr<IDXGISwapChain1> SwapChain;
    internal Dx11Texture? RenderTarget;

    internal unsafe Dx11SwapChain(
        Dx11GraphicsDevice graphicsDevice,
        ref SwapChainDescription description)
        : base(graphicsDevice)
    {
        Description = description;
        Width = (int)description.Width;
        Height = (int)description.Height;

        var desc = new SwapChainDesc1
        {
            Format = Dx11Utilities.GetFormat(description.Format, false),
            AlphaMode = AlphaMode.Ignore,
            Scaling = Scaling.None,
            SwapEffect = SwapEffect.FlipDiscard,
            BufferUsage = DXGI.UsageRenderTargetOutput,
            BufferCount = FRAME_COUNT,
            SampleDesc = new SampleDesc(1, 0),
            Width = description.Width,
            Height = description.Height,
            Stereo = false,
        };

        SilkMarshal.ThrowHResult(GraphicsDevice.NativeFactory.CreateSwapChainForHwnd(
            GraphicsDevice.NativeDevice, description.Handle, in desc, null,
            ref Unsafe.NullRef<IDXGIOutput>(), ref SwapChain));

        CreateSizeDependentResources();
    }

    public bool Present()
    {
        if (Width <= 0 || Height <= 0)
            return false;

        var result = SwapChain.Present(0, 0);

        return HResult.IndicatesSuccess(result);
    }

    public void Resize(int width, int height)
    {
        Debug.Assert(width > 0);
        Debug.Assert(height > 0);

        Width = width;
        Height = height;

        DisposeSizeDependentResources();

        SwapChainDesc1 desc = default;

        SilkMarshal.ThrowHResult(SwapChain.GetDesc1(ref desc));
        SilkMarshal.ThrowHResult(SwapChain.ResizeBuffers(
            desc.BufferCount, (uint)width, (uint)height,
            desc.Format, desc.Flags));

        CreateSizeDependentResources();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeSizeDependentResources();
        }

        ReleaseCom(ref SwapChain);
    }

    private unsafe void CreateSizeDependentResources()
    {
        using var texture = SwapChain.GetBuffer<ID3D11Texture2D>(0);

        RenderTarget = new Dx11Texture(GraphicsDevice, texture);
    }

    private void DisposeSizeDependentResources()
    {
        Utilities.Dispose(ref RenderTarget);
    }
}
