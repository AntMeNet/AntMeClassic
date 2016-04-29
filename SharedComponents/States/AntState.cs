using System;

namespace AntMe.SharedComponents.States
{
    /// <summary>
    /// Holds information about an ant.
    /// </summary>
    [Serializable]
    public class AntState : ColonyBasedState
    {

        #region Constructor

        /// <summary>
        /// Constructor of ant-state
        /// </summary>
        /// <param name="colonyId">Colony-id</param>
        /// <param name="id">id</param>
        public AntState(int colonyId, int id)
            : base(colonyId, id)
        {
            LoadType = LoadType.None;
            TargetType = TargetType.None;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the caste.
        /// </summary>
        public int CasteId { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// Gets or sets the load.
        /// </summary>
        public int Load { get; set; }

        /// <summary>
        /// Gets or sets the type of load.
        /// </summary>
        public LoadType LoadType { get; set; }

        /// <summary>
        /// Gets or sets the x-part of position.
        /// </summary>
        public int PositionX { get; set; }

        /// <summary>
        /// Gets or sets the kind of target.
        /// </summary>
        public TargetType TargetType { get; set; }

        /// <summary>
        /// Gets or sets the x-part of the target position.
        /// </summary>
        public int TargetPositionX { get; set; }

        /// <summary>
        /// Gets or sets the y-part of the target position.
        /// </summary>
        public int TargetPositionY { get; set; }

        /// <summary>
        /// Gets or sets the y-part of position.
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// Gets or sets the vitality.
        /// </summary>
        public int Vitality { get; set; }

        /// <summary>
        /// View Range
        /// </summary>
        public int ViewRange { get; set; }

        /// <summary>
        /// Debug Message
        /// </summary>
        public string DebugMessage { get; set; }

        #endregion
    }
}