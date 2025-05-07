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

namespace Hybrid.Graphics.Textures;

/// <summary>
/// Defines the faces of a cube.
/// </summary>
public enum CubeFace
{
    /// <summary>
    /// No faces.
    /// </summary>
    None = 0,

    /// <summary>
    /// The positive X face (1, 0, 0) of a cube.
    /// </summary>
    PositiveX = 1 << 0,

    /// <summary>
    /// The negative X face (-1, 0, 0) of a cube.
    /// </summary>
    NegativeX = 1 << 1,

    /// <summary>
    /// The positive X face (0, 1, 0) of a cube.
    /// </summary>
    PositiveY = 1 << 2,

    /// <summary>
    /// The negative X face (0, -1, 0) of a cube.
    /// </summary>
    NegativeY = 1 << 3,

    /// <summary>
    /// The positive X face (0, 0, 1) of a cube.
    /// </summary>
    PositiveZ = 1 << 4,

    /// <summary>
    /// The negative X face (0, 0, -1) of a cube.
    /// </summary>
    NegativeZ = 1 << 5,

    /// <summary>
    /// The positive and negative X faces.
    /// </summary>
    AxisX = PositiveX | NegativeX,

    /// <summary>
    /// The positive and negative Y faces.
    /// </summary>
    AxisY = PositiveY | NegativeY,

    /// <summary>
    /// The positive and negative Z faces.
    /// </summary>
    AxisZ = PositiveZ | NegativeZ,

    /// <summary>
    /// All faces.
    /// </summary>
    All = AxisX | AxisY | AxisZ,
}
