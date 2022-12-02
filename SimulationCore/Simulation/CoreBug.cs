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
        /// Indicates whether bug can do movement in this simulation round.
        /// </summary>
        internal bool CanMoveInThisRound = true;

        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> existingInsects)
        {
            base.Init(colony, random, existingInsects);
            Coordinate.Radius = 4;
            currentEnergyCoreInsect = colony.EnergyI[0];
            currentSpeedICoreInsect = colony.SpeedI[0];
            AttackStrengthCoreInsect = colony.AttackI[0];
        }

        /// <summary>
        /// Bug state object with all current data of bug is created.
        /// </summary>
        /// <returns></returns>
        internal BugState GenerateInformation()
        {
            BugState info = new BugState((ushort)Id);
            info.PositionX = (ushort)(CoordinateCoreInsect.X / SimulationEnvironment.PLAYGROUND_UNIT);
            info.PositionY = (ushort)(CoordinateCoreInsect.Y / SimulationEnvironment.PLAYGROUND_UNIT);
            info.Direction = (ushort)CoordinateCoreInsect.Direction;
            info.Vitality = (ushort)currentEnergyCoreInsect;
            return info;
        }
    }
}