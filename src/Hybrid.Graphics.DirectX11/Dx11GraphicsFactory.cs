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

internal sealed class Dx11GraphicsFactory : Dx11DeviceResource, IGraphicsFactoryImpl
{
    private readonly Dx11StateCache stateCache;
    private readonly Dx11ShaderCompiler shaderCompiler;

    internal Dx11GraphicsFactory(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
    {
        stateCache = new Dx11StateCache(graphicsDevice);
        shaderCompiler = new Dx11ShaderCompiler(graphicsDevice);
    }

    public ISwapChainImpl CreateSwapChain(ref SwapChainDescription description)
        => new Dx11SwapChain(GraphicsDevice, ref description);

    public ICommandQueueImpl CreateCommandQueue()
        => new Dx11CommandQueue(GraphicsDevice);

    public ICommandListImpl CreateCommandList()
        => new Dx11CommandList(GraphicsDevice);

    public IPipelineImpl CreatePipeline(ref PipelineDescription description)
        => new Dx11Pipeline(GraphicsDevice, stateCache, ref description);

    public IDescriptorLayoutImpl CreateDescriptorLayout(ref DescriptorLayoutDescription description)
        => new Dx11DescriptorLayout(GraphicsDevice, ref description);

    public IDescriptorSetImpl CreateDescriptorSet(IDescriptorLayoutImpl layout)
    {
        Utilities.AsOrThrow(layout, out Dx11DescriptorLayout dxLayout);

        return new Dx11DescriptorSet(GraphicsDevice, dxLayout);
    }

    public IBufferImpl CreateBuffer(ref BufferDescription description)
        => new Dx11Buffer(GraphicsDevice, ref description);

    public ITextureImpl CreateTexture(ref TextureDescription description)
        => new Dx11Texture(GraphicsDevice, ref description);

    public ITextureViewImpl CreateTextureView(ref TextureViewDescription description)
        => new Dx11TextureView(GraphicsDevice, ref description);

    public ISamplerImpl CreateSampler(ref SamplerDescription description)
        => new Dx11Sampler(GraphicsDevice, ref description);

    public IShaderImpl CreateShader(ref ShaderDescription description)
    {
        var bytecode = shaderCompiler.Compile(ref description);

        Utilities.AsOrThrow(in bytecode, out Dx11ShaderBytecode dxBytecode);

        return new Dx11Shader(GraphicsDevice, dxBytecode, ref description);
    }

    public IFenceImpl CreateFence(bool isSignaled)
        => new Dx11Fence(GraphicsDevice, isSignaled);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            shaderCompiler?.Dispose();
            stateCache?.Dispose();
        }
    }
}
