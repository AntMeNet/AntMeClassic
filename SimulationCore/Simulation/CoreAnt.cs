using System;
using System.Collections.Generic;

using AntMe.SharedComponents.States;

namespace AntMe.Simulation {


    /// <summary>
    /// Abstrakte Basisklasse für alle Ameisen.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreAnt : CoreInsect {
        internal override void Init(CoreColony colony, Dictionary<string, int> availableInsects) {
            base.Init(colony, availableInsects);

            koordinate.Radius = 2;

            // Bestimme die Kaste der neuen Ameise.
            int casteIndex = -1;
            string casteName = string.Empty;
            if (availableInsects != null) {
                casteName = BestimmeKasteBase(availableInsects);
                for (int i = 0; i < colony.Player.Castes.Count; i++) {
                    if (colony.Player.Castes[i].Name == casteName) {
                        casteIndex = i;
                        break;
                    }
                }
            }

            // Check, if caste is available
            if (casteIndex == -1) {
                throw new InvalidOperationException(string.Format(Resource.SimulationCoreChooseWrongCaste, casteName));
            }

            // Setze die von der Kaste abhängigen Werte.
            CasteIndexBase = casteIndex;
            AktuelleEnergieBase = colony.Energie[casteIndex];
            aktuelleGeschwindigkeitI = colony.GeschwindigkeitI[casteIndex];
            AngriffBase = colony.Angriff[casteIndex];
        }

        /// <summary>
        /// Bestimmt die Kaste einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jeder Klaste bereits vorhandenen Ameisen.</param>
        /// <returns>Der Name der Kaste der Ameise.</returns>
        internal virtual string BestimmeKasteBase(Dictionary<string, int> anzahl) {
            return "";
        }

        /// <summary>
        /// Erzeugt ein AmeiseZustand-Objekt mit den aktuellen Daten der Ameise.
        /// </summary>
        internal AntState ErzeugeInfo() {
            AntState zustand = new AntState(colony.Id, id);

            zustand.CasteId = CasteIndexBase;
            zustand.PositionX = CoordinateBase.X/SimulationEnvironment.PLAYGROUND_UNIT;
            zustand.PositionY = CoordinateBase.Y/SimulationEnvironment.PLAYGROUND_UNIT;
            zustand.ViewRange = SichtweiteBase;
            zustand.DebugMessage = debugMessage;
            debugMessage = string.Empty;
            zustand.Direction = CoordinateBase.Richtung;
            if (ZielBase != null) {
                zustand.TargetPositionX = ZielBase.CoordinateBase.X/SimulationEnvironment.PLAYGROUND_UNIT;
                zustand.TargetPositionY = ZielBase.CoordinateBase.Y/SimulationEnvironment.PLAYGROUND_UNIT;
            }
            else {
                zustand.TargetPositionX = 0;
                zustand.TargetPositionY = 0;
            }
            zustand.Load = AktuelleLastBase;
            if (AktuelleLastBase > 0)
                if (GetragenesObstBase != null)
                    zustand.LoadType = LoadType.Fruit;
                else
                    zustand.LoadType = LoadType.Sugar;
            zustand.Vitality = AktuelleEnergieBase;

            if (ZielBase is CoreAnthill)
                zustand.TargetType = TargetType.Anthill;
            else if (ZielBase is CoreSugar)
                zustand.TargetType = TargetType.Sugar;
            else if (ZielBase is CoreFruit)
                zustand.TargetType = TargetType.Fruit;
            else if (ZielBase is CoreBug)
                zustand.TargetType = TargetType.Bug;
            else if (ZielBase is CoreMarker)
                zustand.TargetType = TargetType.Marker;
            else if (ZielBase is CoreAnt)
                zustand.TargetType = TargetType.Ant;
            else
                zustand.TargetType = TargetType.None;

            return zustand;
        }

        #region Fortbewegung

        private bool istMüde;

        /// <summary>
        /// Gibt an, ob die Ameise müde ist.
        /// </summary>
        internal bool IstMüdeBase {
            get { return istMüde; }
            set { istMüde = value; }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        internal virtual void WartetBase() {}

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen 
        /// Reichweite überschritten hat.
        /// </summary>
        internal virtual void WirdMüdeBase() {}

        #endregion

        #region Nahrung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
        /// Zuckerhaufen sieht.
        /// </summary>
        /// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
        internal virtual void SiehtBase(CoreSugar zucker) {}

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="obst">Das nächstgelegene Obststück.</param>
        internal virtual void SiehtBase(CoreFruit obst) {}

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        internal virtual void ZielErreichtBase(CoreSugar zucker) {}

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obstück.</param>
        internal virtual void ZielErreichtBase(CoreFruit obst) {}

        #endregion

        #region Kommunikation

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
        /// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
        /// gerochen.
        /// </summary>
        /// <param name="markierung">Die nächste neue Markierung.</param>
        internal virtual void RiechtFreundBase(CoreMarker markierung) {}

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
        /// selben Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
        internal virtual void SiehtFreundBase(CoreAnt ameise) {}

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise verbündeter
        /// Völker sieht.
        /// </summary>
        /// <param name="ameise"></param>
        internal virtual void SiehtVerbündetenBase(CoreAnt ameise) {}

        #endregion

        #region Kampf

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
        /// sieht.
        /// </summary>
        /// <param name="wanze">Die nächstgelegene Wanze.</param>
        internal virtual void SiehtFeindBase(CoreBug wanze) {}

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
        internal virtual void SiehtFeindBase(CoreAnt ameise) {}

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Die angreifende Wanze.</param>
        internal virtual void WirdAngegriffenBase(CoreBug wanze) {}

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
        /// anderen Volkes Ameise angegriffen wird.
        /// </summary>
        /// <param name="ameise">Die angreifende feindliche Ameise.</param>
        internal virtual void WirdAngegriffenBase(CoreAnt ameise) {}

        #endregion

        #region Sonstiges

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
        /// </summary>
        /// <param name="todesArt">Die Todesart der Ameise</param>
        internal virtual void IstGestorbenBase(CoreKindOfDeath todesArt) {}

        /// <summary>
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        internal virtual void TickBase() {}

        #endregion
    }
}