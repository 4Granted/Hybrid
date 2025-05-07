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
/// Defines the dimensions of an <see cref="ITextureImpl"/>.
/// </summary>
public enum TextureDimension : byte
{
    /// <summary>
    /// A one-dimensional texture.
    /// </summary>
    Texture1D,

    /// <summary>
    /// A two-dimensional texture.
    /// </summary>
    Texture2D,

    /// <summary>
    /// A three-dimensional texture.
    /// </summary>
    Texture3D,
}
