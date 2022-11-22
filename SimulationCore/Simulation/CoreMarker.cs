using AntMe.SharedComponents.States;
using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Eine Duft-Markierung die eine Information enth�lt.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreMarker : ICoordinate
    {
        // Die Id der n�chsten erzeugten Markierung.
        private static int neueId = 0;

        /// <summary>
        /// Die Id die die Markierung w�hrend eines Spiels eindeutig identifiziert.
        /// </summary>
        public readonly int Id;

        private readonly int colonyId;
        private CoreCoordinate koordinate;

        private int age = 0;
        private int totalAge;
        private int endgr��e;
        private int information;


        /// <summary>
        /// Erzeugt eine neue Instanz der Markierung-Klasse.
        /// </summary>
        /// <param name="koordinate">Die Koordinate der Markierung.</param>
        /// <param name="endgr��e">Die Ausbreitung der Markierung in Schritten.
        /// </param>
        /// <param name="colonyId">ID des Volkes</param>
        internal CoreMarker(CoreCoordinate koordinate, int endgr��e, int colonyId)
        {
            this.colonyId = colonyId;
            Id = neueId++;
            this.koordinate = koordinate;

            // Volumen der kleinsten Markierung (r� * PI/2) ermitteln (Halbkugel)
            double baseVolume = Math.Pow(SimulationSettings.Custom.MarkerSizeMinimum, 3) * (Math.PI / 2);

            // Korrektur f�r gr��ere Markierungen
            baseVolume *= 10f;

            // Gesamtvolumen �ber die volle Zeit ermitteln
            double totalvolume = baseVolume * SimulationSettings.Custom.MarkerMaximumAge;

            // Maximale Markergr��e ermitteln
            int maxSize = (int)Math.Pow(4 * ((totalvolume - baseVolume) / Math.PI), 1f / 3f);

            // Gew�nschte Zielgr��e limitieren (Min Markersize, Max MaxSize)
            this.endgr��e = Math.Max(SimulationSettings.Custom.MarkerSizeMinimum, Math.Min(maxSize, endgr��e));

            // Volumen f�r die gr��te Markierung ermitteln
            int diffRadius = this.endgr��e - SimulationSettings.Custom.MarkerSizeMinimum;
            double diffVolume = Math.Pow(diffRadius, 3) * (Math.PI / 4);

            // Lebenszeit bei angestrebter Gesamtgr��e ermitteln
            totalAge = (int)Math.Floor(totalvolume / (baseVolume + diffVolume));
            Aktualisieren();
        }

        /// <summary>
        /// Die gespeicherte Information.
        /// </summary>
        public int Information
        {
            get { return information; }
            internal set { information = value; }
        }

        /// <summary>
        /// Bestimmt ob die Markierung ihre maximales Alter noch nicht �berschritten
        /// hat.
        /// </summary>
        public bool IstAktiv
        {
            get { return age <= totalAge; }
        }

        #region IKoordinate Members

        /// <summary>
        /// Die Position der Markierung auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return koordinate; }
        }

        #endregion

        /// <summary>
        /// Erh�ht das Alter der Markierung und passt ihren Radius an.
        /// </summary>
        internal void Aktualisieren()
        {
            age++;
            if (IstAktiv)
            {
                koordinate.Radius = (int)(
                    SimulationSettings.Custom.MarkerSizeMinimum +
                    endgr��e * ((float)age / totalAge)) * SimulationEnvironment.PLAYGROUND_UNIT;
            }
        }

        /// <summary>
        /// Erzeugt ein MarkierungZustand-Objekt mit den aktuellen Daten der
        /// Markierung.
        /// </summary>
        internal MarkerState ErzeugeInfo()
        {
            MarkerState info = new MarkerState(colonyId, Id);
            info.PositionX = koordinate.X / SimulationEnvironment.PLAYGROUND_UNIT;
            info.PositionY = koordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            info.Radius = koordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT;
            info.Direction = koordinate.Direction;
            return info;
        }
    }
}