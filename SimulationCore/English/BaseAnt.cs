using System.Collections.Generic;

using AntMe.Simulation;

namespace AntMe.English {
    /// <summary>
    /// Baseclass for new Ant-Implementations
    /// </summary>
    public abstract class BaseAnt : CoreAnt {
        #region Eventwrapper

        internal override string BestimmeKasteBase(Dictionary<string, int> anzahl) {
            return ChooseType(anzahl);
        }

        /// <summary>
        /// Returns the name of the ant-type which will be born next
        /// </summary>
        /// <param name="typeCount">List of ant-types and there population</param>
        /// <returns>Name of ant-type which will born next</returns>
        public virtual string ChooseType(Dictionary<string, int> typeCount) {
            return string.Empty;
        }

        internal override void IstGestorbenBase(CoreKindOfDeath todesArt) {
            HasDied((KindOfDeath) (int) todesArt);
        }

        /// <summary>
        /// Will be fired when the ant is going to death
        /// </summary>
        /// <param name="kindOfDeath">Kind of Death</param>
        public virtual void HasDied(KindOfDeath kindOfDeath) {}

        internal override void RiechtFreundBase(CoreMarker markierung) {
            SmellsFriend(new Marker(markierung));
        }

        /// <summary>
        /// Will be fired when the ant smells a Marker
        /// </summary>
        /// <param name="marker">the smelled marker</param>
        public virtual void SmellsFriend(Marker marker) {}

        internal override void SiehtBase(CoreFruit obst) {
            Spots(new Fruit(obst));
        }

        /// <summary>
        /// Will be fired when the ant sees fruit
        /// </summary>
        /// <param name="fruit">the discovered fruit</param>
        public virtual void Spots(Fruit fruit) {}

        internal override void SiehtBase(CoreSugar zucker) {
            Spots(new Sugar(zucker));
        }

        /// <summary>
        /// Will be fired when the ant discovers a sugarhill
        /// </summary>
        /// <param name="sugar">discovered sugarhill</param>
        public virtual void Spots(Sugar sugar) {}

        internal override void SiehtFeindBase(CoreAnt ameise) {
            SpotsEnemy(new Ant(ameise));
        }

        /// <summary>
        /// Will be fired when the ant discovers a foreign ant
        /// </summary>
        /// <param name="ant">the foreign ant</param>
        public virtual void SpotsEnemy(Ant ant) {}

        internal override void SiehtFeindBase(CoreBug wanze) {
            SpotsEnemy(new Bug(wanze));
        }

        /// <summary>
        /// Will be fired when the ant spots a bug
        /// </summary>
        /// <param name="bug">the discovered bug</param>
        public virtual void SpotsEnemy(Bug bug) {}

        internal override void SiehtFreundBase(CoreAnt ameise) {
            SpotsFriend(new Ant(ameise));
        }

        /// <summary>
        /// Will be fired when the ant spots another ant of there own colony.
        /// </summary>
        /// <param name="ant">the other ant</param>
        public virtual void SpotsFriend(Ant ant) {}

        internal override void SiehtVerbündetenBase(CoreAnt ameise) {
            SpotsPartner(new Ant(ameise));
        }

        /// <summary>
        /// Will be fired when the ant spots another ant from friendly team.
        /// </summary>
        /// <param name="ant">the other ant</param>
        public virtual void SpotsPartner(Ant ant) {}

        internal override void TickBase() {
            Tick();
        }

        /// <summary>
        /// Will be fired every round for every single ant
        /// </summary>
        public virtual void Tick() {}

        internal override void WartetBase() {
            Waits();
        }

        /// <summary>
        /// Will be fired everytime the ant don't know whats next
        /// </summary>
        public virtual void Waits() {}

        internal override void WirdAngegriffenBase(CoreAnt ameise) {
            UnderAttack(new Ant(ameise));
        }

        /// <summary>
        /// Will be fired when the ant is under attack by a foreign ant
        /// </summary>
        /// <param name="ant">attacking ant</param>
        public virtual void UnderAttack(Ant ant) {}

        internal override void WirdAngegriffenBase(CoreBug wanze) {
            UnderAttack(new Bug(wanze));
        }

        /// <summary>
        /// Will be fired when the ant is under attack by a bug
        /// </summary>
        /// <param name="bug">attacking bug</param>
        public virtual void UnderAttack(Bug bug) {}

        internal override void WirdMüdeBase() {
            BecomesTired();
        }

        /// <summary>
        /// Will be fired when the ant is going to run out of food
        /// </summary>
        public virtual void BecomesTired() {}

        internal override void ZielErreichtBase(CoreFruit obst) {
            TargetReached(new Fruit(obst));
        }

        /// <summary>
        /// Will be fired when the ant reaches the targeted vegetable
        /// </summary>
        /// <param name="fruit">targeted fruit</param>
        public virtual void TargetReached(Fruit fruit) {}

        internal override void ZielErreichtBase(CoreSugar zucker) {
            TargetReached(new Sugar(zucker));
        }

        /// <summary>
        /// Will be fired when the ant reaches the targeted sugarhill
        /// </summary>
        /// <param name="sugar">targeted sugarhill</param>
        public virtual void TargetReached(Sugar sugar) {}

        #endregion

        #region Command-Wrapper

        /// <summary>
        /// Rotates the ant in the given direction
        /// </summary>
        /// <param name="direction">target direction</param>
        public void TurnToDirection(int direction) {
            DreheInRichtungBase(direction);
        }

        /// <summary>
        /// Rotates the ant by the given degrees
        /// </summary>
        /// <param name="degrees">degrees to rotate</param>
        public void TurnByDegrees(int degrees) {
            DreheUmWinkelBase(degrees);
        }

        /// <summary>
        /// Rotates the ant to the oposit direction
        /// </summary>
        public void TurnAround() {
            DreheUmBase();
        }

        /// <summary>
        /// Rotates the ant to the given Target
        /// </summary>
        /// <param name="target">target</param>
        public void TurnToTarget(Item target) {
            DreheZuZielBase(target.Baseitem);
        }

        /// <summary>
        /// Stops the ant-movement
        /// </summary>
        public void Stop() {
            BleibStehenBase();
        }

        /// <summary>
        /// Let the ant go forward
        /// </summary>
        public void GoAhead() {
            GeheGeradeausBase();
        }

        /// <summary>
        /// Let the ant walk forward the given steps
        /// </summary>
        /// <param name="steps">steps to go</param>
        public void GoAhead(int steps) {
            GeheGeradeausBase(steps);
        }

        /// <summary>
        /// Let the ant go away from given target
        /// </summary>
        /// <param name="target">target</param>
        public void GoAwayFromTarget(Item target) {
            GeheWegVonBase(target.Baseitem);
        }

        /// <summary>
        /// Let the ant go away from given target
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="steps">Steps to go</param>
        public void GoAwayFromTarget(Item target, int steps) {
            GeheWegVonBase(target.Baseitem, steps);
        }

        /// <summary>
        /// Let the ant walk to the given target
        /// </summary>
        /// <param name="target">target</param>
        public void GoToTarget(Item target) {
            GeheZuZielBase(target.Baseitem);
        }

        /// <summary>
        /// Let the ant walk back to the nearest anthill
        /// </summary>
        public void GoBackToAnthill() {
            GeheZuBauBase();
        }

        /// <summary>
        /// Let the ant attack the given target
        /// </summary>
        /// <param name="target">target to attack</param>
        public void Attack(Insect target) {
            GreifeAnBase((CoreInsect) target.Baseitem);
        }

        /// <summary>
        /// Picks up the given food
        /// </summary>
        /// <param name="food">food to pick up</param>
        public void Take(Food food) {
            if (food is Sugar) {
                NimmBase((CoreSugar) food.Baseitem);
            }
            else if (food is Fruit) {
                NimmBase((CoreFruit) food.Baseitem);
            }
        }

        /// <summary>
        /// Let the ant drop all the carring food
        /// </summary>
        public void Drop() {
            LasseNahrungFallenBase();
        }

        /// <summary>
        /// Let the ant create a mark with given information
        /// </summary>
        /// <param name="information">information to put in mark</param>
        public void MakeMark(int information) {
            SprüheMarkierungBase(information);
        }

        /// <summary>
        /// Let the ant create a mark with given information and given range
        /// </summary>
        /// <param name="information">information to put in mark</param>
        /// <param name="range">range of mark</param>
        public void MakeMark(int information, int range) {
            SprüheMarkierungBase(information, range);
        }

        /// <summary>
        /// Let the ant think about the given message. This will be 
        /// shown in the 3D View during the Debug Mode.
        /// </summary>
        /// <param name="message">Message</param>
        public void Think(string message)
        {
            DenkeCore(message);
        }

        #endregion

        #region Property-Wrapper

        /// <summary>
        /// Gives the highest possible Hitpoints of this ant
        /// </summary>
        public int MaximumEnergy {
            get { return MaximaleEnergieBase; }
        }

        /// <summary>
        /// Gives the highest possible Speed of this ant (in steps per round)
        /// </summary>
        public int MaximumSpeed {
            get { return MaximaleGeschwindigkeitBase; }
        }

        /// <summary>
        /// Gives the highest possible Carrage of this ant
        /// </summary>
        public int MaximumLoad {
            get { return MaximaleLastBase; }
        }

        /// <summary>
        /// Gives the Range of this ant (in steps)
        /// </summary>
        public int Range {
            get { return ReichweiteBase; }
        }

        /// <summary>
        /// Gives the attack-strength of this ant
        /// </summary>
        public int Strength {
            get { return AngriffBase; }
        }

        /// <summary>
        /// Gives the range of view of this ant
        /// </summary>
        public int Viewrange {
            get { return SichtweiteBase; }
        }

        /// <summary>
        /// Gives the rotationspeed of this ant (degrees per round)
        /// </summary>
        public int RotationSpeed {
            get { return DrehgeschwindigkeitBase; }
        }

        /// <summary>
        /// Gives the current amount of hitpoints
        /// </summary>
        public int CurrentEnergy {
            get { return AktuelleEnergieBase; }
        }

        /// <summary>
        /// Gives the current speed of this ant
        /// </summary>
        public int CurrentSpeed {
            get { return AktuelleGeschwindigkeitBase; }
        }

        /// <summary>
        /// Gives the current carrage of this ant
        /// </summary>
        public int CurrentLoad {
            get { return AktuelleLastBase; }
        }

        /// <summary>
        /// Gives the count of friendly ants in viewrange of this ant
        /// </summary>
        public new int FriendlyAntsInViewrange {
            get { return base.FriendlyAntsInViewrange; }
        }

        /// <summary>
        /// Gives the count of friendly ants from same caste in viewrange.
        /// </summary>
        public new int FriendlyAntsFromSameCasteInViewrange {
            get { return base.FriendlyAntsFromSameCasteInViewrange; }
        }

        /// <summary>
        /// Gives the count of Ants from same team in Viewrange.
        /// </summary>
        public new int TeamAntsInViewrange {
            get { return base.TeamAntsInViewrange; }
        }

        /// <summary>
        /// Gives the count of foreign ants in viewrange.
        /// </summary>
        public new int ForeignAntsInViewrange {
            get { return base.ForeignAntsInViewrange; }
        }

        /// <summary>
        /// Gives the count of bugs in viewrange.
        /// </summary>
        public int BugsInViewrange {
            get { return base.BugsInViewrange; }
        }

        /// <summary>
        /// Gives the distance to the nearest anthill (in steps)
        /// </summary>
        public int DistanceToAnthill {
            get { return EntfernungZuBauBase; }
        }

        /// <summary>
        /// Gives the loaded Vegetable
        /// </summary>
        public Fruit CarringFruit {
            get {
                if (GetragenesObstBase != null) {
                    return new Fruit(GetragenesObstBase);
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gives the profession of this ant
        /// </summary>
        public string Caste {
            get { return KasteBase; }
        }

        /// <summary>
        /// Gives the Index of the ants profession
        /// </summary>
        public int CasteIndex {
            get { return CasteIndexBase; }
        }

        /// <summary>
        /// Gives the ants target
        /// </summary>
        public Item Target {
            get {
                if (ZielBase is CoreSugar) {
                    return new Sugar((CoreSugar) ZielBase);
                }
                else if (ZielBase is CoreFruit) {
                    return new Fruit((CoreFruit) ZielBase);
                }
                else if (ZielBase is CoreAnt) {
                    return new Ant((CoreAnt) ZielBase);
                }
                else if (ZielBase is CoreBug) {
                    return new Bug((CoreBug) ZielBase);
                }
                else if (ZielBase is CoreMarker) {
                    return new Marker((CoreMarker) ZielBase);
                }
                else if (ZielBase is CoreAnthill) {
                    return new Anthill((CoreAnthill) ZielBase);
                }
                else {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gives back, if the ant is tired
        /// </summary>
        public bool IsTired {
            get { return IstMüdeBase; }
        }

        /// <summary>
        /// Gives the remain distance to target
        /// </summary>
        public int DistanceToTarget {
            get { return RestStreckeBase; }
        }

        /// <summary>
        /// Gives the remain degrees to rotate
        /// </summary>
        public int DegreesToTarget {
            get { return RestWinkelBase; }
        }

        /// <summary>
        /// Gives the current direction of the ant
        /// </summary>
        public int Direction {
            get { return RichtungBase; }
        }

        /// <summary>
        /// Gives back, if the ant reched the target
        /// </summary>
        public bool ReachedTarget {
            get { return AngekommenBase; }
        }

        /// <summary>
        /// Gives the distance that was walked by the ant
        /// </summary>
        public int WalkedRange {
            get { return ZurückgelegteStreckeBase; }
        }

        #endregion

        #region Supporting Methodes

        /// <summary>
        /// Finds out, if the given Vegetable needs more ants to carry
        /// </summary>
        /// <param name="fruit">Fruit to test</param>
        /// <returns>needs more ants</returns>
        public bool NeedsCarrier(Fruit fruit) {
            return ((CoreFruit) fruit.Baseitem).BrauchtNochTräger(colony);
        }

        #endregion
    }
}