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
/// Defines the supported graphics APIs.
/// </summary>
public enum GraphicsApi
{
    /// <summary>
    /// An unknwon graphics API.
    /// </summary>
    Unknown,

    /// <summary>
    /// The OpenGL graphics API.
    /// </summary>
    OpenGL,

    /// <summary>
    /// The Vulkan graphics API.
    /// </summary>
    Vulkan,

    /// <summary>
    /// The DirectX 11 graphics API.
    /// </summary>
    DirectX11,

    /// <summary>
    /// The DirectX 12 graphics API.
    /// </summary>
    DirectX12,

    /// <summary>
    /// The Metal graphics API.
    /// </summary>
    Metal,
}
