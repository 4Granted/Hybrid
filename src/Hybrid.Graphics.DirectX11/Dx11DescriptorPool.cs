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

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11DescriptorPool : Dx11DeviceResource, IDescriptorPoolImpl
{
    public uint AllocationCount => (uint)allocationCount;

    internal readonly DescriptorSetEntry[] Descriptors;

    private int allocationCount;

    internal Dx11DescriptorPool(
        Dx11GraphicsDevice graphicsDevice,
        uint maximumDescriptors)
        : base(graphicsDevice)
    {
        Descriptors = new DescriptorSetEntry[maximumDescriptors];
    }

    public void Reset()
    {
        Array.Clear(Descriptors, 0, allocationCount);

        allocationCount = 0;
    }

    internal int Allocate(int count)
    {
        if (allocationCount + count > Descriptors.Length)
            return -1;

        return allocationCount += count;
    }
}
