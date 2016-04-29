namespace AntMe.Deutsch {
    /// <summary>
    /// Repräsentiert einen Ameisenbau
    /// </summary>
    public sealed class Bau : Spielobjekt {
        internal Bau(Simulation.CoreAnthill bau) : base(bau) {}

        /// <summary>
        /// Liefert die ID dieses Baus
        /// </summary>
        public override int Id {
            get { return ((Simulation.CoreAnthill) element).Id; }
        }
    }
}