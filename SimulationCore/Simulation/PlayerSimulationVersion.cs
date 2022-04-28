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
        /// Version 1.1 - Singleplayer-Variante
        /// </summary>
        Version_1_1 = 1,

        /// <summary>
        /// Version 1.5 - war nur als Beta verfügbar und stellt die erste Multiplayer-Variante dar
        /// </summary>
        Version_1_5 = 2,

        /// <summary>
        /// Version 1.6 - die Basis für die erste Online-Version
        /// </summary>
        Version_1_6 = 4,

        /// <summary>
        /// fx 4.0 Port of Version 1.6
        /// </summary>
        Version_1_7 = 8,

        /// <summary>
        /// Version 2.0 - Bisher nicht vorhanden
        /// </summary>
        Version_2_0 = 16
    }
}