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

using Hybrid.Graphics.Shaders;
using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics;

public struct ComputePipeline : IDistinct<ComputePipeline>
{
    /// <summary>
    /// The default compute pipeline.
    /// </summary>
    public static readonly ComputePipeline Default = new();

    /// <summary>
    /// The descriptor layouts of the compute pipeline.
    /// </summary>
    public DescriptorLayout[] DescriptorLayouts;

    /// <summary>
    /// The compute shader of the compute pipeline.
    /// </summary>
    /// <remarks>
    /// When executing a compute pipeline, a compute shader is required.
    /// </remarks>
    public ComputeShader ComputeShader;

    /// <summary>
    /// The width of the thread group.
    /// </summary>
    public int ThreadGroupWidth;

    /// <summary>
    /// The height of the thread group.
    /// </summary>
    public int ThreadGroupHeight;

    /// <summary>
    /// The depth of the thread group.
    /// </summary>
    public int ThreadGroupDepth;

    /// <inheritdoc/>
    public readonly bool Equals(ComputePipeline other)
        => other.ComputeShader?.Id == ComputeShader?.Id
        && other.ThreadGroupWidth == ThreadGroupWidth
        && other.ThreadGroupHeight == ThreadGroupHeight
        && other.ThreadGroupDepth == ThreadGroupDepth;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is GraphicsPipelineDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        DescriptorLayouts, ComputeShader, ThreadGroupWidth,
        ThreadGroupHeight, ThreadGroupDepth);

    /// <inheritdoc/>
    public static bool operator ==(ComputePipeline left, ComputePipeline right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(ComputePipeline left, ComputePipeline right) => !left.Equals(right);
}
