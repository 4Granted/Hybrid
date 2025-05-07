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

public sealed class DynamicHeap<TItem> : IEnumerable<TItem>
    where TItem : unmanaged
{
    private struct Enumerator : IEnumerator<TItem>
    {
        /// <inheritdoc/>
        public TItem Current { get; private set; }
        readonly object? IEnumerator.Current => Current;

        private readonly TItem[] items;
        private int index;

        internal Enumerator(TItem[] source) => items = source;

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (index + 1 < items.Length)
            {
                Current = items[index++];

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public void Reset() => index = 0;

        /// <inheritdoc/>
        public readonly void Dispose() { }
    }

    /// <summary>
    /// Gets a reference to <typeparamref name="TItem"/>
    /// allocated at the <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index to query.</param>
    /// <returns>The reference.</returns>
    public ref TItem this[int index] => ref items[index];

    /// <summary>
    /// Gets a read-only span of the heap allocations.
    /// </summary>
    public ReadOnlySpan<TItem> Span => new(items, 0, count);

    /// <summary>
    /// Gets the capacity of the heap.
    /// </summary>
    public int Capacity => items.Length;

    /// <summary>
    /// Gets the amount of items allocated in the heap.
    /// </summary>
    public int Count => count;

    private TItem[] items;
    private readonly int growth;
    private int count;

    /// <summary>
    /// Constructs a dynamic heap.
    /// </summary>
    /// <param name="initialCapacity">The initial capacity of the heap.</param>
    /// <param name="growSize">The amount the heap grows by.</param>
    public DynamicHeap(int initialCapacity, int growSize = 64)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(initialCapacity, nameof(initialCapacity));

        items = new TItem[initialCapacity];
        growth = Math.Max(0, growSize - 1);
    }

    /// <summary>
    /// Allocates a reference to a <typeparamref name="TItem"/>
    /// if capacity is available, otherwise growing the heap.
    /// </summary>
    /// <returns>The reference.</returns>
    public ref TItem Allocate()
    {
        if (count >= items.Length)
        {
            var oldSize = items.Length;
            var newSize = oldSize + oldSize / 2;

            newSize = newSize + growth & ~growth;

            Array.Resize(ref items, newSize);

            for (int i = oldSize; i < newSize; i++)
            {
                items[i] = new TItem();
            }
        }

        return ref items[count++];
    }

    /// <summary>
    /// Clears the heap.
    /// </summary>
    public void Clear()
    {
        // TODO: Clearing the items creates a lot of dead objects,
        //  however not clearing them leaves left over garbage.
        //Array.Clear(items, 0, count);

        count = 0;
    }

    /// <inheritdoc/>
    public IEnumerator<TItem> GetEnumerator() => new Enumerator(items);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
