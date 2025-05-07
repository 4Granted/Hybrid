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

namespace Hybrid.Platform;

public static class Platforms
{
    /// <summary>
    /// Gets the current platform.
    /// </summary>
    public static PlatformType Current => current ??= GetPlatformType();

    private static PlatformType? current;

    public static PlatformType GetPlatformType()
    {
        if (OperatingSystem.IsWindows())
            return PlatformType.Windows;

        if (OperatingSystem.IsLinux())
            return PlatformType.Linux;

        if (OperatingSystem.IsMacOS())
            return PlatformType.MacOS;

        return PlatformType.Unknown;
    }
}
