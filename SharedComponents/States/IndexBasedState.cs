using System;
namespace AntMe.SharedComponents.States {
    /// <summary>
    /// Base-class for all index-based states
    /// </summary>
    [Serializable]
    public abstract class IndexBasedState : IComparable<IndexBasedState> {

        /// <summary>
        /// Constructor of this state.
        /// </summary>
        /// <param name="id"></param>
        public IndexBasedState(int id) {
            Id = id;
        }

        /// <summary>
        /// Gets the id of this state.
        /// </summary>
        public int Id { get; set; }

        #region IComparable<IndexBasedState> Member

        /// <summary>
        /// Compares two IndexBasedStates
        /// </summary>
        /// <param name="other">other state</param>
        /// <returns>compare-result</returns>
        public int CompareTo(IndexBasedState other)
        {
            return Id.CompareTo(other.Id);
        }

        #endregion
    }
}