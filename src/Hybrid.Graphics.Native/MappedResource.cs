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

/// <summary>
/// Represents an <see cref="IGpuResource"/> mapped to CPU memory.
/// </summary>
public readonly struct MappedResource
{
    /// <summary>
    /// Gets the mapped resource.
    /// </summary>
    public readonly IGpuResource Resource;

    /// <summary>
    /// Gets the mapped data.
    /// </summary>
    public readonly DataBox Data;

    /// <summary>
    /// Gets the subresource index.
    /// </summary>
    public readonly uint Subresource;

    /// <summary>
    /// Gets the size in bytes.
    /// </summary>
    public readonly uint SizeInBytes;

    /// <summary>
    /// Gets the offset in bytes.
    /// </summary>
    public readonly uint OffsetInBytes;

    public MappedResource(IGpuResource resource, DataBox data,
        uint subresource, uint sizeInBytes, uint offsetInBytes)
    {
        Resource = resource;
        Data = data;
        Subresource = subresource;
        SizeInBytes = sizeInBytes;
        OffsetInBytes = offsetInBytes;
    }

    public MappedResource(IGpuResource resource, DataBox data, uint subresource)
    {
        Resource = resource;
        Data = data;
        Subresource = subresource;
        SizeInBytes = 0;
        OffsetInBytes = 0;
    }
}
