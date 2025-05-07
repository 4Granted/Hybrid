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

using Hybrid.Numerics;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Defines the channels of a <see cref="Color"/>.
/// </summary>
[Flags]
public enum ColorChannels : byte
{
    /// <summary>
    /// No color channels.
    /// </summary>
    None = 0,

    /// <summary>
    /// The red color channel.
    /// </summary>
    Red = 1 << 0,

    /// <summary>
    /// The green color channel.
    /// </summary>
    Green = 1 << 1,

    /// <summary>
    /// The blue color channel.
    /// </summary>
    Blue = 1 << 2,

    /// <summary>
    /// The alpha color channel.
    /// </summary>
    Alpha = 1 << 3,

    /// <summary>
    /// Red, green, and blue color channels.
    /// </summary>
    RGB = Red | Green | Blue,

    /// <summary>
    /// All color channels.
    /// </summary>
    All = Red | Green | Blue | Alpha,
}
