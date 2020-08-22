using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Attribut für die spielerrelevanten Angaben zum Volk
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SpielerAttribute : Attribute
    {

        private string nachname = string.Empty;
        private string volkname = string.Empty;
        private string vorname = string.Empty;

        /// <summary>
        /// Name des Volkes (Angabe erforderlich)
        /// </summary>
        public string Volkname
        {
            get { return volkname; }
            set { volkname = value; }
        }

        /// <summary>
        /// Nachname des Spielers
        /// </summary>
        public string Nachname
        {
            get { return nachname; }
            set { nachname = value; }
        }

        /// <summary>
        /// Vorname des Spielers
        /// </summary>
        public string Vorname
        {
            get { return vorname; }
            set { vorname = value; }
        }
    }
}