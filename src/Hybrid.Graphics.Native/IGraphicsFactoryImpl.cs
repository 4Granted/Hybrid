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

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents an API specific graphics factory implementation.
/// </summary>
public interface IGraphicsFactoryImpl : IDeviceResource
{
    /// <summary>
    /// Creates an <see cref="ISwapChainImpl"/>.
    /// </summary>
    /// <param name="description">The description of the swapchain.</param>
    /// <returns>The created swapchain.</returns>
    public ISwapChainImpl CreateSwapChain(ref SwapChainDescription description);

    /// <summary>
    /// Creates an <see cref="ICommandQueueImpl"/>.
    /// </summary>
    /// <returns>The created command queue.</returns>
    public ICommandQueueImpl CreateCommandQueue();

    /// <summary>
    /// Creates an <see cref="ICommandListImpl"/>.
    /// </summary>
    /// <returns>The created command list.</returns>
    public ICommandListImpl CreateCommandList();

    /// <summary>
    /// Creates an <see cref="IPipelineImpl"/>.
    /// </summary>
    /// <param name="description">The description of the pipeline.</param>
    /// <returns>The created pipeline.</returns>
    public IPipelineImpl CreatePipeline(ref PipelineDescription description);

    /// <summary>
    /// Creates an <see cref="IDescriptorLayoutImpl"/>.
    /// </summary>
    /// <param name="description">The description of the descriptor layout.</param>
    /// <returns>The created descriptor layout.</returns>
    public IDescriptorLayoutImpl CreateDescriptorLayout(ref DescriptorLayoutDescription description);

    /// <summary>
    /// Creates an <see cref="IDescriptorSetImpl"/>.
    /// </summary>
    /// <param name="layout">The layout of the descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    public IDescriptorSetImpl CreateDescriptorSet(IDescriptorLayoutImpl layout);

    /// <summary>
    /// Creates an <see cref="IBufferImpl"/>.
    /// </summary>
    /// <param name="description">The description of the buffer.</param>
    /// <returns>The created buffer.</returns>
    public IBufferImpl CreateBuffer(ref BufferDescription description);

    /// <summary>
    /// Creates an <see cref="ITextureImpl"/>.
    /// </summary>
    /// <param name="description">The description of the texture.</param>
    /// <returns>The created texture.</returns>
    public ITextureImpl CreateTexture(ref TextureDescription description);

    /// <summary>
    /// Creates an <see cref="ITextureViewImpl"/>.
    /// </summary>
    /// <param name="description">The description of the texture view.</param>
    /// <returns>The created texture view.</returns>
    public ITextureViewImpl CreateTextureView(ref TextureViewDescription description);

    /// <summary>
    /// Creates an <see cref="ISamplerImpl"/>.
    /// </summary>
    /// <param name="description">The description of the sampler.</param>
    /// <returns>The created samplersampler.</returns>
    public ISamplerImpl CreateSampler(ref SamplerDescription description);

    /// <summary>
    /// Creates an <see cref="IShaderImpl"/>.
    /// </summary>
    /// <param name="description">The description of the shader.</param>
    /// <returns>The created shader.</returns>
    public IShaderImpl CreateShader(ref ShaderDescription description);

    /// <summary>
    /// Creates an <see cref="IFenceImpl"/>.
    /// </summary>
    /// <param name="isSignaled">Whether the fence is signaled.</param>
    /// <returns>The created fence.</returns>
    public IFenceImpl CreateFence(bool isSignaled);
}
