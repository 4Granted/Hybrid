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

using Silk.NET.Core.Native;
using System.Diagnostics;

namespace Hybrid.Graphics.DirectX11;

/// <summary>
/// Represents a base implementation of <see cref="IDx11Resource"/>.
/// </summary>
internal abstract class Dx11Resource : Resource, IDx11Resource
{
    private static uint nextId;

    public uint Id { get; }

    internal Dx11Resource() => Id = nextId++;

    internal static unsafe void ReleaseCom<T>(ref ComPtr<T> com)
        where T : unmanaged, IComVtbl<T>
    {
        var references = com.Release();

        Debug.Assert(references >= 0);

        if (references > 0)
            com.Dispose();

        com = null;
    }
}
