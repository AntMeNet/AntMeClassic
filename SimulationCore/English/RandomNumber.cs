namespace AntMe.English {
    /// <summary>
    /// Helper-class to generate some random numbers
    /// </summary>
    public static class RandomNumber {
        private static readonly System.Random random = new System.Random();

        /// <summary>
        /// Gives a random number between 0 and the given maximum
        /// </summary>
        /// <param name="maximum">maximum</param>
        /// <returns>random number</returns>
        public static int Number(int maximum) {
            return random.Next(maximum);
        }

        /// <summary>
        /// Gives a random number between the given minimum and the given maximum
        /// </summary>
        /// <param name="minimum">minimum</param>
        /// <param name="maximum">maximum</param>
        /// <returns>random number</returns>
        public static int Number(int minimum, int maximum) {
            if (minimum > maximum) {
                random.Next(maximum, minimum);
            }
            return random.Next(minimum, maximum);
        }
    }
}