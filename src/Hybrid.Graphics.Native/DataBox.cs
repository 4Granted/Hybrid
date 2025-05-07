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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Hybrid.Graphics.Native;

/// <summary>
/// Represents an unmanaged data pointer.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct DataBox : IDistinct<DataBox>
{
    private static readonly DataBox empty;

    /// <summary>
    /// The data pointer of the box.
    /// </summary>
    public nint DataPointer;

    /// <summary>
    /// The row pitch of the box.
    /// </summary>
    public uint RowPitch;

    /// <summary>
    /// The slice pitch of the box.
    /// </summary>
    public uint SlicePitch;

    /// <summary>
    /// Gets whether the box is empty.
    /// </summary>
    public readonly bool IsEmpty => Equals(empty);

    /// <summary>
    /// Constructs a data box.
    /// </summary>
    /// <param name="dataPointer">The data pointer of the box.</param>
    /// <param name="rowPitch">The row pitch of the box.</param>
    /// <param name="slicePitch">The slice pitch of the box.</param>
    public DataBox(nint dataPointer, uint rowPitch, uint slicePitch)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(dataPointer, nint.Zero, nameof(dataPointer));

        DataPointer = dataPointer;
        RowPitch = rowPitch;
        SlicePitch = slicePitch;
    }

    /// <inheritdoc/>
    public readonly bool Equals(DataBox other)
        => other.DataPointer == DataPointer
        && other.RowPitch == RowPitch
        && other.SlicePitch == SlicePitch;

    /// <inheritdoc/>
    public readonly override bool Equals([NotNullWhen(true)] object? obj)
        => obj is DataBox other && Equals(other);

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(
        DataPointer, RowPitch, SlicePitch);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(DataBox left, DataBox right) => left.Equals(right);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(DataBox left, DataBox right) => !left.Equals(right);
}
