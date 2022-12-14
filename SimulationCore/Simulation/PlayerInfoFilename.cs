using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// Player AI file with filename.
    /// </summary>
    [Serializable]
    public sealed class PlayerInfoFilename : PlayerInfo
    {
        #region internal attribute

        /// <summary>
        /// Player AI file with path.
        /// </summary>
        public string File;

        #endregion

        #region constructor and initialization

        /// <summary>
        /// Creates an instance of a player AI info file.
        /// </summary>
        public PlayerInfoFilename() { }

        /// <summary>
        /// Creates an instance of a player AI info file with given filename.
        /// </summary>
        /// <param name="file">Player AI file with path.</param>
        public PlayerInfoFilename(string file)
        {
            File = file;
        }

        /// <summary>
        /// Creates an instance of a player AI info file with given filename an AI info.
        /// </summary>
        /// <param name="info">Player AI info.</param>
        /// <param name="file">Player AI file with path.</param>
        public PlayerInfoFilename(PlayerInfo info, string file)
            : base(info)
        {
            File = file;
        }

        #endregion

        /// <summary>
        /// Determine if this player AI has the same hash code as the given AI.
        /// </summary>
        /// <param name="obj">Given player AI.</param>
        /// <returns>True for equal AI.</returns>
        public override bool Equals(object obj)
        {
            PlayerInfoFilename other = (PlayerInfoFilename)obj;
            return (GetHashCode() == other.GetHashCode());
        }

        /// <summary>
        /// Create a hash for file and class name.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return File.GetHashCode() ^ ClassName.GetHashCode();
        }
    }
}