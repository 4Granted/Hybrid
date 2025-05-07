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

namespace Hybrid.Graphics;

/// <summary>
/// Represents a resource owned by a <see cref="Graphics.GraphicsDevice"/>.
/// </summary>
public abstract class DeviceResource : GraphicsResource
{
    /// <summary>
    /// Gets the graphics device of the resource.
    /// </summary>
    public GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    /// Gets the native graphics factory.
    /// </summary>
    protected IGraphicsFactoryImpl Factory { get; }

    private readonly WeakReference reference;

    /// <summary>
    /// Constructs a device resource.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the resource.</param>
    protected DeviceResource(
        GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;
        Factory = graphicsDevice.Impl.Factory;

        reference = new WeakReference(this);

        graphicsDevice.AddReference(reference);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        GraphicsDevice.RemoveReference(reference);
    }
}
