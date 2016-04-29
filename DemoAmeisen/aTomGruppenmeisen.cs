using System.Collections.Generic;
using AntMe.Deutsch;

namespace AntMe.Spieler.TomWendel
{

	/// <summary>
	/// Diese Demoameise zeigt die Aufteilung der Ameisen in 2 Typen um
	/// verschiedene Aufgaben wahr zu nehmen. Einerseits gibt es die Sammler die
	/// möglichst viel tragen und schnell laufen können. Andererseits gibt es
	/// die Wächter die auf den Ameisenstraßen mitpatroullieren um Käfer
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
		Name = "Kämpfer",
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
				// Sollte das Verhältnis Kämpfer/Sammler über 1 steigen (also mehr
				// Kämpfer als Sammler) werden Sammler produziert, ansonsten Kämpfer.
				return (float)anzahl["Kämpfer"] / anzahl["Sammler"] > 1.0f 
					? "Sammler" : "Kämpfer";
			}
		}

		#region Fortbewegung

		public override void Wartet()
		{

			// Befindet sich die Ameise außerhalb des Nahrungsradius soll sie nach
			// Hause gehen.
			if (EntfernungZuBau > 400)
			{
				GeheZuBau();
			}
			else
			{
				// ansonsten zufällig umherlaufen
				DreheUmWinkel(Zufall.Zahl(-10, 10));
				GeheGeradeaus(20);
			}

			// Wenn die Ameise am Limit ihrer Reichweite ist (abzüglich Entfernung zum
			// Bau und einem Puffer) soll sie nach Hause gehen.
			if (Reichweite - ZurückgelegteStrecke - 50 < EntfernungZuBau)
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
		/// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
		public override void Sieht(Zucker zucker)
		{
			// Wenn Zucker in der Nähe ist soll eine Markierung gesprüht werden. Der
			// Radius dieser Markierung richtet sich nach der Entfernung der Ameise
			// zum Zucker damit die Markierung nicht über den Zucker hinaus zeigt.
			SprüheMarkierung(
					Koordinate.BestimmeRichtung(this, zucker),
					Koordinate.BestimmeEntfernung(this, zucker));
			// Wenn die Ameise nichts trägt soll sie zum Zucker hin.
			if (AktuelleLast == 0)
			{
				GeheZuZiel(zucker);
			}
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
		/// Obststück sieht.
		/// </summary>
		/// <param name="obst">Das nächstgelegene Obststück.</param>
		public override void Sieht(Obst obst)
		{
			// Sollte die betroffene Ameise ein Sammler sein, lastlos UND sollte das
			// gefundene Obst noch Träger brauchen, geh hin.
			if (AktuelleLast == 0 && Kaste == "Sammler" &&
				BrauchtNochTräger(obst))
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
		/// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
		/// bei diesem ankommt.
		/// </summary>
		/// <param name="obst">Das Obstück.</param>
		public override void ZielErreicht(Obst obst)
		{
			// Da nur Sammler überhaupt zum Zucker gehen braucht hier keine
			// Unterscheidung mehr stattzufinden aber alle Sammler nehmen das Obst
			// mit, sofern dieses Obst noch Träger braucht.
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
			// Auf Markierungen wird nur reagiert, wenn das Ziel nicht der Bau oder
			// Zucker ist.
			if (!(Ziel is Zucker) && !(Ziel is Bau))
			{
				// Die Richtung aus der Markierung auslesen und über die Doppelte sichtweite loslaufen
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
			if (Kaste == "Sammler")
			{
				// Bei Käfersicht wird ermittelt ob die Sammlerameise evtl. kollidiert,
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
				// Kämpfer greifen sofort an.
				LasseNahrungFallen();
				GreifeAn(wanze);
			}
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise von einem Käfer angegriffen
		/// wird.
		/// </summary>
		/// <param name="wanze">Der angreifende Käfer.</param>
		public override void WirdAngegriffen(Wanze wanze)
		{
			// Wenn die Ameise direkt angegriffen wird lässt sie erst mal ihre Nahrung
			// fallen.
			LasseNahrungFallen();
			if (Kaste == "Sammler")
			{
				// Sammler flüchten.
				GeheWegVon(wanze, 100);
			}
			else
			{
				// Kämpfer hauen drauf.
				GreifeAn(wanze);
			}
		}

		#endregion
		#region Sonstiges

		/// <summary>
		/// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
		{

			// Markierungshandling.
			if (Ziel is Bau &&
					AktuelleLast > 0 &&
					GetragenesObst == null &&
					Kaste == "Sammler")
			{
				// Sammler, die mit Nahrung auf dem Rücken richtung Bau laufen sollen
				// fortwährend Markierungen sprühen um eine Ameisenstrasse zu erzeugen.
				if (Koordinate.BestimmeEntfernung(this, Ziel) < 100)
				{
					SprüheMarkierung(
							Koordinate.BestimmeRichtung(Ziel, this),
							100 - Koordinate.BestimmeEntfernung(Ziel, this));
				}
				else
				{
					SprüheMarkierung(
							Koordinate.BestimmeRichtung(Ziel, this),
							20);
				}
			}

			// Sollten Kämpfer einen Käfer über größere Strecken verfolgen muss der
			// Kampf iregendwann auch abgebrochen werden. Dies geschieht, wenn weniger
			// als 3 Ameisen in der Nähe sind. Das sollte der Fall sein, wenn sich der
			// Käfer nicht mehr in der Nähe einer Ameisenstrasse befindet.
			if (Ziel is Wanze && AnzahlAmeisenInSichtweite < 3)
			{
				BleibStehen();
			}
		}

		#endregion

	}

}