using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    /// <summary>
    /// Konfigurationsklasse für den Online-Client
    /// </summary>
    [Serializable]
    public sealed class Configuration
    {
        public Configuration()
        {
            ClientId = Guid.NewGuid();
            Roles = new List<string>();
            Reset();
        }

        /// <summary>
        /// Client ID, die pro Client Instanz im Idealfall nur ein mal erzeugt wird.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// User Id des aktuell angemeldeten Users (oder Empty).
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Die Email-Adresse / Username des aktuellen Users.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Liste der verfügbaren Rollen für den aktuellen User.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// API Zugriffstoken des aktuell angemeldeten Users.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Ablaufdatum des Access Tokens.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// Setzt die Konfiguratin auf den Stand eines unangemeldeten Users zurück.
        /// </summary>
        internal void Reset()
        {
            UserId = Guid.Empty;
            Email = string.Empty;
            Roles.Clear();
            AccessToken = string.Empty;
            Expires = DateTime.MinValue;
        }
    }
}
