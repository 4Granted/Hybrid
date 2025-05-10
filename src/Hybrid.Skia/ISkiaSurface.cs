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

using Hybrid.Graphics.Textures;
using SkiaSharp;

namespace Hybrid.Skia;

/// <summary>
/// Represents a Skia surface.
/// </summary>
public interface ISkiaSurface : IDisposable
{
    /// <summary>
    /// Gets the image of the surface.
    /// </summary>
    public Texture2D Image { get; }

    /// <summary>
    /// Gets the width of the surface.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the surface.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Acquires the Skia canvas of the surface.
    /// </summary>
    /// <returns>The canvas.</returns>
    public SKCanvas Acquire();

    /// <summary>
    /// Draws the Skia canvas to the internal render target.
    /// </summary>
    public void Flush();

    /// <summary>
    /// Resizes the surface.
    /// </summary>
    /// <param name="width">The new width of the surface.</param>
    /// <param name="height">The new height of the surface.</param>
    public void Resize(int width, int height);
}
