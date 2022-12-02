using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Team of colonies.
    /// </summary>
    [Serializable]
    public sealed class TeamInfo : ICloneable
    {

        /// <summary>
        /// Guid of Teams.
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// Name of Teams.
        /// </summary>
        public string Name;

        /// <summary>
        /// List of team member players.
        /// </summary>
        private List<PlayerInfo> player;

        #region Constructor and Initialization

        /// <summary>
        /// Team Constructor.
        /// </summary>
        public TeamInfo()
        {
            Guid = System.Guid.NewGuid();
            player = new List<PlayerInfo>();
        }

        /// <summary>
        /// Team Constructor.
        /// </summary>
        /// <param name="player">List of players.</param>
        public TeamInfo(List<PlayerInfo> player)
            : this()
        {
            this.player = player;
        }

        /// <summary>
        /// Team Constructor.
        /// </summary>
        /// <param name="guid">Guid.</param>
        /// <param name="player">List of players.</param>
        public TeamInfo(Guid guid, List<PlayerInfo> player)
            : this(player)
        {
            Guid = guid;
        }

        /// <summary>
        /// Team Constructor.
        /// </summary>
        /// <param name="name">Name of team.</param>
        /// <param name="player">List of players.</param>
        public TeamInfo(string name, List<PlayerInfo> player)
            : this(player)
        {
            Name = name;
        }

        /// <summary>
        /// Team Constructor.
        /// </summary>
        /// <param name="guid">Guid of team.</param>
        /// <param name="name">Name of team.</param>
        /// <param name="player">List of players.</param>
        public TeamInfo(Guid guid, string name, List<PlayerInfo> player)
            : this(player)
        {
            Guid = guid;
            Name = name;
        }

        #endregion

        #region properties

        /// <summary>
        /// List of team members.
        /// </summary>
        public List<PlayerInfo> Player
        {
            get { return player; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Check team against rule set.
        /// </summary>
        public void Rulecheck()
        {
            // Number of players in team.
            if (player == null || player.Count < 1)
            {
                // TODO: resource needs better name
                throw new InvalidOperationException(Resource.SimulationCoreTeamInfoNoName);
            }

            // Check team members against rule set.
            foreach (PlayerInfo info in player)
            {
                info.RuleCheck();
            }
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// Clones team.
        /// </summary>
        /// <returns>Copy of the team.</returns>
        public object Clone()
        {
            TeamInfo output = (TeamInfo)MemberwiseClone();
            output.player = new List<PlayerInfo>(player.Count);
            foreach (PlayerInfo info in player)
            {
                output.player.Add((PlayerInfo)info.Clone());
            }
            return output;
        }

        #endregion
    }
}