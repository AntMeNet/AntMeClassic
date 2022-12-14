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
        private int casteAntsCount = 0;
        private int colonyCount = 0;
        private int bugCount = 0;
        private int teamCount = 0;
        private CoreFruit carryingFruit;
        private int distanceToDestinationI;
        private int residualAngle = 0;
        private ICoordinate destination = null;
        private int numberStepsWalkedI;

        /// <summary>
        /// ID of the insect. It will identify the insect throughout the game.
        /// </summary>
        internal int Id;

        /// <summary>
        /// Coordinates of the insect on the playground.
        /// </summary>
        internal CoreCoordinate Coordinate;

        /// <summary>
        /// Defines whether the insect is awaiting commands.
        /// </summary>
        internal bool AwaitingCommands = false;

        /// <summary>
        /// Depending on the players caste structure, index of the caste.
        /// </summary>
        internal int CasteIndexCoreInsect;

        /// <summary>
        /// Colony the insect is belonging to.
        /// </summary>
        internal CoreColony Colony;

        internal CoreInsect() { }

        /// <summary>
        /// Caste of the insect.
        /// </summary>
        /// <returns>Caste name of the ant as string.</returns>
        internal string CasteCoreInsect => Colony.Player.Castes[CasteIndexCoreInsect].Name;

        /// <summary>
        /// Number of bugs in view range of the insect.
        /// </summary>
        /// <returns>Number of bugs in view range.</returns>
        internal int BugsInViewRange
        {
            get => bugCount;
            set => bugCount = value;
        }

        /// <summary>
        /// Number of enemy ants in view range of the insect.
        /// </summary>
        /// <returns>Number of enemy ants in view range.</returns>
        internal int EnemyAntsInViewRange
        {
            get => antCount;
            set => antCount = value;
        }

        /// <summary>
        /// Number of friendly ants in view range of the insect.
        /// </summary>
        /// <returns>Number of ants from same colony in view range.</returns>
        internal int ColonyAntsInViewRange
        {
            get => colonyCount;
            set => colonyCount = value;
        }

        /// <summary>
        /// Number of ants from the same colony and same caste in view range of the insect.
        /// </summary>
        /// <returns>Number of friends from same colony and caste in view range.</returns>
        internal int CasteAntsInViewRange
        {
            get => casteAntsCount;
            set => casteAntsCount = value;
        }

        /// <summary>
        /// Number of team ants in view range.
        /// </summary>
        /// <returns>Number of team ants in view range.</returns>
        internal int TeamAntsInViewRange
        {
            get => teamCount;
            set => teamCount = value;
        }

        /// <summary>
        /// Direction the insect is turned to.
        /// </summary>
        /// <returns>Direction as integer.</returns>
        internal int GetDirectionCoreInsect()
        {
            return Coordinate.Direction;
        }

        /// <summary>
        /// Travelled distance of ant since last stop in anthill in internal unit.
        /// </summary>
        /// <returns>Travelled distance in internal unit.</returns>
        internal int NumberStepsWalked
        {
            get => numberStepsWalkedI;
            set => numberStepsWalkedI = value;
        }

        /// <summary>
        /// Distance to destination.
        /// After walking this distance the insect will turn to target or it will be waiting for commands.
        /// </summary>
        internal int DistanceToDestinationCoreInsect => distanceToDestinationI / SimulationEnvironment.PLAYGROUND_UNIT;

        /// <summary>
        /// In internal units: distance to destination
        /// After walking this distance the insect will be turn to target or it will be waiting for commands.
        /// </summary>
        internal int DistanceToDestination
        {
            get => distanceToDestinationI; 
            set => distanceToDestinationI = value;
        }

        /// <summary>
        /// Residual angle to turn before insect can go forward again.
        /// </summary>
        internal int ResidualAngle
        {
            get => residualAngle;
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
        internal ICoordinate DestinationCoreInsect
        {
            get => destination;
            set
            {
                if (destination == value && value != null) return;
                destination = value;
                residualAngle = 0;
                distanceToDestinationI = 0;
            }
        }

        /// <summary>
        /// Distance to ant hill in simulation steps.
        /// </summary>
        internal int DistanceToAnthillCoreInsect
        {
            get
            {
                int currentDistance;
                int rememberedDistance = int.MaxValue;
                foreach (CoreAnthill anthill in Colony.AntHills)
                {
                    currentDistance = CoreCoordinate.DetermineDistanceI(CoordinateCoreInsect, anthill.CoordinateCoreInsect);
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
        internal CoreFruit CarryingFruitCoreInsect
        {
            get => carryingFruit;
            set => carryingFruit = value;
        }

        /// <summary>
        /// Defines whether the insect has reached the destination.
        /// </summary>
        internal bool ArrivedCoreInsect => arrived;

        internal Random RandomCoreInsect { get; private set; }

        #region ICoordinate

        /// <summary>
        /// Coordinates of insect on playground.
        /// </summary>
        public CoreCoordinate CoordinateCoreInsect
        {
            get => Coordinate;
            internal set => Coordinate = value;
        }

        #endregion

        /// <summary>
        /// Abstract insect constructor.
        /// </summary>
        /// <param name="colony">New insect will be part of this colony.</param>
        /// <param name="random">Random.</param>
        /// <param name="insectsInColony">In constructor unused!</param>
        internal virtual void Init(CoreColony colony, Random random, Dictionary<string, int> insectsInColony)
        {
            Id = newId;
            newId++;

            this.Colony = colony;
            this.RandomCoreInsect = random;

            Coordinate.Direction = RandomCoreInsect.Next(0, 359);

            // Place randomly on the edge of the playground.
            if (colony.AntHills.Count == 0) // Place on the upper or lower edge.
            {
                if (RandomCoreInsect.Next(2) == 0)
                {
                    Coordinate.X = RandomCoreInsect.Next(0, colony.Playground.Width);
                    Coordinate.X *= SimulationEnvironment.PLAYGROUND_UNIT;
                    if (RandomCoreInsect.Next(2) == 0)
                    {
                        Coordinate.Y = 0;
                    }
                    else
                    {
                        Coordinate.Y = colony.Playground.Height * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                }

                // Place on left or right edge.
                else
                {
                    if (RandomCoreInsect.Next(2) == 0)
                    {
                        Coordinate.X = 0;
                    }
                    else
                    {
                        Coordinate.X = colony.Playground.Width * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                    Coordinate.Y = RandomCoreInsect.Next(0, colony.Playground.Height);
                    Coordinate.Y *= SimulationEnvironment.PLAYGROUND_UNIT;
                }
            }

            // Place a random ant hill.
            else
            {
                int i = RandomCoreInsect.Next(colony.AntHills.Count);
                Coordinate.X = colony.AntHills[i].CoordinateCoreInsect.X +
                               SimulationEnvironment.Cosinus(
                                   colony.AntHills[i].CoordinateCoreInsect.Radius, Coordinate.Direction);
                Coordinate.Y = colony.AntHills[i].CoordinateCoreInsect.Y +
                               SimulationEnvironment.Sinus(
                                   colony.AntHills[i].CoordinateCoreInsect.Radius, Coordinate.Direction);
            }
        }

        /// <summary>
        /// Defines whether the insect needs support to carry fruit.
        /// </summary>
        /// <param name="fruit">Fruit</param>
        /// <returns></returns>
        internal bool NeedSupport(CoreFruit fruit)
        {
            return fruit.NeedSupport(Colony);
        }

        /// <summary>
        /// The insect rotates for given degrees
        /// maximum rotation is 180 degree to the right
        /// this is 180 degrees to the left
        /// greater values will be cut off.
        /// </summary>
        /// <param name="angle">angle</param>
        internal void TurnByDegreesCoreInsect(int angle)
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
        internal void TurnToDirectionCoreInsect(int direction)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TurnIntoDirection(direction);
        }

        private void TurnIntoDirection(int direction)
        {
            ResidualAngle = direction - Coordinate.Direction;
        }

        /// <summary>
        /// Insect rotates towards a given target.
        /// </summary>
        /// <param name="target">Target</param>
        internal void TurnToTargetCoreInsect(ICoordinate target)
        {
            TurnToDirectionCoreInsect(CoreCoordinate.DetermineDirection(this, target));
        }

        /// <summary>
        /// The insect turns for 180 degree. 
        /// </summary>
        internal void TurnAroundCoreInsect()
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
        internal void GoForwardCoreInsect()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToDestinationI = int.MaxValue;
        }

        /// <summary>
        /// Insect will go forward for given simulation steps.
        /// </summary>
        /// <param name="distance">distance in simulation steps</param>
        internal void GoForwardCoreInsect(int distance)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToDestinationI = distance * SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// Insect goes to target. Target can be moving.
        /// Target will be attacked if it is a bug.
        /// </summary>
        /// <param name="target">target</param>
        internal void GoToTargetCoreInsect(ICoordinate target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            DestinationCoreInsect = target;
        }

        /// <summary>
        /// Attacks a target. Target can be moving.
        /// Only bugs can be the target for this overload.
        /// </summary>
        /// <param name="target">target</param>
        internal void AttackCoreInsect(CoreInsect target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            DestinationCoreInsect = target;
        }

        /// <summary>
        /// Insect goes away from source. Usually this is used
        /// to flee from another insect
        /// </summary>
        /// <param name="source">source</param> 
        internal void GoAwayFromCoreInsect(ICoordinate source)
        {
            TurnToDirectionCoreInsect(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardCoreInsect();
        }

        /// <summary>
        /// Insect goes away from source for a given distance in simulation steps.
        /// Usually used to flee from another insect.
        /// </summary>
        /// <param name="source">source</param> 
        /// <param name="distance">distance in simulation steps</param>
        internal void GoAwayFromCoreInsect(ICoordinate source, int distance)
        {
            TurnToDirectionCoreInsect(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardCoreInsect(distance);
        }

        /// <summary>
        /// Insect will go to nearest colony's anthill.
        /// </summary>
        internal void GoToAnthillCoreInsect()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            int currentDistance;
            int rememberedDistance = int.MaxValue;
            CoreAnthill rememberedAnthill = null;
            foreach (CoreAnthill anthill in Colony.AntHills)
            {
                currentDistance = CoreCoordinate.DetermineDistanceI(CoordinateCoreInsect, anthill.CoordinateCoreInsect);
                if (currentDistance < rememberedDistance)
                {
                    rememberedAnthill = anthill;
                    rememberedDistance = currentDistance;
                }
            }
            GoToTargetCoreInsect(rememberedAnthill);
        }

        /// <summary>
        /// Insect stops and forgets target or destination.
        /// </summary>
        internal void StopMovementCoreInsect()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            DestinationCoreInsect = null;
            distanceToDestinationI = 0;
            residualAngle = 0;
        }

        /// <summary>
        /// Insect takes sugar from sugar pile.
        /// </summary>
        /// <param name="sugar">sugar</param>
        internal void TakeCoreInsect(CoreSugar sugar)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            int distance = CoreCoordinate.DetermineDistanceI(CoordinateCoreInsect, sugar.CoordinateCoreInsect);
            if (distance <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                int amount = Math.Min(MaximumLoadCoreInsect - currentLoad, sugar.Amount);
                CurrentLoadCoreInsect += amount;
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
        internal void TakeCoreInsect(CoreFruit fruit)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            if (CarryingFruitCoreInsect == fruit)
            {
                return;
            }
            if (CarryingFruitCoreInsect != null)
            {
                DropFood();
            }
            int distance = CoreCoordinate.DetermineDistanceI(CoordinateCoreInsect, fruit.CoordinateCoreInsect);
            if (distance <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                StopMovementCoreInsect();
                CarryingFruitCoreInsect = fruit;
                fruit.InsectsCarrying.Add(this);
                CurrentLoadCoreInsect = Colony.LoadI[CasteIndexCoreInsect];
            }
        }

        /// <summary>
        /// Insect drops food and forgets target.
        /// </summary>
        internal void DropFood()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            CurrentLoadCoreInsect = 0;
            DestinationCoreInsect = null;
            if (CarryingFruitCoreInsect != null)
            {
                CarryingFruitCoreInsect.InsectsCarrying.Remove(this);
                CarryingFruitCoreInsect = null;
            }
        }

        /// <summary>
        /// Insect sets marker with given information and size.
        /// Bigger marks have shorter lifespan.
        /// </summary>
        /// <param name="information">information</param>
        /// <param name="size">size in simulation steps</param>
        internal void MakeMarkerCoreInsects(int information, int size)
        {
            if (!AwaitingCommands)
            {
                return;
            }

            // Check for unsupported marker size.
            if (size < 0)
            {
                throw new AiException(string.Format("{0}: {1}", Colony.Player.Guid,
                    Resource.SimulationCoreNegativeMarkerSize));
            }

            CoreMarker marker = new CoreMarker(Coordinate, size, Colony.Id);
            marker.Information = information;
            Colony.NewMarker.Add(marker);
            SmelledMarker.Add(marker);
        }

        /// <summary>
        /// Lets the ant spray a mark. The marker contains the information and does not spread.
        /// Thus, the mark has the maximum life span.
        /// </summary>
        /// <param name="information">The information.</param>
        internal void MakeMarkerCoreInsects(int information)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            MakeMarkerCoreInsects(information, 0);
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
                if (Math.Abs(residualAngle) < Colony.RotationSpeedI[CasteIndexCoreInsect])
                {
                    Coordinate.Direction += residualAngle;
                    residualAngle = 0;
                }

                // Insect rotates to the right.
                else if (residualAngle >= Colony.RotationSpeedI[CasteIndexCoreInsect])
                {
                    Coordinate.Direction += Colony.RotationSpeedI[CasteIndexCoreInsect];
                    ResidualAngle -= Colony.RotationSpeedI[CasteIndexCoreInsect];
                }

                // Insect rotates to the left.
                else if (residualAngle <= -Colony.RotationSpeedI[CasteIndexCoreInsect])
                {
                    Coordinate.Direction -= Colony.RotationSpeedI[CasteIndexCoreInsect];
                    ResidualAngle += Colony.RotationSpeedI[CasteIndexCoreInsect];
                }
            }

            // Insect walks.
            else if (distanceToDestinationI > 0)
            {
                if (CarryingFruitCoreInsect == null)
                {
                    int distance = Math.Min(distanceToDestinationI, currentSpeedICoreInsect);

                    distanceToDestinationI -= distance;
                    numberStepsWalkedI += distance;
                    Coordinate.X += SimulationEnvironment.Cos[distance, Coordinate.Direction];
                    Coordinate.Y += SimulationEnvironment.Sin[distance, Coordinate.Direction];
                }
            }

            // Insect walks towards target.
            else if (destination != null)
            {
                int distanceI;

                if (DestinationCoreInsect is CoreMarker)
                {
                    distanceI = CoreCoordinate.DetermineDistanceToCenter(Coordinate, destination.CoordinateCoreInsect);
                }
                else
                {
                    distanceI = CoreCoordinate.DetermineDistanceI(Coordinate, destination.CoordinateCoreInsect);
                }

                arrived = distanceI <= SimulationEnvironment.PLAYGROUND_UNIT;
                if (!arrived)
                {
                    int direction = CoreCoordinate.DetermineDirection(Coordinate, destination.CoordinateCoreInsect);

                    // Target is in sight or insect is carrying fruit.
                    if (distanceI < Colony.ViewRangeI[CasteIndexCoreInsect] || carryingFruit != null)
                    {
                        distanceToDestinationI = distanceI;
                    }

                    // Otherwise randomize direction.
                    else
                    {
                        direction += RandomCoreInsect.Next(-18, 18);
                        distanceToDestinationI = Colony.ViewRangeI[CasteIndexCoreInsect];
                    }

                    TurnIntoDirection(direction);
                }
            }

            // Limit coordinates to the left.
            if (Coordinate.X < 0)
            {
                Coordinate.X = -Coordinate.X;
                if (Coordinate.Direction > 90 && Coordinate.Direction <= 180)
                {
                    Coordinate.Direction = 180 - Coordinate.Direction;
                }
                else if (Coordinate.Direction > 180 && Coordinate.Direction < 270)
                {
                    Coordinate.Direction = 540 - Coordinate.Direction;
                }
            }

            // Limit coordinates to the right.
            else if (Coordinate.X > Colony.WidthI)
            {
                Coordinate.X = Colony.WidthI2 - Coordinate.X;
                if (Coordinate.Direction >= 0 && Coordinate.Direction < 90)
                {
                    Coordinate.Direction = 180 - Coordinate.Direction;
                }
                else if (Coordinate.Direction > 270 && Coordinate.Direction < 360)
                {
                    Coordinate.Direction = 540 - Coordinate.Direction;
                }
            }

            // Limit coordinates at the top.
            if (Coordinate.Y < 0)
            {
                Coordinate.Y = -Coordinate.Y;
                if (Coordinate.Direction > 180 && Coordinate.Direction < 360)
                {
                    Coordinate.Direction = 360 - Coordinate.Direction;
                }
            }

            // Limit coordinates below.
            else if (Coordinate.Y > Colony.HeightI)
            {
                Coordinate.Y = Colony.HeightI2 - Coordinate.Y;
                if (Coordinate.Direction > 0 && Coordinate.Direction < 180)
                {
                    Coordinate.Direction = 360 - Coordinate.Direction;
                }
            }
        }

        #region Speed

        /// <summary>
        /// The current speed of the insect in the internal unit.
        /// </summary>
        internal int currentSpeedICoreInsect;

        /// <summary>
        /// The current speed of the insect in steps. When the insect
        /// carries its maximum load, its speed is halved.
        /// </summary>
        internal int CurrentSpeedCoreInsect => currentSpeedICoreInsect / SimulationEnvironment.PLAYGROUND_UNIT;

        /// <summary>
        /// The insect's maximum speed.
        /// </summary>
        internal int MaximumSpeedCoreInsect => Colony.SpeedI[CasteIndexCoreInsect] / SimulationEnvironment.PLAYGROUND_UNIT;

        #endregion

        #region RotationSpeed

        /// <summary>
        /// The insect's rotation speed in degrees per round.
        /// </summary>
        internal int RotationSpeedCoreInsect => Colony.RotationSpeedI[CasteIndexCoreInsect];

        #endregion

        #region Load

        private int currentLoad = 0;

        /// <summary>
        /// The insect's current load
        /// </summary>
        internal int CurrentLoadCoreInsect
        {
            get => currentLoad;
            set
            {
                currentLoad = value >= 0 ? value : 0;
                currentSpeedICoreInsect = Colony.SpeedI[CasteIndexCoreInsect];
                currentSpeedICoreInsect -= currentSpeedICoreInsect * currentLoad / Colony.LoadI[CasteIndexCoreInsect] / 2;
            }
        }

        /// <summary>
        /// The insect's maximum load.
        /// </summary>
        internal int MaximumLoadCoreInsect => Colony.LoadI[CasteIndexCoreInsect];

        #endregion

        #region ViewRange

        /// <summary>
        /// Insect's view range in simulation steps.
        /// </summary>
        internal int ViewRangeCoreInsect => Colony.ViewRangeI[CasteIndexCoreInsect] / SimulationEnvironment.PLAYGROUND_UNIT;

        /// <summary>
        /// The insect's view range in internal unit.
        /// </summary>
        internal int ViewRangeI => Colony.ViewRangeI[CasteIndexCoreInsect];

        #endregion

        #region Range

        /// <summary>
        /// The insect's range in simulation steps.
        /// </summary>
        internal int RangeCoreInsect => Colony.RangeI[CasteIndexCoreInsect] / SimulationEnvironment.PLAYGROUND_UNIT;

        /// <summary>
        /// The insect's range in internal units.
        /// </summary>
        internal int RangeI => Colony.RangeI[CasteIndexCoreInsect];

        #endregion

        #region Energy

        private int currentEnergy;

        /// <summary>
        /// The insect's current energy.
        /// </summary>
        internal int CurrentEnergyCoreInsect
        {
            get => currentEnergy;
            set => currentEnergy = value >= 0 ? value : 0;
        }

        /// <summary>
        /// The insect's maximum energy.
        /// </summary>
        internal int MaximumEnergyCoreInsect => Colony.EnergyI[CasteIndexCoreInsect];

        #endregion

        #region Attack

        private int attackStrength;

        /// <summary>
        /// The insect's attack strength.
        /// Zero, if insect carries load.
        /// </summary>
        internal int AttackStrengthCoreInsect
        {
            get => currentLoad == 0 ? attackStrength : 0;
            set => attackStrength = value >= 0 ? value : 0;
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