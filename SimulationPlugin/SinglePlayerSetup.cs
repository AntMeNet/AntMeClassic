using AntMe.Simulation;
using System;
using System.Xml.Serialization;

namespace AntMe.Plugin.Simulation
{
    [Serializable]
    public sealed class SinglePlayerSetup
    {
        public string Filename { get; set; }

        public string Typename { get; set; }

        [XmlIgnore]
        public PlayerInfoFilename PlayerInfo { get; set; }
    }
}
