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

using Hybrid;
using Hybrid.Collections;
using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Numerics;
using Hybrid.Rendering.Effects;
using Hybrid.Services;
using System.Runtime.InteropServices;

namespace Sandbox.Galaxy
{
    internal sealed class SimulationRenderer : DeviceResource
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct PerEffect
        {
            public int PertN;
            public float StarSize;
            public float ParticleSize;
            public float PertAmp;
            public float Time;
        }

        private static readonly ConstantsParameter<PerEffect> ParticleConstants
            = EffectParameter.CreateConstants<PerEffect>("PerEffect", ShaderStage.Vertex);

        private const float Gravity = 6.672e-11f;
        private const float PcToKm = 3.08567758129e13f;
        private const float SecPerYr = 365.25f * 86400;

        private readonly SimulationSettings settings;
        private readonly DynamicHeap<GalaxyParticle> particleHeap;
        private readonly ConstantsBuffer<PerEffect> particleConstants;
        private readonly VertexBuffer<GalaxyParticle> particleBuffer;
        private readonly EffectInstance effectInstance;
        private float farRadius;
        private float time;
        private bool isDirty;

        internal SimulationRenderer(
            GraphicsDevice graphicsDevice,
            IServiceScope services)
            : base(graphicsDevice)
        {
            settings = services.GetRequiredService<SimulationSettings>();
            particleHeap = new DynamicHeap<GalaxyParticle>(settings.StarCount);
            particleConstants = new ConstantsBuffer<PerEffect>(graphicsDevice);
            particleBuffer = new VertexBuffer<GalaxyParticle>(graphicsDevice, settings.StarCount);

            var source = File.ReadAllText("Particle.hlsl");

            var effect = new Effect(graphicsDevice,
                new VertexShader(graphicsDevice, source),
                new GeometryShader(graphicsDevice, source),
                new PixelShader(graphicsDevice, source));

            effect.AddParameter(SpatialEffect.PerViewParameter);
            effect.AddParameter(SpatialEffect.PerDrawParameter);
            effect.AddParameter(ParticleConstants);

            effectInstance = new EffectInstance(effect)
            {
                RasterizerState = RasterizerState.DefaultDepth,
                BlendState = BlendState.Additive with
                {
                    Target0 = BlendTarget.Additive,
                },
                Topology = PrimitiveTopology.PointList,
            };
        }

        public void Draw(
            CommandList commandList,
            ConstantsBuffer<SpatialEffect.PerView> perView,
            ConstantsBuffer<SpatialEffect.PerDraw> perDraw)
        {
            if (settings.StarsEnabled ||
                settings.DustEnabled ||
                settings.FilamentsEnabled)
            {
                Generate();
                Rebuild(commandList);

                time += settings.TimeStep;

                var perEffectData = new PerEffect
                {
                    PertN = settings.PertN,
                    StarSize = settings.StarSize,
                    ParticleSize = settings.ParticleSize,
                    PertAmp = settings.PertAmp,
                    Time = time,
                };

                particleConstants.Write(commandList, ref perEffectData);

                var offset = new Vector3(0f, -0.25f, 0f);

                var perDrawData = new SpatialEffect.PerDraw
                {
                    World = Matrix4.CreateScale(0.001f) * Matrix4.CreateTranslation(in offset),
                    WorldInverse = Matrix4.Identity,
                };

                perDraw.Write(commandList, ref perDrawData);

                effectInstance.Parameters.SetParameter(SpatialEffect.PerViewParameter, perView);
                effectInstance.Parameters.SetParameter(SpatialEffect.PerDrawParameter, perDraw);
                effectInstance.Parameters.SetParameter(ParticleConstants, particleConstants);

                commandList.SetVertexBuffer(particleBuffer);

                effectInstance.Apply(commandList);

                commandList.Draw(particleHeap.Count);
            }
        }

        private void Generate()
        {
            if (settings.HasChanged)
            {
                settings.HasChanged = false;

                particleHeap.Clear();

                var coreRadius = settings.CoreRadius;
                var galaxyRadius = settings.GalaxyRadius;
                var baseTemp = settings.BaseTemp;
                var starCount = settings.StarCount;

                farRadius = galaxyRadius * 2f;

                var cdf = new CDF(1.0, 0.02, galaxyRadius / 3.0f, coreRadius, 0, farRadius, 1000);
                var random = new Random();

                if (settings.StarsEnabled)
                {
                    for (int i = 0; i < starCount; i++)
                    {
                        var radius = (float)cdf.ValFromProb(random.NextDouble());

                        var star = new GalaxyParticle
                        {
                            A = radius,
                            B = radius * GetExcentricity(radius),
                            TiltAngle = GetAngularOffset(radius),
                            Theta0 = 360.0f * random.NextSingle(),
                            VelTheta = GetOrbitalVelocity(radius),
                            Temp = 6000 + 4000 * random.NextSingle(),
                            Mag = 0.1f + 0.4f * random.NextSingle(),
                            Type = 0,
                        };

                        if (i < starCount / 60)
                        {
                            star.Mag = MathF.Min(star.Mag + 0.1f + random.NextSingle() * 0.4f, 1.0f);
                        }

                        particleHeap.Allocate() = star;
                    }
                }

                float x, y, r;

                if (settings.DustEnabled)
                {
                    for (int i = 0; i < starCount; ++i)
                    {
                        if (i % 2 == 0)
                        {
                            r = (float)cdf.ValFromProb(random.NextDouble());
                        }
                        else
                        {
                            x = 2 * galaxyRadius * random.NextSingle() - galaxyRadius;
                            y = 2 * galaxyRadius * random.NextSingle() - galaxyRadius;
                            r = MathF.Sqrt(x * x + y * y);
                        }

                        var b = r * GetExcentricity(r);

                        var dust = new GalaxyParticle
                        {
                            A = r,
                            B = b,
                            TiltAngle = GetAngularOffset(r),
                            Theta0 = 360.0f * random.NextSingle(),
                            VelTheta = GetOrbitalVelocity((r + b) / 2.0f),
                            Temp = baseTemp + r / 4.5f,
                            Mag = 0.02f + 0.15f * random.NextSingle(),
                            Type = 1,
                        };

                        particleHeap.Allocate() = dust;
                    }
                }

                if (settings.FilamentsEnabled)
                {
                    for (int i = 0; i < starCount / 100; ++i)
                    {
                        _ = (float)cdf.ValFromProb(random.NextDouble());

                        x = 2 * galaxyRadius * random.NextSingle() - galaxyRadius;
                        y = 2 * galaxyRadius * random.NextSingle() - galaxyRadius;
                        r = MathF.Sqrt(x * x + y * y);

                        var theta = 360.0f * random.NextSingle();
                        var mag = 0.1f + 0.05f * random.NextSingle();
                        var num = (int)(100 * random.NextSingle());

                        for (int j = 0; j < num; ++j)
                        {
                            r = r + 200 - 400 * random.NextSingle();

                            var c = r * GetExcentricity(r);

                            var dust = new GalaxyParticle
                            {
                                A = r,
                                B = r * GetExcentricity(r),
                                TiltAngle = GetAngularOffset(r),
                                Theta0 = theta + 10 - 20 * random.NextSingle(),
                                VelTheta = GetOrbitalVelocity((r + c) / 2.0f),
                                Temp = baseTemp + r / 4.5f - 1000,
                                Mag = mag + 0.025f * random.NextSingle(),
                                Type = 2,
                            };

                            particleHeap.Allocate() = dust;
                        }
                    }
                }

                isDirty = true;
            }
        }

        private void Rebuild(CommandList commandList)
        {
            if (isDirty)
            {
                isDirty = false;

                var count = particleHeap.Count;

                for (int i = 1; i < particleHeap.Count; i++)
                {
                    ref var star = ref particleHeap[i];

                    star.Color = BlackBody.FromTemperature(star.Temp);
                }

                if (count > particleBuffer.Capacity)
                    particleBuffer.Resize(count);

                particleBuffer.Write(commandList, particleHeap.Span);
            }
        }

        private float GetExcentricity(float r)
        {
            var coreRadius = settings.CoreRadius;
            var galaxyRadius = settings.GalaxyRadius;
            var ex1 = settings.Ex1;
            var ex2 = settings.Ex2;

            if (r < coreRadius)
            {
                return 1 + r / coreRadius * (ex1 - 1);
            }
            else if (r > coreRadius && r <= galaxyRadius)
            {
                return ex1 + (r - coreRadius) / (galaxyRadius - coreRadius) * (ex2 - ex1);
            }
            else if (r > galaxyRadius && r < farRadius)
            {
                return ex2 + (r - galaxyRadius) / (farRadius - galaxyRadius) * (1 - ex2);
            }
            else
            {
                return 1;
            }
        }

        private float GetAngularOffset(float r) => r * settings.Angle;

        private static float GetOrbitalVelocity(float r)
        {
            var velKms = VelocityWithDarkMatter(r);

            var u = 2.0f * MathF.PI * r * PcToKm;
            var time = u / (velKms * SecPerYr);

            return 360.0f / time;
        }

        private static float VelocityWithDarkMatter(float r)
        {
            const float MZ = 100;

            return 20000.0f * (float)MathF.Sqrt(Gravity * (MassHalo(r) + MassDisc(r) + MZ) / r);
        }

        private static float MassDisc(float r)
        {
            const float d = 2000;
            const float rho_so = 1;
            const float rH = 2000;

            return (float)rho_so * (float)MathF.Exp(-r / rH) * (r * r) * MathF.PI * d;
        }

        private static float MassHalo(float r)
        {
            const float rho_h0 = 0.15f;
            const float rC = 2500;

            return (float)rho_h0 * 1 / (float)(1 + MathF.Pow(r / rC, 2)) * (float)(4 * MathF.PI * MathF.Pow(r, 3) / 3);
        }
    }
}
