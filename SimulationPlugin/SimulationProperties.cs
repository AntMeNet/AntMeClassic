using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    internal sealed partial class SimulationProperties : Form
    {
        private readonly FreeGameSetup setup;

        public SimulationProperties(FreeGameSetup setup)
        {
            this.setup = setup;
            InitializeComponent();

            fillControls();
        }

        private void fillControls()
        {
            #region Game-Settings

            // Limits
            roundsNumericUpDown.Minimum = SimulatorConfiguration.ROUNDSMIN;
            roundsNumericUpDown.Maximum = SimulatorConfiguration.ROUNDSMAX;
            loopsNumericUpDown.Minimum = SimulatorConfiguration.LOOPSMIN;
            loopsNumericUpDown.Maximum = SimulatorConfiguration.LOOPSMAX;

            // Values
            roundsNumericUpDown.Value = setup.SimulatorConfiguration.RoundCount;
            loopsNumericUpDown.Value = setup.SimulatorConfiguration.LoopCount;

            int initValue = Math.Abs(setup.SimulatorConfiguration.MapInitialValue);
            mapInitCheckBox.Checked = (initValue != 0);
            check_mapInit(null, null);
            mapGeneratorMaskedTextBox.Text = initValue.ToString("000000000");

            #endregion

            #region Debug and Timeouts

            // Limits
            loopTimeoutNumericUpDown.Minimum = SimulatorConfiguration.LOOPTIMEOUTMIN;
            loopTimeoutNumericUpDown.Maximum = int.MaxValue;
            roundTimeoutNumericUpDown.Minimum = SimulatorConfiguration.ROUNDTIMEOUTMIN;
            roundTimeoutNumericUpDown.Maximum = int.MaxValue;

            // Values
            debugInfoCheckBox.Checked = setup.SimulatorConfiguration.AllowDebuginformation;
            check_timeouts(null, null);
            ignoreTimeoutsCheckBox.Checked = setup.SimulatorConfiguration.IgnoreTimeouts;
            loopTimeoutNumericUpDown.Value = setup.SimulatorConfiguration.LoopTimeout;
            roundTimeoutNumericUpDown.Value = setup.SimulatorConfiguration.RoundTimeout;

            #endregion

            #region Security

            // Values
            allowIoCheckBox.Checked = setup.SimulatorConfiguration.AllowFileAccess;
            allowDbCheckBox.Checked = setup.SimulatorConfiguration.AllowDatabaseAccess;
            allowUiCheckBox.Checked = setup.SimulatorConfiguration.AllowUserinterfaceAccess;
            allowRefCheckBox.Checked = setup.SimulatorConfiguration.AllowReferences;
            allowNetworkCheckBox.Checked = setup.SimulatorConfiguration.AllowNetworkAccess;

            #endregion

            #region Core-Settings

            // Set Default 
            presetComboBox.Items.Add(SimulationSettings.Default);

            // Other Presets
            using (var stream = new MemoryStream(Presets.CaptureTheApple))
                presetComboBox.Items.Add(SimulationSettings.LoadSettings(stream));
            using (var stream = new MemoryStream(Presets.Debugging))
                presetComboBox.Items.Add(SimulationSettings.LoadSettings(stream));
            using (var stream = new MemoryStream(Presets.Heros))
                presetComboBox.Items.Add(SimulationSettings.LoadSettings(stream));
            using (var stream = new MemoryStream(Presets.SugarRun))
                presetComboBox.Items.Add(SimulationSettings.LoadSettings(stream));
            using (var stream = new MemoryStream(Presets.SurvivalOfTheFittest))
                presetComboBox.Items.Add(SimulationSettings.LoadSettings(stream));

            // Enumerate all known settingsfiles and add them to the combobox
            List<string> lostSettingsFiles = new List<string>();
            foreach (string knownSettingFile in setup.KnownSettingFiles)
            {
                try
                {
                    SimulationSettings settings = SimulationSettings.LoadSettings(knownSettingFile);
                    presetComboBox.Items.Add(settings);
                }
                catch (Exception)
                {
                    // TODO: Lokalisieren
                    MessageBox.Show("Fehler beim Laden der Settings-Datei " + knownSettingFile + ". Diese Datei wird aus der Liste der bekannten Settings entfernt.");
                    lostSettingsFiles.Add(knownSettingFile);
                }
            }

            // Remove all lost settingsfiles
            foreach (string lostSettingsFile in lostSettingsFiles)
            {
                setup.KnownSettingFiles.Remove(lostSettingsFile);
            }

            // Preselect current settings
            presetComboBox.SelectedItem = setup.SimulatorConfiguration.Settings;

            #endregion
        }

        private void readControls()
        {
            #region Game-Settings

            setup.SimulatorConfiguration.RoundCount = (int)roundsNumericUpDown.Value;
            setup.SimulatorConfiguration.LoopCount = (int)loopsNumericUpDown.Value;

            setup.SimulatorConfiguration.MapInitialValue = 0;
            if (mapInitCheckBox.Checked)
            {
                setup.SimulatorConfiguration.MapInitialValue = int.Parse(mapGeneratorMaskedTextBox.Text);
            }

            #endregion

            #region Debug and Timeouts

            setup.SimulatorConfiguration.AllowDebuginformation = debugInfoCheckBox.Checked;
            setup.SimulatorConfiguration.IgnoreTimeouts = ignoreTimeoutsCheckBox.Checked;
            setup.SimulatorConfiguration.LoopTimeout = (int)loopTimeoutNumericUpDown.Value;
            setup.SimulatorConfiguration.RoundTimeout = (int)roundTimeoutNumericUpDown.Value;

            #endregion

            #region Security

            setup.SimulatorConfiguration.AllowFileAccess = allowIoCheckBox.Checked;
            setup.SimulatorConfiguration.AllowDatabaseAccess = allowDbCheckBox.Checked;
            setup.SimulatorConfiguration.AllowUserinterfaceAccess = allowUiCheckBox.Checked;
            setup.SimulatorConfiguration.AllowReferences = allowRefCheckBox.Checked;
            setup.SimulatorConfiguration.AllowNetworkAccess = allowNetworkCheckBox.Checked;

            #endregion

            #region Core-Settings

            // Set selected Setting
            if (presetComboBox.SelectedIndex > -1)
            {
                SimulationSettings selectedSettings = (SimulationSettings)presetComboBox.SelectedItem;
                if (!selectedSettings.Equals(setup.SimulatorConfiguration.Settings))
                {
                    SimulationSettings.SetCustomSettings(selectedSettings);
                    setup.SimulatorConfiguration.Settings = selectedSettings;
                }
            }

            #endregion
        }

        private void form_closing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                readControls();
            }
        }

        private void button_generateMapInit(object sender, EventArgs e)
        {
            Random rand = new Random();
            mapGeneratorMaskedTextBox.Text = rand.Next(1, 999999999).ToString("000000000");
        }

        private void check_mapInit(object sender, EventArgs e)
        {
            if (mapInitCheckBox.Checked)
            {
                mapGeneratorMaskedTextBox.Enabled = true;
                mapGeneratorButton.Enabled = true;
            }
            else
            {
                mapGeneratorMaskedTextBox.Enabled = false;
                mapGeneratorButton.Enabled = false;
            }
        }

        private void check_timeouts(object sender, EventArgs e)
        {
            if (ignoreTimeoutsCheckBox.Checked)
            {
                roundTimeoutNumericUpDown.Enabled = false;
                loopTimeoutNumericUpDown.Enabled = false;
            }
            else
            {
                roundTimeoutNumericUpDown.Enabled = true;
                loopTimeoutNumericUpDown.Enabled = true;
            }
        }

        private void button_loadSettings(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;

                // Try to load this settings
                try
                {
                    SimulationSettings setting = SimulationSettings.LoadSettings(filename);
                    setting.RuleCheck();

                    // Add to list
                    if (!setup.KnownSettingFiles.Contains(filename.ToLower()))
                    {
                        setup.KnownSettingFiles.Add(filename.ToLower());
                        presetComboBox.Items.Add(setting);
                        presetComboBox.SelectedItem = setting;
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Make that right
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void settingCreateButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;

                try
                {
                    SimulationSettings settings = SimulationSettings.Default;
                    settings.Guid = Guid.NewGuid();
                    SimulationSettings.SaveSettings(settings, filename);
                }
                catch (Exception ex)
                {
                    // TODO: Make that right
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}