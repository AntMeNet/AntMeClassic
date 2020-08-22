using AntMe.SharedComponents.AntVideo;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    public sealed class VideoRecorderPlugin : IConsumerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private VideoRecorderControl control = new VideoRecorderControl();

        private PluginState state = PluginState.Ready;

        private MemoryStream stream;
        private AntVideoWriter writer;
        private SimulationState lastFrame;

        public Guid Guid { get { return Guid.Parse("{BC6AD88E-F6C6-440F-809A-5A49B66B329F}"); } }

        public string Name { get { return "Video Recorder"; } }

        public string Description { get { return "Das ist der Video Recorder"; } }

        public Version Version { get { return version; } }

        public Control Control { get { return control; } }

        public PluginState State { get { return state; } }

        public bool Interrupt { get { return false; } }

        public void CreateState(ref SimulationState state)
        {
        }

        public void CreatingState(ref SimulationState state)
        {
        }

        public void CreatedState(ref SimulationState state)
        {
            if (state.CurrentRound == 1)
                CreateAntvideo();

            if (writer != null)
            {
                lastFrame = state;
                int round = state.CurrentRound;
                control.Invoke((MethodInvoker)(() =>
                {
                    control.RecordedFrame = round;
                }));

                writer.Write(state);
            }
        }

        public void UpdateUI(SimulationState state)
        {
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
            state = PluginState.Running;
        }

        public void Stop()
        {
            FinalizeAntVideo();
            state = PluginState.Ready;
        }

        public void Pause()
        {
            state = PluginState.Paused;
        }

        public void StartupParameter(string[] parameter)
        {
        }

        public void SetVisibility(bool visible)
        {
        }

        private void CreateAntvideo()
        {
            if (writer != null)
                FinalizeAntVideo();

            stream = new MemoryStream();
            writer = new AntVideoWriter(stream);
            control.Invoke((MethodInvoker)(() =>
            {
                control.Recording = true;
                control.RecordedFrame = 0;
            }));
        }

        private void FinalizeAntVideo()
        {
            if (writer != null)
            {
                writer.Close();
                writer = null;
                if (lastFrame != null)
                {
                    string player = string.Join(", ", lastFrame.ColonyStates.Select(c => c.ColonyName + " (" + c.Points.ToString() + ")"));
                    control.Invoke((MethodInvoker)(() =>
                    {
                        control.Add(stream, player);
                    }));
                }
                stream = null;

                control.Invoke((MethodInvoker)(() =>
                {
                    control.Recording = false;
                    control.RecordedFrame = 0;
                }));
            }
        }
    }
}
