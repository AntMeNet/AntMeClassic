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
            coordinate.Radius = 4;
            currentEnergyBase = colony.EnergyI[0];
            currentSpeedI = colony.SpeedI[0];
            AttackStrengthBase = colony.AttackI[0];
        }

        /// <summary>
        /// Erzeugt ein BugState-Objekt mit dem aktuellen Daten der Wanzen.
        /// </summary>
        /// <returns></returns>
        internal BugState GenerateInformation()
        {
            BugState info = new BugState((ushort)id);
            info.PositionX = (ushort)(CoordinateBase.X / SimulationEnvironment.PLAYGROUND_UNIT);
            info.PositionY = (ushort)(CoordinateBase.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Direction = (ushort)CoordinateBase.Direction;
            info.Vitality = (ushort)currentEnergyBase;
            return info;
        }
    }
}