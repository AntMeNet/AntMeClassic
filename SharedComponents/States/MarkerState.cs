using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about a marker.
    /// </summary>
    [Serializable]
    public class MarkerState : ColonyBasedState
    {

        /// <summary>
        /// Constructor of marker-state.
        /// </summary>
        /// <param name="colonyId">Colony-id</param>
        /// <param name="id">id</param>
        public MarkerState(int colonyId, int id) : base(colonyId, id) { }

        #region Properties

        /// <summary>
        /// Gets or sets the x-part of the position.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Gets or sets the y-part of the position.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public int Direction { get; set; }

        #endregion
    }
}