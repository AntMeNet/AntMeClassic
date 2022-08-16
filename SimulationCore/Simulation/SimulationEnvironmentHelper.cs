using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    internal partial class SimulationEnvironment
    {
        #region common stuff

        /// <summary>
        /// Holds the current playground.
        /// </summary>
        internal CorePlayground Playground;

        /// <summary>
        /// Holds a list of active teams.
        /// </summary>
        internal CoreTeam[] Teams;

        /// <summary>
        /// Holds the "colony" of bugs.
        /// </summary>
        internal CoreColony Bugs;

        #endregion


        #region angle-precalculation

        /// <summary>
        /// Holds the calculated sin- and cos-values.
        /// </summary>
        public static int[,] Cos, Sin;

        /// <summary>
        /// Calculates all possible angles.
        /// </summary>
        private static void precalculateAngles()
        {
            int max = SimulationSettings.Custom.MaximumSpeed * PLAYGROUND_UNIT + 1;

            Cos = new int[max + 1, 360];
            Sin = new int[max + 1, 360];

            // Cosinus und Sinus Werte vorberechnen.
            for (int amplitude = 0; amplitude <= max; amplitude++)
            {
                for (int winkel = 0; winkel < 360; winkel++)
                {
                    Cos[amplitude, winkel] =
                      (int)Math.Round(amplitude * Math.Cos(winkel * Math.PI / 180d));
                    Sin[amplitude, winkel] =
                      (int)Math.Round(amplitude * Math.Sin(winkel * Math.PI / 180d));
                }
            }
        }

        public static int Cosinus(int amplitude, int winkel)
        {
            return (int)Math.Round(amplitude * Math.Cos(winkel * Math.PI / 180d));
        }

        public static int Sinus(int amplitude, int winkel)
        {
            return (int)Math.Round(amplitude * Math.Sin(winkel * Math.PI / 180d));
        }

        #endregion


        #region sugar-handling

        /// <summary>
        /// Delay-counter for sugar-respawn.
        /// </summary>
        private int sugarDelay;

        /// <summary>
        /// Counts down the total number of allowed sugar-hills.
        /// </summary>
        private int sugarCountDown;

        /// <summary>
        /// Gets the count of simultaneous existing sugar-hills. 
        /// </summary>
        private int sugarLimit;

        /// <summary>
        /// Removes all empty sugar-hills from list.
        /// </summary>
        private void removeSugar()
        {
            // TODO: speedup
            //List<CoreSugar> gemerkterZucker = new List<CoreSugar>();
            for (int i = 0; i < Playground.SugarHills.Count; i++)
            {
                CoreSugar zucker = Playground.SugarHills[i];
                if (zucker != null)
                {
                    if (zucker.Menge == 0)
                    {
                        //gemerkterZucker.Add(zucker);
                        //Löschen
                        Playground.EntferneZucker(zucker);
                        i--;
                    }
                }
            }
            //for(int i = 0; i < gemerkterZucker.Count; i++) {
            //  CoreSugar zucker = gemerkterZucker[i];
            //  if(zucker != null) {
            //    Playground.SugarHills.Remove(zucker);
            //  }
            //}
            //gemerkterZucker.Clear();
        }

        /// <summary>
        /// Spawns new sugar, if its time.
        /// </summary>
        private void spawnSugar()
        {
            if (Playground.SugarHills.Count < sugarLimit &&
               sugarDelay <= 0 &&
               sugarCountDown > 0)
            {
                sugarDelay = SimulationSettings.Custom.SugarRespawnDelay;
                sugarCountDown--;
                Playground.NeuerZucker();
            }
            sugarDelay--;
        }

        #endregion


        #region fruit-handling

        /// <summary>
        /// Delay-counter for fruit-respawn.
        /// </summary>
        private int fruitDelay;

        /// <summary>
        /// Counts down the total number of allowed fruits.
        /// </summary>
        private int fruitCountDown;

        /// <summary>
        /// Gets the count of simultaneous existing fruits. 
        /// </summary>
        private int fruitLimit;

        /// <summary>
        /// Spawns new fruit, if its time.
        /// </summary>
        private void spawnFruit()
        {
            if (Playground.Fruits.Count < fruitLimit &&
               fruitDelay <= 0 &&
               fruitCountDown > 0)
            {
                fruitDelay = SimulationSettings.Custom.FruitRespawnDelay;
                fruitCountDown--;
                Playground.NeuesObst();
            }
            fruitDelay--;
        }

        /// <summary>
        /// Removes fruit from list.
        /// </summary>
        /// <param name="colony">winning colony</param>
        private void removeFruit(CoreColony colony)
        {
            //List<CoreFruit> gemerktesObst = new List<CoreFruit>();
            for (int j = 0; j < Playground.Fruits.Count; j++)
            {
                CoreFruit obst = Playground.Fruits[j];
                for (int i = 0; i < colony.AntHills.Count; i++)
                {
                    CoreAnthill bau = colony.AntHills[i];
                    if (bau != null)
                    {
                        int entfernung = CoreCoordinate.BestimmeEntfernungI(obst.CoordinateBase, bau.CoordinateBase);
                        if (entfernung <= PLAYGROUND_UNIT)
                        {
                            //gemerktesObst.Add(obst);

                            // Löschen
                            colony.Statistik.CollectedFood += obst.Menge;
                            colony.Statistik.CollectedFruits++;
                            obst.Menge = 0;
                            for (int z = 0; z < obst.TragendeInsekten.Count; z++)
                            {
                                CoreInsect insect = obst.TragendeInsekten[z];
                                if (insect != null)
                                {
                                    insect.GetragenesObstBase = null;
                                    insect.AktuelleLastBase = 0;
                                    insect.RestStreckeI = 0;
                                    insect.RestWinkelBase = 0;
                                    insect.GeheZuBauBase();
                                }
                            }
                            obst.TragendeInsekten.Clear();
                            Playground.EntferneObst(obst);
                            j--;
                        }
                    }
                }
            }
        }

        #endregion


        #region ant-handling

        /// <summary>
        /// Gets the count of simultaneous existing ants. 
        /// </summary>
        private int antLimit;

        /// <summary>
        /// Prüft ob eine Ameise an ihrem Ziel angekommen ist.
        /// </summary>
        /// <param name="ant">betroffene Ameise</param>
        private static void ameiseUndZiel(CoreAnt ant)
        {
            // Ameisenbau.
            if (ant.ZielBase is CoreAnthill)
            {
                if (ant.GetragenesObstBase == null)
                {
                    ant.ZurückgelegteStreckeI = 0;
                    ant.ZielBase = null;
                    ant.SmelledMarker.Clear();
                    ant.colony.Statistik.CollectedFood += ant.AktuelleLastBase;
                    ant.AktuelleLastBase = 0;
                    ant.AktuelleEnergieBase = ant.MaximaleEnergieBase;
                    ant.IstMüdeBase = false;
                }
            }

            // Zuckerhaufen.
            else if (ant.ZielBase is CoreSugar)
            {
                CoreSugar zucker = (CoreSugar)ant.ZielBase;
                ant.ZielBase = null;
                if (zucker.Menge > 0)
                {
                    PlayerCall.TargetReached(ant, zucker);
                }
            }

            // Obststück.
            else if (ant.ZielBase is CoreFruit)
            {
                CoreFruit obst = (CoreFruit)ant.ZielBase;
                ant.ZielBase = null;
                if (obst.Menge > 0)
                {
                    PlayerCall.TargetReached(ant, obst);
                }
            }

            // Insekt.
            else if (ant.ZielBase is CoreInsect) { }

            // Anderes Ziel.
            else
            {
                ant.ZielBase = null;
            }
        }

        /// <summary>
        /// Prüft ob eine Ameise einen Zuckerhaufen sieht.
        /// </summary>
        /// <param name="ant">betroffene Ameise</param>
        private void ameiseUndZucker(CoreAnt ant)
        {
            for (int i = 0; i < Playground.SugarHills.Count; i++)
            {
                CoreSugar sugar = Playground.SugarHills[i];
                int entfernung = CoreCoordinate.BestimmeEntfernungI(ant.CoordinateBase, sugar.CoordinateBase);
                if (ant.ZielBase != sugar && entfernung <= ant.SichtweiteI)
                {
                    PlayerCall.Spots(ant, sugar);
                }
            }
        }

        /// <summary>
        /// Prüft ob eine Ameise ein Obsstück sieht.
        /// </summary>
        /// <param name="ameise">betroffene Ameise</param>
        private void ameiseUndObst(CoreAnt ameise)
        {
            for (int i = 0; i < Playground.Fruits.Count; i++)
            {
                CoreFruit obst = Playground.Fruits[i];
                int entfernung = CoreCoordinate.BestimmeEntfernungI(ameise.CoordinateBase, obst.CoordinateBase);
                if (ameise.ZielBase != obst && entfernung <= ameise.SichtweiteI)
                {
                    PlayerCall.Spots(ameise, obst);
                }
            }
        }

        /// <summary>
        /// Prüft ob die Ameise eine Markierung bemerkt.
        /// </summary>
        /// <param name="ameise">betroffene Ameise</param>
        private static void ameiseUndMarkierungen(CoreAnt ameise)
        {
            CoreMarker marker = ameise.colony.Marker.FindMarker(ameise);
            if (marker != null)
            {
                PlayerCall.SmellsFriend(ameise, marker);
                ameise.SmelledMarker.Add(marker);
            }
        }

        /// <summary>
        /// Erntfernt Ameisen die keine Energie mehr haben.
        /// </summary>
        /// <param name="colony">betroffenes Volk</param>
        private void removeAnt(CoreColony colony)
        {
            List<CoreAnt> liste = new List<CoreAnt>();

            for (int i = 0; i < colony.VerhungerteInsekten.Count; i++)
            {
                CoreAnt ant = colony.VerhungerteInsekten[i] as CoreAnt;
                if (ant != null && !liste.Contains(ant))
                {
                    liste.Add(ant);
                    colony.Statistik.StarvedAnts++;
                    PlayerCall.HasDied(ant, CoreKindOfDeath.Starved);
                }
            }

            for (int i = 0; i < colony.EatenInsects.Count; i++)
            {
                CoreAnt ant = colony.EatenInsects[i] as CoreAnt;
                if (ant != null && !liste.Contains(ant))
                {
                    liste.Add(ant);
                    colony.Statistik.EatenAnts++;
                    PlayerCall.HasDied(ant, CoreKindOfDeath.Eaten);
                }
            }

            for (int i = 0; i < colony.BeatenInsects.Count; i++)
            {
                CoreAnt ant = colony.BeatenInsects[i] as CoreAnt;
                if (ant != null)
                {
                    if (!liste.Contains(ant))
                    {
                        liste.Add(ant);
                        colony.Statistik.BeatenAnts++;
                        PlayerCall.HasDied(ant, CoreKindOfDeath.Beaten);
                    }
                }
            }

            for (int i = 0; i < liste.Count; i++)
            {
                CoreAnt ant = liste[i];
                if (ant != null)
                {
                    colony.EntferneInsekt(ant);

                    for (int j = 0; j < Playground.Fruits.Count; j++)
                    {
                        CoreFruit fruit = Playground.Fruits[j];
                        fruit.TragendeInsekten.Remove(ant);
                    }
                }
            }

            colony.VerhungerteInsekten.Clear();
            colony.EatenInsects.Clear();
            colony.BeatenInsects.Clear();
        }

        /// <summary>
        /// Erzeugt neue Ameisen.
        /// </summary>
        /// <param name="colony">betroffenes Volk</param>
        private void spawnAnt(CoreColony colony)
        {
            if (colony.Insects.Count < antLimit &&
               colony.insectDelay < 0 &&
               colony.insectCountDown > 0)
            {
                colony.NeuesInsekt(random);
                colony.insectDelay = SimulationSettings.Custom.AntRespawnDelay;
                colony.insectCountDown--;
            }
            colony.insectDelay--;
        }

        // Bewegt Obsstücke und alle Insekten die das Obsstück tragen.
        private void bewegeObstUndInsekten()
        {
            Playground.Fruits.ForEach(delegate (CoreFruit fruit)
            {
                if (fruit.TragendeInsekten.Count > 0)
                {
                    int dx = 0;
                    int dy = 0;
                    int last = 0;

                    fruit.TragendeInsekten.ForEach(delegate (CoreInsect insect)
                    {
                        if (insect.ZielBase != fruit && insect.RestWinkelBase == 0)
                        {
                            dx += Cos[insect.aktuelleGeschwindigkeitI, insect.RichtungBase];
                            dy += Sin[insect.aktuelleGeschwindigkeitI, insect.RichtungBase];
                            last += insect.AktuelleLastBase;
                            insect.RestStreckeI -= insect.aktuelleGeschwindigkeitI;
                            insect.ZurückgelegteStreckeI += insect.aktuelleGeschwindigkeitI;
                        }
                    });

                    last = Math.Min((int)(last * SimulationSettings.Custom.FruitLoadMultiplier), fruit.Menge);
                    dx = dx * last / fruit.Menge / fruit.TragendeInsekten.Count;
                    dy = dy * last / fruit.Menge / fruit.TragendeInsekten.Count;

                    fruit.CoordinateBase = new CoreCoordinate(fruit.CoordinateBase, dx, dy);
                    fruit.TragendeInsekten.ForEach(
                      delegate (CoreInsect insect) { insect.CoordinateBase = new CoreCoordinate(insect.CoordinateBase, dx, dy); });
                }
            });
            //foreach(CoreFruit obst in Playground.Fruits) {
            //  if(obst.TragendeInsekten.Count > 0) {
            //    int dx = 0;
            //    int dy = 0;
            //    int last = 0;

            //    foreach(CoreInsect insekt in obst.TragendeInsekten) {
            //      if(insekt.ZielBase != obst && insekt.RestWinkelBase == 0) {
            //        dx += Cos[insekt.aktuelleGeschwindigkeitI, insekt.RichtungBase];
            //        dy += Sin[insekt.aktuelleGeschwindigkeitI, insekt.RichtungBase];
            //        last += insekt.AktuelleLastBase;
            //      }
            //    }

            //    last = Math.Min((int)(last * SimulationSettings.Settings.FruitLoadMultiplier), obst.Menge);
            //    dx = dx * last / obst.Menge / obst.TragendeInsekten.Count;
            //    dy = dy * last / obst.Menge / obst.TragendeInsekten.Count;

            //    obst.Coordinate = new CoreCoordinate(obst.Coordinate, dx, dy);

            //    foreach(CoreInsect insekt in obst.TragendeInsekten) {
            //      insekt.Coordinate = new CoreCoordinate(insekt.Coordinate, dx, dy);
            //    }
            //  }
            //}
        }

        #endregion


        #region marker-handling

        /// <summary>
        /// Entfernt abgelaufene Markierungen und erzeugt neue Markierungen.
        /// </summary>
        /// <param name="colony">betroffenes Volk</param>
        private static void aktualisiereMarkierungen(CoreColony colony)
        {
            // TODO: Settings berücksichtigen
            // Markierungen aktualisieren und inaktive Markierungen löschen.
            List<CoreMarker> gemerkteMarkierungen = new List<CoreMarker>();

            foreach (CoreMarker markierung in colony.Marker)
            {
                if (markierung.IstAktiv)
                {
                    markierung.Aktualisieren();
                }
                else
                {
                    gemerkteMarkierungen.Add(markierung);
                }
            }
            gemerkteMarkierungen.ForEach(delegate (CoreMarker marker)
            {
                colony.Insects.ForEach(delegate (CoreInsect insect)
                {
                    CoreAnt ant = insect as CoreAnt;
                    if (ant != null)
                    {
                        ant.SmelledMarker.Remove(marker);
                    }
                });
            });
            colony.Marker.Remove(gemerkteMarkierungen);

            // Neue Markierungen überprüfen und hinzufügen.
            gemerkteMarkierungen.Clear();
            colony.NewMarker.ForEach(delegate (CoreMarker newMarker)
            {
                bool zuNah = false;
                foreach (CoreMarker markierung in colony.Marker)
                {
                    int entfernung =
                      CoreCoordinate.BestimmeEntfernungDerMittelpunkteI
                        (markierung.CoordinateBase, newMarker.CoordinateBase);
                    if (entfernung < SimulationSettings.Custom.MarkerDistance * PLAYGROUND_UNIT)
                    {
                        zuNah = true;
                        break;
                    }
                }
                if (!zuNah)
                {
                    colony.Marker.Add(newMarker);
                }
            });
            colony.NewMarker.Clear();
        }

        #endregion


        #region bug-handling

        /// <summary>
        /// Gets the count of simultaneous existing bugs. 
        /// </summary>
        private int bugLimit;

        /// <summary>
        /// Remove dead bugs.
        /// </summary>
        private void removeBugs()
        {
            for (int i = 0; i < Bugs.EatenInsects.Count; i++)
            {
                CoreBug bug = Bugs.EatenInsects[i] as CoreBug;
                if (bug != null)
                {
                    Bugs.Insects.Remove(bug);
                }
            }
            Bugs.EatenInsects.Clear();
        }

        /// <summary>
        /// Heals the bugs, if its time.
        /// </summary>
        private void healBugs()
        {
            if (currentRound % SimulationSettings.Custom.BugRegenerationDelay == 0)
            {
                for (int i = 0; i < Bugs.Insects.Count; i++)
                {
                    CoreBug bug = Bugs.Insects[i] as CoreBug;
                    if (bug != null)
                    {
                        if (bug.AktuelleEnergieBase < bug.MaximaleEnergieBase)
                        {
                            bug.AktuelleEnergieBase += SimulationSettings.Custom.BugRegenerationValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spawn new bugs, if needed.
        /// </summary>
        private void spawnBug()
        {
            if (Bugs.Insects.Count < bugLimit &&
               Bugs.insectDelay < 0 &&
               Bugs.insectCountDown > 0)
            {
                Bugs.NeuesInsekt(random);
                Bugs.insectDelay = SimulationSettings.Custom.BugRespawnDelay;
                Bugs.insectCountDown--;
            }
            Bugs.insectDelay--;
        }

        #endregion
    }
}