using AntMe.Deutsch;
using System;
using System.Collections.Generic;

namespace AntMe.Spieler.WolfgangGallo
{

    [Spieler(
        Volkname = "GruppenAmeisen",
        Vorname = "Wolfgang",
        Nachname = "Gallo"
    )]

    [Kaste(
        Name = "",
        GeschwindigkeitModifikator = 0,
        DrehgeschwindigkeitModifikator = 0,
        LastModifikator = 0,
        ReichweiteModifikator = 0,
        SichtweiteModifikator = 0,
        EnergieModifikator = 0,
        AngriffModifikator = 0
    )]

    public class GruppenAmeise : Basisameise
    {

        // Speichert wieviele Ameisen bereits insgesamt erzeugt wurden.
        private static int anzahlGesamt = 0;

        // Speichert die Gruppenführer aller Gruppen.
        private static GruppenAmeise[] gruppenführer = new GruppenAmeise[10];

        // Speichert zu welcher Gruppe die Ameise gehört.
        private int gruppe;

        /// <summary>
        /// Der Konstruktor.
        /// </summary>
        public GruppenAmeise()
        {
            // Die Ameisen eins bis zehn kommen in die erste Gruppe, die Ameisen elf
            // bis 20 kommen in die zweite Gruppe, u.s.w. Ameise 101 kommt wieder in
            // Gruppe eins, Ameise 102 in Gruppe zwei, u.s.w.
            if (anzahlGesamt < 100)
                gruppe = anzahlGesamt / 10;
            else
                gruppe = anzahlGesamt % 10;

            anzahlGesamt++;
        }

        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits vorhandenen
        /// Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            return "";
        }

        #region Fortbewegung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {
            // Wenn die Ameise der Gruppenführer ist, dann lege die Bewegung fest.
            if (gruppenführer[gruppe] == this)
                if (IstMüde || AktuelleEnergie < MaximaleEnergie / 2)
                    GeheZuBau();
                else
                    GeheGeradeaus();
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
        /// Reichweite überschritten hat.
        /// </summary>
        public override void WirdMüde() { }

        #endregion
        #region Nahrung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
        /// Zuckerhaufen sieht.
        /// </summary>
        /// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
        public override void Sieht(Zucker zucker)
        {
            // Eine Markierung darf jede Ameise sprühen.
            int richtung = Koordinate.BestimmeRichtung(this, zucker);
            int entfernung = Koordinate.BestimmeEntfernung(this, zucker);
            SprüheMarkierung(richtung, entfernung);

            // Ein Ziel darf nur der Gruppenführer vorgeben.
            if (gruppenführer[gruppe] == this && (Ziel == null || Ziel is Insekt))
                GeheZuZiel(zucker);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="obst">Das nächstgelegene Obststück.</param>
        public override void Sieht(Obst obst)
        {
            // Eine Markierung darf jede Ameise sprühen.
            int richtung = Koordinate.BestimmeRichtung(this, obst);
            int entfernung = Koordinate.BestimmeEntfernung(this, obst);
            SprüheMarkierung(richtung, entfernung);

            // Ein Ziel darf nur der Gruppenführer vorgeben.
            if (gruppenführer[gruppe] == this && (Ziel == null || Ziel is Insekt))
                GeheZuZiel(obst);
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        public override void ZielErreicht(Zucker zucker)
        {
            // Das Ziel Ameisenbau hat der Gruppenführer dadurch vorgegeben, dass er
            // zuvor das Ziel auf den Zucker gesetzt hat.
            Nimm(zucker);
            GeheZuBau();
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obstück.</param>
        public override void ZielErreicht(Obst obst)
        {
            // Das Ziel Ameisenbau hat der Gruppenführer dadurch vorgegeben, dass er
            // zuvor das Ziel auf das Obst gesetzt hat.
            Nimm(obst);
            GeheZuBau();
        }

        #endregion
        #region Kommunikation

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
        /// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
        /// gerochen.
        /// </summary>
        /// <param name="markierung">Die nächste neue Markierung.</param>
        public override void RiechtFreund(Markierung markierung)
        {
            if (gruppenführer[gruppe] == this && Ziel == null)
            {
                DreheInRichtung(markierung.Information);
                GeheGeradeaus(20);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
        /// selben Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
        public override void SiehtFreund(Ameise ameise) { }

        #endregion
        #region Kampf

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen Käfer
        /// sieht.
        /// </summary>
        /// <param name="wanze">Der nächstgelegene Käfer.</param>
        public override void SiehtFeind(Wanze wanze)
        {
            if (gruppenführer[gruppe] == this && 10 * MaximaleEnergie > wanze.AktuelleEnergie)
            {
                LasseNahrungFallen();
                GreifeAn(wanze);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
        public override void SiehtFeind(Ameise ameise)
        {
            if (gruppenführer[gruppe] == this && Ziel == null &&
                10 * MaximaleEnergie > ameise.AktuelleEnergie &&
                MaximaleGeschwindigkeit > ameise.MaximaleGeschwindigkeit)
            {
                LasseNahrungFallen();
                GreifeAn(ameise);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem Käfer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende Käfer.</param>
        public override void WirdAngegriffen(Wanze wanze) { }

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
        /// anderen Volkes Ameise angegriffen wird.
        /// </summary>
        /// <param name="ameise">Die angreifende feindliche Ameise.</param>
        public override void WirdAngegriffen(Ameise ameise) { }

        #endregion
        #region Sonstiges

        // Speichert ob die Ameise nahe bei der Gruppe ist oder zu weit weg. Wird
        // von Gruppenführern und Gruppenmitgliedern unterschiedlich verwendet.
        private bool istBeiGruppe = false;

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
        /// </summary>
        /// <param name="todesart">Die Todesart der Ameise</param>
        public override void IstGestorben(Todesart todesart) { }

        /// <summary>
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
            // Ameisenstraße wie gehabt.
            if (Ziel is Bau && AktuelleLast > 0 && GetragenesObst == null)
                SprüheMarkierung(Richtung + 180);

            // Wenn die Stelle frei ist, dann bestimme die Ameise zum Gruppenführer.
            if (gruppenführer[gruppe] == null || gruppenführer[gruppe].AktuelleEnergie <= 0)
                gruppenführer[gruppe] = this;

            // Wenn die Ameise der Gruppenführer ist, dann warte ggf. auf die Gruppe.
            if (gruppenführer[gruppe] == this)
            {
                if (!istBeiGruppe)
                {
                    BleibStehen();
                    istBeiGruppe = true;
                }
                return;
            }

            // Wenn das Ziel ein Insekt ist und tot, dann bleib stehen.
            if (Ziel is Insekt && ((Insekt)Ziel).AktuelleEnergie <= 0)
                BleibStehen();

            // Brich ab, wenn die Ameise ein Ziel hat.
            if (Ziel != null)
                return;

            // Wenn der Gruppenführer einen Feind angreift, dann lasse die Nahrung
            // fallen und greife ebenfalls an.
            if (gruppenführer[gruppe].Ziel is Insekt)
            {
                LasseNahrungFallen();
                GreifeAn((Insekt)gruppenführer[gruppe].Ziel);
                return;
            }

            // Wenn der Gruppenführer einen Apfel trägt oder zu einem Apfel oder zu
            // einem Zuckerhaufen geht, dann gehe auch dort hin.
            if (gruppenführer[gruppe].GetragenesObst != null)
            {
                GeheZuZiel(gruppenführer[gruppe].GetragenesObst);
                return;
            }
            if (gruppenführer[gruppe].Ziel is Nahrung)
            {
                GeheZuZiel(gruppenführer[gruppe].Ziel);
                return;
            }

            int entfernung =
                Koordinate.BestimmeEntfernung(this, gruppenführer[gruppe]);

            // Prüfe ob die Ameise den Anschluß zur Gruppe verloren hat.
            if (istBeiGruppe && entfernung > 64)
                istBeiGruppe = false;

            // Prüfe ob die Ameise den Anschluß zur Gruppe wiedergefunden hat.
            if (!istBeiGruppe && entfernung < 16)
                istBeiGruppe = true;

            // Gehe zum Gruppenführer und sag ihm, dass er warten soll.
            if (!istBeiGruppe)
            {
                int richtung = Koordinate.BestimmeRichtung(this, gruppenführer[gruppe]);
                DreheInRichtung(richtung);
                GeheGeradeaus(entfernung);
                gruppenführer[gruppe].istBeiGruppe = false;
                return;
            }

            // Drehe die Ameise in die Richtung in die der Gruppenführer zeigt.
            // Wenn die Ameise weniger als 32 Schritte vom Gruppenführer entfernt
            // ist und ihre Richtung mehr als 135 Grad von der des Gruppenführers
            // abweicht, dann ist dieser sehr wahrscheinlich vom Spielfeldrand
            // abgeprallt. In diesem Fall stelle den alten RestWinkel wieder her,
            // damit die Ameise ebenfalls abprallen kann. Das geht schneller, als
            // sich in die Richtung des Gruppenführers zu drehen.
            int restWinkel = RestWinkel;
            DreheInRichtung(gruppenführer[gruppe].Richtung);
            if (entfernung < 32 && Math.Abs(RestWinkel) > 135)
                DreheUmWinkel(restWinkel);

            GeheGeradeaus(gruppenführer[gruppe].RestStrecke);
        }

        #endregion

    }

}