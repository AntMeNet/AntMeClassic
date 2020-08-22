using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Attribut, um Zugriffsrechte anzufordern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class ZugriffAttribute : Attribute
    {
        /// <summary>
        /// Beschreibung zur Verwendung der erteilten Zugriffsrechte.
        /// </summary>
        public string Beschreibung;
    }
}