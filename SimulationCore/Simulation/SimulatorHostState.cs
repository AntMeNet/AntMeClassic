using System;
using System.Collections.Generic;

namespace AntMe.Simulation {
    /// <summary>
    /// Statusklasse zur Weitergabe des aktuellen Spielstandes aus dem Host zurück zum Simulator
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    [Serializable]
    internal sealed class SimulatorHostState {
        public List<DebugMessage> DebugMessages = new List<DebugMessage>();
        public long ElapsedRoundTime;
        //public Dictionary<PlayerInfo, long> ElapsedPlayerTimes = new Dictionary<PlayerInfo, long>();
    }
}