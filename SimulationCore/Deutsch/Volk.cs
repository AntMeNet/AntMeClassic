namespace AntMe.Deutsch {
    /// <summary>
    /// Repräsentant eines Volkes
    /// </summary>
    public sealed class Volk {
        private readonly Simulation.CoreColony volk;

        internal Volk(Simulation.CoreColony volk) {
            this.volk = volk;
        }

        /// <summary>
        /// Liefert den Namen des Volkes
        /// </summary>
        public string Name {
            get { return volk.Player.ColonyName; }
        }
    }
}