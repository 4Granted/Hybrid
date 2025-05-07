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

internal static class TextureUtilities
{
    public static void GetPitch(
        TextureFormat format, int width, int height,
        out int rowPitch, out int depthPitch)
    {
        rowPitch = GetRowPitch(format, width);
        depthPitch = GetDepthPitch(format, rowPitch, height);
    }

    public static int GetRowPitch(TextureFormat format, int width) => format switch
    {
        // TODO: Compressed formats
        _ => width * GetSizeInBytes(format),
    };

    public static int GetDepthPitch(
        TextureFormat format, int rowPitch, int height)
        => rowPitch * GetNumRows(format, height);

    public static int GetNumRows(TextureFormat format, int height) => format switch
    {
        // TODO: Compressed formats
        _ => height,
    };

    public static int GetSizeInBytes(TextureFormat format) => format switch
    {
        TextureFormat.R8UNorm or
        TextureFormat.R8SNorm or
        TextureFormat.R8UInt or
        TextureFormat.R8SInt => 1,

        TextureFormat.Rg8UNorm or
        TextureFormat.Rg8SNorm or
        TextureFormat.Rg8UInt or
        TextureFormat.Rg8SInt or
        TextureFormat.R16UNorm or
        TextureFormat.R16SNorm or
        TextureFormat.R16UInt or
        TextureFormat.R16SInt or
        TextureFormat.R16Float or
        TextureFormat.D16UNorm => 2,

        TextureFormat.Rg16UNorm or
        TextureFormat.Rg16SNorm or
        TextureFormat.Rg16UInt or
        TextureFormat.Rg16SInt or
        TextureFormat.Rg16Float or
        TextureFormat.R32UInt or
        TextureFormat.R32SInt or
        TextureFormat.R32Float or
        TextureFormat.Rgba8UNorm or
        TextureFormat.Rgba8UNormSrgb or
        TextureFormat.Rgba8SNorm or
        TextureFormat.Rgba8UInt or
        TextureFormat.Rgba8SInt or
        TextureFormat.R32UInt or
        TextureFormat.R32SInt or
        TextureFormat.R32Float or
        TextureFormat.D32Float or
        TextureFormat.D24UNormS8UInt => 4,

        TextureFormat.Rgba16UNorm or
        TextureFormat.Rgba16SNorm or
        TextureFormat.Rgba16UInt or
        TextureFormat.Rgba16SInt or
        TextureFormat.Rgba16Float or
        TextureFormat.Rg32UInt or
        TextureFormat.Rg32SInt or
        TextureFormat.Rg32Float => 8,

        TextureFormat.Rgb32UInt or
        TextureFormat.Rgb32SInt or
        TextureFormat.Rgb32Float => 12,

        TextureFormat.Rgba32UInt or
        TextureFormat.Rgba32SInt or
        TextureFormat.Rgba32Float => 16,

        _ => throw new InvalidOperationException(),
    };
}
