using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using AntMe.Plugin.Simulation.Properties;
using AntMe.Simulation;
using System.Diagnostics;

namespace AntMe.Plugin.Simulation {
    internal partial class TeamSetup : UserControl {
        private SimulationPluginConfiguration config;
        private readonly List<PlayerInfoFilename> players;
        private bool ignoreUpdates;
        private bool active;

        public TeamSetup(SimulationPluginConfiguration config, List<PlayerInfoFilename> players) {
            this.players = players;
            Configuration = config;

            InitializeComponent();
            ignoreUpdates = true;

            ignoreUpdates = false;
            timer.Enabled = true;
            UpdatePanel();
        }

        #region internal Methods

        /// <summary>
        /// Loads all players from filename and add them to the global player-list.
        /// </summary>
        /// <param name="filename">filename</param>
        /// <param name="knownOnly">if only known players should be added</param>
        /// <returns>true, if there was no Exception</returns>
        /// <param name="silent">starts silent start without error-messages</param>
        private List<PlayerInfoFilename> loadPlayerFile(string filename, bool knownOnly, bool silent) {
            List<PlayerInfo> foundPlayers = new List<PlayerInfo>();
            List<PlayerInfoFilename> output = new List<PlayerInfoFilename>();
            try {
                FileInfo file = new FileInfo(filename.ToLower());

                // Load playerinfo
                try {
                    foundPlayers.AddRange(AiAnalysis.Analyse(file.FullName, false));
                }
                catch (Exception ex) {
                    if (!silent) {
                        showException(ex);
                    }
                    return output;
                }

                // Add found players
                if (foundPlayers.Count > 0) {
                    if (!config.knownPlayerFiles.Contains(file.FullName)) {
                        config.knownPlayerFiles.Add(file.FullName);
                    }
                    bool found = false;
                    foreach (PlayerInfo playerInfo in foundPlayers) {
                        PlayerInfoFilename info = new PlayerInfoFilename(playerInfo, file.FullName);
                        output.Add(info);

                        if (!players.Contains(info)) {
                            if (knownOnly) {
                                if (!config.knownPlayer.Contains(info.GetHashCode())) {
                                    continue;
                                }
                            }
                            else {
                                if (!config.knownPlayer.Contains(info.GetHashCode())) {
                                    config.knownPlayer.Add(info.GetHashCode());
                                }
                            }
                            players.Add(info);
                            found = true;
                        }
                    }

                    if (!found && !knownOnly && !silent) {
                        MessageBox.Show(
                            this,
                            string.Format(Resource.SimulatorPluginTeamSetupStillLoaded, file.FullName),
                            Resource.SimulationPluginMessageBoxTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else {
                    if (!silent) {
                        MessageBox.Show(
                            this,
                            string.Format(Resource.SimulatorPluginTeamSetupNoFolksFound, file.FullName),
                            Resource.SimulationPluginMessageBoxTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) {
                showException(
                    new Exception(
                        string.Format(
                            Resource.SimulatorPluginTeamSetupFileloadException,
                            filename,
                            ex.Message),
                        ex));
            }

            UpdatePanel();
            return output;
        }

        /// <summary>
        /// removes the whole player from game.
        /// </summary>
        /// <param name="player"></param>
        private void removePlayer(PlayerInfoFilename player) {
            // Remove from List
            if (players.Contains(player)) {
                players.Remove(player);
            }

            // Remove from known players
            players.Remove(player);
            config.knownPlayer.Remove(player.GetHashCode());

            // Remove from teams
            for (int i = 0; i < config.teams.Length; i++) {
                for (int j = 0; j < config.teams[i].Players.Count; j++) {
                    PlayerItem item = config.teams[i].Players[j];

                    if (item.FileName == player.File && item.ClassName == player.ClassName) {
                        config.teams[i].Players.Remove(item);
                        j--;
                    }
                }
            }

            // Remove known files, if no more players enabled
            bool hit = false;
            for (int i = 0; i < players.Count; i++) {
                if (players[i].File == player.File) {
                    hit = true;
                    break;
                }
            }
            if (!hit) {
                config.knownPlayerFiles.Remove(player.File);
            }

            UpdatePanel();
        }

        /// <summary>
        /// Shows the property-window for ants.
        /// </summary>
        /// <param name="player">target player</param>
        private void playerProperties(PlayerInfoFilename player) {
            AntProperties properties = new AntProperties(player);
            properties.ShowDialog(this);
        }

        /// <summary>
        /// Adds a <see cref="PlayerItem"/> to a team
        /// </summary>
        /// <param name="item">item</param>
        /// <param name="team">team, or null for a new team</param>
        private void addPlayerToTeam(PlayerItem[] item, TeamItem team) {
            // Find the right team, if target-team is null
            if (team == null) {
                int items = SimulatorConfiguration.PLAYERLIMIT + 1;

                // If team is null, search for empty team
                for (int i = 0; i < config.teams.Length; i++) {
                    if (config.teams[i].Players.Count < items) {
                        team = config.teams[i];
                        items = team.Players.Count;
                    }
                }

                // If there are no more empty teams, use the last one
                if (team == null) {
                    team = config.teams[config.teams.Length - 1];
                }
            }

            // Find out, how many players are enabled
            int activePlayers = 0;
            for (int i = 0; i < config.teams.Length; i++) {
                activePlayers += config.teams[i].Players.Count;
            }

            // Add to playerlist
            for (int i = 0; i < item.Length; i++) {
                if (activePlayers >= SimulatorConfiguration.PLAYERLIMIT) {
                    break;
                }

                team.Players.Add(item[i]);
                activePlayers++;
            }

            UpdatePanel();
        }

        /// <summary>
        /// Adds a player to a team.
        /// </summary>
        /// <param name="info">player</param>
        /// <param name="team">team</param>
        private void addPlayerToTeam(PlayerInfoFilename info, TeamItem team) {
            if (ReadyCheck(info, false, false)) {
                addPlayerToTeam(new PlayerItem[] {new PlayerItem(info)}, team);
            }
        }

        /// <summary>
        /// Adds player to a new team.
        /// </summary>
        /// <param name="info">List of players to add</param>
        /// <param name="silent">silent mode</param>
        private void addPlayerToTeam(PlayerInfoFilename[] info, bool silent) {
            List<PlayerItem> items = new List<PlayerItem>();
            for (int i = 0; i < info.Length; i++) {
                if (ReadyCheck(info[i], silent, silent)) {
                    items.Add(new PlayerItem(info[i]));
                }
            }
            addPlayerToTeam(items.ToArray(), null);
        }

        /// <summary>
        /// Checks the given player for match-readyness.
        /// </summary>
        /// <param name="player">requested player</param>
        /// <param name="silent">shows no messageboxes and requests</param>
        /// <param name="secure">asume, that the testet player is secure</param>
        /// <returns>true, if everything is fine</returns>
        private bool ReadyCheck(PlayerInfoFilename player, bool silent, bool secure) {

            // Rulevalidation
            if (!RightsRequest.IsValidPlayer(player)) {

                // Messagebox, if not silent
                if (!silent) {
                    MessageBox.Show(
                        this,
                        RightsRequest.GetRuleViolationMessage(player),
                        Resource.SimulationPluginMessageBoxTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }

                return false;
            }

            // if colony is ready to start without request, do that
            if (!RightsRequest.RequestRights(player)) {
                return true;
            }

            // Security-override
            if (secure && !RightsRequest.LockedRights(config, player)) {
                return true;
            }

            // Ask for request
            RightsRequest form = new RightsRequest(config, player);
            if (!silent && form.ShowDialog(this) == DialogResult.Yes) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a player from team
        /// </summary>
        /// <param name="item">player</param>
        private void removePlayerFromTeam(IEnumerable<PlayerItem> item) {
            removePlayerFromTeam(item, true);
        }

        /// <summary>
        /// Removes a player from team
        /// </summary>
        /// <param name="item">player</param>
        /// <param name="bubble">should empty teams bubble out</param>
        private void removePlayerFromTeam(IEnumerable<PlayerItem> item, bool bubble) {
            // Remove item from all teams
            foreach (PlayerItem playerItem in item) {
                for (int i = 0; i < config.teams.Length; i++) {
                    if (config.teams[i].Players.Contains(playerItem)) {
                        config.teams[i].Players.Remove(playerItem);
                    }
                }
            }

            if (bubble) {
                bubbleTeams();
                UpdatePanel();
            }
        }

        /// <summary>
        /// Moves a player to another team.
        /// </summary>
        /// <param name="item">player</param>
        /// <param name="targetTeam">target team</param>
        private void movePlayerToTeam(PlayerItem[] item, TeamItem targetTeam) {
            removePlayerFromTeam(item, false);
            addPlayerToTeam(item, targetTeam);
            bubbleTeams();
            UpdatePanel();
        }

        private void bubbleTeams() {
            int gap = 0;
            for (int i = 0; i < config.teams.Length; i++) {
                if (config.teams[i].Players.Count == 0) {
                    gap++;
                }
                else if (config.teams[i - gap].Players.Count == 0) {
                    TeamItem lower = config.teams[i - gap];
                    TeamItem current = config.teams[i];
                    lower.Players.AddRange(current.Players);
                    current.Players.Clear();
                }
            }
        }

        /// <summary>
        /// resets the whole team-settings.
        /// </summary>
        private void resetTeams() {
            for (int i = 0; i < config.teams.Length; i++) {
                config.teams[i].Players.Clear();
            }
            UpdatePanel();
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        public void UpdatePanel() {
            if (ignoreUpdates) {
                return;
            }

            ignoreUpdates = true;

            settingsLabel.Text = config.configuration.Settings.SettingsName;

            #region playerListUpdate

            // Delete items
            for (int i = 0; i < playerListView.Items.Count; i++) {
                if (!players.Contains((PlayerInfoFilename) playerListView.Items[i].Tag)) {
                    playerListView.Items.RemoveAt(i);
                    i--;
                }
            }

            // Create new items
            ListViewGroup staticGroup = playerListView.Groups["staticGroup"];
            ListViewGroup nonstaticGroup = playerListView.Groups["nonStaticGroup"];
            for (int i = 0; i < players.Count; i++) {
                PlayerInfoFilename info = players[i];
                if (!playerListView.Items.ContainsKey(info.GetHashCode().ToString())) {
                    ListViewItem item = playerListView.Items.Add(info.GetHashCode().ToString(), info.ColonyName, 0);
                    item.Tag = info;
                    item.Group = info.Static ? staticGroup : nonstaticGroup;
                    item.SubItems.Add(
                        string.Format(Resource.SimulatorPluginAntPropertiesAuthorFormat, info.FirstName, info.LastName));
                }
            }

            // Update Icon
            foreach (ListViewItem listViewItem in playerListView.Items) {

                // collect infos
                PlayerInfoFilename playerInfo = (PlayerInfoFilename) listViewItem.Tag;

                bool playerStatic = playerInfo.Static;
                bool playerEnabled = true;
                bool playerSecure = RightsRequest.RequestRights(playerInfo);
                string hintText = string.Empty;

                if (!RightsRequest.IsValidPlayer(playerInfo)) {
                    playerEnabled = false;
                    hintText = RightsRequest.GetRuleViolationMessage(playerInfo);
                }
                else if (RightsRequest.LockedRights(config, playerInfo)) {
                    playerEnabled = false;
                    hintText = RightsRequest.RequiredRightsList(config, playerInfo);
                }

                // Set Information to Item
                listViewItem.ImageKey =
                    (playerStatic ? "static" : "nonstatic") +
                    (!playerEnabled ? "_disabled" : string.Empty) +
                    (playerSecure ? "_secure" : string.Empty);
                listViewItem.ToolTipText = hintText;
            }

            #endregion

            #region teamListUpdate

            // Kick player
            List<string> kickedPlayer = new List<string>();
            for (int i = 0; i < teamListView.Items.Count; i++) {
                PlayerItem player = (PlayerItem) teamListView.Items[i].Tag;
                if (!config.teams[0].Players.Contains(player) &&
                    !config.teams[1].Players.Contains(player) &&
                    !config.teams[2].Players.Contains(player) &&
                    !config.teams[3].Players.Contains(player) &&
                    !config.teams[4].Players.Contains(player) &&
                    !config.teams[5].Players.Contains(player) &&
                    !config.teams[6].Players.Contains(player) &&
                    !config.teams[7].Players.Contains(player)) {
                    teamListView.Items.RemoveAt(i);
                    i--;
                    continue;
                }

                for (int j = 0; j < 8; j++) {
                    if (config.teams[j].Players.Contains(player)) {
                        if (!RightsRequest.IsValidPlayer(player.PlayerInfo) || 
                            RightsRequest.LockedRights(config, player.PlayerInfo)) {

                            kickedPlayer.Add(
                                string.Format(
                                    Resource.SimulationPluginKicklistEntry,
                                    player.ColonyName,
                                    player.FileName,
                                    player.ClassName,
                                    player.AuthorName));
                            teamListView.Items.RemoveAt(i);
                            config.teams[j].Players.Remove(player);
                            i--;
                        }
                        break;
                    }
                }
            }

            if (kickedPlayer.Count > 0) {
                MessageBox.Show(
                    this,
                    Resource.SimulationPluginKicklistHead + Environment.NewLine + Environment.NewLine +
                    string.Join(Environment.NewLine, kickedPlayer.ToArray()),
                    Resource.SimulationPluginMessageBoxTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            // Create new items and update Context-menues
            for (int i = 0; i < config.teams.Length; i++) {
                ListViewGroup group = teamListView.Groups["teamGroup" + i];

                for (int j = 0; j < config.teams[i].Players.Count; j++) {
                    PlayerItem player = config.teams[i].Players[j];

                    if (teamListView.Items.ContainsKey(player.Guid.ToString())) {
                        ListViewItem item = teamListView.Items[player.Guid.ToString()];
                        if (item.Group != group) {
                            item.Group = group;
                        }
                    }
                    else {
                        ListViewItem item = teamListView.Items.Add(
                            player.Guid.ToString(),
                            player.ColonyName,
                            (player.PlayerInfo.Static ? "static" : "nonstatic"));
                        item.SubItems.Add(player.AuthorName);
                        item.Tag = player;
                        item.Group = group;
                    }
                }
            }

            // Update Team-Lists in Context-Menues
            newTeamMenuItem.Enabled = (config.teams[7].Players.Count == 0);
            chooseTeam1MenuItem.Visible = (config.teams[0].Players.Count > 0);
            chooseTeam2MenuItem.Visible = (config.teams[1].Players.Count > 0);
            chooseTeam3MenuItem.Visible = (config.teams[2].Players.Count > 0);
            chooseTeam4MenuItem.Visible = (config.teams[3].Players.Count > 0);
            chooseTeam5MenuItem.Visible = (config.teams[4].Players.Count > 0);
            chooseTeam6MenuItem.Visible = (config.teams[5].Players.Count > 0);
            chooseTeam7MenuItem.Visible = (config.teams[6].Players.Count > 0);
            chooseTeam8MenuItem.Visible = (config.teams[7].Players.Count > 0);

            moveNewTeamMenuItem.Enabled = (config.teams[7].Players.Count == 0);
            moveTeam1MenuItem.Visible = (config.teams[0].Players.Count > 0);
            moveTeam2MenuItem.Visible = (config.teams[1].Players.Count > 0);
            moveTeam3MenuItem.Visible = (config.teams[2].Players.Count > 0);
            moveTeam4MenuItem.Visible = (config.teams[3].Players.Count > 0);
            moveTeam5MenuItem.Visible = (config.teams[4].Players.Count > 0);
            moveTeam6MenuItem.Visible = (config.teams[5].Players.Count > 0);
            moveTeam7MenuItem.Visible = (config.teams[6].Players.Count > 0);
            moveTeam8MenuItem.Visible = (config.teams[7].Players.Count > 0);

            #endregion

            ignoreUpdates = false;
        }

        /// <summary>
        /// Shows a nice message-box with exception-information.
        /// </summary>
        /// <param name="ex">exception</param>
        private void showException(Exception ex) {
            MessageBox.Show(
                this,
                string.Format(Resource.SimulationPluginExcetionIntro, ex.Message),
                Resource.SimulationPluginMessageBoxTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Searches in the current application-path for valid folks.
        /// </summary>
        public void AutoDiscoverAiFiles() {
            string path = Directory.GetCurrentDirectory();

            string[] potentialFiles = Directory.GetFiles(path, Resources.SimulatorPluginFolkFileFilter);

            foreach (string file in potentialFiles) {
                loadPlayerFile(file, false, true);
            }
            UpdatePanel();
        }

        #endregion

        #region Properties

        public bool Active {
            get { return active; }
            set { active = value; }
        }

        public SimulationPluginConfiguration Configuration {
            get { return config; }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                config = value;

                // Reset lists
                players.Clear();

                // Initialize known stuff
                for (int i = 0; i < config.knownPlayerFiles.Count; i++) {
                    loadPlayerFile(config.knownPlayerFiles[i], true, true);
                }

                // Check, if the used player in saved teamsettings still there
                for (int i = 0; i < config.teams.Length; i++) {
                    // If Team is still null
                    if (config.teams[i] == null) {
                        config.teams[i] = new TeamItem();
                        continue;
                    }

                    for (int j = 0; j < config.teams[i].Players.Count; j++) {
                        // If Player is still null
                        if (config.teams[i].Players == null) {
                            config.teams[i].Players = new List<PlayerItem>();
                            continue;
                        }

                        // Search for player
                        PlayerItem item = config.teams[i].Players[j];
                        bool hit = false;
                        foreach (PlayerInfoFilename player in players) {
                            if (player.File == item.FileName &&
                                player.ClassName == item.ClassName) {
                                hit = true;
                                break;
                            }
                        }

                        // kick playerItem, if there is no suitable player
                        if (!hit) {
                            config.teams[i].Players.RemoveAt(j);
                            j--;
                        }
                    }
                }

                // Load Settings
                if (config.settingFile != string.Empty) {
                    try {
                        config.configuration.Settings = SimulationSettings.LoadSettings(config.settingFile);
                        SimulationSettings.SetCustomSettings(config.configuration.Settings);
                    }
                    catch (Exception ex) {
                        showException(ex);
                        config.configuration.Settings.SetDefaults();
                        config.settingFile = string.Empty;
                        config.knownSettingFiles.Remove(config.settingFile);
                    }
                }
                else {
                    config.configuration.Settings.SetDefaults();
                    SimulationSettings.SetCustomSettings(SimulationSettings.Default);
                }
            }
        }

        #endregion

        #region public Methods

        /// <summary>
        /// Prepares Simulation for a direct start via debugger
        /// </summary>
        /// <param name="filename">target file</param>
        public void DirectStart(string filename) {
            resetTeams();
            loadPlayerFile(filename, false, true);

            foreach (PlayerInfoFilename player in players) {
                if (filename.ToLower() == player.File) {
                    addPlayerToTeam(new PlayerInfoFilename[] {player}, true);
                }
            }
        }

        #endregion

        #region Form-Events

        #region Buttons

        private void button_create(object sender, EventArgs e)
        {
            using (CreateForm form = new CreateForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    // Start Visual Studio
                    Process.Start(form.GeneratedSolutionFile);
                }
            }
        }

        private void button_load(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
                foreach (string fileName in openFileDialog.FileNames) {
                    loadPlayerFile(fileName, false, false);
                }
            }
        }

        private void button_remove(object sender, EventArgs e) {
            foreach (ListViewItem listViewItem in playerListView.SelectedItems) {
                PlayerInfoFilename info = (PlayerInfoFilename) listViewItem.Tag;
                removePlayer(info);
            }
        }

        private void button_antProperties(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                PlayerInfoFilename info = (PlayerInfoFilename) playerListView.SelectedItems[0].Tag;
                playerProperties(info);
            }
        }

        private void button_settings(object sender, EventArgs e) {
            SimulationProperties form = new SimulationProperties(config);
            if (form.ShowDialog(this) == DialogResult.OK) {
                UpdatePanel();
            }
        }

        private void button_newTeam(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                PlayerInfoFilename[] selectedPlayer = new PlayerInfoFilename[playerListView.SelectedItems.Count];
                for (int i = 0; i < playerListView.SelectedItems.Count; i++) {
                    selectedPlayer[i] = (PlayerInfoFilename) playerListView.SelectedItems[i].Tag;
                }
                addPlayerToTeam(selectedPlayer, false);
            }
        }

        private void button_newTeam1(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[0]);
                }
            }
        }

        private void button_newTeam2(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[1]);
                }
            }
        }

        private void button_newTeam3(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[2]);
                }
            }
        }

        private void button_newTeam4(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[3]);
                }
            }
        }

        private void button_newTeam5(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[4]);
                }
            }
        }

        private void button_newTeam6(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[5]);
                }
            }
        }

        private void button_newTeam7(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[6]);
                }
            }
        }

        private void button_newTeam8(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                foreach (ListViewItem item in playerListView.SelectedItems) {
                    PlayerInfoFilename info = (PlayerInfoFilename) item.Tag;
                    addPlayerToTeam(info, config.teams[7]);
                }
            }
        }

        private void button_removeFromTeam(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                removePlayerFromTeam(playerItems);
            }
        }

        private void button_moveNewTeam(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, null);
            }
        }

        private void button_moveTeam1(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[0]);
            }
        }

        private void button_moveTeam2(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[1]);
            }
        }

        private void button_moveTeam3(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[2]);
            }
        }

        private void button_moveTeam4(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[3]);
            }
        }

        private void button_moveTeam5(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[4]);
            }
        }

        private void button_moveTeam6(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[5]);
            }
        }

        private void button_moveTeam7(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[6]);
            }
        }

        private void button_moveTeam8(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                PlayerItem[] playerItems = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    playerItems[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                movePlayerToTeam(playerItems, config.teams[7]);
            }
        }

        private void button_reset(object sender, EventArgs e) {
            resetTeams();
        }

        #endregion

        #region Selections

        private void select_teamList(object sender, EventArgs e) {
            if (teamListView.SelectedItems.Count > 0) {
                kickMenuItem.Enabled = true;
                moveMenuItem.Enabled = true;
            }
            else {
                kickMenuItem.Enabled = false;
                moveMenuItem.Enabled = false;
            }
        }

        private void select_playerList(object sender, EventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                removeButton.Enabled = true;
                removeMenuItem.Enabled = true;
                propertiesButton.Enabled = true;
                propertiesMenuItem.Enabled = true;
            }
            else {
                removeButton.Enabled = false;
                removeMenuItem.Enabled = false;
                propertiesButton.Enabled = false;
                propertiesMenuItem.Enabled = false;
            }
        }

        #endregion

        #region Context-menues

        private void context_open(object sender, CancelEventArgs e) {
            if (playerListView.SelectedItems.Count > 0) {
                // Calculate active playerCount
                int activePlayers = 0;
                for (int i = 0; i < config.teams.Length; i++) {
                    activePlayers += config.teams[i].Players.Count;
                }

                chooseMenuItem.Enabled = activePlayers < SimulatorConfiguration.PLAYERLIMIT;
            }
            else {
                chooseMenuItem.Enabled = false;
            }
        }

        #endregion

        #region DragDrop

        private void dragOver_playerList(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent("FileDrop")) {
                e.Effect = DragDropEffects.Copy;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void dragOver_teamList(object sender, DragEventArgs e) {
            if (
                e.Data.GetDataPresent("FileDrop") ||
                e.Data.GetDataPresent("AntMe.Simulation.PlayerInfoFilename[]") ||
                e.Data.GetDataPresent("AntMe.Plugin.Simulation.PlayerItem[]")) {
                // Move Item, if no Control-Key is pressed
                if (e.Data.GetDataPresent("AntMe.Plugin.Simulation.PlayerItem[]") &&
                    (e.KeyState & 8) == 0) {
                    e.Effect = DragDropEffects.Move;
                }
                else {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void dragDrop_playerList(object sender, DragEventArgs e) {
            string[] files = (string[]) e.Data.GetData("FileDrop");
            foreach (string file in files) {
                loadPlayerFile(file, false, false);
            }
        }

        private void dragDrop_teamList(object sender, DragEventArgs e) {
            // Drop some files
            if (e.Data.GetDataPresent("FileDrop")) {
                string[] files = (string[]) e.Data.GetData("FileDrop");
                foreach (string file in files) {
                    List<PlayerInfoFilename> hits = loadPlayerFile(file, false, true);
                    foreach (PlayerInfoFilename hit in hits) {
                        addPlayerToTeam(hit, null);
                    }
                }
            }

            // Drop new playerInfos
            if (e.Data.GetDataPresent("AntMe.Simulation.PlayerInfoFilename[]")) {
                PlayerInfoFilename[] player =
                    (PlayerInfoFilename[]) e.Data.GetData("AntMe.Simulation.PlayerInfoFilename[]");

                Point dropPoint = new Point(e.X, e.Y);
                ListViewItem hit = teamListView.HitTest(teamListView.PointToClient(dropPoint)).Item;
                if (hit != null) {
                    ListViewGroup group = hit.Group;
                    int team = teamListView.Groups.IndexOf(group);
                    for (int i = 0; i < player.Length; i++) {
                        addPlayerToTeam(player[i], config.teams[team]);
                    }
                }
                else {
                    addPlayerToTeam(player, false);
                }
            }

            // Move PlayerItems
            if (e.Data.GetDataPresent("AntMe.Plugin.Simulation.PlayerItem[]")) {
                PlayerItem[] player = (PlayerItem[]) e.Data.GetData("AntMe.Plugin.Simulation.PlayerItem[]");

                Point dropPoint = new Point(e.X, e.Y);
                ListViewItem hit = teamListView.HitTest(teamListView.PointToClient(dropPoint)).Item;
                if (hit != null) {
                    ListViewGroup group = hit.Group;
                    int team = teamListView.Groups.IndexOf(group);
                    if ((e.KeyState & 8) > 0) {
                        PlayerItem[] newPlayer = new PlayerItem[player.Length];
                        for (int i = 0; i < player.Length; i++) {
                            newPlayer[i] = (PlayerItem) player[i].Clone();
                        }
                        addPlayerToTeam(newPlayer, config.teams[team]);
                    }
                    else {
                        movePlayerToTeam(player, config.teams[team]);
                    }
                }
                else {
                    if ((e.KeyState & 8) > 0) {
                        PlayerItem[] newPlayer = new PlayerItem[player.Length];
                        for (int i = 0; i < player.Length; i++) {
                            newPlayer[i] = (PlayerItem) player[i].Clone();
                        }
                        addPlayerToTeam(newPlayer, null);
                    }
                    else {
                        movePlayerToTeam(player, null);
                    }
                }
            }
        }

        private void drag_playerList(object sender, ItemDragEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                PlayerInfoFilename[] player = new PlayerInfoFilename[playerListView.SelectedItems.Count];
                for (int i = 0; i < playerListView.SelectedItems.Count; i++) {
                    player[i] = (PlayerInfoFilename) playerListView.SelectedItems[i].Tag;
                }
                DoDragDrop(player, DragDropEffects.Copy);
            }
        }

        private void drag_teamList(object sender, ItemDragEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                PlayerItem[] player = new PlayerItem[teamListView.SelectedItems.Count];
                for (int i = 0; i < teamListView.SelectedItems.Count; i++) {
                    player[i] = (PlayerItem) teamListView.SelectedItems[i].Tag;
                }
                DoDragDrop(player, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        #endregion

        #region Timer

        private void timer_Tick(object sender, EventArgs e) {
            Enabled = !active;
        }

        #endregion

        #endregion
    }
}