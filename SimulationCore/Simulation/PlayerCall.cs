using System;
using System.Security;
using System.Security.Permissions;

namespace AntMe.Simulation
{
    /// <summary>
    /// Static Class to encapsulate player-calls to his ant.
    /// </summary>
    internal static class PlayerCall
    {
        private static readonly PermissionSet playerRights;

        /// <summary>
        /// Creates a static instance of PlayerCall
        /// </summary>
        static PlayerCall()
        {
            // set the rights for players
            playerRights = new PermissionSet(PermissionState.None);
            playerRights.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
        }

        public static event AreaChangeEventHandler AreaChanged;

        #region Movement

        /// <summary>
        /// Perform call to "Waits()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        public static void Waits(CoreAnt ant)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.Waits));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.WaitingCoreAnt();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in WaitingBase() method", ant.Colony.Player.Guid), ex);
            }
            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "BecomesTired()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        public static void BecomesTired(CoreAnt ant)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.BecomesTired));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.IsGettingTiredCoreAnt();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in IsGettingTiredBase() method", ant.Colony.Player.Guid), ex);
            }
            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion

        #region Food

        /// <summary>
        /// Perform call to "Spots(Sugar)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="sugar">sugar</param>
        public static void Spots(CoreAnt ant, CoreSugar sugar)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsSugar));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsCoreAnt(sugar);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsBase(sugar) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "Spots(Fruit)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="fruit">fruit</param>
        public static void Spots(CoreAnt ant, CoreFruit fruit)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsFruit));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsCoreAnt(fruit);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsBase(fruit) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "TargetReached(Sugar)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="sugar">sugar</param>
        public static void TargetReached(CoreAnt ant, CoreSugar sugar)
        {
            AreaChanged(
                null,
                new AreaChangeEventArgs(ant.Colony.Player, Area.ReachedSugar));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.ArrivedAtTargetCoreAnt(sugar);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in ArrivedAtTargetBase(sugar) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "TargetReached(Fruit)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="fruit">fruit</param>
        public static void TargetReached(CoreAnt ant, CoreFruit fruit)
        {
            AreaChanged(
                null,
                new AreaChangeEventArgs(ant.Colony.Player, Area.ReachedFruit));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.ArrivedAtTargetCoreAnt(fruit);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in ArrivedAtTargetBase(fruit) method", ant.Colony.Player.Guid), ex);
            }
            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion

        #region Communication

        /// <summary>
        /// Perform call to "SmellsFriend()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="marker">marker</param>
        public static void SmellsFriend(CoreAnt ant, CoreMarker marker)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SmellsFriend));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsFriendCoreAnt(marker);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsFriendBase(marker) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "SpotsFriend()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="friend">friendly ant</param>
        public static void SpotsFriend(CoreAnt ant, CoreAnt friend)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsFriend));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsFriendCoreAnt(friend);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsFriend(marker) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "SpotsTeamMember()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="friend">friendly ant</param>
        public static void SpotsTeamMember(CoreAnt ant, CoreAnt friend)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsTeamMember));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsTeamMemberCoreAnt(friend);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsTeamMember(ant, ant) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion

        #region Fight

        /// <summary>
        /// Perform call to "Spots(Bug)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="bug">bug</param>
        public static void SpotsEnemy(CoreAnt ant, CoreBug bug)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsBug));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsEnemyCoreAnt(bug);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsEnemy(ant, bug) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "Spots(Ant)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="enemy">foreign ant</param>
        public static void SpotsEnemy(CoreAnt ant, CoreAnt enemy)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.SpotsEnemy));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.SpotsEnemyCoreAnt(enemy);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in SpotsEnemy(ant, ant) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "UnderAttack(Ant)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="enemy">enemy</param>
        public static void UnderAttack(CoreAnt ant, CoreAnt enemy)
        {
            AreaChanged(
                null,
                new AreaChangeEventArgs(ant.Colony.Player, Area.UnderAttackByAnt));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.IsUnderAttackCoreAnt(enemy);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in UnderAttack(ant, ant) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "UnderAttack(Bug)" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="bug">bug</param>
        public static void UnderAttack(CoreAnt ant, CoreBug bug)
        {
            AreaChanged(
                null,
                new AreaChangeEventArgs(
                    ant.Colony.Player, Area.UnderAttackByBug));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.UnderAttackCoreAnt(bug);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in UnderAttack(ant, bug) method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion

        #region Misc

        /// <summary>
        /// Perform call to "HasDied()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="kindOfDeath">kind of death</param>
        public static void HasDied(CoreAnt ant, CoreKindOfDeath kindOfDeath)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.HasDied));
            playerRights.PermitOnly();
            try
            {
                ant.HasDiedCoreAnt(kindOfDeath);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in HadDiedBase(kindOfDeath) method", ant.Colony.Player.Guid), ex);
            }

            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        /// <summary>
        /// Perform call to "Tick()" on given ant.
        /// </summary>
        /// <param name="ant">ant</param>
        public static void Tick(CoreAnt ant)
        {
            AreaChanged(
                null, new AreaChangeEventArgs(ant.Colony.Player, Area.Tick));
            playerRights.PermitOnly();
            ant.AwaitingCommands = true;
            try
            {
                ant.TickCoreAnt();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: AI error in TickBase() method", ant.Colony.Player.Guid), ex);
            }

            ant.AwaitingCommands = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion
    }
}