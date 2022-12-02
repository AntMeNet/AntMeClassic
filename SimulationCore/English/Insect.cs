using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Its the interface for all lifeforms on playground.
    /// </summary>
    public abstract class Insect : Item
    {
        internal Insect(CoreInsect insect) : base(insect) { }

        /// <summary>
        /// Returns the unique ID of this insect.
        /// </summary>
        public override int Id
        {
            get { return ((CoreInsect)Baseitem).Id; }
        }

        /// <summary>
        /// Delivers the current energy of this ant.
        /// </summary>
        public int CurrentEnergy
        {
            get { return ((CoreInsect)Baseitem).currentEnergyCoreInsect; }
        }
    }
}