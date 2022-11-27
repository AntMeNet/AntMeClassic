using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{


    /// <summary>
    /// abstract base class for all ants
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreAnt : CoreInsect
    {
        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> availableInsects)
        {
            base.Init(colony, random, availableInsects);

            coordinate.Radius = 2;

            // determine caste of the new ant
            int casteIndex = -1;
            string casteName = string.Empty;
            if (availableInsects != null)
            {
                casteName = DetermineCasteBase(availableInsects);
                for (int i = 0; i < colony.Player.Castes.Count; i++)
                {
                    if (colony.Player.Castes[i].Name == casteName)
                    {
                        casteIndex = i;
                        break;
                    }
                }
            }

            // Check, if caste is available
            if (casteIndex == -1)
            {
                throw new InvalidOperationException(string.Format(Resource.SimulationCoreChooseWrongCaste, casteName));
            }

            // set ants properties depending on its caste
            CasteIndexBase = casteIndex;
            currentEnergyBase = colony.EnergyI[casteIndex];
            currentSpeedI = colony.SpeedI[casteIndex];
            AttackStrengthBase = colony.AttackI[casteIndex];
        }

        /// <summary>
        /// Determine caste for a new ant
        /// </summary>
        /// <param name="existingAntsPerCasteInColony">ants in caste per colony</param>
        /// <returns>name of the caste for the new ant</returns>
        internal virtual string DetermineCasteBase(Dictionary<string, int> existingAntsPerCasteInColony)
        {
            return "";
        }

        /// <summary>
        /// Generate ant state information
        /// </summary>
        internal AntState GenerateAntStateInfo()
        {
            AntState antState = new AntState(colony.Id, id);

            antState.CasteId = CasteIndexBase;
            antState.PositionX = CoordinateBase.X / SimulationEnvironment.PLAYGROUND_UNIT;
            antState.PositionY = CoordinateBase.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            antState.ViewRange = ViewRangeBase;
            antState.DebugMessage = debugMessage;
            debugMessage = string.Empty;
            antState.Direction = CoordinateBase.Direction;
            if (TargetBase != null)
            {
                antState.TargetPositionX = TargetBase.CoordinateBase.X / SimulationEnvironment.PLAYGROUND_UNIT;
                antState.TargetPositionY = TargetBase.CoordinateBase.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            }
            else
            {
                antState.TargetPositionX = 0;
                antState.TargetPositionY = 0;
            }
            antState.Load = CurrentLoadBase;
            if (CurrentLoadBase > 0)
                if (CarryingFruitBase != null)
                    antState.LoadType = LoadType.Fruit;
                else
                    antState.LoadType = LoadType.Sugar;
            antState.Vitality = currentEnergyBase;

            if (TargetBase is CoreAnthill)
                antState.TargetType = TargetType.Anthill;
            else if (TargetBase is CoreSugar)
                antState.TargetType = TargetType.Sugar;
            else if (TargetBase is CoreFruit)
                antState.TargetType = TargetType.Fruit;
            else if (TargetBase is CoreBug)
                antState.TargetType = TargetType.Bug;
            else if (TargetBase is CoreMarker)
                antState.TargetType = TargetType.Marker;
            else if (TargetBase is CoreAnt)
                antState.TargetType = TargetType.Ant;
            else
                antState.TargetType = TargetType.None;

            return antState;
        }

        #region Movement

        private bool isTired;

        /// <summary>
        /// true if ant is tired
        /// </summary>
        internal bool IsTiredBase
        {
            get { return isTired; }
            set { isTired = value; }
        }

        /// <summary>
        /// Waiting is the standard action if the ant has no target
        /// </summary>
        internal virtual void WaitingBase() { }

        /// <summary>
        /// Evoked once when the ant has reached one third of its maximum operational range
        /// </summary>
        internal virtual void IsGettingTiredBase() { }

        #endregion

        #region Food

        /// <summary>
        /// Evoked when ant sees a sugar hill
        /// </summary>
        /// <param name="sugar">spotted sugar hill</param>
        internal virtual void SpotsBase(CoreSugar sugar) { }

        /// <summary>
        /// Evoked when ant sees a fruit
        /// </summary>
        /// <param name="fruit">nearest fruit</param>
        internal virtual void SpotsBase(CoreFruit fruit) { }

        /// <summary>
        /// Evoked when ant arrived at sugar hill
        /// </summary>
        /// <param name="sugar">sugar hill</param>
        internal virtual void ArrivedAtTargetBase(CoreSugar sugar) { }

        /// <summary>
        /// Evoked when ant arrived at fruit
        /// </summary>
        /// <param name="fruit">fruit</param>
        internal virtual void ArrivedAtTargetBase(CoreFruit fruit) { }

        #endregion

        #region Communication

        /// <summary>
        /// Evoked when ant spots marker of a friendly ant from the same colony for the first time
        /// allready seen marks will be remembered
        /// </summary>
        /// <param name="marker">marker of a friendly ant</param>
        internal virtual void SpotsFriendBase(CoreMarker marker) { }

        /// <summary>
        /// Evoked when ant spots a friendly ant from the same colony.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">friendly ant from same colony</param>
        internal virtual void SpotsFriendBase(CoreAnt ant) { }

        /// <summary>
        /// Evoked when ant spots a friendly ant from a team colony.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">friendly ant from a team colony</param>
        internal virtual void SpotsTeamMemberBase(CoreAnt ant) { }

        #endregion

        #region Fight

        /// <summary>
        /// Evoked when ant spots a bug.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="bug">bug</param>
        internal virtual void SpotsEnemyBase(CoreBug bug) { }

        /// <summary>
        /// Evoked when ant spots an enemy ant.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">enemy ant</param>
        internal virtual void SpotsEnemyBase(CoreAnt ant) { }

        /// <summary>
        /// Evoked when ant is attacked by a bug.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="bug">attacking bug</param>
        internal virtual void UnderAttackBase(CoreBug bug) { }

        /// <summary>
        /// Evoked when ant is attacked by an enemy ant.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">attacking ant</param>
        internal virtual void IsUnderAttackBase(CoreAnt ant) { }

        #endregion

        #region Sonstiges

        /// <summary>
        /// Evoked when ant has died.
        /// </summary>
        /// <param name="kindOfDeath">ants kind of death</param>
        internal virtual void HasDiedBase(CoreKindOfDeath kindOfDeath) { }

        /// <summary>
        /// Tick will be evoked in every round for every ant regardless of other circumstances.
        /// </summary>
        internal virtual void TickBase() { }

        #endregion
    }
}