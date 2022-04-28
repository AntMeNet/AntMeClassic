using System;
namespace AntMe.Simulation
{
    /// <summary>
    /// List of possible player-languages.
    /// </summary>
    [Flags]
    public enum PlayerLanguages
    {
        /// <summary>
        /// Unknown language
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// German
        /// </summary>
        Deutsch = 1,

        /// <summary>
        /// English
        /// </summary>
        English = 2
    }
}