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

namespace Hybrid.Collections;

/// <summary>
/// Represents a collection that allows items to
/// be added to and removed from both ends.
/// </summary>
/// <typeparam name="TItem">The type of items in the collection.</typeparam>
public class Deque<TItem> : List<TItem>, IReadOnlyList<TItem>
{
    /// <summary>
    /// Gets or sets the item at the back of the collection.
    /// </summary>
    public TItem Back
    {
        get => this[0];
        set => this[0] = value;
    }

    /// <summary>
    /// Gets or sets the item at the front of the collection.
    /// </summary>
    public TItem Front
    {
        get => this[Count - 1];
        set => this[Count - 1] = value;
    }

    /// <summary>
    /// Pushes an <paramref name="item"/> to the back of the collection.
    /// </summary>
    /// <param name="item">The item to push.</param>
    public void PushBack(TItem item) => Insert(0, item);

    /// <summary>
    /// Pops an item from the back of the collection.
    /// </summary>
    /// <returns>The popped item.</returns>
    public TItem PopBack()
    {
        var item = Back;

        RemoveAt(0);

        return item;
    }

    /// <summary>
    /// Pushes an <paramref name="item"/> to the front of the collection.
    /// </summary>
    /// <param name="item">The item to push.</param>
    public void PushFront(TItem item) => Add(item);

    /// <summary>
    /// Pops an item from the front of the collection.
    /// </summary>
    /// <returns>The popped item.</returns>
    public TItem PopFront()
    {
        var item = Front;

        RemoveAt(Count - 1);

        return item;
    }
}
