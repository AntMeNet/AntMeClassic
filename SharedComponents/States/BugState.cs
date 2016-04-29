using System;

namespace AntMe.SharedComponents.States {
    /// <summary>
    /// Holds information about bugs.
    /// </summary>
    [Serializable]
    public class BugState : IndexBasedState {

        /// <summary>
        /// Constructor of bugstate.
        /// </summary>
        /// <param name="id">id</param>
        public BugState(int id) : base(id) {}

        /// <summary>
        /// Gets or sets the x-part of the position.
        /// </summary>
        public int PositionX {get;set;}

        /// <summary>
        /// Gets or sets the y-part of the position.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// Gets or sets the vitality.
        /// </summary>
        public int Vitality { get; set; }
    }
}