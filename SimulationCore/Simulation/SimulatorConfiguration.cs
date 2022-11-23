using System;
using System.Collections.Generic;
using System.Configuration;

namespace AntMe.Simulation
{
    /// <summary>
    /// simulator configuration class
    /// holds all relevant configuration data of the simulation
    /// </summary>
    [Serializable]
    public sealed class SimulatorConfiguration : ICloneable
    {

        /// <summary>
        /// Maximum count of players per simulation.
        /// </summary>
        public const int PLAYERLIMIT = 8;

        /// <summary>
        /// Minimum count of rounds for a valid simulation.
        /// </summary>
        public const int ROUNDSMIN = 1;

        /// <summary>
        /// Maximum count of rounds for a valid simulation.
        /// </summary>
        public const int ROUNDSMAX = Int16.MaxValue;

        /// <summary>
        /// Minimum count of loops for a valid simulation.
        /// </summary>
        public const int LOOPSMIN = 1;
        /// <summary>
        /// 
        /// Maximum count of loops for a valid simulation.
        /// </summary>
        public const int LOOPSMAX = Int16.MaxValue;

        /// <summary>
        /// Minimum value for round-timeouts.
        /// </summary>
        public const int ROUNDTIMEOUTMIN = 1;

        /// <summary>
        /// Minimum value for loop-timeouts.
        /// </summary>
        public const int LOOPTIMEOUTMIN = 1;

        #region private attributes

        // loop, round and player configuration
        private int loopCount;
        private int roundCount;
        private bool allowDebuginformation;
        private int loopTimeout;

        // attributes to allow access to database, filesystem, reference, user interface and network
        private bool allowDatabaseAccess;
        private bool allowFileAccess;
        private bool allowReferences;
        private bool allowUserinterfaceAccess;
        private bool allowNetworkAccess;

        /// <summary>
        /// timeouts can be ignored
        /// </summary>
        private bool ignoreTimeouts;
        private int roundTimeout;

        private List<TeamInfo> teams;

        // other settings
        private int mapInitialValue;
        private SimulationSettings settings;

        #endregion

        #region constructors and initialization

        /// <summary>
        /// initalization with an empty list of players
        /// </summary>
        public SimulatorConfiguration()
        {
            roundCount = 5000;
            loopCount = 1;
            teams = new List<TeamInfo>();

            ignoreTimeouts = true;
            roundTimeout = 1000;
            loopTimeout = 6000;

            allowDatabaseAccess = false;
            allowReferences = false;
            allowUserinterfaceAccess = false;
            allowFileAccess = false;
            allowNetworkAccess = false;

            allowDebuginformation = false;
            mapInitialValue = 0;

            settings.SetDefaults();
        }

        /// <summary>
        /// initalization with given values (loops, rounds, lists with teams)
        /// </summary>
        /// <param name="loops">number of loops</param>
        /// <param name="rounds">number of rounds</param>
        /// <param name="teams">list of teams</param>
        public SimulatorConfiguration(int loops, int rounds, List<TeamInfo> teams)
            : this()
        {
            if (teams != null)
            {
                this.teams = teams;
            }
            roundCount = rounds;
            loopCount = loops;
            ignoreTimeouts = false;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Check the configuration against simulation rules
        /// </summary>
        /// <returns>valid configuration</returns>
        public void Rulecheck()
        {
            // number of rounds
            if (roundCount < ROUNDSMIN)
            {
                throw new ConfigurationErrorsException(Resource.SimulationCoreConfigurationRoundCountTooSmall);
            }
            if (roundCount > ROUNDSMAX)
            {
                throw new ConfigurationErrorsException(
                    string.Format(Resource.SimulationCoreConfigurationRoundCountTooBig, ROUNDSMAX));
            }

            // number of loops
            if (loopCount < LOOPSMIN)
            {
                throw new ConfigurationErrorsException(Resource.SimulationCoreConfigurationLoopCountTooSmall);
            }
            if (loopCount > LOOPSMAX)
            {
                throw new ConfigurationErrorsException(
                    string.Format(Resource.SimulationCoreConfigurationLoopCountTooBig, LOOPSMAX));
            }

            // timeout values
            if (!ignoreTimeouts)
            {
                if (loopTimeout < LOOPTIMEOUTMIN)
                {
                    throw new ConfigurationErrorsException(
                        Resource.SimulationCoreConfigurationLoopTimeoutTooSmall);
                }
                if (roundTimeout < ROUNDTIMEOUTMIN)
                {
                    throw new ConfigurationErrorsException(
                        Resource.SimulationCoreConfigurationRoundTimeoutTooSmall);
                }
            }

            // check teams
            if (teams == null || teams.Count < 0)
            {
                throw new ConfigurationErrorsException(
                    Resource.SimulationCoreConfigurationNoTeams);
            }

            // check teams against rule base
            int playerCount = 0;
            foreach (TeamInfo team in teams)
            {
                team.Rulecheck();
                playerCount += team.Player.Count;
            }

            if (playerCount > PLAYERLIMIT)
            {
                // TODO: Put string into res-file
                throw new ConfigurationErrorsException("Too many players");
            }

            // Checks all properties against valid ranges of values
            Settings.RuleCheck();
        }

        #endregion

        #region properties

        /// <summary>
        /// number of rounds for the hole simulation
        /// </summary>
        public int RoundCount
        {
            get { return roundCount; }
            set { roundCount = value; }
        }

        /// <summary>
        /// number of loops for the hole simulation
        /// </summary>
        public int LoopCount
        {
            get { return loopCount; }
            set { loopCount = value; }
        }

        /// <summary>
        /// timeout can be ignored (for debugging purpose)
        /// </summary>
        public bool IgnoreTimeouts
        {
            get { return ignoreTimeouts; }
            set { ignoreTimeouts = value; }
        }

        /// <summary>
        /// timeout for rounds in ms
        /// </summary>
        public int RoundTimeout
        {
            get { return roundTimeout; }
            set { roundTimeout = value; }
        }

        /// <summary>
        /// timeout for loops in ms
        /// </summary>
        public int LoopTimeout
        {
            get { return loopTimeout; }
            set { loopTimeout = value; }
        }

        /// <summary>
        /// list of players in the team in the simulation
        /// </summary>
        public List<TeamInfo> Teams
        {
            get { return teams; }
        }

        /// <summary>
        /// grant players access to the user interface 
        /// </summary>
        public bool AllowUserinterfaceAccess
        {
            set { allowUserinterfaceAccess = value; }
            get { return allowUserinterfaceAccess; }
        }

        /// <summary>
        /// grant players access to the file system
        /// </summary>
        public bool AllowFileAccess
        {
            set { allowFileAccess = value; }
            get { return allowFileAccess; }
        }

        /// <summary>
        /// grant players access to foreign libraries
        /// </summary>
        public bool AllowReferences
        {
            set { allowReferences = value; }
            get { return allowReferences; }
        }

        /// <summary>
        /// grant players access to databases
        /// </summary>
        public bool AllowDatabaseAccess
        {
            set { allowDatabaseAccess = value; }
            get { return allowDatabaseAccess; }
        }

        /// <summary>
        /// grant players access to network connections
        /// </summary>
        public bool AllowNetworkAccess
        {
            set { allowNetworkAccess = value; }
            get { return allowNetworkAccess; }
        }

        /// <summary>
        /// simulation will print out debug informaiton
        /// </summary>
        public bool AllowDebuginformation
        {
            set { allowDebuginformation = value; }
            get { return allowDebuginformation; }
        }

        /// <summary>
        /// a map initial value will be generated for the initalization of the random number generator
        /// equal map initial values will result in identical start conditions for the simulation
        /// </summary>
        public int MapInitialValue
        {
            set { mapInitialValue = value; }
            get { return mapInitialValue; }
        }

        /// <summary>
        /// simulation settings for this simulation.
        /// </summary>
        public SimulationSettings Settings
        {
            set { settings = value; }
            get { return settings; }
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// clones the configuration
        /// </summary>
        /// <returns>clone of the simulation configuration</returns>
        public object Clone()
        {
            // clone configuration memberwise and copy the teams of players
            SimulatorConfiguration output = (SimulatorConfiguration)MemberwiseClone();
            output.teams = new List<TeamInfo>(teams.Count);
            foreach (TeamInfo team in teams)
            {
                output.teams.Add((TeamInfo)team.Clone());
            }
            return output;
        }

        #endregion
    }
}