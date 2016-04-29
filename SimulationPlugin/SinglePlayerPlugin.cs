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
    public sealed class SinglePlayerPlugin : IProducerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private SinglePlayerControl control;
        private SinglePlayerSetup setup;
        private int seed;
        private Simulator sim;
        private bool paused;

        public Version Version { get { return version; } }

        public PluginState State
        {
            get
            {
                if (sim == null)
                {
                    if (control.InvokeRequired)
                    {
                        control.BeginInvoke((MethodInvoker)delegate
                        {
                            control.Enabled = true;
                        });
                    }
                    else
                    {
                        control.Enabled = true;
                    }
                    return setup.PlayerInfo != null ? PluginState.Ready : PluginState.NotReady;
                }
                else
                {
                    if (control.InvokeRequired)
                    {
                        control.BeginInvoke((MethodInvoker)delegate
                        {
                            control.Enabled = false;
                        });
                    }
                    else
                    {
                        control.Enabled = false;
                    }
                    return paused ? PluginState.Paused : PluginState.Running;
                }
            }
        }

        public Control Control { get { return control; } }

        public string Description { get { return "Der AntMe! Single Player Modus. Perfekt um Ameisen zu trainieren und zu spezialisieren"; } }

        public Guid Guid { get { return Guid.Parse("{4A9EF502-64BB-4EF4-B0FE-EB649599B560}"); } }

        public string Name { get { return "Single Player"; } }

        public SinglePlayerPlugin()
        {
            setup = new SinglePlayerSetup();
            control = new SinglePlayerControl();
            control.SetSetup(setup);
        }

        public void CreateState(ref SimulationState state)
        {
            if (sim == null)
            {
                throw new Exception(Resource.SimulatorPluginNotStarted);
            }

            sim.Step(ref state);

            if (sim.State == SimulatorState.Finished)
            {
                var colony = state.ColonyStates.First();
                control.ShowSummary(setup.PlayerInfo, seed, colony.Points, colony.CollectedFood, colony.CollectedFruits,
                    colony.EatenAnts, colony.BeatenAnts, colony.StarvedAnts, colony.KilledEnemies, colony.KilledBugs);

                sim.Unload();
                sim = null;
                paused = false;
            }
        }

        public byte[] Settings
        {
            get
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SinglePlayerSetup));
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
                        XmlSerializer serializer = new XmlSerializer(typeof(SinglePlayerSetup));
                        var temp = serializer.Deserialize(puffer) as SinglePlayerSetup;
                        if (temp != null)
                            setup = temp;
                    }

                    DiscoverPlayerInfo();

                    // In das Form bringen
                    control.SetSetup(setup);
                }
            }
        }

        private void DiscoverPlayerInfo()
        {
            if (setup != null &&
                !string.IsNullOrEmpty(setup.Filename) &&
                !string.IsNullOrEmpty(setup.Typename))
            {
                try
                {
                    PlayerStore.Instance.RegisterFile(setup.Filename);
                    setup.PlayerInfo = PlayerStore.Instance.KnownPlayer.FirstOrDefault(p =>
                        p.File.ToLower().Equals(setup.Filename.ToLower()) && p.ClassName.Equals(setup.Typename));
                }
                catch (Exception)
                {
                    // Kick slot, falls es probleme gab
                    setup.Filename = string.Empty;
                    setup.PlayerInfo = null;
                    setup.Typename = string.Empty;
                }
            }
        }

        public void Start()
        {
            if (State == PluginState.Ready)
            {
                // Create new simulator
                Random rand = new Random();
                SimulatorConfiguration config = new SimulatorConfiguration()
                {
                    MapInitialValue = seed = rand.Next(0, int.MaxValue),
                };
                config.Teams.Add(new TeamInfo(Guid.NewGuid(), new List<PlayerInfo> { setup.PlayerInfo }));

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
    }
}
