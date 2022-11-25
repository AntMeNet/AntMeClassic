using AntMe.SharedComponents.States;
using AntMe.SharedComponents.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// ant information and control box
    /// </summary>
    internal class AntInfoBox : InfoBox
    {

        private AntState ant;
        private int colonyId;
        private Brush brush;
        private PointF[] arrow = new PointF[3];
        private string casteColony;

        /// <summary>
        /// Constructor of new ant information box instance
        /// </summary>
        /// <param name="ant">ant</param>
        /// <param name="brush">brush</param>
        public AntInfoBox(AntState ant, int colonyId, string colonyName, string casteName, Brush brush)
        {
            // set size and margin
            Width = 150;
            Height = 73;
            Margin = new Padding(0);

            // bitmap image for background
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
        /// ant id for information box
        /// </summary>
        public override int Id
        {
            get { return ant.Id; }
        }

        /// <summary>
        /// ant state for the information box
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
        /// redraw ant information box
        /// </summary>
        /// <param name="e">event</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear drawing area
            graphics.Clear(Color.White);

            float y = 0f;

            // draw name and caste
            graphics.DrawString(name, boldFont, Brushes.Black, 0f, y);
            y += 15f;
            graphics.DrawString(casteColony, defaultFont, Brushes.Black, 0f, y);
            y += 15f;

            // calculate and draw direction array
            float xx, yy;
            xx = 5f * (float)Math.Cos(ant.Direction * Math.PI / 180d);
            yy = 5f * (float)Math.Sin(ant.Direction * Math.PI / 180d);
            arrow[0] = new PointF(15f + 3f * xx, y + 15f + 3f * yy);
            arrow[1] = new PointF(15f - 3f * xx + yy, y + 15f - 3f * yy - xx);
            arrow[2] = new PointF(15f - 3f * xx - yy, y + 15f - 3f * yy + xx);
            graphics.FillPolygon(brush, arrow);

            // draw energy and load
            graphics.DrawString(ant.Vitality.ToString(), bigFont, Brushes.Red, 40f, y);
            graphics.DrawString(ant.Load.ToString(), bigFont, Brushes.Green, 100f, y);

            base.OnPaint(e);
        }
    }
}