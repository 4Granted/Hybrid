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
using Silk.NET.Direct3D11;
using Silk.NET.DXGI;
using System.Runtime.CompilerServices;

namespace Hybrid.Graphics.DirectX11;

internal static class Dx11Utilities
{
    public static Format GetFormat(TextureFormat format, bool isDepth) => format switch
    {
        TextureFormat.R8UNorm => Format.FormatR8Unorm,
        TextureFormat.R8SNorm => Format.FormatR8SNorm,
        TextureFormat.R8UInt => Format.FormatR8Uint,
        TextureFormat.R8SInt => Format.FormatR8Sint,
        TextureFormat.Rg8UNorm => Format.FormatR8G8Unorm,
        TextureFormat.Rg8SNorm => Format.FormatR8G8SNorm,
        TextureFormat.Rg8UInt => Format.FormatR8G8Uint,
        TextureFormat.Rg8SInt => Format.FormatR8G8Sint,
        TextureFormat.R16UNorm => isDepth
            ? Format.FormatR16Typeless : Format.FormatR16Unorm,
        TextureFormat.R16SNorm => Format.FormatR16SNorm,
        TextureFormat.R16UInt => Format.FormatR16Uint,
        TextureFormat.R16SInt => Format.FormatR16Sint,
        TextureFormat.R16Float => Format.FormatR16Float,
        TextureFormat.Rg16UNorm => Format.FormatR16G16Unorm,
        TextureFormat.Rg16SNorm => Format.FormatR16G16SNorm,
        TextureFormat.Rg16UInt => Format.FormatR16G16Uint,
        TextureFormat.Rg16SInt => Format.FormatR16G16Sint,
        TextureFormat.Rg16Float => Format.FormatR16G16Float,
        TextureFormat.D16UNorm => Format.FormatD16Unorm,
        TextureFormat.R32UInt => Format.FormatR32Uint,
        TextureFormat.R32SInt => Format.FormatR32Sint,
        TextureFormat.R32Float => isDepth
            ? Format.FormatR32Typeless : Format.FormatR32Float,
        TextureFormat.Rgba8UNorm => Format.FormatR8G8B8A8Unorm,
        TextureFormat.Rgba8UNormSrgb => Format.FormatR8G8B8A8UnormSrgb,
        TextureFormat.Rgba8SNorm => Format.FormatR8G8B8A8SNorm,
        TextureFormat.Rgba8UInt => Format.FormatR8G8B8A8Uint,
        TextureFormat.Rgba8SInt => Format.FormatR8G8B8A8Sint,
        TextureFormat.Rgba16UNorm => Format.FormatR16G16B16A16Unorm,
        TextureFormat.Rgba16SNorm => Format.FormatR16G16B16A16SNorm,
        TextureFormat.Rgba16UInt => Format.FormatR16G16B16A16Uint,
        TextureFormat.Rgba16SInt => Format.FormatR16G16B16A16Sint,
        TextureFormat.Rgba16Float => Format.FormatR16G16B16A16Float,
        TextureFormat.Rg32UInt => Format.FormatR32G32Uint,
        TextureFormat.Rg32SInt => Format.FormatR32G32Sint,
        TextureFormat.Rg32Float => Format.FormatR32G32Float,
        TextureFormat.Rgb32UInt => Format.FormatR32G32B32Uint,
        TextureFormat.Rgb32SInt => Format.FormatR32G32B32Sint,
        TextureFormat.Rgb32Float => Format.FormatR32G32B32Float,
        TextureFormat.Rgba32UInt => Format.FormatR32G32B32A32Uint,
        TextureFormat.Rgba32SInt => Format.FormatR32G32B32A32Sint,
        TextureFormat.Rgba32Float => Format.FormatR32G32B32A32Float,
        TextureFormat.D32Float => isDepth ? Format.FormatD32Float
            : throw new InvalidOperationException(),
        TextureFormat.D24UNormS8UInt => isDepth ? Format.FormatR24G8Typeless
            : throw new InvalidOperationException(),
        _ => throw new InvalidOperationException(),
    };

    public static Format GetTypelessFormat(Format format) => format switch
    {
        Format.FormatR8Unorm or
        Format.FormatR8SNorm or
        Format.FormatR8Uint or
        Format.FormatR8Sint => Format.FormatR8Typeless,

        Format.FormatR8G8Unorm or
        Format.FormatR8G8SNorm or
        Format.FormatR8G8Uint or
        Format.FormatR8G8Sint => Format.FormatR8G8Typeless,

        Format.FormatR16Unorm or
        Format.FormatR16SNorm or
        Format.FormatR16Uint or
        Format.FormatR16Sint or
        Format.FormatR16Float or
        Format.FormatD16Unorm => Format.FormatR16Typeless,

        Format.FormatR16G16Unorm or
        Format.FormatR16G16SNorm or
        Format.FormatR16G16Uint or
        Format.FormatR16G16Sint or
        Format.FormatR16G16Float => Format.FormatR16G16Float,

        Format.FormatD24UnormS8Uint => Format.FormatR24G8Typeless,

        Format.FormatR32Uint or
        Format.FormatR32Sint or
        Format.FormatR32Float or
        Format.FormatD32Float => Format.FormatR32Typeless,

        Format.FormatR8G8B8A8Unorm or
        Format.FormatR8G8B8A8UnormSrgb or
        Format.FormatR8G8B8A8SNorm or
        Format.FormatR8G8B8A8Uint or
        Format.FormatR8G8B8A8Sint => Format.FormatR8G8B8A8Typeless,

        Format.FormatR16G16B16A16Unorm or
        Format.FormatR16G16B16A16SNorm or
        Format.FormatR16G16B16A16Uint or
        Format.FormatR16G16B16A16Sint or
        Format.FormatR16G16B16A16Float => Format.FormatR16G16B16A16Typeless,

        Format.FormatR32G32Uint or
        Format.FormatR32G32Sint or
        Format.FormatR32G32Float => Format.FormatR32G32Typeless,

        Format.FormatR32G32B32Uint or
        Format.FormatR32G32B32Sint or
        Format.FormatR32G32B32Float => Format.FormatR32G32B32Typeless,

        Format.FormatR32G32B32A32Uint or
        Format.FormatR32G32B32A32Sint or
        Format.FormatR32G32B32A32Float => Format.FormatR32G32B32A32Typeless,

        _ => format,
    };

    public static Format GetDepthFormat(TextureFormat format) => format switch
    {
        TextureFormat.R16UNorm or
        TextureFormat.D16UNorm => Format.FormatD16Unorm,

        TextureFormat.R32Float or
        TextureFormat.D32Float => Format.FormatD32Float,

        TextureFormat.D24UNormS8UInt => Format.FormatD24UnormS8Uint,
        TextureFormat.D32FloatS8UInt => Format.FormatD32FloatS8X24Uint,

        _ => throw new InvalidOperationException(),
    };

    public static Format GetViewFormat(Format format) => format switch
    {
        Format.FormatR16Typeless => Format.FormatR16Unorm,
        Format.FormatR32Typeless => Format.FormatR32Float,
        Format.FormatR32G8X24Typeless => Format.FormatR32FloatX8X24Typeless,
        Format.FormatR24G8Typeless => Format.FormatR24UnormX8Typeless,
        _ => format,
    };

    public static Format GetFormat(VertexElementFormat format) => format switch
    {
        VertexElementFormat.Byte4Norm => Format.FormatR8G8B8A8Unorm,
        VertexElementFormat.UInt1 => Format.FormatR32Uint,
        VertexElementFormat.UInt2 => Format.FormatR32G32Uint,
        VertexElementFormat.UInt3 => Format.FormatR32G32B32Uint,
        VertexElementFormat.UInt4 => Format.FormatR32G32B32A32Uint,
        VertexElementFormat.Int1 => Format.FormatR32Sint,
        VertexElementFormat.Int2 => Format.FormatR32G32Sint,
        VertexElementFormat.Int3 => Format.FormatR32G32B32Sint,
        VertexElementFormat.Int4 => Format.FormatR32G32B32A32Sint,
        VertexElementFormat.Float1 => Format.FormatR32Float,
        VertexElementFormat.Float2 => Format.FormatR32G32Float,
        VertexElementFormat.Float3 => Format.FormatR32G32B32Float,
        VertexElementFormat.Float4 => Format.FormatR32G32B32A32Float,
        _ => throw new InvalidOperationException(),
    };

    public static TextureAddressMode GetAddressMode(AddressMode mode) => mode switch
    {
        AddressMode.Wrap => TextureAddressMode.Wrap,
        AddressMode.Mirror => TextureAddressMode.Mirror,
        AddressMode.Clamp => TextureAddressMode.Clamp,
        AddressMode.Border => TextureAddressMode.Border,
        _ => throw new InvalidOperationException(),
    };

    public static Filter GetFilter(TextureFilter filter, bool hasComparison) => filter switch
    {
        TextureFilter.PointPointPoint => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinMagMipPoint,
        TextureFilter.PointPointLinear => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinMagPointMipLinear,
        TextureFilter.PointLinearPoint => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinPointMagLinearMipPoint,
        TextureFilter.PointLinearLinear => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinPointMagMipLinear,
        TextureFilter.LinearPointPoint => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinLinearMagMipPoint,
        TextureFilter.LinearPointLinear => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinLinearMagPointMipLinear,
        TextureFilter.LinearLinearPoint => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinMagLinearMipPoint,
        TextureFilter.LinearLinearLinear => hasComparison ? Filter.ComparisonMinMagMipPoint : Filter.MinMagMipLinear,
        TextureFilter.Anisotropic => hasComparison ? Filter.ComparisonAnisotropic : Filter.Anisotropic,
        _ => throw new InvalidOperationException(),
    };

    public static ComparisonFunc GetComparison(ComparisonFunction op) => op switch
    {
        ComparisonFunction.Never => ComparisonFunc.Never,
        ComparisonFunction.Equal => ComparisonFunc.Equal,
        ComparisonFunction.NotEqual => ComparisonFunc.NotEqual,
        ComparisonFunction.Less => ComparisonFunc.Less,
        ComparisonFunction.LessEqual => ComparisonFunc.LessEqual,
        ComparisonFunction.Greater => ComparisonFunc.Greater,
        ComparisonFunction.GreaterEqual => ComparisonFunc.GreaterEqual,
        ComparisonFunction.Always => ComparisonFunc.Always,
        _ => throw new InvalidOperationException(),
    };

    public static BindFlag GetBindFlags(BufferUsage usage)
    {
        var flag = BindFlag.None;

        if ((usage & BufferUsage.VertexBuffer) != 0)
            flag |= BindFlag.VertexBuffer;

        if ((usage & BufferUsage.IndexBuffer) != 0)
            flag |= BindFlag.IndexBuffer;

        if ((usage & BufferUsage.ConstantsBuffer) != 0)
            flag |= BindFlag.ConstantBuffer;

        if ((usage & BufferUsage.StructuredBuffer) != 0 ||
            (usage & BufferUsage.ComputeBuffer) != 0)
            flag |= BindFlag.ShaderResource;

        if ((usage & BufferUsage.ComputeBuffer) != 0)
            flag |= BindFlag.UnorderedAccess;

        return flag;
    }

    public static Map GetMapMode(MapMode usage) => usage switch
    {
        MapMode.Read => Map.Read,
        MapMode.Write => Map.Write,
        MapMode.ReadWrite => Map.ReadWrite,
        MapMode.WriteDiscard => Map.WriteDiscard,
        MapMode.WriteNoOverwrite => Map.WriteNoOverwrite,
        _ => throw new InvalidOperationException(),
    };

    public static BlendOp GetBlendFunction(BlendFunction function) => function switch
    {
        BlendFunction.Add => BlendOp.Add,
        BlendFunction.Subtract => BlendOp.Subtract,
        BlendFunction.ReverseSubtract => BlendOp.RevSubtract,
        BlendFunction.Minimum => BlendOp.Min,
        BlendFunction.Maximum => BlendOp.Max,
        _ => throw new InvalidOperationException(),
    };

    public static Blend GetBlendFactor(BlendFactor factor) => factor switch
    {
        BlendFactor.Zero => Blend.Zero,
        BlendFactor.One => Blend.One,
        BlendFactor.SourceColor => Blend.SrcColor,
        BlendFactor.InverseSourceColor => Blend.InvSrcColor,
        BlendFactor.SourceAlpha => Blend.SrcAlpha,
        BlendFactor.InverseSourceAlpha => Blend.InvSrcAlpha,
        BlendFactor.DestinationColor => Blend.DestColor,
        BlendFactor.InverseDestinationColor => Blend.InvDestColor,
        BlendFactor.DestinationAlpha => Blend.DestAlpha,
        BlendFactor.InverseDestinationAlpha => Blend.InvDestAlpha,
        BlendFactor.Factor => Blend.BlendFactor,
        BlendFactor.InverseFactor => Blend.InvBlendFactor,
        _ => throw new InvalidOperationException(),
    };

    public static StencilOp GetStencilOperation(StencilOperation operation) => operation switch
    {
        StencilOperation.Keep => StencilOp.Keep,
        StencilOperation.Zero => StencilOp.Zero,
        StencilOperation.Replace => StencilOp.Replace,
        StencilOperation.Increment => StencilOp.Incr,
        StencilOperation.IncrementSaturation => StencilOp.IncrSat,
        StencilOperation.Decrement => StencilOp.Decr,
        StencilOperation.DecrementSaturation => StencilOp.DecrSat,
        StencilOperation.Invert => StencilOp.Invert,
        _ => throw new InvalidOperationException(),
    };

    public static ColorWriteEnable GetColorChannels(ColorChannels channels)
    {
        var flag = ColorWriteEnable.None;

        if (channels.HasFlag(ColorChannels.Red))
            flag |= ColorWriteEnable.Red;

        if (channels.HasFlag(ColorChannels.Green))
            flag |= ColorWriteEnable.Green;

        if (channels.HasFlag(ColorChannels.Blue))
            flag |= ColorWriteEnable.Blue;

        if (channels.HasFlag(ColorChannels.Alpha))
            flag |= ColorWriteEnable.Alpha;

        return flag;
    }

    public static D3DPrimitiveTopology GetPrimitiveTopology(PrimitiveTopology topology) => topology switch
    {
        PrimitiveTopology.TriangleList => D3DPrimitiveTopology.D3D11PrimitiveTopologyTrianglelist,
        PrimitiveTopology.TriangleStrip => D3DPrimitiveTopology.D3D11PrimitiveTopologyTrianglestrip,
        PrimitiveTopology.LineList => D3DPrimitiveTopology.D3D11PrimitiveTopologyLinelist,
        PrimitiveTopology.LineStrip => D3DPrimitiveTopology.D3D11PrimitiveTopologyLinestrip,
        PrimitiveTopology.PointList => D3DPrimitiveTopology.D3D11PrimitiveTopologyPointlist,
        _ => throw new InvalidOperationException(),
    };

    public static ref Box AsSharpDX(ref this DataBox self) => ref Unsafe.As<DataBox, Box>(ref self);
    public static ref Box AsDx(ref this ResourceRegion @this) => ref Unsafe.As<ResourceRegion, Box>(ref @this);
}
