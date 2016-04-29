namespace AntMe.Gui {
    partial class ExceptionViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionViewer));
            this.okButton = new System.Windows.Forms.Button();
            this.exceptionListView = new System.Windows.Forms.ListView();
            this.descriptionColumn = new System.Windows.Forms.ColumnHeader();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.eyecatcherPictureBox = new System.Windows.Forms.PictureBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.clipboardButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eyecatcherPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.AccessibleDescription = null;
            this.okButton.AccessibleName = null;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackgroundImage = null;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Font = null;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // exceptionListView
            // 
            this.exceptionListView.AccessibleDescription = null;
            this.exceptionListView.AccessibleName = null;
            resources.ApplyResources(this.exceptionListView, "exceptionListView");
            this.exceptionListView.BackgroundImage = null;
            this.exceptionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.descriptionColumn});
            this.exceptionListView.Font = null;
            this.exceptionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.exceptionListView.Name = "exceptionListView";
            this.exceptionListView.ShowItemToolTips = true;
            this.exceptionListView.SmallImageList = this.imageList;
            this.exceptionListView.UseCompatibleStateImageBehavior = false;
            this.exceptionListView.View = System.Windows.Forms.View.Details;
            // 
            // descriptionColumn
            // 
            resources.ApplyResources(this.descriptionColumn, "descriptionColumn");
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "error");
            // 
            // eyecatcherPictureBox
            // 
            this.eyecatcherPictureBox.AccessibleDescription = null;
            this.eyecatcherPictureBox.AccessibleName = null;
            resources.ApplyResources(this.eyecatcherPictureBox, "eyecatcherPictureBox");
            this.eyecatcherPictureBox.BackgroundImage = null;
            this.eyecatcherPictureBox.Font = null;
            this.eyecatcherPictureBox.Image = global::AntMe.Gui.Properties.Resources.error_48X48;
            this.eyecatcherPictureBox.ImageLocation = null;
            this.eyecatcherPictureBox.Name = "eyecatcherPictureBox";
            this.eyecatcherPictureBox.TabStop = false;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AccessibleDescription = null;
            this.descriptionLabel.AccessibleName = null;
            resources.ApplyResources(this.descriptionLabel, "descriptionLabel");
            this.descriptionLabel.Font = null;
            this.descriptionLabel.Name = "descriptionLabel";
            // 
            // clipboardButton
            // 
            this.clipboardButton.AccessibleDescription = null;
            this.clipboardButton.AccessibleName = null;
            resources.ApplyResources(this.clipboardButton, "clipboardButton");
            this.clipboardButton.BackgroundImage = null;
            this.clipboardButton.Font = null;
            this.clipboardButton.Name = "clipboardButton";
            this.clipboardButton.UseVisualStyleBackColor = true;
            this.clipboardButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ExceptionViewer
            // 
            this.AcceptButton = this.okButton;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.okButton;
            this.Controls.Add(this.clipboardButton);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.eyecatcherPictureBox);
            this.Controls.Add(this.exceptionListView);
            this.Controls.Add(this.okButton);
            this.Font = null;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionViewer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.eyecatcherPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView exceptionListView;
        private System.Windows.Forms.PictureBox eyecatcherPictureBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button clipboardButton;
    }
}