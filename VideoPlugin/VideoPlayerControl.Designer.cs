namespace AntMe.Plugin.Video
{
    partial class VideoPlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoPlayerControl));
            this.titelLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.roundsLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.stateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.playerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.replayTextBox = new System.Windows.Forms.TextBox();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titelLabel
            // 
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.titelLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.titelLabel.Location = new System.Drawing.Point(0, 0);
            this.titelLabel.Name = "titelLabel";
            this.titelLabel.Padding = new System.Windows.Forms.Padding(4);
            this.titelLabel.Size = new System.Drawing.Size(793, 27);
            this.titelLabel.TabIndex = 1;
            this.titelLabel.Text = "Video Player";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(14, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(687, 52);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(17, 107);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 5;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(98, 107);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 6;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // roundsLabel
            // 
            this.roundsLabel.AutoSize = true;
            this.roundsLabel.Location = new System.Drawing.Point(69, 16);
            this.roundsLabel.Name = "roundsLabel";
            this.roundsLabel.Size = new System.Drawing.Size(45, 13);
            this.roundsLabel.TabIndex = 7;
            this.roundsLabel.Text = "[rounds]";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "antvi";
            this.openFileDialog.Filter = "AntMe! Videos|*.antvi";
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.infoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPanel.Controls.Add(this.label4);
            this.infoPanel.Controls.Add(this.stateLabel);
            this.infoPanel.Controls.Add(this.label3);
            this.infoPanel.Controls.Add(this.playerLabel);
            this.infoPanel.Controls.Add(this.label2);
            this.infoPanel.Controls.Add(this.roundsLabel);
            this.infoPanel.Location = new System.Drawing.Point(17, 136);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(293, 197);
            this.infoPanel.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Player:";
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(69, 33);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(45, 13);
            this.stateLabel.TabIndex = 11;
            this.stateLabel.Text = "[rounds]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "State:";
            // 
            // playerLabel
            // 
            this.playerLabel.AutoSize = true;
            this.playerLabel.Location = new System.Drawing.Point(69, 50);
            this.playerLabel.Name = "playerLabel";
            this.playerLabel.Size = new System.Drawing.Size(41, 13);
            this.playerLabel.TabIndex = 9;
            this.playerLabel.Text = "[player]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Rounds:";
            // 
            // loadingProgressBar
            // 
            this.loadingProgressBar.Location = new System.Drawing.Point(17, 339);
            this.loadingProgressBar.Name = "loadingProgressBar";
            this.loadingProgressBar.Size = new System.Drawing.Size(175, 17);
            this.loadingProgressBar.TabIndex = 9;
            // 
            // replayTextBox
            // 
            this.replayTextBox.Location = new System.Drawing.Point(179, 109);
            this.replayTextBox.Name = "replayTextBox";
            this.replayTextBox.Size = new System.Drawing.Size(215, 20);
            this.replayTextBox.TabIndex = 10;
            // 
            // VideoPlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.replayTextBox);
            this.Controls.Add(this.loadingProgressBar);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titelLabel);
            this.Name = "VideoPlayerControl";
            this.Size = new System.Drawing.Size(793, 514);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Label roundsLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.ProgressBar loadingProgressBar;
        private System.Windows.Forms.Label playerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox replayTextBox;
    }
}
