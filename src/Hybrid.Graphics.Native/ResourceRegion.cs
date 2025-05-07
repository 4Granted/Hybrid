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

using System.Runtime.InteropServices;

namespace Hybrid.Graphics.Native;

/// <summary>
/// 
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 0)]
public struct ResourceRegion(
    int left, int top,
    int front, int right,
    int bottom, int back)
{
    /// <summary>
    /// Gets or sets the x position of the left of the region.
    /// </summary>
    public int Left = left;

    /// <summary>
    /// Gets or sets the y position of the top of the region.
    /// </summary>
    public int Top = top;

    /// <summary>
    /// Gets or sets the z position of the front of the region.
    /// </summary>
    public int Front = front;

    /// <summary>
    /// Gets or sets the x position of the rigght of the region.
    /// </summary>
    public int Right = right;

    /// <summary>
    /// Gets or sets the y position of the bottom of the region.
    /// </summary>
    public int Bottom = bottom;

    /// <summary>
    /// Gets or sets the z position of the back of the region.
    /// </summary>
    public int Back = back;
}
