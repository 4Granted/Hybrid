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
using System.Diagnostics;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11DescriptorSet : Dx11DeviceResource, IDescriptorSetImpl
{
    IDescriptorLayoutImpl IDescriptorSetImpl.Layout => Layout;

    internal readonly Dx11DescriptorLayout Layout;
    internal readonly DescriptorSetEntry[] Entries;
    internal readonly Dx11NativeResource[] Resources;

    internal Dx11DescriptorSet(
        Dx11GraphicsDevice graphicsDevice,
        Dx11DescriptorLayout layout)
        : base(graphicsDevice)
    {
        Layout = layout;
        Entries = new DescriptorSetEntry[layout.Elements.Length];
        Resources = new Dx11NativeResource[layout.Elements.Length];
    }

    public void SetResource(int index, IGpuResource? resource)
    {
        Debug.Assert(index >= 0 && index < Entries.Length);

        ref var entry = ref Entries[index];

        if (resource == null)
        {
            entry = default;
        }
        else if (entry.Resource?.Id != resource.Id)
        {
            entry.Resource = resource;
            entry.Index = (uint)index;

            CommonExtensions.AsOrThrow(resource, out Dx11NativeResource dxResource);

            Resources[index] = dxResource;
        }
    }
}
