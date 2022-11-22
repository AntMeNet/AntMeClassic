using AntMe.SharedComponents.States;
using System;
using System.Collections.Generic;

namespace AntMe.Simulation
{


    /// <summary>
    /// Abstrakte Basisklasse für alle Ameisen.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreAnt : CoreInsect
    {
        internal override void Init(CoreColony colony, Random random, Dictionary<string, int> availableInsects)
        {
            base.Init(colony, random, availableInsects);

            coordinate.Radius = 2;

            // Bestimme die Kaste der neuen Ameise.
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

            // Setze die von der Kaste abhängigen Werte.
            CasteIndexBase = casteIndex;
            currentEnergyBase = colony.EnergyI[casteIndex];
            currentSpeedI = colony.SpeedI[casteIndex];
            AttackStrengthBase = colony.AttackI[casteIndex];
        }

        /// <summary>
        /// Bestimmt die Kaste einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jeder Klaste bereits vorhandenen Ameisen.</param>
        /// <returns>Der Name der Kaste der Ameise.</returns>
        internal virtual string DetermineCasteBase(Dictionary<string, int> anzahl)
        {
            return "";
        }

        /// <summary>
        /// Erzeugt ein AmeiseZustand-Objekt mit den aktuellen Daten der Ameise.
        /// </summary>
        internal AntState GenerateInformation()
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
            antState.Load = CurrentBurdenBase;
            if (CurrentBurdenBase > 0)
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
        /// Gibt an, ob die Ameise müde ist.
        /// </summary>
        internal bool IsTiredBase
        {
            get { return isTired; }
            set { isTired = value; }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        internal virtual void WaitingBase() { }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen 
        /// Reichweite überschritten hat.
        /// </summary>
        internal virtual void IsGettingTiredBase() { }

        #endregion

        #region Food

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
        /// Zuckerhaufen sieht.
        /// </summary>
        /// <param name="sugar">Der nächstgelegene Zuckerhaufen.</param>
        internal virtual void SpotsBase(CoreSugar sugar) { }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="fruit">Das nächstgelegene Obststück.</param>
        internal virtual void SiehtBase(CoreFruit fruit) { }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="sugar">Der Zuckerhaufen.</param>
        internal virtual void ArrivedAtTargetBase(CoreSugar sugar) { }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="fruit">Das Obstück.</param>
        internal virtual void ArrivedAtTargetBase(CoreFruit fruit) { }

        #endregion

        #region Communication

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
        /// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
        /// gerochen.
        /// </summary>
        /// <param name="marker">Die nächste neue Markierung.</param>
        internal virtual void SpotsFriendBase(CoreMarker marker) { }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
        /// selben Volkes sieht.
        /// </summary>
        /// <param name="ant">Die nächstgelegene befreundete Ameise.</param>
        internal virtual void SpotsFriendBase(CoreAnt ant) { }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise verbündeter
        /// Völker sieht.
        /// </summary>
        /// <param name="ant"></param>
        internal virtual void SpotsTeamMemberBase(CoreAnt ant) { }

        #endregion

        #region Fight

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
        /// sieht.
        /// </summary>
        /// <param name="bug">Die nächstgelegene Wanze.</param>
        internal virtual void SpotsEnemyBase(CoreBug bug) { }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ant">Die nächstgelegen feindliche Ameise.</param>
        internal virtual void SpotsEnemyBase(CoreAnt ant) { }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
        /// wird.
        /// </summary>
        /// <param name="bug">Die angreifende Wanze.</param>
        internal virtual void UnderAttackBase(CoreBug bug) { }

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
        /// anderen Volkes Ameise angegriffen wird.
        /// </summary>
        /// <param name="ant">Die angreifende feindliche Ameise.</param>
        internal virtual void IsUnderAttackBase(CoreAnt ant) { }

        #endregion

        #region Sonstiges

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
        /// </summary>
        /// <param name="kindOfDeath">Die Todesart der Ameise</param>
        internal virtual void HasDiedBase(CoreKindOfDeath kindOfDeath) { }

        /// <summary>
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        internal virtual void TickBase() { }

        #endregion
    }
}