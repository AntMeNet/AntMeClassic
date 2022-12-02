using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// AntMe! consumer plugin that displays a game in a GDI+ based 2D view.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public class Plugin : IConsumerPlugin
    {
        private readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;

        private Window window;

        /// <summary>
        /// Creates a new instance of the plugin class.
        /// </summary>
        public Plugin()
        {
            window = new Window();
        }

        #region IPlugin

        private PluginState pluginStatus = PluginState.Ready;

        /// <summary>
        /// Function call to start the plugin operation.
        /// </summary>
        public void Start()
        {
            window.Start();
            pluginStatus = PluginState.Running;
        }

        /// <summary>
        /// Function call to stop the plugin operation.
        /// </summary>
        public void Stop()
        {
            window.Stop();
            pluginStatus = PluginState.Ready;
        }

        /// <summary>
        /// Pauses the operation of the plugin.
        /// </summary>
        public void Pause()
        {
            pluginStatus = PluginState.Paused;
        }

        /// <summary>
        /// Returns the name of the plugin.
        /// </summary>
        public string Name
        {
            get { return "2D visualization"; }
        }

        /// <summary>
        /// Returns a description text of this plugin.
        /// </summary>
        public string Description
        {
            get { return "Show the simulation in a 2D world."; }
        }

        /// <summary>
        /// Returns the version number of this plugin.
        /// </summary>
        public Version Version => version;

        /// <summary>
        /// Returns the GUID of this plugin.
        /// </summary>
        public Guid Guid
        {
            get { return new Guid("BBBD7C7A-FD3A-4656-B6DC-6A88463B2815"); }
        }

        /// <summary>
        /// Returns the current state of the plugin.
        /// </summary>
        public PluginState State
        {
            get { return pluginStatus; }
        }

        /// <summary>
        /// Returns a reference to a UserControl that is displayed in the main window. 
        /// </summary>
        public Control Control
        {
            get { return null; }
        }

        /// <summary>
        /// Returns or sets a byte array of serialized configuration data from this plugin.
        /// </summary>
        public byte[] Settings
        {
            get
            {
                return new byte[2]
                    {
                        window.UseAntiAliasing ? (byte)1 : (byte)0,
                        window.ShowScore ? (byte)1 : (byte)0
                    };
            }
            set
            {
                if (value.Length == 2)
                {
                    window.UseAntiAliasing = value[0] == 0 ? false : true;
                    window.ShowScore = value[1] == 0 ? false : true;
                }
            }
        }

        public void StartupParameter(string[] parameter)
        {
        }

        public void SetVisibility(bool visible)
        {
        }

        public void UpdateUI(SimulationState state)
        {
            window.Update(state);
        }

        #endregion

        #region IConsumerPlugin

        public bool Interrupt
        {
            get
            {
                // If the game is running or paused (and not just ready)
                // and the window is not visible, then it has been closed
                // and the simulation can be stopped.
                return (pluginStatus != PluginState.Ready && !window.Visible);
            }
        }

        public void CreateState(ref SimulationState state)
        {
        }

        public void CreatingState(ref SimulationState state)
        {
        }

        public void CreatedState(ref SimulationState state)
        {
        }

        #endregion

    }

}