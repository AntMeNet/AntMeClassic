using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about an anthill.
    /// </summary>
    [Serializable]
    public class AnthillState : ColonyBasedState
    {

        #region Constructor

        /// <summary>
        /// Constructor of anthill-state
        /// </summary>
        /// <param name="colonyId">Colony-id</param>
        /// <param name="id">id</param>
        public AnthillState(int colonyId, int id) : base(colonyId, id) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x-part of position.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Gets or sets the y-part of position.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        public int Radius { get; set; }

        #endregion
    }
}