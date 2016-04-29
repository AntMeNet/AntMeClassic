using System;
namespace AntMe.SharedComponents.States {
    /// <summary>
    /// List of possible targets for an ant.
    /// </summary>
    [Serializable]
    public enum TargetType {
        /// <summary>
        /// There is no target.
        /// </summary>
        None,

        /// <summary>
        /// Target is an ant.
        /// </summary>
        Ant,

        /// <summary>
        /// Target is an anthill.
        /// </summary>
        Anthill,

        /// <summary>
        /// Target are bugs.
        /// </summary>
        Bug,

        /// <summary>
        /// Target is fruit.
        /// </summary>
        Fruit,

        /// <summary>
        /// Target is a marker.
        /// </summary>
        Marker,

        /// <summary>
        /// Target is sugar.
        /// </summary>
        Sugar
    }
}