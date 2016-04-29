using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AntMe.Gui {
    /// <summary>
    /// Window, to manage the plugins
    /// </summary>
    internal sealed partial class Plugins : Form {
        private readonly PluginManager manager;
        private readonly ListViewGroup consumerGroup;
        private readonly ListViewGroup producerGroup;

        private bool ignoreChecks = false;

        public Plugins(PluginManager manager) {
            InitializeComponent();

            // Save Manager
            this.manager = manager;

            // prepareing list
            producerGroup = pluginListView.Groups["producer"];
            consumerGroup = pluginListView.Groups["consumer"];
            UpdateList();
        }

        private void UpdateList() {
            // fill list
            pluginListView.Items.Clear();

            // Producer
            foreach (PluginItem plugin in manager.ProducerPlugins) {
                ListViewItem item = pluginListView.Items.Add(plugin.Name);
                item.Tag = plugin;
                item.Checked = (manager.ActiveProducerPlugin == plugin);
                item.Group = producerGroup;
                item.ToolTipText = plugin.Description;
                item.SubItems.Add(plugin.Version.ToString());
                item.SubItems.Add(plugin.Description);
            }

            // Consumer
            List<PluginItem> activeConsumer = new List<PluginItem>(manager.ActiveConsumerPlugins);
            foreach (PluginItem plugin in manager.ConsumerPlugins) {
                ListViewItem item = pluginListView.Items.Add(plugin.Name);
                item.Tag = plugin;
                item.Checked = (activeConsumer.Contains(plugin));
                item.Group = consumerGroup;
                item.ToolTipText = plugin.Description;
                item.SubItems.Add(plugin.Version.ToString());
                item.SubItems.Add(plugin.Description);
            }
        }

        private void addPluginButton_Click(object sender, EventArgs e) {
            openFileDialog.InitialDirectory = Application.ExecutablePath;
            openFileDialog.FileName = string.Empty;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
                try {
                    foreach (string filename in openFileDialog.FileNames) {

                        FileInfo fileInfo = new FileInfo(filename);
                        manager.CheckForPlugin(fileInfo);
                    }
                    if (manager.Exceptions.Count > 0) {
                        ExceptionViewer problems = new ExceptionViewer(manager.Exceptions);
                        problems.ShowDialog(this);
                        manager.Exceptions.Clear();
                    }
                    UpdateList();
                    manager.SaveSettings();
                }
                catch (Exception ex) {
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        private void pluginListView_ItemCheck(object sender, ItemCheckEventArgs e) {
            // Ignore automatic checks
            if (ignoreChecks) {
                return;
            }

            ignoreChecks = true;

            // Producer has changed
            if (pluginListView.Items[e.Index].Group == producerGroup) {
                // Prevent from uncheck
                if (e.NewValue == CheckState.Unchecked) {
                    e.NewValue = CheckState.Checked;
                }
                else {
                    foreach (ListViewItem item in pluginListView.Items) {
                        if (item.Index != e.Index && item.Group == producerGroup) {
                            item.Checked = false;
                        }
                    }

                    PluginItem plugin = (PluginItem) pluginListView.Items[e.Index].Tag;
                    manager.ActivateProducer(plugin.Guid);
                }
            }

            // Consumer has changed
            if (pluginListView.Items[e.Index].Group == consumerGroup) {
                PluginItem plugin = (PluginItem) pluginListView.Items[e.Index].Tag;
                if (e.NewValue == CheckState.Checked) {
                    // Activate
                    manager.ActivateConsumer(plugin.Guid);
                }
                else {
                    // Deactivate
                    manager.DeactivateConsumer(plugin.Guid);
                }
            }

            ignoreChecks = false;
        }
    }
}