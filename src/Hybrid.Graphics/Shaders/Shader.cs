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

namespace Hybrid.Graphics.Shaders;

public abstract class Shader : DeviceResource
{
    /// <summary>
    /// Gets the native shader.
    /// </summary>
    public IShaderImpl Impl { get; }

    /// <summary>
    /// Gets the pipeline stage of the shader.
    /// </summary>
    public abstract ShaderStage Stage { get; }

    private protected Shader( GraphicsDevice graphicsDevice,
        string source, string? entrypoint = null)
        : base(graphicsDevice)
    {
        var description = new ShaderDescription
        {
            Stage = Stage,
            Source = source,
            EntryPoint = GetEntryPoint(Stage, entrypoint),
        };

        Impl = Factory.CreateShader(ref description);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            Impl?.Dispose();
        }
    }

    private static string GetEntryPoint(ShaderStage stage, string? entrypoint)
    {
        if (!string.IsNullOrEmpty(entrypoint))
            return entrypoint;

        return stage switch
        {
            ShaderStage.Vertex => "VSMain",
            ShaderStage.Hull => "HSMain",
            ShaderStage.Domain => "DSMain",
            ShaderStage.Geometry => "GSMain",
            ShaderStage.Pixel => "PSMain",
            ShaderStage.Compute => "CSMain",
            _ => throw new NotImplementedException(),
        };
    }
}
