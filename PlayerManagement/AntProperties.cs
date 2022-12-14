using AntMe.Simulation;
using System.IO;
using System.Windows.Forms;

namespace AntMe.PlayerManagement
{
    /// <summary>
    /// Property Window for all kinds of <see cref="AntMe.Simulation.PlayerInfoFilename"/> Ants.
    /// </summary>
    public sealed partial class AntProperties : Form
    {
        private readonly PlayerInfoFilename player;

        /// <summary>
        /// Default Contructor.
        /// </summary>
        /// <param name="playerInfo">Reference to the PlayerInfo</param>
        public AntProperties(PlayerInfoFilename playerInfo)
        {
            player = playerInfo;
            InitializeComponent();

            // populate with data
            aiNameLabel1.Text = player.ColonyName;
            aiNameLabel3.Text = player.ColonyName;
            aiNameLabel2.Text = player.ColonyName;
            Text = string.Format(Resource.AntPropertiesTitle, player.ColonyName);

            authorLabel.Text = string.Format(Resource.AntPropertiesAuthorFormat, player.FirstName, player.LastName);
            versionLabel.Text = player.SimulationVersion.ToString();
            languageLabel.Text = player.Language.ToString();
            staticLabel.Text = player.Static ? Resource.Yes : Resource.No;
            debugLabel.Text = player.HasDebugInformation ? Resource.Yes : Resource.No;
            classnameTextBox.Text = player.ClassName;

            filenameTextBox.Text = player.File;
            try
            {
                FileInfo info = new FileInfo(player.File);
                fileSizeLabel.Text = info.Length + " Byte";
                dateLabel.Text = info.CreationTime.ToLongDateString() + " " + info.CreationTime.ToShortTimeString();
            }
            catch
            {
                fileSizeLabel.Text = Resource.AntPropertiesUnknown;
                dateLabel.Text = Resource.AntPropertiesUnknown;
            }

            // populate with ant castes
            castesListView.Items.Clear();
            foreach (CasteInfo info in player.Castes)
            {
                ListViewItem item = castesListView.Items.Add(info.Name, "ant");
                item.Tag = info;
            }
            select_caste(null, null);

            // security
            int count = 0;
            if (player.RequestFileAccess)
            {
                securityListView.Items.Add(Resource.AntPropertiesIoAccess, "security_closed");
                count++;
            }
            if (player.RequestDatabaseAccess)
            {
                securityListView.Items.Add(Resource.AntPropertiesDbAccess, "security_closed");
                count++;
            }
            if (player.RequestReferences)
            {
                securityListView.Items.Add(Resource.AntPropertiesRefAccess, "security_closed");
                count++;
            }
            if (player.RequestUserInterfaceAccess)
            {
                securityListView.Items.Add(Resource.AntPropertiesUiAccess, "security_closed");
                count++;
            }
            if (player.RequestNetworkAccess)
            {
                securityListView.Items.Add(Resource.AntPropertiesNetAccess, "security_closed");
                count++;
            }
            if (count == 0)
            {
                securityListView.Items.Add(Resource.AntPropertiesNoAccess, "security_closed");
            }
            additionalInformationTextBox.Text = player.RequestInformation == string.Empty
                                          ? Resource.AntPropertiesNoAdditionalInfos
                                          : player.RequestInformation;

        }

        private void select_caste(object sender, System.EventArgs e)
        {
            // TODO: add resulting values from the settings // Resultierende Werte aus den Settings hinzu schreiben
            if (castesListView.SelectedItems.Count > 0)
            {
                casteGroupBox.Enabled = true;
                CasteInfo info = (CasteInfo)castesListView.SelectedItems[0].Tag;
                casteNameLabel.Text = info.Name;
                attackLabel.Text = info.Attack.ToString();
                energyLabel.Text = info.Energy.ToString();
                loadLabel.Text = info.Load.ToString();
                speedLabel.Text = info.Speed.ToString();
                rotationSpeedLabel.Text = info.RotationSpeed.ToString();
                rangeLabel.Text = info.Range.ToString();
                viewRangelabel.Text = info.ViewRange.ToString();

            }
            else
            {
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