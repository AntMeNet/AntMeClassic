using System;
using System.Collections.Generic;

using AntMe.Deutsch;

namespace AntMe.Spieler.WolfgangGallo
{

	[Spieler(
		Volkname = "DynamischeAmeisen",
		Vorname = "Wolfgang",
		Nachname = "Gallo"
	)]

	[Kaste(
		Name = "Sammler1",
		GeschwindigkeitModifikator = 2,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = 2,
		ReichweiteModifikator = -1,
		SichtweiteModifikator = 0,
		EnergieModifikator = -1,
		AngriffModifikator = -1
	)]
	[Kaste(
		Name = "Sammler2",
		GeschwindigkeitModifikator = 1,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = 2,
		ReichweiteModifikator = -1,
		SichtweiteModifikator = 0,
		EnergieModifikator = 0,
		AngriffModifikator = -1
	)]
	[Kaste(
		Name = "Sammler3",
		GeschwindigkeitModifikator = 0,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = 2,
		ReichweiteModifikator = -1,
		SichtweiteModifikator = 0,
		EnergieModifikator = 1,
		AngriffModifikator = -1
	)]
    [Kaste(
		Name = "Sammler4",
		GeschwindigkeitModifikator = 0,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = 2,
		ReichweiteModifikator = -1,
		SichtweiteModifikator = -1,
		EnergieModifikator = 2,
		AngriffModifikator = -1
	)]
    [Kaste(
		Name = "Krieger1",
		GeschwindigkeitModifikator = -1,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = -1,
		ReichweiteModifikator = 0,
		SichtweiteModifikator = -1,
		EnergieModifikator = 2,
		AngriffModifikator = 2
	)]
    [Kaste(
		Name = "Krieger2",
		GeschwindigkeitModifikator = 0,
		DrehgeschwindigkeitModifikator = 0,
		LastModifikator = -1,
		ReichweiteModifikator = 0,
		SichtweiteModifikator = -1,
		EnergieModifikator = 1,
		AngriffModifikator = 1
	)]
    [Kaste(
		Name = "Krieger3",
		GeschwindigkeitModifikator = 1,
		DrehgeschwindigkeitModifikator = 0,
		LastModifikator = -1,
		ReichweiteModifikator = -1,
		SichtweiteModifikator = -1,
		EnergieModifikator = 1,
		AngriffModifikator = 1
	)]

	public class DynamischeAmeise : Basisameise
	{

		private static bool erzeugeSammler = false;
		private static int kriegerTyp = 1;
		private static int sammlerTyp = 1;

		/// <summary>
		/// Bestimmt den Typ einer neuen Ameise.
		/// </summary>
		/// <param name="anzahl">Die Anzahl der von jedem Typ bereits vorhandenen
		/// Ameisen.</param>
		/// <returns>Der Name des Typs der Ameise.</param>
		public override string BestimmeKaste(Dictionary<string, int> anzahl)
		{
			erzeugeSammler = !erzeugeSammler;
			if (erzeugeSammler)
				return "Sammler" + sammlerTyp;
			return "Krieger" + kriegerTyp;
		}

		#region Fortbewegung

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
		/// hingehen soll.
		/// </summary>
		public override void Wartet()
		{
			if (Ziel == null && gemerkterZucker != null)
				GeheZuZiel(gemerkterZucker);
			else
				GeheGeradeaus();
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
		/// Reichweite überschritten hat.
		/// </summary>
		public override void WirdMüde() {}

		#endregion
		#region Nahrung

		private Zucker gemerkterZucker;

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
		/// Zuckerhaufen sieht.
		/// </summary>
		/// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
		public override void Sieht(Zucker zucker)
		{
			if (gemerkterZucker == null)
				gemerkterZucker = zucker;

			SprüheMarkierung(0, 60);

			if (Kaste.Substring(0, 7) == "Sammler" && Ziel == null)
				GeheZuZiel(zucker);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
		/// Obststück sieht.
		/// </summary>
		/// <param name="obst">Das nächstgelegene Obststück.</param>
		public override void Sieht(Obst obst)
		{
			if (BrauchtNochTräger(obst))
			{
				SprüheMarkierung
					(Koordinate.BestimmeRichtung(this, obst),
					 Koordinate.BestimmeEntfernung(this, obst));

				if (Kaste.Substring(0, 7) == "Sammler" && Ziel == null)
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
			if (Ziel == null)
				GeheZuZiel(markierung);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
		/// selben Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
		public override void SiehtFreund(Ameise ameise) {}

		#endregion
		#region Kampf

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen Käfer
		/// sieht.
		/// </summary>
		/// <param name="wanze">Der nächstgelegene Käfer.</param>
		public override void SiehtFeind(Wanze wanze)
		{
			SprüheMarkierung(0, 60);

			if (Kaste.Substring(0, 7) == "Krieger" && Ziel == null)
				GreifeAn(wanze);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
		{
			if (Kaste.Substring(0, 7) == "Krieger" && Ziel == null)
				GreifeAn(ameise);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise von einem Käfer angegriffen
		/// wird.
		/// </summary>
		/// <param name="wanze">Der angreifende Käfer.</param>
		public override void WirdAngegriffen(Wanze wanze)
		{
			if (Kaste.Substring(0, 7) == "Krieger" && Ziel == null)
				GreifeAn(wanze);
		}

		/// <summary>
		/// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
		/// anderen Volkes Ameise angegriffen wird.
		/// </summary>
		/// <param name="ameise">Die angreifende feindliche Ameise.</param>
		public override void WirdAngegriffen(Ameise ameise)
		{
			if (Kaste.Substring(0, 7) == "Krieger" && Ziel == null)
				GreifeAn(ameise);
		}

		#endregion
		#region Sonstiges

		private static int erfolgloseKrieger = 0;
		private static int erfolgloseSammler = 0;
		private static int erfolgreicheKrieger = 0;
		private static int erfolgreicheSammler = 0;
		private int letzteLast = 0;
		private bool zielWarInsekt = false;

		private static void aktualisiereSammler()
		{
			sammlerTyp = 1 + (int)Math.Round
			                      	(3f * erfolgloseSammler /
			                      	 (erfolgloseSammler + erfolgreicheSammler));
		}

		private static void aktualisiereKrieger()
		{
			kriegerTyp = 1 + (int)Math.Round
			                      	(2f * erfolgloseKrieger /
			                      	 (erfolgloseKrieger + erfolgreicheKrieger));
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
		/// </summary>
		/// <param name="todesart">Die Todesart der Ameise</param>
		public override void IstGestorben(Todesart todesart)
		{
			if (todesart == Todesart.Verhungert)
				return;

			if (Kaste.Substring(0, 7) == "Sammler")
			{
				if (letzteLast > 0)
				{
					erfolgloseSammler++;
					aktualisiereSammler();
				}
			}
			else
			{
				erfolgloseKrieger++;
				aktualisiereKrieger();
			}
		}

		/// <summary>
		/// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
		{
			if (Kaste.Substring(0, 7) == "Sammler")
			{
				if (letzteLast > 0 && AktuelleLast == 0)
				{
					erfolgreicheSammler++;
					aktualisiereSammler();
				}
				letzteLast = AktuelleLast;
			}
			else
			{
				if (zielWarInsekt && Ziel == null)
				{
					erfolgreicheKrieger++;
					aktualisiereKrieger();
				}
				zielWarInsekt = Ziel is Insekt;
			}
		}

		#endregion

	}

}