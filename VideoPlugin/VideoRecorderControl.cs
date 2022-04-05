using System;
using System.IO;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    public partial class VideoRecorderControl : UserControl
    {
        private bool recording = false;

        private int recordedFrame = 0;

        public VideoRecorderControl()
        {
            InitializeComponent();
            timer.Enabled = true;
        }

        public void Add(Stream stream, string player)
        {
            ListViewItem item = simulationsListView.Items.Add(DateTime.Now.ToString());
            item.Tag = stream;
            item.ImageKey = "video";
            item.SubItems.Add(player);
        }

        public bool Recording
        {
            get { return recording; }
            set
            {
                recording = value;
            }
        }

        public int RecordedFrame
        {
            get { return recordedFrame; }
            set
            {
                recordedFrame = value;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            recorderLabel.Visible = (DateTime.Now.Second % 2 == 0) && Recording;
            saveButton.Enabled = simulationsListView.SelectedItems.Count > 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (simulationsListView.SelectedItems.Count > 0)
            {
                Stream file = simulationsListView.SelectedItems[0].Tag as Stream;
                file.Seek(0, SeekOrigin.Begin);
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // TODO: Über AntMe! Reader/Writer lösen für Anonymisierung
                        using (FileStream output = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                        {
                            byte[] buffer = new byte[1024];
                            int size;
                            do
                            {
                                size = file.Read(buffer, 0, 1024);
                                output.Write(buffer, 0, size);
                            } while (size > 0);
                            MessageBox.Show("Erfolgreich gespeichert");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
