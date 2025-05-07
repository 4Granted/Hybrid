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
using System.Reflection;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a graphics device.
/// </summary>
public sealed class GraphicsDevice : GraphicsResource
{
    //private const string OPENGL_RENDERER = "Hybrid.Graphics.OpenGL";
    //private const string VULKAN_RENDERER = "Hybrid.Graphics.Vulkan";
    private const string DX11_RENDERER = "Hybrid.Graphics.DirectX11";
    //private const string DX12_RENDERER = "Hybrid.Graphics.DirectX12";
    //private const string METAL_RENDERER = "Hybrid.Graphics.Metal";

    /// <summary>
    /// Gets the available renderer backends of the graphics API.
    /// </summary>
    public static IReadOnlyCollection<IGraphicsBackend> Backends
    {
        get
        {
            if (!isInitialized)
            {
                AddRenderers();

                isInitialized = true;
            }

            return backends;
        }
    }

    private static readonly List<IGraphicsBackend> backends = [];
    private static readonly byte[] white = [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF];
    private static bool isInitialized = false;

    /// <summary>
    /// Gets the native graphics device.
    /// </summary>
    public IGraphicsDeviceImpl Impl { get; }

    /// <summary>
    /// Gets the adapter of the device.
    /// </summary>
    public GraphicsAdapter Adapter { get; }

    /// <summary>
    /// Gets the graphics command queue of the device.
    /// </summary>
    public CommandQueue GraphicsQueue { get; }

    /// <summary>
    /// Gets the compute command queue of the device.
    /// </summary>
    public CommandQueue ComputeQueue { get; }

    /// <summary>
    /// Gets the default white texture.
    /// </summary>
    public Texture2D WhiteTexture { get; }

    /// <summary>
    /// Gets the point texture sampler.
    /// </summary>
    public TextureSampler PointSampler { get; }

    /// <summary>
    /// Gets the linear texture sampler.
    /// </summary>
    public TextureSampler LinearSampler { get; }

    /// <summary>
    /// Gets the anisotropic texture sampler.
    /// </summary>
    public TextureSampler AnisotropicSampler { get; }

    /// <summary>
    /// Gets the metrics of the device.
    /// </summary>
    public GraphicsMetrics Metrics { get; }

    /// <summary>
    /// Gets the options of the device.
    /// </summary>
    public GraphicsOptions Options { get; }

    private readonly Dictionary<PipelineDescription, IPipelineImpl> pipelineCache = [];
    private readonly List<WeakReference> resourceReferences = [];
    private readonly Lock resourceLock = new();

    private GraphicsDevice(
        IGraphicsDeviceImpl impl,
        GraphicsOptions options)
    {
        Impl = impl;
        Adapter = new GraphicsAdapter(impl.Adapter);
        Options = options;

        GraphicsQueue = new CommandQueue(this, CommandListType.Graphics);
        ComputeQueue = new CommandQueue(this, CommandListType.Compute);

        WhiteTexture = new Texture2D(this, TextureFormat.Rgba8UNorm, 1, 1);

        var commandList = GraphicsQueue.Allocate();

        WhiteTexture.WriteUnsafe<byte>(commandList, white);

        GraphicsQueue.Execute(commandList);

        PointSampler = new TextureSampler(this, SamplerDescription.Point);
        LinearSampler = new TextureSampler(this, SamplerDescription.Linear);
        AnisotropicSampler = new TextureSampler(this, SamplerDescription.Anisotropic);

        Metrics = new GraphicsMetrics();
    }

    /// <summary>
    /// Begins the next frame.
    /// </summary>
    public void BeginFrame()
    {
        Metrics.Clear();
    }

    /// <summary>
    /// Ends the previous frame.
    /// </summary>
    public void EndFrame() { }

    internal IPipelineImpl GetOrCreatePipeline(ref PipelineDescription description)
    {
        if (!pipelineCache.TryGetValue(description, out var pipeline))
        {
            pipeline = Impl.Factory.CreatePipeline(ref description);

            pipelineCache.Add(description, pipeline);
        }

        return pipeline;
    }

    internal override void Reset()
    {
        lock (resourceLock)
        {
            foreach (var reference in resourceReferences)
            {
                if (reference.Target is GraphicsDevice)
                    throw new InvalidOperationException();

                if (reference.Target is GraphicsResource resource)
                {
                    resource.Reset();
                }
            }

            resourceReferences.RemoveAll(reference => !reference.IsAlive);
        }
    }

    internal void AddReference(WeakReference reference)
    {
        lock (resourceLock)
        {
            resourceReferences.Add(reference);
        }
    }

    internal void RemoveReference(WeakReference reference)
    {
        lock (resourceLock)
        {
            resourceReferences.Remove(reference);
        }
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            lock (resourceLock)
            {
                foreach (var reference in resourceReferences.ToArray())
                {
                    if (reference.Target is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }

                resourceReferences.Clear();
            }

            pipelineCache.DisposeValues(clear: true);

            GraphicsQueue?.Dispose();
            WhiteTexture?.Dispose();
            PointSampler?.Dispose();
            LinearSampler?.Dispose();
            AnisotropicSampler?.Dispose();

            Impl?.Dispose();
        }
    }

    /// <summary>
    /// Creates a graphics device.
    /// </summary>
    /// <param name="options">The device options.</param>
    /// <returns>The graphics device.</returns>
    public static GraphicsDevice Create(GraphicsOptions options)
    {
        static bool SelectAdapter(IGraphicsAdapterImpl? adapter)
            => adapter is { Mode: DeviceMode.Dedicated };

        var backends = Backends;

        if (backends.Count <= 0)
            throw new PlatformNotSupportedException();

        foreach (var backend in backends)
        {
            if (backend.MatchOptions(ref options))
                continue;

            var device = backend.CreateDevice(SelectAdapter);

            if (device == null)
                continue;

            return new GraphicsDevice(device, options);
        }

        throw new PlatformNotSupportedException();
    }

    private static void AddRenderers()
    {
        TryAddRenderer(DX11_RENDERER);
    }

    private static bool TryAddRenderer(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;

        try
        {
            var assembly = Assembly.Load(name);
            var attribute = assembly.GetCustomAttribute<RendererAttribute>();

            if (attribute == null)
                return false;

            var impl = Activator.CreateInstance(
                attribute.DeviceType, true);

            if (impl is not IGraphicsBackend backend)
                return false;

            return AddRenderer(backend);
        }
        catch
        {
            return false;
        }
    }

    private static bool AddRenderer(IGraphicsBackend? backend)
    {
        if (backend == null)
            return false;

        backends.Add(backend);

        return true;
    }
}
