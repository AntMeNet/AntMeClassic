using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// CoreFruit represents a fruit.
    /// </summary>
    internal sealed class CoreFruit : CoreFood
    {
        /// <summary>
        /// List of carrying ants.
        /// </summary>
        internal readonly List<CoreInsect> InsectsCarrying = new List<CoreInsect>();

        /// <summary>
        /// Creates a new instance of the fruit class.
        /// </summary>
        /// <param name="x">X-position of the fruit on the playground</param>
        /// <param name="y">Y-position of the fruit on the playground</param>
        /// <param name="amount">amount of nutrition points</param>
        internal CoreFruit(int x, int y, int amount)
            : base(x, y, amount) { }

        /// <summary>
        /// The remaining amount of nutrition points.
        /// </summary>
        public override int Amount
        {
            internal set
            {
                amount = value;
                coordinate.Radius = (int)(SimulationSettings.Custom.FruitRadiusMultiplier *
                                           Math.Sqrt(amount / Math.PI) * SimulationEnvironment.PLAYGROUND_UNIT);
            }
        }

        /// <summary>
        /// Determines if the piece of fruit for the specified colony still
        /// needs carrying insects to reach the maximum speed of carrying.
        /// </summary>
        /// <param name="colony">colony</param>
        internal bool NeedSupport(CoreColony colony)
        {
            int load = 0;
            foreach (CoreInsect insect in InsectsCarrying)
            {
                if (insect.Colony == colony)
                {
                    load += insect.CurrentLoadCoreInsect;
                }
            }
            return load * SimulationSettings.Custom.FruitLoadMultiplier < Amount;
        }

        /// <summary>
        /// Creates a fruit state object with the data from the state of the fruit.
        /// </summary>
        internal FruitState GenerateInformation()
        {
            FruitState info = new FruitState((ushort)Id);
            info.PositionX = (ushort)(coordinate.X / SimulationEnvironment.PLAYGROUND_UNIT);
            info.PositionY = (ushort)(coordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Radius = (ushort)(coordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Amount = (ushort)amount;
            info.CarryingAnts = (byte)InsectsCarrying.Count;
            return info;
        }
    }
}
