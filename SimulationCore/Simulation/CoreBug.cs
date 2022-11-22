using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{
    /// <summary>
    /// Bug
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    internal sealed class CoreBug : CoreInsect
    {
        /// <summary>
        /// indicates wheter bug can do movement in this simulation round
        /// </summary>
        internal bool CanMoveInThisRound = true;

        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> existingInsects)
        {
            base.Init(colony, random, existingInsects);
            coordinate.Radius = 4;
            currentEnergyBase = colony.EnergyI[0];
            currentSpeedI = colony.SpeedI[0];
            AttackStrengthBase = colony.AttackI[0];
        }

        /// <summary>
        /// Bug state object with all current data of bug is created.
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