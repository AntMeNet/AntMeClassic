using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using AntMe.Simulation;
using System.Collections.Generic;

namespace AntMe.Plugin.Simulation {
    /// <summary>
    /// Plugin-Class to handle access to simulation-core.
    /// </summary>
    [Preselected]
    public class SimulatorPlugin : IProducerPlugin {
        private readonly string name = Resource.SimulatorPluginName;
        private readonly string description = Resource.SimulatorPluginDescription;
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly Guid guid = new Guid("BC5609C4-AFEE-4ebe-B541-D00230349EAA");

        private readonly TeamSetup teamSetup;
        private SimulationPluginConfiguration config = new SimulationPluginConfiguration();
        private readonly List<PlayerInfoFilename> players = new List<PlayerInfoFilename>();
        private Simulator sim;
        private bool paused;

        /// <summary>
        /// Creates a new instance of this plugin.
        /// </summary>
        public SimulatorPlugin() {
            teamSetup = new TeamSetup(config, players);
        }

        #region IProducerPlugin Members

        /// <summary>
        /// Gives the state of this plugin.
        /// </summary>
        public PluginState State {
            get {
                if (sim == null) {
                    int count = 0;
                    for (int i = 0; i < config.teams.Length; i++) {
                        count += config.teams[i].Players.Count;
                    }

                    teamSetup.Active = false;
                    return count > 0 ? PluginState.Ready : PluginState.NotReady;
                }
                else {
                    teamSetup.Active = true;
                    return paused ? PluginState.Paused : PluginState.Running;
                }
            }
        }

        /// <summary>
        /// Gives the control for plugin-tab.
        /// </summary>
        public Control Control {
            get { return teamSetup; }
        }

        /// <summary>
        /// Gets or sets the plugin-configuration.
        /// </summary>
        public byte[] Settings {
            get {

                // Enumerate all known files to find outdated items without a relation to knownPlayers
                List<int> knownPlayer = new List<int>();
                for (int i = 0; i < config.knownPlayerFiles.Count; i++) {

                    try {
                        bool hit = false;
                        List<PlayerInfo> result = AiAnalysis.Analyse(config.knownPlayerFiles[i]);

                        // Enumerate included Player
                        foreach (PlayerInfo info in result) {
                            PlayerInfoFilename infoFile = new PlayerInfoFilename(info, config.knownPlayerFiles[i]);
                            if (config.knownPlayer.Contains(infoFile.GetHashCode())) {
                                hit = true;
                            }
                            knownPlayer.Add(infoFile.GetHashCode());
                        }

                        // If there was no hit to the knownPlayer-List, remove File from known-List
                        if (!hit) {
                            config.knownPlayerFiles.RemoveAt(i--);
                        }
                    }
                    catch (Exception) {
                        config.knownPlayerFiles.RemoveAt(i--);
                    }
                }

                // Also enumerate all knownPlayers to find outdated items
                for (int i = 0; i < config.knownPlayer.Count; i++) {
                    if (!knownPlayer.Contains(config.knownPlayer[i])) {
                        config.knownPlayer.RemoveAt(i--);
                    }
                }

                // Cleanup before serialize
                config.configuration.Teams.Clear();

                // Serialize
                XmlSerializer serializer = new XmlSerializer(typeof (SimulationPluginConfiguration));
                MemoryStream puffer = new MemoryStream();
                serializer.Serialize(puffer, config);
                return puffer.ToArray();
            }
            set {
                if (value != null && value.Length > 0) {
                    XmlSerializer serializer = new XmlSerializer(typeof (SimulationPluginConfiguration));
                    MemoryStream puffer = new MemoryStream(value);
                    config = (SimulationPluginConfiguration) serializer.Deserialize(puffer);
                    teamSetup.Configuration = config;

                    // Special Simulation-Settings
                    SimulationSettings.SetCustomSettings(config.configuration.Settings);
                }
            }
        }

        /// <summary>
        /// Starts the simulation.
        /// </summary>
        public void Start() {
            if (State == PluginState.Ready) {
                // Create new simulator
                config.configuration.Teams.Clear();

                // Create the teams
                for (int i = 0; i < config.teams.Length; i++) {
                    if (config.teams[i].Players.Count > 0) {
                        TeamInfo team = new TeamInfo();
                        team.Guid = Guid.NewGuid();
                        team.Name = config.teams[i].Name;

                        for (int j = 0; j < config.teams[i].Players.Count; j++) {
                            PlayerItem player = config.teams[i].Players[j];

                            for (int k = 0; k < players.Count; k++) {
                                if (players[k].File == player.FileName &&
                                    players[k].ClassName == player.ClassName) {
                                    team.Player.Add(players[k]);
                                }
                            }
                        }

                        config.configuration.Teams.Add(team);
                    }
                }

                sim = new Simulator(config.configuration);

                // und starten
                teamSetup.Active = true;
            }

            if (State == PluginState.Paused) {
                paused = false;
            }
        }

        /// <summary>
        /// Stops all running simulations.
        /// </summary>
        public void Stop() {
            if (State == PluginState.Running || State == PluginState.Paused) {
                sim.Unload();
                sim = null;
                teamSetup.Active = false;
            }
        }

        /// <summary>
        /// Suspend running simulation.
        /// </summary>
        public void Pause() {
            paused = true;
        }

        /// <summary>
        /// Gives start-parameter to the plugin.
        /// </summary>
        /// <param name="parameter">startup-parameter</param>
        public void StartupParameter(string[] parameter) {
            teamSetup.AutoDiscoverAiFiles();

            foreach (string param in parameter) {
                if (param.ToUpper().StartsWith("/FILE")) {
                    teamSetup.DirectStart(param.Substring(6).Trim());
                }
            }
        }

        /// <summary>
        /// Set the visibility of this plugin.
        /// </summary>
        /// <param name="visible">visibility</param>
        public void SetVisibility(bool visible) {}

        /// <summary>
        /// Updates UI
        /// </summary>
        /// <param name="state">current state of simulation</param>
        public void UpdateUI(SimulationState state) {}

        /// <summary>
        /// Gets the name of plugin.
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// Gets the description of plugin.
        /// </summary>
        public string Description {
            get { return description; }
        }

        /// <summary>
        /// Gets the version of plugin.
        /// </summary>
        public Version Version {
            get { return version; }
        }

        /// <summary>
        /// Gets the guid of plugin.
        /// </summary>
        public Guid Guid {
            get { return guid; }
        }

        /// <summary>
        /// Fills the given state with simulation-states.
        /// </summary>
        /// <param name="state">state</param>
        public void CreateState(ref SimulationState state) {
            if (sim == null) {
                throw new Exception(Resource.SimulatorPluginNotStarted);
            }

            sim.Step(ref state);

            if (sim.State == SimulatorState.Finished) {
                sim.Unload();
                sim = null;
                paused = false;
            }
        }

        #endregion
    }
}