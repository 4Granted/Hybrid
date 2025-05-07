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

internal sealed class Dx11DescriptorLayout : Dx11DeviceResource, IDescriptorLayoutImpl
{
    public DescriptorLayoutDescription Description { get; }

    internal readonly DescriptorLayoutElement[] Elements;
    internal readonly uint Constants, Graphics, Compute, Samplers;

    internal Dx11DescriptorLayout(
        Dx11GraphicsDevice graphicsDevice,
        ref DescriptorLayoutDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        Elements = description.Elements;

        for (int i = 0; i < Elements.Length; i++)
        {
            var element = Elements[i];

            switch (element.Type)
            {
                case DescriptorType.ConstantResource:
                    Constants++;
                    break;
                case DescriptorType.GraphicsResource:
                    Graphics++;
                    break;
                case DescriptorType.ComputeResource:
                    Compute++;
                    break;
                case DescriptorType.SamplerResource:
                    Samplers++;
                    break;
            }
        }
    }
}
