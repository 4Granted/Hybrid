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

using Hybrid.Graphics.Native;
using System.Diagnostics;

namespace Hybrid.Graphics;

/// <summary>
/// Represents a command queue.
/// </summary>
public sealed class CommandQueue : DeviceResource, ICommandQueue
{
    private static uint nextId = 0;

    /// <summary>
    /// Gets the native command queue.
    /// </summary>
    public ICommandQueueImpl Impl { get; }

    /// <inheritdoc/>
    public CommandListType Type { get; }

    private readonly List<CommandList> tracked = [];
    private readonly Queue<CommandList> available = [];
    private readonly uint id;

    internal CommandQueue(GraphicsDevice graphicsDevice, CommandListType type)
        : base(graphicsDevice)
    {
        Impl = Factory.CreateCommandQueue();
        Type = type;

        id = nextId++;
    }

    /// <inheritdoc/>
    public CommandList Allocate()
    {
        if (!available.TryDequeue(out var commandList))
        {
            commandList = new CommandList(GraphicsDevice, this, Type);

            tracked.Add(commandList);
        }

        commandList.Impl.Begin();

        return commandList;
    }

    /// <inheritdoc/>
    public void Execute(params IReadOnlyList<CommandList?> commandLists)
    {
        foreach (var commandList in commandLists)
        {
            if (commandList == null)
                continue;

            Execute(commandList);
        }
    }

    /// <inheritdoc/>
    public void Execute(CommandList commandList, Fence? fence = null)
    {
        Debug.Assert(commandList.Type == Type, $"The command list type '{commandList.Type}' does not match the command queue type '{Type}'");
        Debug.Assert(commandList.CommandQueue.id == id, "The command list does not belong to this command queue");

        commandList.Impl.End();

        Impl.Execute(commandList.Impl, fence?.Impl);

        commandList.Reset();

        available.Enqueue(commandList);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            available.Clear();

            foreach (var commandList in tracked)
            {
                commandList.Dispose();
            }

            tracked.Clear();

            Impl?.Dispose();
        }
    }
}
