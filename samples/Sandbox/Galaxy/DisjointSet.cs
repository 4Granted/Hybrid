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
    internal class DisjointSet
    {
        private readonly int[] parent;
        private readonly int[] rank;

        internal DisjointSet(int size)
        {
            parent = new int[size];
            rank = new int[size];

            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]);
            }

            return parent[x];
        }

        public void Union(int x, int y)
        {
            var xRoot = Find(x);
            var yRoot = Find(y);

            if (xRoot == yRoot) return;

            if (rank[xRoot] < rank[yRoot])
            {
                parent[xRoot] = yRoot;
            }
            else if (rank[xRoot] > rank[yRoot])
            {
                parent[yRoot] = xRoot;
            }
            else
            {
                parent[yRoot] = xRoot;
                rank[xRoot]++;
            }
        }
    }
}
