using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Represents the Colony
    /// </summary>
    public sealed class Colony
    {
        private readonly CoreColony colony;

        internal Colony(CoreColony colony)
        {
            this.colony = colony;
        }

        /// <summary>
        /// Delivers the Name of that colony
        /// </summary>
        public string Name
        {
            get { return colony.Player.ColonyName; }
        }
    }
}