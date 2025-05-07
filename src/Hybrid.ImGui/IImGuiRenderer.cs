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
using Hybrid.Graphics.Textures;
using Hybrid.Numerics;

namespace Hybrid.ImGui;

/// <summary>
/// Represents an ImGui renderer.
/// </summary>
public interface IImGuiRenderer : IDisposable
{
    /// <summary>
    /// Updates the interaction state of the ImGui renderer.
    /// </summary>
    /// <param name="deltaTime">The time since the last frame.</param>
    public void Update(float deltaTime);

    /// <summary>
    /// Records the commands necessary to render ImGui geometry.
    /// </summary>
    /// <param name="commandList">The command list to record on.</param>
    public void Draw(CommandList commandList);

    /// <summary>
    /// Includes <paramref name="texture"/> for rendering.
    /// </summary>
    /// <param name="texture">The texture to include.</param>
    public void Import(Texture2D texture);

    /// <summary>
    /// Resizes the renderer.
    /// </summary>
    /// <param name="size">The new size of the renderer.</param>
    public void Resize(Size size);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetMousePosition(float x, float y);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isDown"></param>
    public void SetMouseState(int index, bool isDown);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void SetMouseScroll(float amount);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isDown"></param>
    public void SetKeyboardState(int index, bool isDown);
}
