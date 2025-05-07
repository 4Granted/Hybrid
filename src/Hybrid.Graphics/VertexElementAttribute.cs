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

namespace Hybrid.Graphics;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class VertexElementAttribute : Attribute
{
    /// <summary>
    /// Gets the format of the vertex element.
    /// </summary>
    public VertexElementFormat Format { get; }

    /// <summary>
    /// Gets the name of the vertex element.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the semantic of the vertex element.
    /// </summary>
    public string Semantic { get; }

    /// <summary>
    /// Gets the offset of the vertex element.
    /// </summary>
    public uint Offset { get; }

    public VertexElementAttribute(
        VertexElementFormat format,
        string name,
        string semantic,
        uint offset)
    {
        Format = format;
        Name = name;
        Semantic = semantic;
        Offset = offset;
    }

    public VertexElementAttribute(
        VertexElementFormat format,
        string name,
        string semantic)
    {
        Format = format;
        Name = name;
        Semantic = semantic;
        Offset = 0;
    }

    public VertexElementAttribute(
        VertexElementFormat format,
        string semantic)
    {
        Format = format;
        Name = semantic;
        Semantic = semantic;
        Offset = 0;
    }
}
