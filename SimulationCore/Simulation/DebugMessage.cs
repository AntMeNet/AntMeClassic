using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Klasse zur Weiterleitung von Debug-Information mit Kontextinformationen
    /// </summary>
    [Serializable]
    public sealed class DebugMessage
    {
        #region internal variables

        private readonly int ant;
        private readonly string message;
        private readonly int player;
        private readonly DateTime time;

        #endregion

        #region Construktor and Initialization

        /// <summary>
        /// Konstruktor einer Debugnachricht
        /// </summary>
        /// <param name="player">ID des spielers</param>
        /// <param name="ant">ID der Ameise</param>
        /// <param name="message">Nachricht</param>
        public DebugMessage(int player, int ant, string message)
        {
            time = DateTime.Now;
            this.player = player;
            this.ant = ant;
            this.message = message;
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Zeitpunkt der Nachricht
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// Assoziierter Spieler
        /// </summary>
        public int Player
        {
            get { return player; }
        }

        /// <summary>
        /// ID der betroffenen Ameise
        /// </summary>
        public int Ant
        {
            get { return ant; }
        }

        /// <summary>
        /// Die Debugnachricht
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        #endregion
    }
}