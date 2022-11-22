namespace AntMe.Deutsch
{
    /// <summary>
    /// Basisklasse für alle Nahrungsmittel auf dem Spielfeld
    /// </summary>
    public abstract class Nahrung : Spielobjekt
    {
        internal Nahrung(Simulation.CoreFood nahrung) : base(nahrung) { }

        /// <summary>
        /// Liefert die Menge an Nahrung
        /// </summary>
        public int Menge
        {
            get { return ((Simulation.CoreFood)element).Amount; }
        }

        /// <summary>
        /// Liefer die ID dieser Nahrung
        /// </summary>
        public override int Id
        {
            get { return ((Simulation.CoreFood)element).Id; }
        }
    }
}