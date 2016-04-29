namespace AntMe.Plugin.Simulation
{
    partial class RightsRequest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RightsRequest));
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.acceptCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.colonyLabel = new System.Windows.Forms.Label();
            this.authorLabel = new System.Windows.Forms.Label();
            this.rightsListView = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.detailsButton = new System.Windows.Forms.Button();
            this.acceptPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.sorryPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.acceptPanel.SuspendLayout();
            this.sorryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // yesButton
            // 
            this.yesButton.AccessibleDescription = null;
            this.yesButton.AccessibleName = null;
            resources.ApplyResources(this.yesButton, "yesButton");
            this.yesButton.BackgroundImage = null;
            this.yesButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.yesButton.Font = null;
            this.yesButton.Name = "yesButton";
            this.yesButton.UseVisualStyleBackColor = true;
            // 
            // noButton
            // 
            this.noButton.AccessibleDescription = null;
            this.noButton.AccessibleName = null;
            resources.ApplyResources(this.noButton, "noButton");
            this.noButton.BackgroundImage = null;
            this.noButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.noButton.Font = null;
            this.noButton.Name = "noButton";
            this.noButton.UseVisualStyleBackColor = true;
            // 
            // acceptCheckBox
            // 
            this.acceptCheckBox.AccessibleDescription = null;
            this.acceptCheckBox.AccessibleName = null;
            resources.ApplyResources(this.acceptCheckBox, "acceptCheckBox");
            this.acceptCheckBox.BackgroundImage = null;
            this.acceptCheckBox.Font = null;
            this.acceptCheckBox.Name = "acceptCheckBox";
            this.acceptCheckBox.UseVisualStyleBackColor = true;
            this.acceptCheckBox.CheckedChanged += new System.EventHandler(this.acceptCheckBox_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.AccessibleDescription = null;
            this.pictureBox1.AccessibleName = null;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackgroundImage = null;
            this.pictureBox1.Font = null;
            this.pictureBox1.Image = global::AntMe.Plugin.Simulation.Properties.Resources.security_32x32;
            this.pictureBox1.ImageLocation = null;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            this.label3.AccessibleDescription = null;
            this.label3.AccessibleName = null;
            resources.ApplyResources(this.label3, "label3");
            this.label3.Font = null;
            this.label3.Name = "label3";
            // 
            // colonyLabel
            // 
            this.colonyLabel.AccessibleDescription = null;
            this.colonyLabel.AccessibleName = null;
            resources.ApplyResources(this.colonyLabel, "colonyLabel");
            this.colonyLabel.Name = "colonyLabel";
            // 
            // authorLabel
            // 
            this.authorLabel.AccessibleDescription = null;
            this.authorLabel.AccessibleName = null;
            resources.ApplyResources(this.authorLabel, "authorLabel");
            this.authorLabel.Font = null;
            this.authorLabel.Name = "authorLabel";
            // 
            // rightsListView
            // 
            this.rightsListView.AccessibleDescription = null;
            this.rightsListView.AccessibleName = null;
            resources.ApplyResources(this.rightsListView, "rightsListView");
            this.rightsListView.BackgroundImage = null;
            this.rightsListView.Font = null;
            this.rightsListView.Name = "rightsListView";
            this.rightsListView.SmallImageList = this.imageList;
            this.rightsListView.UseCompatibleStateImageBehavior = false;
            this.rightsListView.View = System.Windows.Forms.View.List;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "closed");
            this.imageList.Images.SetKeyName(1, "open");
            // 
            // detailsButton
            // 
            this.detailsButton.AccessibleDescription = null;
            this.detailsButton.AccessibleName = null;
            resources.ApplyResources(this.detailsButton, "detailsButton");
            this.detailsButton.BackgroundImage = null;
            this.detailsButton.Font = null;
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // acceptPanel
            // 
            this.acceptPanel.AccessibleDescription = null;
            this.acceptPanel.AccessibleName = null;
            resources.ApplyResources(this.acceptPanel, "acceptPanel");
            this.acceptPanel.BackgroundImage = null;
            this.acceptPanel.Controls.Add(this.label5);
            this.acceptPanel.Controls.Add(this.yesButton);
            this.acceptPanel.Controls.Add(this.noButton);
            this.acceptPanel.Controls.Add(this.acceptCheckBox);
            this.acceptPanel.Font = null;
            this.acceptPanel.Name = "acceptPanel";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // sorryPanel
            // 
            this.sorryPanel.AccessibleDescription = null;
            this.sorryPanel.AccessibleName = null;
            resources.ApplyResources(this.sorryPanel, "sorryPanel");
            this.sorryPanel.BackgroundImage = null;
            this.sorryPanel.Controls.Add(this.label4);
            this.sorryPanel.Controls.Add(this.closeButton);
            this.sorryPanel.Font = null;
            this.sorryPanel.Name = "sorryPanel";
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // closeButton
            // 
            this.closeButton.AccessibleDescription = null;
            this.closeButton.AccessibleName = null;
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.BackgroundImage = null;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.closeButton.Font = null;
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // RightsRequest
            // 
            this.AcceptButton = this.yesButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.noButton;
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.rightsListView);
            this.Controls.Add(this.authorLabel);
            this.Controls.Add(this.colonyLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.acceptPanel);
            this.Controls.Add(this.sorryPanel);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RightsRequest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.acceptPanel.ResumeLayout(false);
            this.acceptPanel.PerformLayout();
            this.sorryPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.CheckBox acceptCheckBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label colonyLabel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.ListView rightsListView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Panel acceptPanel;
        private System.Windows.Forms.Panel sorryPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label5;
    }
}