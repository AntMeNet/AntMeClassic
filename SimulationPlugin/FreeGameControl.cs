using AntMe.PlayerManagement;
using AntMe.Simulation;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    public partial class FreeGameControl : UserControl
    {
        private FreeGameSetup setup;

        public FreeGameControl()
        {
            InitializeComponent();
            SetSetup(new FreeGameSetup());
        }

        public void SetSetup(FreeGameSetup setup)
        {
            this.setup = setup;
            UpdateControls();
        }

        private void FreeGameControl_Resize(object sender, EventArgs e)
        {
            mainPanel.Left = (ClientRectangle.Width - mainPanel.Width) / 4;
            mainPanel.Top = (ClientRectangle.Height - mainPanel.Height) / 4;
        }

        #region Opens

        private void open1_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot1.PlayerInfo = player;
                setup.Slot1.Filename = player.File;
                setup.Slot1.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open2_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot2.PlayerInfo = player;
                setup.Slot2.Filename = player.File;
                setup.Slot2.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open3_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot3.PlayerInfo = player;
                setup.Slot3.Filename = player.File;
                setup.Slot3.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open4_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot4.PlayerInfo = player;
                setup.Slot4.Filename = player.File;
                setup.Slot4.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open5_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot5.PlayerInfo = player;
                setup.Slot5.Filename = player.File;
                setup.Slot5.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open6_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot6.PlayerInfo = player;
                setup.Slot6.Filename = player.File;
                setup.Slot6.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open7_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot7.PlayerInfo = player;
                setup.Slot7.Filename = player.File;
                setup.Slot7.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private void open8_Click(object sender, EventArgs e)
        {
            var player = open();
            if (player != null)
            {
                setup.Slot8.PlayerInfo = player;
                setup.Slot8.Filename = player.File;
                setup.Slot8.Typename = player.ClassName;
            }
            UpdateControls();
        }

        private PlayerInfoFilename open()
        {
            using (PlayerSelector selector = new PlayerSelector())
            {
                if (selector.ShowDialog(this) == DialogResult.OK)
                {
                    return selector.SelectedPlayer;
                }
            }

            return null;
        }

        #endregion

        #region Teams

        private void team1Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot1.Team = team1Combo.SelectedIndex + 1;
        }

        private void team2Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot2.Team = team2Combo.SelectedIndex + 1;
        }

        private void team3Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot3.Team = team3Combo.SelectedIndex + 1;
        }

        private void team4Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot4.Team = team4Combo.SelectedIndex + 1;
        }

        private void team5Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot5.Team = team5Combo.SelectedIndex + 1;
        }

        private void team6Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot6.Team = team6Combo.SelectedIndex + 1;
        }

        private void team7Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot7.Team = team7Combo.SelectedIndex + 1;
        }

        private void team8Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            setup.Slot8.Team = team8Combo.SelectedIndex + 1;
        }

        #endregion

        #region Info

        private void info1_Click(object sender, EventArgs e)
        {
            info(setup.Slot1.PlayerInfo);
        }

        private void info2_Click(object sender, EventArgs e)
        {
            info(setup.Slot2.PlayerInfo);
        }

        private void info3_Click(object sender, EventArgs e)
        {
            info(setup.Slot3.PlayerInfo);
        }

        private void info4_Click(object sender, EventArgs e)
        {
            info(setup.Slot4.PlayerInfo);
        }

        private void info5_Click(object sender, EventArgs e)
        {
            info(setup.Slot5.PlayerInfo);
        }

        private void info6_Click(object sender, EventArgs e)
        {
            info(setup.Slot6.PlayerInfo);
        }

        private void info7_Click(object sender, EventArgs e)
        {
            info(setup.Slot7.PlayerInfo);
        }

        private void info8_Click(object sender, EventArgs e)
        {
            info(setup.Slot8.PlayerInfo);
        }

        private void info(PlayerInfoFilename info)
        {
            if (info == null) return;
            using (AntProperties properties = new AntProperties(info))
            {
                properties.ShowDialog(this);
            }
        }

        #endregion

        #region Drop

        private void drop1_Click(object sender, EventArgs e)
        {
            setup.Slot1.PlayerInfo = null;
            setup.Slot1.Filename = string.Empty;
            setup.Slot1.Typename = string.Empty;
            setup.Slot1.Team = 1;
            UpdateControls();
        }
        private void drop2_Click(object sender, EventArgs e)
        {
            setup.Slot2.PlayerInfo = null;
            setup.Slot2.Filename = string.Empty;
            setup.Slot2.Typename = string.Empty;
            setup.Slot2.Team = 2;
            UpdateControls();
        }
        private void drop3_Click(object sender, EventArgs e)
        {
            setup.Slot3.PlayerInfo = null;
            setup.Slot3.Filename = string.Empty;
            setup.Slot3.Typename = string.Empty;
            setup.Slot3.Team = 3;
            UpdateControls();
        }
        private void drop4_Click(object sender, EventArgs e)
        {
            setup.Slot4.PlayerInfo = null;
            setup.Slot4.Filename = string.Empty;
            setup.Slot4.Typename = string.Empty;
            setup.Slot4.Team = 4;
            UpdateControls();
        }
        private void drop5_Click(object sender, EventArgs e)
        {
            setup.Slot5.PlayerInfo = null;
            setup.Slot5.Filename = string.Empty;
            setup.Slot5.Typename = string.Empty;
            setup.Slot5.Team = 5;
            UpdateControls();
        }
        private void drop6_Click(object sender, EventArgs e)
        {
            setup.Slot6.PlayerInfo = null;
            setup.Slot6.Filename = string.Empty;
            setup.Slot6.Typename = string.Empty;
            setup.Slot6.Team = 6;
            UpdateControls();
        }
        private void drop7_Click(object sender, EventArgs e)
        {
            setup.Slot7.PlayerInfo = null;
            setup.Slot7.Filename = string.Empty;
            setup.Slot7.Typename = string.Empty;
            setup.Slot7.Team = 7;
            UpdateControls();
        }

        private void drop8_Click(object sender, EventArgs e)
        {
            setup.Slot8.PlayerInfo = null;
            setup.Slot8.Filename = string.Empty;
            setup.Slot8.Typename = string.Empty;
            setup.Slot8.Team = 8;
            UpdateControls();
        }

        #endregion

        private void UpdateControls()
        {
            SuspendLayout();

            team1Combo.SelectedIndex = setup.Slot1.Team - 1;
            team2Combo.SelectedIndex = setup.Slot2.Team - 1;
            team3Combo.SelectedIndex = setup.Slot3.Team - 1;
            team4Combo.SelectedIndex = setup.Slot4.Team - 1;
            team5Combo.SelectedIndex = setup.Slot5.Team - 1;
            team6Combo.SelectedIndex = setup.Slot6.Team - 1;
            team7Combo.SelectedIndex = setup.Slot7.Team - 1;
            team8Combo.SelectedIndex = setup.Slot8.Team - 1;

            ResumeLayout(false);

            settingsLabel.Text = setup.SimulatorConfiguration.Settings.SettingsName;

            string unselectedName = string.Empty;
            string unselectedAuthor = "Empty Slot";
            name1label.Text = setup.Slot1.PlayerInfo != null ? setup.Slot1.PlayerInfo.ColonyName : unselectedName;
            name2label.Text = setup.Slot2.PlayerInfo != null ? setup.Slot2.PlayerInfo.ColonyName : unselectedName;
            name3label.Text = setup.Slot3.PlayerInfo != null ? setup.Slot3.PlayerInfo.ColonyName : unselectedName;
            name4label.Text = setup.Slot4.PlayerInfo != null ? setup.Slot4.PlayerInfo.ColonyName : unselectedName;
            name5label.Text = setup.Slot5.PlayerInfo != null ? setup.Slot5.PlayerInfo.ColonyName : unselectedName;
            name6label.Text = setup.Slot6.PlayerInfo != null ? setup.Slot6.PlayerInfo.ColonyName : unselectedName;
            name7label.Text = setup.Slot7.PlayerInfo != null ? setup.Slot7.PlayerInfo.ColonyName : unselectedName;
            name8label.Text = setup.Slot8.PlayerInfo != null ? setup.Slot8.PlayerInfo.ColonyName : unselectedName;

            author1label.Text = setup.Slot1.PlayerInfo != null ? setup.Slot1.PlayerInfo.FirstName + " " + setup.Slot1.PlayerInfo.LastName : unselectedAuthor;
            author2label.Text = setup.Slot2.PlayerInfo != null ? setup.Slot2.PlayerInfo.FirstName + " " + setup.Slot2.PlayerInfo.LastName : unselectedAuthor;
            author3label.Text = setup.Slot3.PlayerInfo != null ? setup.Slot3.PlayerInfo.FirstName + " " + setup.Slot3.PlayerInfo.LastName : unselectedAuthor;
            author4label.Text = setup.Slot4.PlayerInfo != null ? setup.Slot4.PlayerInfo.FirstName + " " + setup.Slot4.PlayerInfo.LastName : unselectedAuthor;
            author5label.Text = setup.Slot5.PlayerInfo != null ? setup.Slot5.PlayerInfo.FirstName + " " + setup.Slot5.PlayerInfo.LastName : unselectedAuthor;
            author6label.Text = setup.Slot6.PlayerInfo != null ? setup.Slot6.PlayerInfo.FirstName + " " + setup.Slot6.PlayerInfo.LastName : unselectedAuthor;
            author7label.Text = setup.Slot7.PlayerInfo != null ? setup.Slot7.PlayerInfo.FirstName + " " + setup.Slot7.PlayerInfo.LastName : unselectedAuthor;
            author8label.Text = setup.Slot8.PlayerInfo != null ? setup.Slot8.PlayerInfo.FirstName + " " + setup.Slot8.PlayerInfo.LastName : unselectedAuthor;

            delete1Button.Enabled = setup.Slot1.PlayerInfo != null;
            delete2Button.Enabled = setup.Slot2.PlayerInfo != null;
            delete3Button.Enabled = setup.Slot3.PlayerInfo != null;
            delete4Button.Enabled = setup.Slot4.PlayerInfo != null;
            delete5Button.Enabled = setup.Slot5.PlayerInfo != null;
            delete6Button.Enabled = setup.Slot6.PlayerInfo != null;
            delete7Button.Enabled = setup.Slot7.PlayerInfo != null;
            delete8Button.Enabled = setup.Slot8.PlayerInfo != null;

            team1Combo.Enabled = setup.Slot1.PlayerInfo != null;
            team2Combo.Enabled = setup.Slot2.PlayerInfo != null;
            team3Combo.Enabled = setup.Slot3.PlayerInfo != null;
            team4Combo.Enabled = setup.Slot4.PlayerInfo != null;
            team5Combo.Enabled = setup.Slot5.PlayerInfo != null;
            team6Combo.Enabled = setup.Slot6.PlayerInfo != null;
            team7Combo.Enabled = setup.Slot7.PlayerInfo != null;
            team8Combo.Enabled = setup.Slot8.PlayerInfo != null;

            info1button.Enabled = setup.Slot1.PlayerInfo != null;
            info2button.Enabled = setup.Slot2.PlayerInfo != null;
            info3button.Enabled = setup.Slot3.PlayerInfo != null;
            info4button.Enabled = setup.Slot4.PlayerInfo != null;
            info5button.Enabled = setup.Slot5.PlayerInfo != null;
            info6button.Enabled = setup.Slot6.PlayerInfo != null;
            info7button.Enabled = setup.Slot7.PlayerInfo != null;
            info8button.Enabled = setup.Slot8.PlayerInfo != null;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            setup.Reset();
            UpdateControls();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (SimulationProperties form = new SimulationProperties(setup))
            {
                form.ShowDialog(this);
                UpdateControls();
            }
        }

        internal void DirectStart(string filename)
        {
            PlayerStore.Instance.RegisterFile(filename);

            var players = PlayerStore.Instance.KnownPlayer.Where(p => p.File.ToLower().Equals(filename.ToLower()));
            if (players.Count() > 1)
            {
                throw new Exception("Mehr als einen Spieler gefunden");
            }
            else if (players.Count() == 0)
            {
                throw new Exception("Leider kein Spieler gefunden");
            }
            else
            {
                var player = players.First();
                setup.Slot1.PlayerInfo = player;
                setup.Slot1.Filename = player.File;
                setup.Slot1.Typename = player.ClassName;
                UpdateControls();
            }
        }
    }
}
