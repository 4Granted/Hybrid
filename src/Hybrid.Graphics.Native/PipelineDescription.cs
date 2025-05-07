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

using Hybrid.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents a description of an <see cref="IPipelineImpl"/>.
/// </summary>
public struct PipelineDescription : IDistinct<PipelineDescription>
{
    /// <summary>
    /// The rasterizer state of the pipeline.
    /// </summary>
    public RasterizerState RasterizerState;

    /// <summary>
    /// The blend state of the pipeline.
    /// </summary>
    public BlendState BlendState;

    /// <summary>
    /// The depth state of the pipeline.
    /// </summary>
    public DepthStencilState DepthStencilState;

    /// <summary>
    /// The vertex layout of the pipeline.
    /// </summary>
    public VertexDeclaration VertexLayout;

    /// <summary>
    /// The descriptor layout of the pipeline.
    /// </summary>
    public IDescriptorLayoutImpl[] DescriptorLayouts;

    /// <summary>
    /// The vertex shader of the pipeline.
    /// </summary>
    public IShaderImpl? VertexShader;

    /// <summary>
    /// The hull shader of the pipeline.
    /// </summary>
    public IShaderImpl? HullShader;

    /// <summary>
    /// The domain shader of the pipeline.
    /// </summary>
    public IShaderImpl? DomainShader;

    /// <summary>
    /// The geometry shader of the pipeline.
    /// </summary>
    public IShaderImpl? GeometryShader;

    /// <summary>
    /// The pixel shader of the pipeline.
    /// </summary>
    public IShaderImpl? PixelShader;

    /// <summary>
    /// The compute shader of the pipeline.
    /// </summary>
    public IShaderImpl? ComputeShader;

    /// <summary>
    /// The blend factor of the pipeline.
    /// </summary>
    public Color BlendFactor;

    /// <summary>
    /// The primitive topology of the pipeline.
    /// </summary>
    public PrimitiveTopology Topology;

    /// <inheritdoc/>
    public readonly bool Equals(PipelineDescription other)
        => other.RasterizerState == RasterizerState
        && other.BlendState == BlendState
        && other.DepthStencilState == DepthStencilState
        && other.VertexShader?.Id == VertexShader?.Id
        && other.HullShader?.Id == HullShader?.Id
        && other.DomainShader?.Id == DomainShader?.Id
        && other.GeometryShader?.Id == GeometryShader?.Id
        && other.PixelShader?.Id == PixelShader?.Id
        && other.ComputeShader?.Id == ComputeShader?.Id
        && other.BlendFactor == BlendFactor
        && other.Topology == Topology;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is PipelineDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() =>
        HashCode.Combine(RasterizerState, BlendState, DepthStencilState, VertexShader,
        HashCode.Combine(PixelShader, ComputeShader, BlendFactor, Topology));

    /// <inheritdoc/>
    public static bool operator ==(PipelineDescription left, PipelineDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(PipelineDescription left, PipelineDescription right) => !left.Equals(right);
}
