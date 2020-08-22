namespace AntMe.Deutsch
{
    /// <summary>
    /// Repräsentiert eine Markierung
    /// </summary>
    public sealed class Markierung : Spielobjekt
    {
        internal Markierung(Simulation.CoreMarker markierung) : base(markierung) { }

        /// <summary>
        /// Liefert die eindeutige ID dieser Markeriung
        /// </summary>
        public override int Id
        {
            get { return ((Simulation.CoreMarker)element).Id; }
        }

        /// <summary>
        /// Liefert die Information aus der Markierung
        /// </summary>
        public int Information
        {
            get { return ((Simulation.CoreMarker)element).Information; }
        }
    }
}