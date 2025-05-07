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

using Hybrid.Graphics.Native;

namespace Hybrid.Graphics.Textures;

/// <summary>
/// Represents a texture cube.
/// </summary>
public class TextureCube : Texture, ITextureCube
{
    /// <summary>
    /// Constructs a texture cube.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the texture.</param>
    /// <param name="format">The format of the texture.</param>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="arraySize">The amount of array layers in the texture.</param>
    /// <param name="mipLevels">The amount of the mip levels in the texture.</param>
    /// <param name="usage">The usage of the texture.</param>
    /// <param name="samples">The amount of samples used by the texture.</param>
    /// <param name="access">The resource access of the texture.</param>
    public TextureCube(GraphicsDevice graphicsDevice,
        TextureFormat format, int width, int height,
        int arraySize = 1, int mipLevels = 1,
        TextureUsage usage = TextureUsage.SampleBuffer | TextureUsage.CubeMap,
        TextureSamples samples = TextureSamples.X1,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, TextureDimension.Texture2D, usage,
              format, samples, width, height, 1,
              arraySize, mipLevels, access) { }

    /// <summary>
    /// Resizes the texture.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    public void Resize(int width, int height)
    {
        var description = Description with
        {
            Width = (uint)width,
            Height = (uint)height,
        };

        InitializeUnsafe(ref description);
    }
}
