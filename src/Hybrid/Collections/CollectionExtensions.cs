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

public static class CollectionExtensions
{
    public static IList<T> Shuffle<T>(this IList<T> list, Random? random = null)
    {
        random ??= new Random();

        var n = list.Count;

        while (n > 1)
        {
            n--;

            var k = random.Next(n + 1);

            (list[n], list[k]) = (list[k], list[n]);
        }

        return list;
    }

    public static void ForEach<TValue>(
        this IList<TValue> self,
        Action<TValue> action,
        bool clearAfter = false)
    {
        foreach (var item in self)
        {
            action(item);
        }

        if (clearAfter)
        {
            self.Clear();
        }
    }

    public static IEnumerable<TValue> NotNull<TValue>(
        this IEnumerable<TValue?> self)
    {
        foreach (var item in self)
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }
}
