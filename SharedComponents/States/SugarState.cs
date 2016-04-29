using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about sugar.
    /// </summary>
    [Serializable]
    public class SugarState : IndexBasedState
    {
        /// <summary>
        /// Constructor of sugar-state
        /// </summary>
        /// <param name="id">id</param>
        public SugarState(int id) : base(id) { }

        #region Properties

        /// <summary>
        /// Gets or sets the load of sugar.
        /// </summary>
        public int Amount { get; set; }

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

        #endregion
    }
}