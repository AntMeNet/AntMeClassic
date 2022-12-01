using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Die abstrakte Basisklasse f�r Nahrung.
    /// </summary>
    internal abstract class CoreFood : ICoordinate
    {
        // Die Id der n�chsten erzeugten Nahrung.
        private static int neueId = 0;

        /// <summary>
        /// Die Id die die Nahrung w�hrend eines Spiels eindeutig identifiziert.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Die Position der Nahrung auf dem Spielfeld.
        /// </summary>
        protected CoreCoordinate koordinate;

        /// <summary>
        /// Die verbleibende Menge an Nahrungspunkten.
        /// </summary>
        protected int amount;

        /// <summary>
        /// Der abstrakte Nahrung-Basiskonstruktor.
        /// </summary>
        /// <param name="x">Die X-Position der Koordinate auf dem Spielfeld.</param>
        /// <param name="y">Die Y-Position der Koordinate auf dem Spielfeld.</param>
        /// <param name="amount">Die Anzahl der Nahrungspunkte.</param>
        internal CoreFood(int x, int y, int amount)
        {
            Id = neueId++;
            koordinate = new CoreCoordinate(x, y);
            Amount = amount;
        }

        /// <summary>
        /// Die verbleibende Menge an Nahrungspunkten.
        /// </summary>
        public virtual int Amount
        {
            get { return amount; }
            internal set
            {
                amount = value;
                koordinate.Radius = (int)
                                    (Math.Round(Math.Sqrt(amount / Math.PI) * SimulationEnvironment.PLAYGROUND_UNIT));
            }
        }

        #region IKoordinate Members

        /// <summary>
        /// Die Position der Nahrung auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return koordinate; }
            internal set { koordinate = value; }
        }

        #endregion
    }
}