using AntMe.SharedComponents.States;

namespace AntMe.Simulation
{
    /// <summary>
    /// An ant hill.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreAnthill : ICoordinate
    {
        // The ID of the next generated ant hill.
        private static int neueId = 0;

        /// <summary>
        /// The ID that uniquely identifies the ant hill during a match.
        /// </summary>
        public readonly int Id;

        private readonly int colonyId;

        private CoreCoordinate coordinate;

        /// <summary>
        /// Creates a new instance of the ant hill class.
        /// <param name="x">X-coordinate</param>
        /// <param name="y">Y-coordinate</param>
        /// <param name="radius">Radius</param>
        /// <param name="colonyId">Colony ID</param>
        /// </summary>
        internal CoreAnthill(int x, int y, int radius, int colonyId)
        {
            this.colonyId = colonyId;
            Id = neueId++;
            coordinate = new CoreCoordinate(x, y, radius);
        }

        #region IKoordinate Members

        /// <summary>
        /// The position of the ant hill on the playground.
        /// </summary>
        public CoreCoordinate CoordinateCoreInsect
        {
            get { return coordinate; }
            internal set { coordinate = value; }
        }

        #endregion

        /// <summary>
        /// Creates an ant hill state object with the current its data.
        /// </summary>
        internal AnthillState GenerateAnthillStateInfo()
        {
            AnthillState state = new AnthillState(colonyId, Id);
            state.PositionX = coordinate.X / SimulationEnvironment.PLAYGROUND_UNIT;
            state.PositionY = coordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            state.Radius = coordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT;
            return state;
        }
    }
}