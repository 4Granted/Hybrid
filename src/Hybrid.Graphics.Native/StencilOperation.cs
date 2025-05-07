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
/// Defines the stencil operation of a <see cref="StencilBehavior"/>.
/// </summary>
public enum StencilOperation
{
    /// <summary>
    /// Keep.
    /// </summary>
    Keep,

    /// <summary>
    /// Zero.
    /// </summary>
    Zero,

    /// <summary>
    /// Replace.
    /// </summary>
    Replace,

    /// <summary>
    /// Increment.
    /// </summary>
    Increment,

    /// <summary>
    /// Increment saturation.
    /// </summary>
    IncrementSaturation,

    /// <summary>
    /// Decrement
    /// </summary>
    Decrement,

    /// <summary>
    /// Decrement saturation.
    /// </summary>
    DecrementSaturation,

    /// <summary>
    /// Invert.
    /// </summary>
    Invert,
}
