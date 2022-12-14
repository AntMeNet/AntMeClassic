using AntMe.Gui.Properties;
using AntMe.PlayerManagement;
using AntMe.SharedComponents.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntMe.Gui
{
    internal sealed partial class Main : Form
    {
        #region Variablen

        private readonly PluginManager manager;

        private PluginItem activeProducer;
        private readonly List<PluginItem> activeConsumers = new List<PluginItem>();
        private bool ignoreTimerEvents = false;
        private readonly bool initPhase = false;
        private bool restart = false;
        private readonly bool directstart = false;
        private string updateUrl = string.Empty;

        #endregion

        #region Konstruktor und Initialisierung

        public Main(string[] parameter)
        {
            initPhase = true;

            InitializeComponent();
            CreateHandle();

            // Check language buttons
            switch (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "de":
                    germanMenuItem.Checked = true;
                    break;
                default:
                    englishMenuItem.Checked = true;
                    break;
            }

            // Load Player
            Task t = new Task(() => { PlayerStore.Instance.ToString(); });
            t.Start();

            // Load welcomepage
            try
            {
                infoWebBrowser.Navigate(Resource.MainWelcomePageUrl);
            }
            catch { }

            manager = new PluginManager();

            try
            {
                manager.LoadSettings();
            }
            catch (Exception ex)
            {
                ExceptionViewer problems = new ExceptionViewer(ex);
                problems.ShowDialog(this);
            }

            // Set window position
            WindowState = Settings.Default.windowState;
            Location = Settings.Default.windowPosition;
            Size = Settings.Default.windowSize;

            manager.SearchForPlugins();
            timer.Enabled = true;

            // Forward start parameter
            foreach (PluginItem plugin in manager.ProducerPlugins)
            {
                plugin.Producer.StartupParameter(parameter);
            }
            foreach (PluginItem plugin in manager.ConsumerPlugins)
            {
                plugin.Consumer.StartupParameter(parameter);
            }

            foreach (string p in parameter)
            {
                if (p.ToUpper() == "/START")
                {
                    directstart = true;
                }
            }

            initPhase = false;
        }

        #endregion

        #region Frontend and interaction handling

        /// <summary>
        /// Make updates based on manager settings.
        /// </summary>
        private void updatePanel()
        {

            if (ignoreTimerEvents)
            {
                return;
            }

            ignoreTimerEvents = true;

            // Controlling buttons
            startMenuItem.Enabled = manager.CanStart;
            startToolItem.Enabled = manager.CanStart;
            pauseToolItem.Enabled = manager.CanPause;
            pauseMenuItem.Enabled = manager.CanPause;
            stopToolItem.Enabled = manager.CanStop;
            stopMenuItem.Enabled = manager.CanStop;

            if (manager.FrameLimiterEnabled)
            {
                if ((int)Math.Round(manager.FrameLimit) <= 100)
                {
                    fasterToolItem.Enabled = true;
                }
                else
                {
                    fasterToolItem.Enabled = false;
                }
                if ((int)Math.Round(manager.FrameLimit) > 1)
                {
                    slowerToolItem.Enabled = true;
                }
                else
                {
                    slowerToolItem.Enabled = false;
                }

                speedDropDownToolItem.Text = string.Format(Resource.MainFramesPerSecond, manager.FrameLimit);
            }
            else
            {
                slowerToolItem.Enabled = false;
                fasterToolItem.Enabled = false;
                speedDropDownToolItem.Text = Resource.MainSpeedMaximal;
            }

            // Producer list (button based)
            List<ToolStripItem> remove = new List<ToolStripItem>();
            foreach (ToolStripItem item in producerButton.DropDownItems)
            {
                if (!manager.ProducerPlugins.Any(p => p == item.Tag))
                    remove.Add(item);
            }
            foreach (var item in remove)
            {
                producerButton.DropDownItems.Remove(item);
            }

            foreach (var item in manager.ProducerPlugins)
            {
                if (producerButton.DropDownItems.Find(item.Guid.ToString(), false).Count() == 0)
                {
                    var menuItem = new ToolStripMenuItem()
                    {
                        Text = item.Name,
                        Name = item.Guid.ToString(),
                        Tag = item
                    };

                    menuItem.Click += button_producer;

                    producerButton.DropDownItems.Add(menuItem);
                }
            }

            // Manage tabs
            if (activeProducer != manager.ActiveProducerPlugin)
            {
                bool isSelected = tabControl.SelectedIndex == 1;

                // Update Mode Display
                producerButton.Text = (manager.ActiveProducerPlugin == null ? Resource.MainNoModeSelected : manager.ActiveProducerPlugin.Name);

                // Remove old tab
                if (activeProducer != null)
                {
                    if (activeProducer.Producer.Control != null)
                    {
                        tabControl.TabPages.RemoveAt(1);
                    }
                    activeProducer = null;
                }

                // Add new tab
                if (manager.ActiveProducerPlugin != null)
                {
                    if (manager.ActiveProducerPlugin.Producer.Control != null)
                    {
                        TabPage page = new TabPage(manager.ActiveProducerPlugin.Name);
                        page.Padding = new Padding(0);
                        page.Margin = new Padding(0);
                        page.Controls.Add(manager.ActiveProducerPlugin.Producer.Control);
                        manager.ActiveProducerPlugin.Producer.Control.Padding = new Padding(0);
                        manager.ActiveProducerPlugin.Producer.Control.Margin = new Padding(0);
                        tabControl.TabPages.Insert(1, page);
                        manager.ActiveProducerPlugin.Producer.Control.Dock = DockStyle.Fill;
                        if (isSelected) tabControl.SelectedIndex = 1;
                    }
                    activeProducer = manager.ActiveProducerPlugin;
                }
            }

            // Synchronize consumer
            List<PluginItem> newActiveConsumers = new List<PluginItem>(manager.ActiveConsumerPlugins);
            for (int i = activeConsumers.Count - 1; i >= 0; i--)
            {
                // Kick the old tab
                if (!newActiveConsumers.Contains(activeConsumers[i]))
                {
                    if (tabControl.TabPages.ContainsKey(activeConsumers[i].Guid.ToString()))
                    {
                        tabControl.TabPages.RemoveByKey(activeConsumers[i].Guid.ToString());
                    }
                    activeConsumers.Remove(activeConsumers[i]);
                }
            }
            foreach (PluginItem plugin in newActiveConsumers)
            {
                //Create new, if needed
                if (!activeConsumers.Contains(plugin))
                {
                    // Create tab and place control
                    if (plugin.Consumer.Control != null)
                    {
                        tabControl.TabPages.Add(plugin.Guid.ToString(), plugin.Name);
                        tabControl.TabPages[plugin.Guid.ToString()].Controls.Add(plugin.Consumer.Control);
                        plugin.Consumer.Control.Dock = DockStyle.Fill;
                    }
                    activeConsumers.Add(plugin);
                }
            }

            // popup exceptions
            if (manager.Exceptions.Count > 0)
            {
                ExceptionViewer problems = new ExceptionViewer(manager.Exceptions);
                problems.ShowDialog(this);
                manager.Exceptions.Clear();
            }

            // Status bar information
            stateLabelBarItem.Text = string.Empty;
            switch (manager.State)
            {
                case PluginState.NotReady:
                    stateLabelBarItem.Text = Resource.MainStateNotReady;
                    break;
                case PluginState.Paused:
                    stateLabelBarItem.Text = Resource.MainStatePaused;
                    break;
                case PluginState.Ready:
                    stateLabelBarItem.Text = Resource.MainStateReady;
                    break;
                case PluginState.Running:
                    stateLabelBarItem.Text = Resource.MainStateRunning;
                    break;
            }

            if (manager.State == PluginState.Running || manager.State == PluginState.Paused)
            {
                progressBarItem.Maximum = manager.TotalRounds;
                progressBarItem.Value = manager.CurrentRound;
                stepCounterBarItem.Text = string.Format(Resource.MainStateRoundIndicator, manager.CurrentRound, manager.TotalRounds);
                progressBarItem.Visible = true;
                stepCounterBarItem.Visible = true;
            }
            else
            {
                progressBarItem.Visible = false;
                stepCounterBarItem.Visible = false;
            }

            if (manager.State == PluginState.Running)
            {
                fpsBarItem.Text = manager.FrameRate.ToString(Resource.MainStateFramesPerSecond);
                fpsBarItem.Visible = true;
            }
            else
            {
                fpsBarItem.Visible = false;
            }

            ignoreTimerEvents = false;
        }

        #endregion

        #region form functions

        #region form

        private void form_shown(object sender, EventArgs e)
        {
            updatePanel();

            if (manager.Exceptions.Count > 0)
            {
                ExceptionViewer problems = new ExceptionViewer(manager.Exceptions);
                problems.ShowDialog(this);
                manager.Exceptions.Clear();
            }

            // force a direkt start, if manager is ready.
            if (manager.CanStart && directstart)
            {
                start(sender, e);
            }
        }

        private void form_close(object sender, FormClosingEventArgs e)
        {
            if (manager.CanStop)
            {
                manager.Stop();
            }

            // Save all plugin settings.
            Settings.Default.Save();
            manager.SaveSettings();

            // Show possible problems.
            if (manager.Exceptions != null && manager.Exceptions.Count > 0)
            {
                ExceptionViewer form = new ExceptionViewer(manager.Exceptions);
                manager.Exceptions.Clear();
                form.ShowDialog(this);
            }
        }

        private void form_resize(object sender, EventArgs e)
        {
            if (initPhase)
            {
                return;
            }

            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.windowPosition = Location;
                Settings.Default.windowSize = Size;
            }

            if (WindowState != FormWindowState.Minimized)
            {
                Settings.Default.windowState = WindowState;
            }
        }

        #endregion

        #region tab

        private void tab_select(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Tag != null)
            {
                manager.SetVisiblePlugin(((PluginItem)e.TabPage.Tag).Guid);
            }
            else
            {
                manager.SetVisiblePlugin(new Guid());
            }
        }

        #endregion

        #region buttons

        private void button_close(object sender, EventArgs e)
        {
            Close();
        }

        private void button_plugins(object sender, EventArgs e)
        {
            ignoreTimerEvents = true;
            Plugins pluginForm = new Plugins(manager);
            pluginForm.ShowDialog(this);
            manager.SaveSettings();
            ignoreTimerEvents = false;
            updatePanel();
        }

        private void button_website(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Resource.MainWebsiteLink);
        }

        private void button_wiki(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Resource.MainWikiLink);
        }

        private void button_info(object sender, EventArgs e)
        {
            InfoBox infoBox = new InfoBox();
            infoBox.ShowDialog(this);
        }

        private void button_limitSetTo2(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 2.0f);
        }

        private void button_limitSetTo8(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 8.0f);
        }

        private void button_limitSetTo15(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 15.0f);
        }

        private void button_limitSetTo22(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 22.5f);
        }

        private void button_limitSetTo30(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 30.0f);
        }

        private void button_limitSetTo50(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 50.0f);
        }

        private void button_limitSetTo80(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 80.0f);
        }

        private void button_limitSetTo100(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(true, 100.0f);
        }

        private void button_limitSetToMax(object sender, EventArgs e)
        {
            manager.SetSpeedLimit(false, 0.0f);
        }

        private void button_limitFaster(object sender, EventArgs e)
        {
            if (manager.FrameLimiterEnabled)
            {
                if ((int)Math.Round(manager.FrameLimit) < 100)
                {
                    manager.SetSpeedLimit(true, (int)Math.Round(manager.FrameLimit) + 1);
                }
                else
                {
                    manager.SetSpeedLimit(false, 0.0f);
                }
            }
        }

        private void button_limitSlower(object sender, EventArgs e)
        {
            if (manager.FrameLimiterEnabled && (int)Math.Round(manager.FrameLimit) > 1)
            {
                manager.SetSpeedLimit(true, (int)Math.Round(manager.FrameLimit) - 1);
            }
        }

        private void button_german(object sender, EventArgs e)
        {
            Settings.Default.culture = "de";
            Settings.Default.Save();
            restart = true;
            Close();
        }

        private void button_english(object sender, EventArgs e)
        {
            Settings.Default.culture = "en";
            Settings.Default.Save();
            restart = true;
            Close();
        }

        private void button_producer(object sender, EventArgs e)
        {
            if (ignoreTimerEvents)
                return;

            ignoreTimerEvents = true;

            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            PluginItem plugin = menuItem.Tag as PluginItem;
            manager.ActivateProducer(plugin.Guid);

            updatePanel();
            ignoreTimerEvents = false;
        }

        #endregion

        #region timer

        private void timer_tick(object sender, EventArgs e)
        {
            if (!ignoreTimerEvents)
            {
                updatePanel();
            }
        }

        #endregion

        #endregion

        #region manager control

        private void start(object sender, EventArgs e)
        {
            if (manager.CanStart)
            {
                manager.Start();

                // Aktives Eingangsplugin anzeigen
                if (activeProducer.Producer.Control != null)
                {
                    tabControl.SelectedIndex = 1;
                }
            }
        }

        private void stop(object sender, EventArgs e)
        {
            if (manager.CanStop)
            {
                manager.Stop();
            }
        }

        private void pause(object sender, EventArgs e)
        {
            if (manager.CanPause)
            {
                manager.Pause();
            }
        }

        #endregion

        public bool Restart
        {
            get { return restart; }
        }

        private void infoWebBrowser_NavigateError(object sender, WebBrowserNavigateErrorEventArgs e)
        {
            infoWebBrowser.Navigate("file://" + Application.StartupPath + Resource.MainWelcomePagePath);
        }
    }
}