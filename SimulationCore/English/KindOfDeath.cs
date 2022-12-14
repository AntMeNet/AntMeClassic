namespace AntMe.English
{
    /// <summary>
    /// List of possible kinds of death
    /// </summary>
    public enum KindOfDeath
    {
        /// <summary>
        /// The ant was running out of food.
        /// </summary>
        Starved = 1,

        /// <summary>
        /// The ant was eaten up by a bug.
        /// </summary>
        Eaten = 2,

        /// <summary>
        /// The ant was killed by a foreign ant.
        /// </summary>
        Beaten = 4
    }
}