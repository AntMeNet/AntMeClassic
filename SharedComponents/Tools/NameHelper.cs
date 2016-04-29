using System;

using AntMe.SharedComponents.Properties;

namespace AntMe.SharedComponents.Tools {
    /// <summary>
    /// A static helper class that returns female and male first names.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public static class NameHelper {
        private static readonly int randomNumber;

        /// <summary>
        /// An array of female first names.
        /// </summary>
        private static readonly string[] FemaleNames;

        /// <summary>
        /// An array of male first names.
        /// </summary>
        private static readonly string[] MaleNames;

        // Static constructor.
        static NameHelper() {
            string[] separators = new string[] {"\n", "\r"};
            FemaleNames = Resources.FemaleNames.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            MaleNames = Resources.MaleNames.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            randomNumber = new Random().Next(1024);
        }

        /// <summary>
        /// Returns a female first name for a given hash value.
        /// </summary>
        /// <remarks>
        /// The same hash value will always return the same name within an application session.
        /// </remarks>
        /// <param name="hashValue">A value to use for hashing, for example an id.</param>
        /// <returns>A female first name.</returns>
        public static string GetFemaleName(int hashValue) {
            return FemaleNames[(hashValue*randomNumber)%FemaleNames.Length];
        }

        /// <summary>
        /// Returns a male first name for a given hash value.
        /// </summary>
        /// <remarks>
        /// The same hash value will always return the same name within an application session.
        /// </remarks>
        /// <param name="hashValue">A value to use for hashing, for example an id.</param>
        /// <returns>A male first name.</returns>
        public static string GetMaleName(int hashValue) {
            return MaleNames[(hashValue*randomNumber)%MaleNames.Length];
        }
    }
}