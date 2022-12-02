using System;

namespace AntMe.English
{
    /// <summary>
    /// Attribute to describe the different professions of ants.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class CasteAttribute : Attribute
    {
        /// <summary>
        /// The attack strength modifier.
        /// </summary>
        public int AttackModifier = 0;

        /// <summary>
        /// Energy or hit points modifier.
        /// </summary>
        public int EnergyModifier = 0;

        /// <summary>
        /// Load modifier.
        /// </summary>
        public int LoadModifier = 0;

        /// <summary>
        /// Name.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Range modifier.
        /// </summary>
        public int RangeModifier = 0;

        /// <summary>
        /// Rotation speed modifier.
        /// </summary>
        public int RotationSpeedModifier = 0;

        /// <summary>
        /// Speed modifier.
        /// </summary>
        public int SpeedModifier = 0;

        /// <summary>
        /// View range modifier.
        /// </summary>
        public int ViewRangeModifier = 0;
    }
}