namespace AntMe.Simulation
{
    /// <summary>
    /// Beschreibt wie eine Ameise gestorben ist.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal enum CoreKindOfDeath
    {
        /// <summary>
        /// Gibt an, dass die Ameise verhungert ist.
        /// </summary>
        Starved = 1,

        /// <summary>
        /// Gibt an, dass die Ameise von einer Wanze gefressen wurde.
        /// </summary>
        Eaten = 2,

        /// <summary>
        /// Gibt an, dass die Ameise von einer feindlichen Ameise besiegt wurde.
        /// </summary>
        Beaten = 4
    }
}