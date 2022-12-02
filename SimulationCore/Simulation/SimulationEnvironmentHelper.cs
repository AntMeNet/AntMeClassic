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

            // precalculation of cosinus and sinus
            for (int amplitude = 0; amplitude <= max; amplitude++)
            {
                for (int angle = 0; angle < 360; angle++)
                {
                    Cos[amplitude, angle] =
                      (int)Math.Round(amplitude * Math.Cos(angle * Math.PI / 180d));
                    Sin[amplitude, angle] =
                      (int)Math.Round(amplitude * Math.Sin(angle * Math.PI / 180d));
                }
            }
        }

        public static int Cosinus(int amplitude, int angle)
        {
            return (int)Math.Round(amplitude * Math.Cos(angle * Math.PI / 180d));
        }

        public static int Sinus(int amplitude, int angle)
        {
            return (int)Math.Round(amplitude * Math.Sin(angle * Math.PI / 180d));
        }

        #endregion


        #region sugar handling

        /// <summary>
        /// sugar respawn delay counter
        /// </summary>
        private int sugarDelay;

        /// <summary>
        /// Countdown number of total allowed sugar hills
        /// </summary>
        private int sugarCountDown;

        /// <summary>
        /// number of simultaneous existing sugar hills. 
        /// </summary>
        private int sugarLimit;

        /// <summary>
        /// Removes all empty sugar hills from list.
        /// </summary>
        private void removeSugar()
        {
            // TODO: speedup
            //List<CoreSugar> gemerkterZucker = new List<CoreSugar>();
            for (int i = 0; i < Playground.SugarHillsList.Count; i++)
            {
                CoreSugar sugar = Playground.SugarHillsList[i];
                if (sugar != null)
                {
                    if (sugar.Amount == 0)
                    {
                        //gemerkterZucker.Add(zucker);
                        // Remove Sugar.
                        Playground.RemoveSugar(sugar);
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
            if (Playground.SugarHillsList.Count < sugarLimit &&
               sugarDelay <= 0 &&
               sugarCountDown > 0)
            {
                sugarDelay = SimulationSettings.Custom.SugarRespawnDelay;
                sugarCountDown--;
                Playground.NewSugar();
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
        /// Gets the number of simultaneous existing fruits. 
        /// </summary>
        private int fruitLimit;

        /// <summary>
        /// Spawns new fruit, if its time.
        /// </summary>
        private void spawnFruit()
        {
            if (Playground.FruitsList.Count < fruitLimit &&
               fruitDelay <= 0 &&
               fruitCountDown > 0)
            {
                fruitDelay = SimulationSettings.Custom.FruitRespawnDelay;
                fruitCountDown--;
                Playground.NewFruit();
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
            for (int j = 0; j < Playground.FruitsList.Count; j++)
            {
                CoreFruit fruit = Playground.FruitsList[j];
                for (int i = 0; i < colony.AntHills.Count; i++)
                {
                    CoreAnthill anthill = colony.AntHills[i];
                    if (anthill != null)
                    {
                        int distanceI = CoreCoordinate.DetermineDistanceI(fruit.CoordinateCoreInsect, anthill.CoordinateCoreInsect);
                        if (distanceI <= PLAYGROUND_UNIT)
                        {
                            //gemerktesObst.Add(obst);

                            // Delete
                            colony.Statistic.CollectedFood += fruit.Amount;
                            colony.Statistic.CollectedFruits++;
                            fruit.Amount = 0;
                            for (int z = 0; z < fruit.InsectsCarrying.Count; z++)
                            {
                                CoreInsect insect = fruit.InsectsCarrying[z];
                                if (insect != null)
                                {
                                    insect.CarryingFruitCoreInsect = null;
                                    insect.CurrentLoadCoreInsect = 0;
                                    insect.DistanceToDestination = 0;
                                    insect.ResidualAngle = 0;
                                    insect.GoToAnthillCoreInsect();
                                }
                            }
                            fruit.InsectsCarrying.Clear();
                            Playground.RemoveFruit(fruit);
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
        /// Checks whether an ant has arrived at its destination.
        /// </summary>
        /// <param name="ant">ant</param>
        private static void antAndTarget(CoreAnt ant)
        {
            // Ant hill.
            if (ant.DestinationCoreInsect is CoreAnthill)
            {
                if (ant.CarryingFruitCoreInsect == null)
                {
                    ant.NumberStepsWalked = 0;
                    ant.DestinationCoreInsect = null;
                    ant.SmelledMarker.Clear();
                    ant.Colony.Statistic.CollectedFood += ant.CurrentLoadCoreInsect;
                    ant.CurrentLoadCoreInsect = 0;
                    ant.currentEnergyCoreInsect = ant.MaximumEnergyCoreInsect;
                    ant.IsTiredCoreAnt = false;
                }
            }

            // Sugar hill.
            else if (ant.DestinationCoreInsect is CoreSugar)
            {
                CoreSugar sugar = (CoreSugar)ant.DestinationCoreInsect;
                ant.DestinationCoreInsect = null;
                if (sugar.Amount > 0)
                {
                    PlayerCall.TargetReached(ant, sugar);
                }
            }

            // Fruit.
            else if (ant.DestinationCoreInsect is CoreFruit)
            {
                CoreFruit fruit = (CoreFruit)ant.DestinationCoreInsect;
                ant.DestinationCoreInsect = null;
                if (fruit.Amount > 0)
                {
                    PlayerCall.TargetReached(ant, fruit);
                }
            }

            // Insect.
            else if (ant.DestinationCoreInsect is CoreInsect) { }

            // Other target.
            else
            {
                ant.DestinationCoreInsect = null;
            }
        }

        /// <summary>
        /// Check whether an ant sees a sugar pile.
        /// </summary>
        /// <param name="ant">ant</param>
        private void antAndSugar(CoreAnt ant)
        {
            for (int i = 0; i < Playground.SugarHillsList.Count; i++)
            {
                CoreSugar sugar = Playground.SugarHillsList[i];
                int distanceI = CoreCoordinate.DetermineDistanceI(ant.CoordinateCoreInsect, sugar.CoordinateCoreInsect);
                if (ant.DestinationCoreInsect != sugar && distanceI <= ant.ViewRangeI)
                {
                    PlayerCall.Spots(ant, sugar);
                }
            }
        }

        /// <summary>
        /// Checks if an ant sees a fruit.
        /// </summary>
        /// <param name="ant">ant</param>
        private void antAndFruit(CoreAnt ant)
        {
            for (int i = 0; i < Playground.FruitsList.Count; i++)
            {
                CoreFruit fruit = Playground.FruitsList[i];
                int distanceI = CoreCoordinate.DetermineDistanceI(ant.CoordinateCoreInsect, fruit.CoordinateCoreInsect);
                if (ant.DestinationCoreInsect != fruit && distanceI <= ant.ViewRangeI)
                {
                    PlayerCall.Spots(ant, fruit);
                }
            }
        }

        /// <summary>
        /// Check whether the ant notices a mark.
        /// </summary>
        /// <param name="ant">ant</param>
        private static void antAndMarkers(CoreAnt ant)
        {
            CoreMarker marker = ant.Colony.Marker.FindMarker(ant);
            if (marker != null)
            {
                PlayerCall.SmellsFriend(ant, marker);
                ant.SmelledMarker.Add(marker);
            }
        }

        /// <summary>
        /// Removes ants that have no more energy.
        /// </summary>
        /// <param name="colony">colony</param>
        private void removeAnt(CoreColony colony)
        {
            List<CoreAnt> listAnts = new List<CoreAnt>();

            for (int i = 0; i < colony.StarvedInsects.Count; i++)
            {
                CoreAnt ant = colony.StarvedInsects[i] as CoreAnt;
                if (ant != null && !listAnts.Contains(ant))
                {
                    listAnts.Add(ant);
                    colony.Statistic.StarvedAnts++;
                    PlayerCall.HasDied(ant, CoreKindOfDeath.Starved);
                }
            }

            for (int i = 0; i < colony.EatenInsects.Count; i++)
            {
                CoreAnt ant = colony.EatenInsects[i] as CoreAnt;
                if (ant != null && !listAnts.Contains(ant))
                {
                    listAnts.Add(ant);
                    colony.Statistic.EatenAnts++;
                    PlayerCall.HasDied(ant, CoreKindOfDeath.Eaten);
                }
            }

            for (int i = 0; i < colony.BeatenInsects.Count; i++)
            {
                CoreAnt ant = colony.BeatenInsects[i] as CoreAnt;
                if (ant != null)
                {
                    if (!listAnts.Contains(ant))
                    {
                        listAnts.Add(ant);
                        colony.Statistic.BeatenAnts++;
                        PlayerCall.HasDied(ant, CoreKindOfDeath.Beaten);
                    }
                }
            }

            for (int i = 0; i < listAnts.Count; i++)
            {
                CoreAnt ant = listAnts[i];
                if (ant != null)
                {
                    colony.RemoveInsect(ant);

                    for (int j = 0; j < Playground.FruitsList.Count; j++)
                    {
                        CoreFruit fruit = Playground.FruitsList[j];
                        fruit.InsectsCarrying.Remove(ant);
                    }
                }
            }

            colony.StarvedInsects.Clear();
            colony.EatenInsects.Clear();
            colony.BeatenInsects.Clear();
        }

        /// <summary>
        /// Spawn new ant.
        /// </summary>
        /// <param name="colony">colony</param>
        private void spawnAnt(CoreColony colony)
        {
            if (colony.InsectsList.Count < antLimit &&
               colony.insectDelay < 0 &&
               colony.insectCountDown > 0)
            {
                colony.NewInsect(random);
                colony.insectDelay = SimulationSettings.Custom.AntRespawnDelay;
                colony.insectCountDown--;
            }
            colony.insectDelay--;
        }

        // Moves fruit and all insects that carry the fruit.
        private void MoveFruitsAndInsects()
        {
            Playground.FruitsList.ForEach(delegate (CoreFruit fruit)
            {
                if (fruit.InsectsCarrying.Count > 0)
                {
                    int dx = 0;
                    int dy = 0;
                    int last = 0;

                    fruit.InsectsCarrying.ForEach(delegate (CoreInsect insect)
                    {
                        if (insect.DestinationCoreInsect != fruit && insect.ResidualAngle == 0)
                        {
                            dx += Cos[insect.currentSpeedICoreInsect, insect.GetDirectionCoreInsect()];
                            dy += Sin[insect.currentSpeedICoreInsect, insect.GetDirectionCoreInsect()];
                            last += insect.CurrentLoadCoreInsect;
                        }
                    });

                    last = Math.Min((int)(last * SimulationSettings.Custom.FruitLoadMultiplier), fruit.Amount);
                    dx = dx * last / fruit.Amount / fruit.InsectsCarrying.Count;
                    dy = dy * last / fruit.Amount / fruit.InsectsCarrying.Count;

                    fruit.CoordinateCoreInsect = new CoreCoordinate(fruit.CoordinateCoreInsect, dx, dy);
                    fruit.InsectsCarrying.ForEach(
                      delegate (CoreInsect insect) { insect.CoordinateCoreInsect = new CoreCoordinate(insect.CoordinateCoreInsect, dx, dy); });
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
        /// Removes expired markers and creates new markers.
        /// </summary>
        /// <param name="colony">colony</param>
        private static void updateMarkers(CoreColony colony)
        {
            // TODO: Settings beruecksichtigen
            // Update markers and delete inactive markers.
            List<CoreMarker> rememberedMarkers = new List<CoreMarker>();

            foreach (CoreMarker marker in colony.Marker)
            {
                if (marker.IsActive)
                {
                    marker.Update();
                }
                else
                {
                    rememberedMarkers.Add(marker);
                }
            }
            rememberedMarkers.ForEach(delegate (CoreMarker marker)
            {
                colony.InsectsList.ForEach(delegate (CoreInsect insect)
                {
                    CoreAnt ant = insect as CoreAnt;
                    if (ant != null)
                    {
                        ant.SmelledMarker.Remove(marker);
                    }
                });
            });
            colony.Marker.Remove(rememberedMarkers);

            // Check and add new markers.
            rememberedMarkers.Clear();
            colony.NewMarker.ForEach(delegate (CoreMarker newMarker)
            {
                bool tooNear = false;
                foreach (CoreMarker marker in colony.Marker)
                {
                    int distance =
                      CoreCoordinate.DetermineDistanceToCenter
                        (marker.CoordinateCoreInsect, newMarker.CoordinateCoreInsect);
                    if (distance < SimulationSettings.Custom.MarkerDistance * PLAYGROUND_UNIT)
                    {
                        tooNear = true;
                        break;
                    }
                }
                if (!tooNear)
                {
                    colony.Marker.Add(newMarker);
                }
            });
            colony.NewMarker.Clear();
        }

        #endregion


        #region bug-handling

        /// <summary>
        /// Gets the number of simultaneous existing bugs. 
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
                    Bugs.InsectsList.Remove(bug);
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
                for (int i = 0; i < Bugs.InsectsList.Count; i++)
                {
                    CoreBug bug = Bugs.InsectsList[i] as CoreBug;
                    if (bug != null)
                    {
                        if (bug.currentEnergyCoreInsect < bug.MaximumEnergyCoreInsect)
                        {
                            bug.currentEnergyCoreInsect += SimulationSettings.Custom.BugRegenerationValue;
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
            if (Bugs.InsectsList.Count < bugLimit &&
               Bugs.insectDelay < 0 &&
               Bugs.insectCountDown > 0)
            {
                Bugs.NewInsect(random);
                Bugs.insectDelay = SimulationSettings.Custom.BugRespawnDelay;
                Bugs.insectCountDown--;
            }
            Bugs.insectDelay--;
        }

        #endregion
    }
}