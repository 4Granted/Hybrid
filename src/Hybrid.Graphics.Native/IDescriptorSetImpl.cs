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
/// Represents an API specific descriptor set implementation.
/// </summary>
public interface IDescriptorSetImpl : IDeviceResource
{
    /// <summary>
    /// Gets the layout of the descriptor set.
    /// </summary>
    public IDescriptorLayoutImpl Layout { get; }

    /// <summary>
    /// Binds the <paramref name="resource"/> at the
    /// <paramref name="index"/> to the descriptor set.
    /// </summary>
    /// <param name="index">The index to bind.</param>
    /// <param name="resource">The resource to bind.</param>
    public void SetResource(int index, IGpuResource? resource);
}
