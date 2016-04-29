using System;

namespace AntMe.Simulation {
    /// <summary>
    /// SpielerInfo Klasse mit der angabe eines zusätzlichen Dumps einer Spieler-KI
    /// </summary>
    [Serializable]
    public sealed class PlayerInfoFiledump : PlayerInfo {
        #region interne Variablen

        /// <summary>
        /// Kopie der KI-Assembly
        /// </summary>
        public byte[] File;

        #endregion

        #region Initialisierung und Konstruktor

        /// <summary>
        /// Creates an instance of PlayerInfoFiledump
        /// </summary>
        public PlayerInfoFiledump() {}

        /// <summary>
        /// Konstruktor der SpielerInfo mit Dateikopie
        /// <param name="file">Kopie der Datei in Form eines Byte[]</param>
        /// </summary>
        public PlayerInfoFiledump(byte[] file) {
            File = file;
        }

        /// <summary>
        /// Konstruktor der SpielerInfo mit Dateikopie
        /// </summary>
        /// <param name="info">Basis SpielerInfo</param>
        /// <param name="file">Kopie der Datei in Form eines Byte[]</param>
        public PlayerInfoFiledump(PlayerInfo info, byte[] file)
            : base(info) {
            File = file;
        }

        #endregion
    }
}