using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AntMe.Simulation;

namespace AntMe.Plugin.Simulation {
    public partial class RightsRequest : Form {
        private readonly PlayerInfoFilename playerInfo;
        private const string open = "open";
        private const string closed = "closed";
        private readonly bool locked = false;

        public RightsRequest(SimulationPluginConfiguration config, PlayerInfoFilename player) {
            playerInfo = player;
            InitializeComponent();
            yesButton.Enabled = acceptCheckBox.Checked;

            colonyLabel.Text = player.ColonyName;
            authorLabel.Text =
                string.Format(Resource.SimulatorPluginAntPropertiesAuthorFormat, player.FirstName, player.LastName);

            if (player.RequestFileAccess) {
                if (config.configuration.AllowFileAccess) {
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesIoAccess, open);
                }
                else {
                    locked = true;
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesIoAccess, closed);
                }
            }
            if (player.RequestDatabaseAccess) {
                if (config.configuration.AllowDatabaseAccess) {
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesDbAccess, open);
                }
                else {
                    locked = true;
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesDbAccess, closed);
                }
            }
            if (player.RequestReferences) {
                if (config.configuration.AllowReferences) {
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesRefAccess, open);
                }
                else {
                    locked = true;
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesRefAccess, closed);
                }
            }
            if (player.RequestUserInterfaceAccess) {
                if (config.configuration.AllowUserinterfaceAccess) {
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesUiAccess, open);
                }
                else {
                    locked = true;
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesUiAccess, closed);
                }
            }
            if (player.RequestNetworkAccess) {
                if (config.configuration.AllowNetworkAccess) {
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesNetAccess, open);
                }
                else {
                    locked = true;
                    rightsListView.Items.Add(Resource.SimulatorPluginAntPropertiesNetAccess, closed);
                }
            }

            if (locked) {
                sorryPanel.Visible = true;
                acceptPanel.Visible = false;
                AcceptButton = closeButton;
                CancelButton = closeButton;
            }
            else {
                sorryPanel.Visible = false;
                acceptPanel.Visible = true;
                AcceptButton = yesButton;
                CancelButton = noButton;
            }
        }

        /// <summary>
        /// Finds out, if the given player needs a security-request
        /// </summary>
        /// <returns>true, if request is needed</returns>
        public bool NeedRequest() {
            return
                playerInfo.RequestFileAccess ||
                playerInfo.RequestDatabaseAccess ||
                playerInfo.RequestReferences ||
                playerInfo.RequestUserInterfaceAccess ||
                playerInfo.RequestNetworkAccess;
        }

        /// <summary>
        /// Finds out, if the given player is locked through the antme-settings.
        /// </summary>
        /// <returns>true, if player is locked</returns>
        public bool IsLocked() {
            return locked;
        }

        private void acceptCheckBox_CheckedChanged(object sender, EventArgs e) {
            yesButton.Enabled = acceptCheckBox.Checked;
        }

        private void detailsButton_Click(object sender, EventArgs e) {
            AntProperties properties = new AntProperties(playerInfo);
            properties.ShowDialog(this);
        }

        /// <summary>
        /// Checks, if the player is still valid.
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>true, if player is valid.</returns>
        public static bool IsValidPlayer(PlayerInfo player) {
            bool output;
            try {
                player.RuleCheck();
                output = true;
            }
            catch (RuleViolationException) {
                output = false;
            }
            return output;
        }

        /// <summary>
        /// Delivers the RuleViolationException-Message for that player.
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>RuleViolation-Message</returns>
        public static string GetRuleViolationMessage(PlayerInfo player) {
            string message = string.Empty;
            try {
                player.RuleCheck();
            }
            catch (RuleViolationException ex) {
                message = string.Format(Resource.SimulationPluginRuleViolation, ex.Message);
            }
            return message;
        }

        /// <summary>
        /// Checks player for right-requests.
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>true, if player requests rights</returns>
        public static bool RequestRights(PlayerInfo player) {
            return
                player.RequestFileAccess ||
                player.RequestDatabaseAccess ||
                player.RequestReferences ||
                player.RequestUserInterfaceAccess ||
                player.RequestNetworkAccess;
        }

        public static bool LockedRights(SimulationPluginConfiguration config, PlayerInfo player) {
            // Global check. If the player needs no rights there is no way to lock
            if (!RequestRights(player)) {
                return false;
            }

            // Fileaccess
            if (player.RequestFileAccess && !config.configuration.AllowFileAccess) {
                return true;
            }

            // Database
            if (player.RequestDatabaseAccess && !config.configuration.AllowDatabaseAccess) {
                return true;
            }

            // Refs
            if (player.RequestReferences && !config.configuration.AllowReferences) {
                return true;
            }

            // Userinterfaces
            if (player.RequestUserInterfaceAccess && !config.configuration.AllowUserinterfaceAccess) {
                return true;
            }

            // Network
            if (player.RequestNetworkAccess && !config.configuration.AllowNetworkAccess) {
                return true;
            }

            // No locks
            return false;
        }

        public static string RequiredRightsList(SimulationPluginConfiguration config, PlayerInfo player) {
            List<string> securityRequests = new List<string>();

            // Security-Settings
            if (player.RequestFileAccess) {
                if (!config.configuration.AllowFileAccess) {
                    securityRequests.Add("- " + Resource.SimulatorPluginAntPropertiesIoAccess);
                }
            }

            if (player.RequestDatabaseAccess) {
                if (!config.configuration.AllowDatabaseAccess) {
                    securityRequests.Add("- " + Resource.SimulatorPluginAntPropertiesDbAccess);
                }
            }

            if (player.RequestUserInterfaceAccess) {
                if (!config.configuration.AllowUserinterfaceAccess) {
                    securityRequests.Add("- " + Resource.SimulatorPluginAntPropertiesUiAccess);
                }
            }

            if (player.RequestReferences) {
                if (!config.configuration.AllowReferences) {
                    securityRequests.Add("- " + Resource.SimulatorPluginAntPropertiesRefAccess);
                }
            }

            if (player.RequestNetworkAccess) {
                if (!config.configuration.AllowNetworkAccess) {
                    securityRequests.Add("- " + Resource.SimulatorPluginAntPropertiesNetAccess);
                }
            }

            if (securityRequests.Count > 0) {
                return
                    Resource.SimulationPluginRightsLocked + Environment.NewLine +
                    string.Join(Environment.NewLine, securityRequests.ToArray());
            }

            return string.Empty;
        }
    }
}