using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents an ant hill
    /// </summary>
    public sealed class Anthill : Item
    {
        internal Anthill(CoreAnthill anthill) : base(anthill) { }

        /// <summary>
        /// Delivers the unique ID of this ant hill
        /// </summary>
        public override int Id
        {
            get { return ((CoreAnthill)Baseitem).Id; }
        }
    }
}