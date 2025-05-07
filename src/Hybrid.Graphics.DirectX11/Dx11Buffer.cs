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
using Silk.NET.Direct3D.Compilers;
using Silk.NET.Direct3D11;

namespace Hybrid.Graphics.DirectX11;

internal sealed class Dx11Buffer : Dx11NativeResource, IBufferImpl
{
    public BufferDescription Description { get; }
    public uint SizeInBytes { get; }
    public uint StrideInBytes { get; }

    internal ComPtr<ID3D11Buffer> NativeBuffer;

    private readonly Lock accessLock = new();
    private readonly BindFlag bindFlags;

    private ComPtr<ID3D11ShaderResourceView> srv;
    private ComPtr<ID3D11UnorderedAccessView> uav;
    private Dictionary<BufferRange, ComPtr<ID3D11ShaderResourceView>>? srvs;
    private Dictionary<BufferRange, ComPtr<ID3D11UnorderedAccessView>>? uavs;

    internal unsafe Dx11Buffer(
        Dx11GraphicsDevice graphicsDevice,
        ref BufferDescription description)
        : base(graphicsDevice)
    {
        Description = description;

        var usage = description.Usage;

        SizeInBytes = description.SizeInBytes;
        StrideInBytes = description.StrideInBytes;

        bindFlags = Dx11Utilities.GetBindFlags(description.Usage);

        var desc = new BufferDesc
        {
            ByteWidth = SizeInBytes,
            BindFlags = (uint)bindFlags,
            Usage = Usage.Default,
        };

        if ((usage & BufferUsage.StructuredBuffer) != 0 ||
            (usage & BufferUsage.ComputeBuffer) != 0)
        {
            desc.StructureByteStride = StrideInBytes;
            desc.MiscFlags = (uint)ResourceMiscFlag.BufferStructured;
        }

        if ((usage & BufferUsage.IndirectBuffer) == BufferUsage.IndirectBuffer)
            desc.MiscFlags = (uint)ResourceMiscFlag.DrawindirectArgs;

        if ((usage & BufferUsage.Dynamic) == BufferUsage.Dynamic)
        {
            desc.Usage = Usage.Dynamic;
            desc.CPUAccessFlags = (uint)CpuAccessFlag.Write;
        }
        else if ((usage & BufferUsage.CopyBuffer) == BufferUsage.CopyBuffer)
        {
            desc.Usage = Usage.Staging;
            desc.CPUAccessFlags = (uint)(CpuAccessFlag.Read | CpuAccessFlag.Write);
        }

        SilkMarshal.ThrowHResult(NativeDevice.CreateBuffer(ref desc, null, ref NativeBuffer));
        SilkMarshal.ThrowHResult(NativeBuffer.QueryInterface(out NativeResource));

        var range = new BufferRange(0, SizeInBytes);

        if ((bindFlags & BindFlag.ShaderResource) != 0)
        {
            srv = GetShaderResourceView(range);
        }

        if ((bindFlags & BindFlag.UnorderedAccess) != 0)
        {
            uav = GetUnorderedAccessView(range);
        }
    }

    public unsafe ComPtr<ID3D11ShaderResourceView> GetShaderResourceView(BufferRange range)
    {
        ComPtr<ID3D11ShaderResourceView> candidate = srv;

        if (range.Offset == 0 && range.Length == SizeInBytes && candidate.Handle != default)
            return candidate;

        if ((bindFlags & BindFlag.ShaderResource) == 0)
            throw new InvalidOperationException();

        lock (accessLock)
        {
            srvs ??= [];

            if (!srvs.TryGetValue(range, out candidate))
            {
                var desc = new ShaderResourceViewDesc
                {
                    ViewDimension = D3DSrvDimension.D3D11SrvDimensionBuffer,
                };

                desc.Buffer.ElementOffset = range.Offset / StrideInBytes;
                desc.Buffer.NumElements = range.Length / StrideInBytes;

                SilkMarshal.ThrowHResult(NativeDevice.CreateShaderResourceView(NativeBuffer, ref desc, ref candidate));

                srvs.Add(range, candidate);
            }

            return candidate;
        }
    }

    public unsafe ComPtr<ID3D11UnorderedAccessView> GetUnorderedAccessView(BufferRange range)
    {
        ComPtr<ID3D11UnorderedAccessView> candidate = uav;

        if (range.Offset == 0 && range.Length == SizeInBytes && candidate.Handle != default)
            return candidate;

        if ((bindFlags & BindFlag.UnorderedAccess) == 0)
            throw new InvalidOperationException();

        lock (accessLock)
        {
            uavs ??= [];

            if (!uavs.TryGetValue(range, out candidate))
            {
                var desc = new UnorderedAccessViewDesc
                {
                    Format = Silk.NET.DXGI.Format.FormatUnknown,
                };

                desc.Buffer.FirstElement = range.Offset / StrideInBytes;
                desc.Buffer.NumElements = range.Length / StrideInBytes;

                SilkMarshal.ThrowHResult(NativeDevice.CreateUnorderedAccessView(NativeBuffer, ref desc, ref candidate));

                uavs.Add(range, candidate);
            }

            return candidate;
        }
    }

    protected override void Dispose(bool disposing)
    {
        ReleaseCom(ref srv);
        ReleaseCom(ref uav);

        srvs?.DisposeValues();
        uavs?.DisposeValues();

        ReleaseCom(ref NativeBuffer);
        //ReleaseCom(ref NativeResource);
    }
}
