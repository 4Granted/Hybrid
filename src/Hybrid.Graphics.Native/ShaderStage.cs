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
/// Defines the stages of a <see cref="IShaderImpl"/>.
/// </summary>
public enum ShaderStage
{
    /// <summary>
    /// No stages.
    /// </summary>
    None = 0,

    /// <summary>
    /// The vertex stage.
    /// </summary>
    Vertex = 1 << 0,

    /// <summary>
    /// The hull stage.
    /// </summary>
    Hull = 1 << 1,

    /// <summary>
    /// The domain stage.
    /// </summary>
    Domain = 1 << 2,

    /// <summary>
    /// The geometry stage.
    /// </summary>
    Geometry = 1 << 3,

    /// <summary>
    /// The pixel stage.
    /// </summary>
    Pixel = 1 << 4,

    /// <summary>
    /// The compute stage.
    /// </summary>
    Compute = 1 << 5,

    /// <summary>
    /// All stages.
    /// </summary>
    All = Vertex | Hull | Domain | Geometry | Pixel,
}
