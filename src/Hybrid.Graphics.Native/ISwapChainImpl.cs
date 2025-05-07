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
/// Represents an API specific swapchain implementation.
/// </summary>
public interface ISwapChainImpl : IDeviceResource
{
    /// <summary>
    /// Gets the texture of the swapchain.
    /// </summary>
    public ITextureImpl Texture { get; }

    /// <summary>
    /// Gets the description of the swapchain.
    /// </summary>
    public SwapChainDescription Description { get; }

    /// <summary>
    /// Gets the current width of the swapchain surface.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the current height of the swapchain surface.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Presents the swapchain to the surface.
    /// </summary>
    /// <returns>Whether the swapchain was presented.</returns>
    public bool Present();

    /// <summary>
    /// Resizes the swapchain surface.
    /// </summary>
    /// <param name="width">The new width of the surface.</param>
    /// <param name="height">The new height of the surface.</param>
    public void Resize(int width, int height);
}
