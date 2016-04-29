using System;
using AntMe.Simulation;

namespace AntMe.Plugin.Simulation {
    /// <summary>
    /// Represents an instance of a player in a team.
    /// </summary>
    [Serializable]
    public sealed class PlayerItem : ICloneable {

        /// <summary>
        /// Creates an empty PlayerItem.
        /// </summary>
        public PlayerItem() {
        }

        /// <summary>
        /// Creates a new instance of PlayerItem.
        /// </summary>
        /// <param name="info">base <see cref="PlayerInfoFilename"/></param>
        public PlayerItem(PlayerInfoFilename info) {
            PlayerInfo = info;
            ColonyName = info.ColonyName;
            FileName = info.File;
            ClassName = info.ClassName;
            AuthorName = string.Format(Resource.AntPropertiesAuthorFormat, info.FirstName, info.LastName);
        }

        /// <summary>
        /// Unique ID of this <see cref="PlayerItem"/>
        /// </summary>
        public Guid Guid = System.Guid.NewGuid();

        /// <summary>
        /// Represents the Playerinfo
        /// </summary>
        public PlayerInfoFilename PlayerInfo;

        /// <summary>
        /// Name of the colony.
        /// </summary>
        public string ColonyName;

        /// <summary>
        /// Name of the file.
        /// </summary>
        public string FileName;

        /// <summary>
        /// Name of the class.
        /// </summary>
        public string ClassName;

        /// <summary>
        /// Name of Colony-Author.
        /// </summary>
        public string AuthorName;

        #region ICloneable Member

        /// <summary>
        /// Clones the current <see cref="PlayerItem"/>.
        /// </summary>
        /// <returns>copy of item</returns>
        public object Clone()
        {
            PlayerItem item = (PlayerItem) MemberwiseClone();
            item.Guid = Guid.NewGuid();
            return item;
        }

        #endregion
    }
}