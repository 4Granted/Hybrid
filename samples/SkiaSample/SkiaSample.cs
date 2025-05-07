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

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Skia;
using SampleFramework;
using SkiaSharp;

namespace SkiaSample;

internal sealed class SkiaSample : Sample
{
    private GraphicsPipeline? graphicsPipeline;
    private DescriptorSet descriptorSet = default!;
    private SkiaSurface surface = default!;

    protected override void OnInitialize()
    {
        var source = File.ReadAllText("Present.hlsl");

        var vertexShader = new VertexShader(GraphicsDevice, source);

        var pixelShader = new PixelShader(GraphicsDevice, source);

        var descriptorLayout = DescriptorLayoutBuilder.Create()
            .AddDescriptor(DescriptorType.GraphicsResource, ShaderStage.Pixel)
            .Build(GraphicsDevice);

        var graphicsPipelineDesc = GraphicsPipelineDescription.Default with
        {
            VertexShader = vertexShader,
            PixelShader = pixelShader,
            DescriptorLayouts = [descriptorLayout],
            RasterizerState = RasterizerState.Default with
            {
                WindingMode = WindingMode.CounterClockwise,
            },
        };

        graphicsPipeline = new GraphicsPipeline(GraphicsDevice, ref graphicsPipelineDesc);

        descriptorSet = new DescriptorSet(GraphicsDevice, descriptorLayout);

        surface = new SkiaSurface(GraphicsDevice, SwapChain.Width, SwapChain.Height);

        RenderSurface();
    }

    protected override void OnRender(GameTime time)
    {
        var commandQueue = GraphicsDevice.GraphicsQueue;

        var commandList = commandQueue.Allocate();

        commandList.ClearRenderTarget(SwapChain, in Color.Black);

        commandList.SetRenderTarget(SwapChain);

        commandList.SetDescriptorSet(0, descriptorSet);

        commandList.SetPipeline(graphicsPipeline);

        commandList.Draw(3);

        commandQueue.Execute(commandList);
    }

    protected override void OnResize(int width, int height)
    {
        surface.Resize(width, height);

        RenderSurface();
    }

    private void RenderSurface()
    {
        descriptorSet.SetTexture(0, surface.Image);

        var canvas = surface.Acquire();

        canvas.Clear(SKColors.White);

        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        using var font = new SKFont()
        {
            Size = 24
        };

        var coord = new SKPoint(SwapChain.Width / 2, (SwapChain.Height + font.Size) / 2);

        canvas.DrawText("SkiaSharp", coord, SKTextAlign.Center, font, paint);

        canvas.Flush();

        surface.Flush();
    }
}
