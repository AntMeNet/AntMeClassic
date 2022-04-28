using System;
using System.Collections.Generic;
using System.Threading;

namespace AntMe.Simulation
{
    /// <summary>
    /// Abstrakte Basisklasse f�r alle Insekten.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public abstract class CoreInsect : ICoordinate
    {
        /// <summary>
        /// Die Id des n�chste erzeugten Insekts.
        /// </summary>
        private static int neueId = 0;

        /// <summary>
        /// Speichert die Markierungen, die das Insekt schon gesehen hat. Das
        /// verhindert, da� das Insekt zwischen Markierungen im Kreis l�uft.
        /// </summary>
        internal readonly List<CoreMarker> SmelledMarker = new List<CoreMarker>();

        private bool reached = false;
        private int antCount = 0;
        private int casteCount = 0;
        private int colonyCount = 0;
        private int bugCount = 0;
        private int teamCount = 0;
        private CoreFruit getragenesObst;

        /// <summary>
        /// Die Id die das Insekt w�hrend eines Spiels eindeutig indentifiziert.
        /// </summary>
        internal int id;

        /// <summary>
        /// Die Position des Insekts auf dem Spielfeld.
        /// </summary>
        internal CoreCoordinate koordinate;

        /// <summary>
        /// Legt fest, ob das Insekt Befehle entgegen nimmt.
        /// </summary>
        internal bool NimmBefehleEntgegen = false;

        private int restStreckeI;
        private int restWinkel = 0;

        /// <summary>
        /// Der Index der Kaste des Insekts in der Kasten-Struktur des Spielers.
        /// </summary>
        internal int CasteIndexBase;

        /// <summary>
        /// Das Volk zu dem das Insekts geh�rt.
        /// </summary>
        internal CoreColony colony;

        private ICoordinate ziel = null;
        private int zur�ckgelegteStreckeI;

        internal CoreInsect() { }

        /// <summary>
        /// Die Kaste des Insekts.
        /// </summary>
        internal string KasteBase
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
        /// Anzahl Ameisen aus befreundeten V�lkern in sichtweite des Insekts.
        /// </summary>
        internal int TeamAntsInViewrange
        {
            get { return teamCount; }
            set { teamCount = value; }
        }

        /// <summary>
        /// Die Richtung in die das Insekt gedreht ist.
        /// </summary>
        internal int RichtungBase
        {
            get { return koordinate.Richtung; }
        }

        /// <summary>
        /// Die Strecke die die Ameise zur�ckgelegt hat, seit sie das letzte Mal in
        /// einem Ameisenbau war.
        /// </summary>
        internal int Zur�ckgelegteStreckeBase
        {
            get { return zur�ckgelegteStreckeI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Strecke die die Ameise zur�ckgelegt hat, seit sie das letzte Mal in
        /// einem Ameisenbau war in der internen Einheit.
        /// </summary>
        internal int Zur�ckgelegteStreckeI
        {
            get { return zur�ckgelegteStreckeI; }
            set { zur�ckgelegteStreckeI = value; }
        }

        /// <summary>
        /// Die Strecke die das Insekt geradeaus gehen wird, bevor das n�chste Mal
        /// Wartet() aufgerufen wird bzw. das Insekt sich zu seinem Ziel ausrichtet.
        /// </summary>
        internal int RestStreckeBase
        {
            get { return restStreckeI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Strecke die das Insekt geradeaus gehen wird, bevor das n�chste 
        /// Mal Wartet() aufgerufen wird bzw. das Insekt sich zu seinem Ziel
        /// ausrichtet in der internen Einheit.
        /// </summary>
        internal int RestStreckeI
        {
            get { return restStreckeI; }
            set { restStreckeI = value; }
        }

        /// <summary>
        /// Der Winkel um den das Insekt sich noch drehen mu�, bevor es wieder
        /// geradeaus gehen kann.
        /// </summary>
        internal int RestWinkelBase
        {
            get { return restWinkel; }
            set
            {
                // TODO: Modulo?
                restWinkel = value;
                while (restWinkel > 180)
                {
                    restWinkel -= 360;
                }
                while (restWinkel < -180)
                {
                    restWinkel += 360;
                }
            }
        }

        /// <summary>
        /// Das Ziel auf das das Insekt zugeht.
        /// </summary>
        internal ICoordinate ZielBase
        {
            get { return ziel; }
            set
            {
                if (ziel != value || value == null)
                {
                    ziel = value;
                    restWinkel = 0;
                    restStreckeI = 0;
                }
            }
        }

        /// <summary>
        /// Liefert die Entfernung in Schritten zum n�chsten Ameisenbau.
        /// </summary>
        internal int EntfernungZuBauBase
        {
            get
            {
                int aktuelleEntfernung;
                int gemerkteEntfernung = int.MaxValue;
                foreach (CoreAnthill bau in colony.AntHills)
                {
                    aktuelleEntfernung = CoreCoordinate.BestimmeEntfernungI(CoordinateBase, bau.CoordinateBase);
                    if (aktuelleEntfernung < gemerkteEntfernung)
                    {
                        gemerkteEntfernung = aktuelleEntfernung;
                    }
                }
                return gemerkteEntfernung / SimulationEnvironment.PLAYGROUND_UNIT;
            }
        }

        /// <summary>
        /// Gibt das Obst zur�ck, das das Insekt gerade tr�gt.
        /// </summary>
        internal CoreFruit GetragenesObstBase
        {
            get { return getragenesObst; }
            set { getragenesObst = value; }
        }

        /// <summary>
        /// Gibt zur�ck on das Insekt bei seinem Ziel angekommen ist.
        /// </summary>
        internal bool AngekommenBase
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
            get { return koordinate; }
            internal set { koordinate = value; }
        }

        #endregion

        /// <summary>
        /// Der abstrakte Insekt-Basiskonstruktor.
        /// </summary>
        /// <param name="colony">Das Volk zu dem das neue Insekt geh�rt.</param>
        /// <param name="vorhandeneInsekten">Hier unbenutzt!</param>
        internal virtual void Init(CoreColony colony, Random random, Dictionary<string, int> vorhandeneInsekten)
        {
            id = neueId;
            neueId++;

            this.colony = colony;
            this.RandomBase = random;

            koordinate.Richtung = RandomBase.Next(0, 359);

            // Zuf�llig auf dem Spielfeldrand platzieren.
            if (colony.AntHills.Count == 0) // Am oberen oder unteren Rand platzieren.
            {
                if (RandomBase.Next(2) == 0)
                {
                    koordinate.X = RandomBase.Next(0, colony.Playground.Width);
                    koordinate.X *= SimulationEnvironment.PLAYGROUND_UNIT;
                    if (RandomBase.Next(2) == 0)
                    {
                        koordinate.Y = 0;
                    }
                    else
                    {
                        koordinate.Y = colony.Playground.Height * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                }

                // Am linken oder rechten Rand platzieren.
                else
                {
                    if (RandomBase.Next(2) == 0)
                    {
                        koordinate.X = 0;
                    }
                    else
                    {
                        koordinate.X = colony.Playground.Width * SimulationEnvironment.PLAYGROUND_UNIT;
                    }
                    koordinate.Y = RandomBase.Next(0, colony.Playground.Height);
                    koordinate.Y *= SimulationEnvironment.PLAYGROUND_UNIT;
                }
            }

            // In einem zuf�lligen Bau platzieren.
            else
            {
                int i = RandomBase.Next(colony.AntHills.Count);
                koordinate.X = colony.AntHills[i].CoordinateBase.X +
                               SimulationEnvironment.Cosinus(
                                   colony.AntHills[i].CoordinateBase.Radius, koordinate.Richtung);
                koordinate.Y = colony.AntHills[i].CoordinateBase.Y +
                               SimulationEnvironment.Sinus(
                                   colony.AntHills[i].CoordinateBase.Radius, koordinate.Richtung);
            }
        }

        /// <summary>
        /// Gibt an, ob weitere Insekten ben�tigt werden, um ein St�ck Obst zu
        /// tragen.
        /// </summary>
        /// <param name="obst">Obst</param>
        /// <returns></returns>
        internal bool BrauchtNochTr�ger(CoreFruit obst)
        {
            return obst.BrauchtNochTr�ger(colony);
        }

        /// <summary>
        /// Dreht das Insekt um den angegebenen Winkel. Die maximale Drehung betr�gt
        /// -180 Grad (nach links) bzw. 180 Grad (nach rechts). Gr��ere Werte werden
        /// abgeschnitten.
        /// </summary>
        /// <param name="winkel">Winkel</param>
        internal void DreheUmWinkelBase(int winkel)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            RestWinkelBase = winkel;
        }

        /// <summary>
        /// Dreht das Insekt in die angegebene Richtung (Grad).
        /// </summary>
        /// <param name="richtung">Richtung</param>
        internal void DreheInRichtungBase(int richtung)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            dreheInRichtung(richtung);
        }

        private void dreheInRichtung(int richtung)
        {
            RestWinkelBase = richtung - koordinate.Richtung;
        }

        /// <summary>
        /// Dreht das Insekt in die Richtung des angegebenen Ziels.
        /// </summary>
        /// <param name="ziel">Ziel</param>
        internal void DreheZuZielBase(ICoordinate ziel)
        {
            DreheInRichtungBase(CoreCoordinate.BestimmeRichtung(this, ziel));
        }

        /// <summary>
        /// Dreht das Insekt um 180 Grad.
        /// </summary>
        internal void DreheUmBase()
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            if (restWinkel > 0)
            {
                restWinkel = 180;
            }
            else
            {
                restWinkel = -180;
            }
        }

        /// <summary>
        /// L�sst das Insekt unbegrenzt geradeaus gehen.
        /// </summary>
        internal void GeheGeradeausBase()
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            restStreckeI = int.MaxValue;
        }

        /// <summary>
        /// L�sst das Insekt die angegebene Entfernung in Schritten geradeaus gehen.
        /// </summary>
        /// <param name="entfernung">Die Entfernung.</param>
        internal void GeheGeradeausBase(int entfernung)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            restStreckeI = entfernung * SimulationEnvironment.PLAYGROUND_UNIT;
        }

        /// <summary>
        /// L�sst das Insekt auf ein Ziel zugehen. Das Ziel darf sich bewegen.
        /// Wenn das Ziel eine Wanze ist, wird dieser angegriffen.
        /// </summary>
        /// <param name="ziel">Das Ziel.</param>
        internal void GeheZuZielBase(ICoordinate ziel)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            ZielBase = ziel;
        }

        /// <summary>
        /// L�sst das Insekt ein Ziel angreifen. Das Ziel darf sich bewegen.
        /// In der aktuellen Version kann das Ziel nur eine Wanze sein.
        /// </summary>
        /// <param name="ziel">Ziel</param>
        internal void GreifeAnBase(CoreInsect ziel)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            ZielBase = ziel;
        }

        /// <summary>
        /// L�sst das Insekt von der aktuellen Position aus entgegen der Richtung zu
        /// einer Quelle gehen. Wenn die Quelle ein Insekt eines anderen Volkes ist,
        /// befindet sich das Insekt auf der Flucht.
        /// </summary>
        /// <param name="quelle">Die Quelle.</param> 
        internal void GeheWegVonBase(ICoordinate quelle)
        {
            DreheInRichtungBase(CoreCoordinate.BestimmeRichtung(this, quelle) + 180);
            GeheGeradeausBase();
        }

        /// <summary>
        /// L�sst das Insekt von der aktuellen Position aus die angegebene
        /// Entfernung in Schritten entgegen der Richtung zu einer Quelle gehen.
        /// Wenn die Quelle ein Insekt eines anderen Volkes ist, befindet sich das
        /// Insekt auf der Flucht.
        /// </summary>
        /// <param name="quelle">Die Quelle.</param> 
        /// <param name="entfernung">Die Entfernung in Schritten.</param>
        internal void GeheWegVonBase(ICoordinate quelle, int entfernung)
        {
            DreheInRichtungBase(CoreCoordinate.BestimmeRichtung(this, quelle) + 180);
            GeheGeradeausBase(entfernung);
        }

        /// <summary>
        /// L�sst das Insekt zum n�chsten Bau gehen.
        /// </summary>
        internal void GeheZuBauBase()
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            int aktuelleEntfernung;
            int gemerkteEntfernung = int.MaxValue;
            CoreAnthill gemerkterBau = null;
            foreach (CoreAnthill bau in colony.AntHills)
            {
                aktuelleEntfernung = CoreCoordinate.BestimmeEntfernungI(CoordinateBase, bau.CoordinateBase);
                if (aktuelleEntfernung < gemerkteEntfernung)
                {
                    gemerkterBau = bau;
                    gemerkteEntfernung = aktuelleEntfernung;
                }
            }
            GeheZuZielBase(gemerkterBau);
        }

        /// <summary>
        /// L�sst das Insekt anhalten. Dabei geht sein Ziel verloren.
        /// </summary>
        internal void BleibStehenBase()
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            ZielBase = null;
            restStreckeI = 0;
            restWinkel = 0;
        }

        /// <summary>
        /// L�sst das Insekt Zucker von einem Zuckerhaufen nehmen.
        /// </summary>
        /// <param name="zucker">Zuckerhaufen</param>
        internal void NimmBase(CoreSugar zucker)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            int entfernung = CoreCoordinate.BestimmeEntfernungI(CoordinateBase, zucker.CoordinateBase);
            if (entfernung <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                int menge = Math.Min(MaximaleLastBase - aktuelleLast, zucker.Menge);
                AktuelleLastBase += menge;
                zucker.Menge -= menge;
            }
            else
            {
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// L�sst das Insekt ein Obstst�ck nehmen.
        /// </summary>
        /// <param name="obst">Das Obstst�ck.</param>
        internal void NimmBase(CoreFruit obst)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            if (GetragenesObstBase == obst)
            {
                return;
            }
            if (GetragenesObstBase != null)
            {
                LasseNahrungFallenBase();
            }
            int entfernung = CoreCoordinate.BestimmeEntfernungI(CoordinateBase, obst.CoordinateBase);
            if (entfernung <= SimulationEnvironment.PLAYGROUND_UNIT)
            {
                BleibStehenBase();
                GetragenesObstBase = obst;
                obst.TragendeInsekten.Add(this);
                AktuelleLastBase = colony.Last[CasteIndexBase];
            }
        }

        /// <summary>
        /// L�sst das Insekt die aktuell getragene Nahrung fallen. Das Ziel des
        /// Insekts geht dabei verloren.
        /// </summary>
        internal void LasseNahrungFallenBase()
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            AktuelleLastBase = 0;
            ZielBase = null;
            if (GetragenesObstBase != null)
            {
                GetragenesObstBase.TragendeInsekten.Remove(this);
                GetragenesObstBase = null;
            }
        }

        /// <summary>
        /// L�sst die Ameise eine Markierung spr�hen. Die Markierung enth�lt die
        /// angegebene Information und breitet sich um die angegebene Anzahl an
        /// Schritten weiter aus. Je weiter sich eine Markierung ausbreitet, desto
        /// k�rzer bleibt sie aktiv.
        /// </summary>
        /// <param name="information">Die Information.</param>
        /// <param name="ausbreitung">Die Ausbreitung in Schritten.</param>
        internal void Spr�heMarkierungBase(int information, int ausbreitung)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }

            // Check for unsupported markersize
            if (ausbreitung < 0)
            {
                throw new AiException(string.Format("{0}: {1}", colony.Player.Guid,
                    Resource.SimulationCoreNegativeMarkerSize));
            }

            CoreMarker markierung = new CoreMarker(koordinate, ausbreitung, colony.Id);
            markierung.Information = information;
            colony.NewMarker.Add(markierung);
            SmelledMarker.Add(markierung);
        }

        /// <summary>
        /// L�sst die Ameise eine Markierung spr�hen. Die Markierung enth�lt die
        /// angegebene Information und breitet sich nicht aus. So hat die Markierung
        /// die maximale Lebensdauer.
        /// </summary>
        /// <param name="information">Die Information.</param>
        internal void Spr�heMarkierungBase(int information)
        {
            if (!NimmBefehleEntgegen)
            {
                return;
            }
            Spr�heMarkierungBase(information, 0);
        }

        /// <summary>
        /// Berechnet die Bewegung des Insekts.
        /// </summary>
        internal void Bewegen()
        {
            reached = false;

            // Insekt dreht sich.
            if (restWinkel != 0)
            {
                // Zielwinkel wird erreicht.
                if (Math.Abs(restWinkel) < colony.Drehgeschwindigkeit[CasteIndexBase])
                {
                    koordinate.Richtung += restWinkel;
                    restWinkel = 0;
                }

                // Insekt dreht sich nach rechts.
                else if (restWinkel >= colony.Drehgeschwindigkeit[CasteIndexBase])
                {
                    koordinate.Richtung += colony.Drehgeschwindigkeit[CasteIndexBase];
                    RestWinkelBase -= colony.Drehgeschwindigkeit[CasteIndexBase];
                }

                // Insekt dreht sich nach links.
                else if (restWinkel <= -colony.Drehgeschwindigkeit[CasteIndexBase])
                {
                    koordinate.Richtung -= colony.Drehgeschwindigkeit[CasteIndexBase];
                    RestWinkelBase += colony.Drehgeschwindigkeit[CasteIndexBase];
                }
            }

            // Insekt geht.
            else if (restStreckeI > 0)
            {
                if (GetragenesObstBase == null)
                {
                    int strecke = Math.Min(restStreckeI, aktuelleGeschwindigkeitI);

                    restStreckeI -= strecke;
                    zur�ckgelegteStreckeI += strecke;
                    koordinate.X += SimulationEnvironment.Cos[strecke, koordinate.Richtung];
                    koordinate.Y += SimulationEnvironment.Sin[strecke, koordinate.Richtung];
                }
            }

            // Insekt geht auf Ziel zu.
            else if (ziel != null)
            {
                int entfernungI;

                if (ZielBase is CoreMarker)
                {
                    entfernungI = CoreCoordinate.BestimmeEntfernungDerMittelpunkteI(koordinate, ziel.CoordinateBase);
                }
                else
                {
                    entfernungI = CoreCoordinate.BestimmeEntfernungI(koordinate, ziel.CoordinateBase);
                }

                reached = entfernungI <= SimulationEnvironment.PLAYGROUND_UNIT;
                if (!reached)
                {
                    int richtung = CoreCoordinate.BestimmeRichtung(koordinate, ziel.CoordinateBase);

                    // Ziel ist in Sichtweite oder Insekt tr�gt Obst.
                    if (entfernungI < colony.SichtweiteI[CasteIndexBase] || getragenesObst != null)
                    {
                        restStreckeI = entfernungI;
                    }

                    // Ansonsten Richtung verf�lschen.
                    else
                    {
                        richtung += RandomBase.Next(-18, 18);
                        restStreckeI = colony.SichtweiteI[CasteIndexBase];
                    }

                    dreheInRichtung(richtung);
                }
            }

            // Koordinaten links begrenzen.
            if (koordinate.X < 0)
            {
                koordinate.X = -koordinate.X;
                if (koordinate.Richtung > 90 && koordinate.Richtung <= 180)
                {
                    koordinate.Richtung = 180 - koordinate.Richtung;
                }
                else if (koordinate.Richtung > 180 && koordinate.Richtung < 270)
                {
                    koordinate.Richtung = 540 - koordinate.Richtung;
                }
            }

            // Koordinaten rechts begrenzen.
            else if (koordinate.X > colony.BreiteI)
            {
                koordinate.X = colony.BreiteI2 - koordinate.X;
                if (koordinate.Richtung >= 0 && koordinate.Richtung < 90)
                {
                    koordinate.Richtung = 180 - koordinate.Richtung;
                }
                else if (koordinate.Richtung > 270 && koordinate.Richtung < 360)
                {
                    koordinate.Richtung = 540 - koordinate.Richtung;
                }
            }

            // Koordinaten oben begrenzen.
            if (koordinate.Y < 0)
            {
                koordinate.Y = -koordinate.Y;
                if (koordinate.Richtung > 180 && koordinate.Richtung < 360)
                {
                    koordinate.Richtung = 360 - koordinate.Richtung;
                }
            }

            // Koordinaten unten begrenzen.
            else if (koordinate.Y > colony.H�heI)
            {
                koordinate.Y = colony.H�heI2 - koordinate.Y;
                if (koordinate.Richtung > 0 && koordinate.Richtung < 180)
                {
                    koordinate.Richtung = 360 - koordinate.Richtung;
                }
            }
        }

        #region Geschwindigkeit

        /// <summary>
        /// Die aktuelle Geschwindigkeit des Insekts in der internen Einheit.
        /// </summary>
        internal int aktuelleGeschwindigkeitI;

        /// <summary>
        /// Die aktuelle Geschwindigkeit des Insekts in Schritten. Wenn das Insekt
        /// seine maximale Last tr�gt, halbiert sich seine Geschwindigkeit.
        /// </summary>
        internal int AktuelleGeschwindigkeitBase
        {
            get { return aktuelleGeschwindigkeitI / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die maximale Geschwindigkeit des Insekts.
        /// </summary>
        internal int MaximaleGeschwindigkeitBase
        {
            get { return colony.GeschwindigkeitI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        #endregion

        #region Drehgeschwindigkeit

        /// <summary>
        /// Die Drehgeschwindigkeit des Insekts in Grad pro Runde.
        /// </summary>
        internal int DrehgeschwindigkeitBase
        {
            get { return colony.Drehgeschwindigkeit[CasteIndexBase]; }
        }

        #endregion

        #region Last

        private int aktuelleLast = 0;

        /// <summary>
        /// Die Last die die Ameise gerade tr�gt.
        /// </summary>
        internal int AktuelleLastBase
        {
            get { return aktuelleLast; }
            set
            {
                aktuelleLast = value >= 0 ? value : 0;
                aktuelleGeschwindigkeitI = colony.GeschwindigkeitI[CasteIndexBase];
                aktuelleGeschwindigkeitI -= aktuelleGeschwindigkeitI * aktuelleLast / colony.Last[CasteIndexBase] / 2;
            }
        }

        /// <summary>
        /// Die maximale Last die das Insekt tragen kann.
        /// </summary>
        internal int MaximaleLastBase
        {
            get { return colony.Last[CasteIndexBase]; }
        }

        #endregion

        #region Sichtweite

        /// <summary>
        /// Die Sichtweite des Insekts in Schritten.
        /// </summary>
        internal int SichtweiteBase
        {
            get { return colony.SichtweiteI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Sichtweite des Insekts in der internen Einheit.
        /// </summary>
        internal int SichtweiteI
        {
            get { return colony.SichtweiteI[CasteIndexBase]; }
        }

        #endregion

        #region Reichweite

        /// <summary>
        /// Die Reichweite des Insekts in Schritten.
        /// </summary>
        internal int ReichweiteBase
        {
            get { return colony.ReichweiteI[CasteIndexBase] / SimulationEnvironment.PLAYGROUND_UNIT; }
        }

        /// <summary>
        /// Die Reichweite des Insekts in der internen Einheit.
        /// </summary>
        internal int ReichweiteI
        {
            get { return colony.ReichweiteI[CasteIndexBase]; }
        }

        #endregion

        #region Energie

        private int aktuelleEnergie;

        /// <summary>
        /// Die verbleibende Energie des Insekts.
        /// </summary>
        internal int AktuelleEnergieBase
        {
            get { return aktuelleEnergie; }
            set { aktuelleEnergie = value >= 0 ? value : 0; }
        }

        /// <summary>
        /// Die maximale Energie des Insetks.
        /// </summary>
        internal int MaximaleEnergieBase
        {
            get { return colony.Energie[CasteIndexBase]; }
        }

        #endregion

        #region Angriff

        private int angriff;

        /// <summary>
        /// Die Angriffst�rke des Insekts. Wenn das Insekt Last tr�gt ist die
        /// Angriffst�rke gleich Null.
        /// </summary>
        internal int AngriffBase
        {
            get { return aktuelleLast == 0 ? angriff : 0; }
            set { angriff = value >= 0 ? value : 0; }
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