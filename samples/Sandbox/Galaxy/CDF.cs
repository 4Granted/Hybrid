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

using System.Diagnostics;

namespace Sandbox.Galaxy
{
    internal sealed class CDF
    {
        private readonly List<double> vM1 = [];
        private readonly List<double> vY1 = [];
        private readonly List<double> vX1 = [];
        private readonly List<double> vM2 = [];
        private readonly List<double> vY2 = [];
        private readonly List<double> vX2 = [];
        private readonly double rBulge;
        private readonly double fMin;
        private readonly double fMax;
        private readonly double fI0;
        private readonly double fK;
        private readonly double fA;
        private readonly int nSteps;

        internal CDF(double i0, double k, double a, double bulge, double min, double max, int steps)
        {
            fMin = min;
            fMax = max;
            nSteps = steps;

            fI0 = i0;
            fK = k;
            fA = a;
            rBulge = bulge;

            Build(steps);
        }

        public void Build(int steps)
        {
            var h = (fMax - fMin) / steps;

            var y = 0.0;

            vX1.Clear();
            vY1.Clear();

            vX2.Clear();
            vY2.Clear();

            vM1.Clear();
            vM2.Clear();

            vX1.Add(0.0);
            vY1.Add(0.0);

            for (int i = 0; i < steps; i += 2)
            {
                var x = h * (i + 2);

                y += h / 3 * (Intensity(fMin + i * h) + 4 * Intensity(fMin + (i + 1) * h) + Intensity(fMin + (i + 2) * h));

                vM1.Add((y - vY1[^1]) / (2 * h));
                vX1.Add(x);
                vY1.Add(y);
            }

            vM1.Add(0.0);

            if (vM1.Count != vX1.Count || vM1.Count != vY1.Count)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < vY1.Count; i++)
            {
                vY1[i] /= vY1[^1];
                vM1[i] /= vY1[^1];
            }

            vX2.Add(0.0);
            vY2.Add(0.0);

            h = 1.0 / steps;

            for (int i = 1, k = 0; i < steps; i++)
            {
                var p = i * h;

                for (; vY1[k + 1] <= p; k++) ;

                y = vX1[k] + (p - vY1[k]) / vM1[k];

                vM2.Add((y - vY2[^1]) / h);
                vX2.Add(p);
                vY2.Add(y);
            }

            vM2.Add(0.0);

            if (vM2.Count != vX2.Count || vM2.Count != vY2.Count)
            {
                throw new InvalidOperationException();
            }
        }

        public double ProbFromVal(double value)
        {
            if (value < fMin || value > fMax)
            {
                throw new InvalidOperationException();
            }

            var h = 2 * ((fMax - fMin) / nSteps);
            var i = (int)((value - fMin) / h);
            var rem = value - i * h;

            Debug.Assert(i >= 0 && i < vM1.Count);

            return vY1[i] + vM1[i] * rem;
        }

        public double ValFromProb(double value)
        {
            if (value < 0 || value > 1)
            {
                throw new InvalidOperationException();
            }

            var h = 1.0 / (vY2.Count - 1);
            var i = (int)(value / h);
            var rem = value - i * h;

            Debug.Assert(i >= 0 && i < vM2.Count);

            return vY2[i] + vM2[i] * rem;
        }

        private double Intensity(double x)
            => x < rBulge ? IntensityBulge(x, fI0, fK)
            : IntensityDisc(x - rBulge, IntensityBulge(rBulge, fI0, fK), fA);

        private static double IntensityBulge(double R, double I0, double k)
            => I0 * Math.Exp(-k * Math.Pow(R, 0.25));

        private static double IntensityDisc(double R, double I0, double a)
            => I0 * Math.Exp(-R / a);
    }
}
