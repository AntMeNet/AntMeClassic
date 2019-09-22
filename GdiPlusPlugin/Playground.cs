using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using AntMe.SharedComponents.Tools;
using AntMe.SharedComponents.States;

namespace AntMe.Plugin.GdiPlusPlugin
{

	/// <summary>
	/// Kontrollelement, das ein Spielfeld zeichnet.
	/// </summary>
	/// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
	internal class Playground : Control
	{
		// Verweis auf das übergeordnete Fenster. Der Verweis kann auch über
		// (Fenster)Control.Parent geholt werden, das geht aber erst wenn das
		// Kontrollelement dem Fenster hinzugefügt wurde. So ist es einfacher.
		private Window window;

		// Zeichenflächen für die Hintergrund-Grafik und das
		// Spielfeld-Kontrollelement.
		private Bitmap bitmap;
		private Graphics bitmapGraphics, controlGraphics;

		/// <summary>
		/// Gibt zurück oder legt fest ob die Punktetabelle angezeigt wird.
		/// </summary>
		public bool ShowScore;

		#region Statischer Teil

		public static Color[] playerColors = {
            Color.Black,
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Orange,
            Color.Cyan,
            Color.Fuchsia,
            Color.White
        };

		private static Color skyColor = Color.FromArgb(51, 153, 255);
		private static Color playgroundColor = Color.FromArgb(162, 162, 104);
        private static Color sugarColor = Color.White;
		private static Color fruitColor = Color.FromArgb(0, 224, 0);
		private static Color bugColor = Color.FromArgb(0, 0, 150);
		private static Color anthillColor = Color.FromArgb(97, 97, 63);
        private static Color markerColor = Color.Yellow;
		private static Color boxColor = Color.FromArgb(255, 255, 255, 128);

		private static SolidBrush anthillBrush = new SolidBrush(anthillColor);
		public static SolidBrush bugBrush = new SolidBrush(bugColor);
		private static SolidBrush fruitBrush = new SolidBrush(fruitColor);
		private static SolidBrush playgroundBrush = new SolidBrush(playgroundColor);
		private static SolidBrush sugarBrush = new SolidBrush(sugarColor);
		private static SolidBrush markerBrush = new SolidBrush(Color.FromArgb(21, Color.Yellow));
		private static SolidBrush[] markerBrushes;
		public static SolidBrush[] playerBrushes;

		private static Font bigBoldFont = new Font("Courier New", 30f, FontStyle.Bold);
		private static Font smallFont = new Font("MS Sans Serif", 10f);
		private static Font smallBoldFont = new Font("MS Sans Serif", 10f, FontStyle.Bold);

		private static SolidBrush selectionBrush = new SolidBrush(Color.FromArgb(128, SystemColors.Highlight));
		private static SolidBrush scoreBrush = new SolidBrush(Color.FromArgb(192, 255, 255, 255));

		private static Pen selectionPen = new Pen(SystemColors.Highlight);
		private static Pen bugPen = new Pen(bugColor, 6f);
		private static Pen sugarPen = new Pen(sugarColor);
		private static Pen[] playerPens;

		private Rectangle playgroundRectangle;

		private static readonly float[] cos1 = new float[360];
		private static readonly float[] cos2 = new float[360];
		private static readonly float[] cos4 = new float[360];
		private static readonly float[] sin1 = new float[360];
		private static readonly float[] sin2 = new float[360];
		private static readonly float[] sin4 = new float[360];

		static Playground()
		{
			playerBrushes = new SolidBrush[playerColors.Length];
			markerBrushes = new SolidBrush[playerColors.Length];
			playerPens = new Pen[playerColors.Length];
			for (int i = 0; i < playerColors.Length; i++)
			{
				playerBrushes[i] = new SolidBrush(playerColors[i]);
				markerBrushes[i] = new SolidBrush(Color.FromArgb(16, playerColors[i]));
				playerPens[i] = new Pen(playerBrushes[i], 2f);
			}

			for (int angle = 0; angle < 360; angle++)
			{
				cos1[angle] = (float)Math.Cos(angle * Math.PI / 180d);
				sin1[angle] = (float)Math.Sin(angle * Math.PI / 180d);
				cos2[angle] = 2f * cos1[angle];
				sin2[angle] = 2f * sin1[angle];
				cos4[angle] = 4f * cos1[angle];
				sin4[angle] = 4f * sin1[angle];
			}
		}

		#endregion

		/// <summary>
		/// Erzeugt eine Playground-Instanz.
		/// </summary>
		/// <param name="window">Das übergeordnete Fenster</param>
		public Playground(Window window)
		{
			this.window = window;

			// Sage Windows, daß wir das Puffern beim Zeichnen selbst übernehmen.
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			UpdateStyles();

			KeyDown += playground_KeyDown;
			KeyUp += playground_KeyUp;
			MouseWheel += playground_MouseWheel;
			MouseDown += playground_MouseDown;
			MouseMove += playground_MouseMove;
			MouseUp += playground_MouseUp;
			MouseDoubleClick += playground_MouseDoubleClick;
		}

		#region Ereignisbehandlung

		private List<int> selectedAnts = new List<int>();
		private List<int> selectedBugs = new List<int>();

		private int selectionX0, selectionY0, selectionX1, selectionY1;
		private int deltaX, deltaY;
		private int mouseX, mouseY;

		private bool shiftPressed;
		private bool controlPressed;

		private Matrix transform;
		private Matrix transformOld;

		private long timeStamp;

		private void startTransform()
		{
			transformOld = transform.Clone();
			mouseX = -1;
		}

		private void playground_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				// Skalieren des Spielfelds mit der Maus.
				case Keys.ShiftKey:
					if (shiftPressed)
					{
						startTransform();
						shiftPressed = false;
					}
					break;

				// Drehen des Spielfelds mit der Maus.
				case Keys.ControlKey:
					if (controlPressed)
					{
						startTransform();
						controlPressed = false;
					}
					break;
			}
		}

		private void playground_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				// Skalieren des Spielfelds mit der Maus.
				case Keys.ShiftKey:
					if (!shiftPressed)
					{
						startTransform();
						shiftPressed = true;
					}
					break;

				// Drehen des Spielfelds mit der Maus.
				case Keys.ControlKey:
					if (!controlPressed)
					{
						startTransform();
						controlPressed = true;
					}
					break;
			}
		}

		private void playground_MouseDown(object sender, MouseEventArgs e)
		{
			Focus();

			if (e.Button == MouseButtons.Left)
				mouseX = -1;

			else if (e.Button == MouseButtons.Right)
				startTransform();
		}

		private void playground_MouseMove(object sender, MouseEventArgs e)
		{
			if (mouseX == -1)
			{
				mouseX = e.X;
				mouseY = e.Y;
			}

			// Bestimme wie weit die Maus bewegt wurde.
			deltaX = e.X - mouseX;
			deltaY = e.Y - mouseY;

			if (e.Button == MouseButtons.Left)
			{
				if (deltaX >= 0)
				{
					selectionX0 = mouseX;
					selectionX1 = mouseX + deltaX;
				}
				else
				{
					selectionX1 = mouseX;
					selectionX0 = mouseX + deltaX;
				}

				if (deltaY >= 0)
				{
					selectionY0 = mouseY;
					selectionY1 = mouseY + deltaY;
				}
				else
				{
					selectionY1 = mouseY;
					selectionY0 = mouseY + deltaY;
				}

				// Zeichne nur dann, wenn der letzte Zeichenvorgang lange genug zurückliegt.
				if (DateTime.Now.Ticks - timeStamp > 400000)
					Draw();
			}
			else if (e.Button == MouseButtons.Right)
			{
				// Stelle die gesicherte Transformationsmatrix wieder her.
				transform = transformOld.Clone();

				// Die neue Transformationsmatrix ergibt sich aus der alten Matrix und der
				// Bewegung seit Beginn der Transformation. Das ist genauer und numerisch
				// stabiler (d.h. weniger Rechenfehler durch Zahlen nahe bei der Null)
				// als die Matrix immer nur ein klein wenig zu verändern.

				// Skaliere das Spielfeld.
				if (shiftPressed)
				{
					float factor = (float)Math.Pow(1.01d, deltaX);
					transform.Translate(-Width / 2f, -Height / 2f, MatrixOrder.Append);
					transform.Scale(factor, factor, MatrixOrder.Append);
					transform.Translate(Width / 2f, Height / 2f, MatrixOrder.Append);
				}

				// Drehe das Spielfeld.
				else if (controlPressed)
					transform.RotateAt(deltaX, new PointF(Width / 2f, Height / 2f), MatrixOrder.Append);

				// Verschiebe das Spielfeld.
				else
					transform.Translate(deltaX, deltaY, MatrixOrder.Append);

				// Zeichne nur dann, wenn der letzte Zeichenvorgang lange genug zurückliegt.
				if (DateTime.Now.Ticks - timeStamp > 400000)
					Draw();
			}
		}

		private void playground_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				PointF[] points;
				selectedAnts.Clear();
				selectedBugs.Clear();

				// Erzeuge ein Array für die Koordinaten aller Käfer.
				points = new PointF[window.State.BugStates.Count];

				// Kopiere die Koordinaten in das Array.
				for (int k = 0; k < points.Length; k++)
					points[k] = new PointF(window.State.BugStates[k].PositionX, window.State.BugStates[k].PositionY);

				// Transfomiere die Koordinaten.
				transform.TransformPoints(points);

				// Füge alle Käfer die sich innerhalb des Auswahlrechtecks befinden
				// der Liste der ausgewählten Käfer hinzu.
				for (int k = 0; k < points.Length; k++)
					if (points[k].X >= selectionX0 && points[k].X <= selectionX1 && points[k].Y >= selectionY0 &&
						points[k].Y <= selectionY1)
						selectedBugs.Add(window.State.BugStates[k].Id);

				// Das selbe für die Ameisen aller Völker.

				for (int v = 0; v < window.State.ColonyStates.Count; v++)
				{
					points = new PointF[window.State.ColonyStates[v].AntStates.Count];

					for (int a = 0; a < points.Length; a++)
						points[a] =
							new PointF
								(window.State.ColonyStates[v].AntStates[a].PositionX,
								 window.State.ColonyStates[v].AntStates[a].PositionY);

					transform.TransformPoints(points);

					for (int a = 0; a < points.Length; a++)
						if (points[a].X >= selectionX0 && points[a].X <= selectionX1 && points[a].Y >= selectionY0 &&
							points[a].Y <= selectionY1)
							selectedAnts.Add(window.State.ColonyStates[v].AntStates[a].Id);
				}

				// Gib die Auswahl an das übergeordnete Fenster weiter.
				window.ShowInformation(selectedBugs, selectedAnts);

				selectionX0 = -1;
			}

			Draw();
		}

		private void playground_MouseWheel(object sender, MouseEventArgs e)
		{
			float factor = (float)Math.Pow(1.01d, e.Delta / 10d);
			transform.Translate(-Width / 2f, -Height / 2f, MatrixOrder.Append);
			transform.Scale(factor, factor, MatrixOrder.Append);
			transform.Translate(Width / 2f, Height / 2f, MatrixOrder.Append);

			// Zeichne nur dann, wenn der letzte Zeichenvorgang lange genug zurückliegt.
			if (DateTime.Now.Ticks - timeStamp > 400000)
				Draw();
		}

		private void playground_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				ResetSelection();

			// Zeichne nur dann, wenn der letzte Zeichenvorgang lange genug zurückliegt.
			Draw();
		}

		public void ResetSelection()
		{
			selectedAnts.Clear();
			selectedBugs.Clear();
			selectionX0 = -1;
		}

		#endregion

		/// <summary>
		/// Zeichnet das Spielfeld in die Hintergrund-Grafik und diese auf das
		/// Kontrollelement.
		/// </summary>
		public void Draw()
		{

			// Brich ab, falls die Hintergrund-Zeichenfläche noch nicht existiert.
			if (bitmapGraphics == null)
				return;

			int a, b, k, m, o, v, z;

			// Erzeuge in der ersten Runde nötige Objekte.
			if (window.State.CurrentRound == 1)
			{
				playgroundRectangle = new Rectangle(0, 0, window.State.PlaygroundWidth, window.State.PlaygroundHeight);
				ResetView();
			}

			// Setze die Transformationsmatrix für das Spielfeld. Dadurch können wir
			// in Spielfeldkoordinaten zeichnen, egal wie das Fenster aussieht.
			bitmapGraphics.Transform = transform.Clone();

			// Zeichne den Himmel und das Spielfeld.
			bitmapGraphics.Clear(skyColor);
			bitmapGraphics.FillRectangle(playgroundBrush, playgroundRectangle);

			// Zeichne die Markierungen aller Völker.
			for (v = 0; v < window.State.ColonyStates.Count; v++)
			{
				SolidBrush brush = window.State.ColonyStates.Count == 1
					? markerBrush : markerBrushes[v];

				for (m = 0; m < window.State.ColonyStates[v].MarkerStates.Count; m++)
					bitmapGraphics.FillEllipse
						(brush,
						 window.State.ColonyStates[v].MarkerStates[m].PositionX -
						 window.State.ColonyStates[v].MarkerStates[m].Radius,
						 window.State.ColonyStates[v].MarkerStates[m].PositionY -
						 window.State.ColonyStates[v].MarkerStates[m].Radius,
						 window.State.ColonyStates[v].MarkerStates[m].Radius * 2f,
						 window.State.ColonyStates[v].MarkerStates[m].Radius * 2f);
			}

			// Zeichne die Bauten aller Völker.
			for (v = 0; v < window.State.ColonyStates.Count; v++)
				for (b = 0; b < window.State.ColonyStates[v].AnthillStates.Count; b++)
				{
					bitmapGraphics.FillEllipse
						(anthillBrush,
						 window.State.ColonyStates[v].AnthillStates[b].PositionX - window.State.ColonyStates[v].AnthillStates[b].Radius,
						 window.State.ColonyStates[v].AnthillStates[b].PositionY - window.State.ColonyStates[v].AnthillStates[b].Radius,
						 window.State.ColonyStates[v].AnthillStates[b].Radius * 2f,
						 window.State.ColonyStates[v].AnthillStates[b].Radius * 2f);

					if (window.State.ColonyStates.Count > 1)
						bitmapGraphics.DrawString
							((v + 1).ToString(), bigBoldFont, playerBrushes[v],
							 window.State.ColonyStates[v].AnthillStates[b].PositionX - 18f,
							 window.State.ColonyStates[v].AnthillStates[b].PositionY - 20f);
				}

			// Zeichne alle Zuckerhaufen.
			for (z = 0; z < window.State.SugarStates.Count; z++)
				bitmapGraphics.FillEllipse
					(sugarBrush, window.State.SugarStates[z].PositionX - window.State.SugarStates[z].Radius,
					 window.State.SugarStates[z].PositionY - window.State.SugarStates[z].Radius,
					 window.State.SugarStates[z].Radius * 2f, window.State.SugarStates[z].Radius * 2f);

			// Markiere die ausgewählten Wanzen.
			if (selectedBugs.Count > 0)
				for (k = 0; k < window.State.BugStates.Count; k++)
					if (selectedBugs.Contains(window.State.BugStates[k].Id))
						bitmapGraphics.FillEllipse
							(selectionBrush, window.State.BugStates[k].PositionX - 12f,
							 window.State.BugStates[k].PositionY - 12f, 24f, 24f);

			// Zeichne alle Wanzen.
			for (k = 0; k < window.State.BugStates.Count; k++)
				bitmapGraphics.DrawLine
					(bugPen, window.State.BugStates[k].PositionX - cos4[window.State.BugStates[k].Direction],
					 window.State.BugStates[k].PositionY - sin4[window.State.BugStates[k].Direction],
					 window.State.BugStates[k].PositionX + cos4[window.State.BugStates[k].Direction],
					 window.State.BugStates[k].PositionY + sin4[window.State.BugStates[k].Direction]);

			// Markiere die ausgewählten Ameisen.
			if (selectedAnts.Count > 0)
				for (v = 0; v < window.State.ColonyStates.Count; v++)
					for (a = 0; a < window.State.ColonyStates[v].AntStates.Count; a++)
						if (selectedAnts.Contains(window.State.ColonyStates[v].AntStates[a].Id))
							bitmapGraphics.FillEllipse
								(selectionBrush, window.State.ColonyStates[v].AntStates[a].PositionX - 8f,
								 window.State.ColonyStates[v].AntStates[a].PositionY - 8f, 16f, 16f);

			// Zeichne die Ameisen aller Völker.
			for (v = 0; v < window.State.ColonyStates.Count; v++)
				for (a = 0; a < window.State.ColonyStates[v].AntStates.Count; a++)
				{
					bitmapGraphics.DrawLine
						(playerPens[v],
						 window.State.ColonyStates[v].AntStates[a].PositionX -
						 cos2[window.State.ColonyStates[v].AntStates[a].Direction],
						 window.State.ColonyStates[v].AntStates[a].PositionY -
						 sin2[window.State.ColonyStates[v].AntStates[a].Direction],
						 window.State.ColonyStates[v].AntStates[a].PositionX +
						 cos2[window.State.ColonyStates[v].AntStates[a].Direction],
						 window.State.ColonyStates[v].AntStates[a].PositionY +
						 sin2[window.State.ColonyStates[v].AntStates[a].Direction]);

					if (window.State.ColonyStates[v].AntStates[a].LoadType == LoadType.Sugar)
						bitmapGraphics.DrawLine
							(sugarPen,
							 window.State.ColonyStates[v].AntStates[a].PositionX -
							 cos1[window.State.ColonyStates[v].AntStates[a].Direction],
							 window.State.ColonyStates[v].AntStates[a].PositionY -
							 sin1[window.State.ColonyStates[v].AntStates[a].Direction],
							 window.State.ColonyStates[v].AntStates[a].PositionX +
							 cos1[window.State.ColonyStates[v].AntStates[a].Direction],
							 window.State.ColonyStates[v].AntStates[a].PositionY +
							 sin1[window.State.ColonyStates[v].AntStates[a].Direction]);
				}

			// Zeichne alle Obststücke.
			for (o = 0; o < window.State.FruitStates.Count; o++)
				bitmapGraphics.FillEllipse
					(fruitBrush, window.State.FruitStates[o].PositionX - window.State.FruitStates[o].Radius,
					 window.State.FruitStates[o].PositionY - window.State.FruitStates[o].Radius,
					 window.State.FruitStates[o].Radius * 2, window.State.FruitStates[o].Radius * 2);

			// Setze die Einheitsmatrix als Transformationsmatrix. Damit zeichnen
			// wir jetzt wieder in Fensterkoordinaten.
			bitmapGraphics.Transform = new Matrix();

			// Auswahlrechteck.
			if (selectionX0 > -1)
			{
				bitmapGraphics.FillRectangle
					(selectionBrush, selectionX0, selectionY0, selectionX1 - selectionX0, selectionY1 - selectionY0);
				bitmapGraphics.DrawRectangle
					(selectionPen, selectionX0, selectionY0, selectionX1 - selectionX0, selectionY1 - selectionY0);
			}

			if (ShowScore)
            {
                DrawScore();
            }

            timeStamp = DateTime.Now.Ticks;

			// Zeichne die Hintergrund-Grafik auf das Fenster.
			OnPaint(new PaintEventArgs(controlGraphics, ClientRectangle));
		}

        // Zeichnet Punkte als Tabelle und richtet Spalten nach Textlaenge aus, 18f horizontaler Abstand
        private void DrawScore()
        {
            // Merke die Texte zum Ausgeben
            string[][] renderElements = new string[window.State.ColonyStates.Count + 1][];
            renderElements[0] = new string[]
            {
                Resource.ColonyName,
                Resource.CollectedFood,
                Resource.DeadAnts,
                Resource.KilledBugs,
                Resource.KilledEnemies,
                Resource.Points
            };

            // fuelle die Tabelle mit Zahlen der Voelker
            for (int currentRenderElementY = 1; currentRenderElementY < renderElements.Length; currentRenderElementY++)
            {
                renderElements[currentRenderElementY] = new string[]
                {
                    window.State.ColonyStates[currentRenderElementY - 1].ColonyName,
                    window.State.ColonyStates[currentRenderElementY - 1].CollectedFood.ToString(),
                    (window.State.ColonyStates[currentRenderElementY - 1].BeatenAnts
                        + window.State.ColonyStates[currentRenderElementY - 1].EatenAnts
                        + window.State.ColonyStates[currentRenderElementY - 1].StarvedAnts).ToString(),
                    window.State.ColonyStates[currentRenderElementY - 1].KilledBugs.ToString(),
                    window.State.ColonyStates[currentRenderElementY - 1].KilledEnemies.ToString(),
                    window.State.ColonyStates[currentRenderElementY - 1].Points.ToString()
                };
            }

            // Merke die Laenge der Texte zum Ausgeben, eine Extrazeile fuer die Max Laengen pro Spalte
            float[][] renderSizes = new float[renderElements.Length + 1][];
            for (int currentRenderSizeY = 0; currentRenderSizeY < renderSizes.Length -1; currentRenderSizeY++)
            {
                renderSizes[currentRenderSizeY] = new float[renderElements[0].Length];
                for(int currentRenderSizeX = 0; currentRenderSizeX < renderElements[0].Length; currentRenderSizeX++)
                {
                    renderSizes[currentRenderSizeY][currentRenderSizeX] = bitmapGraphics.MeasureString(
                        renderElements[currentRenderSizeY][currentRenderSizeX],
                        smallBoldFont).Width;
                }
            }

            // Gesamtbreite der Tabelle
            float totalWidth = 0;

            // Berechne Max-Laenge pro Spalte
            renderSizes[renderElements.Length] = new float[renderElements[0].Length];
            for (int currentRenderSizeX = 0; currentRenderSizeX < renderElements[0].Length; currentRenderSizeX++)
            {
                float maxLength = 0;
                for (int currentRenderSizeY = 0; currentRenderSizeY < renderSizes.Length - 1; currentRenderSizeY++)
                {

                    if (renderSizes[currentRenderSizeY][currentRenderSizeX] > maxLength)
                    {
                        maxLength = renderSizes[currentRenderSizeY][currentRenderSizeX];
                    }
                }

                renderSizes[renderElements.Length][currentRenderSizeX] = maxLength;
                totalWidth += maxLength + 18f;
            }       
            
            // Zeichne den Hintergrund des Punktekastens.
            bitmapGraphics.FillRectangle
                (scoreBrush, 10f, 10f, totalWidth, renderElements.Length * 16f + 18f);

            float y = 20f;

            // Zeichne alle Text-Elemente an den jeweiligen Positionen
            for (int currentRenderElementY = 0; currentRenderElementY < renderElements.Length; currentRenderElementY++)
            {
                if (currentRenderElementY != 0)
                {
                    bitmapGraphics.FillRectangle(playerBrushes[currentRenderElementY - 1], 20f, y + 2f, 11f, 11f);
                }

                float currentXStart = 37f;
                for (int currentRenderElementX = 0; currentRenderElementX < renderElements[0].Length; currentRenderElementX++)
                {
                    bitmapGraphics.DrawString(renderElements[currentRenderElementY][currentRenderElementX], smallFont, Brushes.Black, currentXStart, y);
                    currentXStart += renderSizes[renderSizes.Length - 1][currentRenderElementX];
                }

                y += 16f;
            }
        }

        /// <summary>
        /// Zeichnet die Hintergrund-Grafik auf das Kontrollelement.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
		{
			// Zeichne die Hintergrund-Grafik auf das Fenster.
			if (bitmap != null)
				e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
		}

		// Die Hintergrund-Grafik in die das Spielfeld gezeichnet wird. Durch das
		// Verwenden eines Puffers wird das Flimmern beim Neuzeichnen verhindert.

		/// <summary>
		/// Erzwingt das Resize Ereignis.
		/// </summary>
		public void DoResize()
		{
			OnResize(null);
		}

		/// <summary>
		/// Passt die Größe des Spielfeldes innerhalb des Kontrollelementes an.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			// Brich ab, falls noch kein Zustand übergeben wurde.
			if (window.State == null)
				return;

			// Erzeuge die Zeichenfläche des Kontrollelements.
			controlGraphics = CreateGraphics();

			// Erzeuge die Hintergrund-Grafik und die zugehörige Zeichenfläche.
			bitmap = new Bitmap(Width, Height);
			bitmapGraphics = Graphics.FromImage(bitmap);

			// Schalte das Anti-Aliasing ein bzw. aus.
			bitmapGraphics.SmoothingMode =
				window.UseAntiAliasing ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed;

			// Zeichne das Spielfeld neu.
			Draw();
		}

		/// <summary>
		/// Setzt die Ansicht des Spielfeldes zurück.
		/// </summary>
		public void ResetView()
		{
			// Berechne den Skalierungsfaktor und den linken und oberen Versatz.
			float xScale = Width / (float)window.State.PlaygroundWidth;
			float yScale = Height / (float)window.State.PlaygroundHeight;
			float scale = Math.Min(xScale, yScale);
			float width = window.State.PlaygroundWidth * scale;
			float height = window.State.PlaygroundHeight * scale;
			float xOffset = (Width - width) / 2f;
			float yOffset = (Height - height) / 2f;

			// Berechne die Transformationsmatrix für das Spielfeld.
			transform = new Matrix();
			transform.Translate(xOffset, yOffset);
			transform.Scale(scale, scale);
		}
	}
}