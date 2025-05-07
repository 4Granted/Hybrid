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

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Textures;
using StbImageSharp;
using System.Diagnostics;

namespace SampleFramework;

/// <summary>
/// Represents a collection of extension methods used by the samples.
/// </summary>
public static class SampleExtensions
{
    /// <summary>
    /// Loads a texture from the <paramref name="stream"/>.
    /// </summary>
    /// <param name="self">The graphics device of the texture.</param>
    /// <param name="stream">The stream to load the texture from.</param>
    /// <param name="format">The format to load the texture as.</param>
    /// <returns>The texture.</returns>
    public static Texture2D FromStream(
        this GraphicsDevice self, Stream stream,
        TextureFormat format = TextureFormat.Rgba8UNorm)
    {
        Debug.Assert(self != null);
        Debug.Assert(stream != null);

        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        var bitmap = new Bitmap<byte>(image.Data, image.Width, image.Height, 4);

        return new Texture2D<byte>(self, format, bitmap);
    }
}
