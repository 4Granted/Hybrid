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

using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents a description of a <see cref="ISwapChainImpl"/>.
/// </summary>
public struct SwapChainDescription : IDistinct<SwapChainDescription>
{
    /// <summary>
    /// The handle of the swapchain.
    /// </summary>
    public nint Handle;

    /// <summary>
    /// The format of the swapchain.
    /// </summary>
    public TextureFormat Format;

    /// <summary>
    /// The initial width of the swapchain.
    /// </summary>
    public uint Width;

    /// <summary>
    /// The initial height of the swapchain.
    /// </summary>
    public uint Height;

    /// <inheritdoc/>
    public readonly bool Equals(SwapChainDescription other)
        => other.Handle == Handle
        && other.Width == Width
        && other.Height == Height;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is SwapChainDescription other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode()
        => HashCode.Combine(Handle, Width, Height);

    /// <inheritdoc/>
    public static bool operator ==(SwapChainDescription left, SwapChainDescription right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(SwapChainDescription left, SwapChainDescription right) => !left.Equals(right);
}
