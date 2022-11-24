using System;
using System.Collections.Generic;
using System.Threading;

namespace AntMe.Simulation
{
    /// <summary>
    /// abstract base class of all insects
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreInsect : ICoordinate
    {
        /// <summary>
        /// id for newly created insect
        /// </summary>
        private static int newId = 0;

        /// <summary>
        /// list of all markers the insect has spoted
        /// prevents insects to run in circles 
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
        /// id of the insect. It will identify the insect throughout the game
        /// </summary>
        internal int id;

        /// <summary>
        /// coordinates of the insect on the playground
        /// </summary>
        internal CoreCoordinate coordinate;

        /// <summary>
        /// defines whether the insect is awaiting commands
        /// </summary>
        internal bool AwaitingCommands = false;

        private int distanceToDestination;
        private int residualAngle = 0;

        /// <summary>
        /// depending on the players caste structure, index of the caste
        /// </summary>
        internal int CasteIndexBase;

        /// <summary>
        /// colony the insect is belonging to
        /// </summary>
        internal CoreColony colony;

        private ICoordinate target = null;
        private int travelledDistanceI;

        internal CoreInsect() { }

        /// <summary>
        /// Caste of the insect
        /// </summary>
        internal string CasteBase
        {
            get { return colony.Player.Castes[CasteIndexBase].Name; }
        }

        /// <summary>
        /// number of bugs in view range of the insect
        /// </summary>
        internal int BugsInViewrange
        {
            get { return bugCount; }
            set { bugCount = value; }
        }

        /// <summary>
        /// number of enemy ants in view range of the insect
        /// </summary>
        internal int ForeignAntsInViewrange
        {
            get { return antCount; }
            set { antCount = value; }
        }

        /// <summary>
        /// number of friendly ants in view range of the insect
        /// </summary>
        internal int FriendlyAntsInViewrange
        {
            get { return colonyCount; }
            set { colonyCount = value; }
        }

        /// <summary>
        /// number of friendly ants of same caste in view range of the insect
        /// </summary>
        internal int FriendlyAntsFromSameCasteInViewrange
        {
            get { return casteCount; }
            set { casteCount = value; }
        }

        /// <summary>
        /// number of team ants in view range
        /// </summary>
        internal int TeamAntsInViewrange
        {
            get { return teamCount; }
            set { teamCount = value; }
        }

        /// <summary>
        /// direction the insect is turned to
        /// </summary>
        internal int DirectionBase
        {
            get { return coordinate.Direction; }
        }

        /// <summary>
        /// travelled distance of ant since last stop in anthill
        /// </summary>
        internal int travelledDistanceBase
        {
            get { return travelledDistanceI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// in internal units: travelled distance of ant since last stop in anthill
        /// </summary>
        internal int travelledDistance
        {
            get { return travelledDistanceI; }
            set { travelledDistanceI = value; }
        }

        /// <summary>
        /// distance to destination
        /// after this distance the insect will be turn to target or it will be waiting for commands
        /// </summary>
        internal int DistanceToDestinationBase
        {
            get { return distanceToDestination / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// in internal units: distance to destination
        /// after this distance the insect will be turn to target or it will be waiting for commands
        /// </summary>
        internal int DistanceToDestination
        {
            get { return distanceToDestination; }
            set { distanceToDestination = value; }
        }

        /// <summary>
        /// residual angle to turn before insect can go forward again
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
        /// insects target
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
        /// distance to anthill in simulation steps
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
        /// carried fruit
        /// </summary>
        internal CoreFruit CarryingFruitBase
        {
            get { return carryingFruit; }
            set { carryingFruit = value; }
        }

        /// <summary>
        /// defines whether the insect has reached the destination
        /// </summary>
        internal bool ArrivedBase
        {
            get { return arrived; }
        }

        internal Random RandomBase { get; private set; }

        #region ICoordinate

        /// <summary>
        /// coordinates of insect on playground
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return coordinate; }
            internal set { coordinate = value; }
        }

        #endregion

        /// <summary>
        /// abstract insect constructor
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

            // Zufällig auf dem Spielfeldrand platzieren.
            if (colony.AntHills.Count == 0) // Am oberen oder unteren Rand platzieren.
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

                // Am linken oder rechten Rand platzieren.
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

            // In einem zufälligen Bau platzieren.
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
        /// defines whether the insect needs support to carry fruit
        /// </summary>
        /// <param name="fruit">Fruit</param>
        /// <returns></returns>
        internal bool NeedSupport(CoreFruit fruit)
        {
            return fruit.NeedSupport(colony);
        }

        /// <summary>
        /// insect rotates for given degrees
        /// maximum rotation is 180 degree to the right
        /// this is 180 degrees to the left
        /// greater values will be cut off
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
        /// Dreht das Insekt in die angegebene Richtung (Grad).
        /// </summary>
        /// <param name="direction">Richtung</param>
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
        /// Dreht das Insekt in die Richtung des angegebenen Ziels.
        /// </summary>
        /// <param name="target">Ziel</param>
        internal void TurnToTargetBase(ICoordinate target)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, target));
        }

        /// <summary>
        /// Dreht das Insekt um 180 Grad.
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
        /// insect will go forward without delimiter
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
        /// insect will go forward for given simulation steps
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
        /// insect goes to target, target can be moving
        /// target will be attacked if it is bug
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
        /// attacks a target
        /// target can be moving
        /// only bugs can be the target for this version
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
        /// insect goes away from source usually this is used to flee from another insect
        /// </summary>
        /// <param name="source">source</param> 
        internal void GoAwayFromBase(ICoordinate source)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardBase();
        }

        /// <summary>
        /// insect goes away from source for a given distance in simulation steps
        /// usually used to flee from another insect
        /// </summary>
        /// <param name="source">source</param> 
        /// <param name="distance">distance in simulation steps</param>
        internal void GoAwayFromBase(ICoordinate source, int distance)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoForwardBase(distance);
        }

        /// <summary>
        /// insect will go to nearest colony anthill
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
        /// insect stops and forgets target or destination
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
        /// insect takes sugar from sugar pile
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
                int amount = Math.Min(MaximumLoadBase - currentBurden, sugar.Amount);
                CurrentLoadBase += amount;
                sugar.Amount -= amount;
            }
            else
            {
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// insect takes fruit
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
        /// insect drops food and forgets target
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
        /// insect sets marker with given information and size
        /// bigger marks have shorter lifespan
        /// </summary>
        /// <param name="information">information</param>
        /// <param name="size">size in simulation steps</param>
        internal void MakeMarkerBase(int information, int size)
        {
            if (!AwaitingCommands)
            {
                return;
            }

            // Check for unsupported markersize
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
        /// Lässt die Ameise eine Markierung sprühen. Die Markierung enthält die
        /// angegebene Information und breitet sich nicht aus. So hat die Markierung
        /// die maximale Lebensdauer.
        /// </summary>
        /// <param name="information">Die Information.</param>
        internal void MakeMarkerBase(int information)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            MakeMarkerBase(information, 0);
        }

        /// <summary>
        /// Berechnet die Bewegung des Insekts.
        /// </summary>
        internal void Move()
        {
            arrived = false;

            // Insekt dreht sich.
            if (residualAngle != 0)
            {
                // Zielwinkel wird erreicht.
                if (Math.Abs(residualAngle) < colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction += residualAngle;
                    residualAngle = 0;
                }

                // Insekt dreht sich nach rechts.
                else if (residualAngle >= colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction += colony.RotationSpeedI[CasteIndexBase];
                    ResidualAngle -= colony.RotationSpeedI[CasteIndexBase];
                }

                // Insekt dreht sich nach links.
                else if (residualAngle <= -colony.RotationSpeedI[CasteIndexBase])
                {
                    coordinate.Direction -= colony.RotationSpeedI[CasteIndexBase];
                    ResidualAngle += colony.RotationSpeedI[CasteIndexBase];
                }
            }

            // Insekt geht.
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

            // Insekt geht auf Ziel zu.
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

                    // Ziel ist in Sichtweite oder Insekt trägt Obst.
                    if (distanceI < colony.ViewRangeI[CasteIndexBase] || carryingFruit != null)
                    {
                        distanceToDestination = distanceI;
                    }

                    // Ansonsten Richtung verfälschen.
                    else
                    {
                        direction += RandomBase.Next(-18, 18);
                        distanceToDestination = colony.ViewRangeI[CasteIndexBase];
                    }

                    turnIntoDirection(direction);
                }
            }

            // Koordinaten links begrenzen.
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

            // Koordinaten rechts begrenzen.
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

            // Koordinaten oben begrenzen.
            if (coordinate.Y < 0)
            {
                coordinate.Y = -coordinate.Y;
                if (coordinate.Direction > 180 && coordinate.Direction < 360)
                {
                    coordinate.Direction = 360 - coordinate.Direction;
                }
            }

            // Koordinaten unten begrenzen.
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
        /// Die aktuelle Geschwindigkeit des Insekts in der internen Einheit.
        /// </summary>
        internal int currentSpeedI;

        /// <summary>
        /// Die aktuelle Geschwindigkeit des Insekts in Schritten. Wenn das Insekt
        /// seine maximale Last trägt, halbiert sich seine Geschwindigkeit.
        /// </summary>
        internal int CurrentSpeedBase
        {
            get { return currentSpeedI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die maximale Geschwindigkeit des Insekts.
        /// </summary>
        internal int MaximumSpeedBase
        {
            get { return colony.SpeedI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        #endregion

        #region RotationSpeed

        /// <summary>
        /// Die Drehgeschwindigkeit des Insekts in Grad pro Runde.
        /// </summary>
        internal int RotationSpeedBase
        {
            get { return colony.RotationSpeedI[CasteIndexBase]; }
        }

        #endregion

        #region Last

        private int currentBurden = 0;

        /// <summary>
        /// current burden carried by ant
        /// </summary>
        internal int CurrentLoadBase
        {
            get { return currentBurden; }
            set
            {
                currentBurden = value >= 0 ? value : 0;
                currentSpeedI = colony.SpeedI[CasteIndexBase];
                currentSpeedI -= currentSpeedI * currentBurden / colony.LoadI[CasteIndexBase] / 2;
            }
        }

        /// <summary>
        /// maximum burden insect can carry.
        /// </summary>
        internal int MaximumLoadBase
        {
            get { return colony.LoadI[CasteIndexBase]; }
        }

        #endregion

        #region ViewRange

        /// <summary>
        /// view range of insect in simulation steps
        /// </summary>
        internal int ViewRangeBase
        {
            get { return colony.ViewRangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// view range of insect in internal unit
        /// </summary>
        internal int ViewRangeI
        {
            get { return colony.ViewRangeI[CasteIndexBase]; }
        }

        #endregion

        #region Range

        /// <summary>
        /// range of insect in simulation steps
        /// </summary>
        internal int RangeBase
        {
            get { return colony.RangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// range of insect in internal units
        /// </summary>
        internal int RangeI
        {
            get { return colony.RangeI[CasteIndexBase]; }
        }

        #endregion

        #region Energy

        private int currentEngergy;

        /// <summary>
        /// current energy of insect
        /// </summary>
        internal int currentEnergyBase
        {
            get { return currentEngergy; }
            set { currentEngergy = value >= 0 ? value : 0; }
        }

        /// <summary>
        /// maximum energy of insect
        /// </summary>
        internal int MaximumEnergyBase
        {
            get { return colony.EnergyI[CasteIndexBase]; }
        }

        #endregion

        #region Attack

        private int attackStrength;

        /// <summary>
        /// attack strength of insect
        /// zero, if insect carries burden
        /// </summary>
        internal int AttackStrengthBase
        {
            get { return currentBurden == 0 ? attackStrength : 0; }
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