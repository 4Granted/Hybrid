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
/// Defines the filtering mode of a <see cref="ISamplerImpl"/>.
/// </summary>
public enum TextureFilter : byte
{
    /// <summary>
    /// Point minification; point magnification; point mip.
    /// </summary>
    PointPointPoint,

    /// <summary>
    /// Point minification; point magnification; linear mip.
    /// </summary>
    PointPointLinear,

    /// <summary>
    /// Point minification; linear magnification; point mip.
    /// </summary>
    PointLinearPoint,

    /// <summary>
    /// Point minification; linear magnification; linear mip.
    /// </summary>
    PointLinearLinear,

    /// <summary>
    /// Linear minification; point magnification; point mip.
    /// </summary>
    LinearPointPoint,

    /// <summary>
    /// Linear minification; point magnification; linear mip.
    /// </summary>
    LinearPointLinear,

    /// <summary>
    /// Linear minification; linear magnification; point mip.
    /// </summary>
    LinearLinearPoint,

    /// <summary>
    /// Linear minification; linear magnification; linear mip.
    /// </summary>
    LinearLinearLinear,

    /// <summary>
    /// Anisotropic filtering.
    /// </summary>
    Anisotropic,
}
