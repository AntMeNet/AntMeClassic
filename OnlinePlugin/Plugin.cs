using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AntMe.Plugin.Online
{
    public sealed class Plugin : IProducerPlugin
    {
        private MainControl control;

        public Plugin()
        {
            control = new MainControl();
        }

        #region Meta Infos

        public Guid Guid { get { return Guid.Parse("{F3BB2A56-DAD4-45E0-8448-E56F6E4D803F}"); } }

        public string Name { get { return "AntMe! Online"; } }

        public Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

        public string Description { get { return "Kleine AntMe! Online Erweiterung"; } }

        public Control Control { get { return control; } }

        #endregion

        public void CreateState(ref SimulationState state)
        {
            throw new NotImplementedException();
        }

        public void UpdateUI(SimulationState state)
        {
            throw new NotImplementedException();
        }

        public PluginState State
        {
            get { return PluginState.NotReady; }
        }


        public byte[] Settings
        {
            get
            {
                return new byte[0];
            }
            set
            {
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }

        public void StartupParameter(string[] parameter) { }

        public void SetVisibility(bool visible) { }
    }
}
