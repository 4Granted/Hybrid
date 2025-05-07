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
/// Defines the modes used for mapping
/// GPU resources to CPU memory.
/// </summary>
public enum MapMode
{
    /// <summary>
    /// The resources is read-only.
    /// </summary>
    Read = 1,

    /// <summary>
    /// The resource is write-only.
    /// </summary>
    Write = 2,

    /// <summary>
    /// The resource is read-write.
    /// </summary>
    ReadWrite = 3,

    /// <summary>
    /// The resource is written to and discarded.
    /// </summary>
    WriteDiscard = 4,

    /// <summary>
    /// The resource is written to if not already.
    /// </summary>
    WriteNoOverwrite = 5,
}
