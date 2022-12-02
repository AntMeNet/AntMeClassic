using AntMe.SharedComponents.States;
using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Holds the caste-information.
    /// </summary>
    [Serializable]
    public sealed class CasteInfo
    {
        /// <summary>
        /// Attack modificator of the caste.
        /// </summary>
        public int Attack = 0;

        /// <summary>
        /// Rotation speed modificator of the caste.
        /// </summary>
        public int RotationSpeed = 0;

        /// <summary>
        /// Energy modificator of the caste.
        /// </summary>
        public int Energy = 0;

        /// <summary>
        /// Movement speed modificator of the caste.
        /// </summary>
        public int Speed = 0;

        /// <summary>
        /// Load modificator of the caste.
        /// </summary>
        public int Load = 0;

        /// <summary>
        /// Name of the caste.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Movement range modificator of the caste.
        /// </summary>
        public int Range = 0;

        /// <summary>
        /// View range modificator of the caste.
        /// </summary>
        public int ViewRange = 0;

        /// <summary>
        /// Checks caste against rule set.
        /// </summary>
        /// <throws>RuleViolationException</throws>
        public void Rulecheck(string aiName)
        {
            // Caste is ignored if it has no name.
            if (string.IsNullOrEmpty(Name))
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleNoName, aiName));
            }

            // Speed modificator check against allowed minimum and maximum.
            if (Speed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Speed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleSpeedFailed, Name, aiName));
            }

            // Rotation speed modificator check against allowed minimum and maximum.
            if (RotationSpeed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                RotationSpeed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleRotationSpeedFailed,
                        Name,
                        aiName));
            }

            // Load modificator check against allowed minimum and maximum.
            if (Load < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Load > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleLoadFailed, Name, aiName));
            }

            // View range modificator check against allowed minimum and maximum.
            if (ViewRange < SimulationSettings.Custom.CasteSettings.MinIndex ||
                ViewRange > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleViewRangeFailed, Name, aiName));
            }

            // Range modificator check against allowed minimum and maximum.
            if (Range < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Range > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleRangeFailed, Name, aiName));
            }

            // Energy modificator check against allowed minimum and maximum.
            if (Energy < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Energy > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleEnergyFailed, Name, aiName));
            }

            // Attack modificator check against allowed minimum and maximum.
            if (Attack < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Attack > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleAttackFailed, Name, aiName));
            }

            // Check the sum of all modificator against the allowed sum.
            if (Speed +
                RotationSpeed +
                Load +
                ViewRange +
                Range +
                Energy +
                Attack > SimulationSettings.Custom.CasteSettings.Sum)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleSumFailed, Name, aiName));
            }
        }

        /// <summary>
        /// Indicates whether this ant caste is the default caste.
        /// </summary>
        /// <returns>true for default caste</returns>
        public bool IsEmpty()
        {
            return Name == String.Empty &&
                   Attack == 0 &&
                   RotationSpeed == 0 &&
                   Energy == 0 &&
                   Speed == 0 &&
                   Load == 0 &&
                   Range == 0 &&
                   ViewRange == 0;
        }

        /// <summary>
        /// Creates a caste state object.
        /// </summary>
        /// <returns></returns>
        public CasteState CreateCasteStateInfo(int colonyId, int id)
        {
            CasteState state = new CasteState(colonyId, id);
            state.Name = Name;
            state.SpeedModificator = (byte)Speed;
            state.RotationSpeedModificator = (byte)RotationSpeed;
            state.LoadModificator = (byte)Load;
            state.ViewRangeModificator = (byte)ViewRange;
            state.RangeModificator = (byte)Range;
            state.VitalityModificator = (byte)Energy;
            state.AttackModificator = (byte)Attack;
            return state;
        }
    }
}