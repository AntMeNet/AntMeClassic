using System;

namespace AntMe.SharedComponents.Plugin
{
    /// <summary>
    /// Attribute, to signal, that the marked plugin reads the custom field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ReadCustomStateAttribute : Attribute
    {
        /// <summary>
        /// The name of the custom field.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The full name of the used type.
        /// </summary>
        public string Type = string.Empty;

        /// <summary>
        /// Optional description of usage.
        /// </summary>
        public string Description = string.Empty;
    }
}