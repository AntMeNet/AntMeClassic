using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Online.Client
{
    public sealed class Row1Values
    {
        /// <summary>
        /// ID des Listings
        /// </summary>
        public Guid ListingId { get; set; }

        /// <summary>
        /// ID des Users
        /// </summary>
        public Guid UserId { get; set; }

        #region Min-Values

        /// <summary>
        /// Minimalwert an Punkten.
        /// </summary>
        public int MinPoints { get; set; }

        /// <summary>
        /// Minimalwert an verhungerten Ameisen.
        /// </summary>
        public int MinStarvedAnts { get; set; }

        /// <summary>
        /// Minimalwert an aufgefressenen Ameisen.
        /// </summary>
        public int MinEatenAnts { get; set; }

        /// <summary>
        /// Minimalwert an geschlagenen Ameisen.
        /// </summary>
        public int MinBeatenAnts { get; set; }

        /// <summary>
        /// Minimalwert an getöteten Wanzen.
        /// </summary>
        public int MinKilledBugs { get; set; }

        /// <summary>
        /// Minimalwert an getöteten gegnerischen Ameisen.
        /// </summary>
        public int MinKilledEnemies { get; set; }

        /// <summary>
        /// Minimalwert an gesammeltem Essen (Gesamt).
        /// </summary>
        public int MinCollectedFood { get; set; }

        /// <summary>
        /// Minimalwert an gesammelten Äpfeln.
        /// </summary>
        public int MinCollectedFruits { get; set; }

        #endregion

        #region Max-Values

        /// <summary>
        /// Maximalwert an Punkten.
        /// </summary>
        public int MaxPoints { get; set; }

        /// <summary>
        /// Maximalwert an verhungerten Ameisen.
        /// </summary>
        public int MaxStarvedAnts { get; set; }

        /// <summary>
        /// Maximalwert an aufgefressenen Ameisen.
        /// </summary>
        public int MaxEatenAnts { get; set; }

        /// <summary>
        /// Maximalwert an geschlagenen Ameisen.
        /// </summary>
        public int MaxBeatenAnts { get; set; }

        /// <summary>
        /// Maximalwert an getöteten Wanzen.
        /// </summary>
        public int MaxKilledBugs { get; set; }

        /// <summary>
        /// Maximalwert an getöteten gegnerischen Ameisen.
        /// </summary>
        public int MaxKilledEnemies { get; set; }

        /// <summary>
        /// Maximalwert an gesammeltem Essen (Gesamt).
        /// </summary>
        public int MaxCollectedFood { get; set; }

        /// <summary>
        /// Maximalwert an gesammelten Äpfeln.
        /// </summary>
        public int MaxCollectedFruits { get; set; }

        #endregion
    }
}
