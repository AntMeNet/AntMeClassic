using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    [Preselected]
    public sealed class StatisticPlugin : IConsumerPlugin
    {
        private readonly string name = Resource.StatisticPluginName;
        private readonly string description = Resource.StatisticPluginDescription;
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly Guid guid = new Guid("C83570BF-E5A7-492c-BEF2-9D25C005D6A9");

        public StatisticPlugin()
        {
            control = new StatisticControl();
            pluginState = PluginState.Ready;
        }

        #region IConsumerPlugin Member

        private PluginState pluginState;
        private StatisticControl control;

        public bool Interrupt { get { return false; } }
        public void CreateState(ref SimulationState state) { }
        public void CreatingState(ref SimulationState state) { }
        public void CreatedState(ref SimulationState state) { }

        #endregion

        #region IPlugin Member

        public string Description { get { return description; } }
        public Guid Guid { get { return guid; } }
        public string Name { get { return name; } }
        public Version Version { get { return version; } }
        public PluginState State { get { return pluginState; } }
        public Control Control { get { return control; } }

        public byte[] Settings
        {
            get { return new byte[0]; }
            set { }
        }

        public void Start()
        {
            control.Start();
            pluginState = PluginState.Running;
        }
        public void Stop()
        {
            control.Stop();
            pluginState = PluginState.Ready;
        }
        public void Pause() { pluginState = PluginState.Paused; }

        public void StartupParameter(string[] parameter) { }
        public void SetVisibility(bool visible)
        {

        }

        public void UpdateUI(SimulationState state)
        {
            control.SimulationState(state);
        }

        #endregion
    }
}