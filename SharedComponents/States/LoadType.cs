using System;
namespace AntMe.SharedComponents.States {
    /// <summary>
    /// List of possible loads.
    /// </summary>
    [Serializable]
    public enum LoadType {
        /// <summary>
        /// No load
        /// </summary>
        None = 0,

        /// <summary>
        /// Sugar
        /// </summary>
        Sugar = 1,

        /// <summary>
        /// Fruit
        /// </summary>
        Fruit = 2
    }
}