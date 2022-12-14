using System.Drawing;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

    /// <summary>
    /// Base class for all info control box.
    /// </summary>
    internal abstract class InfoBox : Control
    {

        /// <summary>
        /// Bold and big fonts. 
        /// </summary>
        protected static Font boldFont, bigFont;

        /// <summary>
        /// Default font for information.
        /// </summary>
        protected static Font defaultFont;

        /// <summary>
        /// Insect's name.
        /// </summary>
        protected string name;

        /// <summary>
        /// Static constructor of info box.
        /// </summary>
        static InfoBox()
        {
            // Definition of the fonts.
            defaultFont = new Font("Microsoft Sans Serif", 8.25f);
            boldFont = new Font(defaultFont, FontStyle.Bold);
            bigFont = new Font("Microsoft Sans Serif", 20f);
        }

        public InfoBox()
        {
            // Buffering while drawing will be under control by AntMe.
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        /// <summary>
        /// Public get ID of insect.
        /// </summary>
        public abstract int Id { get; }

        // Bitmap graphics for background and drawing area.
        protected Bitmap bitmap;
        protected Graphics graphics;

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background bitmap graphics.
            e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
        }

    }

}