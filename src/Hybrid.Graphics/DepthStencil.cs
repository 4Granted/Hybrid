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
using Hybrid.Graphics.Textures;

namespace Hybrid.Graphics;

public class DepthStencil : Texture2D
{
    public DepthStencil(GraphicsDevice graphicsDevice, TextureFormat format,
        int width, int height, int arraySize = 1, int mipLevels = 1,
        TextureUsage usage = TextureUsage.DepthStencil,
        TextureSamples samples = TextureSamples.X1,
        ResourceAccess access = ResourceAccess.Default)
        : base(graphicsDevice, format, width, height, arraySize,
            mipLevels, usage, samples, access) { }
}
