using System;
using System.Collections.Generic;
using System.Configuration;

namespace AntMe.Simulation
{
    /// <summary>
    /// Klasse zur Haltung aller relevanten Konfigurationsdaten einer Simulation.
    /// </summary>
    [Serializable]
    public sealed class SimulatorConfiguration : ICloneable
    {

        /// <summary>
        /// Maximum count of players per Simulation.
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

        #region lokale Variablen

        // Runden- und Spielereinstellungen
        private int loopCount;
        private int roundCount;
        private bool allowDebuginformation;
        private int loopTimeout;

        // Zusätzliche Rechte anforderbar
        private bool allowDatabaseAccess;
        private bool allowFileAccess;
        private bool allowReferences;
        private bool allowUserinterfaceAccess;
        private bool allowNetworkAccess;

        /// <summary>
        /// Timeout-Handling
        /// </summary>
        private bool ignoreTimeouts;

        private int roundTimeout;
        private List<TeamInfo> teams;

        // Sonstiges
        private int mapInitialValue;
        private SimulationSettings settings;

        #endregion

        #region Initialisierung und Konstruktoren

        /// <summary>
        /// Initialisiert eine leere Spielerliste
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
        /// Initialsiert mit den übergebenen Werten
        /// </summary>
        /// <param name="loops">Anzahl Durchläufe</param>
        /// <param name="rounds">Anzahl Runden</param>
        /// <param name="teams">Teamliste</param>
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

        #region öffentliche Methoden

        /// <summary>
        /// Ermittelt, ob die Angaben der Konfiguration simulationsfähig sind
        /// </summary>
        /// <returns>Regelkonformer Konfigurationsinhalt</returns>
        public void Rulecheck()
        {
            // Rundenanzahl prüfen
            if (roundCount < ROUNDSMIN)
            {
                throw new ConfigurationErrorsException(Resource.SimulationCoreConfigurationRoundCountTooSmall);
            }
            if (roundCount > ROUNDSMAX)
            {
                throw new ConfigurationErrorsException(
                    string.Format(Resource.SimulationCoreConfigurationRoundCountTooBig, ROUNDSMAX));
            }

            // Durchlaufmenge prüfen
            if (loopCount < LOOPSMIN)
            {
                throw new ConfigurationErrorsException(Resource.SimulationCoreConfigurationLoopCountTooSmall);
            }
            if (loopCount > LOOPSMAX)
            {
                throw new ConfigurationErrorsException(
                    string.Format(Resource.SimulationCoreConfigurationLoopCountTooBig, LOOPSMAX));
            }

            // Timeoutwerte
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

            // Teams checken
            if (teams == null || teams.Count < 0)
            {
                throw new ConfigurationErrorsException(
                    Resource.SimulationCoreConfigurationNoTeams);
            }

            // Regelcheck der Teams
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

            // Regeln für die kern-Einstellungen
            Settings.RuleCheck();
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Gibt die Anzahl der Runden insgesamt an, die in der Simulation durchlaufen werden sollen oder legt diese fest.
        /// </summary>
        public int RoundCount
        {
            get { return roundCount; }
            set { roundCount = value; }
        }

        /// <summary>
        /// Gibt die Anzahl Durchläufe insgesamt an, die in der Simulation durchlaufen werden sollen oder legt diese fest.
        /// </summary>
        public int LoopCount
        {
            get { return loopCount; }
            set { loopCount = value; }
        }

        /// <summary>
        /// Legt fest, ob die Simulation zu debugzwecken Timeouts ignorieren soll.
        /// </summary>
        public bool IgnoreTimeouts
        {
            get { return ignoreTimeouts; }
            set { ignoreTimeouts = value; }
        }

        /// <summary>
        /// Legt die Timeout-Zeit von Runden in ms fest
        /// </summary>
        public int RoundTimeout
        {
            get { return roundTimeout; }
            set { roundTimeout = value; }
        }

        /// <summary>
        /// Legt die Timeout-Zeit von Durchläufen in ms fest
        /// </summary>
        public int LoopTimeout
        {
            get { return loopTimeout; }
            set { loopTimeout = value; }
        }

        /// <summary>
        /// Liefert die Liste der Mitspieler in dieser Simulation.
        /// </summary>
        public List<TeamInfo> Teams
        {
            get { return teams; }
        }

        /// <summary>
        /// Legt fest, ob es den Spielern erlaubt ist auf das Userinterface zuzugreifen
        /// </summary>
        public bool AllowUserinterfaceAccess
        {
            set { allowUserinterfaceAccess = value; }
            get { return allowUserinterfaceAccess; }
        }

        /// <summary>
        /// Legt fest, ob es den Spielern erlaubt ist auf das Dateisystem zuzugreifen
        /// </summary>
        public bool AllowFileAccess
        {
            set { allowFileAccess = value; }
            get { return allowFileAccess; }
        }

        /// <summary>
        /// Legt fest, ob es den Spielern erlaubt ist auf fremde Bibliotheken zuzugreifen
        /// </summary>
        public bool AllowReferences
        {
            set { allowReferences = value; }
            get { return allowReferences; }
        }

        /// <summary>
        /// Legt fest, ob es den Spielern erlaubt ist Datenbankverbindungen zu öffnen
        /// </summary>
        public bool AllowDatabaseAccess
        {
            set { allowDatabaseAccess = value; }
            get { return allowDatabaseAccess; }
        }

        /// <summary>
        /// Legt fest, ob es den Spielern erlaubt ist eine Netzwerkverbindung zu öffnen
        /// </summary>
        public bool AllowNetworkAccess
        {
            set { allowNetworkAccess = value; }
            get { return allowNetworkAccess; }
        }

        /// <summary>
        /// Gibt an, ob die Simulation Debuginformationen ausgeben soll
        /// </summary>
        public bool AllowDebuginformation
        {
            set { allowDebuginformation = value; }
            get { return allowDebuginformation; }
        }

        /// <summary>
        /// Gibt einen Startwert für die Initialisierung des Zufallsgenerators an. Durch einen gleichen
        /// Startwert werden gleiche Startbedingungen garantiert.
        /// </summary>
        public int MapInitialValue
        {
            set { mapInitialValue = value; }
            get { return mapInitialValue; }
        }

        /// <summary>
        /// Gets or sets the simulation-settings for this simulation.
        /// </summary>
        public SimulationSettings Settings
        {
            set { settings = value; }
            get { return settings; }
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// Erstellt eine Kopie der Konfiguration
        /// </summary>
        /// <returns>Kopie der aktuellen Konfiguration</returns>
        public object Clone()
        {
            // Kopie erstellen und Spielerliste kopieren
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