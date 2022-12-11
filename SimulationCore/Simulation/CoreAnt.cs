using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{


    /// <summary>
    /// Abstract base class for all ants.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreAnt : CoreInsect
    {
        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> availableInsects)
        {
            base.Init(colony, random, availableInsects);

            Coordinate.Radius = 2;

            // Determine caste of the new ant.
            int casteIndex = -1;
            string casteName = string.Empty;
            if (availableInsects != null)
            {
                casteName = DetermineCasteCoreAnt(availableInsects);
                for (int i = 0; i < colony.Player.Castes.Count; i++)
                {
                    if (colony.Player.Castes[i].Name == casteName)
                    {
                        casteIndex = i;
                        break;
                    }
                }
            }

            // Check, if caste is available.
            if (casteIndex == -1)
            {
                throw new InvalidOperationException(string.Format(Resource.SimulationCoreChooseWrongCaste, casteName));
            }

            // Set ants properties depending on its caste.
            CasteIndexCoreInsect = casteIndex;
            CurrentEnergyCoreInsect = colony.EnergyI[casteIndex];
            currentSpeedICoreInsect = colony.SpeedI[casteIndex];
            AttackStrengthCoreInsect = colony.AttackI[casteIndex];
        }
        
        /// <summary>
        /// Determine caste for a new ant.
        /// </summary>
        /// <param name="number">Ants in caste per colony.</param>
        /// <returns>Name of the caste for the new ant.</returns>
        internal virtual string DetermineCasteCoreAnt(Dictionary<string, int> number)
        {
            return "";
        }

        /// <summary>
        /// Generate ant state information
        /// </summary>
        internal AntState GenerateAntStateInfo()
        {
            AntState antState = new AntState(Colony.Id, Id);

            antState.CasteId = CasteIndexCoreInsect;
            antState.PositionX = CoordinateCoreInsect.X / SimulationEnvironment.PLAYGROUND_UNIT;
            antState.PositionY = CoordinateCoreInsect.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            antState.ViewRange = ViewRangeCoreInsect;
            antState.DebugMessage = debugMessage;
            debugMessage = string.Empty;
            antState.Direction = CoordinateCoreInsect.Direction;
            if (DestinationCoreInsect != null)
            {
                antState.TargetPositionX = DestinationCoreInsect.CoordinateCoreInsect.X / SimulationEnvironment.PLAYGROUND_UNIT;
                antState.TargetPositionY = DestinationCoreInsect.CoordinateCoreInsect.Y / SimulationEnvironment.PLAYGROUND_UNIT;
            }
            else
            {
                antState.TargetPositionX = 0;
                antState.TargetPositionY = 0;
            }
            antState.Load = CurrentLoadCoreInsect;
            if (CurrentLoadCoreInsect > 0)
                if (CarryingFruitCoreInsect != null)
                    antState.LoadType = LoadType.Fruit;
                else
                    antState.LoadType = LoadType.Sugar;
            antState.Vitality = CurrentEnergyCoreInsect;

            if (DestinationCoreInsect is CoreAnthill)
                antState.TargetType = TargetType.Anthill;
            else if (DestinationCoreInsect is CoreSugar)
                antState.TargetType = TargetType.Sugar;
            else if (DestinationCoreInsect is CoreFruit)
                antState.TargetType = TargetType.Fruit;
            else if (DestinationCoreInsect is CoreBug)
                antState.TargetType = TargetType.Bug;
            else if (DestinationCoreInsect is CoreMarker)
                antState.TargetType = TargetType.Marker;
            else if (DestinationCoreInsect is CoreAnt)
                antState.TargetType = TargetType.Ant;
            else
                antState.TargetType = TargetType.None;

            return antState;
        }

        #region Movement

        private bool isTired;

        /// <summary>
        /// True if ant is tired.
        /// </summary>
        internal bool IsTiredCoreAnt
        {
            get { return isTired; }
            set { isTired = value; }
        }

        /// <summary>
        /// Waiting is the standard action if the ant has no target.
        /// </summary>
        internal virtual void WaitingCoreAnt() { }

        /// <summary>
        /// Evoked once when the ant has reached one third of its maximum operational range.
        /// </summary>
        internal virtual void IsGettingTiredCoreAnt() { }

        #endregion

        #region Food

        /// <summary>
        /// Evoked when ant sees a sugar hill.
        /// </summary>
        /// <param name="sugar">Spotted sugar hill.</param>
        internal virtual void SpotsCoreAnt(CoreSugar sugar) { }

        /// <summary>
        /// Evoked when ant sees a fruit.
        /// </summary>
        /// <param name="fruit">Nearest fruit.</param>
        internal virtual void SpotsCoreAnt(CoreFruit fruit) { }

        /// <summary>
        /// Evoked when ant arrived at sugar hill.
        /// </summary>
        /// <param name="sugar">Sugar hill.</param>
        internal virtual void ArrivedAtTargetCoreAnt(CoreSugar sugar) { }

        /// <summary>
        /// Evoked when ant arrived at fruit.
        /// </summary>
        /// <param name="fruit">Fruit.</param>
        internal virtual void ArrivedAtTargetCoreAnt(CoreFruit fruit) { }

        #endregion

        #region Communication

        /// <summary>
        /// Evoked when ant spots marker of a friendly ant from the same colony for the first time
        /// already seen marks will be remembered.
        /// </summary>
        /// <param name="marker">Marker of a friendly ant.</param>
        internal virtual void SpotsFriendCoreAnt(CoreMarker marker) { }

        /// <summary>
        /// Evoked when ant spots a friendly ant from the same colony.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">Friendly ant from same colony.</param>
        internal virtual void SpotsFriendCoreAnt(CoreAnt ant) { }

        /// <summary>
        /// Evoked when ant spots a friendly ant from a team colony.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">Friendly ant from a team colony.</param>
        internal virtual void SpotsTeamMemberCoreAnt(CoreAnt ant) { }

        #endregion

        #region Fight

        /// <summary>
        /// Evoked when ant spots a bug.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="bug">Bug.</param>
        internal virtual void SpotsEnemyCoreAnt(CoreBug bug) { }

        /// <summary>
        /// Evoked when ant spots an enemy ant.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">Enemy ant.</param>
        internal virtual void SpotsEnemyCoreAnt(CoreAnt ant) { }

        /// <summary>
        /// Evoked when ant is attacked by a bug.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="bug">attacking Bug.</param>
        internal virtual void UnderAttackCoreAnt(CoreBug bug) { }

        /// <summary>
        /// Evoked when ant is attacked by an enemy ant.
        /// Will be evoked every time.
        /// </summary>
        /// <param name="ant">Attacking ant.</param>
        internal virtual void IsUnderAttackCoreAnt(CoreAnt ant) { }

        #endregion

        #region other

        /// <summary>
        /// Evoked when ant has died.
        /// </summary>
        /// <param name="kindOfDeath">Ants kind of death.</param>
        internal virtual void HasDiedCoreAnt(CoreKindOfDeath kindOfDeath) { }

        /// <summary>
        /// Tick will be evoked in every round for every ant regardless of other circumstances.
        /// </summary>
        internal virtual void TickCoreAnt() { }

        #endregion
    }
}