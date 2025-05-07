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
/// Defines the blending factor of a <see cref="BlendTarget"/>.
/// </summary>
public enum BlendFactor
{
    /// <summary>
    /// Zero.
    /// </summary>
    Zero,

    /// <summary>
    /// One.
    /// </summary>
    One,

    /// <summary>
    /// Source color.
    /// </summary>
    SourceColor,

    /// <summary>
    /// Inverse source color.
    /// </summary>
    InverseSourceColor,

    /// <summary>
    /// Source alpha.
    /// </summary>
    SourceAlpha,

    /// <summary>
    /// Inverse source alpha.
    /// </summary>
    InverseSourceAlpha,

    /// <summary>
    /// Destination color.
    /// </summary>
    DestinationColor,

    /// <summary>
    /// Inverse destination color.
    /// </summary>
    InverseDestinationColor,

    /// <summary>
    /// Destination alpha.
    /// </summary>
    DestinationAlpha,

    /// <summary>
    /// Inverse destination alpha.
    /// </summary>
    InverseDestinationAlpha,

    /// <summary>
    /// Factor.
    /// </summary>
    Factor,

    /// <summary>
    /// Inverse factor.
    /// </summary>
    InverseFactor,
}
