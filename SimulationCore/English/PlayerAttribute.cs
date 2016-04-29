using System;

namespace AntMe.English {
    /// <summary>
    /// Attribute to descripe an ant
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PlayerAttribute : Attribute {
        /// <summary>
        /// First name of the player
        /// </summary>
        public string FirstName = string.Empty;

        /// <summary>
        /// Name of the colony
        /// </summary>
        public string ColonyName = string.Empty;

        /// <summary>
        /// Last name of the player
        /// </summary>
        public string LastName = string.Empty;
    }
}