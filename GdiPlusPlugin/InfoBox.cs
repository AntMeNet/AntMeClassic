using System.Drawing;
using System.Windows.Forms;

namespace AntMe.Plugin.GdiPlusPlugin
{

	/// <summary>
	/// Basisklasse für alle Information-Kontrollelemente.
	/// </summary>
	internal abstract class InfoBox : Control
	{

		/// <summary>
		/// Schriftart zur Anzeige der Informationen.
		/// </summary>
		protected static Font boldFont, bigFont;

		/// <summary>
		/// Schriftart zur Anzeige der Informationen.
		/// </summary>
		protected static Font defaultFont;

		/// <summary>
		/// Der Name des Insekts.
		/// </summary>
		protected string name;

		/// <summary>
		/// Der statische Konstruktor.
		/// </summary>
		static InfoBox()
		{
			// Erzeuge die Schriftarten.
			defaultFont = new Font("Microsoft Sans Serif", 8.25f);
			boldFont = new Font(defaultFont, FontStyle.Bold);
			bigFont = new Font("Microsoft Sans Serif", 20f);
		}

		public InfoBox()
		{
			// Sage Windows, daß wir das Puffern beim Zeichnen selbst übernehmen.
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			UpdateStyles();
		}

		/// <summary>
		/// Die Id des Insekts zu dem Informationen dargestellt werden sollen.
		/// </summary>
		public abstract int Id { get; }

		// Die Hintergrund-Grafik und ihre Zeichenfläche.
		protected Bitmap bitmap;
		protected Graphics graphics;

		protected override void OnPaint(PaintEventArgs e)
		{
			// Zeichne die Hintergrund-Grafik.
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
		}

	}

}