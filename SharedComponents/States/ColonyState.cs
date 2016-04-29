using System;
using System.Collections.ObjectModel;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds the information of one colony in a simulation-state.
    /// </summary>
    [Serializable]
    public class ColonyState : IndexBasedState
    {
        /// <summary>
        /// Constructor of colony-state
        /// </summary>
        /// <param name="id">id</param>
        public ColonyState(int id)
            : base(id)
        {
            AntStates = new Collection<AntState>();
            AnthillStates = new Collection<AnthillState>();
            MarkerStates = new Collection<MarkerState>();
            CasteStates = new Collection<CasteState>();
        }

        /// <summary>
        /// Constructor of colony-state
        /// </summary>
        /// <param name="id">id of this colony</param>
        /// <param name="guid"><c>guid</c></param>
        /// <param name="colonyName">Name of this colony</param>
        /// <param name="playerName">Name of player</param>
        public ColonyState(int id, Guid guid, string colonyName, string playerName)
            : this(id)
        {
            Guid = guid;
            ColonyName = colonyName;
            PlayerName = playerName;
        }

        #region Properties

        /// <summary>
        /// Gets a list of ants.
        /// </summary>
        public Collection<AntState> AntStates { get; set; }

        /// <summary>
        /// Gets a list of anthills.
        /// </summary>
        public Collection<AnthillState> AnthillStates { get; set; }

        /// <summary>
        /// Gets a list of markers.
        /// </summary>
        public Collection<MarkerState> MarkerStates { get; set; }

        /// <summary>
        /// gets a list of castes.
        /// </summary>
        public Collection<CasteState> CasteStates { get; set; }

        /// <summary>
        /// Gets or sets the guid of the colony.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name of this colony.
        /// </summary>
        public string ColonyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the count of starved ants.
        /// </summary>
        public int StarvedAnts { get; set; }

        /// <summary>
        /// Gets or sets the count of eaten ants.
        /// </summary>
        public int EatenAnts { get; set; }

        /// <summary>
        /// Gets or sets the count of beaten ants.
        /// </summary>
        public int BeatenAnts { get; set; }

        /// <summary>
        /// Gets or sets the count of killed bugs.
        /// </summary>
        public int KilledBugs { get; set; }

        /// <summary>
        /// Gets or sets the count of killed enemies.
        /// </summary>
        public int KilledEnemies { get; set; }

        /// <summary>
        /// Gets or sets the amount of collected food.
        /// </summary>
        public int CollectedFood { get; set; }

        /// <summary>
        /// Gets or sets the amount of collected fruits.
        /// </summary>
        public int CollectedFruits { get; set; }

        /// <summary>
        /// Gets or sets the total points.
        /// </summary>
        public int Points { get; set; }

        #endregion
    }
}