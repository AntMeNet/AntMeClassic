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
        /// Der Angriffmodifikator der Kaste.
        /// </summary>
        public int Attack = 0;

        /// <summary>
        /// Der Drehgeschwindigkeitmodifikator der Kaste.
        /// </summary>
        public int RotationSpeed = 0;

        /// <summary>
        /// Der Energiemodifikator der Kaste.
        /// </summary>
        public int Energy = 0;

        /// <summary>
        /// Der Geschwindigkeitmodifikator der Kaste.
        /// </summary>
        public int Speed = 0;

        /// <summary>
        /// Der Lastmodifikator der Kaste.
        /// </summary>
        public int Load = 0;

        /// <summary>
        /// Der Name der Kaste.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Der Reichweitenmodifikator der Kaste.
        /// </summary>
        public int Range = 0;

        /// <summary>
        /// Der Sichtweitenmodifikator der Kaste.
        /// </summary>
        public int ViewRange = 0;

        /// <summary>
        /// Prüft, ob diese Ameisenkaste den Regeln entspricht
        /// </summary>
        /// <throws>RuleViolationException</throws>
        public void Rulecheck(string aiName)
        {
            // Ignoriere die Kaste, wenn er keinen Namen hat.
            if (string.IsNullOrEmpty(Name))
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleNoName, aiName));
            }

            // Prüfen, ob der Geschindwigkeitsmodifikator im Rahmen ist
            if (Speed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Speed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleSpeedFailed, Name, aiName));
            }

            // Prüfen, ob der Drehgeschwindigkeitsmodifikator im Rahmen ist
            if (RotationSpeed < SimulationSettings.Custom.CasteSettings.MinIndex ||
                RotationSpeed > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleRotationSpeedFailed,
                        Name,
                        aiName));
            }

            // Prüfen, ob der Lastmodifikator im Rahmen ist
            if (Load < SimulationSettings.Custom.CasteSettings.MinIndex ||
                Load > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCoreCasteRuleLoadFailed, Name, aiName));
            }

            // Prüfen, ob der Sichtweitemodifikator im Rahmen ist
            if (ViewRange < SimulationSettings.Custom.CasteSettings.MinIndex ||
                ViewRange > SimulationSettings.Custom.CasteSettings.MaxIndex)
            {
                throw new RuleViolationException(
                    string.Format(
                        Resource.SimulationCoreCasteRuleViewRangeFailed, Name, aiName));
            }

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
        public CasteState CreateState(int colonyId, int id)
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