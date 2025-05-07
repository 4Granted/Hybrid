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
/// Represents an API independent GPU resource.
/// </summary>
public abstract class GpuResource : DeviceResource
{
    /// <summary>
    /// Gets the access of the resource.
    /// </summary>
    public ResourceAccess Access { get; }

    private protected GpuResource(
        GraphicsDevice graphicsDevice,
        ResourceAccess access)
        : base(graphicsDevice)
        => Access = access;

    protected static ulong GenerateId(
        GraphicsResource parent, IGraphicsResource child)
        => (ulong)parent.Id << 32 | child.Id;
}
