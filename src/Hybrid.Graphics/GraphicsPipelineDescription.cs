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
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics;

public struct GraphicsPipelineDescription : IDistinct<GraphicsPipelineDescription>
{
    /// <summary>
    /// The default graphics pipeline.
    /// </summary>
    public static readonly GraphicsPipelineDescription Default = new()
    {
        RasterizerState = RasterizerState.Default,
        BlendState = BlendState.Default,
        DepthStencilState = DepthStencilState.Default,
    };

    /// <summary>
    /// The rasterizer state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how geometry is rasterized during
    /// execution of the pipeline, e.g. face culling
    /// or vertex winding order.
    /// </remarks>
    public RasterizerState RasterizerState;

    /// <summary>
    /// The blend state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how fragments are blended during
    /// execution of the pipeline.
    /// </remarks>
    public BlendState BlendState;

    /// <summary>
    /// The depth state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how geometry is ordered during
    /// execution of the pipeline.
    /// </remarks>
    public DepthStencilState DepthStencilState;

    /// <summary>
    /// The descriptor layouts of the graphics pipeline.
    /// </summary>
    public DescriptorLayout[] DescriptorLayouts;

    /// <summary>
    /// The vertex layout of the graphics pipeline.
    /// </summary>
    public VertexDeclaration VertexLayout;

    /// <summary>
    /// The vertex shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a vertex shader is required.
    /// </remarks>
    public VertexShader VertexShader;

    /// <summary>
    /// The hull shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a hull shader is optional.
    /// </remarks>
    public GeometryShader? HullShader;

    /// <summary>
    /// The domain shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a domain shader is optional.
    /// </remarks>
    public GeometryShader? DomainShader;

    /// <summary>
    /// The geometry shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a geometry shader is optional.
    /// </remarks>
    public GeometryShader? GeometryShader;

    /// <summary>
    /// The pixel shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a pixel shader is required.
    /// </remarks>
    public PixelShader PixelShader;

    /// <summary>
    /// The blend factor of the pipeline.
    /// </summary>
    public Color BlendFactor;

    /// <summary>
    /// The primitive topology of the pipeline.
    /// </summary>
    public PrimitiveTopology Topology;

    /// <inheritdoc/>
    public readonly bool Equals(GraphicsPipelineDescription other)
        => other.RasterizerState == RasterizerState
        && other.BlendState == BlendState
        && other.DepthStencilState == DepthStencilState
        && other.VertexLayout == VertexLayout
        && other.VertexShader.Id == VertexShader.Id
        && other.HullShader?.Id == HullShader?.Id
        && other.DomainShader?.Id == DomainShader?.Id
        && other.GeometryShader?.Id == GeometryShader?.Id
        && other.PixelShader.Id == PixelShader.Id
        && other.BlendFactor == BlendFactor
        && other.Topology == Topology;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is GraphicsPipelineDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() =>
        HashCode.Combine(RasterizerState, BlendState, DepthStencilState, DescriptorLayouts,
        HashCode.Combine(VertexLayout, VertexShader, PixelShader, BlendFactor, Topology));

    /// <inheritdoc/>
    public static bool operator ==(GraphicsPipelineDescription left, GraphicsPipelineDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(GraphicsPipelineDescription left, GraphicsPipelineDescription right) => !left.Equals(right);
}
