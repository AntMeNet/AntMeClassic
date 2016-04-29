using System;
namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Base-class for all colony-based and index-based states.
    /// </summary>
    [Serializable]
    public abstract class ColonyBasedState : IndexBasedState
    {

        /// <summary>
        /// Constructor of this state.
        /// </summary>
        /// <param name="colonyId">colony-id</param>
        /// <param name="id">id</param>
        public ColonyBasedState(int colonyId, int id)
            : base(id)
        {
            ColonyId = colonyId;
        }

        /// <summary>
        /// Gets the colony-id of this state.
        /// </summary>
        public int ColonyId { get; set; }
    }
}