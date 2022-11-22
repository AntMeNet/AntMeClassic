using System;
using System.Collections.Generic;
using System.Threading;

namespace AntMe.Simulation
{
    /// <summary>
    /// Abstrakte Basisklasse für alle Insekten.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreInsect : ICoordinate
    {
        /// <summary>
        /// Die Id des nächste erzeugten Insekts.
        /// </summary>
        private static int newId = 0;

        /// <summary>
        /// Speichert die Markierungen, die das Insekt schon gesehen hat. Das
        /// verhindert, daß das Insekt zwischen Markierungen im Kreis läuft.
        /// </summary>
        internal readonly List<CoreMarker> SmelledMarker = new List<CoreMarker>();

        private bool reached = false;
        private int antCount = 0;
        private int casteCount = 0;
        private int colonyCount = 0;
        private int bugCount = 0;
        private int teamCount = 0;
        private CoreFruit carryingFruit;

        /// <summary>
        /// Die Id die das Insekt während eines Spiels eindeutig indentifiziert.
        /// </summary>
        internal int id;

        /// <summary>
        /// Die Position des Insekts auf dem Spielfeld.
        /// </summary>
        internal CoreCoordinate coordinate;

        /// <summary>
        /// Legt fest, ob das Insekt Befehle entgegen nimmt.
        /// </summary>
        internal bool AwaitingCommands = false;

        private int distanceToGo;
        private int angleToTurn = 0;

        /// <summary>
        /// Der Index der Kaste des Insekts in der Kasten-Struktur des Spielers.
        /// </summary>
        internal int CasteIndexBase;

        /// <summary>
        /// Das Volk zu dem das Insekts gehört.
        /// </summary>
        internal CoreColony colony;

        private ICoordinate target = null;
        private int zurückgelegteStreckeI;

        internal CoreInsect() { }

        /// <summary>
        /// Die Kaste des Insekts.
        /// </summary>
        internal string CasteBase
        {
            get { return colony.Player.Castes[CasteIndexBase].Name; }
        }

        /// <summary>
        /// Die Anzahl von Wanzen in Sichtweite des Insekts.
        /// </summary>
        internal int BugsInViewrange
        {
            get { return bugCount; }
            set { bugCount = value; }
        }

        /// <summary>
        /// Die Anzahl feindlicher Ameisen in Sichtweite des Insekts.
        /// </summary>
        internal int ForeignAntsInViewrange
        {
            get { return antCount; }
            set { antCount = value; }
        }

        /// <summary>
        /// Die Anzahl befreundeter Ameisen in Sichtweite des Insekts.
        /// </summary>
        internal int FriendlyAntsInViewrange
        {
            get { return colonyCount; }
            set { colonyCount = value; }
        }

        /// <summary>
        /// Die Anzahl befreundeter Ameisen der selben Kaste in Sichtweite des
        /// Insekts.
        /// </summary>
        internal int FriendlyAntsFromSameCasteInViewrange
        {
            get { return casteCount; }
            set { casteCount = value; }
        }

        /// <summary>
        /// Anzahl Ameisen aus befreundeten Völkern in sichtweite des Insekts.
        /// </summary>
        internal int TeamAntsInViewrange
        {
            get { return teamCount; }
            set { teamCount = value; }
        }

        /// <summary>
        /// Die Richtung in die das Insekt gedreht ist.
        /// </summary>
        internal int DirectionBase
        {
            get { return coordinate.Direction; }
        }

        /// <summary>
        /// Die Strecke die die Ameise zurückgelegt hat, seit sie das letzte Mal in
        /// einem Ameisenbau war.
        /// </summary>
        internal int WalkedRangeBase
        {
            get { return zurückgelegteStreckeI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Strecke die die Ameise zurückgelegt hat, seit sie das letzte Mal in
        /// einem Ameisenbau war in der internen Einheit.
        /// </summary>
        internal int walkedDistance
        {
            get { return zurückgelegteStreckeI; }
            set { zurückgelegteStreckeI = value; }
        }

        /// <summary>
        /// Die Strecke die das Insekt geradeaus gehen wird, bevor das nächste Mal
        /// Wartet() aufgerufen wird bzw. das Insekt sich zu seinem Ziel ausrichtet.
        /// </summary>
        internal int DistanceToDestinationBase
        {
            get { return distanceToGo / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Strecke die das Insekt geradeaus gehen wird, bevor das nächste 
        /// Mal Wartet() aufgerufen wird bzw. das Insekt sich zu seinem Ziel
        /// ausrichtet in der internen Einheit.
        /// </summary>
        internal int RestStreckeI
        {
            get { return distanceToGo; }
            set { distanceToGo = value; }
        }

        /// <summary>
        /// Der Winkel um den das Insekt sich noch drehen muß, bevor es wieder
        /// geradeaus gehen kann.
        /// </summary>
        internal int angleToGo
        {
            get { return angleToTurn; }
            set
            {
                // TODO: Modulo?
                angleToTurn = value;
                while (angleToTurn > 180)
                {
                    angleToTurn -= 360;
                }
                while (angleToTurn < -180)
                {
                    angleToTurn += 360;
                }
            }
        }

        /// <summary>
        /// Das Ziel auf das das Insekt zugeht.
        /// </summary>
        internal ICoordinate TargetBase
        {
            get { return target; }
            set
            {
                if (target != value || value == null)
                {
                    target = value;
                    angleToTurn = 0;
                    distanceToGo = 0;
                }
            }
        }

        /// <summary>
        /// Liefert die Entfernung in Schritten zum nächsten Ameisenbau.
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
        /// Gibt das Obst zurück, das das Insekt gerade trägt.
        /// </summary>
        internal CoreFruit CarryingFruitBase
        {
            get { return carryingFruit; }
            set { carryingFruit = value; }
        }

        /// <summary>
        /// Gibt zurück on das Insekt bei seinem Ziel angekommen ist.
        /// </summary>
        internal bool ReachedBase
        {
            get { return reached; }
        }

        internal Random RandomBase { get; private set; }

        #region IKoordinate Members

        /// <summary>
        /// Die Position des Insekts auf dem Spielfeld.
        /// </summary>
        public CoreCoordinate CoordinateBase
        {
            get { return coordinate; }
            internal set { coordinate = value; }
        }

        #endregion

        /// <summary>
        /// Der abstrakte Insekt-Basiskonstruktor.
        /// </summary>
        /// <param name="colony">Das Volk zu dem das neue Insekt gehört.</param>
        /// <param name="insectsInColony">Hier unbenutzt!</param>
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
        /// Gibt an, ob weitere Insekten benötigt werden, um ein Stück Obst zu
        /// tragen.
        /// </summary>
        /// <param name="fruit">Obst</param>
        /// <returns></returns>
        internal bool NeedSupport(CoreFruit fruit)
        {
            return fruit.NeedSupport(colony);
        }

        /// <summary>
        /// Dreht das Insekt um den angegebenen Winkel. Die maximale Drehung beträgt
        /// -180 Grad (nach links) bzw. 180 Grad (nach rechts). Größere Werte werden
        /// abgeschnitten.
        /// </summary>
        /// <param name="angle">Winkel</param>
        internal void TurnByAngleBase(int angle)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            angleToGo = angle;
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
            angleToGo = direction - coordinate.Direction;
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
            if (angleToTurn > 0)
            {
                angleToTurn = 180;
            }
            else
            {
                angleToTurn = -180;
            }
        }

        /// <summary>
        /// Lässt das Insekt unbegrenzt geradeaus gehen.
        /// </summary>
        internal void GoStraightAheadBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToGo = int.MaxValue;
        }

        /// <summary>
        /// Lässt das Insekt die angegebene Entfernung in Schritten geradeaus gehen.
        /// </summary>
        /// <param name="distance">Die Entfernung.</param>
        internal void GoStraightAheadBase(int distance)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            distanceToGo = distance * SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// Lässt das Insekt auf ein Ziel zugehen. Das Ziel darf sich bewegen.
        /// Wenn das Ziel eine Wanze ist, wird dieser angegriffen.
        /// </summary>
        /// <param name="target">Das Ziel.</param>
        internal void GoToTargetBase(ICoordinate target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = target;
        }

        /// <summary>
        /// Lässt das Insekt ein Ziel angreifen. Das Ziel darf sich bewegen.
        /// In der aktuellen Version kann das Ziel nur eine Wanze sein.
        /// </summary>
        /// <param name="target">Ziel</param>
        internal void AttackBase(CoreInsect target)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = target;
        }

        /// <summary>
        /// Lässt das Insekt von der aktuellen Position aus entgegen der Richtung zu
        /// einer Quelle gehen. Wenn die Quelle ein Insekt eines anderen Volkes ist,
        /// befindet sich das Insekt auf der Flucht.
        /// </summary>
        /// <param name="source">Die Quelle.</param> 
        internal void GoAwayFromBase(ICoordinate source)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoStraightAheadBase();
        }

        /// <summary>
        /// Lässt das Insekt von der aktuellen Position aus die angegebene
        /// Entfernung in Schritten entgegen der Richtung zu einer Quelle gehen.
        /// Wenn die Quelle ein Insekt eines anderen Volkes ist, befindet sich das
        /// Insekt auf der Flucht.
        /// </summary>
        /// <param name="source">Die Quelle.</param> 
        /// <param name="distance">Die Entfernung in Schritten.</param>
        internal void GoAwayFromBase(ICoordinate source, int distance)
        {
            TurnIntoDirectionBase(CoreCoordinate.DetermineDirection(this, source) + 180);
            GoStraightAheadBase(distance);
        }

        /// <summary>
        /// Lässt das Insekt zum nächsten Bau gehen.
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
        /// Lässt das Insekt anhalten. Dabei geht sein Ziel verloren.
        /// </summary>
        internal void StopMovementBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            TargetBase = null;
            distanceToGo = 0;
            angleToTurn = 0;
        }

        /// <summary>
        /// Lässt das Insekt Zucker von einem Zuckerhaufen nehmen.
        /// </summary>
        /// <param name="sugar">Zuckerhaufen</param>
        internal void TakeBase(CoreSugar sugar)
        {
            if (!AwaitingCommands)
            {
                return;
            }
            int distance = CoreCoordinate.DetermineDistanceI(CoordinateBase, sugar.CoordinateBase);
            if (distance <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                int amount = Math.Min(MaximumBurdenBase - currentBurden, sugar.Amount);
                CurrentBurdenBase += amount;
                sugar.Amount -= amount;
            }
            else
            {
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// Lässt das Insekt ein Obststück nehmen.
        /// </summary>
        /// <param name="fruit">Das Obststück.</param>
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
                CurrentBurdenBase = colony.Burden[CasteIndexBase];
            }
        }

        /// <summary>
        /// Lässt das Insekt die aktuell getragene Nahrung fallen. Das Ziel des
        /// Insekts geht dabei verloren.
        /// </summary>
        internal void DropFoodBase()
        {
            if (!AwaitingCommands)
            {
                return;
            }
            CurrentBurdenBase = 0;
            TargetBase = null;
            if (CarryingFruitBase != null)
            {
                CarryingFruitBase.InsectsCarrying.Remove(this);
                CarryingFruitBase = null;
            }
        }

        /// <summary>
        /// Lässt die Ameise eine Markierung sprühen. Die Markierung enthält die
        /// angegebene Information und breitet sich um die angegebene Anzahl an
        /// Schritten weiter aus. Je weiter sich eine Markierung ausbreitet, desto
        /// kürzer bleibt sie aktiv.
        /// </summary>
        /// <param name="information">Die Information.</param>
        /// <param name="size">Die Ausbreitung in Schritten.</param>
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
            reached = false;

            // Insekt dreht sich.
            if (angleToTurn != 0)
            {
                // Zielwinkel wird erreicht.
                if (Math.Abs(angleToTurn) < colony.RotationSpeed[CasteIndexBase])
                {
                    coordinate.Direction += angleToTurn;
                    angleToTurn = 0;
                }

                // Insekt dreht sich nach rechts.
                else if (angleToTurn >= colony.RotationSpeed[CasteIndexBase])
                {
                    coordinate.Direction += colony.RotationSpeed[CasteIndexBase];
                    angleToGo -= colony.RotationSpeed[CasteIndexBase];
                }

                // Insekt dreht sich nach links.
                else if (angleToTurn <= -colony.RotationSpeed[CasteIndexBase])
                {
                    coordinate.Direction -= colony.RotationSpeed[CasteIndexBase];
                    angleToGo += colony.RotationSpeed[CasteIndexBase];
                }
            }

            // Insekt geht.
            else if (distanceToGo > 0)
            {
                if (CarryingFruitBase == null)
                {
                    int distance = Math.Min(distanceToGo, currentSpeedI);

                    distanceToGo -= distance;
                    zurückgelegteStreckeI += distance;
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

                reached = distanceI <= SimulationEnvironment.PLAYGROUND_UNIT;
                if (!reached)
                {
                    int direction = CoreCoordinate.DetermineDirection(coordinate, target.CoordinateBase);

                    // Ziel ist in Sichtweite oder Insekt trägt Obst.
                    if (distanceI < colony.ViewRangeI[CasteIndexBase] || carryingFruit != null)
                    {
                        distanceToGo = distanceI;
                    }

                    // Ansonsten Richtung verfälschen.
                    else
                    {
                        direction += RandomBase.Next(-18, 18);
                        distanceToGo = colony.ViewRangeI[CasteIndexBase];
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
            else if (coordinate.Y > colony.HightI)
            {
                coordinate.Y = colony.HightI2 - coordinate.Y;
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
            get { return colony.RotationSpeed[CasteIndexBase]; }
        }

        #endregion

        #region Last

        private int currentBurden = 0;

        /// <summary>
        /// Die Last die die Ameise gerade trägt.
        /// </summary>
        internal int CurrentBurdenBase
        {
            get { return currentBurden; }
            set
            {
                currentBurden = value >= 0 ? value : 0;
                currentSpeedI = colony.SpeedI[CasteIndexBase];
                currentSpeedI -= currentSpeedI * currentBurden / colony.Burden[CasteIndexBase] / 2;
            }
        }

        /// <summary>
        /// Die maximale Last die das Insekt tragen kann.
        /// </summary>
        internal int MaximumBurdenBase
        {
            get { return colony.Burden[CasteIndexBase]; }
        }

        #endregion

        #region ViewRange

        /// <summary>
        /// Die Sichtweite des Insekts in Schritten.
        /// </summary>
        internal int ViewRangeBase
        {
            get { return colony.ViewRangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Sichtweite des Insekts in der internen Einheit.
        /// </summary>
        internal int ViewRangeI
        {
            get { return colony.ViewRangeI[CasteIndexBase]; }
        }

        #endregion

        #region Range

        /// <summary>
        /// Die Reichweite des Insekts in Schritten.
        /// </summary>
        internal int RangeBase
        {
            get { return colony.RangeI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Reichweite des Insekts in der internen Einheit.
        /// </summary>
        internal int RangeI
        {
            get { return colony.RangeI[CasteIndexBase]; }
        }

        #endregion

        #region Energy

        private int currentEngergy;

        /// <summary>
        /// Die verbleibende Energie des Insekts.
        /// </summary>
        internal int currentEnergyBase
        {
            get { return currentEngergy; }
            set { currentEngergy = value >= 0 ? value : 0; }
        }

        /// <summary>
        /// Die maximale Energie des Insetks.
        /// </summary>
        internal int MaximumEnergyBase
        {
            get { return colony.EnergyI[CasteIndexBase]; }
        }

        #endregion

        #region Attack

        private int attackStrength;

        /// <summary>
        /// Die Angriffstärke des Insekts. Wenn das Insekt Last trägt ist die
        /// Angriffstärke gleich Null.
        /// </summary>
        internal int AttackStrengthBase
        {
            get { return currentBurden == 0 ? attackStrength : 0; }
            set { attackStrength = value >= 0 ? value : 0; }
        }

        #endregion

        #region Debug

        internal string debugMessage;

        internal void DenkeCore(string message)
        {
            debugMessage = message.Length > 100 ? message.Substring(0, 100) : message;
        }

        #endregion
    }
}