// Hybrid - A versatile framework for application development.
// Copyright (C) 2024  Fielding Baran
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY- without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Hybrid.Numerics;

namespace Sandbox
{
    internal static class PoissonDisc
    {
        public static IReadOnlyList<Vector3> Sample3(float width, float height, float radius, int k = 30, Random? random = null)
        {
            random ??= new Random();

            // Cell size (use radius / sqrt(2) or something close)
            var cellSize = radius / (float)Math.Sqrt(2);

            // Calculate grid dimensions
            var gridWidth = (int)Math.Ceiling(width / cellSize);
            var gridHeight = (int)Math.Ceiling(height / cellSize);

            // Grid to store indices of points, -1 means empty
            var grid = new int[gridWidth * gridHeight];

            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = -1;
            }

            // List of sample points
            var points = new List<Vector3>();

            // Active list (indices of points in 'points' list that are not processed yet)
            var active = new List<int>();

            // Helper function to get a grid index
            int toGridIndex(int gx, int gz) => gz * gridWidth + gx;

            // 1) Add the first random point
            var firstPoint = new Vector3(
                (float)random.NextDouble() * width, 0f,
                (float)random.NextDouble() * height);

            points.Add(firstPoint);
            active.Add(0);

            var xIndex = (int)(firstPoint.X / cellSize);
            var zIndex = (int)(firstPoint.Z / cellSize);

            grid[toGridIndex(xIndex, zIndex)] = 0;

            while (active.Count > 0)
            {
                var activeIndex = active[random.Next(active.Count)];
                var activePoint = points[activeIndex];

                var foundNewPoint = false;

                for (int i = 0; i < k; i++)
                {
                    var angle = (float)(2 * Math.PI * random.NextDouble());
                    var mag = radius * (1 + (float)random.NextDouble());
                    var dir = new Vector3((float)Math.Cos(angle), 0f, (float)Math.Sin(angle));
                    var candidate = new Vector3(
                        activePoint.X + dir.X * mag, 0f,
                        activePoint.Z + dir.Z * mag);

                    if (candidate.X >= 0 && candidate.X < width &&
                        candidate.Z >= 0 && candidate.Z < height)
                    {
                        var cgx = (int)(candidate.X / cellSize);
                        var cgz = (int)(candidate.Z / cellSize);

                        var tooClose = false;

                        var minX = Math.Max(cgx - 2, 0);
                        var maxX = Math.Min(cgx + 2, gridWidth - 1);
                        var minZ = Math.Max(cgz - 2, 0);
                        var maxZ = Math.Min(cgz + 2, gridHeight - 1);

                        for (int gx = minX; gx <= maxX && !tooClose; gx++)
                        {
                            for (int gz = minZ; gz <= maxZ && !tooClose; gz++)
                            {
                                var idx = grid[toGridIndex(gx, gz)];

                                if (idx != -1)
                                {
                                    var neighbor = points[idx];
                                    var diff = candidate - neighbor;

                                    if (diff.LengthSquared < radius * radius)
                                    {
                                        tooClose = true;

                                        break;
                                    }
                                }
                            }
                        }

                        if (!tooClose)
                        {
                            // Valid point!
                            points.Add(candidate);
                            active.Add(points.Count - 1);

                            grid[toGridIndex(cgx, cgz)] = points.Count - 1;

                            foundNewPoint = true;

                            break; // break the for-loop, move on to next active point
                        }
                    }
                }

                // If no point was found, remove this one from active list
                if (!foundNewPoint)
                {
                    active.Remove(activeIndex);
                }
            }

            return points;
        }

        public static IReadOnlyList<Vector2> Sample2(float width, float height, float radius, int k = 30, Random? random = null)
        {
            random ??= new Random();

            // Cell size (use radius / sqrt(2) or something close)
            var cellSize = radius / (float)Math.Sqrt(2);

            // Calculate grid dimensions
            var gridWidth = (int)Math.Ceiling(width / cellSize);
            var gridHeight = (int)Math.Ceiling(height / cellSize);

            // Grid to store indices of points, -1 means empty
            var grid = new int[gridWidth * gridHeight];

            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = -1;
            }

            // List of sample points
            var points = new List<Vector2>();

            // Active list (indices of points in 'points' list that are not processed yet)
            var active = new List<int>();

            // Helper function to get a grid index
            int toGridIndex(int gx, int gy) => gy * gridWidth + gx;

            // 1) Add the first random point
            var firstPoint = new Vector2(
                (float)random.NextDouble() * width,
                (float)random.NextDouble() * height);

            points.Add(firstPoint);
            active.Add(0);

            var xIndex = (int)(firstPoint.X / cellSize);
            var yIndex = (int)(firstPoint.Y / cellSize);

            grid[toGridIndex(xIndex, yIndex)] = 0;

            while (active.Count > 0)
            {
                var activeIndex = active[random.Next(active.Count)];
                var activePoint = points[activeIndex];

                var foundNewPoint = false;

                for (int i = 0; i < k; i++)
                {
                    var angle = (float)(2 * Math.PI * random.NextDouble());
                    var mag = radius * (1 + (float)random.NextDouble());
                    var dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    var candidate = new Vector2(
                        activePoint.X + dir.X * mag,
                        activePoint.Y + dir.Y * mag);

                    if (candidate.X >= 0 && candidate.X < width &&
                        candidate.Y >= 0 && candidate.Y < height)
                    {
                        var cgx = (int)(candidate.X / cellSize);
                        var cgy = (int)(candidate.Y / cellSize);

                        var tooClose = false;

                        var minX = Math.Max(cgx - 2, 0);
                        var maxX = Math.Min(cgx + 2, gridWidth - 1);
                        var minY = Math.Max(cgy - 2, 0);
                        var maxY = Math.Min(cgy + 2, gridHeight - 1);

                        for (int gx = minX; gx <= maxX && !tooClose; gx++)
                        {
                            for (int gy = minY; gy <= maxY && !tooClose; gy++)
                            {
                                var idx = grid[toGridIndex(gx, gy)];

                                if (idx != -1)
                                {
                                    var neighbor = points[idx];
                                    var diff = candidate - neighbor;

                                    if (diff.LengthSquared < radius * radius)
                                    {
                                        tooClose = true;

                                        break;
                                    }
                                }
                            }
                        }

                        if (!tooClose)
                        {
                            // Valid point!
                            points.Add(candidate);
                            active.Add(points.Count - 1);

                            grid[toGridIndex(cgx, cgy)] = points.Count - 1;

                            foundNewPoint = true;

                            break; // break the for-loop, move on to next active point
                        }
                    }
                }

                // If no point was found, remove this one from active list
                if (!foundNewPoint)
                {
                    active.Remove(activeIndex);
                }
            }

            return points;
        }
    }
}
