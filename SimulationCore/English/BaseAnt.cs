using System;
using AntMe.Simulation;
using System.Collections.Generic;

namespace AntMe.English
{
    /// <summary>
    /// Baseclass for implementing english Ants.
    /// <see href="http://wiki.antme.net/en/Ant_Development">Read more</see>
    /// </summary>
    public abstract class BaseAnt : CoreAnt
    {
        #region Event wrapper

        internal override string DetermineCasteCoreAnt(Dictionary<string, int> number)
        {
            return ChooseCaste(number);
        }

        /// <summary>
        /// Every time a new ant is born, its caste must be set. You can do 
        /// so with the help of the value returned by this method.
        /// </summary>
        /// <param name="number">Dictionary with caste name and corresponding number of ants in this caste</param>
        /// <returns>Caste name for the next ant</returns>
        public virtual string ChooseCaste(Dictionary<string, int> number)
        {
            return string.Empty;
        }
        /// <summary>
        /// This method is called if an ant dies. It informs you that the ant has 
        /// died. The ant cannot undertake any more actions from that point forward.
        /// </summary>
        /// <param name="kindOfDeath">Kind of Death</param>
        public virtual void HasDied(KindOfDeath kindOfDeath) { }


        internal override void HasDiedCoreAnt(CoreKindOfDeath kindOfDeath)
        {
            HasDied((KindOfDeath)(int)kindOfDeath);
        }

        internal override void SpotsFriendCoreAnt(CoreMarker marker)
        {
            SpotsFriend(new Marker(marker));
        }

        /// <summary>
        /// Friendly ants can detect markers left by other ants. This method is 
        /// called when an ant smells a friendly marker for the first time.
        /// </summary>
        /// <param name="marker">Marker</param>
        public virtual void SpotsFriend(Marker marker) { }

        internal override void SpotsCoreAnt(CoreFruit fruit)
        {
            Spots(new Fruit(fruit));
        }

        /// <summary>
        /// This method is called as soon as an ant sees an apple within its 
        /// 360° visual range. The parameter is the piece of fruit that the 
        /// ant has spotted.
        /// </summary>
        /// <param name="fruit">spotted fruit</param>
        public virtual void Spots(Fruit fruit) { }

        internal override void SpotsCoreAnt(CoreSugar sugar)
        {
            Spots(new Sugar(sugar));
        }

        /// <summary>
        /// This method is called as soon as an ant sees a fruit hill in 
        /// its 360° visual range. The parameter is the fruit hill that 
        /// the ant has spotted.
        /// </summary>
        /// <param name="sugar">spotted fruit</param>
        public virtual void Spots(Sugar sugar) { }

        internal override void SpotsEnemyCoreAnt(CoreAnt ant)
        {
            SpotsEnemy(new Ant(ant));
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually 
        /// detect other game elements. This method is called if the ant detects 
        /// an ant from an enemy colony.
        /// </summary>
        /// <param name="ant">Spotted enemy ant.</param>
        public virtual void SpotsEnemy(Ant ant)
        {
        }

        internal override void SpotsEnemyCoreAnt(CoreBug bug)
        {
            SpotsEnemy(new Bug(bug));
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually 
        /// detect other game elements. This method is called if the ant sees 
        /// a bug.
        /// </summary>
        /// <param name="bug">spotted bug</param>
        public virtual void SpotsEnemy(Bug bug) { }

        internal override void SpotsFriendCoreAnt(CoreAnt ant)
        {
            SpotsFriend(new Ant(ant));
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually 
        /// detect other game elements. This method is called if the ant sees an 
        /// ant from the same colony.
        /// </summary>
        /// <param name="ant">spotted ant</param>
        public virtual void SpotsFriend(Ant ant) { }

        internal override void SpotsTeamMemberCoreAnt(CoreAnt ant)
        {
            SpotsTeammate(new Ant(ant));
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually 
        /// detect other game elements. This method is called if the ant detects 
        /// an ant from a friendly colony (an ant on the same team).
        /// </summary>
        /// <param name="ant">spotted ant</param>
        public virtual void SpotsTeammate(Ant ant) { }

        internal override void TickCoreAnt()
        {
            Tick();
        }

        /// <summary>
        /// This method is called in every simulation round, regardless of additional 
        /// conditions. It is ideal for actions that must be executed but that are not 
        /// addressed by other methods.
        /// </summary>
        public virtual void Tick() { }

        internal override void WaitingCoreAnt()
        {
            Waiting();
        }

        /// <summary>
        /// If the ant has no assigned tasks, it waits for new tasks. This method 
        /// is called to inform you that it is waiting.
        /// </summary>
        public virtual void Waiting() { }

        internal override void IsUnderAttackCoreAnt(CoreAnt ant)
        {
            UnderAttack(new Ant(ant));
        }

        /// <summary>
        /// Enemy creatures may actively attack the ant. This method is called if an 
        /// enemy ant attacks. the ant can then decide how to react.
        /// </summary>
        /// <param name="ant">attacking ant</param>
        public virtual void UnderAttack(Ant ant) { }

        internal override void UnderAttackCoreAnt(CoreBug bug)
        {
            UnderAttack(new Bug(bug));
        }

        /// <summary>
        /// Enemy creatures may actively attack the ant. This method is called if a 
        /// bug attacks. the ant can decide how to react.
        /// </summary>
        /// <param name="bug">attacking bug</param>
        public virtual void UnderAttack(Bug bug) { }

        internal override void IsGettingTiredCoreAnt()
        {
            GettingTired();
        }

        /// <summary>
        /// This method is called when an ant has travelled one third of its movement range.
        /// </summary>
        public virtual void GettingTired() { }

        internal override void ArrivedAtTargetCoreAnt(CoreFruit fruit)
        {
            DestinationReached(new Fruit(fruit));
        }

        /// <summary>
        /// If the ant’s destination is a piece of fruit, this method is called as soon 
        /// as the ant reaches its destination. It means that the ant is now near enough 
        /// to its destination/target to interact with it.
        /// </summary>
        /// <param name="fruit">reached fruit</param>
        public virtual void DestinationReached(Fruit fruit) { }

        internal override void ArrivedAtTargetCoreAnt(CoreSugar sugar)
        {
            DestinationReached(new Sugar(sugar));
        }

        /// <summary>
        /// If the ant’s destination is a sugar pile, this method is called as soon 
        /// as the ant has reached its destination. It means that the ant is now near 
        /// enough to its destination/target to interact with it.
        /// </summary>
        /// <param name="sugar">Reached sugar pile.</param>
        public virtual void DestinationReached(Sugar sugar) { }

        #endregion

        #region Command wrapper
        /// <summary>
        /// The ant moves forward. The ant’s destination remains unaltered. If a 
        /// value is specified, the ant will aim for its destination again as soon 
        /// as it has travelled the specified distance.
        /// </summary>
        public void GoForward()
        {
            GoForwardCoreInsect();
        }

        /// <summary>
        /// The ant moves forward. The ant’s destination remains unaltered. If a 
        /// value is specified, the ant will aim for its destination again as soon 
        /// as it has travelled the specified distance.
        /// <see href="https://wiki.antme.net/docs/commands#movement">Read more</see>
        /// </summary>
        /// <param name="steps">steps</param>
        public void GoForward(int steps)
        {
            GoForwardCoreInsect(steps);
        }

        /// <summary>
        /// The ant saves the specified destination and walks to it.
        /// <see href="https://wiki.antme.net/docs/commands#movementn">Read more</see>
        /// </summary>
        /// <param name="destination">destination</param>
        public void GoToDestination(Item destination)
        {
            GoToTargetCoreInsect(destination.Baseitem);
        }

        /// <summary>
        /// The ant saves the nearest anthill as its destination and walks towards it.
        /// <see href="https://wiki.antme.net/docs/commands#movement">Read more</see>
        /// </summary>
        public void GoToAnthill()
        {
            GoToAnthillCoreInsect();
        }

        /// <summary>
        /// The ant picks up the specified food. In the case of a mound of fruit, 
        /// it takes as much as possible until it reaches its maximum load (see 
        /// CurrentLoad and MaximumLoad). In the case of a piece of fruit, the ant 
        /// begins carrying the fruit (see CarryingFruit).
        /// <see href="https://wiki.antme.net/docs/commands#food">Read more</see>
        /// </summary>
        /// <param name="food">food</param>
        public void Take(Food food)
        {
            if (food is Sugar)
            {
                TakeCoreInsect((CoreSugar)food.Baseitem);
            }
            else if (food is Fruit)
            {
                TakeCoreInsect((CoreFruit)food.Baseitem);
            }
        }

        /// <summary>
        /// The ant sprays a scent marker at the current location. The possible 
        /// parameters are data contained in the marker (these can be read out of the 
        /// result of Spots(Marker) via marker.Information) and how far the maker 
        /// spreads out. The farther the marker spreads out, the faster it will disappear.
        /// <see href="https://wiki.antme.net/docs/commands#communication">Read more</see>
        /// </summary>
        /// <param name="information">information</param>
        public void MakeMarker(int information)
        {
            MakeMarkerCoreInsects(information);
        }

        /// <summary>
        /// The ant sprays a scent marker at the current location. The possible parameters 
        /// are data contained in the marker (these can be read out of the result of 
        /// Spots(Marker) via marker.Information) and how far the maker spreads out. 
        /// The farther the marker spreads out, the faster it will disappear.
        /// <see href="https://wiki.antme.net/docs/commands#communication">Read more</see>
        /// </summary>
        /// <param name="information">information</param>
        /// <param name="range">range</param>
        public void MakeMarker(int information, int range)
        {
            MakeMarkerCoreInsects(information, range);
        }


        #endregion

        #region Property-Wrapper

        /// <summary>
        /// Returns the ant’s maximum energy. The unit is hit points.
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int MaximumEnergy => MaximumEnergyCoreInsect;

        /// <summary>
        /// Returns the ant’s current energy. The unit is hit points. If an ant has 0 
        /// hit points or fewer, it dies. This value is always less than or equal to 
        /// MaximumEnergy.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int CurrentEnergy => CurrentEnergyCoreInsect;

        /// <summary>
        /// Returns the ant's current destination. If the ant currently has no 
        /// destination, the value returned is null.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public Item Destination
        {
            get
            {
                switch (DestinationCoreInsect)
                {
                    case CoreSugar _:
                        return new Sugar((CoreSugar)DestinationCoreInsect);
                    case CoreFruit _:
                        return new Fruit((CoreFruit)DestinationCoreInsect);
                    case CoreAnt _:
                        return new Ant((CoreAnt)DestinationCoreInsect);
                    case CoreBug _:
                        return new Bug((CoreBug)DestinationCoreInsect);
                    case CoreMarker _:
                        return new Marker((CoreMarker)DestinationCoreInsect);
                    case CoreAnthill _:
                        return new Anthill((CoreAnthill)DestinationCoreInsect);
                    default:
                        return null;
                }
            }
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Returns whether the ant is tired. The ant becomes tired as soon 
        /// as it has travelled a third of its maximum range. Once this value 
        /// has been exceeded, this property changes from false to true and 
        /// the event GettingTired() is called.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public bool IsTired => IsTiredCoreAnt;

        #endregion
        
        #region Fight
        
        /// <summary>
        /// The ant saves the specified bug or the specified enemy ant as its 
        /// destination and walks toward it. When the ant arrives at its destination, 
        /// it begins to fight.
        /// <see href="https://wiki.antme.net/docs/commands#combat">Read more</see>
        /// </summary>
        /// <param name="target">target</param>
        public void Attack(Insect target)
        {
            AttackCoreInsect((CoreInsect)target.Baseitem);
        }

/////////// from here
        /// <summary>
        /// The ant turns in the specified direction. The angle which it 
        /// turns around is determined automatically.
        /// </summary>
        /// <param name="direction">Direction as integer value.</param>
        public void TurnToDirection(int direction)
        {
            TurnToDirectionCoreInsect(direction);
        }

        /// <summary>
        /// The ant turns itself around the specified angle. Positive values turn 
        /// the ant to the right, negative values turn it to the left.
        /// </summary>
        /// <param name="degrees">Degrees as integer value.</param>
        public void TurnByDegrees(int degrees)
        {
            TurnByDegreesCoreInsect(degrees);
        }

        /// <summary>
        /// The ant turns 180 degrees in the opposite direction. The same
        /// as TurnByDegrees(180).
        /// </summary>
        public void TurnAround()
        {
            TurnAroundCoreInsect();
        }

        /// <summary>
        /// The ant turns into the direction of the specified item.
        /// </summary>
        /// <param name="destination">Destination is an item.</param>
        public void TurnToDestination(Item destination)
        {
            TurnToTargetCoreInsect(destination.Baseitem);
        }

        /// <summary>
        /// The ant stands still and forgets its current destination. In the next 
        /// round the result of Waiting() is called.
        /// </summary>
        public void Stop()
        {
            StopMovementCoreInsect();
        }

        /// <summary>
        /// The ant turns in the direction opposite the specified destination and 
        /// then walks straight ahead. The ant’s destination remains unaltered and 
        /// the walking distance can be specified.
        /// <see href="https://wiki.antme.net/docs/commands#movement">Read more</see>
        /// </summary>
        /// <param name="item">item</param>
        public void GoAwayFrom(Item item)
        {
            GoAwayFromCoreInsect(item.Baseitem);
        }

        /// <summary>
        /// The ant turns in the direction opposite the specified destination and 
        /// then walks straight ahead. The ant’s destination remains unaltered and 
        /// the walking distance can be specified.
        /// <see href="https://wiki.antme.net/docs/commands#movement">Read more</see>
        /// </summary>
        /// <param name="item">item</param>
        /// <param name="steps">steps</param>
        public void GoAwayFrom(Item item, int steps)
        {
            GoAwayFromCoreInsect(item.Baseitem, steps);
        }

        /// <summary>
        /// The ant drops the food that it is currently carrying. Sugar is lost while 
        /// apples remain where they fall and can be picked up again later. The command 
        /// is not necessary when delivering food to an anthill—that occurs automatically.
        /// <see href="https://wiki.antme.net/docs/commands#food">Read more</see>
        /// </summary>
        public void Drop()
        {
            DropFood();
        }

        /// <summary>
        /// This command causes the ant to display thought bubbles that can be used for 
        /// troubleshooting and debugging.
        /// <see href="https://wiki.antme.net/docs/commands#debug">Read more</see>
        /// </summary>
        /// <param name="message">message</param>
        public void Think(string message)
        {
            ThinkCore(message);
        }

        /// <summary>
        /// Returns the ant’s maximum speed. The unit is steps per round.
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int MaximumSpeed => ((CoreInsect)this).MaximumSpeedCoreInsect;

        /// <summary>
        /// Returns the maximum load that the ant can bear. The unit is food points. 
        /// This value determines how much fruit the ant can carry at once and how 
        /// fast it can carry an apple without the help of other ants.
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int MaximumLoad => ((CoreInsect)this).MaximumLoadCoreInsect;

        /// <summary>
        /// Specifies the distance in steps that the ant can travel before it dies of 
        /// hunger. After the ant has travelled a third of the value, the event 
        /// GettingTired() is called and the value of IsTired is set to "true". (See WalkedRange).
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int Range => ((CoreInsect)this).RangeCoreInsect;

        /// <summary>
        /// Specifies the ant’s attack value. The attack value determines how 
        /// many hit points the ant deducts from an enemy in each round. The 
        /// unit is hit points.
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int Strength => ((CoreInsect)this).AttackStrengthCoreInsect;

        /// <summary>
        /// Specifies the ant’s visual range in steps. This range determines how 
        /// far the ant must be from game elements like fruit in order for the ant 
        /// to see them. The direction that the ant is facing does not play a role 
        /// (ants have 360 vision in this game).
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int ViewRange => ((CoreInsect)this).ViewRangeCoreInsect;

        /// <summary>
        /// Specifies the speed with which an ant can turn. The unit is degrees per 
        /// round.
        /// <see href="https://wiki.antme.net/docs/commands#caste-related">Read more</see>
        /// </summary>
        public int RotationSpeed => ((CoreInsect)this).RotationSpeedCoreInsect;

        /// <summary>
        /// Returns the ant’s current possible speed. The unit is steps per round. The 
        /// value is influenced by the ant’s current load. Ants that are carrying a full 
        /// load can only travel at half of their maximum speed. The property always 
        /// returns a value greater than 0, even if the ant is standing still. This value 
        /// is always less than or equal to MaximumSpeed.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int CurrentSpeed => ((CoreInsect)this).CurrentSpeedCoreInsect;

        /// <summary>
        /// Returns the weight of the load that the ant is currently carrying. The unit 
        /// is food points. This value is always smaller than or equal to MaximumLoad.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int CurrentLoad => ((CoreInsect)this).CurrentLoadCoreInsect;

        /// <summary>
        /// Returns the number of friendly ants from the same colony in the ant’s 360° visual 
        /// range. The result of the calculation depends on the ant’s visual range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int FriendlyAntsInViewRange => ((CoreInsect)this).ColonyAntsInViewRange;

        /// <summary>
        /// Returns the number of friendly ants from the same colony and the same caste in 
        /// the ant’s 360° visual range. The result of this calculation depends on the ant’s 
        /// visual range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int CasteAntsInViewRange => ((CoreInsect)this).CasteAntsInViewRange;

        /// <summary>
        /// Returns the number of friendly ants from the same team in the ant’s 360° visual 
        /// range. The result of this calculation depends on the ant’s visual range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int TeamAntsInViewRange => ((CoreInsect)this).TeamAntsInViewRange;

        /// <summary>
        /// Returns the number of enemy ants in the ant’s 360° visual range. The result of 
        /// this calculation depends on the ant’s visual range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int EnemyAntsInViewRange => ((CoreInsect)this).EnemyAntsInViewRange;

        /// <summary>
        /// Returns the number of bugs in the ant’s 360° visual range. The result of this 
        /// calculation depends on the ant’s visual range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int BugsInViewRange => ((CoreInsect)this).BugsInViewRange; 
 
        /// <summary>
        /// Returns the distance in steps to the nearest friendly anthill.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int DistanceToAnthill => ((CoreInsect)this).DistanceToAnthillCoreInsect;


        /// <summary>
        /// Returns the fruit the ant is currently carrying. If the ant is not 
        /// carrying a fruit, the value returned is null.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public Fruit CarryingFruit
        {
            get
            {
                CoreAnt temp = this;
                if (temp.CarryingFruitCoreInsect != null) 
                {
                    return new Fruit(temp.CarryingFruitCoreInsect);
                }
                    return null;
            }
        }


        /// <summary>
        /// Returns the name of the ant's caste.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public string Caste
        {
            get
            {
                try
                {
                    return CasteCoreInsect;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns how many steps forward the ant must go before it reaches 
        /// its destination. This value is reduced each round by CurrentSpeed.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int DistanceToDestination => ((CoreInsect)this).DistanceToDestinationCoreInsect;

        /// <summary>
        /// Returns how many degrees the ant still has to turn before it moves 
        /// forward again. This value is reduced each round by RotationSpeed.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int ResidualAngle => ((CoreInsect)this).ResidualAngle;

        /// <summary>
        /// Returns the direction that the ant is facing on the map.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public int Direction => ((CoreInsect)this).GetDirectionCoreInsect();

        /// <summary>
        /// Returns whether the ant has reached its destination or not.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public bool Arrived => ((CoreInsect)this).ArrivedCoreInsect;

        /// <summary>
        /// This property returns the total number of steps that the ant has 
        /// travelled since its last visit to an anthill. See Range.
        /// <see href="https://wiki.antme.net/docs/commands#misc">Read more</see>
        /// </summary>
        public new int NumberStepsWalked => ((CoreInsect)this).NumberStepsWalked;

        private RandomNumber randomNumber;
        /// <summary>
        /// Generates a random number within the specified limits. If only one parameter 
        /// is specified, a random number will be generated between 0 and the specified 
        /// limit -1. If two parameters are specified, a number between will be generated 
        /// between the lower limit and the upper limit -1.
        /// <see href="https://wiki.antme.net/docs/commands#random-numbers"></see>
        /// </summary>
        public RandomNumber RandomNumber
        {
            get
            {
                if (randomNumber == null)
                    randomNumber = new RandomNumber(RandomCoreInsect);
                return randomNumber;
            }
        }

        /// <summary>
        /// Evaluates if the specified fruit needs more ants to carry it.
        /// <see href="https://wiki.antme.net/docs/commands#food-1">Read more</see>
        /// </summary>
        /// <param name="fruit">fruit</param>
        /// <returns>more ants required</returns>
        public bool NeedsCarrier(Fruit fruit) => ((CoreFruit)fruit.Baseitem).NeedSupport(Colony);
        
        #endregion

    }
}