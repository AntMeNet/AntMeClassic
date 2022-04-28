using AntMe.Deutsch;
using System.Collections.Generic;

namespace AntMe.Spieler.TomWendel
{

    /// <summary>
    /// Demonstrationsameise die sich nur mit dem Sammeln von Zucker besch�ftigt.
    /// Strategie dieser Ameise ist die m�glichst schnelle Publikation neuer
    /// Zuckerhaufen um dort durch effiziente Ameisenstra�en alle restlichen
    /// Ameisen f�r den Zuckertransport zu binden. Obst wird komplett ignoriert
    /// und K�fern wird ausgewichen.
    /// </summary>

    [Spieler(
        Volkname = "aTom Zuckermeisen",
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

    public class aTomZuckermeise : Basisameise
    {
        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits
        /// vorhandenen Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            // Erzeugt immer den Standardtyp "Sammler".
            return "Sammler";
        }

        #region Fortbewegung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {

            // Sollte die Ameise au�erhalb des Nahrungsmittelradiuses liegen...
            if (EntfernungZuBau > 400)
            {
                // ... soll sie wieder heim gehen.
                GeheZuBau();
            }
            else
            {
                // ... ansonsten soll sie sich ein bischen drehen (zuf�lliger Winkel
                // zwischen -10 und 10 Grad) und wieder ein paar Schritte laufen.
                DreheUmWinkel(Zufall.Zahl(-10, 10));
                GeheGeradeaus(20);
            }

            // Wenn die restliche verf�gbare Strecke der Ameise (minus einem Puffer
            // von 50 Schritten) kleiner als die Entfernung zum Bau ist...
            if (Reichweite - Zur�ckgelegteStrecke - 50 < EntfernungZuBau)
            {
                // ... soll sie nach Hause gehen um nicht zu sterben.
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

            // Gebe Debug-Nachricht aus
            Denke("Wow! Zucker!");

            // Wenn die Ameise nichts tr�gt soll sie zum Zucker hin.
            if (AktuelleLast == 0)
            {
                GeheZuZiel(zucker);
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

                // Die Richtung aus der Markierung auslesen und �ber die doppelte
                // Sichtweite loslaufen.
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
            // Bei K�fersicht wird ermittelt ob die Ameise evtl. kollidiert, wenn sie
            // geradeaus weitergeht.
            int relativeRichtung =
                Koordinate.BestimmeRichtung(this, wanze) - Richtung;
            if (relativeRichtung > -15 && relativeRichtung < 15)
            {
                // Wenn ja, soll sie erstmal die Nahrung fallen lassen um schneller zu
                // laufen und dann, je nachdem auf welcher Seite der K�fer ist, in einem
                // 20 Grad-Winkel in die andere Richtung weggehen.
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

        #endregion
        #region Sonstiges

        /// <summary>
        /// Wird unabh�ngig von �u�eren Umst�nden in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
            // Sollte die Ameise gerade mit Nahrung unterwegs sein...
            if (Ziel != null && AktuelleLast > 0)
            {
                // ... Ist die Ameise n�her als 100 Schritte am Zeil werden die
                // Markierungen stetig gr��er um die Ameisen abzugreifen die am anderen
                // Ende des Baus weglaufen.
                if (Koordinate.BestimmeEntfernung(this, Ziel) < 100)
                {
                    Spr�heMarkierung(Koordinate.BestimmeRichtung(Ziel, this),
                        100 - Koordinate.BestimmeEntfernung(Ziel, this));
                }
                else
                {
                    // ... ansonsten eine m�glichst d�nne Ameisenstra�e um pr�zise genug
                    // leiten zu k�nnen.
                    Spr�heMarkierung(Koordinate.BestimmeRichtung(Ziel, this), 20);
                }
            }
        }

        #endregion

    }

}
