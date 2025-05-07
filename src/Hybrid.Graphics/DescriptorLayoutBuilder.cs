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

namespace Hybrid.Graphics;

public sealed class DescriptorLayoutBuilder
{
    private static readonly Queue<DescriptorLayoutBuilder> available = [];

    private readonly List<DescriptorLayoutElement> elements;
    private int constants, graphics, compute, samplers;

    private DescriptorLayoutBuilder() => elements = [];

    public DescriptorLayout Build(GraphicsDevice graphicsDevice)
    {
        var description = new DescriptorLayoutDescription
        {
            Elements = [.. elements],
        };

        elements.Clear();

        constants = graphics = compute = samplers = 0;

        available.Enqueue(this);

        return new DescriptorLayout(graphicsDevice, ref description);
    }

    public DescriptorLayoutBuilder AddDescriptor(
        DescriptorType type, ShaderStage stage, int index = -1)
    {
        var slot = type switch
        {
            DescriptorType.ConstantResource => constants++,
            DescriptorType.GraphicsResource => graphics++,
            DescriptorType.ComputeResource => compute++,
            DescriptorType.SamplerResource => samplers++,
            _ => index,
        };

        var element = new DescriptorLayoutElement
        {
            Type = type,
            Stage = stage,
            Index = (uint)(index < 0 ? slot : index),
        };

        elements.Add(element);

        return this;
    }

    public static DescriptorLayoutBuilder Create()
    {
        if (!available.TryDequeue(out var builder))
            builder = new DescriptorLayoutBuilder();

        return builder;
    }
}
