using System;

namespace AntMe.Simulation {
    /// <summary>
    /// List of possible areas
    /// </summary>
    internal enum Area {
        Unknown,
        Constructor,
        ChooseType,
        Waits,
        SpotsSugar,
        SpotsFruit,
        ReachedSugar,
        ReachedFruit,
        BecomesTired,
        SmellsFriend,
        SpotsFriend,
        SpotsEnemy,
        SpotsTeamMember,
        SpotsBug,
        UnderAttackByAnt,
        UnderAttackByBug,
        HasDied,
        Tick
    }

    /// <summary>
    /// Event-Arguments for a AreaChange-Event
    /// </summary>
    internal class AreaChangeEventArgs : EventArgs {
        private readonly Area area;
        private readonly PlayerInfo player;

        /// <summary>
        /// Creates a new AreaChangeEventArgs to unknown area.
        /// </summary>
        public AreaChangeEventArgs() {
            area = Area.Unknown;
            player = null;
        }

        /// <summary>
        /// Creates a new AreaChangeEventArgs to given area and player.
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="area">area</param>
        public AreaChangeEventArgs(PlayerInfo player, Area area) {
            this.player = player;
            this.area = area;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        public PlayerInfo Player {
            get { return player; }
        }

        /// <summary>
        /// Gets the area.
        /// </summary>
        public Area Area {
            get { return area; }
        }
    }

    /// <summary>
    /// Delegate for area-changes.
    /// </summary>
    /// <param name="sender">sending simulation-environment</param>
    /// <param name="e">arguments for event</param>
    internal delegate void AreaChangeEventHandler(object sender, AreaChangeEventArgs e);
}