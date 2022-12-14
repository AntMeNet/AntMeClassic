using System;

namespace AntMe.English
{
    /// <summary>
    /// Attribute to describe an ant.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PlayerAttribute : Attribute
    {

        private string colonyName = string.Empty;
        private string lastName = string.Empty;
        private string firstName = string.Empty;

        /// <summary>
        /// Colony name (obligatory).
        /// </summary>
        public string ColonyName
        {
            get { return colonyName; }
            set { colonyName = value; }
        }

        /// <summary>
        /// Last name (nice to have).
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        /// <summary>
        /// First name of the player (nice to have).
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
    }
}