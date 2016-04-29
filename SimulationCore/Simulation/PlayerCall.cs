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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.Waits));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.WartetBase();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der Wartet()-Methode", ant.colony.Player.Guid), ex);
            }
            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.BecomesTired));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.WirdMüdeBase();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der WirdMüde()-Methode", ant.colony.Player.Guid), ex);
            }
            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsSugar));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtBase(sugar);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der Sieht(Zucker)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsFruit));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtBase(fruit);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der Sieht(Obst)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                new AreaChangeEventArgs(ant.colony.Player, Area.ReachedSugar));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.ZielErreichtBase(sugar);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der ZielErreicht(Zucker)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                new AreaChangeEventArgs(ant.colony.Player, Area.ReachedFruit));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.ZielErreichtBase(fruit);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der ZielErreicht(Obst)-Methode", ant.colony.Player.Guid), ex);
            }
            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SmellsFriend));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.RiechtFreundBase(marker);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der RiechtFreund(Markierung)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsFriend));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtFreundBase(friend);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der SiehtFreund(Ameise)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsTeamMember));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtVerbündetenBase(friend);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der SiehtVerbündeten(Ameise)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsBug));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtFeindBase(bug);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der SiehtFeind(Wanze)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.SpotsEnemy));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.SiehtFeindBase(enemy);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der SiehtFeind(Ameise)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                new AreaChangeEventArgs(ant.colony.Player, Area.UnderAttackByAnt));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.WirdAngegriffenBase(enemy);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der WirdAngegriffen(Ameise)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                    ant.colony.Player, Area.UnderAttackByBug));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.WirdAngegriffenBase(bug);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der WirdAngegriffen(Wanze)-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.HasDied));
            playerRights.PermitOnly();
            try
            {
                ant.IstGestorbenBase(kindOfDeath);
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der IstGestorben()-Methode", ant.colony.Player.Guid), ex);
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
                null, new AreaChangeEventArgs(ant.colony.Player, Area.Tick));
            playerRights.PermitOnly();
            ant.NimmBefehleEntgegen = true;
            try
            {
                ant.TickBase();
            }
            catch (Exception ex)
            {
                throw new AiException(string.Format("{0}: KI-Fehler in der Tick()-Methode", ant.colony.Player.Guid), ex);
            }

            ant.NimmBefehleEntgegen = false;
            AreaChanged(
                null, new AreaChangeEventArgs(null, Area.Unknown));
        }

        #endregion
    }
}