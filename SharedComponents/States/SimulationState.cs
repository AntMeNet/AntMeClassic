using System;
using System.Collections.ObjectModel;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds the information of one single simulation-step
    /// </summary>
    [Serializable]
    public class SimulationState
    {

        #region Konstruction

        /// <summary>
        /// Constructor to initialize the lists.
        /// </summary>
        public SimulationState()
        {
            TimeStamp = DateTime.Now;
            BugStates = new Collection<BugState>();
            FruitStates = new Collection<FruitState>();
            TeamStates = new Collection<TeamState>();
            SugarStates = new Collection<SugarState>();
            CustomFields = new CustomState();
        }

        /// <summary>
        /// Constructor to initialize the lists and set the basic parameters.
        /// </summary>
        /// <param name="width">width of the playground</param>
        /// <param name="height">height of the playground</param>
        /// <param name="round">the current round</param>
        /// <param name="rounds">the number of total rounds</param>
        public SimulationState(int width, int height, int round, int rounds) :
            this()
        {
            PlaygroundWidth = width;
            PlaygroundHeight = height;
            CurrentRound = round;
            TotalRounds = rounds;
        }

        /// <summary>
        /// Constructor to initialize the lists and set the basic parameters.
        /// </summary>
        /// <param name="width">width of the playground</param>
        /// <param name="height">height of the playground</param>
        /// <param name="round">the current round</param>
        /// <param name="rounds">the number of total rounds</param>
        /// <param name="time">the time-stamp of this simulation-state</param>
        public SimulationState(int width, int height, int round, int rounds, DateTime time) :
            this(width, height, round, rounds)
        {
            TimeStamp = time;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of bugs.
        /// </summary>
        public Collection<BugState> BugStates { get; set; }

        /// <summary>
        /// Gets a list of fruits.
        /// </summary>
        public Collection<FruitState> FruitStates { get; set; }

        public Collection<ColonyState> ColonyStates
        {
            get
            {
                Collection<ColonyState> colonies = new Collection<ColonyState>();
                foreach (TeamState team in TeamStates)
                    foreach (ColonyState colony in team.ColonyStates)
                        colonies.Add(colony);
                return colonies;
            }
        }

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        public Collection<TeamState> TeamStates { get; set; }

        /// <summary>
        /// Gets a list of sugar.
        /// </summary>
        public Collection<SugarState> SugarStates { get; set; }

        /// <summary>
        /// Gets the list of custom fields.
        /// </summary>
        public CustomState CustomFields { get; set; }

        /// <summary>
        /// Gets or sets the time-stamp of this simulation-state.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the number of total rounds.
        /// </summary>
        public int TotalRounds { get; set; }

        /// <summary>
        /// Gets or sets the number of current round.
        /// </summary>
        public int CurrentRound { get; set; }

        /// <summary>
        /// Gets or sets the width of the playground.
        /// </summary>
        public int PlaygroundWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the playground.
        /// </summary>
        public int PlaygroundHeight { get; set; }

        #endregion
    }
}