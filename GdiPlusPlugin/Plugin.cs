using System;
using System.Drawing;
using System.Windows.Forms;

using AntMe.SharedComponents.Plugin;
using AntMe.SharedComponents.States;

namespace AntMe.Plugin.GdiPlusPlugin
{

	/// <summary>
    /// AntMe! Verbraucher-Plugin das ein Spiel in einer GDI+ basierten 2D-Ansicht darstellt.
    /// </summary>
    /// <author>Wolfgang Gallo (wolfgang@antme.net)</author>
    public class Plugin : IConsumerPlugin
    {

        private Window window;

        /// <summary>
        /// Erzeugt eine neue Instanz der Plugin-Klasse.
        /// </summary>
        public Plugin()
        {
            window = new Window();
        }

        #region IPlugin

        private PluginState pluginStatus = PluginState.Ready;

        /// <summary>
        /// Funktionsaufruf zum Starten des Plugin-Betriebs.
        /// </summary>
        public void Start()
        {
            window.Start();
            pluginStatus = PluginState.Running;
        }

        /// <summary>
        /// Funktionsaufruf zum Stoppen des Plugin-Betriebs.
        /// </summary>
        public void Stop()
        {
            window.Stop();
            pluginStatus = PluginState.Ready;
        }

        /// <summary>
        /// Hält den Betrieb des Plugins an.
        /// </summary>
        public void Pause()
        {
            pluginStatus = PluginState.Paused;
        }

        /// <summary>
        /// Gibt den Namen des Plugins zurück.
        /// </summary>
        public string Name
        {
            get { return "2D-Visualisierung"; }
        }

        /// <summary>
        /// Gibt einen Beschreibungstext dieses Plugins zurück.
        /// </summary>
        public string Description
        {
            get { return "Zeigen Sie die Simulation in einer 2D-Welt an"; }
        }

        /// <summary>
        /// Gibt die Versionsnummer dieses Plugins zurück.
        /// </summary>
        public Version Version
        {
            get { return new Version(1, 7); }
        }

        /// <summary>
        /// Gibt die GUID dieses Plugins zurück.
        /// </summary>
        public Guid Guid
        {
            get { return new Guid("BBBD7C7A-FD3A-4656-B6DC-6A88463B2815"); }
        }

        /// <summary>
        /// Liefert den aktuellen Status des Plugins zurück.
        /// </summary>
        public PluginState State
        {
            get { return pluginStatus; }
        }

        /// <summary>
        /// Liefert einen Verweis auf ein UserControl das im Hauptfenster
        /// angezeigt wird.
        /// </summary>
        public Control Control
        {
            get { return null; }
        }

        /// <summary>
        /// Gibt einen Bytearray aus serialisierten Konfigurationsdaten dieses 
        /// Plugins zurück oder legt diesen fest.
        /// </summary>
        public byte[] Settings
        {
            get
            {
                return new byte[2]
                    {
                        window.UseAntiAliasing ? (byte)1 : (byte)0,
                        window.ShowScore ? (byte)1 : (byte)0
                    };
            }
            set
            {
                if (value.Length == 2)
                {
                    window.UseAntiAliasing = value[0] == 0 ? false : true;
                    window.ShowScore = value[1] == 0 ? false : true;
                }
            }
        }

		public void StartupParameter(string[] parameter)
		{
		}

		public void SetVisibility(bool visible)
		{
		}

		public void UpdateUI(SimulationState state)
		{
			window.Update(state);
		}

        #endregion

        #region IConsumerPlugin

        public bool Interrupt
        {
            get
			{ 
				// Wenn das Spiel läuft oder pausiert ist (also nicht nur bereit)
				// und das Fenster nicht sichtbar, dann wurde es geschlossen und
				// die Simulation kann abgebrochen werden.
				return (pluginStatus != PluginState.Ready && !window.Visible);
			}
        }

        public void CreateState(ref SimulationState state)
		{
		}

        public void CreatingState(ref SimulationState state)
		{
		}

        public void CreatedState(ref SimulationState state)
		{
		}

        #endregion

    }

}