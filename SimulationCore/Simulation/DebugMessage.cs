using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Klasse zur Weiterleitung von Debug-Information mit Kontextinformationen
    /// </summary>
    [Serializable]
    public sealed class DebugMessage
    {
        #region interne Variablen

        private readonly int ameise;
        private readonly string nachricht;
        private readonly int spieler;
        private readonly DateTime zeit;

        #endregion

        #region Konstruktor und Initialisierung

        /// <summary>
        /// Konstruktor einer Debugnachricht
        /// </summary>
        /// <param name="spieler">ID des spielers</param>
        /// <param name="ameise">ID der Ameise</param>
        /// <param name="nachricht">Nachricht</param>
        public DebugMessage(int spieler, int ameise, string nachricht)
        {
            zeit = DateTime.Now;
            this.spieler = spieler;
            this.ameise = ameise;
            this.nachricht = nachricht;
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Zeitpunkt der Nachricht
        /// </summary>
        public DateTime Zeit
        {
            get { return zeit; }
        }

        /// <summary>
        /// Assoziierter Spieler
        /// </summary>
        public int Spieler
        {
            get { return spieler; }
        }

        /// <summary>
        /// ID der betroffenen Ameise
        /// </summary>
        public int Ameise
        {
            get { return ameise; }
        }

        /// <summary>
        /// Die Debugnachricht
        /// </summary>
        public string Nachricht
        {
            get { return nachricht; }
        }

        #endregion
    }
}