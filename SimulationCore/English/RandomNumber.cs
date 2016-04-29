using System;

namespace AntMe.English
{
    /// <summary>
    /// Helper-class to generate some random numbers
    /// </summary>
    public class RandomNumber
    {
        private readonly Random random;

        internal RandomNumber(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Gives a random number between 0 and the given maximum
        /// </summary>
        /// <param name="maximum">maximum</param>
        /// <returns>random number</returns>
        public int Number(int maximum)
        {
            return random.Next(maximum);
        }

        /// <summary>
        /// Gives a random number between the given minimum and the given maximum
        /// </summary>
        /// <param name="minimum">minimum</param>
        /// <param name="maximum">maximum</param>
        /// <returns>random number</returns>
        public int Number(int minimum, int maximum)
        {
            if (minimum > maximum)
            {
                random.Next(maximum, minimum);
            }
            return random.Next(minimum, maximum);
        }
    }
}