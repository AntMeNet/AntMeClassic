using AntMe.PlayerManagement;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AntMe.Plugin.Simulation
{
    public sealed class FreeGamePlugin : IProducerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private FreeGameControl control;
        private FreeGameSetup setup;
        private Simulator sim;
        private bool paused;

        public FreeGamePlugin()
        {
            setup = new FreeGameSetup();
            control = new FreeGameControl();
            control.SetSetup(setup);
        }

        public Guid Guid { get { return Guid.Parse("{77257ACF-6E73-48DF-B3A4-B469E34BFC35}"); } }
        public string Name { get { return Resource.FreeGamePluginName; } }
        public Version Version { get { return version; } }
        public string Description { get { return Resource.FreeGamePluginDescription; } }
        public Control Control { get { return control; } }

        public PluginState State
        {
            get
            {
                if (sim == null)
                {
                    int count = 0;
                    if (setup.Slot1.PlayerInfo != null) count++;
                    if (setup.Slot2.PlayerInfo != null) count++;
                    if (setup.Slot3.PlayerInfo != null) count++;
                    if (setup.Slot4.PlayerInfo != null) count++;
                    if (setup.Slot5.PlayerInfo != null) count++;
                    if (setup.Slot6.PlayerInfo != null) count++;
                    if (setup.Slot7.PlayerInfo != null) count++;
                    if (setup.Slot8.PlayerInfo != null) count++;
                    control.Enabled = true;
                    return count > 0 ? PluginState.Ready : PluginState.NotReady;
                }
                else
                {
                    control.Enabled = false;
                    return paused ? PluginState.Paused : PluginState.Running;
                }
            }
        }

        public byte[] Settings
        {
            get
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FreeGameSetup));
                    serializer.Serialize(stream, setup);
                    return stream.ToArray();
                }
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    using (MemoryStream puffer = new MemoryStream(value))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(FreeGameSetup));
                        var temp = serializer.Deserialize(puffer) as FreeGameSetup;
                        if (temp != null)
                            setup = temp;
                    }

                    DiscoverPlayerInfo(setup.Slot1);
                    DiscoverPlayerInfo(setup.Slot2);
                    DiscoverPlayerInfo(setup.Slot3);
                    DiscoverPlayerInfo(setup.Slot4);
                    DiscoverPlayerInfo(setup.Slot5);
                    DiscoverPlayerInfo(setup.Slot6);
                    DiscoverPlayerInfo(setup.Slot7);
                    DiscoverPlayerInfo(setup.Slot8);

                    // In das Form bringen
                    control.SetSetup(setup);
                }
            }
        }

        private void DiscoverPlayerInfo(FreeGameSlot slot)
        {
            if (slot != null && 
                !string.IsNullOrEmpty(slot.Filename) && 
                !string.IsNullOrEmpty(slot.Typename))
            {
                try
                {
                    PlayerStore.Instance.RegisterFile(slot.Filename);
                    slot.PlayerInfo = PlayerStore.Instance.KnownPlayer.FirstOrDefault(p =>
                        p.File.ToLower().Equals(slot.Filename.ToLower()) && p.ClassName.Equals(slot.Typename));
                }
                catch (Exception) 
                {
                    // Kick slot, falls es probleme gab
                    slot.Filename = string.Empty;
                    slot.PlayerInfo = null;
                    slot.Typename = string.Empty;
                }
            }
        }

        public void Start()
        {
            if (State == PluginState.Ready)
            {
                // Create new simulator
                SimulatorConfiguration config = setup.SimulatorConfiguration.Clone() as SimulatorConfiguration;
                FreeGameSlot[] slots = new[] { 
                    setup.Slot1, setup.Slot2, 
                    setup.Slot3, setup.Slot4, 
                    setup.Slot5, setup.Slot6, 
                    setup.Slot7, setup.Slot8 };

                for (int i = 0; i < 8; i++)
                {
                    var hits = slots.Where(s => s.Team == i + 1 && s.PlayerInfo != null);
                    if (hits.Count() > 0)
                    {
                        var team = new TeamInfo() { Guid = Guid.NewGuid(), Name = "Team " + (i + 1) };
                        team.Player.AddRange(hits.Select(p => p.PlayerInfo));
                        config.Teams.Add(team);
                    }
                }

                sim = new Simulator(config);
                control.Enabled = false;
            }

            if (State == PluginState.Paused)
            {
                paused = false;
            }
        }

        public void Stop()
        {
            if (State == PluginState.Running || State == PluginState.Paused)
            {
                sim.Unload();
                sim = null;
                control.Enabled = true;
            }
        }

        public void Pause()
        {
            paused = true;
        }

        public void StartupParameter(string[] parameter)
        {
            foreach (string param in parameter)
            {
                if (param.ToUpper().StartsWith("/FILE"))
                {
                    control.DirectStart(param.Substring(6).Trim());
                }
            }
        }

        public void SetVisibility(bool visible) { }

        public void UpdateUI(SimulationState state) { }

        public void CreateState(ref SimulationState state)
        {
            if (sim == null)
            {
                throw new Exception(Resource.SimulatorPluginNotStarted);
            }

            sim.Step(ref state);

            if (sim.State == SimulatorState.Finished)
            {
                sim.Unload();
                sim = null;
                paused = false;
            }
        }
    }
}
