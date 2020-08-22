using AntMe.Online.Client;
using AntMe.PlayerManagement;
using AntMe.SharedComponents.Controls;
using AntMe.Simulation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    public partial class SinglePlayerControl : UserControl
    {
        private SinglePlayerSetup setup;

        private bool needAchievementUpdate = true;
        private bool needHighscoreUpdate = true;

        public SinglePlayerControl()
        {
            InitializeComponent();
            SetSetup(new SinglePlayerSetup());

            foreach (var value in Enum.GetValues(typeof(ListingOrder)))
                orderComboBox.Items.Add(value);
            orderComboBox.SelectedItem = ListingOrder.MaxPoints;
        }

        public void SetSetup(SinglePlayerSetup setup)
        {
            this.setup = setup;
            listingPlayer = setup.PlayerInfo;
            UpdateControls();
            needHighscoreUpdate = true;
        }

        public void ShowSummary(PlayerInfo player, int seed,
            int points, int collectedFood, int collectedFruit, int eatenAnts,
            int beatenAnts, int starvedAnts, int killedAnts, int killedBugs)
        {
            this.BeginInvoke((MethodInvoker)(() =>
                {
                    using (var form = new SinglePlayerSummaryForm())
                    {
                        form.Init(player, seed, points, collectedFood, collectedFruit,
                            eatenAnts, beatenAnts, starvedAnts, killedAnts, killedBugs);
                        form.ShowDialog(this);
                    }
                }));
        }

        private void UpdateControls()
        {
            string unselectedName = string.Empty;
            string unselectedAuthor = "Empty";
            namelabel.Text = setup.PlayerInfo != null ? setup.PlayerInfo.ColonyName : unselectedName;
            authorlabel.Text = setup.PlayerInfo != null ? setup.PlayerInfo.FirstName + " " + setup.PlayerInfo.LastName : unselectedAuthor;

            removeLabel.Enabled = setup.PlayerInfo != null;
            propertyLabel.Enabled = setup.PlayerInfo != null;
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
                setup.PlayerInfo = player;
                setup.Filename = player.File;
                setup.Typename = player.ClassName;
                UpdateControls();
            }
        }

        private void playerPanel_Click(object sender, EventArgs e)
        {
            PlayerInfoFilename playerInfo = null;
            using (PlayerSelector selector = new PlayerSelector())
            {
                if (selector.ShowDialog(this) == DialogResult.OK)
                {
                    playerInfo = selector.SelectedPlayer;
                }
            }

            if (playerInfo != null)
            {
                setup.PlayerInfo = playerInfo;
                setup.Filename = playerInfo.File;
                setup.Typename = playerInfo.ClassName;
            }
            UpdateControls();

            listingPlayer = playerInfo;
            needHighscoreUpdate = true;
        }

        private void propertyLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (setup.PlayerInfo == null) return;
            using (AntProperties properties = new AntProperties(setup.PlayerInfo))
            {
                properties.ShowDialog(this);
            }
        }

        private void removeLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            setup.PlayerInfo = null;
            setup.Filename = string.Empty;
            setup.Typename = string.Empty;
            UpdateControls();

            listingPlayer = null;

            needHighscoreUpdate = true;
            // UpdateHighscore();
        }

        private PlayerInfo listingPlayer = null;
        private ListingOrder listingOrder = ListingOrder.MaxPoints;
        private int listingPage = 0;

        private void UpdateHighscore()
        {
            if (!this.Visible)
                return;

            leaderboardListView.Items.Clear();
            backButton.Enabled = false;
            nextButton.Enabled = false;
            homeButton.Enabled = false;
            totalPagesLabel.Text = "1";

            leaderboardLoading.ShutterState = ShutterState.Loading;

            if (listingPlayer != null)
                staticLabel.Text = listingPlayer.Static ? "(static)" : "(nonstatic)";
            else
                staticLabel.Text = string.Empty;

            if (listingPlayer != null)
            {
                pageTextBox.Enabled = false;

                Task t = new Task(() =>
                {
                    Guid listingId = listingPlayer.Static ? Connection.Instance.Highscores1.DefaultStaticListing : Connection.Instance.Highscores1.DefaultNonstaticListing;
                    try
                    {
                        Pagination<Row1> rows = Connection.Instance.Highscores1.GetRows(listingId, listingOrder, listingPage, 20);
                        Invoke((MethodInvoker)(() =>
                        {
                            totalPagesLabel.Text = rows.TotalPages.ToString();
                            backButton.Enabled = (rows.CurrentPage + 1) > 1;
                            nextButton.Enabled = (rows.CurrentPage + 1) < rows.TotalPages;
                            pageTextBox.Minimum = 1;
                            pageTextBox.Maximum = Math.Max(1, rows.TotalPages);
                            pageTextBox.Value = rows.CurrentPage + 1;

                            leaderboardLoading.ShutterState = ShutterState.Open;

                            foreach (var row in rows.Result)
                            {
                                var item = leaderboardListView.Items.Add(row.Rank.ToString());
                                item.SubItems.Add(row.Username);
                                item.SubItems.Add(row.Value.ToString());
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke((MethodInvoker)(() =>
                        {
                            leaderboardLoading.ErrorMessage = ex.Message;
                            leaderboardLoading.ShutterState = ShutterState.Error;
                            needHighscoreUpdate = true;
                        }));
                    }
                });
                t.Start();
            }
        }

        private void UpdateAchievements()
        {
            achievementPanel.Controls.Clear();
            achievementsLoading.ShutterState = ShutterState.Loading;

            Task t = new Task(() =>
            {
                try
                {
                    var achievements = Connection.Instance.Achievements.GetAchievements();
                    Invoke((MethodInvoker)(() =>
                    {
                        foreach (var achievement in achievements)
                        {
                            AchievementControl control = new AchievementControl();
                            control.SetAchievement(achievement);
                            achievementPanel.Controls.Add(control);
                        }
                        achievementsLoading.ShutterState = ShutterState.Open;
                    }));
                }
                catch (Exception ex)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        achievementsLoading.ErrorMessage = ex.Message;
                        achievementsLoading.ShutterState = ShutterState.Error;
                        needAchievementUpdate = true;
                    }));
                }
            });
            t.Start();
        }

        private void orderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            listingOrder = (ListingOrder)orderComboBox.SelectedItem;
            listingPage = 0;
            needHighscoreUpdate = true;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            listingPage++;
            needHighscoreUpdate = true;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            listingPage--;
            needHighscoreUpdate = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            needAchievementUpdate = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Connection.Instance.State != Online.Client.ConnectionState.Connected)
            {
                achievementsLoading.InitMessage = "Not connected to the AntMe! Server";
                leaderboardLoading.InitMessage = "Not connected to the AntMe! Server";
                return;
            }

            if (Handle == null)
                return;

            if (needHighscoreUpdate)
            {
                UpdateHighscore();
                needHighscoreUpdate = false;
            }

            if (needAchievementUpdate)
            {
                UpdateAchievements();
                needAchievementUpdate = false;
            }
        }
    }
}
