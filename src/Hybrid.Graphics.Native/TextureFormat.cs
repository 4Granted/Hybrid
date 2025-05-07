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
/// Defines the format of an <see cref="ITextureImpl"/>.
/// </summary>
public enum TextureFormat
{
    /// <summary>
    /// Unsigned 8-bit normalized single channel color.
    /// </summary>
    R8UNorm,

    /// <summary>
    /// Signed 8-bit normalized single channel color.
    /// </summary>
    R8SNorm,

    /// <summary>
    /// Unsigned 8-bit single channel color.
    /// </summary>
    R8UInt,

    /// <summary>
    /// Signed 8-bit single channel color.
    /// </summary>
    R8SInt,

    /// <summary>
    /// Unsigned 8-bit normalized two channel color.
    /// </summary>
    Rg8UNorm,

    /// <summary>
    /// Signed 8-bit normalized two channel color.
    /// </summary>
    Rg8SNorm,

    /// <summary>
    /// Unsigned 8-bit two channel color.
    /// </summary>
    Rg8UInt,

    /// <summary>
    /// Signed 8-bit two channel color.
    /// </summary>
    Rg8SInt,

    /// <summary>
    /// Unsigned 16-bit normalized single channel color.
    /// </summary>
    R16UNorm,

    /// <summary>
    /// Signed 16-bit normalized single channel color.
    /// </summary>
    R16SNorm,

    /// <summary>
    /// Unsigned 16-bit single channel color.
    /// </summary>
    R16UInt,

    /// <summary>
    /// Signed 16-bit single channel color.
    /// </summary>
    R16SInt,

    /// <summary>
    /// Floating point 16-bit single channel color.
    /// </summary>
    R16Float,

    /// <summary>
    /// Unsigned 16-bit normalized two channel color.
    /// </summary>
    Rg16UNorm,

    /// <summary>
    /// Signed 16-bit normalized two channel color.
    /// </summary>
    Rg16SNorm,

    /// <summary>
    /// Unsigned 16-bit two channel color.
    /// </summary>
    Rg16UInt,

    /// <summary>
    /// Signed 16-bit two channel color.
    /// </summary>
    Rg16SInt,

    /// <summary>
    /// Floating point 16-bit two channel color.
    /// </summary>
    Rg16Float,

    /// <summary>
    /// Unsigned 16-bit normalized depth value.
    /// </summary>
    D16UNorm,

    /// <summary>
    /// Unsigned 32-bit single channel color.
    /// </summary>
    R32UInt,

    /// <summary>
    /// Signed 32-bit single channel color.
    /// </summary>
    R32SInt,

    /// <summary>
    /// Floating point 32-bit single channel color.
    /// </summary>
    R32Float,

    /// <summary>
    /// Floating point 32-bit depth value.
    /// </summary>
    D32Float,

    /// <summary>
    /// Unsigned 24-bit normalized depth value and unsigned 8-bit stencil value.
    /// </summary>
    D24UNormS8UInt,

    /// <summary>
    /// Floating point 32-bit depth value and unsigned 8-bit stencil value.
    /// </summary>
    D32FloatS8UInt,

    /// <summary>
    /// Unsigned 8-bit four channel color.
    /// </summary>
    Rgba8UNorm,

    /// <summary>
    /// Unsigned 8-bit four channel color in the SRGB color-space.
    /// </summary>
    Rgba8UNormSrgb,

    /// <summary>
    /// Signed 8-bit normalized four channel color.
    /// </summary>
    Rgba8SNorm,

    /// <summary>
    /// Unsigned 8-bit four channel color.
    /// </summary>
    Rgba8UInt,

    /// <summary>
    /// Signed 8-bit four channel color.
    /// </summary>
    Rgba8SInt,

    /// <summary>
    /// Unsigned 16-bit four channel color.
    /// </summary>
    Rgba16UNorm,

    /// <summary>
    /// Signed 16-bit normalized four channel color.
    /// </summary>
    Rgba16SNorm,

    /// <summary>
    /// Unsigned 16-bit four channel color.
    /// </summary>
    Rgba16UInt,

    /// <summary>
    /// Signed 16-bit four channel color.
    /// </summary>
    Rgba16SInt,

    /// <summary>
    /// Floating point 16-bit four channel color.
    /// </summary>
    Rgba16Float,

    /// <summary>
    /// Unsigned 32-bit two channel color.
    /// </summary>
    Rg32UInt,

    /// <summary>
    /// Signed 32-bit two channel color.
    /// </summary>
    Rg32SInt,

    /// <summary>
    /// Floating point 32-bit two channel color.
    /// </summary>
    Rg32Float,

    /// <summary>
    /// Unsigned 32-bit three channel color.
    /// </summary>
    Rgb32UInt,

    /// <summary>
    /// Signed 32-bit three channel color.
    /// </summary>
    Rgb32SInt,

    /// <summary>
    /// Floating point 32-bit four channel color.
    /// </summary>
    Rgb32Float,

    /// <summary>
    /// Unsigned 32-bit four channel color.
    /// </summary>
    Rgba32UInt,

    /// <summary>
    /// Signed 32-bit four channel color.
    /// </summary>
    Rgba32SInt,

    /// <summary>
    /// Floating point 32-bit four channel color.
    /// </summary>
    Rgba32Float,
}
