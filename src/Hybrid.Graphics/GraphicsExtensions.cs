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
using Hybrid.Platform;
using System.Collections.ObjectModel;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a collection of graphics extensions.
/// </summary>
public static class GraphicsExtensions
{
    private static readonly ReadOnlyDictionary<PlatformType, IReadOnlyList<GraphicsApi>> supportedApis;
    private static readonly ReadOnlyDictionary<PlatformType, GraphicsApi> defaultApis;

    static GraphicsExtensions()
    {
        supportedApis = new(new Dictionary<PlatformType, IReadOnlyList<GraphicsApi>>
        {
            { PlatformType.Unknown, [GraphicsApi.Unknown] },
            { PlatformType.Windows, [GraphicsApi.OpenGL, GraphicsApi.Vulkan, GraphicsApi.DirectX11, GraphicsApi.DirectX12] },
            { PlatformType.Linux, [GraphicsApi.OpenGL, GraphicsApi.Vulkan] },
            { PlatformType.MacOS, [GraphicsApi.OpenGL, GraphicsApi.Vulkan, GraphicsApi.Metal] },
        });

        defaultApis = new(new Dictionary<PlatformType, GraphicsApi>
        {
            { PlatformType.Unknown, GraphicsApi.Unknown },
            { PlatformType.Windows, GraphicsApi.DirectX11 },
            { PlatformType.Linux, GraphicsApi.OpenGL },
            { PlatformType.MacOS, GraphicsApi.Metal },
        });
    }

    public static GraphicsContext CreateContext(
        this GraphicsDevice self)
        => GraphicsContext.Create(self);

    public static GraphicsContext CreateContext(
        this GraphicsDevice self,
        out CommandList commandList)
    {
        var context = GraphicsContext.Create(self);

        commandList = context.CommandList;

        return context;
    }

    /// <summary>
    /// Queries the supported graphics APIs of a specified platform.
    /// </summary>
    /// <param name="self">The platform to query.</param>
    /// <returns>The supported APIs.</returns>
    public static IReadOnlyList<GraphicsApi> GetSupportedApis(this PlatformType self)
        => supportedApis.TryGetValue(self, out var result) ? result : [];

    /// <summary>
    /// Queries the default graphics API of the specified platform.
    /// </summary>
    /// <param name="self">The platform to query.</param>
    /// <returns>The default API.</returns>
    public static GraphicsApi GetDefaultApi(this PlatformType self)
        => defaultApis.TryGetValue(self, out var result)
            ? result : GraphicsApi.Unknown;

    public static void SetConstants(
        this IDescriptorSet? self,
        int index, Buffer? buffer)
        => self?.SetResource(index, buffer?.Impl);

    public static void SetTexture(
        this IDescriptorSet? self,
        int index, ITexture? texture)
        => self?.SetResource(index, texture?.Impl.View);

    public static void SetSampler(
        this IDescriptorSet? self,
        int index, TextureSampler? sampler)
        => self?.SetResource(index, sampler?.Impl);

    internal static bool MatchOptions(
        this IGraphicsBackend self,
        ref GraphicsOptions options)
    {
        if (options is not { Api: GraphicsApi api })
            return false;

        if (api == GraphicsApi.Unknown)
            return false;

        return api != self.Api;
    }
}
