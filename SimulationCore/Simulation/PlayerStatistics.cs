using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Saves statistics of player.
    /// </summary>
    [Serializable]
    public struct PlayerStatistics
    {
        /// <summary>
        /// Current ant count.
        /// </summary>
        public int CurrentAntCount;

        private int LoopCount;

        /// <summary>
        /// Number of killed enemy ants defeated by players ants.
        /// </summary>
        public int KilledAnts;

        /// <summary>
        /// Number of killed bugs defeated by players ants.
        /// </summary>
        public int KilledBugs;

        /// <summary>
        /// Sum of collected food points.
        /// </summary>
        public int CollectedFood;

        /// <summary>
        /// Number of collected fruits.
        /// </summary>
        public int CollectedFruits;

        /// <summary>
        /// Sum of starved ants.
        /// </summary>
        public int StarvedAnts;

        /// <summary>
        /// Number of beaten ants defeated by the enemy.
        /// </summary>
        public int BeatenAnts;

        /// <summary>
        /// Number of ants eaten by bugs.
        /// </summary>
        public int EatenAnts;

        /// <summary>
        /// Sum of all points.
        /// </summary>
        public int Points
        {
            get
            {
                return (
                    (int)(SimulationSettings.Custom.PointsForFoodMultiplier * CollectedFood) +
                    (SimulationSettings.Custom.PointsForFruits * CollectedFruits) +
                    (SimulationSettings.Custom.PointsForBug * KilledBugs) +
                    (SimulationSettings.Custom.PointsForForeignAnt * KilledAnts) +
                    (SimulationSettings.Custom.PointsForBeatenAnts * BeatenAnts) +
                    (SimulationSettings.Custom.PointsForEatenAnts * EatenAnts) +
                    (SimulationSettings.Custom.PointsForStarvedAnts * StarvedAnts)
                       ) / Math.Max(LoopCount, 1);
            }
        }

        /// <summary>
        /// Sums up two statistics of the same player.
        /// </summary>
        /// <param name="s1">statistics 1</param>
        /// <param name="s2">statistics 2</param>
        /// <returns>Statistik 1 + Statistik 2.</returns>
        public static PlayerStatistics
            operator +(PlayerStatistics s1, PlayerStatistics s2)
        {
            PlayerStatistics s = new PlayerStatistics();
            s.CollectedFood = s1.CollectedFood + s2.CollectedFood;
            s.CollectedFruits = s1.CollectedFruits + s2.CollectedFruits;
            s.CurrentAntCount = s1.CurrentAntCount + s2.CurrentAntCount;
            s.StarvedAnts = s1.StarvedAnts + s2.StarvedAnts;
            s.EatenAnts = s1.EatenAnts + s2.EatenAnts;
            s.BeatenAnts = s1.BeatenAnts + s2.BeatenAnts;
            s.KilledAnts = s1.KilledAnts + s2.KilledAnts;
            s.KilledBugs = s1.KilledBugs + s2.KilledBugs;
            s.LoopCount = Math.Max(s1.LoopCount + s2.LoopCount, 2);
            return s;
        }
    }
}