namespace AntMe.SharedComponents.AntVideo {
    /// <summary>
    /// Interface for all updateable item-states
    /// </summary>
    internal interface IUpdateable<T, V> {
        /// <summary>
        /// Calculates the next state based on the last update-information
        /// </summary>
        void Interpolate();

        /// <summary>
        /// Delivers a new update-information
        /// </summary>
        /// <param name="update">new Update</param>
        void Update(T update);

        /// <summary>
        /// Generates a new update-information based on given state
        /// </summary>
        /// <returns>update-information</returns>
        T GenerateUpdate(V state);

        /// <summary>
        /// Generates a state of current states
        /// </summary>
        /// <returns>state</returns>
        V GenerateState();

        /// <summary>
        /// Flag to mark activity
        /// </summary>
        bool IsAlive { get; set; }
    }
}