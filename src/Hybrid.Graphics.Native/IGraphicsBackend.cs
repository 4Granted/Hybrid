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

namespace Hybrid.Graphics.Native;

public delegate bool QueryAdapter(IGraphicsAdapterImpl? adapter);

/// <summary>
/// Represents a graphics backend.
/// </summary>
public interface IGraphicsBackend
{
    /// <summary>
    /// Gets the graphics API of the backend.
    /// </summary>
    public GraphicsApi Api { get; }

    /// <summary>
    /// Creates a graphics device.
    /// </summary>
    /// <param name="queryAdapter">A delegate used to select the graphics adapter.</param>
    /// <returns>The device instance.</returns>
    public IGraphicsDeviceImpl? CreateDevice(QueryAdapter queryAdapter);
}
