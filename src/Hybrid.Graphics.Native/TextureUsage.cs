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
/// Defines the usage of an <see cref="ITextureImpl"/>.
/// </summary>
[Flags]
public enum TextureUsage
{
    /// <summary>
    /// A texture used as a render target in a graphics pipeline.
    /// </summary>
    RenderTarget = 1 << 0,

    /// <summary>
    /// A texture used as a depth stencil in a graphics pipeline.
    /// </summary>
    DepthStencil = 1 << 1,

    /// <summary>
    /// A texture used as a sample buffer in a graphics pipeline.
    /// </summary>
    SampleBuffer = 1 << 2,

    /// <summary>
    /// A texture used as a color buffer in a compute pipeline.
    /// </summary>
    ColorBuffer = 1 << 3,

    /// <summary>
    /// A texture used as a copy buffer in a graphics or compute pipeline.
    /// </summary>
    CopyBuffer = 1 << 4,

    /// <summary>
    /// Generate mipmaps.
    /// </summary>
    GenerateMipmaps = 1 << 5,

    /// <summary>
    /// Cube map.
    /// </summary>
    CubeMap = 1 << 7,
}
