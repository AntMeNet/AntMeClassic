using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using AntMe.Gui.Properties;

namespace AntMe.Gui {
    internal sealed class Program {
        [STAThread]
        public static void Main(string[] parameter) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool restart = true;

            while (restart) {

                // Language-Settings
                switch (Settings.Default.culture)
                {
                    case "de":
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                        break;
                    case "en":
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                        break;
                }

                // Run
                using (Main form = new Main(parameter)) {
                    Application.Run(form);
                    restart = form.Restart;
                }
            }
        }
    }
}