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
using System.Diagnostics;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a graphics pipeline.
/// </summary>
public sealed class GraphicsPipeline : DeviceResource
{
    /// <summary>
    /// Gets the native pipeline.
    /// </summary>
    /// <remarks>
    /// If pipeline properties are changed, but never used in a SetPipeline
    /// call, then the pipeline implementation will not reflect the changes.
    /// </remarks>
    public IPipelineImpl Impl => pipelineImpl ?? throw new InvalidOperationException();

    /// <summary>
    /// Gets or sets the rasterizer state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how geometry is rasterized during
    /// execution of the pipeline, e.g. face culling
    /// or vertex winding order.
    /// </remarks>
    public RasterizerState RasterizerState { get; set; }

    /// <summary>
    /// Gets or sets the blend state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how fragments are blended during
    /// execution of the pipeline.
    /// </remarks>
    public BlendState BlendState { get; set; }

    /// <summary>
    /// Gets or sets the depth state of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// Determines how geometry is ordered during
    /// execution of the pipeline.
    /// </remarks>
    public DepthStencilState DepthStencilState { get; set; }

    /// <summary>
    /// Gets or sets the descriptor layouts of the graphics pipeline.
    /// </summary>
    public DescriptorLayout[] DescriptorLayouts { get; set; }

    /// <summary>
    /// Gets or sets the vertex shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a vertex shader is required.
    /// </remarks>
    public VertexShader? VertexShader { get; set; }

    /// <summary>
    /// Gets or sets the hull shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a hull shader is optional.
    /// </remarks>
    public GeometryShader? HullShader { get; set; }

    /// <summary>
    /// Gets or sets the domain shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a domain shader is optional.
    /// </remarks>
    public GeometryShader? DomainShader { get; set; }

    /// <summary>
    /// Gets or sets the geometry shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a geometry shader is optional.
    /// </remarks>
    public GeometryShader? GeometryShader { get; set; }

    /// <summary>
    /// Gets or sets the pixel shader of the graphics pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a graphics pipeline, a pixel shader is required.
    /// </remarks>
    public PixelShader? PixelShader { get; set; }

    /// <summary>
    /// Gets or sets the blend factor of the pipeline.
    /// </summary>
    public Color BlendFactor { get; set; }

    /// <summary>
    /// Gets or sets the primitive topology of the pipeline.
    /// </summary>
    public PrimitiveTopology Topology { get; set; }

    /// <summary>
    /// Gets whether the graphics pipeline is initialized.
    /// </summary>
    public bool IsInitialized => pipelineImpl != null;

    private IPipelineImpl? pipelineImpl;
    private IDescriptorLayoutImpl[]? layoutImpls;
    private VertexDeclaration cachedLayout;

    public GraphicsPipeline(
        GraphicsDevice graphicsDevice,
        RasterizerState? rasterizerState = null,
        BlendState? blendState = null,
        DepthStencilState? depthStencilState = null,
        DescriptorLayout[]? descriptorLayouts = null,
        VertexShader? vertexShader = null,
        GeometryShader? hullShader = null,
        GeometryShader? domainShader = null,
        GeometryShader? geometryShader = null,
        PixelShader? pixelShader = null,
        Color? blendFactor = null,
        PrimitiveTopology? topology = null)
        : base(graphicsDevice)
    {
        RasterizerState = rasterizerState ?? RasterizerState.Default;
        BlendState = blendState ?? BlendState.Default;
        DepthStencilState = depthStencilState ?? DepthStencilState.Default;
        DescriptorLayouts = descriptorLayouts ?? [];
        VertexShader = vertexShader;
        HullShader = hullShader;
        DomainShader = domainShader;
        GeometryShader = geometryShader;
        PixelShader = pixelShader;
        BlendFactor = blendFactor ?? Color.White;
        Topology = topology ?? PrimitiveTopology.TriangleList;
    }

    internal void Initialize(Span<VertexLayout> layouts)
    {
        Debug.Assert(VertexShader != null);
        Debug.Assert(PixelShader != null);

        layoutImpls ??= new IDescriptorLayoutImpl[DescriptorLayouts.Length];

        CommonExtensions.EnsureSize(ref layoutImpls, DescriptorLayouts.Length);

        for (int i = 0; i < DescriptorLayouts.Length; i++)
        {
            var layout = DescriptorLayouts[i];

            layoutImpls[i] = layout.Impl;
        }

        cachedLayout = new VertexDeclaration(layouts);

        var desc = new PipelineDescription
        {
            RasterizerState = RasterizerState,
            BlendState = BlendState,
            DepthStencilState = DepthStencilState,
            DescriptorLayouts = layoutImpls,
            VertexLayout = cachedLayout,
            VertexShader = VertexShader.Impl,
            GeometryShader = GeometryShader?.Impl,
            PixelShader = PixelShader.Impl,
            BlendFactor = BlendFactor,
            Topology = Topology,
        };

        pipelineImpl = GraphicsDevice.GetOrCreatePipeline(ref desc);
    }
}
