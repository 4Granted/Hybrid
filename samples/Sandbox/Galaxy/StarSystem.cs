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
    internal sealed class StarSystem : StellarObject
    {
        /// <summary>
        /// Gets the stars of the system.
        /// </summary>
        public IReadOnlyList<StarObject> Stars => [];

        /// <summary>
        /// Gets the planets of the system.
        /// </summary>
        public IReadOnlyList<PlanetObject> Planets => [];
    }

    internal sealed class StarObject : StellarObject
    {
        /// <summary>
        /// Gets the type of the star.
        /// </summary>
        public StarType Type { get; }

        /// <summary>
        /// Gets the class of the star.
        /// </summary>
        public StarClass Class { get; }
    }

    internal sealed class PlanetObject : StellarObject
    {

    }

    /// <summary>
    /// Defines the classification of a planet.
    /// </summary>
    internal enum PlanetClass
    {
        /// <summary>
        /// The planet is classified as terrestial.
        /// </summary>
        Terrestrial,

        /// <summary>
        /// The planet is classified as a gas giant.
        /// </summary>
        GasGiant,

        /// <summary>
        /// The planet is classified as an ice giant.
        /// </summary>
        IceGiant,
    }

    /// <summary>
    /// Defines the Harvard spectral classification of a star.
    /// </summary>
    internal enum StarClass
    {
        /// <summary>
        /// Stars that do not have a classification.
        /// </summary>
        None,

        /// <summary>
        /// Stars with a temperature of 33,00 Kelvin or greater.
        /// </summary>
        ClassO,

        /// <summary>
        /// Stars with a temperature of 10,000 to 33,000 Kelvin.
        /// </summary>
        ClassB,

        /// <summary>
        /// Stars with a temperature of 7,300 to 10,000 Kelvin.
        /// </summary>
        ClassA,

        /// <summary>
        /// Stars with a temperature of 6,000 to 7,300 Kelvin.
        /// </summary>
        ClassF,

        /// <summary>
        /// Stars with a temperature of 5,300 to 6,000 Kelvin.
        /// </summary>
        ClassG,

        /// <summary>
        /// Stars with a temperature of 3,900 to 5,300 Kelvin.
        /// </summary>
        ClassK,

        /// <summary>
        /// Stars with a temperature of 2,300 to 3,900 Kelvin.
        /// </summary>
        ClassM,
    }

    /// <summary>
    /// Defines the type of a star.
    /// </summary>
    internal enum StarType
    {
        /// <summary>
        /// The star is a main sequence star.
        /// </summary>
        MainSequence,

        /// <summary>
        /// The star is a blackhole.
        /// </summary>
        Blackhole,

        /// <summary>
        /// The star is a nuetron star.
        /// </summary>
        NeutronStar,
    }

    /// <summary>
    /// Defines the amount of stars in a system.
    /// </summary>
    internal enum SystemClass
    {
        /// <summary>
        /// The system has one star.
        /// </summary>
        Unary,

        /// <summary>
        /// The system has two stars.
        /// </summary>
        Binary,

        /// <summary>
        /// The system has three stars.
        /// </summary>
        Trinary,
    }
}
