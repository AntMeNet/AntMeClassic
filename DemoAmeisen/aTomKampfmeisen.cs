using AntMe.Deutsch;
using System.Collections.Generic;

namespace AntMe.Spieler.TomWendel
{

    /// <summary>
    /// Demoameise die sich ausschließlich mit der Bekämpfung von gegnerischen
    /// Käfern beschäftigt. Eine Ameise die einen Käfer sieht holt sofort mit
    /// einem großen Markierungs-Ping möglichst viel Hilfe aus der Umgebung. Nach
    /// einiger Zeit bilden sich dadurch kleine Jagdrudel die sehr effizient
    /// Käfer töten
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
            // Ameise soll möglichst gut gestreut aber ziellos umherirren um möglichst
            // schnell Käfer zu finden.
            GeheGeradeaus(40);
            DreheUmWinkel(Zufall.Zahl(-10, 10));
        }

        #endregion
        #region Kommunikation

        /// <summary>
        /// Wird wiederholt aufgerufen in der die Ameise mindestens eine
        /// Markierung des selben Volkes riecht.
        /// </summary>
        /// <param name="markierung">Die nächstgelegene neue Markierung.</param>
        public override void RiechtFreund(Markierung markierung)
        {
            // Die Ameise soll, sofern sie nicht schon ein Ziel wie "Käfer",
            // "Markierung" oder "Bau" hat auf direktem Weg zum Markierungsmittelpunkt
            // laufen von wo aus man hoffentlich weitere Markierungen oder direkt den
            // Käfer sieht.
            if (Ziel == null)
            {
                GeheZuZiel(markierung);
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
            // Wenn ein Käfer gesehen wird muss eine angemessen große Markierung
            // gesprüht werden. Ist diese Markierung zu klein kommt zu wenig Hilfe,
            // ist sie zu groß haben die weit entfernten Ameisen eine zu große Strecke
            // und kommen erst nach dem Kampf an.
            SprüheMarkierung(0, 150);
            GreifeAn(wanze);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
        /// anderen Volkes sieht.
        /// </summary>
        /// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
        public override void SiehtFeind(Ameise ameise)
        {
            // Feindliche Ameisen werden bedingungslos angegriffen!
            GreifeAn(ameise);
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise von einem Käfer angegriffen
        /// wird.
        /// </summary>
        /// <param name="wanze">Der angreifende Käfer.</param>
        public override void WirdAngegriffen(Wanze wanze)
        {
            // Wenn der Käfer angreift: Zurückschlagen.
            GreifeAn(wanze);
        }

        #endregion
        #region Sonstiges

        public override void Tick()
        {

            // Sollte die Ameise am Ende ihrer Reichweite sein (Abzüglich einem Puffer
            // und der Strecke die sie noch zum Bau zurücklegen muss) soll sie nach
            // Hause gehen um aufzuladen.
            if (Reichweite - ZurückgelegteStrecke - 100 <
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