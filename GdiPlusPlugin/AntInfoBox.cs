using AntMe.SharedComponents.States;
using AntMe.SharedComponents.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Information-Kontrollelement für Ameisen.
    /// </summary>
    internal class AntInfoBox : InfoBox
    {

        private AntState ant;
        private int colonyId;
        private Brush brush;
        private PointF[] arrow = new PointF[3];
        private string casteColony;

        /// <summary>
        /// Erzeugt eine AntInfoBox-Instanz.
        /// </summary>
        /// <param name="ant">Die Ameise.</param>
        /// <param name="brush">Der Pinsel mit dem gezeichnet werden soll.</param>
        public AntInfoBox(AntState ant, int colonyId, string colonyName, string casteName, Brush brush)
        {
            // Setze Größe und Randabstand.
            Width = 150;
            Height = 73;
            Margin = new Padding(0);

            // Erzeuge die Hintergrund-Grafik und hole ihre Zeichenfläche.
            bitmap = new Bitmap(Width, Height);
            graphics = Graphics.FromImage(bitmap);

            this.ant = ant;
            this.colonyId = colonyId;
            this.brush = brush;

            name = string.Format(Resource.AntName, NameHelper.GetFemaleName(ant.Id));
            if (!string.IsNullOrEmpty(casteName))
                casteColony = casteName + ", " + colonyName;
            else
                casteColony = colonyName;
        }

        public int ColonyId
        {
            get { return colonyId; }
        }

        /// <summary>
        /// Die Id der Ameise zu der Informationen dargestellt werden sollen.
        /// </summary>
        public override int Id
        {
            get { return ant.Id; }
        }

        /// <summary>
        /// Die Ameise zu der Informationen dargestellt werden sollen.
        /// </summary>
        public AntState Ant
        {
            set
            {
                ant = value;
                OnPaint(new PaintEventArgs(CreateGraphics(), ClientRectangle));
            }
        }

        /// <summary>
        /// Zeichnet die Informationen neu.
        /// </summary>
        /// <param name="e">Ereignisargumente</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Lösche die Zeichenfläche.
            graphics.Clear(Color.White);

            float y = 0f;

            // Zeichne den Namen, die Kaste und das Volk.
            graphics.DrawString(name, boldFont, Brushes.Black, 0f, y);
            y += 15f;
            graphics.DrawString(casteColony, defaultFont, Brushes.Black, 0f, y);
            y += 15f;

            // Berechne und zeichne den Richtungspfeil.
            float xx, yy;
            xx = 5f * (float)Math.Cos(ant.Direction * Math.PI / 180d);
            yy = 5f * (float)Math.Sin(ant.Direction * Math.PI / 180d);
            arrow[0] = new PointF(15f + 3f * xx, y + 15f + 3f * yy);
            arrow[1] = new PointF(15f - 3f * xx + yy, y + 15f - 3f * yy - xx);
            arrow[2] = new PointF(15f - 3f * xx - yy, y + 15f - 3f * yy + xx);
            graphics.FillPolygon(brush, arrow);

            // Zeichne Energie und Last.
            graphics.DrawString(ant.Vitality.ToString(), bigFont, Brushes.Red, 40f, y);
            graphics.DrawString(ant.Load.ToString(), bigFont, Brushes.Green, 100f, y);

            base.OnPaint(e);
        }
    }
}