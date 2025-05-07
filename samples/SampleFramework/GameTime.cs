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

namespace SampleFramework;

/// <summary>
/// Represents a chronological snapshot of a <see cref="Sample"/>.
/// </summary>
public readonly struct GameTime(float deltaTime, int frameCount)
{
    /// <summary>
    /// The time since the last frame in milliseconds.
    /// </summary>
    public readonly float DeltaTime = deltaTime;

    /// <summary>
    /// The amount of frames since the game started.
    /// </summary>
    public readonly int FrameCount = frameCount;
}
