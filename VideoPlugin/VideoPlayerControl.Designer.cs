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
            this.roundsLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.stateLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.playerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titelLabel
            // 
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.titelLabel, "titelLabel");
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.Name = "titelLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // openButton
            // 
            resources.ApplyResources(this.openButton, "openButton");
            this.openButton.Name = "openButton";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // roundsLabel
            // 
            resources.ApplyResources(this.roundsLabel, "roundsLabel");
            this.roundsLabel.Name = "roundsLabel";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "antvi";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
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
            resources.ApplyResources(this.infoPanel, "infoPanel");
            this.infoPanel.Name = "infoPanel";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // stateLabel
            // 
            resources.ApplyResources(this.stateLabel, "stateLabel");
            this.stateLabel.Name = "stateLabel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // playerLabel
            // 
            resources.ApplyResources(this.playerLabel, "playerLabel");
            this.playerLabel.Name = "playerLabel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // loadingProgressBar
            // 
            resources.ApplyResources(this.loadingProgressBar, "loadingProgressBar");
            this.loadingProgressBar.Name = "loadingProgressBar";
            // 
            // VideoPlayerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.loadingProgressBar);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titelLabel);
            this.Name = "VideoPlayerControl";
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label roundsLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.ProgressBar loadingProgressBar;
        private System.Windows.Forms.Label playerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
