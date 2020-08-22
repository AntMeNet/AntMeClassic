using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AntMe.Simulation
{
    /// <summary>
    /// Holds all meta-information about a player.
    /// </summary>
    [Serializable]
    public class PlayerInfo : ICloneable
    {
        #region lokale Variablen

        /// <summary>
        /// List of all castes.
        /// </summary>
        private readonly List<CasteInfo> castes;

        /// <summary>
        /// Reference to the ai-assembly-file.
        /// </summary>
        internal Assembly assembly;

        /// <summary>
        /// true, if the Ai gives some debug-information.
        /// </summary>
        public bool HasDebugInformation;

        /// <summary>
        /// Guid of player.
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// Name of colony.
        /// </summary>
        public string ColonyName;

        /// <summary>
        /// Complete Class-name of colony-class.
        /// </summary>
        public string ClassName;

        /// <summary>
        /// true, if the colony needs access to a database.
        /// </summary>
        public bool RequestDatabaseAccess;

        /// <summary>
        /// true, if the colony needs access to files.
        /// </summary>
        public bool RequestFileAccess;

        /// <summary>
        /// true, if the colony needs access to the network.
        /// </summary>
        public bool RequestNetworkAccess;

        /// <summary>
        /// Additional information about access-requests.
        /// </summary>
        public string RequestInformation;

        /// <summary>
        /// true, if the colony has references to other assemblies.
        /// </summary>
        public bool RequestReferences;

        /// <summary>
        /// true, if the colony needs access to user-interfaces.
        /// </summary>
        public bool RequestUserInterfaceAccess;

        /// <summary>
        /// Last name of colony-author.
        /// </summary>
        public string LastName;

        /// <summary>
        /// First name of colony-author.
        /// </summary>
        public string FirstName;

        /// <summary>
        /// Language of colony.
        /// </summary>
        public PlayerLanguages Language;

        /// <summary>
        /// true, if the colony uses any static types.
        /// </summary>
        public bool Static;

        /// <summary>
        /// Simulator-Version of this colony.
        /// </summary>
        public PlayerSimulationVersions SimulationVersion;

        #endregion

        #region Constructor and Initializaion

        /// <summary>
        /// Creates a new instance of PlayerInfo.
        /// </summary>
        public PlayerInfo()
        {
            // Init default-values
            Guid = System.Guid.NewGuid();
            ColonyName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ClassName = string.Empty;
            SimulationVersion = PlayerSimulationVersions.Version_1_6;
            Language = PlayerLanguages.Unknown;
            Static = true;
            castes = new List<CasteInfo>();
            HasDebugInformation = false;
            RequestUserInterfaceAccess = false;
            RequestDatabaseAccess = false;
            RequestFileAccess = false;
            RequestReferences = false;
            RequestNetworkAccess = false;
            RequestInformation = string.Empty;
        }

        /// <summary>
        /// Creates a new instance of PlayerInfo.
        /// </summary>
        /// <param name="info">Base-info</param>
        public PlayerInfo(PlayerInfo info)
        {
            // Daten kopieren
            Guid = info.Guid;
            ColonyName = info.ColonyName;
            FirstName = info.FirstName;
            LastName = info.LastName;
            ClassName = info.ClassName;
            SimulationVersion = info.SimulationVersion;
            Language = info.Language;
            Static = info.Static;
            castes = info.castes;
            HasDebugInformation = info.HasDebugInformation;
            RequestUserInterfaceAccess = info.RequestUserInterfaceAccess;
            RequestDatabaseAccess = info.RequestDatabaseAccess;
            RequestFileAccess = info.RequestFileAccess;
            RequestReferences = info.RequestReferences;
            RequestNetworkAccess = info.RequestNetworkAccess;
            RequestInformation = info.RequestInformation;
        }

        /// <summary>
        /// Creates a new instance of PlayerInfo.
        /// </summary>
        /// <param name="guid">Guid of colony</param>
        /// <param name="colonyName">Name of colony</param>
        /// <param name="lastName">Last name of author</param>
        /// <param name="firstName">First name of author</param>
        /// <param name="className">Class-name of colony</param>
        /// <param name="staticVariables">Colony uses static types</param>
        /// <param name="simulationVersion">Version of simulator of this colony</param>
        /// <param name="language">Language of this colony</param>
        /// <param name="castes">List of castes</param>
        /// <param name="haveDebugInformation">Colony produces debug-information</param>
        /// <param name="requestUserinterfaceAccess">Needs access to user-interfaces</param>
        /// <param name="requestDatabaseAccess">Needs access to databases</param>
        /// <param name="requestFileAccess">Needs access to files</param>
        /// <param name="requestNetworkAccess">Needs access to network</param>
        /// <param name="requestReferences">Needs references to other assemblies</param>
        /// <param name="requestInformation">Additional information about security-requests</param>
        public PlayerInfo(
            Guid guid,
            string colonyName,
            string firstName,
            string lastName,
            string className,
            PlayerSimulationVersions simulationVersion,
            PlayerLanguages language,
            bool staticVariables,
            List<CasteInfo> castes,
            bool haveDebugInformation,
            bool requestUserinterfaceAccess,
            bool requestDatabaseAccess,
            bool requestFileAccess,
            bool requestNetworkAccess,
            bool requestReferences,
            string requestInformation)
        {
            // Ameisenkasten überprüfen
            if (castes == null)
            {
                this.castes = new List<CasteInfo>();
            }
            else
            {
                this.castes = castes;
            }

            // Restliche Daten übertragen
            Guid = guid;
            ColonyName = colonyName;
            FirstName = firstName;
            LastName = lastName;
            ClassName = className;
            SimulationVersion = simulationVersion;
            Language = language;
            Static = staticVariables;
            HasDebugInformation = haveDebugInformation;
            RequestUserInterfaceAccess = requestUserinterfaceAccess;
            RequestDatabaseAccess = requestDatabaseAccess;
            RequestFileAccess = requestFileAccess;
            RequestNetworkAccess = requestNetworkAccess;
            RequestReferences = requestReferences;
            RequestInformation = requestInformation;
        }

        #endregion

        #region Hilfsmethoden

        /// <summary>
        /// Checks the rules.
        /// </summary>
        /// <throws><see cref="RuleViolationException"/></throws>
        public void RuleCheck()
        {

            // Invalidate colonies without a name
            if (string.IsNullOrEmpty(ColonyName))
            {
                throw new RuleViolationException(
                    string.Format(Resource.SimulationCorePlayerRuleNoName, ClassName));
            }

            // Check included castes
            foreach (CasteInfo caste in castes)
            {
                caste.Rulecheck(ClassName);
            }
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Delivers the list of castes.
        /// </summary>
        public List<CasteInfo> Castes
        {
            get { return castes; }
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// Clones the whole object
        /// </summary>
        /// <returns>clone</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {

            if (!string.IsNullOrEmpty(ColonyName))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ColonyName);
                if (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName))
                {
                    sb.Append(" (");
                    if (!string.IsNullOrEmpty(FirstName))
                    {
                        sb.Append(FirstName);
                        if (!string.IsNullOrEmpty(LastName))
                        {
                            sb.Append(" ");
                            sb.Append(LastName);
                        }
                    }
                    else sb.Append(LastName);
                    sb.Append(")");
                }
                return sb.ToString();
            }
            else
            {
                return ClassName;
            }
        }
    }
}