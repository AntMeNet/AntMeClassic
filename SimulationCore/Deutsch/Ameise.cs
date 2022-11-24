namespace AntMe.Deutsch
{
    /// <summary>
    /// Repräsentiert eine Ameise
    /// </summary>
    public sealed class Ameise : Insekt
    {
        internal Ameise(Simulation.CoreAnt ant) : base(ant) { }

        /// <summary>
        /// Liefert die aktuelle Last
        /// </summary>
        public int AktuelleLast
        {
            get { return ((Simulation.CoreAnt)element).CurrentLoadBase; }
        }

        /// <summary>
        /// Liefert einen Verweis auf das aktuell getragene Obst
        /// </summary>
        public Obst GetragenesObst
        {
            get
            {
                Simulation.CoreAnt temp = (Simulation.CoreAnt)element;
                if (temp.CarryingFruitBase == null)
                {
                    return null;
                }
                else
                {
                    return new Obst(temp.CarryingFruitBase);
                }
            }
        }

        /// <summary>
        /// Liefert die maximale Belastbarkeit der Ameise
        /// </summary>
        public int MaximaleLast
        {
            get { return ((Simulation.CoreAnt)element).MaximumLoadBase; }
        }

        /// <summary>
        /// Liefert die Reichweite der Ameise
        /// </summary>
        public int Reichweite
        {
            get { return ((Simulation.CoreAnt)element).RangeBase; }
        }

        /// <summary>
        /// Liefet den Volknamen dieser Ameise
        /// </summary>
        public string Volk
        {
            get { return ((Simulation.CoreAnt)element).colony.Player.ColonyName; }
        }
    }
}