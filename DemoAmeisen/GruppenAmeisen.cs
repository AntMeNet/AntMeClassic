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

        // Speichert die Gruppenf�hrer aller Gruppen.
        private static GruppenAmeise[] gruppenf�hrer = new GruppenAmeise[10];

        // Speichert zu welcher Gruppe die Ameise geh�rt.
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
            // Wenn die Ameise der Gruppenf�hrer ist, dann lege die Bewegung fest.
            if (gruppenf�hrer[gruppe] == this)
                if (IstM�de || AktuelleEnergie < MaximaleEnergie / 2)
                    GeheZuBau();
                else
                    GeheGeradeaus();
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
        /// Reichweite �berschritten hat.
        /// </summary>
        public override void WirdM�de() { }

        #endregion
        #region Nahrung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
        /// Zuckerhaufen sieht.
        /// </summary>
        /// <param name="zucker">Der n�chstgelegene Zuckerhaufen.</param>
        public override void Sieht(Zucker zucker)
        {
            // Eine Markierung darf jede Ameise spr�hen.
            int richtung = Koordinate.BestimmeRichtung(this, zucker);
            int entfernung = Koordinate.BestimmeEntfernung(this, zucker);
            Spr�heMarkierung(richtung, entfernung);

            // Ein Ziel darf nur der Gruppenf�hrer vorgeben.
            if (gruppenf�hrer[gruppe] == this && (Ziel == null || Ziel is Insekt))
                GeheZuZiel(zucker);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obstst�ck sieht.
        /// </summary>
        /// <param name="obst">Das n�chstgelegene Obstst�ck.</param>
        public override void Sieht(Obst obst)
        {
            // Eine Markierung darf jede Ameise spr�hen.
            int richtung = Koordinate.BestimmeRichtung(this, obst);
            int entfernung = Koordinate.BestimmeEntfernung(this, obst);
            Spr�heMarkierung(richtung, entfernung);

            // Ein Ziel darf nur der Gruppenf�hrer vorgeben.
            if (gruppenf�hrer[gruppe] == this && (Ziel == null || Ziel is Insekt))
                GeheZuZiel(obst);
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        public override void ZielErreicht(Zucker zucker)
        {
            // Das Ziel Ameisenbau hat der Gruppenf�hrer dadurch vorgegeben, dass er
            // zuvor das Ziel auf den Zucker gesetzt hat.
            Nimm(zucker);
            GeheZuBau();
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obstst�ck als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obst�ck.</param>
        public override void ZielErreicht(Obst obst)
        {
            // Das Ziel Ameisenbau hat der Gruppenf�hrer dadurch vorgegeben, dass er
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
        /// <param name="markierung">Die n�chste neue Markierung.</param>
        public override void RiechtFreund(Markierung markierung)
        {
            if (gruppenf�hrer[gruppe] == this && Ziel == null)
            {
                DreheInRichtung(markierung.Information);
                GeheGeradeaus(20);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
        /// selben Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die n�chstgelegene befreundete Ameise.</param>
        public override void SiehtFreund(Ameise ameise) { }

        #endregion
        #region Kampf

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen K�fer
        /// sieht.
        /// </summary>
        /// <param name="wanze">Der n�chstgelegene K�fer.</param>
        public override void SiehtFeind(Wanze wanze)
        {
            if (gruppenf�hrer[gruppe] == this && 10 * MaximaleEnergie > wanze.AktuelleEnergie)
            {
                LasseNahrungFallen();
                GreifeAn(wanze);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die n�chstgelegen feindliche Ameise.</param>
        public override void SiehtFeind(Ameise ameise)
        {
            if (gruppenf�hrer[gruppe] == this && Ziel == null &&
                10 * MaximaleEnergie > ameise.AktuelleEnergie &&
                MaximaleGeschwindigkeit > ameise.MaximaleGeschwindigkeit)
            {
                LasseNahrungFallen();
                GreifeAn(ameise);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem K�fer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende K�fer.</param>
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
        // von Gruppenf�hrern und Gruppenmitgliedern unterschiedlich verwendet.
        private bool istBeiGruppe = false;

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
        /// </summary>
        /// <param name="todesart">Die Todesart der Ameise</param>
        public override void IstGestorben(Todesart todesart) { }

        /// <summary>
        /// Wird unabh�ngig von �u�eren Umst�nden in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
            // Ameisenstra�e wie gehabt.
            if (Ziel is Bau && AktuelleLast > 0 && GetragenesObst == null)
                Spr�heMarkierung(Richtung + 180);

            // Wenn die Stelle frei ist, dann bestimme die Ameise zum Gruppenf�hrer.
            if (gruppenf�hrer[gruppe] == null || gruppenf�hrer[gruppe].AktuelleEnergie <= 0)
                gruppenf�hrer[gruppe] = this;

            // Wenn die Ameise der Gruppenf�hrer ist, dann warte ggf. auf die Gruppe.
            if (gruppenf�hrer[gruppe] == this)
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

            // Wenn der Gruppenf�hrer einen Feind angreift, dann lasse die Nahrung
            // fallen und greife ebenfalls an.
            if (gruppenf�hrer[gruppe].Ziel is Insekt)
            {
                LasseNahrungFallen();
                GreifeAn((Insekt)gruppenf�hrer[gruppe].Ziel);
                return;
            }

            // Wenn der Gruppenf�hrer einen Apfel tr�gt oder zu einem Apfel oder zu
            // einem Zuckerhaufen geht, dann gehe auch dort hin.
            if (gruppenf�hrer[gruppe].GetragenesObst != null)
            {
                GeheZuZiel(gruppenf�hrer[gruppe].GetragenesObst);
                return;
            }
            if (gruppenf�hrer[gruppe].Ziel is Nahrung)
            {
                GeheZuZiel(gruppenf�hrer[gruppe].Ziel);
                return;
            }

            int entfernung =
                Koordinate.BestimmeEntfernung(this, gruppenf�hrer[gruppe]);

            // Pr�fe ob die Ameise den Anschlu� zur Gruppe verloren hat.
            if (istBeiGruppe && entfernung > 64)
                istBeiGruppe = false;

            // Pr�fe ob die Ameise den Anschlu� zur Gruppe wiedergefunden hat.
            if (!istBeiGruppe && entfernung < 16)
                istBeiGruppe = true;

            // Gehe zum Gruppenf�hrer und sag ihm, dass er warten soll.
            if (!istBeiGruppe)
            {
                int richtung = Koordinate.BestimmeRichtung(this, gruppenf�hrer[gruppe]);
                DreheInRichtung(richtung);
                GeheGeradeaus(entfernung);
                gruppenf�hrer[gruppe].istBeiGruppe = false;
                return;
            }

            // Drehe die Ameise in die Richtung in die der Gruppenf�hrer zeigt.
            // Wenn die Ameise weniger als 32 Schritte vom Gruppenf�hrer entfernt
            // ist und ihre Richtung mehr als 135 Grad von der des Gruppenf�hrers
            // abweicht, dann ist dieser sehr wahrscheinlich vom Spielfeldrand
            // abgeprallt. In diesem Fall stelle den alten RestWinkel wieder her,
            // damit die Ameise ebenfalls abprallen kann. Das geht schneller, als
            // sich in die Richtung des Gruppenf�hrers zu drehen.
            int restWinkel = RestWinkel;
            DreheInRichtung(gruppenf�hrer[gruppe].Richtung);
            if (entfernung < 32 && Math.Abs(RestWinkel) > 135)
                DreheUmWinkel(restWinkel);

            GeheGeradeaus(gruppenf�hrer[gruppe].RestStrecke);
        }

        #endregion

    }

}