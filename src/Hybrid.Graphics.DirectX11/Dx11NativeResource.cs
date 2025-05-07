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

internal abstract class Dx11NativeResource : Dx11DeviceResource, IGpuResource
{
    internal ComPtr<ID3D11Resource> NativeResource;

    private protected Dx11NativeResource(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice) { }

    protected override void Dispose(bool disposing)
        => ReleaseCom(ref NativeResource);
}
