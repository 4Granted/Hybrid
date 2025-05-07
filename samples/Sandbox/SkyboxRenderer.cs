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

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Services;
using System.Runtime.InteropServices;

namespace Sandbox
{
    internal sealed class SkyboxRenderer : DeviceResource
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SkyboxData
        {
            public Color StarColor;
            public float StarDensity;
            public float StarIntensity;
        }

        public static readonly ConstantsParameter<SkyboxData> SkyboxParameter
            = EffectParameter.CreateConstants<SkyboxData>("PerFrame", ShaderStage.Pixel);

        private static readonly VertexPosition[] vertices =
        [
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3( 1.0f, -1.0f, -1.0f),
            new Vector3( 1.0f,  1.0f, -1.0f),
            new Vector3(-1.0f,  1.0f, -1.0f),

            new Vector3(-1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f,  1.0f,  1.0f),
            new Vector3(-1.0f,  1.0f,  1.0f),
        ];

        private static readonly ushort[] indices =
        [
            0, 1, 2, 0, 2, 3,  // Front
            4, 5, 6, 4, 6, 7,  // Back
            0, 3, 7, 0, 7, 4,  // Left
            1, 5, 6, 1, 6, 2,  // Right
            3, 2, 6, 3, 6, 7,  // Top
            0, 1, 5, 0, 5, 4,  // Bottom
        ];

        private readonly SkyboxSettings settings;
        private readonly ConstantsBuffer<SkyboxData> skyboxConstants;
        private readonly VertexBuffer<VertexPosition> vertexBuffer;
        private readonly IndexBuffer<ushort> indexBuffer;
        private readonly Effect skyboxEffect;
        private readonly EffectInstance skyboxEffectInstance;

        internal SkyboxRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            settings = services.GetRequiredService<SkyboxSettings>();

            skyboxConstants = new ConstantsBuffer<SkyboxData>(graphicsDevice);
            vertexBuffer = new VertexBuffer<VertexPosition>(graphicsDevice, vertices);
            indexBuffer = new IndexBuffer<ushort>(graphicsDevice, indices);

            var source = File.ReadAllText("Skybox.hlsl");

            skyboxEffect = new Effect(graphicsDevice,
                new VertexShader(graphicsDevice, source),
                new PixelShader(graphicsDevice, source));

            skyboxEffect.AddParameter(SpatialEffect.PerViewParameter);
            skyboxEffect.AddParameter(SkyboxParameter);

            skyboxEffectInstance = new EffectInstance(skyboxEffect)
            {
                RasterizerState = RasterizerState.Default with
                {
                    //CullMode = CullMode.Back,
                },
                BlendState = BlendState.Opaque,
                DepthStencilState = DepthStencilState.DepthRead,
            };
        }

        public void Draw(
            CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView)
        {
            var skyboxData = new SkyboxData
            {
                StarColor = settings.StarColor,
                StarDensity = settings.StarDensity,
                StarIntensity = settings.StarIntensity,
            };

            skyboxConstants.Write(commandList, ref skyboxData);

            skyboxEffectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);
            skyboxEffectInstance.Parameters.SetParameter(SkyboxParameter, skyboxConstants);

            commandList.SetVertexBuffer(vertexBuffer);
            commandList.SetIndexBuffer(indexBuffer);

            skyboxEffectInstance.Apply(commandList);

            commandList.DrawIndexed(36);
        }
    }
}
