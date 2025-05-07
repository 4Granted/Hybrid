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

namespace Hybrid.Graphics;

/// <summary>
/// Represents a swapchain contract.
/// </summary>
public interface ISwapChain : IRenderTarget2D
{
    /// <summary>
    /// An event invoked upon the swapchain surface being resized.
    /// </summary>
    public event Action<int, int>? OnResize;

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
