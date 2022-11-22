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
        /// Weitergabe des Verantwortungswechsels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerCallAreaChanged(object sender, AreaChangeEventArgs e)
        {
            AreaChange(this, e);
        }

        /// <summary>
        /// Initialisiert die Simulation um mit Runde 1 zu beginnen
        /// </summary>
        /// <param name="configuration">Konfiguration der Simulation</param>
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

            // Spielfeld erzeugen
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

            // Völker erzeugen
            Teams = new CoreTeam[configuration.Teams.Count];
            for (int i = 0; i < configuration.Teams.Count; i++)
            {
                TeamInfo team = configuration.Teams[i];
                Teams[i] = new CoreTeam(i, team.Guid, team.Name);
                Teams[i].Colonies = new CoreColony[team.Player.Count];

                // Völker erstellen
                for (int j = 0; j < team.Player.Count; j++)
                {
                    PlayerInfo player = team.Player[j];
                    CoreColony colony = new CoreColony(Playground, player, Teams[i]);
                    Teams[i].Colonies[j] = colony;

                    colony.AntHills.Add(Playground.NeuerBau(colony.Id));
                    colony.insectCountDown = antCountDown;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Berechnet einen neuen Spielschritt
        /// </summary>
        /// <returns>Zustandskopie des Simulationsstandes nachdem der Schritt ausgeführt wurde</returns>
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
                    Bugs.Grids[0].Add(Teams[i].Colonies[j].Insects);
                }
            }

            // Lasse die Wanzen von der Spiel Befehle entgegen nehmen.
            //foreach (CoreBug wanze in Bugs.Insects) {
            //    wanze.NimmBefehleEntgegen = true;
            //}

            // Schleife über alle Wanzen.
            for (int bugIndex = 0; bugIndex < Bugs.Insects.Count; bugIndex++)
            {
                CoreBug bug = Bugs.Insects[bugIndex] as CoreBug;
                Debug.Assert(bug != null);

                bug.AwaitingCommands = true;

                // Finde Ameisen in Angriffsreichweite.
                List<CoreInsect> ants = Bugs.Grids[0].FindAnts(bug);

                // Bestimme wie der Schaden auf die Ameisen verteilt wird.
                if (ants.Count >= SimulationSettings.Custom.BugAttack)
                {
                    // Es sind mehr Ameisen in der SpielUmgebung als die Wanze
                    // Schadenpunke verteilen kann. Daher werden den Ameisen am
                    // Anfang der Liste jeweils ein Energiepunkt abgezogen.
                    for (int index = 0; index < SimulationSettings.Custom.BugAttack; index++)
                    {
                        ants[index].currentEnergyBase--;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt)ants[index], bug);
                        if (ants[index].currentEnergyBase <= 0)
                        {
                            ants[index].colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }
                else if (ants.Count > 0)
                {
                    // Bestimme die Energie die von jeder Ameise abgezogen wird.
                    // Hier können natürlich Rundungsfehler auftreten, die die Wanze
                    // abschwächen, die ignorieren wir aber.
                    int damage = SimulationSettings.Custom.BugAttack / ants.Count;
                    for (int index = 0; index < ants.Count; index++)
                    {
                        ants[index].currentEnergyBase -= damage;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt)ants[index], bug);
                        if (ants[index].currentEnergyBase <= 0)
                        {
                            ants[index].colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }

                // Während eines Kampfes kann die Wanze sich nicht bewegen.
                if (ants.Count > 0)
                {
                    continue;
                }

                // Bewege die Wanze.
                bug.Move();
                if (bug.DistanceToDestinationBase == 0)
                {
                    bug.TurnIntoDirectionBase(random.Next(360));
                    bug.GoStraightAheadBase(random.Next(160, 320));
                }
                bug.AwaitingCommands = false;
            }

            // Verhindere, daß ein Spieler einer gesichteten Wanze Befehle gibt.
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

                    // Leere alle Buckets.
                    for (int casteIndex = 0; casteIndex < colony.CasteCount; casteIndex++)
                    {
                        colony.Grids[casteIndex].Clear();
                    }

                    // Fülle alle Buckets, aber befülle keinen Bucket doppelt.
                    for (int casteIndex = 0; casteIndex < colony.CasteCount; casteIndex++)
                    {
                        if (colony.Grids[casteIndex].Count == 0)
                        {
                            colony.Grids[casteIndex].Add(Bugs.Insects);
                            for (int j = 0; j < Teams.Length; j++)
                            {
                                for (int i = 0; i < Teams[j].Colonies.Length; i++)
                                {
                                    CoreColony v = Teams[j].Colonies[i];
                                    colony.Grids[casteIndex].Add(v.Insects);
                                }
                            }
                        }
                    }

                    // Schleife über alle Ameisen.
                    for (int antIndex = 0; antIndex < colony.Insects.Count; antIndex++)
                    {
                        CoreAnt ant = colony.Insects[antIndex] as CoreAnt;
                        Debug.Assert(ant != null);

                        // Finde und Zähle die Insekten im Sichtkreis der Ameise.
                        CoreBug bug;
                        CoreAnt enemy;
                        CoreAnt friend;
                        CoreAnt teammember;
                        int bugCount, enemyAntCount, colonyAntCount, casteAntCount, teamAntCount;
                        colony.Grids[ant.CasteIndexBase].FindAndCountInsects(
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
                        ant.BugsInViewrange = bugCount;
                        ant.ForeignAntsInViewrange = enemyAntCount;
                        ant.FriendlyAntsInViewrange = colonyAntCount;
                        ant.FriendlyAntsFromSameCasteInViewrange = casteAntCount;
                        ant.TeamAntsInViewrange = teamAntCount;

                        // Bewege die Ameise.
                        ant.Move();

                        #region Range

                        // Ameise hat ihre Reichweite überschritten.
                        if (ant.walkedDistance > colony.RangeI[ant.CasteIndexBase])
                        {
                            ant.currentEnergyBase = 0;
                            colony.StarvedInsects.Add(ant);
                            continue;
                        }

                        // Ameise hat ein Drittel ihrer Reichweite zurückgelegt.
                        else if (ant.walkedDistance > colony.RangeI[ant.CasteIndexBase] / 3)
                        {
                            if (ant.IsTiredBase == false)
                            {
                                ant.IsTiredBase = true;
                                PlayerCall.BecomesTired(ant);
                            }
                        }

                        #endregion

                        #region Fight

                        // Rufe die Ereignisse auf, falls die Ameise nicht schon ein 
                        // entsprechendes Ziel hat.
                        if (bug != null && !(ant.TargetBase is CoreBug))
                        {
                            PlayerCall.SpotsEnemy(ant, bug);
                        }
                        if (enemy != null && !(ant.TargetBase is CoreAnt) ||
                            (ant.TargetBase is CoreAnt && ((CoreAnt)ant.TargetBase).colony == colony))
                        {
                            PlayerCall.SpotsEnemy(ant, enemy);
                        }
                        if (friend != null && !(ant.TargetBase is CoreAnt) ||
                            (ant.TargetBase is CoreAnt && ((CoreAnt)ant.TargetBase).colony != colony))
                        {
                            PlayerCall.SpotsFriend(ant, friend);
                        }
                        if (teammember != null && !(ant.TargetBase is CoreAnt) ||
                            (ant.TargetBase is CoreAnt && ((CoreAnt)ant.TargetBase).colony != colony))
                        {
                            PlayerCall.SpotsTeamMember(ant, teammember);
                        }

                        // Kampf mit Wanze.
                        if (ant.TargetBase is CoreBug)
                        {
                            CoreBug k = (CoreBug)ant.TargetBase;
                            if (k.currentEnergyBase > 0)
                            {
                                int distance =
                                    CoreCoordinate.DetermineDistanceI(ant.CoordinateBase, ant.TargetBase.CoordinateBase);
                                if (distance < SimulationSettings.Custom.BattleRange * PLAYGROUND_UNIT)
                                {
                                    k.currentEnergyBase -= ant.AttackStrengthBase;
                                    if (k.currentEnergyBase <= 0)
                                    {
                                        Bugs.EatenInsects.Add(k);
                                        colony.Statistic.KilledBugs++;
                                        ant.StopMovementBase();
                                    }
                                }
                            }
                            else
                            {
                                ant.TargetBase = null;
                            }
                        }

                        // Kampf mit feindlicher Ameise.
                        else if (ant.TargetBase is CoreAnt)
                        {
                            CoreAnt a = (CoreAnt)ant.TargetBase;
                            if (a.colony != colony && a.currentEnergyBase > 0)
                            {
                                int distance =
                                    CoreCoordinate.DetermineDistanceI(ant.CoordinateBase, ant.TargetBase.CoordinateBase);
                                if (distance < SimulationSettings.Custom.BattleRange * PLAYGROUND_UNIT)
                                {
                                    PlayerCall.UnderAttack(a, ant);
                                    a.currentEnergyBase -= ant.AttackStrengthBase;
                                    if (a.currentEnergyBase <= 0)
                                    {
                                        a.colony.BeatenInsects.Add(a);
                                        colony.Statistic.KilledAnts++;
                                        ant.StopMovementBase();
                                    }
                                }
                            }
                            else
                            {
                                ant.TargetBase = null;
                            }
                        }

                        #endregion

                        // Prüfe ob die Ameise an ihrem Ziel angekommen ist.
                        if (ant.ReachedBase)
                        {
                            antAndTarget(ant);
                        }

                        // Prüfe ob die Ameise einen Zuckerhaufen oder ein Obststück sieht.
                        antAndSugar(ant);
                        if (ant.CarryingFruitBase == null)
                        {
                            antAndFruit(ant);
                        }

                        // Prüfe ob die Ameise eine Markierung bemerkt.
                        antAndMarkers(ant);

                        if (ant.TargetBase == null && ant.DistanceToDestinationBase == 0)
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
        /// Erzeugt einen Zustand aus dem aktuellen Spielumstand
        /// </summary>
        /// <returns>aktueller Spielstand</returns>
        private void GenerateState(SimulationState simulationState)
        {
            simulationState.PlaygroundWidth = Playground.Width;
            simulationState.PlaygroundHeight = Playground.Height;
            simulationState.CurrentRound = currentRound;

            for (int i = 0; i < Teams.Length; i++)
            {
                simulationState.TeamStates.Add(Teams[i].CreateState());
            }

            for (int i = 0; i < Bugs.Insects.Count; i++)
            {
                simulationState.BugStates.Add(((CoreBug)Bugs.Insects[i]).GenerateInformation());
            }

            for (int i = 0; i < Playground.SugarHills.Count; i++)
            {
                simulationState.SugarStates.Add(Playground.SugarHills[i].CreateState());
            }

            for (int i = 0; i < Playground.Fruits.Count; i++)
            {
                simulationState.FruitStates.Add(Playground.Fruits[i].GenerateInformation());
            }
        }

        #endregion

        #region Events

        public event AreaChangeEventHandler AreaChange;

        #endregion
    }
}