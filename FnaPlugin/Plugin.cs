using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;

namespace AntMe.Plugin.Fna
{
    [Preselected]
    public sealed class Plugin : IConsumerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private PluginState state = PluginState.Ready;

        private InstructionPanel control = new InstructionPanel();

        private RenderWindow window;

        public Guid Guid { get { return Guid.Parse("{12A1A289-C318-41B0-81F0-9CCB6ABB6654}"); } }

        public Version Version { get { return version; } }

        public string Name { get { return Strings.PluginName; } }

        public string Description { get { return Strings.PluginDescription; } }

        public void StartupParameter(string[] parameter) { }

        public void SetVisibility(bool visible) { }

        public bool Interrupt
        {
            // get { return (state != PluginState.Ready && window == null); }
            get { return false; }
        }

        public PluginState State { get { return state; } }

        public Control Control
        {
            get { return control; }
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

        private void Loop()
        {
            window = new RenderWindow();
            window.Run();
            window.Dispose();
            window = null;
            state = PluginState.Ready;
        }

        public void Start()
        {
            if (window == null)
            {
                Thread t = new Thread(Loop);
                t.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                t.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                t.IsBackground = true;
                t.Start();
            }
            state = PluginState.Running;
        }

        public void Stop()
        {
            if (window != null)
                window.Exit();
        }

        public void Pause()
        {
            state = PluginState.Paused;
        }


        public void CreateState(ref SimulationState state)
        {
        }

        public void CreatingState(ref SimulationState state)
        {
        }

        public void CreatedState(ref SimulationState state)
        {
            if (window != null)
                window.CurrentState = state;
        }

        public void UpdateUI(SimulationState state)
        {
        }
    }
}
