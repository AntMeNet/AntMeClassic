using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Baseclass for all items on playground.
    /// </summary>
    public abstract class Item
    {
        /// <summary>
        /// Saves the reference to the original simulation object.
        /// </summary>
        private readonly ICoordinate item;

        internal Item(ICoordinate item)
        {
            this.item = item;
        }

        /// <summary>
        /// Delivers the base item.
        /// </summary>
        internal ICoordinate Baseitem
        {
            get { return item; }
        }

        /// <summary>
        /// Gives the unique ID of this item.
        /// </summary>
        public abstract int Id { get; }

        #region Comparation

        /// <summary>
        /// Delivers a unique code for that item
        /// </summary>
        /// <returns>unique Code</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Comparison between this item object and another.
        /// </summary>
        /// <param name="obj">Item object to compare with.</param>
        /// <returns>True if both refer to the same element. With check against null objects.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() == GetType())
            {
                return obj.GetHashCode() == GetHashCode();
            }
            return false;
        }

        /// <summary>
        /// operator ==
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>True if both refer to the same element. With check against null objects.</returns>
        public static bool operator ==(Item a, Item b)
        {
            // check, if both items are null
            if ((object)a == null)
            {
                return (object)b == null;
            }

            // check, if b is null
            if ((object)b == null)
            {
                return false;
            }

            // both are instances - check if equal
            return a.Equals(b);
        }

        /// <summary>
        /// operator !=
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>True if both refer to different elements. No check against null objects.</returns>
        public static bool operator !=(Item a, Item b)
        {
            return !(a == b);
        }

        #endregion
    }
}