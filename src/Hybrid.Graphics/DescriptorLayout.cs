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

public sealed class DescriptorLayout : DeviceResource
{
    /// <summary>
    /// Gets the native descriptor layout.
    /// </summary>
    public IDescriptorLayoutImpl Impl { get; }

    /// <summary>
    /// Gets the amount of descriptors in the layout.
    /// </summary>
    public int DescriptorCount { get; }

    internal DescriptorLayout(
        GraphicsDevice graphicsDevice,
        ref DescriptorLayoutDescription description)
        : base(graphicsDevice)
    {
        Impl = Factory.CreateDescriptorLayout(ref description);

        DescriptorCount = description.Elements?.Length ?? 0;
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
}
