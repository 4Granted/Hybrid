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

using System.Collections;

namespace Hybrid.Collections;

public sealed class Pool<TItem> : IReadOnlyCollection<TItem>
    where TItem : class
{
    /// <summary>
    /// Gets or sets the item factory.
    /// </summary>
    public Func<TItem> Factory
    {
        get => factory ?? throw new InvalidOperationException();
        set => factory = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets the optional item constructor.
    /// </summary>
    public Action<TItem>? Constructor { get; set; }

    /// <summary>
    /// Gets or sets the optional item destructor.
    /// </summary>
    public Action<TItem>? Destructor { get; set; }

    /// <summary>
    /// Gets the amount of items available in the pool.
    /// </summary>
    public int Count => pooled.Count;

    private readonly Stack<TItem> pooled = [];
    private Func<TItem>? factory;

    public Pool(Func<TItem> factory,
        Action<TItem>? constructor = null,
        Action<TItem>? destructor = null)
    {
        Factory = factory;
        Constructor = constructor;
        Destructor = destructor;
    }

    public TItem Allocate()
    {
        var item = pooled.Count > 0 ? pooled.Pop() : Factory();

        Constructor?.Invoke(item);

        return item;
    }

    public void Free(IEnumerable<TItem> items)
    {
        foreach (var item in items)
        {
            Free(item);
        }
    }

    public void Free(TItem item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));

        Destructor?.Invoke(item);

        pooled.Push(item);
    }

    public void Clear() => pooled.Clear();

    /// <inheritdoc/>
    public IEnumerator<TItem> GetEnumerator() => pooled.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
