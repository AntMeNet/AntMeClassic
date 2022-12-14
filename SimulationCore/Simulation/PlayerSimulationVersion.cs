using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// List of possible versions of simulation.
    /// </summary>
    [Flags]
    public enum PlayerSimulationVersions
    {

        /// <summary>
        /// Version 1.1 - singleplayer version
        /// </summary>
        Version_1_1 = 1,

        /// <summary>
        /// Version 1.5 - first multiplayer version, beta only
        /// </summary>
        Version_1_5 = 2,

        /// <summary>
        /// Version 1.6 - base for firs online version
        /// </summary>
        Version_1_6 = 4,

        /// <summary>
        /// fx 4.0 Port of version 1.6
        /// </summary>
        Version_1_7 = 8,

        /// <summary>
        /// Version 2.0 - not available right now
        /// </summary>
        Version_2_0 = 16
    }
}