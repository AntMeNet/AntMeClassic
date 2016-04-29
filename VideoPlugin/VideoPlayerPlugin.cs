using AntMe.SharedComponents.AntVideo;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    public sealed class VideoPlayerPlugin : IProducerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private VideoPlayerControl control = new VideoPlayerControl();

        private PluginState state = PluginState.Ready;

        private AntVideoReader reader;

        public Guid Guid { get { return Guid.Parse("{F715DF3B-507E-49FB-9D9A-D303457A6491}"); } }

        public string Name { get { return "Video Player"; } }

        public string Description { get { return "Das ist der Video Player"; } }

        public Version Version { get { return version; } }

        public Control Control { get { return control; } }

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

        public void CreateState(ref SimulationState state)
        {
            if (reader != null)
                state = reader.Read();

            if (reader.Complete)
            {
                control.Invoke((MethodInvoker)(() =>
                {
                    Stop();
                }));
            }
        }

        public byte[] Settings
        {
            get
            {
                return null;
            }
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

        public void UpdateUI(SimulationState state)
        {
        }
    }
}
