using System.Collections.Generic;
using AntMe.Deutsch;

namespace AntMe.Spieler.TomWendel
{

    /// <summary>
    /// Demonstrationsameise die sich nur mit dem Sammeln von Zucker beschäftigt.
    /// Strategie dieser Ameise ist die möglichst schnelle Publikation neuer
    /// Zuckerhaufen um dort durch effiziente Ameisenstraßen alle restlichen
    /// Ameisen für den Zuckertransport zu binden. Obst wird komplett ignoriert
    /// und Käfern wird ausgewichen.
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

            // Sollte die Ameise außerhalb des Nahrungsmittelradiuses liegen...
            if (EntfernungZuBau > 400)
            {
                // ... soll sie wieder heim gehen.
                GeheZuBau();
            }
            else
            {
                // ... ansonsten soll sie sich ein bischen drehen (zufälliger Winkel
                // zwischen -10 und 10 Grad) und wieder ein paar Schritte laufen.
                DreheUmWinkel(Zufall.Zahl(-10, 10));
                GeheGeradeaus(20);
            }

            // Wenn die restliche verfügbare Strecke der Ameise (minus einem Puffer
            // von 50 Schritten) kleiner als die Entfernung zum Bau ist...
            if (Reichweite - ZurückgelegteStrecke - 50 < EntfernungZuBau)
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
        /// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
        public override void Sieht(Zucker zucker)
        {

            // Wenn Zucker in der Nähe ist soll eine Markierung gesprüht werden. Der
            // Radius dieser Markierung richtet sich nach der Entfernung der Ameise
            // zum Zucker damit die Markierung nicht über den Zucker hinaus zeigt.
            SprüheMarkierung(
                    Koordinate.BestimmeRichtung(this, zucker),
                    Koordinate.BestimmeEntfernung(this, zucker));

            // Gebe Debug-Nachricht aus
            Denke("Wow! Zucker!");

            // Wenn die Ameise nichts trägt soll sie zum Zucker hin.
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
        /// <param name="markierung">Die nächste neue Markierung.</param>
        public override void RiechtFreund(Markierung markierung)
        {
            // Auf Markierungen wird nur reagiert, wenn das Ziel nicht der Bau oder
            // Zucker ist.
            if (!(Ziel is Zucker) && !(Ziel is Bau))
            {

                // Die Richtung aus der Markierung auslesen und über die doppelte
                // Sichtweite loslaufen.
                DreheInRichtung(markierung.Information);
                GeheGeradeaus(Sichtweite * 2);

                // Sollte die Entfernung mehr als 50 schritte zum Mittelpunkt betragen
                // soll eine Folgemarkierung gesprüht werden um denn Effektradius zu
                // erhöhen.
                if (Koordinate.BestimmeEntfernung(this, markierung) > 50)
                {
                    SprüheMarkierung(
                            Koordinate.BestimmeRichtung(this, markierung),
                            Koordinate.BestimmeEntfernung(this, markierung));
                }
            }
        }

        #endregion
        #region Kampf

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen Käfer
        /// sieht.
        /// </summary>
        /// <param name="wanze">Der nächstgelegene Käfer.</param>
        public override void SiehtFeind(Wanze wanze)
        {
            // Bei Käfersicht wird ermittelt ob die Ameise evtl. kollidiert, wenn sie
            // geradeaus weitergeht.
            int relativeRichtung =
                Koordinate.BestimmeRichtung(this, wanze) - Richtung;
            if (relativeRichtung > -15 && relativeRichtung < 15)
            {
                // Wenn ja, soll sie erstmal die Nahrung fallen lassen um schneller zu
                // laufen und dann, je nachdem auf welcher Seite der Käfer ist, in einem
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
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
            // Sollte die Ameise gerade mit Nahrung unterwegs sein...
            if (Ziel != null && AktuelleLast > 0)
            {
                // ... Ist die Ameise näher als 100 Schritte am Zeil werden die
                // Markierungen stetig größer um die Ameisen abzugreifen die am anderen
                // Ende des Baus weglaufen.
                if (Koordinate.BestimmeEntfernung(this, Ziel) < 100)
                {
                    SprüheMarkierung(Koordinate.BestimmeRichtung(Ziel, this),
                        100 - Koordinate.BestimmeEntfernung(Ziel, this));
                }
                else
                {
                    // ... ansonsten eine möglichst dünne Ameisenstraße um präzise genug
                    // leiten zu können.
                    SprüheMarkierung(Koordinate.BestimmeRichtung(Ziel, this), 20);
                }
            }
        }

        #endregion

    }

}
