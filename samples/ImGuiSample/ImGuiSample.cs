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
using Hybrid.ImGui;
using Hybrid.Numerics;
using ImGuiNET;
using SampleFramework;

namespace ImGuiSample;

internal sealed class ImGuiSample : Sample
{
    private ImGuiRenderer imGuiRenderer = default!;

    protected override void OnInitialize()
    {
        // Set any needed ImGui options
        var options = new ImGuiOptions
        {
            InitialSize = new Size(SwapChain.Width, SwapChain.Height),
        };

        // Create the ImGui renderer
        imGuiRenderer = ImGuiRenderer.Create(GraphicsDevice, options);
    }

    protected override void OnUpdate(GameTime time)
    {
        // Update the ImGui context
        imGuiRenderer.Update(time.DeltaTime);
    }

    protected override void OnRender(GameTime time)
    {
        // Show the ImGui demo window
        ImGui.ShowDemoWindow();

        // Get the graphics command queue
        var commandQueue = GraphicsDevice.GraphicsQueue;

        // Allocate a command list via the queue
        var commandList = commandQueue.Allocate();

        // Fill the swapchain buffer with a provided color
        commandList.ClearRenderTarget(SwapChain, in Color.Black);

        // Set the render target of the command list to the window swapchain
        commandList.SetRenderTarget(SwapChain);

        // Draw the ImGui geometry to the window
        imGuiRenderer.Draw(commandList);

        // Submit the command list to the GPU
        commandQueue.Execute(commandList);
    }

    protected override void OnResize(int width, int height)
    {
        // Resize the ImGui context when the window resizes
        imGuiRenderer.Resize(new Size(width, height));
    }
}
