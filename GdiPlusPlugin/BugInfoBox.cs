using AntMe.SharedComponents.States;
using AntMe.SharedComponents.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Information-Kontrollelement für Wanze.
    /// </summary>
    internal class BugInfoBox : InfoBox
    {

        private BugState bug;
        private PointF[] arrow = new PointF[3];

        /// <summary>
        /// Erzeugt eine BugInfoBox-Instanz.
        /// </summary>
        /// <param name="bugState">Die Wanze.</param>
        public BugInfoBox(BugState bugState)
        {
            // Setze Größe und Randabstand.
            Width = 150;
            Height = 63;
            Margin = new Padding(0);

            // Erzeuge die Hintergrund-Grafik und hole ihre Zeichenfläche.
            bitmap = new Bitmap(Width, Height);
            graphics = Graphics.FromImage(bitmap);

            bug = bugState;
            name = string.Format(Resource.BugName, NameHelper.GetMaleName(bugState.Id));
        }

        /// <summary>
        /// Die Id der Wanze zu der Informationen dargestellt werden sollen.
        /// </summary>
        public override int Id
        {
            get { return bug.Id; }
        }

        /// <summary>
        /// Die Wanze deren Werte dargestellt werden sollen.
        /// </summary>
        public BugState Bug
        {
            set
            {
                bug = value;
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

            // Zeichne den Namen.
            graphics.DrawString(name, boldFont, Brushes.Black, 0f, 0f);

            // Berechne und zeichne den Richtungspfeil.
            float x, y;
            x = 5f * (float)Math.Cos(bug.Direction * Math.PI / 180d);
            y = 5f * (float)Math.Sin(bug.Direction * Math.PI / 180d);
            arrow[0] = new PointF(15f + 3f * x, 30f + 3f * y);
            arrow[1] = new PointF(15f - 3f * x + y, 30f - 3f * y - x);
            arrow[2] = new PointF(15f - 3f * x - y, 30f - 3f * y + x);
            graphics.FillPolygon(Playground.bugBrush, arrow);

            // Zeichne die Energie.
            graphics.DrawString(bug.Vitality.ToString(), bigFont, Brushes.Red, 40f, 15f);

            base.OnPaint(e);
        }
    }
}