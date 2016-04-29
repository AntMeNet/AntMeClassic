using System;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Hilfsklasse zur Generierung von Zufallszahlen
    /// </summary>
    public class Zufall
    {
        private readonly Random random;

        internal Zufall(Random random)
        {
            this.random = random;
        }

        ///// <summary>
        /// Generiert eine Zufallszahl zwischen 0 und dem angegebenen Maximum
        /// </summary>
        /// <param name="maximum">Maximum</param>
        /// <returns>Zufallszahl</returns>
        public int Zahl(int maximum)
        {
            return random.Next(maximum);
        }

        /// <summary>
        /// Generiert eine Zufallszahl zwischen dem angegebenen Minimum und dem angegebenen Maximum
        /// </summary>
        /// <param name="minimum">Minumum</param>
        /// <param name="maximum">Maximum</param>
        /// <returns>Zufallszahl</returns>
        public int Zahl(int minimum, int maximum)
        {
            if (maximum < minimum)
            {
                return random.Next(maximum, minimum);
            }
            return random.Next(minimum, maximum);
        }
    }
}