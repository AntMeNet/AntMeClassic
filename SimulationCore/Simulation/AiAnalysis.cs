using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace AntMe.Simulation
{
    /// <summary>
    /// Class extracting player information from given AI assembly.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    public static class AiAnalysis
    {
        #region public, static Methods

        /// <summary>
        /// Analyse AI file for given assembly filename
        /// </summary>
        /// <param name="filename">AI file to analyse</param>
        /// <returns>list of <see cref="PlayerInfo"/> from the given assembly file</returns>
        /// <throws><see cref="ArgumentException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="PathTooLongException"/></throws>
        /// <throws><see cref="DirectoryNotFoundException"/></throws>
        /// <throws><see cref="IOException"/></throws>
        /// <throws><see cref="UnauthorizedAccessException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="NotSupportedException"/></throws>
        /// <throws><see cref="SecurityException"/></throws>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        public static List<PlayerInfo> Analyse(string filename)
        {
            return Analyse(filename, true);
        }

        /// <summary>
        /// Analyse AI file for given assembly filename.
        /// </summary>
        /// <param name="filename">AI file to analyse</param>
        /// <param name="checkRules">true to check player rules</param>
        /// <returns>list of <see cref="PlayerInfo"/> from assembly file</returns>
        /// <throws><see cref="ArgumentException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="PathTooLongException"/></throws>
        /// <throws><see cref="DirectoryNotFoundException"/></throws>
        /// <throws><see cref="IOException"/></throws>
        /// <throws><see cref="UnauthorizedAccessException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="NotSupportedException"/></throws>
        /// <throws><see cref="SecurityException"/></throws>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        public static List<PlayerInfo> Analyse(string filename, bool checkRules)
        {
            return Analyse(File.ReadAllBytes(filename), checkRules);
        }

        /// <summary>
        /// Analyse AI file based on binary file dump.
        /// </summary>
        /// <param name="file">AI file to analyse</param>
        /// <returns>list of <see cref="PlayerInfo"/> from binary file</returns>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        public static List<PlayerInfo> Analyse(byte[] file)
        {
            return Analyse(file, true);
        }

        /// <summary>
        /// Analyzes a AI binary file
        /// </summary>
        /// <param name="file">AI file to analyze.</param>
        /// <param name="checkRules">true for check player rules</param>
        /// <returns>list of <see cref="PlayerInfo"/> from binary file</returns>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        public static List<PlayerInfo> Analyse(byte[] file, bool checkRules)
        {
            PlayerAnalysis analysisHost = new PlayerAnalysis();
            // analyses and return list of <see cref="PlayerInfo"/>
            return analysisHost.Analyse(file, checkRules);
        }

        /// <summary>
        /// Find a specific Player-information in given Ai-File.
        /// </summary>
        /// <param name="file">File as binary file dump</param>
        /// <param name="className">className of AI</param>
        /// <returns>the right <see cref="PlayerInfo"/></returns>
        /// <throws><see cref="NotSupportedException"/></throws>
        public static PlayerInfoFiledump FindPlayerInformation(byte[] file, string className)
        {
            // load all included players
            List<PlayerInfo> foundPlayers = Analyse(file);

            // If there is no classname, just take the only one
            if (className == string.Empty)
            {
                if (foundPlayers.Count == 1)
                {
                    return new PlayerInfoFiledump(foundPlayers[0], file);
                }
                throw new InvalidOperationException(Resource.SimulationCoreAnalysisNoClassFound);
            }

            // search for needed classname
            foreach (PlayerInfo player in foundPlayers)
            {
                if (player.ClassName == className)
                {
                    return new PlayerInfoFiledump(player, file);
                }
            }

            // Exception, if there was no hit
            throw new InvalidOperationException(Resource.SimulationCoreAnalysisNoClassFound);
        }

        /// <summary>
        /// Find a specific <see cref="PlayerInfo"/> in given Ai-File.
        /// </summary>
        /// <param name="file">File as filename</param>
        /// <param name="className">Class-name of Ai</param>
        /// <returns>the right <see cref="PlayerInfo"/> or null for no hits</returns>
        public static PlayerInfoFilename FindPlayerInformation(string file, string className)
        {
            // load all included players
            List<PlayerInfo> foundPlayer = Analyse(file);

            // If there is no classname, just take the only one
            if (className == string.Empty)
            {
                if (foundPlayer.Count == 1)
                {
                    return new PlayerInfoFilename(foundPlayer[0], file);
                }
                throw new InvalidOperationException(Resource.SimulationCoreAnalysisNoClassFound);
            }

            // search for needed classname
            foreach (PlayerInfo player in foundPlayer)
            {
                if (player.ClassName == className)
                {
                    return new PlayerInfoFilename(player, file);
                }
            }

            // Exception, if there was no hit
            throw new InvalidOperationException(Resource.SimulationCoreAnalysisNoClassFound);
        }

        #endregion
    }
}