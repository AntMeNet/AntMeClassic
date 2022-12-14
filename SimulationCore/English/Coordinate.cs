using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Helper-class to calculate with distances and angles
    /// </summary>
    public static class Coordinate
    {
        #region Distance
        /// <summary>
        /// Gives the distance between two given objects.
        /// </summary>
        /// <param name="a">Item 1</param>
        /// <param name="b">Item 2</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(Item a, Item b)
        {
            return CoreCoordinate.DetermineDistance(a.Baseitem, b.Baseitem);
        }
        
        /// <summary>
        /// Gives the distance between two given objects.
        /// </summary>
        /// <param name="a">Ant.</param>
        /// <param name="b">Item b.</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(CoreAnt a, Item b)
        {
            return CoreCoordinate.DetermineDistance(a, b.Baseitem);
        }

        /// <summary>
        /// Gives the distance between two given objects.
        /// </summary>
        /// <param name="a">Item a.</param>
        /// <param name="b">Ant.</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(Item a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDistance(a.Baseitem, b);
        }

        /// <summary>
        /// Gives the distance between two given objects.
        /// </summary>
        /// <param name="a">Ant a</param>
        /// <param name="b">Ant b</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(CoreAnt a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDistance(a, b);
        }

        #endregion
        #region Directions

        /// <summary>
        /// Gives the direction from one object to another
        /// </summary>
        /// <param name="a">Item a.</param>
        /// <param name="b">Item b.</param>
        /// <returns>Direction from a to b.</returns>
        public static int GetDirectionFromTo(Item a, Item b)
        {
            return CoreCoordinate.DetermineDirection(a.Baseitem, b.Baseitem);
        }

        /// <summary>
        /// Gives the direction from one object to another
        /// </summary>
        /// <param name="a">Ant a.</param>
        /// <param name="b">Item b.</param>
        /// <returns>Direction from a to b.</returns>
        public static int GetDirectionFromTo(CoreAnt a, Item b)
        {
            return CoreCoordinate.DetermineDirection(a, b.Baseitem);
        }

        /// <summary>
        /// Gives the direction from one object to another
        /// </summary>
        /// <param name="a">Item a.</param>
        /// <param name="b">Ant b.</param>
        /// <returns>Direction from a to b.</returns>
        public static int GetDirectionFromTo(Item a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDirection(a.Baseitem, b);
        }

        /// <summary>
        /// Gives the direction from one object to another
        /// </summary>
        /// <param name="a">Ant a.</param>
        /// <param name="b">Ant b.</param>
        /// <returns>Direction from a to b.</returns>
        public static int GetDirectionFromTo(CoreAnt a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDirection(a, b);
        }
        
        #endregion
    }
}