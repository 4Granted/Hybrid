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

using System.Diagnostics;

namespace Hybrid;

/// <summary>
/// Represents a collection of general utility functions.
/// </summary>
public static class CommonExtensions
{
    public static void EnsureSize<TArray>(ref TArray[] array, int size)
    {
        array ??= new TArray[size];

        if (array.Length < size)
        {
            Array.Resize(ref array, size);
        }
    }

    public static bool CompareArray<TArray>(
        in TArray[] left, in TArray[] right)
    {
        if (right.Length != left.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (!Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CompareDistinct<TArray>(
        in TArray[] left, in TArray[] right)
        where TArray : IDistinct<TArray>
    {
        if (right.Length != left.Length)
            return false;

        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] != right[i])
            {
                return false;
            }
        }

        return true;
    }

    public static bool CompareNullable<TValue>(in TValue? left, in TValue? right)
        where TValue : class => left?.Equals(right) ?? false;

    public static bool Equals<TValue>(TValue left, TValue right)
        => EqualityComparer<TValue>.Default.Equals(left, right);

    public static void InitializeArray<TValue>(
        ref TValue[] self, TValue value)
    {
        for (int i = 0; i < self.Length; i++)
        {
            self[i] = value;
        }
    }

    public static void CopyArray<TValue>(
        TValue[] source, ref TValue[] destination)
    {
        Debug.Assert(source != null);

        var length = source.Length;

        EnsureSize(ref destination, length);

        Array.Copy(source, destination, length);
    }

    public static void CopyArray<TValue>(
        ReadOnlySpan<TValue> source, ref TValue[] destination)
    {
        var length = source.Length;

        EnsureSize(ref destination, length);

        source.CopyTo(destination);
    }

    public static void Dispose<TDisposable>(ref TDisposable? disposable)
        where TDisposable : IDisposable
    {
        disposable?.Dispose();
        disposable = default;
    }

    public static void DisposeKeys<TKey, TValue>(
        this IDictionary<TKey, TValue> self,
        bool clear = false)
        where TKey : IDisposable
    {
        foreach (var key in self.Keys)
        {
            key?.Dispose();
        }

        if (clear)
        {
            self.Clear();
        }
    }

    public static void DisposeValues<TKey, TValue>(
        this IDictionary<TKey, TValue> self,
        bool clear = false)
        where TValue : IDisposable
    {
        foreach (var value in self.Values)
        {
            value?.Dispose();
        }

        if (clear)
        {
            self.Clear();
        }
    }

    public static void AsOrThrow<TBase, TDerived>(in TBase value, out TDerived derived)
        where TDerived : TBase
    {
        if (value is TDerived typed)
        {
            derived = typed;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static void AsOrThrow<TBase, TDerived>(in TBase[] values, out TDerived[] derived)
        where TDerived : TBase
    {
        derived = new TDerived[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            var value = values[i];

            if (value is TDerived typed)
            {
                derived[i] = typed;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
