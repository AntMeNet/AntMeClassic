using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AntMe.Online.Client;

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

        public int RecordedFrame {
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
            uploadButton.Enabled = simulationsListView.SelectedItems.Count > 0 && Connection.Instance.IsLoggedIn;
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

        private void uploadButton_Click(object sender, EventArgs e)
        {
            if (simulationsListView.SelectedItems.Count > 0)
            {
                try
                {
                    Stream file = simulationsListView.SelectedItems[0].Tag as Stream;
                    file.Seek(0, SeekOrigin.Begin);

                    byte[] clone = new byte[file.Length];
                    file.Read(clone, 0, clone.Length);
                    using (MemoryStream cloneStream = new MemoryStream(clone))
                    {
                        Replay replay = Connection.Instance.Replays.CreatePrivateReplay(cloneStream);
                        uploadTextbox.Text = replay.Id.ToString();
                        uploadTextbox.Visible = true;
                        MessageBox.Show("Upload erfolgreich. Verschicke die unten angezeigte ID an alle, die das Video sehen sollen.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Upload der Datei. " + ex.Message);
                }
            }
        }
    }
}
