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

namespace Hybrid.Numerics.Tests;

public sealed class Vector2Tests
{
    [Fact]
    public void Addition()
    {
        var a = new Vector2(5f, 10f);
        var b = new Vector2(10f, 5f);

        var result = a + b;

        var expected = new Vector2(15f, 15f);

        Assert.True(result == expected);
    }

    [Fact]
    public void Subtraction()
    {
        var a = new Vector2(5f, 10f);
        var b = new Vector2(10f, 5f);

        var result = a - b;

        var expected = new Vector2(-5f, 5f);

        Assert.True(result == expected);
    }

    [Fact]
    public void Multiplication()
    {
        var a = new Vector2(5f, 10f);
        var b = new Vector2(10f, 5f);

        var result = a * b;

        var expected = new Vector2(50f, 50f);

        Assert.True(result == expected);
    }

    [Fact]
    public void Division()
    {
        var a = new Vector2(5f, 10f);
        var b = new Vector2(10f, 5f);

        var result = a / b;

        var expected = new Vector2(0.5f, 2f);

        Assert.True(result == expected);
    }
}
