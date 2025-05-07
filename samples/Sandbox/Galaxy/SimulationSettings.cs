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
    internal sealed class SimulationSettings
    {
        public float Angle
        {
            get
            {
                const float MIN = 0.0001f;
                const float MAX = 0.0008f;

                return MIN + DeltaAngle * (MAX - MIN);
            }
        }
        public float CoreRadius => GalaxyRadius * Core;

        public float TimeStep = 2000f; // 2,000
        public float GalaxyRadius = 13000f; // 13,000
        public float Core = 0.31f; // 0.31
        public float DeltaAngle = 0.43f; // 0.43
        public float Ex1 = 0.63f; // 0.85
        public float Ex2 = 1.09f; // 0.95
        public float PertAmp = 75f; // 40
        public float BaseTemp = 2700f; // 4,000
        public float StarSize = 8f; // 4
        public float ParticleSize = 1000f; // 500
        public int StarCount = 50000; // 100,000
        public int PertN = 2; // 2
        public bool HasDarkMatter = true;
        public bool StarsEnabled = false;
        public bool DustEnabled = true;
        public bool FilamentsEnabled = true;
        public bool HasChanged = true;
    }
}
