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

namespace Sandbox.Galaxy
{
    internal static class MST
    {
        public static IReadOnlyList<Hyperlane> Kruskal(
            List<Hyperlane> edges, int vertices)
        {
            edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            // Initialize Disjoint Set (Union-Find).
            var ds = new DisjointSet(vertices);

            // This will store the final MST.
            var mst = new List<Hyperlane>();

            // Process edges in order of increasing weight.
            foreach (var edge in edges)
            {
                // Check if adding the edge forms a cycle
                if (ds.Find(edge.Source) != ds.Find(edge.Destination))
                {
                    // If not, include the edge in the MST and union their sets
                    mst.Add(edge);

                    ds.Union(edge.Source, edge.Destination);

                    // If we've already chosen (vertexCount - 1) edges, we can stop
                    if (mst.Count == vertices - 1)
                    {
                        break;
                    }
                }
            }

            return mst;
        }
    }
}
