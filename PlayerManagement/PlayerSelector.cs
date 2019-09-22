using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntMe.PlayerManagement
{
    /// <summary>
    /// Form to select one of the known Player Files.
    /// </summary>
    public sealed partial class PlayerSelector : Form
    {
        private List<PlayerInfoFilename> players = new List<PlayerInfoFilename>();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PlayerSelector()
        {
            InitializeComponent();

            UpdateList();
        }

        /// <summary>
        /// The selected Player.
        /// </summary>
        public PlayerInfoFilename SelectedPlayer { get; private set; }

        private void UpdateList()
        {
            players.Clear();
            foreach (var player in PlayerStore.Instance.KnownPlayer)
                players.Add(player as PlayerInfoFilename);

            // Delete items
            for (int i = 0; i < playerListView.Items.Count; i++)
            {
                if (!players.Contains((PlayerInfoFilename)playerListView.Items[i].Tag))
                {
                    playerListView.Items.RemoveAt(i);
                    i--;
                }
            }

            // Create new items
            ListViewGroup staticGroup = playerListView.Groups["staticGroup"];
            ListViewGroup nonstaticGroup = playerListView.Groups["nonStaticGroup"];
            for (int i = 0; i < players.Count; i++)
            {
                PlayerInfoFilename info = players[i];
                if (!playerListView.Items.ContainsKey(info.GetHashCode().ToString()))
                {
                    ListViewItem item = playerListView.Items.Add(info.GetHashCode().ToString(), info.ColonyName, 0);
                    item.Tag = info;
                    item.Group = info.Static ? staticGroup : nonstaticGroup;
                    item.SubItems.Add(
                        string.Format(Resource.AntPropertiesAuthorFormat, info.FirstName, info.LastName));
                }
            }

            // Update Icon
            foreach (ListViewItem listViewItem in playerListView.Items)
            {

                // collect infos
                PlayerInfoFilename playerInfo = (PlayerInfoFilename)listViewItem.Tag;

                bool playerStatic = playerInfo.Static;
                bool playerEnabled = true;
                bool playerSecure = RightsRequest.RequestRights(playerInfo);
                string hintText = string.Empty;

                if (!RightsRequest.IsValidPlayer(playerInfo))
                {
                    playerEnabled = false;
                    hintText = RightsRequest.GetRuleViolationMessage(playerInfo);
                }
                //else if (RightsRequest.LockedRights(config, playerInfo))
                //{
                //    playerEnabled = false;
                //    hintText = RightsRequest.RequiredRightsList(config, playerInfo);
                //}

                // Set Information to Item
                listViewItem.ImageKey =
                    (playerStatic ? "static" : "nonstatic") +
                    (!playerEnabled ? "_disabled" : string.Empty) +
                    (playerSecure ? "_secure" : string.Empty);
                listViewItem.ToolTipText = hintText;
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            using (CreateForm form = new CreateForm())
            {
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (MessageBox.Show(this, Resource.PlayerSelectorOpenIde, Resource.PlayerSelectorOpenIdeTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        // Start Visual Studio
                        Process.Start(form.GeneratedSolutionFile);
                    }
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                PlayerStore.Instance.RegisterFile(openFileDialog.FileName);
                UpdateList();
            }
        }

        private void playerListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (playerListView.SelectedItems.Count > 0)
                SelectedPlayer = playerListView.SelectedItems[0].Tag as PlayerInfoFilename;
            else
                SelectedPlayer = null;

            UpdateSelection();
        }

        private void playerListView_DoubleClick(object sender, EventArgs e)
        {
            if (playerListView.SelectedItems.Count > 0)
                SelectedPlayer = playerListView.SelectedItems[0].Tag as PlayerInfoFilename;
            else
                SelectedPlayer = null;

            UpdateSelection();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void UpdateSelection()
        {
            OkButton.Enabled = SelectedPlayer != null;
            propertiesButton.Enabled = SelectedPlayer != null;
            propertiesMenuItem.Enabled = SelectedPlayer != null;
        }

        private void propertiesButton_Click(object sender, EventArgs e)
        {
            using (AntProperties properties = new AntProperties(SelectedPlayer))
            {
                properties.ShowDialog(this);
            }
        }
    }
}
