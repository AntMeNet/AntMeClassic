using AntMe.SharedComponents.States;

namespace AntMe.Simulation {
    /// <summary>
    /// Ein Bau eines Ameisenvolkes.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreAnthill : ICoordinate {
        // Die Id des nächsten erzeugten Bau.
        private static int neueId = 0;

        /// <summary>
        /// Die Id die den Bau während eines Spiels eindeutig identifiziert.
        /// </summary>
        public readonly int Id;

        private readonly int colonyId;

        private CoreCoordinate koordinate;

        /// <summary>
        /// Erzeugt eine neue Instanz der Bau-Klasse.
        /// <param name="x">X-Koordinate</param>
        /// <param name="y">Y.Koordinate</param>
        /// <param name="radius">Radius</param>
        /// <param name="colonyId">Volk ID</param>
        /// </summary>
        internal CoreAnthill(int x, int y, int radius, int colonyId) {
            this.colonyId = colonyId;
            Id = neueId++;
            koordinate = new CoreCoordinate(x, y, radius);
        }

        #region IKoordinate Members

        /// <summary>
        /// Die Position des Bau auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase {
            get { return koordinate; }
            internal set { koordinate = value; }
        }

        #endregion

        /// <summary>
        /// Erzeugt ein BauZustand-Objekt mit den aktuellen Daten des Bau.
        /// </summary>
        internal AnthillState ErzeugeInfo() {
            AnthillState zustand = new AnthillState(colonyId, Id);
            zustand.PositionX = koordinate.X/SimulationEnvironment.PLAYGROUND_UNIT;
            zustand.PositionY = koordinate.Y/SimulationEnvironment.PLAYGROUND_UNIT;
            zustand.Radius = koordinate.Radius/SimulationEnvironment.PLAYGROUND_UNIT;
            return zustand;
        }
    }
}