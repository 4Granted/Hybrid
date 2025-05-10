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

namespace Hybrid.Graphics;

/// <summary>
/// Represents a command queue.
/// </summary>
public interface ICommandQueue
{
    /// <summary>
    /// Gets the type of the command queue.
    /// </summary>
    public CommandListType Type { get; }

    /// <summary>
    /// Allocates or recycles a command list.
    /// </summary>
    /// <returns>The command list.</returns>
    public CommandList Allocate();

    /// <summary>
    /// Executes the <paramref name="commandLists"/> on the GPU.
    /// </summary>
    /// <param name="commandLists">The command lists.</param>
    public void Execute(params IReadOnlyList<CommandList?> commandLists);

    /// <summary>
    /// Executes the <paramref name="commandList"/> on the GPU,
    /// then signals the <paramref name="fence"/> when finished.
    /// </summary>
    /// <param name="commandList">The command list.</param>
    /// <param name="fence">The fence.</param>
    public void Execute(CommandList commandList, Fence? fence = null);
}
