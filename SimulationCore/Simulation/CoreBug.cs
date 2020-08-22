using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Eine Wanze
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreBug : CoreInsect
    {
        /// <summary>
        /// Gibt an, ob die Wanze sich in der aktuellen Runde noch bewegen kann.
        /// </summary>
        internal bool KannSichNochBewegen = true;

        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> vorhandeneInsekten)
        {
            base.Init(colony, random, vorhandeneInsekten);
            koordinate.Radius = 4;
            AktuelleEnergieBase = colony.Energie[0];
            aktuelleGeschwindigkeitI = colony.GeschwindigkeitI[0];
            AngriffBase = colony.Angriff[0];
        }

        /// <summary>
        /// Erzeugt ein BugState-Objekt mit dem aktuellen Daten der Wanzen.
        /// </summary>
        /// <returns></returns>
        internal BugState ErzeugeInfo()
        {
            BugState info = new BugState((ushort)id);
            info.PositionX = (ushort)(CoordinateBase.X / SimulationEnvironment.PLAYGROUND_UNIT);
            info.PositionY = (ushort)(CoordinateBase.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Direction = (ushort)CoordinateBase.Richtung;
            info.Vitality = (ushort)AktuelleEnergieBase;
            return info;
        }
    }
}