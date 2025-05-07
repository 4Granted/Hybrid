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
using Silk.NET.Core.Native;
using System.Diagnostics.CodeAnalysis;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11ShaderBytecode : Dx11DeviceResource, IShaderBytecodeImpl
{
    public ReadOnlyMemory<byte> Data { get; }
    public ShaderStage Stage { get; }
    public bool IsValid => Stage != ShaderStage.None;

    internal ComPtr<ID3D10Blob> Blob;

    internal Dx11ShaderBytecode(
        Dx11GraphicsDevice graphicsDevice,
        ComPtr<ID3D10Blob> blob,
        ReadOnlyMemory<byte> data,
        ShaderStage stage)
        : base(graphicsDevice)
    {
        Blob = blob;
        Data = data;
        Stage = stage;
    }

    public unsafe bool Equals(Dx11ShaderBytecode? other)
        => other != null
        && other.Blob.Handle == Blob.Handle
        && other.Stage == Stage;
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Dx11ShaderBytecode other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Blob, Stage);

    protected override void Dispose(bool disposing)
        => ReleaseCom(ref Blob);
}
