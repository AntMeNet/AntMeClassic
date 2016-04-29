using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;

using AntMe.SharedComponents.States;

namespace AntMe.Simulation {
    /// <summary>
    /// Holds all simulation-elements.
    /// </summary>
    internal sealed partial class SimulationEnvironment {
        #region Constants

        public const int PLAYGROUND_UNIT = 64;

        #endregion

        #region private variables

        private int currentRound;
        private int playerCount;

        #endregion

        #region Constructor and Init

        /// <summary>
        /// Creates a new instance of SimulationEnvironment
        /// </summary>
        public SimulationEnvironment() {
            precalculateAngles();
            PlayerCall.AreaChanged += playerCallAreaChanged;
        }

        /// <summary>
        /// Weitergabe des Verantwortungswechsels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playerCallAreaChanged(object sender, AreaChangeEventArgs e) {
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
        public void Init(SimulatorConfiguration configuration) {
            // Init some varialbes
            currentRound = 0;

            // count players
            playerCount = 0;
            foreach (TeamInfo team in configuration.Teams) {
                playerCount += team.Player.Count;
            }

            // Sugar-relevant stuff
            sugarDelay = 0;
            sugarCountDown = (int) (SimulationSettings.Custom.SugarTotalCount*
                                    (1 + (SimulationSettings.Custom.SugarTotalCountPlayerMultiplier*playerCount)));
            sugarLimit = (int) (SimulationSettings.Custom.SugarSimultaneousCount*
                                (1 + (SimulationSettings.Custom.SugarCountPlayerMultiplier*playerCount)));

            // Fruit-relevant stuff
            fruitDelay = 0;
            fruitCountDown = (int) (SimulationSettings.Custom.FruitTotalCount*
                                    (1 + (SimulationSettings.Custom.FruitTotalCountPlayerMultiplier*playerCount)));
            fruitLimit = (int) (SimulationSettings.Custom.FruitSimultaneousCount*
                                (1 + (SimulationSettings.Custom.FruitCountPlayerMultiplier*playerCount)));

            // Ant-relevant stuff
            int antCountDown = (int) (SimulationSettings.Custom.AntTotalCount*
                                      (1 + (SimulationSettings.Custom.AntTotalCountPlayerMultiplier*playerCount)));
            antLimit = (int) (SimulationSettings.Custom.AntSimultaneousCount*
                              (1 + (SimulationSettings.Custom.AntCountPlayerMultiplier*playerCount)));

            // Spielfeld erzeugen
            float area = SimulationSettings.Custom.PlayGroundBaseSize;
            area *= 1 + (playerCount * SimulationSettings.Custom.PlayGroundSizePlayerMultiplier);
            int playgroundWidth = (int)Math.Round(Math.Sqrt(area * 4 / 3));
            int playgroundHeight = (int)Math.Round(Math.Sqrt(area * 3 / 4));
            Playground = new CorePlayground(playgroundWidth, playgroundHeight, configuration.MapInitialValue);

            // Bugs-relevant stuff
            Bugs = new CoreColony(Playground);
            Bugs.insectCountDown = (int) (SimulationSettings.Custom.BugTotalCount*
                                          (1 + (SimulationSettings.Custom.BugTotalCountPlayerMultiplier*playerCount)));
            bugLimit = (int) (SimulationSettings.Custom.BugSimultaneousCount*
                              (1 + (SimulationSettings.Custom.BugCountPlayerMultiplier*playerCount)));

            // Völker erzeugen
            Teams = new CoreTeam[configuration.Teams.Count];
            for (int i = 0; i < configuration.Teams.Count; i++) {
                TeamInfo team = configuration.Teams[i];
                Teams[i] = new CoreTeam(i, team.Guid, team.Name);
                Teams[i].Colonies = new CoreColony[team.Player.Count];

                // Völker erstellen
                for (int j = 0; j < team.Player.Count; j++) {
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
        public void Step(SimulationState simulationState) {
            currentRound++;

            #region Food

            removeSugar();
            spawnSugar();
            spawnFruit();

            #endregion

            #region Bugs

            Bugs.Grids[0].Clear();
            for (int i = 0; i < Teams.Length; i++) {
                for (int j = 0; j < Teams[i].Colonies.Length; j++) {
                    Bugs.Grids[0].Add(Teams[i].Colonies[j].Insects);
                }
            }

            // Lasse die Wanzen von der Spiel Befehle entgegen nehmen.
            //foreach (CoreBug wanze in Bugs.Insects) {
            //    wanze.NimmBefehleEntgegen = true;
            //}

            // Schleife über alle Wanzen.
            for (int bugIndex = 0; bugIndex < Bugs.Insects.Count; bugIndex++) {
                CoreBug bug = Bugs.Insects[bugIndex] as CoreBug;
                Debug.Assert(bug != null);

                bug.NimmBefehleEntgegen = true;

                // Finde Ameisen in Angriffsreichweite.
                List<CoreInsect> ants = Bugs.Grids[0].FindAnts(bug);

                // Bestimme wie der Schaden auf die Ameisen verteilt wird.
                if (ants.Count >= SimulationSettings.Custom.BugAttack) {
                    // Es sind mehr Ameisen in der SpielUmgebung als die Wanze
                    // Schadenpunke verteilen kann. Daher werden den Ameisen am
                    // Anfang der Liste jeweils ein Energiepunkt abgezogen.
                    for (int index = 0; index < SimulationSettings.Custom.BugAttack; index++) {
                        ants[index].AktuelleEnergieBase--;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt) ants[index], bug);
                        if (ants[index].AktuelleEnergieBase <= 0) {
                            ants[index].colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }
                else if (ants.Count > 0) {
                    // Bestimme die Energie die von jeder Ameise abgezogen wird.
                    // Hier können natürlich Rundungsfehler auftreten, die die Wanze
                    // abschwächen, die ignorieren wir aber.
                    int schaden = SimulationSettings.Custom.BugAttack/ants.Count;
                    for (int index = 0; index < ants.Count; index++) {
                        ants[index].AktuelleEnergieBase -= schaden;
                        //((Ameise)ameisen[i]).WirdAngegriffen(wanze);
                        PlayerCall.UnderAttack((CoreAnt) ants[index], bug);
                        if (ants[index].AktuelleEnergieBase <= 0) {
                            ants[index].colony.EatenInsects.Add(ants[index]);
                        }
                    }
                }

                // Während eines Kampfes kann die Wanze sich nicht bewegen.
                if (ants.Count > 0) {
                    continue;
                }

                // Bewege die Wanze.
                bug.Bewegen();
                if (bug.RestStreckeBase == 0) {
                    bug.DreheInRichtungBase(AntMe.English.RandomNumber.Number(360));
                    bug.GeheGeradeausBase(AntMe.English.RandomNumber.Number(160, 320));
                }
                bug.NimmBefehleEntgegen = false;
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
            for (int teamIndex = 0; teamIndex < Teams.Length; teamIndex++) {
                // Loop through all colonies in that team.
                for (int colonyIndex = 0; colonyIndex < Teams[teamIndex].Colonies.Length; colonyIndex++) {
                    CoreColony colony = Teams[teamIndex].Colonies[colonyIndex];

                    // Leere alle Buckets.
                    for (int casteIndex = 0; casteIndex < colony.AnzahlKasten; casteIndex++) {
                        colony.Grids[casteIndex].Clear();
                    }

                    // Fülle alle Buckets, aber befülle keinen Bucket doppelt.
                    for (int casteIndex = 0; casteIndex < colony.AnzahlKasten; casteIndex++) {
                        if (colony.Grids[casteIndex].Count == 0) {
                            colony.Grids[casteIndex].Add(Bugs.Insects);
                            for (int j = 0; j < Teams.Length; j++) {
                                for (int i = 0; i < Teams[j].Colonies.Length; i++) {
                                    CoreColony v = Teams[j].Colonies[i];
                                    colony.Grids[casteIndex].Add(v.Insects);
                                }
                            }
                        }
                    }

                    // Schleife über alle Ameisen.
                    for (int antIndex = 0; antIndex < colony.Insects.Count; antIndex++) {
                        CoreAnt ameise = colony.Insects[antIndex] as CoreAnt;
                        Debug.Assert(ameise != null);

                        // Finde und Zähle die Insekten im Sichtkreis der Ameise.
                        CoreBug wanze;
                        CoreAnt feind;
                        CoreAnt freund;
                        CoreAnt teammember;
                        int bugCount, enemyAntCount, colonyAntCount, casteAntCount, teamAntCount;
                        colony.Grids[ameise.CasteIndexBase].FindAndCountInsects(
                            ameise,
                            out wanze,
                            out bugCount,
                            out feind,
                            out enemyAntCount,
                            out freund,
                            out colonyAntCount,
                            out casteAntCount,
                            out teammember,
                            out teamAntCount);
                        ameise.BugsInViewrange = bugCount;
                        ameise.ForeignAntsInViewrange = enemyAntCount;
                        ameise.FriendlyAntsInViewrange = colonyAntCount;
                        ameise.FriendlyAntsFromSameCasteInViewrange = casteAntCount;
                        ameise.TeamAntsInViewrange = teamAntCount;

                        // Bewege die Ameise.
                        ameise.Bewegen();

                        #region Reichweite

                        // Ameise hat ihre Reichweite überschritten.
                        if (ameise.ZurückgelegteStreckeI > colony.ReichweiteI[ameise.CasteIndexBase]) {
                            ameise.AktuelleEnergieBase = 0;
                            colony.VerhungerteInsekten.Add(ameise);
                            continue;
                        }

                            // Ameise hat ein Drittel ihrer Reichweite zurückgelegt.
                        else if (ameise.ZurückgelegteStreckeI > colony.ReichweiteI[ameise.CasteIndexBase]/3) {
                            if (ameise.IstMüdeBase == false) {
                                ameise.IstMüdeBase = true;
                                PlayerCall.BecomesTired(ameise);
                            }
                        }

                        #endregion

                        #region Kampf

                        // Rufe die Ereignisse auf, falls die Ameise nicht schon ein 
                        // entsprechendes Ziel hat.
                        if (wanze != null && !(ameise.ZielBase is CoreBug)) {
                            PlayerCall.SpotsEnemy(ameise, wanze);
                        }
                        if (feind != null && !(ameise.ZielBase is CoreAnt) ||
                            (ameise.ZielBase is CoreAnt && ((CoreAnt) ameise.ZielBase).colony == colony)) {
                            PlayerCall.SpotsEnemy(ameise, feind);
                        }
                        if (freund != null && !(ameise.ZielBase is CoreAnt) ||
                            (ameise.ZielBase is CoreAnt && ((CoreAnt) ameise.ZielBase).colony != colony)) {
                            PlayerCall.SpotsFriend(ameise, freund);
                        }
                        if (teammember != null && !(ameise.ZielBase is CoreAnt) ||
                            (ameise.ZielBase is CoreAnt && ((CoreAnt) ameise.ZielBase).colony != colony)) {
                            PlayerCall.SpotsTeamMember(ameise, teammember);
                        }

                        // Kampf mit Wanze.
                        if (ameise.ZielBase is CoreBug) {
                            CoreBug k = (CoreBug) ameise.ZielBase;
                            if (k.AktuelleEnergieBase > 0) {
                                int entfernung =
                                    CoreCoordinate.BestimmeEntfernungI(ameise.CoordinateBase, ameise.ZielBase.CoordinateBase);
                                if (entfernung < SimulationSettings.Custom.BattleRange*PLAYGROUND_UNIT) {
                                    k.AktuelleEnergieBase -= ameise.AngriffBase;
                                    if (k.AktuelleEnergieBase <= 0) {
                                        Bugs.EatenInsects.Add(k);
                                        colony.Statistik.KilledBugs++;
                                        ameise.BleibStehenBase();
                                    }
                                }
                            }
                            else {
                                ameise.ZielBase = null;
                            }
                        }

                            // Kampf mit feindlicher Ameise.
                        else if (ameise.ZielBase is CoreAnt) {
                            CoreAnt a = (CoreAnt) ameise.ZielBase;
                            if (a.colony != colony && a.AktuelleEnergieBase > 0) {
                                int entfernung =
                                    CoreCoordinate.BestimmeEntfernungI(ameise.CoordinateBase, ameise.ZielBase.CoordinateBase);
                                if (entfernung < SimulationSettings.Custom.BattleRange*PLAYGROUND_UNIT) {
                                    PlayerCall.UnderAttack(a, ameise);
                                    a.AktuelleEnergieBase -= ameise.AngriffBase;
                                    if (a.AktuelleEnergieBase <= 0) {
                                        a.colony.BeatenInsects.Add(a);
                                        colony.Statistik.KilledAnts++;
                                        ameise.BleibStehenBase();
                                    }
                                }
                            }
                            else {
                                ameise.ZielBase = null;
                            }
                        }

                        #endregion

                        // Prüfe ob die Ameise an ihrem Ziel angekommen ist.
                        if (ameise.AngekommenBase) {
                            ameiseUndZiel(ameise);
                        }

                        // Prüfe ob die Ameise einen Zuckerhaufen oder ein Obststück sieht.
                        ameiseUndZucker(ameise);
                        if (ameise.GetragenesObstBase == null) {
                            ameiseUndObst(ameise);
                        }

                        // Prüfe ob die Ameise eine Markierung bemerkt.
                        ameiseUndMarkierungen(ameise);

                        if (ameise.ZielBase == null && ameise.RestStreckeBase == 0) {
                            PlayerCall.Waits(ameise);
                        }

                        PlayerCall.Tick(ameise);
                    }

                    removeAnt(colony);
                    spawnAnt(colony);

                    aktualisiereMarkierungen(colony);
                    removeFruit(colony);
                }
            }

            #endregion

            #region Bugs again

            removeBugs();
            healBugs();
            spawnBug();

            #endregion

            bewegeObstUndInsekten();

            erzeugeZustand(simulationState);
        }

        #endregion

        #region Helpermethods

        /// <summary>
        /// Erzeugt einen Zustand aus dem aktuellen Spielumstand
        /// </summary>
        /// <returns>aktueller Spielstand</returns>
        private void erzeugeZustand(SimulationState zustand) {
            zustand.PlaygroundWidth = Playground.Width;
            zustand.PlaygroundHeight = Playground.Height;
            zustand.CurrentRound = currentRound;

            for (int i = 0; i < Teams.Length; i++) {
                zustand.TeamStates.Add(Teams[i].CreateState());
            }

            for (int i = 0; i < Bugs.Insects.Count; i++) {
                zustand.BugStates.Add(((CoreBug) Bugs.Insects[i]).ErzeugeInfo());
            }

            for (int i = 0; i < Playground.SugarHills.Count; i++) {
                zustand.SugarStates.Add(Playground.SugarHills[i].CreateState());
            }

            for (int i = 0; i < Playground.Fruits.Count; i++) {
                zustand.FruitStates.Add(Playground.Fruits[i].ErzeugeInfo());
            }
        }

        #endregion

        #region Events

        public event AreaChangeEventHandler AreaChange;

        #endregion
    }
}