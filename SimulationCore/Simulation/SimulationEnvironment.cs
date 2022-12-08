using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;

namespace AntMe.Simulation
{
    /// <summary>
    /// Holds all simulation-elements.
    /// </summary>
    internal sealed partial class SimulationEnvironment
    {
        #region Constants

        public const int PLAYGROUND_UNIT = 64;

        #endregion

        #region private variables

        private int currentRound;
        private int playerCount;
        private Random random;

        #endregion

        #region Constructor and Init

        /// <summary>
        /// Creates a new instance of SimulationEnvironment
        /// </summary>
        public SimulationEnvironment()
        {
            precalculateAngles();
            PlayerCall.AreaChanged += playerCallAreaChanged;
        }

        /// <summary>
        /// Change responsibility player call area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerCallAreaChanged(object sender, AreaChangeEventArgs e)
        {
            AreaChange(this, e);
        }

        /// <summary>
        /// simulation initialisation and preparation for round one
        /// </summary>
        /// <param name="configuration">Configuration of simulation</param>
        /// <throws><see cref="ArgumentException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="PathTooLongException"/></throws>
        /// <throws><see cref="DirectoryNotFoundException"/></throws>
        /// <throws><see cref="IOException"/></throws>
        /// <throws><see cref="UnauthorizedAccessException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="NotSupportedException"/></throws>
        /// <throws><see cref="SecurityException"/></throws>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        /// <throws><see cref="TypeLoadException"/></throws>
        public void Init(SimulatorConfiguration configuration)
        {
            // Init some varialbes
            currentRound = 0;
            if (configuration.MapInitialValue != 0)
                random = new Random(configuration.MapInitialValue);
            else
                random = new Random();

            // count players
            playerCount = 0;
            foreach (TeamInfo team in configuration.Teams)
            {
                playerCount += team.Player.Count;
            }

            // Sugar-relevant stuff
            sugarDelay = 0;
            sugarCountDown = (int)(SimulationSettings.Custom.SugarTotalCount *
                                    (1 + (SimulationSettings.Custom.SugarTotalCountPlayerMultiplier * playerCount)));
            sugarLimit = (int)(SimulationSettings.Custom.SugarSimultaneousCount *
                                (1 + (SimulationSettings.Custom.SugarCountPlayerMultiplier * playerCount)));

            // Fruit-relevant stuff
            fruitDelay = 0;
            fruitCountDown = (int)(SimulationSettings.Custom.FruitTotalCount *
                                    (1 + (SimulationSettings.Custom.FruitTotalCountPlayerMultiplier * playerCount)));
            fruitLimit = (int)(SimulationSettings.Custom.FruitSimultaneousCount *
                                (1 + (SimulationSettings.Custom.FruitCountPlayerMultiplier * playerCount)));

            // Ant-relevant stuff
            int antCountDown = (int)(SimulationSettings.Custom.AntTotalCount *
                                      (1 + (SimulationSettings.Custom.AntTotalCountPlayerMultiplier * playerCount)));
            antLimit = (int)(SimulationSettings.Custom.AntSimultaneousCount *
                              (1 + (SimulationSettings.Custom.AntCountPlayerMultiplier * playerCount)));

            // create playground
            float area = SimulationSettings.Custom.PlayGroundBaseSize;
            area *= 1 + (playerCount * SimulationSettings.Custom.PlayGroundSizePlayerMultiplier);
            int playgroundWidth = (int)Math.Round(Math.Sqrt(area * 4 / 3));
            int playgroundHeight = (int)Math.Round(Math.Sqrt(area * 3 / 4));
            Playground = new CorePlayground(playgroundWidth, playgroundHeight, random, playerCount);

            // Bugs-relevant stuff
            Bugs = new CoreColony(Playground);
            Bugs.insectCountDown = (int)(SimulationSettings.Custom.BugTotalCount *
                                          (1 + (SimulationSettings.Custom.BugTotalCountPlayerMultiplier * playerCount)));
            bugLimit = (int)(SimulationSettings.Custom.BugSimultaneousCount *
                              (1 + (SimulationSettings.Custom.BugCountPlayerMultiplier * playerCount)));

            // create teams
            Teams = new CoreTeam[configuration.Teams.Count];
            for (int i = 0; i < configuration.Teams.Count; i++)
            {
                TeamInfo team = configuration.Teams[i];
                Teams[i] = new CoreTeam(i, team.Guid, team.Name);
                Teams[i].Colonies = new CoreColony[team.Player.Count];

                // create colonies
                for (int j = 0; j < team.Player.Count; j++)
                {
                    PlayerInfo player = team.Player[j];
                    CoreColony colony = new CoreColony(Playground, player, Teams[i]);
                    Teams[i].Colonies[j] = colony;

                    colony.AntHills.Add(Playground.NewAnthill(colony.Id));
                    colony.insectCountDown = antCountDown;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// calculations for new step in game
        /// </summary>
        /// <returns>state of simulation after the step</returns>
        /// <throws>RuleViolationException</throws>
        /// <throws>Exception</throws>
        public void Step(SimulationState simulationState)
        {
            currentRound++;

            #region Food

            removeSugar();
            spawnSugar();
            spawnFruit();

            #endregion

            #region Bugs

            Bugs.Grids[0].Clear();
            for (int i = 0; i < Teams.Length; i++)
            {
                for (int j = 0; j < Teams[i].Colonies.Length; j++)
                {
                    Bugs.Grids[0].Add(Teams[i].Colonies[j].InsectsList);
                }
            }

            // let bugs beeing controled by the game.
            //foreach (CoreBug bug in Bugs.Insects) {
            //    bug.NimmBefehleEntgegen = true;
            //}

            // Loop over all the bugs.
            for (int bugIndex = 0; bugIndex < Bugs.InsectsList.Count; bugIndex++)
            {
                CoreBug bug = Bugs.InsectsList[bugIndex] as CoreBug;
                Debug.Assert(bug != null);

                bug.AwaitingCommands = true;

                // Find ants within attack range.
                List<CoreInsect> ants = Bugs.Grids[0].FindAnts(bug);

                // Determine how the damage is distributed among the ants.
                if (ants.Count >= SimulationSettings.Custom.BugAttack)
                {
                    // There are more ants in the game environment than the
                    // bug can distribute damage points. Therefore, ants at
                    // the top of the list will have one energy point deducted.
                    for (int index = 0; index < SimulationSettings.Custom.BugAttack; index++)
                    {
                        ants[index].CurrentEnergyCoreInsect--;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt)ants[index], bug);
                        if (ants[index].CurrentEnergyCoreInsect <= 0)
                        {
                            ants[index].Colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }
                else if (ants.Count > 0)
                {
                    // Determine the energy that is subtracted from each ant.
                    // Rounding errors that weaken the bug can occur but we ignore them.
                    int damage = SimulationSettings.Custom.BugAttack / ants.Count;
                    for (int index = 0; index < ants.Count; index++)
                    {
                        ants[index].CurrentEnergyCoreInsect -= damage;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt)ants[index], bug);
                        if (ants[index].CurrentEnergyCoreInsect <= 0)
                        {
                            ants[index].Colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }

                // The bug cannot move during a fight.
                if (ants.Count > 0)
                {
                    continue;
                }

                // Move the bug.
                bug.Move();
                if (bug.DistanceToDestinationCoreInsect == 0)
                {
                    bug.TurnToDirectionCoreInsect(random.Next(360));
                    bug.GoForwardCoreInsect(random.Next(160, 320));
                }
                bug.AwaitingCommands = false;
            }

            // Prevent a player from giving orders to a sighted bug.
            //for(int i = 0; i < Bugs.Insects.Count; i++) {
            //  CoreBug wanze = Bugs.Insects[i] as CoreBug;
            //  if(wanze != null) {
            //    wanze.NimmBefehleEntgegen = false;
            //  }
            //}

            #endregion

            #region Ants

            // Loop through all teams.
            for (int teamIndex = 0; teamIndex < Teams.Length; teamIndex++)
            {
                // Loop through all colonies in that team.
                for (int colonyIndex = 0; colonyIndex < Teams[teamIndex].Colonies.Length; colonyIndex++)
                {
                    CoreColony colony = Teams[teamIndex].Colonies[colonyIndex];

                    // Empty all buckets.
                    for (int casteIndex = 0; casteIndex < colony.CasteCount; casteIndex++)
                    {
                        colony.Grids[casteIndex].Clear();
                    }

                    // Fill all buckets, but do not fill any bucket twice.
                    for (int casteIndex = 0; casteIndex < colony.CasteCount; casteIndex++)
                    {
                        if (colony.Grids[casteIndex].Count == 0)
                        {
                            colony.Grids[casteIndex].Add(Bugs.InsectsList);
                            for (int j = 0; j < Teams.Length; j++)
                            {
                                for (int i = 0; i < Teams[j].Colonies.Length; i++)
                                {
                                    CoreColony v = Teams[j].Colonies[i];
                                    colony.Grids[casteIndex].Add(v.InsectsList);
                                }
                            }
                        }
                    }

                    // Loop over all ants.
                    for (int antIndex = 0; antIndex < colony.InsectsList.Count; antIndex++)
                    {
                        CoreAnt ant = colony.InsectsList[antIndex] as CoreAnt;
                        Debug.Assert(ant != null);

                        // Find and count the insects in the ant's field of vision.
                        CoreBug bug;
                        CoreAnt enemy;
                        CoreAnt friend;
                        CoreAnt teammember;
                        int bugCount, enemyAntCount, colonyAntCount, casteAntCount, teamAntCount;
                        colony.Grids[ant.CasteIndexCoreAnt].FindAndCountInsects(
                            ant,
                            out bug,
                            out bugCount,
                            out enemy,
                            out enemyAntCount,
                            out friend,
                            out colonyAntCount,
                            out casteAntCount,
                            out teammember,
                            out teamAntCount);
                        ant.BugsInViewRange = bugCount;
                        ant.EnemyAntsInViewRange = enemyAntCount;
                        ant.ColonyAntsInViewRange = colonyAntCount;
                        ant.CasteAntsInViewRange = casteAntCount;
                        ant.TeamAntsInViewRange = teamAntCount;

                        // Move the ant.
                        ant.Move();

                        #region Range

                        // Ant has exceeded its range. Ant dies.
                        if (ant.NumberStepsWalked > colony.RangeI[ant.CasteIndexCoreAnt])
                        {
                            ant.CurrentEnergyCoreInsect = 0;
                            colony.StarvedInsects.Add(ant);
                            continue;
                        }

                        // Ant has covered a third of its range.
                        else if (ant.NumberStepsWalked > colony.RangeI[ant.CasteIndexCoreAnt] / 3)
                        {
                            if (ant.IsTiredCoreAnt == false)
                            {
                                ant.IsTiredCoreAnt = true;
                                PlayerCall.BecomesTired(ant);
                            }
                        }

                        #endregion

                        #region Fight

                        // If the ant does not already have a corresponding target, call the events.
                        if (bug != null && !(ant.DestinationCoreInsect is CoreBug))
                        {
                            PlayerCall.SpotsEnemy(ant, bug);
                        }
                        if (enemy != null && !(ant.DestinationCoreInsect is CoreAnt) ||
                            (ant.DestinationCoreInsect is CoreAnt && ((CoreAnt)ant.DestinationCoreInsect).Colony == colony))
                        {
                            PlayerCall.SpotsEnemy(ant, enemy);
                        }
                        if (friend != null && !(ant.DestinationCoreInsect is CoreAnt) ||
                            (ant.DestinationCoreInsect is CoreAnt && ((CoreAnt)ant.DestinationCoreInsect).Colony != colony))
                        {
                            PlayerCall.SpotsFriend(ant, friend);
                        }
                        if (teammember != null && !(ant.DestinationCoreInsect is CoreAnt) ||
                            (ant.DestinationCoreInsect is CoreAnt && ((CoreAnt)ant.DestinationCoreInsect).Colony != colony))
                        {
                            PlayerCall.SpotsTeamMember(ant, teammember);
                        }

                        // Fight with a bug.
                        if (ant.DestinationCoreInsect is CoreBug)
                        {
                            CoreBug k = (CoreBug)ant.DestinationCoreInsect;
                            if (k.CurrentEnergyCoreInsect > 0)
                            {
                                int distance =
                                    CoreCoordinate.DetermineDistanceI(ant.CoordinateCoreInsect, ant.DestinationCoreInsect.CoordinateCoreInsect);
                                if (distance < SimulationSettings.Custom.BattleRange * PLAYGROUND_UNIT)
                                {
                                    k.CurrentEnergyCoreInsect -= ant.AttackStrengthCoreInsect;
                                    if (k.CurrentEnergyCoreInsect <= 0)
                                    {
                                        Bugs.EatenInsects.Add(k);
                                        colony.Statistic.KilledBugs++;
                                        ant.StopMovementCoreInsect();
                                    }
                                }
                            }
                            else
                            {
                                ant.DestinationCoreInsect = null;
                            }
                        }

                        // Fight with an enemy ant.
                        else if (ant.DestinationCoreInsect is CoreAnt)
                        {
                            CoreAnt a = (CoreAnt)ant.DestinationCoreInsect;
                            if (a.Colony != colony && a.CurrentEnergyCoreInsect > 0)
                            {
                                int distance =
                                    CoreCoordinate.DetermineDistanceI(ant.CoordinateCoreInsect, ant.DestinationCoreInsect.CoordinateCoreInsect);
                                if (distance < SimulationSettings.Custom.BattleRange * PLAYGROUND_UNIT)
                                {
                                    PlayerCall.UnderAttack(a, ant);
                                    a.CurrentEnergyCoreInsect -= ant.AttackStrengthCoreInsect;
                                    if (a.CurrentEnergyCoreInsect <= 0)
                                    {
                                        a.Colony.BeatenInsects.Add(a);
                                        colony.Statistic.KilledAnts++;
                                        ant.StopMovementCoreInsect();
                                    }
                                }
                            }
                            else
                            {
                                ant.DestinationCoreInsect = null;
                            }
                        }

                        #endregion

                        // Check if the ant has reached its destination.
                        if (ant.ArrivedCoreInsect)
                        {
                            antAndTarget(ant);
                        }

                        // Check if the ant sees a pile of sugar or a fruit.
                        antAndSugar(ant);
                        if (ant.CarryingFruitCoreInsect == null)
                        {
                            antAndFruit(ant);
                        }

                        // Check whether the ant notices a mark.
                        antAndMarkers(ant);

                        if (ant.DestinationCoreInsect == null && ant.DistanceToDestinationCoreInsect == 0)
                        {
                            PlayerCall.Waits(ant);
                        }

                        PlayerCall.Tick(ant);
                    }

                    removeAnt(colony);
                    spawnAnt(colony);

                    updateMarkers(colony);
                    removeFruit(colony);
                }
            }

            #endregion

            #region Bugs again

            removeBugs();
            healBugs();
            spawnBug();

            #endregion

            MoveFruitsAndInsects();

            GenerateState(simulationState);
        }

        #endregion

        #region Helpermethods

        /// <summary>
        /// Creates a state from the current game state
        /// </summary>
        /// <returns>current game state</returns>
        private void GenerateState(SimulationState simulationState)
        {
            simulationState.PlaygroundWidth = Playground.Width;
            simulationState.PlaygroundHeight = Playground.Height;
            simulationState.CurrentRound = currentRound;

            for (int i = 0; i < Teams.Length; i++)
            {
                simulationState.TeamStates.Add(Teams[i].CreateState());
            }

            for (int i = 0; i < Bugs.InsectsList.Count; i++)
            {
                simulationState.BugStates.Add(((CoreBug)Bugs.InsectsList[i]).GenerateInformation());
            }

            for (int i = 0; i < Playground.SugarHillsList.Count; i++)
            {
                simulationState.SugarStates.Add(Playground.SugarHillsList[i].CreateState());
            }

            for (int i = 0; i < Playground.FruitsList.Count; i++)
            {
                simulationState.FruitStates.Add(Playground.FruitsList[i].GenerateInformation());
            }
        }

        #endregion

        #region Events

        public event AreaChangeEventHandler AreaChange;

        #endregion
    }
}