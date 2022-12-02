using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Class for forwarding debug information with context information
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
        /// Constructor of a debug message.
        /// </summary>
        /// <param name="player">ID of player.</param>
        /// <param name="ant">ID of ant.</param>
        /// <param name="message">Message.</param>
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
        /// Message time.
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }

        /// <summary>
        /// Associated player.
        /// </summary>
        public int Player
        {
            get { return player; }
        }

        /// <summary>
        /// ID of the affected ant.
        /// </summary>
        public int Ant
        {
            get { return ant; }
        }

        /// <summary>
        /// The debug message.
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        #endregion
    }
}