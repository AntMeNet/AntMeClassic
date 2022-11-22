using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents a foreign ant
    /// </summary>
    public sealed class Ant : Insect
    {
        internal Ant(CoreAnt ant) : base(ant) { }

        /// <summary>
        /// Delivers the current load of this ant.
        /// </summary>
        public int CurrentLoad
        {
            get { return ((CoreAnt)Baseitem).CurrentBurdenBase; }
        }

        /// <summary>
        /// delivers the current carried fruit.
        /// </summary>
        public Fruit CarriedFruit
        {
            get
            {
                CoreAnt temp = (CoreAnt)Baseitem;
                if (temp.CarryingFruitBase == null)
                {
                    return null;
                }
                else
                {
                    return new Fruit(temp.CarryingFruitBase);
                }
            }
        }

        /// <summary>
        /// Delivers the maximum load.
        /// </summary>
        public int MaximumLoad
        {
            get { return ((CoreAnt)Baseitem).MaximumBurdenBase; }
        }

        /// <summary>
        /// Delivers the range.
        /// </summary>
        public int Range
        {
            get { return ((CoreAnt)Baseitem).RangeBase; }
        }

        /// <summary>
        /// Delivers the Colony-Name.
        /// </summary>
        public string Colony
        {
            get { return ((CoreAnt)Baseitem).colony.Player.ColonyName; }
        }
    }
}