using System;
using System.Windows.Forms;

using AntMe.SharedComponents.States;

namespace AntMe.SharedComponents.Plugin {
    /// <summary>
    /// Base-Interface for all AntMe-Plugins.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    public interface IPlugin {
        /// <summary>
        /// Plugin-Description. Only called by UI-Thread.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Plugin-Guid. Only called by UI-Thread.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Plugin-Name. Only called by UI-Thread.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin-Version. Only called by UI-Thread.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets the current plugin-state.  Called by UI- and GameLoop-Thread.
        /// </summary>
        PluginState State { get; }

        /// <summary>
        /// Gets the plugin-user-control so show in main application or null, if there is no user-control. Only called by UI-Thread.
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Gets or sets the settings for this plugin. usually a serialized configuration-class. Only called by UI-Thread.
        /// </summary>
        byte[] Settings { get; set; }

        /// <summary>
        /// Starts the plugin-activity or resumes activity, if paused. Only called by UI-Thread.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the plugin-activity. Only called by UI-Thread.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the activity or starts and pauses, if stopped. Only called by UI-Thread.
        /// </summary>
        void Pause();

        /// <summary>
        /// Delivers the start-parameter from main application to this plugin. Only called by UI-Thread.
        /// </summary>
        /// <param name="parameter">start-parameter</param>
        void StartupParameter(string[] parameter);

        /// <summary>
        /// Sets the state of visibility of plugins user-control. Only called by UI-Thread.
        /// </summary>
        /// <param name="visible">is user-control visible in main window</param>
        void SetVisibility(bool visible);

        /// <summary>
        /// Updates UI. Only called by UI-Thread.
        /// </summary>
        void UpdateUI(SimulationState state);
    }
}