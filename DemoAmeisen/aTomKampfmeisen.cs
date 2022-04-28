using AntMe.Deutsch;
using System.Collections.Generic;

namespace AntMe.Spieler.TomWendel
{

    /// <summary>
    /// Demoameise die sich ausschlie�lich mit der Bek�mpfung von gegnerischen
    /// K�fern besch�ftigt. Eine Ameise die einen K�fer sieht holt sofort mit
    /// einem gro�en Markierungs-Ping m�glichst viel Hilfe aus der Umgebung. Nach
    /// einiger Zeit bilden sich dadurch kleine Jagdrudel die sehr effizient
    /// K�fer t�ten
    /// </summary>

    [Spieler(
        Volkname = "aTom Kampfmeisen",
        Vorname = "Tom",
        Nachname = "Wendel"
    )]

    [Kaste(
        Name = "Killer",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = 0,
        EnergieModifikator = 2,
        AngriffModifikator = 2
    )]

    public class aTomKampfmeisen : Basisameise
    {
        /// <summary>
        /// Bestimmt den Typ einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jedem Typ bereits
        /// vorhandenen Ameisen.</param>
        /// <returns>Der Name des Typs der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
        {
            // Erzeuge immer eine Ameise vom Typ "Killer".
            return "Killer";
        }

        #region Fortbewegung

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
        /// hingehen soll.
        /// </summary>
        public override void Wartet()
        {
            // Ameise soll m�glichst gut gestreut aber ziellos umherirren um m�glichst
            // schnell K�fer zu finden.
            GeheGeradeaus(40);
            DreheUmWinkel(Zufall.Zahl(-10, 10));
        }

        #endregion
        #region Kommunikation

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise mindestens eine
        /// Markierung des selben Volkes riecht.
        /// </summary>
        /// <param name="markierung">Die n�chstgelegene neue Markierung.</param>
        public override void RiechtFreund(Markierung markierung)
        {
            // Die Ameise soll, sofern sie nicht schon ein Ziel wie "K�fer",
            // "Markierung" oder "Bau" hat auf direktem Weg zum Markierungsmittelpunkt
            // laufen von wo aus man hoffentlich weitere Markierungen oder direkt den
            // K�fer sieht.
            if (Ziel == null)
            {
                GeheZuZiel(markierung);
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
            // Wenn ein K�fer gesehen wird muss eine angemessen gro�e Markierung
            // gespr�ht werden. Ist diese Markierung zu klein kommt zu wenig Hilfe,
            // ist sie zu gro� haben die weit entfernten Ameisen eine zu gro�e Strecke
            // und kommen erst nach dem Kampf an.
            Spr�heMarkierung(0, 150);
            GreifeAn(wanze);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die n�chstgelegen feindliche Ameise.</param>
        public override void SiehtFeind(Ameise ameise)
        {
            // Feindliche Ameisen werden bedingungslos angegriffen!
            GreifeAn(ameise);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem K�fer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende K�fer.</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            // Wenn der K�fer angreift: Zur�ckschlagen.
            GreifeAn(wanze);
        }

        #endregion
        #region Sonstiges

        public override void Tick()
        {

            // Sollte die Ameise am Ende ihrer Reichweite sein (Abz�glich einem Puffer
            // und der Strecke die sie noch zum Bau zur�cklegen muss) soll sie nach
            // Hause gehen um aufzuladen.
            if (Reichweite - Zur�ckgelegteStrecke - 100 <
                EntfernungZuBau)
            {
                GeheZuBau();
            }

            // Sollte eine Ameise durch den Kampf unter die 2/3-Marke ihrer Energie
            // fallen soll sie nach Hause gehen um aufzuladen.
            if (AktuelleEnergie < MaximaleEnergie * 2 / 3)
            {
                GeheZuBau();
            }

        }

        #endregion

    }

}