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
    /// <summary>
    /// Plugin class for the video recorder plugin
    /// </summary>
    public sealed class VideoRecorderPlugin : IConsumerPlugin
    {
        private readonly VideoRecorderControl control = new VideoRecorderControl();

        private PluginState state = PluginState.Ready;

        private MemoryStream stream;
        private AntVideoWriter writer;
        private SimulationState lastFrame;

        public Guid Guid => Guid.Parse("{BC6AD88E-F6C6-440F-809A-5A49B66B329F}");

        public string Name => Resource.RecorderPluginName;

        public string Description => Resource.RecorderPluginDescription;

        public Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public Control Control => control;

        public PluginState State => state;

        public bool Interrupt => false;

        public void CreateState(ref SimulationState simulationState)
        {
        }

        public void CreatingState(ref SimulationState simulationState)
        {
        }

        public void CreatedState(ref SimulationState simulationState)
        {
            if (simulationState.CurrentRound == 1)
                CreateAntvideo();

            if (writer != null)
            {
                lastFrame = simulationState;
                int round = simulationState.CurrentRound;
                control.Invoke((MethodInvoker)(() =>
                {
                    control.RecordedFrame = round;
                }));

                writer.Write(simulationState);
            }
        }

        public void UpdateUI(SimulationState simulationState)
        {
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
                    var player = string.Join(", ", lastFrame.ColonyStates.Select(c => c.ColonyName + $" ({c.Points})"));
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
