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
/// Defines the supported device vendors.
/// </summary>
public enum DeviceVendor : uint
{
    /// <summary>
    /// The device is from an unknown vendor.
    /// </summary>
    Unknown,

    /// <summary>
    /// The device is from Nvidia.
    /// </summary>
    Nvidia = 0x10DE,

    /// <summary>
    /// The device is from AMD.
    /// </summary>
    AMD = 0x1002,

    /// <summary>
    /// The device is from Intel.
    /// </summary>
    Intel = 0x8086,

    /// <summary>
    /// The device is from Microsoft.
    /// </summary>
    Microsoft = 0x1414,
}
