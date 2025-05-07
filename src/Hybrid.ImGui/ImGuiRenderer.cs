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

using Hybrid.Graphics;
using Hybrid.Graphics.Native;
using Hybrid.Graphics.Shaders;
using Hybrid.Graphics.Textures;
using Hybrid.Numerics;
using ImGuiNET;
using static ImGuiNET.ImGui;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Hybrid.ImGui;

/// <summary>
/// Represents an implementation of an <see cref="IImGuiRenderer"/>.
/// </summary>
public sealed class ImGuiRenderer : DeviceResource, IImGuiRenderer
{
    /// <summary>
    /// Represents a vertex used in ImGui rendering
    /// </summary>
    /// <remarks>
    /// Note that colors in an ImGui vertex are packed into a <see cref="uint"/>.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Vertex(Vector2 position, Vector2 uv, uint color) : IVertex
    {
        public static VertexLayout Layout => layout;

        private static readonly VertexLayout layout = new(
            VertexElement.POSITION2, VertexElement.TEXCOORD,
            new VertexElement(VertexElementFormat.Byte4Norm, VertexSemantics.COLOR));

        public readonly Vector2 Position = position;
        public readonly Vector2 TextureCoordinate = uv;
        public readonly uint Color = color;
    }

    private const nint FONT_ATLAS_ID = 1;

    /// <summary>
    /// Whether ImGui wants to capture keyboard input.
    /// </summary>
    public bool KeyboardCaptured => GetIO().WantCaptureKeyboard;

    /// <summary>
    /// Whether ImGui wants to capture mouse input.
    /// </summary>
    public bool MouseCaptured => GetIO().WantCaptureMouse;

    private readonly Dictionary<nint, Texture2D> textureMap = [];
    private readonly VertexShader vertexShader;
    private readonly PixelShader pixelShader;
    private DescriptorLayout descriptorLayout = default!;
    private DescriptorSet descriptorSet = default!;
    private VertexBuffer<Vertex> vertexBuffer = default!;
    private IndexBuffer<ushort> indexBuffer = default!;
    private ConstantsBuffer<Matrix4x4> constantsBuffer = default!;
    private GraphicsPipeline? graphicsPipeline;
    private Texture2D? fontTexture;
    private Size currentSize;
    private System.Numerics.Vector2 scaleFactor = System.Numerics.Vector2.One;
    private bool isRendering;

    private unsafe ImGuiRenderer(GraphicsDevice graphicsDevice, ImGuiOptions options)
        : base(graphicsDevice)
    {
        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Hybrid.ImGui.ImGui.hlsl")
            ?? throw new InvalidOperationException();
        using var reader = new StreamReader(stream);

        var source = reader.ReadToEnd();

        ArgumentException.ThrowIfNullOrWhiteSpace(source);

        vertexShader = new VertexShader(graphicsDevice, source);
        pixelShader = new PixelShader(graphicsDevice, source);

        currentSize = options.InitialSize;

        var context = CreateContext();

        SetCurrentContext(context);

        var io = GetIO();

        io.BackendFlags |=
            ImGuiBackendFlags.RendererHasVtxOffset |
            ImGuiBackendFlags.HasMouseCursors;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        io.Fonts.Flags |= ImFontAtlasFlags.NoBakedLines;

        CreateDeviceResources();
        UpdatePerFrameData(1f / 60f);

        NewFrame();

        isRendering = true;
    }

    /// <inheritdoc/>
    public void Update(float deltaTime)
    {
        if (isRendering)
        {
            Render();
        }

        UpdatePerFrameData(deltaTime);
        //UpdateInput(input);

        isRendering = true;

        NewFrame();
    }

    /// <inheritdoc/>
    public void Draw(CommandList commandList)
    {
        if (isRendering)
        {
            isRendering = false;

            Render();

            RenderPerFrameData(GetDrawData(), commandList);

            textureMap.Clear();
        }
    }

    /// <inheritdoc/>
    public void Import(Texture2D texture)
        => textureMap.Add((nint)texture.Impl.Id, texture);

    /// <inheritdoc/>
    public void Resize(Size size) => currentSize = size;

    public void SetMousePosition(float x, float y)
    {
        var io = GetIO();

        io.AddMousePosEvent(x, y);
    }

    public void SetMouseState(int index, bool isDown)
    {
        var io = GetIO();

        io.AddMouseButtonEvent(index, isDown);
    }

    public void SetMouseScroll(float amount)
    {
        var io = GetIO();

        io.AddMouseWheelEvent(0f, amount);
    }

    public void SetKeyboardState(int index, bool isDown)
    {
        var io = GetIO();

        io.AddKeyEvent((ImGuiKey)index, isDown);
    }

    private void CreateDeviceResources()
    {
        descriptorLayout = DescriptorLayoutBuilder.Create()
            .AddDescriptor(DescriptorType.ConstantResource, ShaderStage.Vertex)
            .AddDescriptor(DescriptorType.GraphicsResource, ShaderStage.Pixel)
            .AddDescriptor(DescriptorType.SamplerResource, ShaderStage.Pixel)
            .Build(GraphicsDevice);

        descriptorSet = new DescriptorSet(GraphicsDevice, descriptorLayout);

        vertexBuffer = new VertexBuffer<Vertex>(GraphicsDevice, 256);

        indexBuffer = new IndexBuffer<ushort>(GraphicsDevice, 256);

        constantsBuffer = new ConstantsBuffer<Matrix4x4>(GraphicsDevice);

        var graphicsPipelineDesc = new GraphicsPipelineDescription
        {
            RasterizerState = RasterizerState.Default with
            {
                WindingMode = WindingMode.Clockwise,
                CullMode = CullMode.None,
                DepthEnabled = false,
                ScissorEnabled = true,
            },
            BlendState = BlendState.NonPremultiplied,
            DepthStencilState = DepthStencilState.DepthRead,
            DescriptorLayouts = [descriptorLayout],
            VertexLayout = new(Vertex.Layout),
            VertexShader = vertexShader,
            PixelShader = pixelShader,
            BlendFactor = Color.White,
            Topology = PrimitiveTopology.TriangleList,
        };

        graphicsPipeline = new GraphicsPipeline(GraphicsDevice, ref graphicsPipelineDesc);

        CreateFontAtlas();
    }

    private void CreateFontAtlas()
    {
        var io = GetIO();

        io.Fonts.GetTexDataAsRGBA32(
            out nint pixels,
            out int width,
            out int height,
            out int bytesPerPixel);

        io.Fonts.SetTexID(FONT_ATLAS_ID);

        fontTexture = new Texture2D(
            GraphicsDevice,
            TextureFormat.Rgba8UNorm,
            width, height);

        var textureData = new DataBox
        {
            DataPointer = pixels,
            RowPitch = (uint)(bytesPerPixel * width),
            SlicePitch = 0,
        };

        var commandQueue = GraphicsDevice.GraphicsQueue;
        var commandList = commandQueue.Allocate();

        fontTexture.WriteUnsafe(commandList, textureData);

        commandQueue.Execute(commandList);

        io.Fonts.ClearTexData();
    }

    private void UpdatePerFrameData(float deltaTime)
    {
        var io = GetIO();

        io.DisplaySize = new System.Numerics.Vector2(
            currentSize.Width / scaleFactor.X,
            currentSize.Height / scaleFactor.Y);
        io.DisplayFramebufferScale = scaleFactor;
        io.DeltaTime = deltaTime;
    }

    private unsafe void RenderPerFrameData(ImDrawDataPtr data, CommandList commandList)
    {
        var vertexOffsetInVertices = 0;
        var indexOffsetInElements = 0;

        var commandListCount = data.CmdListsCount;

        if (commandListCount == 0 ||
            graphicsPipeline == null ||
            vertexBuffer == null ||
            indexBuffer == null ||
            constantsBuffer == null)
            return;

        var vertexBufferSize = data.TotalVtxCount;

        if (vertexBufferSize > vertexBuffer.Capacity)
        {
            vertexBuffer.Resize((int)(vertexBufferSize * 1.5f));
        }

        var indexBufferSize = data.TotalIdxCount;

        if (indexBufferSize > indexBuffer.Capacity)
        {
            indexBuffer.Resize((int)(indexBufferSize * 1.5f));
        }

        for (int i = 0; i < commandListCount; i++)
        {
            var imGuiCommandList = data.CmdLists[i];

            var vertices = new ReadOnlySpan<Vertex>(
                imGuiCommandList.VtxBuffer.Data.ToPointer(),
                imGuiCommandList.VtxBuffer.Size);

            vertexBuffer.Write(commandList, vertices, vertexOffsetInVertices);

            var indices = new ReadOnlySpan<ushort>(
                imGuiCommandList.IdxBuffer.Data.ToPointer(),
                imGuiCommandList.IdxBuffer.Size);

            indexBuffer.Write(commandList, indices, indexOffsetInElements);

            vertexOffsetInVertices += imGuiCommandList.VtxBuffer.Size;
            indexOffsetInElements += imGuiCommandList.IdxBuffer.Size;
        }

        var io = GetIO();

        var mvp = CreateProjectionMatrix(io.DisplaySize.X, io.DisplaySize.Y);

        constantsBuffer.Write(commandList, ref mvp);

        descriptorSet.SetConstants(0, constantsBuffer);
        descriptorSet.SetSampler(2, GraphicsDevice.PointSampler);

        commandList.SetVertexBuffer(vertexBuffer);
        commandList.SetIndexBuffer(indexBuffer);
        commandList.SetPipeline(graphicsPipeline);

        data.ScaleClipRects(io.DisplayFramebufferScale);

        var vertexOffset = 0;
        var indexOffset = 0;

        for (int i = 0; i < commandListCount; i++)
        {
            var imGuiCommandList = data.CmdLists[i];

            for (int j = 0; j < imGuiCommandList.CmdBuffer.Size; j++)
            {
                var imGuiCommand = imGuiCommandList.CmdBuffer[j];

                if (imGuiCommand.UserCallback != nint.Zero)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var textureId = imGuiCommand.TextureId;

                    if (textureId != nint.Zero)
                    {
                        if (textureId == FONT_ATLAS_ID)
                        {
                            descriptorSet.SetResource(1, fontTexture!.Impl.View);
                        }
                        else if (textureMap.TryGetValue(textureId, out var texture))
                        {
                            descriptorSet.SetResource(1, texture.Impl.View);
                        }
                    }

                    commandList.SetDescriptorSet(0, descriptorSet);

                    commandList.SetScissor(new Rectangle(
                        imGuiCommand.ClipRect.X, imGuiCommand.ClipRect.Y,
                        imGuiCommand.ClipRect.Z - imGuiCommand.ClipRect.X,
                        imGuiCommand.ClipRect.W - imGuiCommand.ClipRect.Y));

                    commandList.DrawIndexed(
                        indexCount: (int)imGuiCommand.ElemCount,
                        indexStart: (int)imGuiCommand.IdxOffset + indexOffset,
                        baseVertex: (int)imGuiCommand.VtxOffset + vertexOffset);
                }
            }

            vertexOffset += imGuiCommandList.VtxBuffer.Size;
            indexOffset += imGuiCommandList.IdxBuffer.Size;
        }
    }

    /// <summary>
    /// Creates an ImGui renderer.
    /// </summary>
    /// <param name="graphicsDevice">The graphics device of the renderer.</param>
    /// <param name="options">The options used to configure the renderer.</param>
    /// <returns>A renderer instance.</returns>
    public static ImGuiRenderer Create(
        GraphicsDevice graphicsDevice, ImGuiOptions options)
        => new(graphicsDevice, options);

    private static Matrix4x4 CreateProjectionMatrix(float width, float height, float near = 0.0f, float far = 1.0f)
    {
        return new Matrix4x4(
            2.0f / width, 0.0f, 0.0f, -1.0f,
            0.0f, -2.0f / height, 0.0f, 1.0f,
            0.0f, 0.0f, -1.0f / (far - near), -near / (far - near),
            0.0f, 0.0f, 0.0f, 1.0f
        );
    }
}
