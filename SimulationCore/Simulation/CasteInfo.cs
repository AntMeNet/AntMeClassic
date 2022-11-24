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
        /// attack modificator of the caste
        /// </summary>
        public int Attack = 0;

        /// <summary>
        /// rotation speed modificator of the caste
        /// </summary>
        public int RotationSpeed = 0;

        /// <summary>
        /// energy modificator of the caste
        /// </summary>
        public int Energy = 0;

        /// <summary>
        /// movement speed modificator of the caste
        /// </summary>
        public int Speed = 0;

        /// <summary>
        /// load modificator of the caste
        /// </summary>
        public int Load = 0;

        /// <summary>
        /// name of the caste
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// movement range modificator of the caste
        /// </summary>
        public int Range = 0;

        /// <summary>
        /// view range modificator of the caste
        /// </summary>
        public int ViewRange = 0;

        /// <summary>
        /// checks caste against rule set
        /// </summary>
        /// <throws>RuleViolationException</throws>
        public void Rulecheck(string aiName)
        {
            // caste is ignored if it has no name
            if (string.IsNullOrEmpty(Name))
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleNoName, aiName));
            }

            // speed modificator check against allowed minimum and maximum
            if (Speed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Speed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleSpeedFailed, Name, aiName));
            }

            // rotation speed modificator check against allowed minimum and maximum
            if (RotationSpeed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                RotationSpeed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleRotationSpeedFailed,
                        Name,
                        aiName));
            }

            // load modificator check against allowed minimum and maximum
            if (Load < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Load > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleLoadFailed, Name, aiName));
            }

            // view range modificator check against allowed minimum and maximum
            if (ViewRange < SimulationSettings.Custom.CasteSettings.MinIndex ||
                ViewRange > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleViewRangeFailed, Name, aiName));
            }

            // smell range modificator check against allowed minimum and maximum
            // Prüfen, ob der Riechweitemodifikator im Rahmen ist
            if (Range < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Range > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleRangeFailed, Name, aiName));
            }

            // Prüfen, ob der Energiemodifikator im Rahmen ist
            if (Energy < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Energy > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleEnergyFailed, Name, aiName));
            }

            // Prüfen, ob der Angriffsmodifikator im Rahmen ist
            if (Attack < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Attack > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleAttackFailed, Name, aiName));
            }

            // Prüfen, ob die Eigenschaftssumme stimmt
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
        /// Gibt an, ob es sich bei dieser Ameisenkaste um die Standard-Kaste handelt
        /// </summary>
        /// <returns>Standardwert</returns>
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
        /// Erzeugt ein CasteState-Objekt.
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