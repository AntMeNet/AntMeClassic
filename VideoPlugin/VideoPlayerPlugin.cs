using AntMe.SharedComponents.AntVideo;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    /// <summary>
    /// Plugin class of the video plugin
    /// </summary>
    public sealed class VideoPlayerPlugin : IProducerPlugin
    {
        private readonly VideoPlayerControl control = new VideoPlayerControl();

        private PluginState state = PluginState.Ready;

        private AntVideoReader reader;

        public Guid Guid { get; } = Guid.Parse("{F715DF3B-507E-49FB-9D9A-D303457A6491}");

        public string Name => Resource.PlayerPluginName;

        public string Description => Resource.PlayerPluginDescription;

        public Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public Control Control => control;

        public PluginState State
        {
            get
            {
                if (state == PluginState.Ready)
                    return control.Stream != null ?
                        PluginState.Ready : PluginState.NotReady;
                return state;
            }
        }

        public void CreateState(ref SimulationState simulationState)
        {
            if (reader != null)
            {
                simulationState = reader.Read();

                if (reader.Complete)
                {
                    control.Invoke((MethodInvoker) Stop);
                }
            }
        }

        public byte[] Settings
        {
            get => null;
            set
            {
            }
        }

        public void Start()
        {
            if (State == PluginState.Paused)
            {
                state = PluginState.Running;
            }
            else if (State == PluginState.Ready)
            {
                control.Enabled = false;
                state = PluginState.Running;
                control.Stream.Seek(0, SeekOrigin.Begin);
                reader = new AntVideoReader(control.Stream);
            }
        }

        public void Stop()
        {
            if (State == PluginState.Running)
            {
                reader = null;
                state = PluginState.Ready;
                control.Enabled = true;
            }
        }

        public void Pause()
        {
            if (State == PluginState.Running)
                state = PluginState.Paused;
        }

        public void StartupParameter(string[] parameter)
        {
        }

        public void SetVisibility(bool visible)
        {
        }

        public void UpdateUI(SimulationState simulationState)
        {
        }
    }
}
