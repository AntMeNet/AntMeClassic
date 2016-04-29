using System;

using AntMe.SharedComponents.Plugin;

namespace AntMe.Gui {
    /// <summary>
    /// Holds information about custom states.
    /// </summary>
    internal sealed class CustomStateItem {
        public readonly string Name;
        public readonly string Type;
        public readonly string Description;

        /// <summary>
        /// Creates a new instance of custom state.
        /// </summary>
        /// <param name="name">Name of custom state</param>
        /// <param name="type">Type of custom state</param>
        /// <param name="description">Description of state-access</param>
        public CustomStateItem(string name, string type, string description) {
            Name = name;
            Type = type;
            Description = description;
        }
    }
}