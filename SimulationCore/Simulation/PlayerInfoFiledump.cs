using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// PlayerInfo class with the specification of an additional dump of a player AI.
    /// </summary>
    [Serializable]
    public sealed class PlayerInfoFiledump : PlayerInfo
    {
        #region internal attributes

        /// <summary>
        /// Copy of the AI assembly.
        /// </summary>
        public byte[] File;

        #endregion

        #region Initialization and constructor

        /// <summary>
        /// Creates an instance of player info file dump.
        /// </summary>
        public PlayerInfoFiledump() { }

        /// <summary>
        /// Constructor of the PlayerInfo with file copy.
        /// <param name="file">Copy of the file dump one byte[].</param>
        /// </summary>
        public PlayerInfoFiledump(byte[] file)
        {
            File = file;
        }

        /// <summary>
        /// Constructor of the PlayerInfo with file copy.
        /// </summary>
        /// <param name="info">Base player info.</param>
        /// <param name="file">Copy of the file dump one byte[].</param>
        public PlayerInfoFiledump(PlayerInfo info, byte[] file)
            : base(info)
        {
            File = file;
        }

        #endregion
    }
}