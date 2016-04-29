using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Spielerinfo mit zusätzlicher Angabe eines Dateinamens
    /// </summary>
    [Serializable]
    public sealed class PlayerInfoFilename : PlayerInfo
    {
        #region interne Variablen

        /// <summary>
        /// Pfad zur KI-Datei
        /// </summary>
        public string File;

        #endregion

        #region Initialisierung und Konstruktor

        /// <summary>
        /// Creates an instance of PlayerInfoFilename
        /// </summary>
        public PlayerInfoFilename() { }

        /// <summary>
        /// Konstruktor der SpielerInfo mit Dateinamen
        /// </summary>
        public PlayerInfoFilename(string file)
        {
            File = file;
        }

        /// <summary>
        /// Konstruktor der SpielerInfo mit Dateinamen
        /// </summary>
        /// <param name="info"></param>
        /// <param name="file"></param>
        public PlayerInfoFilename(PlayerInfo info, string file)
            : base(info)
        {
            File = file;
        }

        #endregion

        /// <summary>
        /// Ermittelt, ob die KIs gleich sind
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PlayerInfoFilename other = (PlayerInfoFilename)obj;
            return (GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        /// Erzeugt einen Hash aus den gegebenen Daten
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return File.GetHashCode() ^ ClassName.GetHashCode();
        }
    }
}