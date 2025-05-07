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

internal sealed class Dx11CommandQueue : Dx11DeviceResource, ICommandQueueImpl
{
    // Do not release this here - let the graphics device handle that
    internal readonly ComPtr<ID3D11DeviceContext4> DxImmediateContext;

    private readonly Lock contextLock = new();

    internal Dx11CommandQueue(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
    {
        DxImmediateContext = graphicsDevice.NativeImmediateContext;
    }

    public void Execute(ICommandListImpl commandList, IFenceImpl? fence)
    {
        lock (contextLock)
        {
            Utilities.AsOrThrow<ICommandListImpl, Dx11CommandList>(commandList, out var dxCommandList);

            DxImmediateContext.ExecuteCommandList(dxCommandList.NativeCommandList, false);

            dxCommandList.Complete();
        }

        if (fence is Dx11Fence dxFence)
        {
            dxFence.Set();
        }
    }
}
