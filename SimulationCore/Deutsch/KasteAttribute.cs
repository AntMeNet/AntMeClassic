using System;

namespace AntMe.Deutsch {
    /// <summary>
    /// Attribut für die Beschreibung von verschiedenen Ameisenkasten
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class KasteAttribute : Attribute {
        /// <summary>
        /// Legt die Angriffsstärke der Ameise fest
        /// </summary>
        public int AngriffModifikator = 0;

        /// <summary>
        /// Legt die Drehgeschwindigkeit der Ameise fest
        /// </summary>
        public int DrehgeschwindigkeitModifikator = 0;

        /// <summary>
        /// Legt die Energie einer Ameise fest
        /// </summary>
        public int EnergieModifikator = 0;

        /// <summary>
        /// Legt die Bewegungsgeschwindigkeit einer Ameise fest
        /// </summary>
        public int GeschwindigkeitModifikator = 0;

        /// <summary>
        /// Legt die Belastbarkeit einer Ameise fest
        /// </summary>
        public int LastModifikator = 0;

        /// <summary>
        /// Legt den Namen der Ameisenkaste fest
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Legt die Reichweite einer Ameise fest
        /// </summary>
        public int ReichweiteModifikator = 0;

        /// <summary>
        /// Legt die Sichtweite einer Ameise fest
        /// </summary>
        public int SichtweiteModifikator = 0;
    }
}