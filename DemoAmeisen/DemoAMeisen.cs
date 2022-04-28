using AntMe.Deutsch;

namespace AntMe.Spieler.WolfgangGallo
{
    [Spieler(
        Volkname = "Demo-A-Meisen",
        Vorname = "Wolfgang",
        Nachname = "Gallo"
        )]
    public class DemoAMeise : Basisameise
    {
        private bool laufeWeg = false;

        #region Fortbewegung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {
            laufeWeg = false;
            if (AktuelleEnergie < MaximaleEnergie / 2)
            {
                GeheZuBau();
            }
            else
            {
                DreheUmWinkel(Zufall.Zahl(-36, 36));
                GeheGeradeaus(Zufall.Zahl(20, 40));
            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
        /// Reichweite �berschritten hat.
        /// </summary>
        public override void WirdM�de()
        {
            if (Ziel == null)
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
            if (!laufeWeg)
            {
                int entfernung = Koordinate.BestimmeEntfernung(this, zucker);
                Spr�heMarkierung(Koordinate.BestimmeRichtung(this, zucker), entfernung);
                if (Ziel == null && AktuelleLast < MaximaleLast)
                {
                    GeheZuZiel(zucker);
                }
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obstst�ck sieht.
        /// </summary>
        /// <param name="obst">Das n�chstgelegene Obstst�ck.</param>
        public override void Sieht(Obst obst)
        {
            if (!laufeWeg && Ziel == null && AktuelleLast == 0 && BrauchtNochTr�ger(obst))
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
            Nimm(zucker);
            if (AktuelleLast == MaximaleLast)
            {
                GeheZuBau();
            }
        }

        /// <summary>
        /// Wird einmal aufgerufen, wenn die Ameise ein Obstst�ck als Ziel hat und
        /// bei diesem ankommt.
        /// </summary>
        /// <param name="obst">Das Obst�ck.</param>
        public override void ZielErreicht(Obst obst)
        {
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
            if (!laufeWeg && Ziel == null)
            {
                DreheInRichtung(markierung.Information);
                GeheGeradeaus(10);
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
            laufeWeg = true;
            LasseNahrungFallen();
            GeheGeradeaus(40);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem K�fer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende K�fer.</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            laufeWeg = true;
            LasseNahrungFallen();
            GeheGeradeaus(40);
        }

        #endregion

        #region Sonstiges

        /// <summary>
        /// Wird unabh�ngig von �u�eren Umst�nden in jeder Runde aufgerufen.
        /// </summary>
        public override void Tick()
        {
            if (Ziel is Obst && !BrauchtNochTr�ger((Obst)Ziel))
            {
                BleibStehen();
            }
            else if (Ziel is Bau && AktuelleLast > 0 && GetragenesObst == null)
            {
                Spr�heMarkierung(Richtung + 180);
            }
        }

        #endregion
    }
}