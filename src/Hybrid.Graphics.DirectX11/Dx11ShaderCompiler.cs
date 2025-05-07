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
using System.Runtime.CompilerServices;
using System.Text;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11ShaderCompiler : Dx11DeviceResource
{
    internal Dx11ShaderCompiler(Dx11GraphicsDevice graphicsDevice)
        : base(graphicsDevice) { }

    public unsafe IShaderBytecodeImpl Compile(ref ShaderDescription description)
    {
        ComPtr<ID3D10Blob> blob = default;
        ComPtr<ID3D10Blob> errors = default;

        var source = Encoding.ASCII.GetBytes(description.Source);
        var stage = description.Stage;

        HResult hr = GraphicsDevice.D3DC.Compile
        (
            in source[0],
            (nuint)source.Length,
            nameof(source),
            null,
            ref Unsafe.NullRef<ID3DInclude>(),
            description.EntryPoint,
            GetShaderTarget(stage),
            0,
            0,
            ref blob,
            ref errors
        );

        // Check for compilation errors.
        if (hr.IsFailure)
        {
            if (errors.Handle is not null)
            {
                var message = SilkMarshal.PtrToString((nint)errors.GetBufferPointer());

                throw new InvalidOperationException(message);

                //Logger.Default.Fatal("Shader Error: {0}", SilkMarshal.PtrToString((nint)errors.GetBufferPointer()));
            }

            hr.Throw();
        }

        errors.Dispose();

        return new Dx11ShaderBytecode(GraphicsDevice, blob, source, stage);
    }

    private static string GetShaderTarget(ShaderStage stage) => stage switch
    {
        ShaderStage.Vertex => "vs_5_0",
        ShaderStage.Hull => "hs_5_0",
        ShaderStage.Domain => "ds_5_0",
        ShaderStage.Geometry => "gs_5_0",
        ShaderStage.Pixel => "ps_5_0",
        ShaderStage.Compute => "cs_5_0",
        _ => throw new InvalidOperationException(),
    };
}
