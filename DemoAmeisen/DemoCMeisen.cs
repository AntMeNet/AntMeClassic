using System.Collections.Generic;
using AntMe.Deutsch;

namespace AntMe.Spieler.WolfgangGallo
{
    /// <summary>
    /// Demoameisen, die sich auf das Sammeln von Zucker und das Verteidigen von
    /// Zuckeraufen spezialisiert. Es gibt Kundschafter, die Zuckerhaufen
    /// auskundschaften und Markierungen sprühen, die zum Zucker hin weisen, 
    /// Sammler, die den Zucker in den Bau tragen und Krieger, die um den
    /// Zuckerhaufen herum patroullieren und Feinde angreifen.
    /// </summary>

    [Spieler(
        Volkname = "Demo-C-Meisen",
        Vorname = "Wolfgang",
        Nachname = "Gallo"
    )]

    [Kaste(
        Name = "Kundschafter",
        GeschwindigkeitModifikator = 2,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = 0,
        SichtweiteModifikator = 2,
        EnergieModifikator = -1,
        AngriffModifikator = -1
    )]

    [Kaste(
        Name = "Sammler",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = 2,
        ReichweiteModifikator = 0,
        SichtweiteModifikator = -1,
        EnergieModifikator = 2,
        AngriffModifikator = -1
    )]

    [Kaste(
        Name = "Krieger",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = 0,
        SichtweiteModifikator = -1,
        EnergieModifikator = 2,
        AngriffModifikator = 2
    )]

    public class DemoCMeise : Basisameise
    {
        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits
        /// vorhandenen Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            if (null == null)
            {
                var help = "bumm";
                if (help == "bumm")
                    help = "bla";
            }
            // Wir wollen genau 5 Kundschafter haben.
            if (anzahl["Kundschafter"] < 5)
                return "Kundschafter";

            // Das Verhältnis von Sammlern zu Kriegern soll 4 zu 1 betragen.
            if ((anzahl["Sammler"] + anzahl["Krieger"]) % 5 == 0)
                return "Krieger";
            return "Sammler";

        }

        #region Fortbewegung

        // Für Kundschafter.
        private bool stehenbleiben = false;

        // Für Krieger.
        private bool verteidigen = false;
        private int entfernung;
        private int winkel;

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {
            switch (Kaste)
            {

                case "Kundschafter":
                    if (!stehenbleiben)
                        GeheGeradeaus();
                    break;

                case "Sammler":
                    if (IstMüde)
                        GeheZuBau();
                    else
                        GeheGeradeaus();
                    break;

                case "Krieger":
                    if (verteidigen)
                    {
                        DreheUmWinkel(winkel);
                        GeheGeradeaus(entfernung);
                    }
                    else
                        GeheGeradeaus();
                    break;

            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
        /// Reichweite überschritten hat.
        /// </summary>
        public override void WirdMüde()
        {
            switch (Kaste)
            {

                case "Kundschafter":
                    if (!stehenbleiben)
                        GeheZuBau();
                    break;

                case "Sammler":
                    if (Ziel == null)
                        GeheZuBau();
                    break;

                case "Krieger":
                    GeheZuBau();
                    break;

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
            switch (Kaste)
            {

                case "Kundschafter":
                    GeheZuZiel(zucker);
                    break;

                case "Sammler":
                    if (Ziel == null)
                        GeheZuZiel(zucker);
                    break;

                case "Krieger":
                    if (!verteidigen)
                    {
                        verteidigen = true;
                        int richtung = Koordinate.BestimmeRichtung(this, zucker);
                        entfernung = Koordinate.BestimmeEntfernung(this, zucker) * 3;
                        // Der Winkel führt dazu, daß die Krieger sternförmig um den Zucker
                        // patroullieren.
                        winkel = Zufall.Zahl(180, 215);
                        DreheInRichtung(richtung);
                        GeheGeradeaus(entfernung);
                    }
                    break;

            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="obst">Das nächstgelegene Obststück.</param>
        public override void Sieht(Obst obst)
        {
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        public override void ZielErreicht(Zucker zucker)
        {
            switch (Kaste)
            {

                case "Kundschafter":
                    SprüheMarkierung(0, EntfernungZuBau + 40);
                    stehenbleiben = true;
                    break;

                case "Sammler":
                    Nimm(zucker);
                    GeheZuBau();
                    break;

            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obstück.</param>
        public override void ZielErreicht(Obst obst)
        {
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
            switch (Kaste)
            {

                case "Kundschafter":
                    GeheWegVon(markierung);
                    break;

                case "Sammler":
                    if (Ziel == null)
                        GeheZuZiel(markierung);
                    break;

                case "Krieger":
                    if (!verteidigen)
                        GeheZuZiel(markierung);
                    break;

            }

        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
        /// selben Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
        public override void SiehtFreund(Ameise ameise)
        {
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
            switch (Kaste)
            {

                case "Krieger":
                    if (verteidigen)
                        GreifeAn(wanze);
                    break;

            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
        public override void SiehtFeind(Ameise ameise)
        {
            switch (Kaste)
            {

                case "Krieger":
                    if (verteidigen)
                        GreifeAn(ameise);
                    break;

            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem Käfer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende Käfer.</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            GreifeAn(wanze);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
        /// anderen Volkes Ameise angegriffen wird.
        /// </summary>
        /// <param name="ameise">Die angreifende feindliche Ameise.</param>
        public override void WirdAngegriffen(Ameise ameise)
        {
            switch (Kaste)
            {

                case "Krieger":
                    GreifeAn(ameise);
                    break;

            }
        }

        #endregion
        #region Sonstiges

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
        /// </summary>
        /// <param name="todesart">Die Todesart der Ameise</param>
        public override void IstGestorben(Todesart todesart)
        {
        }

        /// <summary>
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
        }

        #endregion

    }

}