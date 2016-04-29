using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AntMe.SharedComponents.States;

namespace AntMe.Plugin.Simulation {
    internal sealed partial class StatisticControl : UserControl {
        private readonly SummaryRoot root = new SummaryRoot();

        private SummarySimulation currentSim;
        private SummaryLoop currentLoop;

        private TreeNode currentSimNode;
        private TreeNode currentLoopNode;
        private int loopCount = 0;

        public StatisticControl() {
            InitializeComponent();
        }

        public void Start() {
            currentSim = new SummarySimulation();
            root.simulations.Add(currentSim);
            loopCount = 0;

            // Create Node
            currentSimNode = loopsTreeView.Nodes.Add(currentSim.startDate.ToString());
            currentSimNode.Tag = currentSim;
            currentSimNode.ImageKey = "simset_running";

            timer.Enabled = true;
        }

        public void Stop() {
            currentSimNode.ImageKey = "simset_complete";
            currentSim = null;

            timer.Enabled = false;
        }

        public void SimulationState(SimulationState state) {
            if (state.CurrentRound == 1) {
                currentLoop = new SummaryLoop();
                currentLoop.rounds = state.TotalRounds;
                currentSim.loops.Add(currentLoop);
                
                // Create Node
                currentLoopNode = currentSimNode.Nodes.Add("Loop " + (++loopCount));
                currentLoopNode.Tag = currentLoop;
                currentLoopNode.ImageKey = "loop_running";
                currentLoopNode.EnsureVisible();
                loopsTreeView.SelectedNode = currentLoopNode;
            }

            foreach (TeamState teamState in state.TeamStates)
            {
                SummaryTeam team;

                if (!currentLoop.teams.ContainsKey(teamState.Guid))
                {
                    team = new SummaryTeam();
                    team.name = teamState.Name;
                    team.guid = teamState.Guid;
                    currentLoop.teams.Add(team.guid, team);
                }
                else
                {
                    team = currentLoop.teams[teamState.Guid];
                }

                foreach (ColonyState colonyState in teamState.ColonyStates)
                {
                    SummaryPlayer player;

                    if (!team.players.ContainsKey(colonyState.Guid))
                    {
                        player = new SummaryPlayer();
                        player.name = colonyState.ColonyName;
                        player.guid = colonyState.Guid;
                        team.players.Add(player.guid, player);
                    }
                    else
                    {
                        player = team.players[colonyState.Guid];
                    }

                    SummaryValueSet valueSet = new SummaryValueSet();
                    valueSet.collectedFood = colonyState.CollectedFood;
                    valueSet.collectedFruit = colonyState.CollectedFruits;
                    valueSet.killedAnts = colonyState.KilledEnemies;
                    valueSet.killedBugs = colonyState.KilledBugs;
                    valueSet.starvedAnts = colonyState.StarvedAnts;
                    valueSet.beatenAnts = colonyState.BeatenAnts;
                    valueSet.eatenAnts = colonyState.EatenAnts;
                    valueSet.totalPoints = colonyState.Points;
                    player.values.Add(valueSet);
                }
            }

            if (state.CurrentRound == state.TotalRounds) {
                currentLoopNode.ImageKey = "loop_complete";
                currentLoop.completed = true;
                currentLoop = null;
            }
        }

        private void tree_select(object sender, TreeViewEventArgs e)
        {
            if (loopsTreeView.SelectedNode != null) {

                summaryListView.Items.Clear();

                // In case of a selected loop
                if (loopsTreeView.SelectedNode.Tag is SummaryLoop) {
                    SummaryLoop loop = (SummaryLoop) loopsTreeView.SelectedNode.Tag;

                    int teamcount = 0;
                    foreach (SummaryTeam team in loop.teams.Values) {
                        teamcount++;

                        ListViewGroup teamGroup = summaryListView.Groups["team" + teamcount + "Group"];

                        foreach (SummaryPlayer player in team.players.Values) {
                            SummaryValueSet set = player.values[player.values.Count - 1];

                            ListViewItem item = summaryListView.Items.Add(player.name, "colony");
                            item.SubItems.Add(set.collectedFood.ToString());
                            item.SubItems.Add(set.collectedFruit.ToString());
                            item.SubItems.Add(set.killedAnts.ToString());
                            item.SubItems.Add(set.killedBugs.ToString());
                            item.SubItems.Add(set.starvedAnts.ToString());
                            item.SubItems.Add(set.beatenAnts.ToString());
                            item.SubItems.Add(set.eatenAnts.ToString());
                            item.SubItems.Add(set.totalPoints.ToString());
                            item.Group = teamGroup;
                        }
                    }
                }

                // In case of a selected simulation-set
                if (loopsTreeView.SelectedNode.Tag is SummarySimulation) {
                    SummarySimulation simulation = (SummarySimulation) loopsTreeView.SelectedNode.Tag;

                    // Check for any Teaminformation
                    if (simulation.loops.Count > 0) {

                        // Create the summary for included loops
                        int loops = 0;
                        Dictionary<Guid, SummaryTeam> teams = 
                            new Dictionary<Guid, SummaryTeam>(simulation.loops[0].teams.Count);
                        foreach (SummaryTeam team in simulation.loops[0].teams.Values) {

                            // Clone team
                            SummaryTeam summaryTeam = new SummaryTeam();
                            summaryTeam.guid = team.guid;
                            summaryTeam.name = team.name;
                            summaryTeam.players = new Dictionary<Guid, SummaryPlayer>(team.players.Count);
                            teams.Add(team.guid, summaryTeam);

                            foreach (SummaryPlayer player in team.players.Values) {

                                // Clone player
                                SummaryPlayer summaryPlayer = new SummaryPlayer();
                                summaryPlayer.guid = player.guid;
                                summaryPlayer.name = player.name;
                                summaryPlayer.values = new List<SummaryValueSet>(1);
                                summaryPlayer.values.Add(new SummaryValueSet());
                                summaryTeam.players.Add(summaryPlayer.guid, summaryPlayer);
                            }
                        }

                        // Sum up all loops
                        foreach (SummaryLoop loop in simulation.loops) {
                            if (loop.completed) {
                                loops++;

                                foreach (SummaryTeam team in loop.teams.Values) {
                                    foreach (SummaryPlayer player in team.players.Values) {
                                        SummaryValueSet set = player.values[player.values.Count - 1];
                                        teams[team.guid].players[player.guid].values[0].Add(set);
                                    }
                                }
                            }
                        }

                        // Fill into Listview
                        int teamcount = 0;
                        foreach (SummaryTeam team in teams.Values)
                        {
                            teamcount++;

                            ListViewGroup teamGroup = summaryListView.Groups["team" + teamcount + "Group"];

                            foreach (SummaryPlayer player in team.players.Values)
                            {
                                SummaryValueSet set = player.values[0];

                                if (loops > 0) {
                                    // Calculate average values
                                    float collectedFood = (float) set.collectedFood/loops;
                                    float collectedFruit = (float) set.collectedFruit/loops;
                                    float killedAnts = (float) set.killedAnts/loops;
                                    float killedBugs = (float) set.killedBugs/loops;
                                    float starvedAnts = (float) set.starvedAnts/loops;
                                    float beatenAnts = (float) set.beatenAnts/loops;
                                    float eatenAnts = (float) set.eatenAnts/loops;
                                    float totalPoints = (float) set.totalPoints/loops;

                                    // push it to listview
                                    ListViewItem item = summaryListView.Items.Add(player.name, "colony");
                                    item.SubItems.Add(collectedFood.ToString("0.00"));
                                    item.SubItems.Add(collectedFruit.ToString("0.00"));
                                    item.SubItems.Add(killedAnts.ToString("0.00"));
                                    item.SubItems.Add(killedBugs.ToString("0.00"));
                                    item.SubItems.Add(starvedAnts.ToString("0.00"));
                                    item.SubItems.Add(beatenAnts.ToString("0.00"));
                                    item.SubItems.Add(eatenAnts.ToString("0.00"));
                                    item.SubItems.Add(totalPoints.ToString("0.00"));
                                    item.Group = teamGroup;
                                }
                                else {
                                    // push empty row
                                    ListViewItem item = summaryListView.Items.Add(player.name, "colony");
                                    item.Group = teamGroup;
                                }
                            }
                        }

                    }
                }

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tree_select(null, null);
        }
    }
}