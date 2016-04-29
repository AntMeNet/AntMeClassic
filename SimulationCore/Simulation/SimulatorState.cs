namespace AntMe.Simulation
{
    /// <summary>
    /// Liste der möglichen Stati eines Simulators
    /// </summary>
    public enum SimulatorState
    {
        /// <summary>
        /// Der Simulator ist initialisiert, aber noch nicht gestartet worden
        /// </summary>
        Ready,

        /// <summary>
        /// Der Simulator simuliert gerade
        /// </summary>
        Simulating,

        /// <summary>
        /// Die Simulation wurde beendet
        /// </summary>
        Finished
    }
}