using System;
using System.Collections.Generic;

namespace AntMe.Plugin.Simulation {
    /// <summary>
    /// Holds information about team-configuration.
    /// </summary>
    [Serializable]
    public sealed class TeamItem {

        /// <summary>
        /// Name of team.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// List of included players.
        /// </summary>
        public List<PlayerItem> Players = new List<PlayerItem>();
    }
}