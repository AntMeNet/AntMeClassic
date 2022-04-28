using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AntMe.Plugin.Simulation
{
    [Serializable]
    public sealed class FreeGameSetup
    {
        public SimulatorConfiguration SimulatorConfiguration { get; set; }
        public FreeGameSlot Slot1 { get; set; }
        public FreeGameSlot Slot2 { get; set; }
        public FreeGameSlot Slot3 { get; set; }
        public FreeGameSlot Slot4 { get; set; }
        public FreeGameSlot Slot5 { get; set; }
        public FreeGameSlot Slot6 { get; set; }
        public FreeGameSlot Slot7 { get; set; }
        public FreeGameSlot Slot8 { get; set; }

        public List<string> KnownSettingFiles { get; set; }

        public FreeGameSetup()
        {
            Slot1 = new FreeGameSlot();
            Slot2 = new FreeGameSlot();
            Slot3 = new FreeGameSlot();
            Slot4 = new FreeGameSlot();
            Slot5 = new FreeGameSlot();
            Slot6 = new FreeGameSlot();
            Slot7 = new FreeGameSlot();
            Slot8 = new FreeGameSlot();
            SimulatorConfiguration = new SimulatorConfiguration();
            KnownSettingFiles = new List<string>();
            Reset();
        }

        public void Reset()
        {
            Slot1.Filename = string.Empty;
            Slot1.Typename = string.Empty;
            Slot1.PlayerInfo = null;
            Slot1.Team = 1;

            Slot2.Filename = string.Empty;
            Slot2.Typename = string.Empty;
            Slot2.PlayerInfo = null;
            Slot2.Team = 2;

            Slot3.Filename = string.Empty;
            Slot3.Typename = string.Empty;
            Slot3.PlayerInfo = null;
            Slot3.Team = 3;

            Slot4.Filename = string.Empty;
            Slot4.Typename = string.Empty;
            Slot4.PlayerInfo = null;
            Slot4.Team = 4;

            Slot5.Filename = string.Empty;
            Slot5.Typename = string.Empty;
            Slot5.PlayerInfo = null;
            Slot5.Team = 5;

            Slot6.Filename = string.Empty;
            Slot6.Typename = string.Empty;
            Slot6.PlayerInfo = null;
            Slot6.Team = 6;

            Slot7.Filename = string.Empty;
            Slot7.Typename = string.Empty;
            Slot7.PlayerInfo = null;
            Slot7.Team = 7;

            Slot8.Filename = string.Empty;
            Slot8.Typename = string.Empty;
            Slot8.PlayerInfo = null;
            Slot8.Team = 8;
        }
    }

    [Serializable]
    public sealed class FreeGameSlot
    {
        public string Filename { get; set; }

        public string Typename { get; set; }

        [XmlIgnore]
        public PlayerInfoFilename PlayerInfo { get; set; }

        public int Team { get; set; }
    }
}
