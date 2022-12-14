using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about ant-castes
    /// </summary>
    [Serializable]
    public class CasteState : ColonyBasedState
    {

        /// <summary>
        /// Constructor of caste-state
        /// </summary>
        /// <param name="colonyId">Id of colony</paraparam>
        /// <param name="id">Id of caste</param>
        public CasteState(int colonyId, int id) : base(colonyId, id) { }

        #region Properties

        /// <summary>
        /// Gets or sets the attack-modificator.
        /// </summary>
        public byte AttackModificator { get; set; }

        /// <summary>
        /// Gets or sets the load-modificator.
        /// </summary>
        public byte LoadModificator { get; set; }

        /// <summary>
        /// Gets or sets the name of this caste.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the smell range modificator.
        /// </summary>
        public byte RangeModificator { get; set; }

        /// <summary>
        /// Gets or sets the rotation speed modificator.
        /// </summary>
        public byte RotationSpeedModificator { get; set; }

        /// <summary>
        /// Gets or sets the speed modificator.
        /// </summary>
        public byte SpeedModificator { get; set; }

        /// <summary>
        /// Gets or sets the view range modificator.
        /// </summary>
        public byte ViewRangeModificator { get; set; }

        /// <summary>
        /// Gets or sets the vitality modificator.
        /// </summary>
        public byte VitalityModificator { get; set; }

        #endregion
    }
}