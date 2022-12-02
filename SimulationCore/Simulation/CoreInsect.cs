using System;
using System.Collections.Generic;
using System.Threading;

namespace AntMe.Simulation
{
    /// <summary>
    /// Abstract base class of all insects.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreInsect : ICoordinate
    {
        /// <summary>
        /// ID for newly created insect.
        /// </summary>
        private static int newId = 0;

        /// <summary>
        /// List of all markers the insect has spotted.
        /// Prevents insects from running in circles. 
        /// </summary>
        internal readonly List<CoreMarker> SmelledMarker = new List<CoreMarker>();

        private bool arrived = false;
        private int antCount = 0;
        private int casteCount = 0;
        private int colonyCount = 0;
        private int bugCount = 0;
        private int teamCount = 0;
        private CoreFruit carryingFruit;

        /// <summary>
        /// ID of the insect. It will identify the insect throughout the game.
        /// </summary>
        internal int id;

        /// <summary>
        /// Coordinates of the insect on the playground.
        /// </summary>
        internal CoreCoordinate coordinate;

        /// <summary>
        /// Defines whether the insect is awaiting commands.
        /// </summary>
        internal bool AwaitingCommands = false;

        private int distanceToDestination;
        private int residualAngle = 0;

        /// <summary>
        /// Depending on the players caste structure, index of the caste.
        /// </summary>
        internal int CasteIndexBase;

        /// <summary>
        /// Colony the insect is belonging to.
        /// </summary>
        internal CoreColony colony;

        private ICoordinate target = null;
        private int travelledDistanceI;

        internal CoreInsect() { }

        /// <summary>
        /// Caste of the insect.
        /// </summary>
        internal string CasteBase
        {
            get { return colony.Player.Castes[CasteIndexBase].Name; }
        }

        /// <summary>
        /// Number of bugs in view range of the insect.
        /// </summary>
        internal int BugsInViewrange
        {
            get { return bugCount; }
            set { bugCount = value; }
        }

        /// <summary>
        /// Number of enemy ants in view range of the insect.
        /// </summary>
        internal int ForeignAntsInViewrange
        {
            get { return antCount; }
            set { antCount = value; }
        }

        /// <summary>
        /// Number of friendly ants in view range of the insect.
        /// </summary>
        internal int FriendlyAntsInViewrange
        {
            get { return colonyCount; }
            set { colonyCount = value; }
        }

        /// <summary>
        /// Number of friendly ants of same caste in view range of the insect.
        /// </summary>
        internal int FriendlyAntsFromSameCasteInViewrange
        {
            get { return casteCount; }
            set { casteCount = value; }
        }

        /// <summary>
        /// Number of team ants in view range.
        /// </summary>
        internal int TeamAntsInViewrange
        {
            get { return teamCount; }
            set { teamCount = value; }
        }

        /// <summary>
        /// Direction the insect is turned to.
        /// </summary>
        internal int DirectionBase
        {
            get { return coordinate.Direction; }
        }

        /// <summary>
        /// Travelled distance of ant since last stop in anthill.
        /// </summary>
        internal int travelledDistanceBase
        {
            get { return travelledDistanceI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// In internal units: travelled distance of ant since last stop in anthill.
        /// </summary>
        internal int travelledDistance
        {
            get { return travelledDistanceI; }
            set { travelledDistanceI = value; }
        }

        /// <summary>
        /// Distance to destination.
        /// After walking this distance the insect will be turn to target or it will be waiting for commands.
        /// </summary>
        internal int DistanceToDestinationBase
        {
            get { return distanceToDestination / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// In internal units: distance to destination
        /// After walking this distance the insect will be turn to target or it will be waiting for commands.
        /// </summary>
        internal int DistanceToDestination
        {
            get { return distanceToDestination; }
            set { distanceToDestination = value; }
        }

        /// <summary>
        /// Residual angle to turn before insect can go forward again.
        /// </summary>
        internal int ResidualAngle
        {
            get { return residualAngle; }
            set
            {
                // TODO: Modulo?
                residualAngle = value;
                while (residualAngle > 180)
                {
                    residualAngle -= 360;
                }
                while (residualAngle < -180)
                {
                    residualAngle += 360;
                }
            }
        }

        /// <summary>
        /// Insects target.
        /// </summary>
        internal ICoordinate TargetBase
        {
            get { return target; }
            set
            {
                if (target != value || value == null)
                {
                    target = value;
                    residualAngle = 0;
                    distanceToDestination = 0;
                }
            }
        }

        /// <summary>
        /// Distance to anthill in simulation steps.
        /// </summary>
        internal int DistanceToAnthillBase
        {
            get
            {
                int currentDistance;
                int rememberedDistance = int.MaxValue;
                foreach (CoreAnthill anthill in colony.AntHills)
                {
                    currentDistance = CoreCoordinate.DetermineDistanceI(CoordinateBase, anthill.CoordinateBase);
                    if (currentDistance < rememberedDistance)
                    {
                        rememberedDistance = currentDistance;
                    }
                }
                return rememberedDistance / SimulationEnvironment.PLAYGROUND_UNIT;
            }
        }

        /// <summary>
        /// Carried fruit.
        /// </summary>
        internal CoreFruit CarryingFruitBase
        {
            get { return carryingFruit; }
            set { carryingFruit = value; }
        }

        /// <summary>
        /// Defines whether the insect has reached the destination.
        /// </summary>
        internal bool ArrivedBase
        {
            get { return arrived; }
        }

        internal Random RandomBase { get; private set; }

        #region ICoordinate

        /// <summary>
        /// Coordinates of insect on playground.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return coordinate; }
            internal set { coordinate = value; }
        }

        #endregion

        /// <summary>
        /// Abstract insect constructor.
        /// </summary>
        /// <param name="colony">new insect will be part of this colony</param>
        /// <param name="insectsInColony">in constructor unused</param>
        internal virtual void Init(CoreColony colony, Random random, Dictionary<string, int> insectsInColony)
        {
            id = newId;
            newId++;

            this.colony = colony;
            this.RandomBase = random;

            coordinate.Direction = RandomBase.Next(0, 359);

            // Place randomly on the edge of the playground.
            if (colony.AntHills.Count == 0) // Place on the upper or lower edge.
            {
                if (RandomBase.Next(2) == 0)
                {
                    coordinate.X = RandomBase.Next(0, colony.Playground.Width);
                    coordinate.X *= SimulationEnvironment.PLAYGROUND_UNIT;
                    if (RandomBase.Next(2) == 0)
                    {
                        coordinate.Y = 0;
                    }
                    else
                    {
                        coordinate.Y = colony.Playground.Height * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                }

                // Place on left or right edge.
                else
                {
                    if (RandomBase.Next(2) == 0)
                    {
                        coordinate.X = 0;
                    }
                    else
                    {
                        coordinate.X = colony.Playground.Width * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                    coordinate.Y = RandomBase.Next(0, colony.Playground.Height);
                    coordinate.Y *= SimulationEnvironment.PLAYGROUND_UNIT;
                }
            }

            // Place in a random ant hill.
            else
            {
                int i = RandomBase.Next(colony.AntHills.Count);
                coordinate.X = colony.AntHills[i].CoordinateBase.X +
                               SimulationEnvironment.Cosinus(
                                   colony.AntHills[i].CoordinateBase.Radius, coordinate.Direction);
                coordinate.Y = colony.AntHills[i].CoordinateBase.Y +
                               SimulationEnvironment.Sinus(
                                   colony.AntHills[i].CoordinateBase.Radius, coordinate.Direction);
            }
        }

        /// <summary>
        /// Defines whether the insect needs support to carry fruit.
        /// </summary>
        /// <param name="fruit">Fruit</param>
        /// <returns></returns>
        internal bool NeedSupport(CoreFruit fruit)
        {
            return fruit.NeedSupport(colony);
        }

        /// <summary>
        /// The insect rotates for given degrees
        /// maximum rotation is 180 degree to the right
        /// this is 180 degrees to the left
        /// greater values will be cut off.
        /// </summary>
        /// <param name="angle">angle</param>
        internal void TurnByDegreesBase(int angle)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            ResidualAngle = angle;
        }

        /// <summary>
        /// Insect rotates into a given direction.
        /// </summary>
        /// <param name="direction">Direction</param>
        internal void TurnIntoDirectionBase(int direction)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            turnIntoDirection(direction);
        }

        private void turnIntoDirection(int direction)
        {
            ResidualAngle = direction - coordinate.Direction;
        }

        /// <summary>
        /// Insect rotates towards a given target.
        /// </summary>
        /// <param name="target">Target</param>
        internal void TurnToTargetBase(ICoordinate target)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, target));
        }

        /// <summary>
        /// The insect turns for 180 degree. 
        /// </summary>
        internal void TurnAroundBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            if (residualAngle > 0)
            {
                residualAngle = 180;
            }
            else
            {
                residualAngle = -180;
            }
        }

        /// <summary>
        /// Insect will go forward without delimiter.
        /// </summary>
        internal void GoForwardBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToDestination = int.MaxValue;
        }

        /// <summary>
        /// Insect will go forward for given simulation steps.
        /// </summary>
        /// <param name="distance">distance in simulation steps</param>
        internal void GoForwardBase(int distance)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToDestination = distance * SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// Insect goes to target. Target can be moving.
        /// Target will be attacked if it is a bug.
        /// </summary>
        /// <param name="target">target</param>
        internal void GoToTargetBase(ICoordinate target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = target;
        }

        /// <summary>
        /// Attacks a target. Target can be moving.
        /// Only bugs can be the target for this overload.
        /// </summary>
        /// <param name="target">target</param>
        internal void AttackBase(CoreInsect target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = target;
        }

        /// <summary>
        /// Insect goes away from source. Usually this is used
        /// to flee from another insect
        /// </summary>
        /// <param name="source">source</param> 
        internal void GoAwayFromBase(ICoordinate source)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardBase();
        }

        /// <summary>
        /// Insect goes away from source for a given distance in simulation steps.
        /// Usually used to flee from another insect.
        /// </summary>
        /// <param name="source">source</param> 
        /// <param name="distance">distance in simulation steps</param>
        internal void GoAwayFromBase(ICoordinate source, int distance)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardBase(distance);
        }

        /// <summary>
        /// Insect will go to nearest colony's anthill.
        /// </summary>
        internal void GoToAnthillBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            int currentDistance;
            int rememberedDistance = int.MaxValue;
            CoreAnthill rememberedAnthill = null;
            foreach (CoreAnthill anthill in colony.AntHills)
            {
                currentDistance = CoreCoordinate.DetermineDistanceI(CoordinateBase, anthill.CoordinateBase);
                if (currentDistance < rememberedDistance)
                {
                    rememberedAnthill = anthill;
                    rememberedDistance = currentDistance;
                }
            }
            GoToTargetBase(rememberedAnthill);
        }

        /// <summary>
        /// Insect stops and forgets target or destination.
        /// </summary>
        internal void StopMovementBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = null;
            distanceToDestination = 0;
            residualAngle = 0;
        }

        /// <summary>
        /// Insect takes sugar from sugar pile.
        /// </summary>
        /// <param name="sugar">sugar</param>
        internal void TakeBase(CoreSugar sugar)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            int distance = CoreCoordinate.DetermineDistanceI(CoordinateBase, sugar.CoordinateBase);
            if (distance <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                int amount = Math.Min(MaximumLoadBase - currentLoad, sugar.Amount);
                CurrentLoadBase += amount;
                sugar.Amount -= amount;
            }
            else
            {
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// Insect takes fruit.
        /// </summary>
        /// <param name="fruit">fruit</param>
        internal void TakeBase(CoreFruit fruit)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            if (CarryingFruitBase == fruit)
            {
                return;
            }
            if (CarryingFruitBase != null)
            {
                DropFoodBase();
            }
            int distance = CoreCoordinate.DetermineDistanceI(CoordinateBase, fruit.CoordinateBase);
            if (distance <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                StopMovementBase();
                CarryingFruitBase = fruit;
                fruit.InsectsCarrying.Add(this);
                CurrentLoadBase = colony.LoadI[CasteIndexBase];
            }
        }

        /// <summary>
        /// Insect drops food and forgets target.
        /// </summary>
        internal void DropFoodBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            CurrentLoadBase = 0;
            TargetBase = null;
            if (CarryingFruitBase != null)
            {
                CarryingFruitBase.InsectsCarrying.Remove(this);
                CarryingFruitBase = null;
            }
        }

        /// <summary>
        /// Insect sets marker with given information and size.
        /// Bigger marks have shorter lifespan.
        /// </summary>
        /// <param name="information">information</param>
        /// <param name="size">size in simulation steps</param>
        internal void MakeMarkerBase(int information, int size)
        {
            if (!AwaitingCommands)
            {
                return;
            }

            // Check for unsupported marker size.
            if (size < 0)
            {
                throw new AiException(string.Format("{0}: {1}", colony.Player.Guid,
                    Resource.SimulationCoreNegativeMarkerSize));
            }

            CoreMarker marker = new CoreMarker(coordinate, size, colony.Id);
            marker.Information = information;
            colony.NewMarker.Add(marker);
            SmelledMarker.Add(marker);
        }

        /// <summary>
        /// Lets the ant spray a mark. The marker contains the information and does not spread.
        /// Thus, the mark has the maximum life span.
        /// </summary>
        /// <param name="information">The information.</param>
        internal void MakeMarkerBase(int information)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            MakeMarkerBase(information, 0);
        }

        /// <summary>
        /// Calculates the movement of the insect.
        /// </summary>
        internal void Move()
        {
            arrived = false;

            // Insect rotates.
            if (residualAngle != 0)
            {
                // Target angle is reached.
                if (Math.Abs(residualAngle) < colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction += residualAngle;
                    residualAngle = 0;
                }

                // Insect rotates to the right.
                else if (residualAngle >= colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction += colony.RotationSpeedI[CasteIndexBase];
                    ResidualAngle -= colony.RotationSpeedI[CasteIndexBase];
                }

                // Insect rotates to the left.
                else if (residualAngle <= -colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction -= colony.RotationSpeedI[CasteIndexBase];
                    ResidualAngle += colony.RotationSpeedI[CasteIndexBase];
                }
            }

            // Insect walks.
            else if (distanceToDestination > 0)
            {
                if (CarryingFruitBase == null)
                {
                    int distance = Math.Min(distanceToDestination, currentSpeedI);

                    distanceToDestination -= distance;
                    travelledDistanceI += distance;
                    coordinate.X += SimulationEnvironment.Cos[distance, coordinate.Direction];
                    coordinate.Y += SimulationEnvironment.Sin[distance, coordinate.Direction];
                }
            }

            // Insect walks towards target.
            else if (target != null)
            {
                int distanceI;

                if (TargetBase is CoreMarker)
                {
                    distanceI = CoreCoordinate.DetermineDistanceToCenter(coordinate, target.CoordinateBase);
                }
                else
                {
                    distanceI = CoreCoordinate.DetermineDistanceI(coordinate, target.CoordinateBase);
                }

                arrived = distanceI <= SimulationEnvironment.PLAYGROUND_UNIT;
                if (!arrived)
                {
                    int direction = CoreCoordinate.DetermineDirection(coordinate, target.CoordinateBase);

                    // Target is in sight or insect is carrying fruit.
                    if (distanceI < colony.ViewRangeI[CasteIndexBase] || carryingFruit != null)
                    {
                        distanceToDestination = distanceI;
                    }

                    // Otherwise randomize direction.
                    else
                    {
                        direction += RandomBase.Next(-18, 18);
                        distanceToDestination = colony.ViewRangeI[CasteIndexBase];
                    }

                    turnIntoDirection(direction);
                }
            }

            // Limit coordinates to the left.
            if (coordinate.X < 0)
            {
                coordinate.X = -coordinate.X;
                if (coordinate.Direction > 90 && coordinate.Direction <= 180)
                {
                    coordinate.Direction = 180 - coordinate.Direction;
                }
                else if (coordinate.Direction > 180 && coordinate.Direction < 270)
                {
                    coordinate.Direction = 540 - coordinate.Direction;
                }
            }

            // Limit coordinates to the right.
            else if (coordinate.X > colony.WidthI)
            {
                coordinate.X = colony.WidthI2 - coordinate.X;
                if (coordinate.Direction >= 0 && coordinate.Direction < 90)
                {
                    coordinate.Direction = 180 - coordinate.Direction;
                }
                else if (coordinate.Direction > 270 && coordinate.Direction < 360)
                {
                    coordinate.Direction = 540 - coordinate.Direction;
                }
            }

            // Limit coordinates at the top.
            if (coordinate.Y < 0)
            {
                coordinate.Y = -coordinate.Y;
                if (coordinate.Direction > 180 && coordinate.Direction < 360)
                {
                    coordinate.Direction = 360 - coordinate.Direction;
                }
            }

            // Limit coordinates below.
            else if (coordinate.Y > colony.HeightI)
            {
                coordinate.Y = colony.HeightI2 - coordinate.Y;
                if (coordinate.Direction > 0 && coordinate.Direction < 180)
                {
                    coordinate.Direction = 360 - coordinate.Direction;
                }
            }
        }

        #region Speed

        /// <summary>
        /// The current speed of the insect in the internal unit.
        /// </summary>
        internal int currentSpeedI;

        /// <summary>
        /// The current speed of the insect in steps. When the insect
        /// carries its maximum load, its speed is halved.
        /// </summary>
        internal int CurrentSpeedBase
        {
            get { return currentSpeedI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// The insect's maximum speed.
        /// </summary>
        internal int MaximumSpeedBase
        {
            get { return colony.SpeedI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        #endregion

        #region RotationSpeed

        /// <summary>
        /// The insect's rotation speed in degrees per round.
        /// </summary>
        internal int RotationSpeedBase
        {
            get { return colony.RotationSpeedI[CasteIndexBase]; }
        }

        #endregion

        #region Load

        private int currentLoad = 0;

        /// <summary>
        /// The insect's current load
        /// </summary>
        internal int CurrentLoadBase
        {
            get { return currentLoad; }
            set
            {
                currentLoad = value >= 0 ? value : 0;
                currentSpeedI = colony.SpeedI[CasteIndexBase];
                currentSpeedI -= currentSpeedI * currentLoad / colony.LoadI[CasteIndexBase] / 2;
            }
        }

        /// <summary>
        /// The insect's maximum load.
        /// </summary>
        internal int MaximumLoadBase
        {
            get { return colony.LoadI[CasteIndexBase]; }
        }

        #endregion

        #region ViewRange

        /// <summary>
        /// Insect's view range in simulation steps.
        /// </summary>
        internal int ViewRangeBase
        {
            get { return colony.ViewRangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// The insect's view range in internal unit.
        /// </summary>
        internal int ViewRangeI
        {
            get { return colony.ViewRangeI[CasteIndexBase]; }
        }

        #endregion

        #region Range

        /// <summary>
        /// The insect's range in simulation steps.
        /// </summary>
        internal int RangeBase
        {
            get { return colony.RangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// The insect's range in internal units.
        /// </summary>
        internal int RangeI
        {
            get { return colony.RangeI[CasteIndexBase]; }
        }

        #endregion

        #region Energy

        private int currentEngergy;

        /// <summary>
        /// The insect's current energy.
        /// </summary>
        internal int currentEnergyBase
        {
            get { return currentEngergy; }
            set { currentEngergy = value >= 0 ? value : 0; }
        }

        /// <summary>
        /// The insect's maximum energy.
        /// </summary>
        internal int MaximumEnergyBase
        {
            get { return colony.EnergyI[CasteIndexBase]; }
        }

        #endregion

        #region Attack

        private int attackStrength;

        /// <summary>
        /// The insect's attack strength.
        /// Zero, if insect carries load.
        /// </summary>
        internal int AttackStrengthBase
        {
            get { return currentLoad == 0 ? attackStrength : 0; }
            set { attackStrength = value >= 0 ? value : 0; }
        }

        #endregion

        #region Debug

        internal string debugMessage;

        internal void ThinkCore(string message)
        {
            debugMessage = message.Length > 100 ? message.Substring(0, 100) : message;
        }

        #endregion
    }
}