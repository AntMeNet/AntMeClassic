using System;

namespace AntMe.English
{
    /// <summary>
    /// Helper class to generate a random number.
    /// </summary>
    public class RandomNumber
    {
        private readonly Random random;

        internal RandomNumber(Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Gives a random number between 0 and the given maximum.
        /// </summary>
        /// <param name="maximum">Maximum.</param>
        /// <returns>Random number.</returns>
        public int Number(int maximum)
        {
            return random.Next(maximum);
        }

        /// <summary>
        /// Gives a random number between the given minimum and the given maximum.
        /// </summary>
        /// <param name="minimum">Minimum,</param>
        /// <param name="maximum">Maximum.</param>
        /// <returns>Random number.</returns>
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