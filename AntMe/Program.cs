using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using AntMe.Gui.Properties;

namespace AntMe.Gui
{
    internal sealed class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDefaultDllDirectories(int directoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern void AddDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        [STAThread]
        public static void Main(string[] parameter)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                try
                {
                    SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
                    AddDllDirectory(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        Environment.Is64BitProcess ? "x64" : "x86"
                    ));
                }
                catch
                {
                    // Pre-Windows 7, KB2533623 
                    SetDllDirectory(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        Environment.Is64BitProcess ? "x64" : "x86"
                    ));
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool restart = true;

            while (restart)
            {

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
                using (Main form = new Main(parameter))
                {
                    Application.Run(form);
                    restart = form.Restart;
                }
            }
        }
    }
}