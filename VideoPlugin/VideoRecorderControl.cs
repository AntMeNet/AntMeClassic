using System;
using System.IO;
using System.Windows.Forms;

namespace AntMe.Plugin.Video
{
    public partial class VideoRecorderControl : UserControl
    {
        public VideoRecorderControl()
        {
            InitializeComponent();
            timer.Enabled = true;
        }

        public void Add(Stream stream, string player)
        {
            var item = simulationsListView.Items.Add(DateTime.Now.ToString());
            item.Tag = stream;
            item.ImageKey = "video";
            item.SubItems.Add(player);
        }

        public bool Recording { get; set; }

        public int RecordedFrame { get; set; }

        private void timer_Tick(object sender, EventArgs e)
        {
            recorderLabel.Visible = DateTime.Now.Second % 2 == 0 && Recording;
            saveButton.Enabled = simulationsListView.SelectedItems.Count > 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (simulationsListView.SelectedItems.Count > 0)
            {
                var file = simulationsListView.SelectedItems[0].Tag as Stream;
                file.Seek(0, SeekOrigin.Begin);
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // TODO: Solve with AntMe! reader/writer for anonymization
                        using (var output = File.Open(saveFileDialog.FileName, FileMode.CreateNew))
                        {
                            byte[] buffer = new byte[1024];
                            int size;
                            do
                            {
                                size = file.Read(buffer, 0, 1024);
                                output.Write(buffer, 0, size);
                            } while (size > 0);
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
