using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AntMe.PlayerManagement
{
    /// <summary>
    /// PlayerManagement Configuration
    /// </summary>
    [Serializable]
    public sealed class PlayerStoreConfiguration
    {
        public PlayerStoreConfiguration()
        {
            KnownFiles = new List<string>();
        }

        /// <summary>
        /// List of all known Player
        /// </summary>
        public List<string> KnownFiles { get; set; }
    }
}
