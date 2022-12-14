using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Its the interface for all kind of food
    /// </summary>
    public abstract class Food : Item
    {
        internal Food(CoreFood food) : base(food) { }

        /// <summary>
        /// Returns the amount of food
        /// </summary>
        public int Amount
        {
            get { return ((CoreFood)Baseitem).Amount; }
        }

        /// <summary>
        /// Delivers the unique Id of this food
        /// </summary>
        public override int Id
        {
            get { return ((CoreFood)Baseitem).Id; }
        }
    }
}