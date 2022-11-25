using System.Drawing;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// base class for all info control box
    /// </summary>
    internal abstract class InfoBox : Control
    {

        /// <summary>
        /// bold and big fonts 
        /// </summary>
        protected static Font boldFont, bigFont;

        /// <summary>
        /// default font for information
        /// </summary>
        protected static Font defaultFont;

        /// <summary>
        /// insects name
        /// </summary>
        protected string name;

        /// <summary>
        /// static constructor of info box
        /// </summary>
        static InfoBox()
        {
            // definition of the fonts
            defaultFont = new Font("Microsoft Sans Serif", 8.25f);
            boldFont = new Font(defaultFont, FontStyle.Bold);
            bigFont = new Font("Microsoft Sans Serif", 20f);
        }

        public InfoBox()
        {
            // buffering while drawing will be under control by antme
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        /// <summary>
        /// public get id of insect
        /// </summary>
        public abstract int Id { get; }

        // bitmap graphics for background and drawing area
        protected Bitmap bitmap;
        protected Graphics graphics;

        protected override void OnPaint(PaintEventArgs e)
        {
            // draw background bitmap graphics
            e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
        }

    }

}