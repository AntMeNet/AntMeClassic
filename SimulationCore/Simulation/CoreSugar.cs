using AntMe.SharedComponents.States;

namespace AntMe.Simulation
{
    /// <summary>
    /// Represents a sugar pile.
    /// </summary>
    internal sealed class CoreSugar : CoreFood
    {
        /// <summary>
        /// Creates an instance of CoreSugar.
        /// </summary>
        /// <param name="x">The x-position of sugar on playground.</param>
        /// <param name="y">The y-position of sugar on playground.</param>
        /// <param name="amount">The amount of food.</param>
        internal CoreSugar(int x, int y, int amount)
            : base(x, y, amount) { }

        /// <summary>
        /// Creates a sugar state of this sugar hill.
        /// </summary>
        /// <returns>current state of that sugar hill.</returns>
        internal SugarState CreateState()
        {
            SugarState state = new SugarState((ushort)Id);
            state.PositionX = (ushort)(coordinate.X / SimulationEnvironment.PLAYGROUND_UNIT);
            state.PositionY = (ushort)(coordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            state.Radius = (ushort)(coordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT);
            state.Amount = (ushort)amount;
            return state;
        }
    }
}