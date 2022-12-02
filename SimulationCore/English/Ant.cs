using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents a foreign ant.
    /// </summary>
    public sealed class Ant : Insect
    {
        internal Ant(CoreAnt ant) : base(ant) { }

        /// <summary>
        /// Delivers the current load of this ant.
        /// </summary>
        public int CurrentLoad => ((CoreAnt)Baseitem).CurrentLoadCoreInsect;

        /// <summary>
        /// Delivers the current carried fruit.
        /// </summary>
        public Fruit CarriedFruit
        {
            get
            {
                CoreAnt temp = (CoreAnt)Baseitem;
                if (temp.CarryingFruitCoreInsect == null)
                {
                    return null;
                }
                else
                {
                    return new Fruit(temp.CarryingFruitCoreInsect);
                }
            }
        }

        /// <summary>
        /// Delivers the maximum load.
        /// </summary>
        public int MaximumLoad => ((CoreAnt)Baseitem).MaximumLoadCoreInsect;

        /// <summary>
        /// Delivers the range.
        /// </summary>
        public int Range => ((CoreAnt)Baseitem).RangeCoreInsect; 

        /// <summary>
        /// Delivers the colony name.
        /// </summary>
        public string Colony => ((CoreAnt)Baseitem).Colony.Player.ColonyName;
    }
}