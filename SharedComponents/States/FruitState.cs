using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about fruit.
    /// </summary>
    [Serializable]
    public class FruitState : IndexBasedState
    {

        /// <summary>
        /// Constructor of fruit-state.
        /// </summary>
        /// <param name="id">id</param>
        public FruitState(int id) : base(id) { }

        /// <summary>
        /// Gets or sets the amount of fruit.
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

        /// <summary>
        /// Gets or sets the number of carrying ants.
        /// </summary>
        public byte CarryingAnts { get; set; }
    }
}