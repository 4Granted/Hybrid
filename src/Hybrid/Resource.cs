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

namespace Hybrid;

/// <summary>
/// Represents an object with a managed lifetime.
/// </summary>
public abstract class Resource : IDisposable
{
    /// <inheritdoc/>
    public bool IsDisposed { get; private set; }

    /// <inheritdoc/>
    public event Action? OnDisposing;

    /// <inheritdoc/>
    public event Action? OnDisposed;

    protected Resource() { }

    ~Resource() => Dispose(false);

    /// <inheritdoc/>
    public void Dispose()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;

        OnDisposing?.Invoke();

        Dispose(true);

        OnDisposed?.Invoke();

        OnDisposing = null;
        OnDisposed = null;

        GC.SuppressFinalize(this);
        GC.KeepAlive(this);
    }

    protected virtual void Dispose(bool disposing) { }
}
