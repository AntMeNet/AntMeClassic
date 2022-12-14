namespace AntMe.Simulation
{
    /// <summary>
    /// Kind of death.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal enum CoreKindOfDeath
    {
        /// <summary>
        /// Ant has starved.
        /// </summary>
        Starved = 1,

        /// <summary>
        /// Bug has eaten the ant. 
        /// </summary>
        Eaten = 2,

        /// <summary>
        /// Enemy ant has beaten the ant.
        /// </summary>
        Beaten = 4
    }
}