using System;
using System.Collections.ObjectModel;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds the information of a team of multiple colonies.
    /// </summary>
    [Serializable]
    public class TeamState : IndexBasedState
    {
        /// <summary>
        /// Constructor of team-state
        /// </summary>
        /// <param name="id">id</param>
        public TeamState(int id)
            : base(id)
        {
            ColonyStates = new Collection<ColonyState>();
        }

        /// <summary>
        /// Constructor of team-state
        /// </summary>
        /// <param name="id">id of this team</param>
        /// <param name="guid"><c>guid</c></param>
        /// <param name="name">Name of this team</param>
        public TeamState(int id, Guid guid, string name)
            : this(id)
        {
            Guid = guid;
            Name = name;
        }

        #region Properties

        /// <summary>
        /// gets a list of castes.
        /// </summary>
        public Collection<ColonyState> ColonyStates { get; set; }

        /// <summary>
        /// Gets or sets the <c>guid</c> of the team.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}