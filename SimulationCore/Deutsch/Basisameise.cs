using System.Collections.Generic;
using AntMe.Simulation;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Basisklasse für die Implementierung einer neuen Ameise
    /// </summary>
    public abstract class Basisameise : CoreAnt
    {
        #region Event-Wrapper

        internal override string BestimmeKasteBase(Dictionary<string, int> anzahl)
        {
            return BestimmeKaste(anzahl);
        }

        /// <summary>
        /// Legt die Kaste der Ameise fest die als nächstes geboren wird
        /// </summary>
        /// <param name="anzahl">Liste von Ameisenkasten und deren aktuell lebende Anzahl</param>
        /// <returns>Zeichenkette des neuen Ameisenkaste</returns>
        public virtual string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            return "";
        }

        internal override void IstGestorbenBase(CoreKindOfDeath todesArt)
        {
            IstGestorben((Todesart) (int) todesArt);
        }

        /// <summary>
        /// Wird aufgerufen, wenn eine Ameise stirbt
        /// </summary>
        /// <param name="todesart">Art des Todes</param>
        public virtual void IstGestorben(Todesart todesart)
        {
        }

        internal override void RiechtFreundBase(CoreMarker markierung)
        {
            RiechtFreund(new Markierung(markierung));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise eine Markierung riecht
        /// </summary>
        /// <param name="markierung">gerochene Markierung</param>
        public virtual void RiechtFreund(Markierung markierung)
        {
        }

        internal override void SiehtBase(CoreFruit obst)
        {
            Sieht(new Obst(obst));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise Obst sieht
        /// </summary>
        /// <param name="obst">obst</param>
        public virtual void Sieht(Obst obst)
        {
        }

        internal override void SiehtBase(CoreSugar zucker)
        {
            Sieht(new Zucker(zucker));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise Zucker sieht
        /// </summary>
        /// <param name="zucker">Verweis auf den Zucker</param>
        public virtual void Sieht(Zucker zucker)
        {
        }

        internal override void SiehtFeindBase(CoreAnt ameise)
        {
            SiehtFeind(new Ameise(ameise));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise eine feindliche Ameise sieht
        /// </summary>
        /// <param name="ameise">feindliche Ameise</param>
        public virtual void SiehtFeind(Ameise ameise)
        {
        }

        internal override void SiehtFeindBase(CoreBug wanze)
        {
            SiehtFeind(new Wanze(wanze));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise eine Wanze trifft
        /// </summary>
        /// <param name="wanze">Wanze</param>
        public virtual void SiehtFeind(Wanze wanze)
        {
        }

        internal override void SiehtFreundBase(CoreAnt ameise)
        {
            SiehtFreund(new Ameise(ameise));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise eine befreundete Ameise trifft
        /// </summary>
        /// <param name="ameise">befreundete Ameise</param>
        public virtual void SiehtFreund(Ameise ameise)
        {
        }

        internal override void SiehtVerbündetenBase(CoreAnt ameise)
        {
            SiehtVerbündeten(new Ameise(ameise));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise eine befreundete Ameise eines anderen Teams trifft.
        /// </summary>
        /// <param name="ameise"></param>
        public virtual void SiehtVerbündeten(Ameise ameise) {}

        internal override void TickBase()
        {
            Tick();
        }

        /// <summary>
        /// Wird in jeder Runde aufgerufen
        /// </summary>
        public virtual void Tick()
        {
        }

        internal override void WartetBase()
        {
            Wartet();
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise keinen Arbeitsauftrag mehr hat
        /// </summary>
        public virtual void Wartet()
        {
        }

        internal override void WirdAngegriffenBase(CoreAnt ameise)
        {
            WirdAngegriffen(new Ameise(ameise));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise von einer feindlichen Ameise attackiert wird
        /// </summary>
        /// <param name="ameise">feindliche Ameise</param>
        public virtual void WirdAngegriffen(Ameise ameise)
        {
        }

        internal override void WirdAngegriffenBase(CoreBug wanze)
        {
            WirdAngegriffen(new Wanze(wanze));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise von einer Wanze attackiert wird
        /// </summary>
        /// <param name="wanze">Wanze</param>
        public virtual void WirdAngegriffen(Wanze wanze)
        {
        }

        internal override void WirdMüdeBase()
        {
            WirdMüde();
        }

        /// <summary>
        /// Wird aufgerufen, sobald die Ameise ein Drittel ihrer Reichweite zurückgelegt hat
        /// </summary>
        public virtual void WirdMüde()
        {
        }

        internal override void ZielErreichtBase(CoreFruit obst)
        {
            ZielErreicht(new Obst(obst));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise am anvisierten Obst angekommen ist
        /// </summary>
        /// <param name="obst">anvisiertes Obst</param>
        public virtual void ZielErreicht(Obst obst)
        {
        }

        internal override void ZielErreichtBase(CoreSugar zucker)
        {
            ZielErreicht(new Zucker(zucker));
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Ameise am anvisierten Zuckerberg angekommen ist
        /// </summary>
        /// <param name="zucker">anvisierter Zuckerberg</param>
        public virtual void ZielErreicht(Zucker zucker)
        {
        }

        #endregion

        #region Befehlswrapper

        /// <summary>
        /// Dreht die Ameise in die angegebene Richtung
        /// </summary>
        /// <param name="richtung">Zielrichtung</param>
        public void DreheInRichtung(int richtung)
        {
            DreheInRichtungBase(richtung);
        }

        /// <summary>
        /// Dreht die Ameise um den angegebenen Winkel
        /// </summary>
        /// <param name="winkel">winkel</param>
        public void DreheUmWinkel(int winkel)
        {
            DreheUmWinkelBase(winkel);
        }

        /// <summary>
        /// Dreht die Ameise in die entgegengesetzt Richtung
        /// </summary>
        public void DreheUm()
        {
            DreheUmBase();
        }

        /// <summary>
        /// Dreht die Ameise in Richtung des Ziels
        /// </summary>
        /// <param name="ziel">anvisiertes Ziel</param>
        public void DreheZuZiel(Spielobjekt ziel)
        {
            DreheZuZielBase(ziel.Element);
        }

        /// <summary>
        /// Lässt die Ameise sofort anhalten
        /// </summary>
        public void BleibStehen()
        {
            BleibStehenBase();
        }

        /// <summary>
        /// Lässt die Ameise geradeaus gehen
        /// </summary>
        public void GeheGeradeaus()
        {
            GeheGeradeausBase();
        }

        /// <summary>
        /// Lässt die Ameise die angegebenen Schritte geradeaus laufen
        /// </summary>
        /// <param name="entfernung">zu laufende Strecke</param>
        public void GeheGeradeaus(int entfernung)
        {
            GeheGeradeausBase(entfernung);
        }

        /// <summary>
        /// Lässt die Ameise in entgegengesetzte Richtung davon laufen
        /// </summary>
        /// <param name="ziel">Objekt, vor dem weggerannt werden soll</param>
        public void GeheWegVon(Spielobjekt ziel)
        {
            GeheWegVonBase(ziel.Element);
        }

        /// <summary>
        /// Lässt die Ameise in entgegengesetzte Richtung davon laufen
        /// </summary>
        /// <param name="ziel">Objekt, vor dem weggerannt werden soll</param>
        /// <param name="entfernung">Entfernung, die zurückgelegt werden soll</param>
        public void GeheWegVon(Spielobjekt ziel, int entfernung) {
            GeheWegVonBase(ziel.Element, entfernung);
        }

        /// <summary>
        /// Lässt die Ameise zum angegebenen Ziel laufen
        /// </summary>
        /// <param name="ziel">Ziel</param>
        public void GeheZuZiel(Spielobjekt ziel)
        {
            GeheZuZielBase(ziel.Element);
        }

        /// <summary>
        /// Lässt die Ameise zurück zum Bau laufen
        /// </summary>
        public void GeheZuBau()
        {
            GeheZuBauBase();
        }

        /// <summary>
        /// Lässt die Ameise das angegebene Insekt angreifen
        /// </summary>
        /// <param name="ziel">Angriffsziel</param>
        public void GreifeAn(Insekt ziel)
        {
            GreifeAnBase((CoreInsect) ziel.Element);
        }

        /// <summary>
        /// Nimmt die angegebene Nahrung auf
        /// </summary>
        /// <param name="nahrung">Nahrung</param>
        public void Nimm(Nahrung nahrung)
        {
            if (nahrung is Zucker)
            {
                NimmBase((CoreSugar) nahrung.Element);
            }
            else if (nahrung is Obst)
            {
                NimmBase((CoreFruit) nahrung.Element);
            }
        }

        /// <summary>
        /// Lässt die gerade getragene Nahrung fallen
        /// </summary>
        public void LasseNahrungFallen()
        {
            LasseNahrungFallenBase();
        }

        /// <summary>
        /// Sprüht eine Markierung mit minimaler Größe
        /// </summary>
        /// <param name="information">Information</param>
        public void SprüheMarkierung(int information)
        {
            SprüheMarkierungBase(information);
        }

        /// <summary>
        /// Sprüht eine Markierung mit angegebener Größe
        /// </summary>
        /// <param name="information">Information</param>
        /// <param name="größe">Größe der Markierung</param>
        public void SprüheMarkierung(int information, int größe)
        {
            SprüheMarkierungBase(information, größe);
        }

        /// <summary>
        /// Lässt die Ameise über diese Nachricht nachdenken. 
        /// Im Debug-Modus wird das als Denkblase über der Ameise angezeigt.
        /// </summary>
        /// <param name="nachricht">Nachricht</param>
        public void Denke(string nachricht)
        {
            DenkeCore(nachricht);
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Liefert die maximale Energie
        /// </summary>
        public int MaximaleEnergie
        {
            get { return MaximaleEnergieBase; }
        }

        /// <summary>
        /// Liefert die maximale Geschwindigkeit
        /// </summary>
        public int MaximaleGeschwindigkeit
        {
            get { return MaximaleGeschwindigkeitBase; }
        }

        /// <summary>
        /// Liefert die maximale Last
        /// </summary>
        public int MaximaleLast
        {
            get { return MaximaleLastBase; }
        }

        /// <summary>
        /// Gibt die Reichweite an
        /// </summary>
        public int Reichweite
        {
            get { return ReichweiteBase; }
        }

        /// <summary>
        /// Angriffswert
        /// </summary>
        public int Angriff
        {
            get { return AngriffBase; }
        }

        /// <summary>
        /// Sichtweite der Ameise
        /// </summary>
        public int Sichtweite
        {
            get { return SichtweiteBase; }
        }

        /// <summary>
        /// Drehgeschwindigkeit
        /// </summary>
        public int Drehgeschwindigkeit
        {
            get { return DrehgeschwindigkeitBase; }
        }

        /// <summary>
        /// Aktuelle Energie
        /// </summary>
        public int AktuelleEnergie
        {
            get { return AktuelleEnergieBase; }
        }

        /// <summary>
        /// Aktuelle Geschwindigkeit
        /// </summary>
        public int AktuelleGeschwindigkeit
        {
            get { return AktuelleGeschwindigkeitBase; }
        }

        /// <summary>
        /// Aktuelle Last
        /// </summary>
        public int AktuelleLast
        {
            get { return AktuelleLastBase; }
        }

        /// <summary>
        /// Anzahl befreundeter Ameisen in Sichtweite
        /// </summary>
        public int AnzahlAmeisenInSichtweite
        {
            get { return FriendlyAntsInViewrange; }
        }

        /// <summary>
        /// Anzahl befreundeter Ameisen aus der selben Kaste in Sichtweite
        /// </summary>
        public int AnzahlAmeisenDerSelbenKasteInSichtweite {
            get { return FriendlyAntsFromSameCasteInViewrange; }
        }

        /// <summary>
        /// Anzahl Ameisen aus Völkern des eigenen Teams in Sichtweite.
        /// </summary>
        public int AnzahlAmeisenDesTeamsInSichtweite {
            get { return TeamAntsInViewrange; }
        }

        /// <summary>
        /// Anzahl fremder Ameisen aus anderen Teams in Sichtweite.
        /// </summary>
        public int AnzahlFremderAmeisenInSichtweite {
            get { return ForeignAntsInViewrange; }
        }

        /// <summary>
        /// Anzahl Wanzen in Sichtweite.
        /// </summary>
        public int WanzenInSichtweite {
            get { return BugsInViewrange; }
        }

        /// <summary>
        /// Gibt die Entfernung zum nächsten Bau an
        /// </summary>
        public int EntfernungZuBau
        {
            get { return EntfernungZuBauBase; }
        }

        /// <summary>
        /// Liefert das aktuell getragene Obst
        /// </summary>
        public Obst GetragenesObst
        {
            get
            {
                if (GetragenesObstBase != null)
                {
                    return new Obst(GetragenesObstBase);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Liefert die Ameisenkaste
        /// </summary>
        public string Kaste
        {
            get { return KasteBase; }
        }

        /// <summary>
        /// Index der Ameisenkaste
        /// </summary>
        public int CasteIndex
        {
            get { return CasteIndexBase; }
        }

        /// <summary>
        /// Liefert das Ziel der Ameise
        /// </summary>
        public Spielobjekt Ziel
        {
            get
            {
                if (ZielBase is CoreSugar)
                {
                    return new Zucker((CoreSugar) ZielBase);
                }
                else if (ZielBase is CoreFruit)
                {
                    return new Obst((CoreFruit) ZielBase);
                }
                else if (ZielBase is CoreAnt)
                {
                    return new Ameise((CoreAnt) ZielBase);
                }
                else if (ZielBase is CoreBug)
                {
                    return new Wanze((CoreBug) ZielBase);
                }
                else if (ZielBase is CoreMarker)
                {
                    return new Markierung((CoreMarker) ZielBase);
                }
                else if (ZielBase is CoreAnthill)
                {
                    return new Bau((CoreAnthill) ZielBase);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Ist die Ameise müde
        /// </summary>
        public bool IstMüde
        {
            get { return IstMüdeBase; }
        }

        /// <summary>
        /// Verbleibende Reststrecke bis zum erreichen des Ziels
        /// </summary>
        public int RestStrecke
        {
            get { return RestStreckeBase; }
        }

        /// <summary>
        /// Verbleibende Drehung bis zur Ausrichtung zum Ziel
        /// </summary>
        public int RestWinkel
        {
            get { return RestWinkelBase; }
        }

        /// <summary>
        /// Ausrichtung der Ameise
        /// </summary>
        public int Richtung
        {
            get { return RichtungBase; }
        }

        /// <summary>
        /// Informiert darüber, ob die Ameise ihr Ziel erreicht hat
        /// </summary>
        public bool Angekommen
        {
            get { return AngekommenBase; }
        }

        /// <summary>
        /// Die, von der Ameise zurückgelgte Strecke
        /// </summary>
        public int ZurückgelegteStrecke
        {
            get { return ZurückgelegteStreckeBase; }
        }

        #endregion

        #region Hilfemethoden

        /// <summary>
        /// Ermittelt, ob das übergebene Stück Obst noch weitere Träger braucht
        /// </summary>
        /// <param name="obst">zu prüfendes Obst</param>
        /// <returns>Braucht noch Träger</returns>
        public bool BrauchtNochTräger(Obst obst)
        {
            return ((CoreFruit) obst.Element).BrauchtNochTräger(colony);
        }

        #endregion
    }
}