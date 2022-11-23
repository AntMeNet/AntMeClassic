namespace AntMe.Simulation
{
    /// <summary>
    /// all possibilities of states the simulation can be
    /// </summary>
    public enum SimulatorState
    {
        /// <summary>
        /// ready, initialized, but not started yet
        /// </summary>
        Ready,

        /// <summary>
        /// running simulation
        /// </summary>
        Simulating,

        /// <summary>
        /// finished simulation
        /// </summary>
        Finished
    }
}