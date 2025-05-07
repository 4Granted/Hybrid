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
/// Defines the sample count of an <see cref="ITextureImpl"/>
/// </summary>
public enum TextureSamples : uint
{
    /// <summary>
    /// Samples once per pixel.
    /// </summary>
    X1 = 1,

    /// <summary>
    /// Samples two times per pixel.
    /// </summary>
    X2 = 2,

    /// <summary>
    /// Samples four times per pixel.
    /// </summary>
    X4 = 4,

    /// <summary>
    /// Samples eight times per pixel.
    /// </summary>
    X8 = 8,

    /// <summary>
    /// Samples sixteen times per pixel.
    /// </summary>
    X16 = 16,
}
