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

/// <summary>
/// Represents a pixel shader.
/// </summary>
/// <param name="graphicsDevice">The graphics device of the shader.</param>
/// <param name="source">The source of the shader.</param>
/// <param name="entrypoint">The entrypoint of the shader.</param>
/// <remarks>
/// The <paramref name="entrypoint"/> will default to <c>PSMain</c> when null.
/// </remarks>
public sealed class PixelShader(GraphicsDevice graphicsDevice,
    string source, string? entrypoint = null)
    : Shader(graphicsDevice, source, entrypoint)
{
    /// <inheritdoc/>
    public override ShaderStage Stage => ShaderStage.Pixel;
}
