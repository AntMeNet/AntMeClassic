namespace AntMe.Simulation
{
    /// <summary>
    /// Allows to query the coordinate of an object on the field.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public interface ICoordinate
    {
        /// <summary>
        /// Gets the coordinates of an object on the playground.
        /// </summary>
        CoreCoordinate CoordinateBase { get; }
    }
}
