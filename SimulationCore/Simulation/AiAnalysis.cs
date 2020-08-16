using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace AntMe.Simulation
{
    /// <summary>
    /// Class, to extract PlayerInfos from given Ai-Assembly.
    /// </summary>
    /// <author>Tom Wendel (tom@antme.net)</author>
    public static class AiAnalysis
    {
        #region public, static Methods

        /// <summary>
        /// Analyzes a Ai-File based on filename.
        /// </summary>
        /// <param name="filename">Ai-File to analyze.</param>
        /// <returns>List of found PlayerInfos</returns>
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
        /// Analyzes a Ai-File based on filename.
        /// </summary>
        /// <param name="filename">Ai-File to analyze.</param>
        /// <param name="checkRules">True, if Analyser should also check player-rules.</param>
        /// <returns>List of found PlayerInfos</returns>
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
        /// Analyzes a Ai-File based on binary file-dump.
        /// </summary>
        /// <param name="file">Ai-File to analyze.</param>
        /// <returns>List of found PlayerInfos</returns>
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
        /// Analyzes a Ai-File based on binary file-dump.
        /// </summary>
        /// <param name="file">Ai-File to analyze.</param>
        /// <param name="checkRules">True, if Analyser should also check player-rules.</param>
        /// <returns>List of found PlayerInfos</returns>
        /// <throws><see cref="FileLoadException"/></throws>
        /// <throws><see cref="BadImageFormatException"/></throws>
        /// <throws><see cref="ArgumentNullException"/></throws>
        /// <throws><see cref="FileNotFoundException"/></throws>
        /// <throws><see cref="RuleViolationException"/></throws>
        public static List<PlayerInfo> Analyse(byte[] file, bool checkRules)
        {
            PlayerAnalysis analysisHost = new PlayerAnalysis();
            // analies and return list of PlayerInfo
            return analysisHost.Analyse(file, checkRules);

        }

        /// <summary>
        /// Find a specific Player-information in given Ai-File.
        /// </summary>
        /// <param name="file">File as binary file-dump</param>
        /// <param name="className">Class-name of Ai</param>
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