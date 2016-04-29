using AntMe.Online.Client;
using AntMe.SharedComponents.States;
using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    public partial class SinglePlayerSummaryForm : Form
    {
        private Guid listingId;
        private PlayerInfoFiledump playerInfo;
        private int seed;

        public SinglePlayerSummaryForm()
        {
            InitializeComponent();
        }

        public void Init(PlayerInfo player, int seed,
            int points, int collectedFood, int collectedFruit, int eatenAnts,
            int beatenAnts, int starvedAnts, int killedAnts, int killedBugs)
        {
            playerInfo = new PlayerInfoFiledump(player, File.ReadAllBytes((player as PlayerInfoFilename).File));
            this.seed = seed;

            pointsLabel.Text = points.ToString();
            eatenAntsLabel.Text = eatenAnts.ToString();
            beatenAntsLabel.Text = beatenAnts.ToString();
            starvedAntsLabel.Text = starvedAnts.ToString();
            killedAntsLabel.Text = killedAnts.ToString();
            killedBugsLabel.Text = killedBugs.ToString();
            collectedFoodLabel.Text = collectedFood.ToString();
            collectedFruitLabel.Text = collectedFruit.ToString();

            seedLabel.Text = seed.ToString();

            nameLabel.Text = player.ColonyName;
            authorLabel.Text = string.Format("{0} {1}", player.FirstName, player.LastName);
            staticLabel.Text = player.Static ? "true" : "false";
            listingId = player.Static ?
                Connection.Instance.Highscores1.DefaultStaticListing :
                Connection.Instance.Highscores1.DefaultNonstaticListing;
            listingIdLabel.Text = listingId.ToString();

            Task t = new Task(() =>
            {
                Listing1 listing = null;
                try
                {
                    listing = Connection.Instance.Highscores1.GetListing(listingId);
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)(() => { errorLabel.Text = ex.Message; }));
                }

                this.Invoke((MethodInvoker)(() =>
                {
                    listingNameLabel.Text = listing != null ? listing.Name : "Fehler";
                }));

                Row1Values values = null;
                try
                {
                    values = Connection.Instance.Highscores1.GetRowValues(listingId);
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)(() => { errorLabel.Text = ex.Message; }));
                }

                bool needsSubmit = values == null ||
                    values.MinPoints > points || values.MaxPoints < points;


                this.Invoke((MethodInvoker)(() =>
                {
                    if (values != null)
                    {
                        // TODO: Werte zuweisen
                        minPointsLabel.Text = values.MinPoints.ToString();
                        minEatenAntsLabel.Text = values.MinEatenAnts.ToString();
                        minBeatenAntsLabel.Text = values.MinBeatenAnts.ToString();
                        minStarvedAntsLabel.Text = values.MinStarvedAnts.ToString();
                        minKilledAntsLabel.Text = values.MinKilledEnemies.ToString();
                        minKilledBugsLabel.Text = values.MinKilledBugs.ToString();
                        minCollectedFoodLabel.Text = values.MinCollectedFood.ToString();
                        minCollectedFruitLabel.Text = values.MinCollectedFruits.ToString();

                        maxPointsLabel.Text = values.MaxPoints.ToString();
                        maxEatenAntsLabel.Text = values.MaxEatenAnts.ToString();
                        maxBeatenAntsLabel.Text = values.MaxBeatenAnts.ToString();
                        maxStarvedAntsLabel.Text = values.MaxStarvedAnts.ToString();
                        maxKilledAntsLabel.Text = values.MaxKilledEnemies.ToString();
                        maxKilledBugsLabel.Text = values.MaxKilledBugs.ToString();
                        maxCollectedFoodLabel.Text = values.MaxCollectedFood.ToString();
                        maxCollectedFruitLabel.Text = values.MaxCollectedFruits.ToString();
                    }
                    else
                    {
                        minPointsLabel.Text = string.Empty;
                        minEatenAntsLabel.Text = string.Empty;
                        minBeatenAntsLabel.Text = string.Empty;
                        minStarvedAntsLabel.Text = string.Empty;
                        minKilledAntsLabel.Text = string.Empty;
                        minKilledBugsLabel.Text = string.Empty;
                        minCollectedFoodLabel.Text = string.Empty;
                        minCollectedFruitLabel.Text = string.Empty;

                        maxPointsLabel.Text = string.Empty;
                        maxEatenAntsLabel.Text = string.Empty;
                        maxBeatenAntsLabel.Text = string.Empty;
                        maxStarvedAntsLabel.Text = string.Empty;
                        maxKilledAntsLabel.Text = string.Empty;
                        maxKilledBugsLabel.Text = string.Empty;
                        maxCollectedFoodLabel.Text = string.Empty;
                        maxCollectedFruitLabel.Text = string.Empty;
                    }

                    submitButton.Enabled = needsSubmit;
                }));
            });
            t.Start();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                Connection.Instance.Highscores1.SubmitRow(listingId, playerInfo.File, playerInfo.ClassName, seed);
                this.Invoke((MethodInvoker)(() => { errorLabel.Text = "Erfolgreich abgesendet"; }));
                Close();
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)(() => { errorLabel.Text = ex.Message; }));
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
