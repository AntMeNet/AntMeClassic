using AntMe.Deutsch;
using System.Collections.Generic;

namespace AntMe.Spieler.TomWendel
{

    /// <summary>
    /// Diese Demoameise zeigt die Aufteilung der Ameisen in 2 Typen um
    /// verschiedene Aufgaben wahr zu nehmen. Einerseits gibt es die Sammler die
    /// m�glichst viel tragen und schnell laufen k�nnen. Andererseits gibt es
    /// die W�chter die auf den Ameisenstra�en mitpatroullieren um K�fer
    /// abzuwehren.
    /// </summary>

    [Spieler(
        Volkname = "aTom Gruppenmeisen",
        Vorname = "Tom",
        Nachname = "Wendel"
    )]

    [Kaste(
        Name = "Sammler",
        GeschwindigkeitModifikator = 2,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = 2,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = 0,
        EnergieModifikator = -1,
        AngriffModifikator = -1
    )]
    [Kaste(
        Name = "K�mpfer",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = 0,
        EnergieModifikator = 2,
        AngriffModifikator = 2
    )]

    public class aTomGruppenmeise : Basisameise
    {
        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits
        /// vorhandenen Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            // Sollten noch keine Sammler existieren soll ein Sammler erzeugt werden.
            // Diese Vorabfrage soll nur eine Null-Division bei der folgenden
            // Entscheidung verhindern.
            if (anzahl["Sammler"] == 0)
            {
                return "Sammler";
            }
            else
            {
                // Sollte das Verh�ltnis K�mpfer/Sammler �ber 1 steigen (also mehr
                // K�mpfer als Sammler) werden Sammler produziert, ansonsten K�mpfer.
                return (float)anzahl["K�mpfer"] / anzahl["Sammler"] > 1.0f
                    ? "Sammler" : "K�mpfer";
            }
        }

        #region Fortbewegung

        public override void Wartet()
        {

            // Befindet sich die Ameise au�erhalb des Nahrungsradius soll sie nach
            // Hause gehen.
            if (EntfernungZuBau > 400)
            {
                GeheZuBau();
            }
            else
            {
                // ansonsten zuf�llig umherlaufen
                DreheUmWinkel(Zufall.Zahl(-10, 10));
                GeheGeradeaus(20);
            }

            // Wenn die Ameise am Limit ihrer Reichweite ist (abz�glich Entfernung zum
            // Bau und einem Puffer) soll sie nach Hause gehen.
            if (Reichweite - Zur�ckgelegteStrecke - 50 < EntfernungZuBau)
            {
                GeheZuBau();
            }

        }

        #endregion
        #region Nahrung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
        /// Zuckerhaufen sieht.
        /// </summary>
        /// <param name="zucker">Der n�chstgelegene Zuckerhaufen.</param>
        public override void Sieht(Zucker zucker)
        {
            // Wenn Zucker in der N�he ist soll eine Markierung gespr�ht werden. Der
            // Radius dieser Markierung richtet sich nach der Entfernung der Ameise
            // zum Zucker damit die Markierung nicht �ber den Zucker hinaus zeigt.
            Spr�heMarkierung(
                    Koordinate.BestimmeRichtung(this, zucker),
                    Koordinate.BestimmeEntfernung(this, zucker));
            // Wenn die Ameise nichts tr�gt soll sie zum Zucker hin.
            if (AktuelleLast == 0)
            {
                GeheZuZiel(zucker);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obstst�ck sieht.
        /// </summary>
        /// <param name="obst">Das n�chstgelegene Obstst�ck.</param>
        public override void Sieht(Obst obst)
        {
            // Sollte die betroffene Ameise ein Sammler sein, lastlos UND sollte das
            // gefundene Obst noch Tr�ger brauchen, geh hin.
            if (AktuelleLast == 0 && Kaste == "Sammler" &&
                BrauchtNochTr�ger(obst))
            {
                GeheZuZiel(obst);
            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        public override void ZielErreicht(Zucker zucker)
        {
            // Zucker nehmen und damit nach Hause laufen.
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
            // Da nur Sammler �berhaupt zum Zucker gehen braucht hier keine
            // Unterscheidung mehr stattzufinden aber alle Sammler nehmen das Obst
            // mit, sofern dieses Obst noch Tr�ger braucht.
            if (BrauchtNochTr�ger(obst))
            {
                Nimm(obst);
                GeheZuBau();
            }
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
            // Auf Markierungen wird nur reagiert, wenn das Ziel nicht der Bau oder
            // Zucker ist.
            if (!(Ziel is Zucker) && !(Ziel is Bau))
            {
                // Die Richtung aus der Markierung auslesen und �ber die Doppelte sichtweite loslaufen
                DreheInRichtung(markierung.Information);
                GeheGeradeaus(Sichtweite * 2);

                // Sollte die Entfernung mehr als 50 schritte zum Mittelpunkt betragen
                // soll eine Folgemarkierung gespr�ht werden um denn Effektradius zu
                // erh�hen.
                if (Koordinate.BestimmeEntfernung(this, markierung) > 50)
                {
                    Spr�heMarkierung(
                            Koordinate.BestimmeRichtung(this, markierung),
                            Koordinate.BestimmeEntfernung(this, markierung));
                }
            }
        }


        #endregion
        #region Kampf

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen K�fer
        /// sieht.
        /// </summary>
        /// <param name="wanze">Der n�chstgelegene K�fer.</param>
        public override void SiehtFeind(Wanze wanze)
        {
            if (Kaste == "Sammler")
            {
                // Bei K�fersicht wird ermittelt ob die Sammlerameise evtl. kollidiert,
                // wenn sie geradeaus weitergeht.
                int relativeRichtung =
                    Koordinate.BestimmeRichtung(this, wanze) - Richtung;
                if (relativeRichtung > -15 && relativeRichtung < 15)
                {
                    LasseNahrungFallen();
                    if (relativeRichtung < 0)
                    {
                        DreheUmWinkel(20 + relativeRichtung);
                    }
                    else
                    {
                        DreheUmWinkel(-20 - relativeRichtung);
                    }
                    GeheGeradeaus(100);
                }
            }
            else
            {
                // K�mpfer greifen sofort an.
                LasseNahrungFallen();
                GreifeAn(wanze);
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem K�fer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende K�fer.</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            // Wenn die Ameise direkt angegriffen wird l�sst sie erst mal ihre Nahrung
            // fallen.
            LasseNahrungFallen();
            if (Kaste == "Sammler")
            {
                // Sammler fl�chten.
                GeheWegVon(wanze, 100);
            }
            else
            {
                // K�mpfer hauen drauf.
                GreifeAn(wanze);
            }
        }

        #endregion
        #region Sonstiges

        /// <summary>
        /// Wird unabh�ngig von �u�eren Umst�nden in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {

            // Markierungshandling.
            if (Ziel is Bau &&
                    AktuelleLast > 0 &&
                    GetragenesObst == null &&
                    Kaste == "Sammler")
            {
                // Sammler, die mit Nahrung auf dem R�cken richtung Bau laufen sollen
                // fortw�hrend Markierungen spr�hen um eine Ameisenstrasse zu erzeugen.
                if (Koordinate.BestimmeEntfernung(this, Ziel) < 100)
                {
                    Spr�heMarkierung(
                            Koordinate.BestimmeRichtung(Ziel, this),
                            100 - Koordinate.BestimmeEntfernung(Ziel, this));
                }
                else
                {
                    Spr�heMarkierung(
                            Koordinate.BestimmeRichtung(Ziel, this),
                            20);
                }
            }

            // Sollten K�mpfer einen K�fer �ber gr��ere Strecken verfolgen muss der
            // Kampf iregendwann auch abgebrochen werden. Dies geschieht, wenn weniger
            // als 3 Ameisen in der N�he sind. Das sollte der Fall sein, wenn sich der
            // K�fer nicht mehr in der N�he einer Ameisenstrasse befindet.
            if (Ziel is Wanze && AnzahlAmeisenInSichtweite < 3)
            {
                BleibStehen();
            }
        }

        #endregion

    }

}