using System;

namespace AntMe.Plugin.Simulation
{
    /// <summary>
    /// Holds a single set of values for a simulation-round.
    /// </summary>
    [Serializable]
    public sealed class SummaryValueSet
    {
        /// <summary>
        /// Amount of collected food.
        /// </summary>
        public int collectedFood;

        /// <summary>
        /// Amount of colleted fruit.
        /// </summary>
        public int collectedFruit;

        /// <summary>
        /// Amount of killed foreign ants.
        /// </summary>
        public int killedAnts;

        /// <summary>
        /// Amount of killed bugs.
        /// </summary>
        public int killedBugs;

        /// <summary>
        /// Amount of starved ants.
        /// </summary>
        public int starvedAnts;

        /// <summary>
        /// Amount of beaten ants.
        /// </summary>
        public int beatenAnts;

        /// <summary>
        /// Amount of eaten ants.
        /// </summary>
        public int eatenAnts;

        /// <summary>
        /// Total count of points.
        /// </summary>
        public int totalPoints;

        /// <summary>
        /// Adds another valueSet to the current one.
        /// </summary>
        /// <param name="valueSet">valueSet to add to this set.</param>
        public void Add(SummaryValueSet valueSet)
        {
            collectedFood += valueSet.collectedFood;
            collectedFruit += valueSet.collectedFruit;
            killedAnts += valueSet.killedAnts;
            killedBugs += valueSet.killedBugs;
            starvedAnts += valueSet.starvedAnts;
            beatenAnts += valueSet.beatenAnts;
            eatenAnts += valueSet.eatenAnts;
            totalPoints += valueSet.totalPoints;
        }
    }
}
