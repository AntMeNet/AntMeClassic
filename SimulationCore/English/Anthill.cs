using AntMe.Simulation;

namespace AntMe.English {
    /// <summary>
    /// Represents an anthill
    /// </summary>
    public sealed class Anthill : Item {
        internal Anthill(CoreAnthill anthill) : base(anthill) {}

        /// <summary>
        /// Delivers the unique ID of this anthill
        /// </summary>
        public override int Id {
            get { return ((CoreAnthill) Baseitem).Id; }
        }
    }
}