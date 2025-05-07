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

using Hybrid.Collections;
using System.Diagnostics;

namespace Hybrid.Graphics;

public sealed class GraphicsContext : IDisposable
{
    private static Pool<GraphicsContext>? pool;

    /// <summary>
    /// Gets the graphics device of the context.
    /// </summary>
    public GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    /// Gets the command list of the context.
    /// </summary>
    public CommandList CommandList => commandList ??= commandQueue.Allocate();

    private readonly CommandQueue commandQueue;
    private CommandList? commandList;

    private GraphicsContext(GraphicsDevice graphicsDevice)
    {
        GraphicsDevice = graphicsDevice;

        commandQueue = graphicsDevice.GraphicsQueue;
        commandList = commandQueue.Allocate();
    }

    /// <summary>
    /// Executes the graphics context.
    /// </summary>
    public void Dispose()
    {
        commandQueue.Execute(CommandList);

        commandList = null;

        pool?.Free(this);
    }

    /// <summary>
    /// Creates a graphics context.
    /// </summary>
    /// <param name="graphicsDevice">The device of the graphics context.</param>
    /// <returns>The graphics context.</returns>
    public static GraphicsContext Create(GraphicsDevice graphicsDevice)
    {
        Debug.Assert(graphicsDevice != null);

        pool ??= new Pool<GraphicsContext>(() => new GraphicsContext(graphicsDevice));

        return pool.Allocate();
    }
}
