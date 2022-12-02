using AntMe.Simulation;
using System.Collections.Generic;

namespace AntMe.Deutsch
{
    /// <summary>
    /// Basisklasse für die Implementierung einer deutschen Ameise. Hier befinden sich 
    /// alle notwendigen Methoden, Eigenschaften und Events zur Interaktion mit der Umgebung.
    /// <see href="http://wiki.antme.net/de/Ameisenentwicklung">Weitere Infos</see>
    /// </summary>
    public abstract class Basisameise : CoreAnt
    {
        #region Event-Wrapper

        internal override string DetermineCasteCoreAnt(Dictionary<string, int> number)
        {
            return BestimmeKaste(number);
        }

        /// <summary>
        /// Jedes mal, wenn eine neue Ameise geboren wird, muss ihre Berufsgruppe
        /// bestimmt werden. Das kannst du mit Hilfe dieses Rückgabewertes dieser 
        /// Methode steuern.
        /// <see href="http://wiki.antme.net/de/API1:BestimmeKaste">Weitere Infos</see>
        /// </summary>
        /// <param name="anzahl">Anzahl Ameisen pro Kaste</param>
        /// <returns>Name der Kaste zu der die geborene Ameise gehören soll</returns>
        public virtual string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            return "";
        }

        internal override void HasDiedCoreAnt(CoreKindOfDeath todesArt)
        {
            IstGestorben((Todesart)(int)todesArt);
        }

        /// <summary>
        /// Wenn eine Ameise stirbt, wird diese Methode aufgerufen. Man erfährt dadurch, wie 
        /// die Ameise gestorben ist. Die Ameise kann zu diesem Zeitpunkt aber keinerlei Aktion 
        /// mehr ausführen.
        /// <see href="http://wiki.antme.net/de/API1:IstGestorben">Weitere Infos</see>
        /// </summary>
        /// <param name="todesart">Art des Todes</param>
        public virtual void IstGestorben(Todesart todesart)
        {
        }

        internal override void SpotsFriendCoreAnt(CoreMarker markierung)
        {
            RiechtFreund(new Markierung(markierung));
        }

        /// <summary>
        /// Markierungen, die von anderen Ameisen platziert werden, können von befreundeten Ameisen 
        /// gewittert werden. Diese Methode wird aufgerufen, wenn eine Ameise zum ersten Mal eine 
        /// befreundete Markierung riecht.
        /// <see href="http://wiki.antme.net/de/API1:RiechtFreund(Markierung)">Weitere Infos</see>
        /// </summary>
        /// <param name="markierung">Die gerochene Markierung</param>
        public virtual void RiechtFreund(Markierung markierung) { }

        internal override void SpotsCoreAnt(CoreFruit obst)
        {
            Sieht(new Obst(obst));
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Apfel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt das betroffene Stück Obst.
        /// <see href="http://wiki.antme.net/de/API1:Sieht(Obst)">Weitere Infos</see>
        /// </summary>
        /// <param name="obst">Das gesichtete Stück Obst</param>
        public virtual void Sieht(Obst obst) { }

        internal override void SpotsCoreAnt(CoreSugar zucker)
        {
            Sieht(new Zucker(zucker));
        }

        /// <summary>
        /// Sobald eine Ameise innerhalb ihres Sichtradius einen Zuckerhügel erspäht wird 
        /// diese Methode aufgerufen. Als Parameter kommt der betroffene Zuckerghügel.
        /// <see href="http://wiki.antme.net/de/API1:Sieht(Zucker)">Weitere Infos</see>
        /// </summary>
        /// <param name="zucker">Der gesichtete Zuckerhügel</param>
        public virtual void Sieht(Zucker zucker) { }

        internal override void SpotsEnemyCoreAnt(CoreAnt ameise)
        {
            SiehtFeind(new Ameise(ameise));
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem feindlichen Volk, 
        /// so wird diese Methode aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:SiehtFeind(Ameise)">Weitere Infos</see>
        /// </summary>
        /// <param name="ameise">Erspähte feindliche Ameise</param>
        public virtual void SiehtFeind(Ameise ameise) { }

        internal override void SpotsEnemyCoreAnt(CoreBug wanze)
        {
            SiehtFeind(new Wanze(wanze));
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Wanze, so wird diese Methode aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:SiehtFeind(Wanze)">Weitere Infos</see>
        /// </summary>
        /// <param name="wanze">Erspähte Wanze</param>
        public virtual void SiehtFeind(Wanze wanze) { }

        internal override void SpotsFriendCoreAnt(CoreAnt ameise)
        {
            SiehtFreund(new Ameise(ameise));
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus dem eigenen Volk, so 
        /// wird diese Methode aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:SiehtFreund(Ameise)">Weitere Infos</see>
        /// </summary>
        /// <param name="ameise">Erspähte befreundete Ameise</param>
        public virtual void SiehtFreund(Ameise ameise) { }

        internal override void SpotsTeamMemberCoreAnt(CoreAnt ameise)
        {
            SiehtVerbündeten(new Ameise(ameise));
        }

        /// <summary>
        /// So wie Ameisen unterschiedliche Nahrungsmittel erspähen können, entdecken Sie auch 
        /// andere Spielelemente. Entdeckt die Ameise eine Ameise aus einem befreundeten Volk 
        /// (Völker im selben Team), so wird diese Methode aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:SiehtVerb%C3%BCndeten(Ameise)">Weitere Infos</see>
        /// </summary>
        /// <param name="ameise">Erspähte verbündete Ameise</param>
        public virtual void SiehtVerbündeten(Ameise ameise) { }

        internal override void TickCoreAnt()
        {
            Tick();
        }

        /// <summary>
        /// Diese Methode wird in jeder Simulationsrunde aufgerufen - ungeachtet von zusätzlichen 
        /// Bedingungen. Dies eignet sich für Aktionen, die unter Bedingungen ausgeführt werden 
        /// sollen, die von den anderen Methoden nicht behandelt werden.
        /// <see href="http://wiki.antme.net/de/API1:Tick">Weitere Infos</see>
        /// </summary>
        public virtual void Tick() { }

        internal override void WaitingCoreAnt()
        {
            Wartet();
        }

        /// <summary>
        /// Wenn die Ameise keinerlei Aufträge hat, wartet sie auf neue Aufgaben. Um dir das 
        /// mitzuteilen, wird diese Methode hier aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:Wartet">Weitere Infos</see>
        /// </summary>
        public virtual void Wartet() { }

        internal override void IsUnderAttackCoreAnt(CoreAnt ameise)
        {
            WirdAngegriffen(new Ameise(ameise));
        }

        // <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine feindliche Ameise angreifen, wird diese Methode hier aufgerufen und die 
        /// Ameise kann entscheiden, wie sie darauf reagieren möchte.
        /// <see href="http://wiki.antme.net/de/API1:WirdAngegriffen(Ameise)">Weitere Infos</see>
        /// </summary>
        /// <param name="ameise">Angreifende Ameise</param>
        public virtual void WirdAngegriffen(Ameise ameise) { }

        internal override void UnderAttackCoreAnt(CoreBug wanze)
        {
            WirdAngegriffen(new Wanze(wanze));
        }

        /// <summary>
        /// Es kann vorkommen, dass feindliche Lebewesen eine Ameise aktiv angreifen. Sollte 
        /// eine Wanze angreifen, wird diese Methode hier aufgerufen und die Ameise kann 
        /// entscheiden, wie sie darauf reagieren möchte.
        /// <see href="http://wiki.antme.net/de/API1:WirdAngegriffen(Wanze)">Weitere Infos</see>
        /// </summary>
        /// <param name="wanze">Angreifende Wanze</param>
        public virtual void WirdAngegriffen(Wanze wanze) { }

        internal override void IsGettingTiredCoreAnt()
        {
            WirdMüde();
        }

        /// <summary>
        /// Erreicht eine Ameise ein drittel ihrer Laufreichweite, wird diese Methode aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:WirdM%C3%BCde">Weitere Infos</see>
        /// </summary>
        public virtual void WirdMüde() { }

        internal override void ArrivedAtTargetCoreAnt(CoreFruit obst)
        {
            ZielErreicht(new Obst(obst));
        }

        /// <summary>
        /// Hat die Ameise ein Stück Obst als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// <see href="http://wiki.antme.net/de/API1:ZielErreicht(Obst)">Weitere Infos</see>
        /// </summary>
        /// <param name="obst">Das erreichte Stück Obst</param>
        public virtual void ZielErreicht(Obst obst) { }

        internal override void ArrivedAtTargetCoreAnt(CoreSugar zucker)
        {
            ZielErreicht(new Zucker(zucker));
        }

        /// <summary>
        /// Hat die Ameise eine Zuckerhügel als Ziel festgelegt, wird diese Methode aufgerufen, 
        /// sobald die Ameise ihr Ziel erreicht hat. Ab jetzt ist die Ameise nahe genug um mit 
        /// dem Ziel zu interagieren.
        /// <see href="http://wiki.antme.net/de/API1:ZielErreicht(Zucker)">Weitere Infos</see>
        /// </summary>
        /// <param name="zucker">Der erreichte Zuckerhügel</param>
        public virtual void ZielErreicht(Zucker zucker) { }

        #endregion

        #region Befehlswrapper

        /// <summary>
        /// Die Ameise dreht sich in die angegebene Richtung. Die Drehrichtung wird 
        /// dabei automatisch bestimmt.
        /// <see href="http://wiki.antme.net/de/API1:DreheInRichtung">Weitere Infos</see>
        /// </summary>
        /// <param name="richtung">Zielrichtung</param>
        public void DreheInRichtung(int richtung)
        {
            TurnToDirectionCoreInsect(richtung);
        }

        /// <summary>
        /// Die Ameise dreht sich um den angegebenen Winkel. Positive Werte drehen 
        /// die Ameise nach rechts, negative nach links.
        /// <see href="http://wiki.antme.net/de/API1:DreheUmWinkel">Weitere Infos</see>
        /// </summary>
        /// <param name="winkel">Winkel</param>
        public void DreheUmWinkel(int winkel)
        {
            TurnByDegreesCoreInsect(winkel);
        }

        /// <summary>
        /// Die Ameise dreht sich um 180 Grad in die entgegengesetzte Richtung. Hat 
        /// die selbe Wirkung wie DreheUmWinkel(180).
        /// <see href="http://wiki.antme.net/de/API1:DreheUm">Weitere Infos</see>
        /// </summary>
        public void DreheUm()
        {
            TurnAroundCoreInsect();
        }

        /// <summary>
        /// Die Ameise dreht sich in die Richtung des angegebenen Ziels.
        /// <see href="http://wiki.antme.net/de/API1:DreheZuZiel">Weitere Infos</see>
        /// </summary>
        /// <param name="ziel">Anvisiertes Ziel</param>
        public void DreheZuZiel(Spielobjekt ziel)
        {
            TurnToTargetCoreInsect(ziel.Element);
        }

        /// <summary>
        /// Die Ameise bleibt stehen und vergisst ihr aktuelles Ziel. In der nächsten 
        /// Runde wird das Ereignis Wartet() aufgerufen werden.
        /// <see href="http://wiki.antme.net/de/API1:BleibStehen">Weitere Infos</see>
        /// </summary>
        public void BleibStehen()
        {
            StopMovementCoreInsect();
        }

        /// <summary>
        /// Die Ameise geht geradeaus. Das Ziel der Ameise bleibt unangetastet. Wenn 
        /// ein Wert angegeben wird, wird die Ameise wieder ihr Ziel anvisieren, 
        /// nachdem sie die angegebene Entfernung zurückgelegt hat.
        /// <see href="http://wiki.antme.net/de/API1:GeheGeradeaus">Weitere Infos</see>
        /// </summary>
        public void GeheGeradeaus()
        {
            GoForwardCoreInsect();
        }

        /// <summary>
        /// Die Ameise geht geradeaus. Das Ziel der Ameise bleibt unangetastet. Wenn 
        /// ein Wert angegeben wird, wird die Ameise wieder ihr Ziel anvisieren, 
        /// nachdem sie die angegebene Entfernung zurückgelegt hat.
        /// <see href="http://wiki.antme.net/de/API1:GeheGeradeaus">Weitere Infos</see>
        /// </summary>
        /// <param name="entfernung">Zu laufende Strecke in Ameisenschritten</param>
        public void GeheGeradeaus(int entfernung)
        {
            GoForwardCoreInsect(entfernung);
        }

        /// <summary>
        /// Die Ameise dreht sich in die Richtung die vom angegebenen Ziel weg zeigt 
        /// und geht dann geradeaus. Das Ziel der Ameise bleibt unangetastet und es 
        /// kann eine Entfernung angegeben werden.
        /// <see href="http://wiki.antme.net/de/API1:GeheWegVon">Weitere Infos</see>
        /// </summary>
        /// <param name="ziel">Objekt, vor dem weggegangen werden soll</param>
        public void GeheWegVon(Spielobjekt ziel)
        {
            GoAwayFromCoreInsect(ziel.Element);
        }

        /// <summary>
        /// Die Ameise dreht sich in die Richtung die vom angegebenen Ziel weg zeigt 
        /// und geht dann geradeaus. Das Ziel der Ameise bleibt unangetastet und es 
        /// kann eine Entfernung angegeben werden.
        /// <see href="http://wiki.antme.net/de/API1:GeheWegVon">Weitere Infos</see>
        /// </summary>
        /// <param name="ziel">Objekt, vor dem weggegangen werden soll</param>
        /// <param name="entfernung">Entfernung, die zurückgelegt werden soll</param>
        public void GeheWegVon(Spielobjekt ziel, int entfernung)
        {
            GoAwayFromCoreInsect(ziel.Element, entfernung);
        }

        /// <summary>
        /// Die Ameise speichert das angegebene Ziel und geht dort hin.
        /// <see href="http://wiki.antme.net/de/API1:GeheZuZiel">Weitere Infos</see>
        /// </summary>
        /// <param name="ziel">Ziel</param>
        public void GeheZuZiel(Spielobjekt ziel)
        {
            GoToTargetCoreInsect(ziel.Element);
        }

        /// <summary>
        /// Die Ameise speichert den nächstgelegenen Bau als Ziel und geht dort hin.
        /// <see href="http://wiki.antme.net/de/API1:GeheZuBau">Weitere Infos</see>
        /// </summary>
        public void GeheZuBau()
        {
            GoToAnthillCoreInsect();
        }

        /// <summary>
        /// Die Ameise speichert die angegebene Wanze oder die angegebene feindliche 
        /// Ameise als Ziel und geht dort hin. Wenn die Ameise angekommen ist, 
        /// beginnt der Kampf.
        /// <see href="http://wiki.antme.net/de/API1:GreifeAn">Weitere Infos</see>
        /// </summary>
        /// <param name="ziel">Angriffsziel</param>
        public void GreifeAn(Insekt ziel)
        {
            AttackCoreInsect((CoreInsect)ziel.Element);
        }

        /// <summary>
        /// Die Ameise nimmt die angegebene Nahrung auf. Bei einem Zuckerhaufen nimmt 
        /// sie so viel wie möglich weg, bis sie ihre maximale Last erreicht hat 
        /// (siehe <see cref="AktuelleLast" /> und <see cref="MaximaleLast" />).
        /// Im Falle eines Obststückes beginnt die Ameise das Obst zu tragen 
        /// (siehe <see cref="GetragenesObst" />).
        /// <see href="http://wiki.antme.net/de/API1:Nimm">Weitere Infos</see>
        /// </summary>
        /// <param name="nahrung">Nahrung</param>
        public void Nimm(Nahrung nahrung)
        {
            if (nahrung is Zucker)
            {
                TakeCoreInsect((CoreSugar)nahrung.Element);
            }
            else if (nahrung is Obst)
            {
                TakeCoreInsect((CoreFruit)nahrung.Element);
            }
        }

        /// <summary>
        /// Die Ameise lässt die gerade getragene Nahrung fallen. Zucker geht dabei 
        /// verloren, Äpfel bleiben liegen und können wieder aufgenommen werden. Der 
        /// Befehl muss nicht ausgeführt werden um Nahrung im Bau abzuliefern. Das 
        /// passiert dort automatisch.
        /// <see href="http://wiki.antme.net/de/API1:LasseNahrungFallen">Weitere Infos</see>
        /// </summary>
        public void LasseNahrungFallen()
        {
            DropFood();
        }

        /// <summary>
        /// Die Ameise sprüht an der aktuellen Stelle eine Duftmarkierung. Mögliche 
        /// Parameter sind eine in der Markierung hinterlegte Information (diese kann im 
        /// Ereignis Sieht(Markierung) über "markierung.Information" ausgelesen werden) und 
        /// die Ausbreitung der Markierung. Je größer die Ausbreitung, desto schneller 
        /// verschwindet die Markierung wieder.
        /// <see href="http://wiki.antme.net/de/API1:Spr%C3%BCheMarkierung">Weitere Infos</see>
        /// </summary>
        /// <param name="information">Information</param>
        public void SprüheMarkierung(int information)
        {
            MakeMarkerCoreInsects(information);
        }

        /// <summary>
        /// Die Ameise sprüht an der aktuellen Stelle eine Duftmarkierung. Mögliche Parameter 
        /// sind eine in der Markierung hinterlegte Information (diese kann im Ereignis 
        /// Sieht(Markierung) über markierung.Information ausgelesen werden) und die Ausbreitung 
        /// der Markierung. Je größer die Ausbreitung, desto schneller verschwindet die 
        /// Markierung wieder.
        /// <see href="http://wiki.antme.net/de/API1:Spr%C3%BCheMarkierung">Weitere Infos</see>
        /// </summary>
        /// <param name="information">Information</param>
        /// <param name="größe">Größe der Markierung in Ameisenschritten</param>
        public void SprüheMarkierung(int information, int größe)
        {
            MakeMarkerCoreInsects(information, größe);
        }

        /// <summary>
        /// Mit Hilfe dieses Befehls gibt die Ameise Denkblasen aus, die zur Fehlersuche 
        /// eingesetzt werden können.
        /// <see href="http://wiki.antme.net/de/API1:Denke">Weitere Infos</see>
        /// </summary>
        /// <param name="nachricht">Nachricht</param>
        public void Denke(string nachricht)
        {
            ThinkCore(nachricht);
        }

        #endregion

        #region Eigenschaften

        /// <summary>
        /// Gibt die maximale Energie der Ameise an. Die Einheit ist Lebenspunkte.
        /// <see href="http://wiki.antme.net/de/API1:MaximaleEnergie">Weitere Infos</see>
        /// </summary>
        public int MaximaleEnergie
        {
            get { return MaximumEnergyCoreInsect; }
        }

        /// <summary>
        /// Gibt die maximale Geschwindigkeit der Ameise an. Die Einheit ist Schritte pro Runde.
        /// <see href="http://wiki.antme.net/de/API1:MaximaleGeschwindigkeit">Weitere Infos</see>
        /// </summary>
        public int MaximaleGeschwindigkeit
        {
            get { return MaximumSpeedCoreInsect; }
        }

        /// <summary>
        /// Gibt die maximal tragbare Last der Ameise an. Die Einheit ist Nahrungspunkte. 
        /// Dieser Wert bestimmt, wie viel Zucker die Ameise auf einmal tragen kann und 
        /// wie schnell sie ohne die Hilfe anderer Ameisen einen Apfel tragen kann.
        /// <see href="http://wiki.antme.net/de/API1:MaximaleLast">Weitere Infos</see>
        /// </summary>
        public int MaximaleLast
        {
            get { return MaximumLoadCoreInsect; }
        }

        /// <summary>
        /// Gibt die Reichweite in Schritten an die die Ameise zurücklegen kann, bevor sie 
        /// vor Hunger stirbt. Nachdem die Ameise ein Drittel dieser Strecke zurückgelegt hat, 
        /// wird das Ereignis WirdMüde() aufgerufen und der Wert von IstMüde auf wahr gesetzt. 
        /// Siehe ZurückgelegteStrecke.
        /// <see href="http://wiki.antme.net/de/API1:Reichweite">Weitere Infos</see>
        /// </summary>
        public int Reichweite
        {
            get { return RangeCoreInsect; }
        }

        /// <summary>
        /// Gibt den Angriffswert der Ameise an. Der Angriffswert bestimmt wie viele 
        /// Lebenspunkte die Ameise einem Gegner in jeder Runde abzieht. Die Einheit 
        /// ist Lebenspunkte.
        /// <see href="http://wiki.antme.net/de/API1:Angriff">Weitere Infos</see>
        /// </summary>
        public int Angriff
        {
            get { return AttackStrengthCoreInsect; }
        }

        /// <summary>
        /// Gibt den Wahrnehmungsradius der Ameise in Schritten an. Dieser Radius bestimmt 
        /// wie weit die Ameise von Spielelementen wie z.B. Zucker entfernt sein muss damit 
        /// die Ameise sie sieht. Die Blickrichtung der Ameise spielt dabei keine Rolle.
        /// <see href="http://wiki.antme.net/de/API1:Sichtweite">Weitere Infos</see>
        /// </summary>
        public int Sichtweite
        {
            get { return ViewRangeCoreInsect; }
        }

        /// <summary>
        /// Gibt die Geschwindigkeit an mit der sich eine Ameise drehen kann. Die Einheit 
        /// ist Grad pro Runde.
        /// <see href="http://wiki.antme.net/de/API1:Drehgeschwindigkeit">Weitere Infos</see>
        /// </summary>
        public int Drehgeschwindigkeit
        {
            get { return RotationSpeedCoreInsect; }
        }

        /// <summary>
        /// Gibt die aktuelle Energie der Ameise an. Die Einheit ist Lebenspunkte. Hat die 
        /// Ameise 0 Lebenspunkte oder weniger, dann stirbt sie. Dieser Wert ist immer 
        /// kleiner oder gleich MaximaleEnergie.
        /// <see href="http://wiki.antme.net/de/API1:AktuelleEnergie">Weitere Infos</see>
        /// </summary>
        public int AktuelleEnergie
        {
            get { return currentEnergyCoreInsect; }
        }

        /// <summary>
        /// Gibt die aktuell mögliche Geschwindigkeit der Ameise an. Die Einheit ist Schritte 
        /// pro Runde. Der Wert wird von der aktuellen Last der Ameise beeinflusst. Ameisen 
        /// die unter voller Last bewegt werden, können nur die Hälfte ihrer 
        /// Maximalgeschwindigkeit erreichen. Diese Eigenschaft liefert immer einen Wert 
        /// größer 0 zurück, auch wenn die Ameise still steht. Dieser Wert ist immer kleiner 
        /// oder gleich MaximaleGeschwindigkeit.
        /// <see href="http://wiki.antme.net/de/API1:AktuelleGeschwindigkeit">Weitere Infos</see>
        /// </summary>
        public int AktuelleGeschwindigkeit
        {
            get { return CurrentSpeedCoreInsect; }
        }

        /// <summary>
        /// Gibt die aktuelle Last an, die die Ameise gerade trägt. Die Einheit ist Nahrungspunkte. 
        /// Dieser Wert ist immer kleiner oder gleich MaximaleLast.
        /// <see href="http://wiki.antme.net/de/API1:AktuelleLast">Weitere Infos</see>
        /// </summary>
        public int AktuelleLast
        {
            get { return CurrentLoadCoreInsect; }
        }

        /// <summary>
        /// Gibt die Anzahl der Ameisen desselben Volkes des im Wahrnehmungsradius der Ameise zurück. 
        /// Das Ergebnis dieser Berechnung ist von der Sichtweite der Ameise abhängig.
        /// <see href="http://wiki.antme.net/de/API1:AnzahlAmeisenInSichtweite">Weitere Infos</see>
        /// </summary>
        public int AnzahlAmeisenInSichtweite
        {
            get { return ColonyAntsInViewRange; }
        }

        /// <summary>
        /// Gibt die Anzahl der befreundeten Ameisen desselben Volkes und derselben Kaste im 
        /// Wahrnehmungsradius der Ameise zurück. Das Ergebnis dieser Berechnung ist von der 
        /// Sichtweite der Ameise abhängig.
        /// <see href="http://wiki.antme.net/de/API1:AnzahlAmeisenDerSelbenKasteInSichtweite">Weitere Infos</see>
        /// </summary>
        public int AnzahlAmeisenDerSelbenKasteInSichtweite
        {
            get { return CasteAntsInViewRange; }
        }

        /// <summary>
        /// Gibt die Anzahl der befreundeten Ameisen desselben Teams im Wahrnehmungsradius der Ameise zurück. 
        /// Das Ergebnis dieser Berechnung ist von der Sichtweite der Ameise abhängig.
        /// <see href="http://wiki.antme.net/de/API1:AnzahlAmeisenDesTeamsInSichtweite">Weitere Infos</see>
        /// </summary>
        public int AnzahlAmeisenDesTeamsInSichtweite
        {
            get { return TeamAntsInViewRange; }
        }

        /// <summary>
        /// Gibt die Anzahl der feindlichen Ameisen im Wahrnehmungsradius der Ameise zurück. 
        /// Das Ergebnis dieser Berechnung ist von der Sichtweite der Ameise abhängig.
        /// <see href="http://wiki.antme.net/de/API1:AnzahlFremderAmeisenInSichtweite">Weitere Infos</see>
        /// </summary>
        public int AnzahlFremderAmeisenInSichtweite
        {
            get { return EnemyAntsInViewRange; }
        }

        /// <summary>
        /// Gibt die Anzahl der Wanzen im Wahrnehmungsradius der Ameise zurück. Das Ergebnis 
        /// dieser Berechnung ist von der Sichtweite der Ameise abhängig.
        /// <see href="http://wiki.antme.net/de/API1:WanzenInSichtweite">Weitere Infos</see>
        /// </summary>
        public int WanzenInSichtweite
        {
            get { return BugsInViewRange; }
        }

        /// <summary>
        /// Gibt die Entfernung in Schritten zum nächstgelegenen befreundeten Ameisenbau an.
        /// <see href="http://wiki.antme.net/de/API1:EntfernungZuBau">Weitere Infos</see>
        /// </summary>
        public int EntfernungZuBau
        {
            get { return DistanceToAnthillCoreInsect; }
        }

        /// <summary>
        /// Gibt das aktuell getragene Obststück zurück. Wenn die Ameise gerade kein Obst 
        /// trägt, zeigt dieser Verweis ins Leere.
        /// <see href="http://wiki.antme.net/de/API1:GetragenesObst">Weitere Infos</see>
        /// </summary>
        public Obst GetragenesObst
        {
            get
            {
                if (CarryingFruitCoreInsect != null)
                {
                    return new Obst(CarryingFruitCoreInsect);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gibt des Namen der Kaste der Ameise zurück.
        /// <see href="http://wiki.antme.net/de/API1:Kaste">Weitere Infos</see>
        /// </summary>
        public string Kaste
        {
            get { return CasteCoreInsect; }
        }

        /// <summary>
        /// Gibt das aktuelle Ziel der Ameise zurück. Wenn die Ameise gerade 
        /// kein Ziel hat, zeigt dieser Verweis ins Leere.
        /// <see href="http://wiki.antme.net/de/API1:Ziel">Weitere Infos</see>
        /// </summary>
        public Spielobjekt Ziel
        {
            get
            {
                if (DestinationCoreInsect is CoreSugar)
                {
                    return new Zucker((CoreSugar)DestinationCoreInsect);
                }
                else if (DestinationCoreInsect is CoreFruit)
                {
                    return new Obst((CoreFruit)DestinationCoreInsect);
                }
                else if (DestinationCoreInsect is CoreAnt)
                {
                    return new Ameise((CoreAnt)DestinationCoreInsect);
                }
                else if (DestinationCoreInsect is CoreBug)
                {
                    return new Wanze((CoreBug)DestinationCoreInsect);
                }
                else if (DestinationCoreInsect is CoreMarker)
                {
                    return new Markierung((CoreMarker)DestinationCoreInsect);
                }
                else if (DestinationCoreInsect is CoreAnthill)
                {
                    return new Bau((CoreAnthill)DestinationCoreInsect);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gibt an ob die Ameise müde ist. Die Ameise wird müde, sobald sie ein 
        /// Drittel ihrer maximalen Reichweite zurückgelegt hat. Nach dem Übergang 
        /// des Wertes dieser Eigenschaft von falsch auf wahr wird das Ereignis 
        /// WirdMüde() aufgerufen.
        /// <see href="http://wiki.antme.net/de/API1:IstM%C3%BCde">Weitere Infos</see>
        /// </summary>
        public bool IstMüde
        {
            get { return IsTiredCoreAnt; }
        }

        /// <summary>
        /// Gibt an wie viele Schritte die Ameise noch geradeaus gehen wird, bevor 
        /// sie wieder ihr Ziel anvisiert. Dieser Wert wird in jeder Runde um 
        /// AktuelleGeschwindigkeit verringert.
        /// <see href="http://wiki.antme.net/de/API1:RestStrecke">Weitere Infos</see>
        /// </summary>
        public int RestStrecke
        {
            get { return DistanceToDestinationCoreInsect; }
        }

        /// <summary>
        /// Gibt an wie viele Grad die Ameise sich noch drehen wird, bevor sie wieder 
        /// geradeaus gehen wird. Dieser Wert wird in jeder Runde um DrehGeschwindigkeit 
        /// verringert.
        /// <see href="http://wiki.antme.net/de/API1:RestWinkel">Weitere Infos</see>
        /// </summary>
        public int RestWinkel
        {
            get { return ResidualAngle; }
        }

        /// <summary>
        /// Gibt die Blickrichtung der Ameise in Grad auf dem Spielfeld an. 0 ist dabei 
        /// rechts (Osten) und der Winkel öffnet sich im Uhrzeigersinn. 90 ist daher 
        /// unten (Süden), 180 rechts (Westen) und 270 oben.
        /// <see href="http://wiki.antme.net/de/API1:Richtung">Weitere Infos</see>
        /// </summary>
        public int Richtung
        {
            get { return GetDirectionCoreInsect(); }
        }

        /// <summary>
        /// Gibt an ob die Ameise an ihrem Ziel angekommen ist.
        /// <see href="http://wiki.antme.net/de/API1:Angekommen">Weitere Infos</see>
        /// </summary>
        public bool Angekommen
        {
            get { return ArrivedCoreInsect; }
        }

        /// <summary>
        /// Diese Eigenschaft gibt die Gesamtanzahl an Schritten zurück die die Ameise 
        /// seit ihrem letzten Besuch in einem Ameisenbau zurückgelegt hat. Siehe Reichweite
        /// <see href="http://wiki.antme.net/de/API1:Zur%C3%BCckgelegteStrecke">Weitere Infos</see>
        /// </summary>
        public int ZurückgelegteStrecke
        {
            get { return NumberStepsWalked; }
        }

        private Zufall zufall;

        /// <summary>
        /// Erzeugt eine zufüllige Zahl zwischen den angegebenen Grenzen. Wenn nur ein 
        /// Parameter angegeben wird, wird eine Zahl zwischen 0 und der angegebenen Grenze - 1 
        /// bestimmt, wenn zwei Parameter angegeben werden, wird eine Zahl zwischen der unteren 
        /// Grenze und der oberen Grenze - 1 bestimmt.
        /// <see href="http://wiki.antme.net/de/API1:Zufall.Zahl">Weitere Infos</see>
        /// </summary>
        public Zufall Zufall
        {
            get
            {
                if (zufall == null)
                    zufall = new Zufall(RandomCoreInsect);
                return zufall;
            }
        }

        #endregion

        #region Hilfemethoden

        /// <summary>
        /// Ermittelt ob das angegebene Obst noch mehr Ameisen zum Tragen benötigt.
        /// <see href="http://wiki.antme.net/de/API1:BrauchtNochTr%C3%A4ger">Weitere Infos</see>
        /// </summary>
        /// <param name="obst">zu prüfendes Obst</param>
        /// <returns>Braucht noch Träger</returns>
        public bool BrauchtNochTräger(Obst obst)
        {
            return ((CoreFruit)obst.Element).NeedSupport(Colony);
        }

        #endregion
    }
}