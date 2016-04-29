using System.IO;
using System.Windows.Forms;

using AntMe.Simulation;

namespace AntMe.Plugin.Simulation {
    internal sealed partial class AntProperties : Form {
        private readonly PlayerInfoFilename player;

        public AntProperties(PlayerInfoFilename playerInfo) {
            player = playerInfo;
            InitializeComponent();

            // Daten einfüllen
            kiNameLabel1.Text = player.ColonyName;
            kiNameLabel3.Text = player.ColonyName;
            kiNameLabel2.Text = player.ColonyName;
            Text = string.Format(Resource.SimulatorPluginAntPropertiesTitle, player.ColonyName);

            autorLabel.Text = string.Format(Resource.SimulatorPluginAntPropertiesAuthorFormat, player.FirstName, player.LastName);
            versionLabel.Text = player.SimulationVersion.ToString();
            spracheLabel.Text = player.Language.ToString();
            statischLabel.Text = player.Static ? Resource.Yes : Resource.No;
            debugLabel.Text = player.HasDebugInformation ? Resource.Yes : Resource.No;
            klassennameTextBox.Text = player.ClassName;

            dateinameTextBox.Text = player.File;
            try {
                FileInfo info = new FileInfo(player.File);
                dateigrößeLabel.Text = info.Length + " Byte";
                datumLabel.Text = info.CreationTime.ToLongDateString() + " " + info.CreationTime.ToShortTimeString();
            }
            catch {
                dateigrößeLabel.Text = Resource.SimulatorPluginAntPropertiesUnknown;
                datumLabel.Text = Resource.SimulatorPluginAntPropertiesUnknown;
            }

            // Ameisenkasten einfügen
            castesListView.Items.Clear();
            foreach (CasteInfo info in player.Castes) {
                ListViewItem item = castesListView.Items.Add(info.Name, "ant");
                item.Tag = info;
            }
            select_caste(null, null);

            // Sicherheitsliste
            int count = 0;
            if (player.RequestFileAccess) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesIoAccess, "security_closed");
                count++;
            }
            if (player.RequestDatabaseAccess) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesDbAccess, "security_closed");
                count++;
            }
            if (player.RequestReferences) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesRefAccess, "security_closed");
                count++;
            }
            if (player.RequestUserInterfaceAccess) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesUiAccess, "security_closed");
                count++;
            }
            if (player.RequestNetworkAccess) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesNetAccess, "security_closed");
                count++;
            }
            if (count==0) {
                rechteListView.Items.Add(Resource.SimulatorPluginAntPropertiesNoAccess, "security_closed");
            }
            zusatzinfosTextBox.Text = player.RequestInformation == string.Empty
                                          ? Resource.SimulatorPluginAntPropertiesNoAdditionalInfos
                                          : player.RequestInformation;

        }

        private void select_caste(object sender, System.EventArgs e)
        {
            // TODO: Resultierende Werte aus den Settings hinzu schreiben
            if (castesListView.SelectedItems.Count > 0) {
                casteGroupBox.Enabled = true;
                CasteInfo info = (CasteInfo) castesListView.SelectedItems[0].Tag;
                casteNameLabel.Text = info.Name;
                attackLabel.Text = info.Attack.ToString();
                energyLabel.Text = info.Energy.ToString();
                loadLabel.Text = info.Load.ToString();
                speedLabel.Text = info.Speed.ToString();
                rotationSpeedLabel.Text = info.RotationSpeed.ToString();
                rangeLabel.Text = info.Range.ToString();
                viewRangelabel.Text = info.ViewRange.ToString();
                
            }
            else {
                casteGroupBox.Enabled = false;
                casteNameLabel.Text = string.Empty;
                attackLabel.Text = string.Empty;
                energyLabel.Text = string.Empty;
                loadLabel.Text = string.Empty;
                speedLabel.Text = string.Empty;
                rotationSpeedLabel.Text = string.Empty;
                rangeLabel.Text = string.Empty;
                viewRangelabel.Text = string.Empty;
            }
        }
    }
}