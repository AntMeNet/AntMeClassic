using AntMe.SharedComponents.States;
using AntMe.SharedComponents.Tools;
using System;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Information control element for bug.
    /// </summary>
    internal class BugInfoBox : InfoBox
    {

        private BugState bug;
        private PointF[] arrow = new PointF[3];

        /// <summary>
        /// Creates a BugInfoBox instance.
        /// </summary>
        /// <param name="bugState">bug state</param>
        public BugInfoBox(BugState bugState)
        {
            // Set size and margin.
            Width = 150;
            Height = 63;
            Margin = new Padding(0);

            // Create the background graphic and get its drawing area.
            bitmap = new Bitmap(Width, Height);
            graphics = Graphics.FromImage(bitmap);

            bug = bugState;
            name = string.Format(Resource.BugName, NameHelper.GetMaleName(bugState.Id));
        }

        /// <summary>
        /// The ID of the bug for which information is to be displayed.
        /// </summary>
        public override int Id
        {
            get { return bug.Id; }
        }

        /// <summary>
        /// The bug whose values are to be displayed.
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
        /// Draw new information
        /// </summary>
        /// <param name="e">paint event argument</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // clear
            graphics.Clear(Color.White);

            // draw name
            graphics.DrawString(name, boldFont, Brushes.Black, 0f, 0f);

            // calculate direction and draw arrow
            float x, y;
            x = 5f * (float)Math.Cos(bug.Direction * Math.PI / 180d);
            y = 5f * (float)Math.Sin(bug.Direction * Math.PI / 180d);
            arrow[0] = new PointF(15f + 3f * x, 30f + 3f * y);
            arrow[1] = new PointF(15f - 3f * x + y, 30f - 3f * y - x);
            arrow[2] = new PointF(15f - 3f * x - y, 30f - 3f * y + x);
            graphics.FillPolygon(Playground.bugBrush, arrow);

            // draw bug vitality
            graphics.DrawString(bug.Vitality.ToString(), bigFont, Brushes.Red, 40f, 15f);

            base.OnPaint(e);
        }
    }
}