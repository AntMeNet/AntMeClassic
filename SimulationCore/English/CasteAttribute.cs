using System;

namespace AntMe.English {
    /// <summary>
    /// Attribute to descripe the different professions of ants
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CasteAttribute : Attribute {
        /// <summary>
        /// The Attackstrength
        /// </summary>
        public int AttackModificator = 0;

        /// <summary>
        /// Hitpoints
        /// </summary>
        public int EnergyModificator = 0;

        /// <summary>
        /// Load
        /// </summary>
        public int LoadModificator = 0;

        /// <summary>
        /// name
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// range
        /// </summary>
        public int RangeModificator = 0;

        /// <summary>
        /// Rotationspeed
        /// </summary>
        public int RotationSpeedModificator = 0;

        /// <summary>
        /// Spped
        /// </summary>
        public int SpeedModificator = 0;

        /// <summary>
        /// viewrange
        /// </summary>
        public int ViewRangeModificator = 0;
    }
}