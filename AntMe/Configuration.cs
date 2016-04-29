using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AntMe.Gui
{
    /// <summary>
    /// Holds all configuration-data for antme.
    /// </summary>
    [Serializable]
    public sealed class Configuration
    {
        /// <summary>
        /// List of known files, that containing plugins.
        /// </summary>
        public List<string> knownPluginFiles = new List<string>();

        /// <summary>
        /// Lists all active plugins.
        /// </summary>
        public List<Guid> selectedPlugins = new List<Guid>();

        /// <summary>
        /// Gives the limit for frame-rate.
        /// </summary>
        public float speedLimit;

        /// <summary>
        /// Gives the state of frame-rate limiter.
        /// </summary>
        public bool speedLimitEnabled;

        /// <summary>
        /// Indicated, if the configuration was loaded.
        /// </summary>
        [XmlIgnore]
        public bool loaded;
    }
}