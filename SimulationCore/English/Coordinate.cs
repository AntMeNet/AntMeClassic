using AntMe.Simulation;

namespace AntMe.English
{
    /// <summary>
    /// Helper-class to calculate with distances and angles
    /// </summary>
    public static class Coordinate
    {
        /// <summary>
        /// Gives the distance between the to given objects
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(Item a, Item b)
        {
            return CoreCoordinate.BestimmeEntfernung(a.Baseitem, b.Baseitem);
        }

        /// <summary>
        /// Gives the distance between the to given objects
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(CoreAnt a, Item b)
        {
            return CoreCoordinate.BestimmeEntfernung(a, b.Baseitem);
        }

        /// <summary>
        /// Gives the distance between the to given objects
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(Item a, CoreAnt b)
        {
            return CoreCoordinate.BestimmeEntfernung(a.Baseitem, b);
        }

        /// <summary>
        /// Gives the distance between the to given objects
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>Distance between</returns>
        public static int GetDistanceBetween(CoreAnt a, CoreAnt b)
        {
            return CoreCoordinate.BestimmeEntfernung(a, b);
        }

        /// <summary>
        /// Gives the direction from object 1 to object 2
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>direction</returns>
        public static int GetDegreesBetween(Item a, Item b)
        {
            return CoreCoordinate.DetermineDirection(a.Baseitem, b.Baseitem);
        }

        /// <summary>
        /// Gives the direction from object 1 to object 2
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>direction</returns>
        public static int GetDegreesBetween(CoreAnt a, Item b)
        {
            return CoreCoordinate.DetermineDirection(a, b.Baseitem);
        }

        /// <summary>
        /// Gives the direction from object 1 to object 2
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>direction</returns>
        public static int GetDegreesBetween(Item a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDirection(a.Baseitem, b);
        }

        /// <summary>
        /// Gives the direction from object 1 to object 2
        /// </summary>
        /// <param name="a">object 1</param>
        /// <param name="b">object 2</param>
        /// <returns>direction</returns>
        public static int GetDegreesBetween(CoreAnt a, CoreAnt b)
        {
            return CoreCoordinate.DetermineDirection(a, b);
        }
    }
}