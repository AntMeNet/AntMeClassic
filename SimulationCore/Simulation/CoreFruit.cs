using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Represents fruit.
    /// </summary>
    internal sealed class CoreFruit : CoreFood
    {
        /// <summary>
        /// Liste der tragenden Ameisen.
        /// </summary>
        internal readonly List<CoreInsect> InsectsCarrying = new List<CoreInsect>();

        /// <summary>
        /// Erzeugt eine neue Instanz der Obst-Klasse.
        /// </summary>
        /// <param name="x">Die X-Position des Obststücks auf dem Spielfeld.</param>
        /// <param name="y">Die Y-Position des Obststücks auf dem Spielfeld.</param>
        /// <param name="menge">Die Anzahl an Nahrungspunkten.</param>
        internal CoreFruit(int x, int y, int menge)
            : base(x, y, menge) { }

        /// <summary>
        /// Die verbleibende Menge an Nahrungspunkten.
        /// </summary>
        public override int Amount
        {
            internal set
            {
                menge = value;
                koordinate.Radius = (int)(SimulationSettings.Custom.FruitRadiusMultiplier *
                                           Math.Sqrt(menge / Math.PI) * SimulationEnvironment.PLAYGROUND_UNIT);
            }
        }

        /// <summary>
        /// Bestimmt, ob das Stück Obst für das angegebene Volk noch tragende
        /// Insekten benötigt, um die maximale Geschwindigkeit beim Tragen zu
        /// erreichen.
        /// </summary>
        /// <param name="colony">Das Volk.</param>
        internal bool NeedSupport(CoreColony colony)
        {
            int last = 0;
            foreach (CoreInsect insekt in InsectsCarrying)
            {
                if (insekt.colony == colony)
                {
                    last += insekt.CurrentBurdenBase;
                }
            }
            return last * SimulationSettings.Custom.FruitLoadMultiplier < Amount;
        }

        /// <summary>
        /// Erzeugt ein ObstZustand-Objekt mit dem Daten Zustand des Obststücks.
        /// </summary>
        internal FruitState GenerateInformation()
        {
            FruitState info = new FruitState((ushort)Id);
            info.PositionX = (ushort)(koordinate.X / SimulationEnvironment.PLAYGROUND_UNIT);
            info.PositionY = (ushort)(koordinate.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Radius = (ushort)(koordinate.Radius / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Amount = (ushort)menge;
            info.CarryingAnts = (byte)InsectsCarrying.Count;
            return info;
        }
    }
}
