using System;
using System.Collections.Generic;

using AntMe.Simulation;

namespace AntMe.Plugin.Simulation {
    /// <summary>
    /// Class, to holds all configuration-information.
    /// </summary>
    [Serializable]
    public sealed class SimulationPluginConfiguration {
        private const int TEAMLIMIT = 8;

        /// <summary>
        /// Creates a new instance of SimulationPluginConfiguration.
        /// </summary>
        public SimulationPluginConfiguration() {
            teams = new TeamItem[8];
            for (int i = 0; i < TEAMLIMIT; i++) {
                teams[i] = new TeamItem();
            }
        }

        /// <summary>
        /// List of known player-files.
        /// </summary>
        public List<string> knownPlayerFiles = new List<string>();

        /// <summary>
        /// List of known player.
        /// </summary>
        public List<int> knownPlayer = new List<int>();

        /// <summary>
        /// List of teams.
        /// </summary>
        public TeamItem[] teams;

        /// <summary>
        /// Holds the current simulation-configuration.
        /// </summary>
        public SimulatorConfiguration configuration = new SimulatorConfiguration();

        /// <summary>
        /// List of known settings-Files.
        /// </summary>
        public List<string> knownSettingFiles = new List<string>();

        /// <summary>
        /// Holds the path to the default settings-file.
        /// </summary>
        public string settingFile = string.Empty;
    }
}