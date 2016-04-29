using AntMe.SharedComponents.States;

namespace AntMe.Simulation {
    /// <summary>
    /// Eine Duft-Markierung die eine Information enthält.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreMarker : ICoordinate {
        // Die Id der nächsten erzeugten Markierung.
        private static int neueId = 0;

        /// <summary>
        /// Die Id die die Markierung während eines Spiels eindeutig identifiziert.
        /// </summary>
        public readonly int Id;

        private readonly int colonyId;

        private int age = 0;
        private int ausbreitung;
        private int information;
        private CoreCoordinate koordinate;
        private int maximalAlter;

        /// <summary>
        /// Erzeugt eine neue Instanz der Markierung-Klasse.
        /// </summary>
        /// <param name="koordinate">Die Koordinate der Markierung.</param>
        /// <param name="ausbreitung">Die Ausbreitung der Markierung in Schritten.
        /// </param>
        /// <param name="colonyId">ID des Volkes</param>
        internal CoreMarker(CoreCoordinate koordinate, int ausbreitung, int colonyId) {
            this.colonyId = colonyId;
            Id = neueId++;
            this.koordinate = koordinate;
            maximalAlter = SimulationSettings.Custom.MarkerMaximumAge;
            if (ausbreitung < 0) {
                ausbreitung = 0;
            }
            else {
                if (ausbreitung > SimulationSettings.Custom.MarkerRangeMaximum)
                {
                    ausbreitung = SimulationSettings.Custom.MarkerRangeMaximum;
                }
                maximalAlter =
                    maximalAlter * SimulationSettings.Custom.MarkerSizeMinimum /
                    (SimulationSettings.Custom.MarkerSizeMinimum + ausbreitung);
            }
            this.ausbreitung = ausbreitung*SimulationEnvironment.PLAYGROUND_UNIT;
            Aktualisieren();
        }

        /// <summary>
        /// Die gespeicherte Information.
        /// </summary>
        public int Information {
            get { return information; }
            internal set { information = value; }
        }

        /// <summary>
        /// Bestimmt ob die Markierung ihre maximales Alter noch nicht überschritten
        /// hat.
        /// </summary>
        public bool IstAktiv {
            get { return age < maximalAlter; }
        }

        #region IKoordinate Members

        /// <summary>
        /// Die Position der Markierung auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase {
            get { return koordinate; }
        }

        #endregion

        /// <summary>
        /// Erhöht das Alter der Markierung und passt ihren Radius an.
        /// </summary>
        internal void Aktualisieren() {
            age++;
            koordinate.Radius = SimulationSettings.Custom.MarkerSizeMinimum * SimulationEnvironment.PLAYGROUND_UNIT;
            koordinate.Radius += ausbreitung * age / maximalAlter;
        }

        /// <summary>
        /// Erzeugt ein MarkierungZustand-Objekt mit den aktuellen Daten der
        /// Markierung.
        /// </summary>
        internal MarkerState ErzeugeInfo() {
            MarkerState info = new MarkerState(colonyId, Id);
            info.PositionX = koordinate.X/SimulationEnvironment.PLAYGROUND_UNIT;
            info.PositionY = koordinate.Y/SimulationEnvironment.PLAYGROUND_UNIT;
            info.Radius = koordinate.Radius/SimulationEnvironment.PLAYGROUND_UNIT;
            info.Direction = koordinate.Richtung;
            return info;
        }
    }
}