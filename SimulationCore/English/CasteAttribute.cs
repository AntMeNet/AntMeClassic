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
        public int AttackModifier = 0;

        /// <summary>
        /// Hitpoints
        /// </summary>
        public int EnergyModifier = 0;

        /// <summary>
        /// Load
        /// </summary>
        public int LoadModifier = 0;

        /// <summary>
        /// name
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// range
        /// </summary>
        public int RangeModifier = 0;

        /// <summary>
        /// Rotationspeed
        /// </summary>
        public int RotationSpeedModifier = 0;

        /// <summary>
        /// Spped
        /// </summary>
        public int SpeedModifier = 0;

        /// <summary>
        /// viewrange
        /// </summary>
        public int ViewRangeModifier = 0;
    }
}