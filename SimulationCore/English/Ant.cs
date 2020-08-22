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
            get { return ((CoreAnt)Baseitem).AktuelleLastBase; }
        }

        /// <summary>
        /// delivers the current carried fruit.
        /// </summary>
        public Fruit CarriedFruit
        {
            get
            {
                CoreAnt temp = (CoreAnt)Baseitem;
                if (temp.GetragenesObstBase == null)
                {
                    return null;
                }
                else
                {
                    return new Fruit(temp.GetragenesObstBase);
                }
            }
        }

        /// <summary>
        /// Delivers the maximum load.
        /// </summary>
        public int MaximumLoad
        {
            get { return ((CoreAnt)Baseitem).MaximaleLastBase; }
        }

        /// <summary>
        /// Delivers the range.
        /// </summary>
        public int Range
        {
            get { return ((CoreAnt)Baseitem).ReichweiteBase; }
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