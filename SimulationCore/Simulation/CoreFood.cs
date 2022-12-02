using System;

namespace AntMe.Simulation
{
    /// <summary>
    /// The abstract base class for food.
    /// </summary>
    internal abstract class CoreFood : ICoordinate
    {
        // The ID of the next generated food.
        private static int newId = 0;

        /// <summary>
        /// The ID that uniquely identifies the food during a match.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// The position of the food on the playground.
        /// </summary>
        protected CoreCoordinate coordinate;

        /// <summary>
        /// The remaining amount of food points.
        /// </summary>
        protected int amount;

        /// <summary>
        /// The abstract food base constructor.
        /// </summary>
        /// <param name="x">X-coordinate on the playground.</param>
        /// <param name="y">Y-coordinate on the playground.</param>
        /// <param name="amount">amount of food points.</param>
        internal CoreFood(int x, int y, int amount)
        {
            Id = newId++;
            coordinate = new CoreCoordinate(x, y);
            Amount = amount;
        }

        /// <summary>
        /// The remaining amount of food points.
        /// </summary>
        public virtual int Amount
        {
            get { return amount; }
            internal set
            {
                amount = value;
                coordinate.Radius = (int)
                                    (Math.Round(Math.Sqrt(amount / Math.PI) * SimulationEnvironment.PLAYGROUND_UNIT));
            }
        }

        #region ICoordinate members

        /// <summary>
        /// The position of the food on the playground.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return coordinate; }
            internal set { coordinate = value; }
        }

        #endregion
    }
}