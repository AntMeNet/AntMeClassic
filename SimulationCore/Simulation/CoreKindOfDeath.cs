namespace AntMe.Simulation
{
    /// <summary>
    /// kind of death
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal enum CoreKindOfDeath
    {
        /// <summary>
        /// ant has starved
        /// </summary>
        Starved = 1,

        /// <summary>
        /// bug has eaten the ant 
        /// </summary>
        Eaten = 2,

        /// <summary>
        /// enemy ant has beaten the ant
        /// </summary>
        Beaten = 4
    }
}