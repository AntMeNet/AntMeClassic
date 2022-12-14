using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// class to transfer the simulation state from the host back to the simulator
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    [Serializable]
    internal sealed class SimulatorHostState
    {
        /// <summary>
        /// List of debug messages
        /// </summary>
        public List<DebugMessage> DebugMessages = new List<DebugMessage>();

        /// <summary>
        /// total time elapsed in the round
        /// </summary>
        public long ElapsedRoundTime;

        /// <summary>
        /// total time elapsed per player
        /// </summary>
        public Dictionary<Guid, long> ElapsedPlayerTimes = new Dictionary<Guid, long>();
    }
}