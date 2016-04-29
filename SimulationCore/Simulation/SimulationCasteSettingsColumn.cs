using System;
using System.Configuration;

namespace AntMe.Simulation {
    /// <summary>
    /// Holds a set of caste-Settings in one column.
    /// </summary>
    [Serializable]
    public struct SimulationCasteSettingsColumn {
        
        /// <summary>
        /// Minimum attack-value
        /// </summary>
        public const int ATTACK_MINIMUM = 0;

        /// <summary>
        /// Maximum attack-value
        /// </summary>
        public const int ATTACK_MAXIMUM = 999999;

        /// <summary>
        /// Minimum rotationspeed
        /// </summary>
        public const int ROTATIONSPEED_MINIMUM = 0;

        /// <summary>
        /// Maximum rotationspeed
        /// </summary>
        public const int ROTATIONSPEED_MAXIMUM = 360;

        /// <summary>
        /// Minimum rotationspeed
        /// </summary>
        public const int ENERGY_MINIMUM = 1;
        /// <summary>
        /// Maximum rotationspeed
        /// </summary>
        public const int ENERGY_MAXIMUM = 999999;

        /// <summary>
        /// Minimum rotationspeed
        /// </summary>
        public const int SPEED_MINIMUM = 0;
        /// <summary>
        /// Maximum rotationspeed
        /// </summary>
        public const int SPEED_MAXIMUM = 9999;

        /// <summary>
        /// Minimum load
        /// </summary>
        public const int LOAD_MINIMUM = 0;

        /// <summary>
        /// Maximum load
        /// </summary>
        public const int LOAD_MAXIMUM = 9999;

        /// <summary>
        /// Minimum range
        /// </summary>
        public const int RANGE_MINIMUM = 1;

        /// <summary>
        /// Maximum range
        /// </summary>
        public const int RANGE_MAXIMUM = 999999;

        /// <summary>
        /// Minimum viewrange
        /// </summary>
        public const int VIEWRANGE_MINIMUM = 0;

        /// <summary>
        /// Maximum viewrange
        /// </summary>
        public const int VIEWRANGE_MAXIMUM = 9999;

        /// <summary>
        /// Attack-Value (Hit-points per Round)
        /// </summary>
        public int Attack;

        /// <summary>
        /// Rotation-speed (Degrees per Round)
        /// </summary>
        public int RotationSpeed;

        /// <summary>
        /// Hit-points-Value (Total points)
        /// </summary>
        public int Energy;

        /// <summary>
        /// Speed-Value (Steps per Round)
        /// </summary>
        public int Speed;

        /// <summary>
        /// Load-Value (total food-load)
        /// </summary>
        public int Load;

        /// <summary>
        /// Range-Value (total count of steps per life)
        /// </summary>
        public int Range;

        /// <summary>
        /// View-range-Value (range in steps)
        /// </summary>
        public int ViewRange;

        /// <summary>
        /// Checks, if values are valid
        /// </summary>
        public void RuleCheck() {

            if (Attack < ATTACK_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Attack (Aktuell: {0}) muss größer oder gleich {1} sein", Attack, ATTACK_MINIMUM));
            }
            if (Attack > ATTACK_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Attack (Aktuell: {0}) muss kleiner oder gleich {1} sein", Attack, ATTACK_MINIMUM));
            }


            if (RotationSpeed < ROTATIONSPEED_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei RotationSpeed (Aktuell: {0}) muss größer oder gleich {1} sein", RotationSpeed, ROTATIONSPEED_MINIMUM));
            }
            if (RotationSpeed > ROTATIONSPEED_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei RotationSpeed (Aktuell: {0}) muss kleiner oder gleich {1} sein", RotationSpeed, ROTATIONSPEED_MAXIMUM));
            }
            
            if (Energy < ENERGY_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Energy (Aktuell: {0}) muss größer oder gleich {1} sein", Energy, ENERGY_MINIMUM));
            }
            if (Energy > ENERGY_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Energy (Aktuell: {0}) muss kleiner oder gleich {1} sein", Energy, ENERGY_MAXIMUM));
            }

            if (Speed < SPEED_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Speed (Aktuell: {0}) muss größer oder gleich {1} sein", Speed, SPEED_MINIMUM));
            }
            if (Speed > SPEED_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Speed (Aktuell: {0}) muss kleiner oder gleich {1} sein", Speed, SPEED_MAXIMUM));
            }

            if (Load < LOAD_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Load (Aktuell: {0}) muss größer oder gleich {1} sein", Load, LOAD_MINIMUM));
            }
            if (Load > LOAD_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Load (Aktuell: {0}) muss kleiner oder gleich {1} sein", Load, LOAD_MAXIMUM));
            }

            if (Range < RANGE_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Range (Aktuell: {0}) muss größer oder gleich {1} sein", Range, RANGE_MINIMUM));
            }
            if (Range > RANGE_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei Range (Aktuell: {0}) muss kleiner oder gleich {1} sein", Range, RANGE_MAXIMUM));
            }

            if (ViewRange < VIEWRANGE_MINIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei ViewRange (Aktuell: {0}) muss größer oder gleich {1} sein", ViewRange, VIEWRANGE_MINIMUM));
            }
            if (ViewRange > VIEWRANGE_MAXIMUM)
            {
                throw new ConfigurationErrorsException(string.Format("Der Wert bei ViewRange (Aktuell: {0}) muss kleiner oder gleich {1} sein", ViewRange, VIEWRANGE_MINIMUM));
            }
        }
    }
}