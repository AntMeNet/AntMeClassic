using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Repräsentiert ein Team innerhalb der Simulationskonfiguration
    /// </summary>
    [Serializable]
    public sealed class TeamInfo : ICloneable
    {

        /// <summary>
        /// Guid des Teams
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// Name des Teams
        /// </summary>
        public string Name;

        /// <summary>
        /// Liste der enthaltenen Spieler
        /// </summary>
        private List<PlayerInfo> player;

        #region Initialisierung und Konstruktor

        /// <summary>
        /// Konstruktor des Teams
        /// </summary>
        public TeamInfo()
        {
            Guid = System.Guid.NewGuid();
            player = new List<PlayerInfo>();
        }

        /// <summary>
        /// Konstruktor des Teams
        /// </summary>
        /// <param name="player">Liste der Spieler</param>
        public TeamInfo(List<PlayerInfo> player)
            : this()
        {
            this.player = player;
        }

        /// <summary>
        /// Konstruktor des Teams
        /// </summary>
        /// <param name="guid">Guid des Teams</param>
        /// <param name="player">Liste der Spieler</param>
        public TeamInfo(Guid guid, List<PlayerInfo> player)
            : this(player)
        {
            Guid = guid;
        }

        /// <summary>
        /// Konstruktor des Teams
        /// </summary>
        /// <param name="name">Name des Teams</param>
        /// <param name="player">Liste der Spieler</param>
        public TeamInfo(string name, List<PlayerInfo> player)
            : this(player)
        {
            Name = name;
        }

        /// <summary>
        /// Konstruktor des Teams
        /// </summary>
        /// <param name="guid">Guid des Teams</param>
        /// <param name="name">Name des Teams</param>
        /// <param name="player">Liste der Spieler</param>
        public TeamInfo(Guid guid, string name, List<PlayerInfo> player)
            : this(player)
        {
            Guid = guid;
            Name = name;
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Liste der spieler dieses Teams
        /// </summary>
        public List<PlayerInfo> Player
        {
            get { return player; }
        }

        #endregion

        #region öffentliche Methoden

        /// <summary>
        /// Prüft, ob das Team regelkonform aufgebaut ist
        /// </summary>
        public void Rulecheck()
        {
            // Menge der Spieler prüfen
            if (player == null || player.Count < 1)
            {
                // TODO: Name der Resource ist Mist
                throw new InvalidOperationException(Resource.SimulationCoreTeamInfoNoName);
            }

            // Regelcheck bei den enthaltenen Spielern
            foreach (PlayerInfo info in player)
            {
                info.RuleCheck();
            }
        }

        #endregion

        #region ICloneable Member

        /// <summary>
        /// Erstellt eine Kopie des Teams
        /// </summary>
        /// <returns>Kopie des Teams</returns>
        public object Clone()
        {
            TeamInfo output = (TeamInfo)MemberwiseClone();
            output.player = new List<PlayerInfo>(player.Count);
            foreach (PlayerInfo info in player)
            {
                output.player.Add((PlayerInfo)info.Clone());
            }
            return output;
        }

        #endregion
    }
}