// Hybrid - A versatile framework for application development.
// Copyright (C) 2024  Fielding Baran
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY- without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Hybrid.Framework;
using Hybrid.Graphics;
using Hybrid.Input;
using Hybrid.Rendering.ImGui;
using Hybrid.Services;
using ImGuiNET;
using Hybrid.Numerics;
using Sandbox.Galaxy;
using Hybrid.Reflection;

namespace Sandbox
{
    internal sealed class DebugLayer : Layer
    {
        private GraphicsDevice graphicsDevice = default!;
        private RenderContext renderContext = default!;
        private InputContext inputContext = default!;
        private GalaxyContext galaxyContext = default!;
        private RenderSettings renderSettings = default!;
        private SkyboxSettings skyboxSettings = default!;
        private GenerationSettings generationSettings = default!;
        private SimulationSettings simulationSettings = default!;
        private ImGuiRenderer renderer = default!;

        protected override void OnAttach(IServiceScope services)
        {
            graphicsDevice = services.GetRequiredService<GraphicsDevice>();
            renderContext = services.GetRequiredService<RenderContext>();
            inputContext = services.GetRequiredService<InputContext>();
            galaxyContext = services.GetRequiredService<GalaxyContext>();
            renderSettings = services.GetRequiredService<RenderSettings>();
            skyboxSettings = services.GetRequiredService<SkyboxSettings>();
            generationSettings = services.GetRequiredService<GenerationSettings>();
            simulationSettings = services.GetRequiredService<SimulationSettings>();

            renderContext.Compositor.AddLayer(OnDraw);

            renderContext.Compositor.OnResize += size => renderer.Resize(size);

            var options = new ImGuiOptions
            {
                InitialSize = renderContext.ScreenSize,
            };

            renderer = ImGuiRenderer.Create(graphicsDevice, options);
        }

        protected override void OnUpdate(GameTime time)
        {
            renderer.Update(inputContext, time.DeltaTime);
        }

        private void OnDraw(CommandList commandList)
        {
            ImGui.SetNextWindowSize(System.Numerics.Vector2.Zero);

            if (ImGui.Begin("Debug", ImGuiWindowFlags.NoResize))
            {
                ImGui.TextDisabled("Platform:");
                ImGui.SameLine();
                ImGui.Text(graphicsDevice.PlatformHost.CurrentPlatform.ToString());

                ImGui.TextDisabled("Graphics:");
                ImGui.SameLine();
                ImGui.Text(graphicsDevice.GraphicsApi.ToString());

                ImGui.Separator();

                if (ImGui.CollapsingHeader("Renderer", ImGuiTreeNodeFlags.DefaultOpen))
                {
                    Color("Background", ref renderSettings.Background);

                    EnumSelect("Samples", ref renderSettings.Samples);

                    ImGui.SliderFloat("Gamma", ref renderSettings.Gamma, 0.6f, 3.0f);
                }

                if (ImGui.CollapsingHeader("Skybox"))
                {
                    Color("Star Color", ref skyboxSettings.StarColor);

                    ImGui.SliderFloat("Star Density", ref skyboxSettings.StarDensity, 0.0f, 1.0f);
                    ImGui.SliderFloat("Star Intensity", ref skyboxSettings.StarIntensity, 0.0f, 25.0f);
                }

                if (ImGui.CollapsingHeader("Galaxy"))
                {
                    var generationChanged = false;

                    ImGui.SeparatorText("Generation Properties");

                    if (ImGui.InputInt("Seed", ref generationSettings.Seed))
                    {
                        generationSettings.Random = new Random(generationSettings.Seed);

                        generationChanged = true;
                    }

                    generationChanged |= ImGui.SliderInt("Samples", ref generationSettings.Samples, 100, 500);
                    generationChanged |= ImGui.SliderFloat("Inner Radius", ref generationSettings.InnerRadius, 0f, generationSettings.OuterRadius);
                    generationChanged |= ImGui.SliderFloat("Outer Radius", ref generationSettings.OuterRadius, generationSettings.InnerRadius, 30f);

                    ImGui.Checkbox("Enable Hyperlanes", ref generationSettings.EnableHyperlanes);

                    ImGui.SameLine();

                    ImGui.Checkbox("Enable Octree", ref generationSettings.EnableOctree);

                    if (generationChanged)
                    {
                        generationSettings.HasChanged = true;
                    }

                    ImGui.SeparatorText("Simulation Properties");

                    var simulationChanged = false;

                    ImGui.SliderFloat("Time Step", ref simulationSettings.TimeStep, 0f, 10000f);
                    ImGui.SliderFloat("Star Size", ref simulationSettings.StarSize, 1f, 64f);
                    ImGui.SliderFloat("Particle Size", ref simulationSettings.ParticleSize, 500f, 2000f);
                    ImGui.SliderInt("Perturbations", ref simulationSettings.PertN, 0, 4);
                    ImGui.SliderFloat("Perturbations Amplitude", ref simulationSettings.PertAmp, 2f, 100f);

                    simulationChanged |= ImGui.SliderInt("Stars", ref simulationSettings.StarCount, 1000, 100000);
                    simulationChanged |= ImGui.SliderFloat("Radius", ref simulationSettings.GalaxyRadius, 1000f, 20000f);
                    simulationChanged |= ImGui.SliderFloat("Core", ref simulationSettings.Core, 0.05f, 0.95f);
                    simulationChanged |= ImGui.SliderFloat("Angle", ref simulationSettings.DeltaAngle, 0f, 1f);
                    simulationChanged |= ImGui.SliderFloat("Inner Excentricity", ref simulationSettings.Ex1, 0f, 2f);
                    simulationChanged |= ImGui.SliderFloat("Outter Excentricity", ref simulationSettings.Ex2, 0f, 2f);
                    simulationChanged |= ImGui.SliderFloat("Temperature", ref simulationSettings.BaseTemp, 1000, 10000);

                    simulationChanged |= ImGui.Checkbox("Enable Stars", ref simulationSettings.StarsEnabled);

                    ImGui.SameLine();

                    simulationChanged |= ImGui.Checkbox("Enable Dust", ref simulationSettings.DustEnabled);

                    ImGui.SameLine();

                    simulationChanged |= ImGui.Checkbox("Enable Filaments", ref simulationSettings.FilamentsEnabled);

                    if (simulationChanged)
                    {
                        simulationSettings.HasChanged = true;
                    }
                }

                ImGui.Text($"Selected: {galaxyContext.SelectedSystem?.Position}");
            }

            ImGui.End();

            commandList.SetRenderTarget(renderContext.Compositor.Surface);

            renderer.Draw(commandList);
        }

        private static bool Color(string label, ref Color color)
        {
            var temp = new System.Numerics.Vector3(color.R, color.G, color.B);
            var changed = false;

            if (ImGui.ColorEdit3(label, ref temp))
            {
                color = new Color(temp.X, temp.Y, temp.Z);

                changed = true;
            }

            return changed;
        }

        private static bool EnumSelect<TEnum>(string label, ref TEnum value)
            where TEnum : struct, Enum
        {
            var names = EnumInfo<TEnum>.Names;
            var values = EnumInfo<TEnum>.Values;

            var preview = value.ToString();

            ImGui.PushID($"##{label}Select");

            if (ImGui.BeginCombo(label, preview))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var isSelected = value.Equals(values[i]);

                    ImGui.PushID($"##Option{i}");

                    if (ImGui.Selectable(names[i], isSelected))
                    {
                        value = values[i];

                        return true;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }

                    ImGui.PopID();
                }

                ImGui.EndCombo();
            }

            ImGui.PopID();

            return false;
        }
    }
}
