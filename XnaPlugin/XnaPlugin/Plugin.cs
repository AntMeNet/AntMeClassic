using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace AntMe.Plugin.Xna
{
    [Preselected]
    public sealed class Plugin : IConsumerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private PluginState state = PluginState.Ready;

        private RenderWindow window;

        public Guid Guid { get { return Guid.Parse("{AC254307-B465-493B-B99C-9E7BC8F19234}"); } }

        public Version Version { get { return version; } }

        public string Name { get { return Resource.PluginName; } }

        public string Description { get { return Resource.PluginDescription; } }

        public void StartupParameter(string[] parameter) { }

        public void SetVisibility(bool visible) { }

        public bool Interrupt
        {
            get { return (state != PluginState.Ready && window == null); }
        }

        public PluginState State { get { return state; } }

        public Control Control
        {
            get { return null; }
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
