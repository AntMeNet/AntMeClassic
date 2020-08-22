using AntMe.Deutsch;
using System.Collections.Generic;

namespace AntMe.Spieler.WolfgangGallo
{

    [Spieler(
        Volkname = "Demo-B-Meisen",
        Vorname = "Wolfgang",
        Nachname = "Gallo"
    )]

    [Kaste(
        Name = "Sammler",
        GeschwindigkeitModifikator = 1,
        DrehgeschwindigkeitModifikator = 0,
        LastModifikator = 2,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = -1,
        EnergieModifikator = 0,
        AngriffModifikator = -1
    )]
    [Kaste(
        Name = "Krieger",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = 0,
        EnergieModifikator = 2,
        AngriffModifikator = 2
    )]
    [Kaste(
        Name = "Kundschafter",
        GeschwindigkeitModifikator = 1,
        DrehgeschwindigkeitModifikator = 1,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = 2,
        EnergieModifikator = -1,
        AngriffModifikator = -1
    )]

    public class DemoBMeise : Basisameise
    {
        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits
        /// vorhandenen Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            if (anzahl["Krieger"] < 5)
                return "Krieger";
            if (anzahl["Kundschafter"] < 5)
                return "Kundschafter";
            if (anzahl["Sammler"] < 5)
                return "Sammler";
            if (anzahl["Krieger"] < 15)
                return "Krieger";
            if (anzahl["Kundschafter"] < 15)
                return "Kundschafter";
            if (anzahl["Sammler"] < 25)
                return "Sammler";
            if (anzahl["Krieger"] < 30)
                return "Krieger";
            return "Sammler";
        }

        #region Fortbewegung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {

            if (IstMüde || AktuelleEnergie < MaximaleEnergie / 4)
                GeheZuBau();
            else
                switch (Kaste)
                {

                    case "Kundschafter":
                        DreheUmWinkel(Zufall.Zahl(-8, 8));
                        GeheGeradeaus(Zufall.Zahl(40, 80));
                        break;

                    case "Sammler":
                        DreheUmWinkel(Zufall.Zahl(-32, 32));
                        GeheGeradeaus(Zufall.Zahl(20, 40));
                        break;

                    case "Krieger":
                        DreheUmWinkel(Zufall.Zahl(-64, 64));
                        GeheGeradeaus(Zufall.Zahl(80, 160));
                        break;

                }

        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
        /// Reichweite überschritten hat.
        /// </summary>
        public override void WirdMüde()
        {
            if (Ziel == null)
                GeheZuBau();
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

                case "Sammler":
                    if (Ziel == null && AktuelleLast < MaximaleLast)
                        GeheZuZiel(zucker);
                    goto case "Kundschafter";

                case "Kundschafter":
                    int entfernung = Koordinate.BestimmeEntfernung(this, zucker);
                    if (entfernung > 50)
                        entfernung = 50;
                    SprüheMarkierung
                            ((ushort)Koordinate.BestimmeRichtung(this, zucker), entfernung);
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
            switch (Kaste)
            {

                case "Sammler":
                    if (Ziel == null && AktuelleLast == 0)
                        GeheZuZiel(obst);
                    goto case "Kundschafter";

                case "Kundschafter":
                    int entfernung = Koordinate.BestimmeEntfernung(this, obst);
                    if (entfernung > 50)
                        entfernung = 50;
                    SprüheMarkierung
                            ((ushort)Koordinate.BestimmeRichtung(this, obst), entfernung);
                    break;

            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise einen Zuckerhaufen als Ziel
        /// hat und bei diesem ankommt.
        /// </summary>
        /// <param name="zucker">Der Zuckerhaufen.</param>
        public override void ZielErreicht(Zucker zucker)
        {
            Nimm(zucker);
            if (AktuelleLast == MaximaleLast)
                GeheZuBau();
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obstück.</param>
        public override void ZielErreicht(Obst obst)
        {

            if (BrauchtNochTräger(obst))
            {
                Nimm(obst);
                GeheZuBau();
            }
            else
                BleibStehen();

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

                case "Sammler":
                    if (markierung.Information != -1 && Ziel == null)
                    {
                        DreheInRichtung(markierung.Information);
                        GeheGeradeaus(20);
                    }
                    break;

                case "Krieger":
                    if (markierung.Information == -1 && Ziel == null)
                    {
                        int entfernung = Koordinate.BestimmeEntfernung(this, markierung);
                        if (entfernung > 50)
                            entfernung = 50;
                        SprüheMarkierung(-1, entfernung);
                        GeheZuZiel(markierung);
                    }
                    break;

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
            switch (Kaste)
            {

                case "Sammler":
                    LasseNahrungFallen();
                    goto case "Kundschafter";

                case "Kundschafter":
                    GeheGeradeaus(Zufall.Zahl(20, 40));
                    break;

                case "Krieger":
                    SprüheMarkierung(-1, 50);
                    if (Ziel == null)
                        GreifeAn(wanze);
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
            if (AktuelleEnergie < MaximaleEnergie / 4)
                GeheZuBau();
        }

        #endregion
        #region Sonstiges

        /// <summary>
        /// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {

            if (Ziel is Obst && !BrauchtNochTräger((Obst)Ziel))
                BleibStehen();

            else if (Ziel is Bau && AktuelleLast > 0 && GetragenesObst == null)
                SprüheMarkierung(Richtung + 180);

            else if (Ziel is Wanze)
            {
                int entfernung = Koordinate.BestimmeEntfernung(this, Ziel);
                SprüheMarkierung(-1, entfernung);
            }

        }

        #endregion

    }

}