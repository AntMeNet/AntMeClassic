using AntMe.SharedComponents.AntVideo;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    public partial class VideoPlayerControl : UserControl
    {
        public Stream Stream { get; private set; }

        public VideoPlayerControl()
        {
            InitializeComponent();
            infoPanel.Visible = false;
            loadingProgressBar.Visible = false;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (Stream != null)
            {
                Stream.Close();
                Stream.Dispose();
                Stream = null;
                infoPanel.Visible = false;
            }

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Stream = File.Open(openFileDialog.FileName, FileMode.Open);
                roundsLabel.Text = openFileDialog.FileName;
                UpdateUi();
            }
        }

        private void UpdateUi()
        {
            if (Stream != null)
            {
                roundsLabel.Text = string.Empty;
                playerLabel.Text = string.Empty;
                stateLabel.Text = string.Empty;
                infoPanel.Visible = true;

                loadingProgressBar.Value = 0;
                loadingProgressBar.Visible = true;
                try
                {
                    Stream.Seek(0, SeekOrigin.Begin);
                    var reader = new AntVideoReader(Stream);
                    var lastState = reader.Read();
                    loadingProgressBar.Maximum = lastState.TotalRounds;
                    while (!reader.Complete)
                    {
                        lastState = reader.Read();
                        loadingProgressBar.Value = lastState.CurrentRound;
                    }
                    roundsLabel.Text = lastState.CurrentRound.ToString();
                    stateLabel.Text = lastState.CurrentRound == lastState.TotalRounds ? Resource.PlayerPluginStateFinished : Resource.PlayerPluginStateUnfinished;
                    playerLabel.Text = string.Join("\r\n", lastState.ColonyStates.Select(c => c.ColonyName));
                }
                catch (Exception ex)
                {
                    infoPanel.Visible = false;

                    MessageBox.Show(ex.Message);

                    Stream.Close();
                    Stream.Dispose();
                    Stream = null;
                }

                loadingProgressBar.Visible = false;
            }
            else
            {
                infoPanel.Visible = false;
                loadingProgressBar.Visible = false;

            }
        }
    }
}
