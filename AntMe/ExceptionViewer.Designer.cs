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
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.eyecatcherPictureBox = new System.Windows.Forms.PictureBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.clipboardButton = new System.Windows.Forms.Button();
            this.mailButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.eyecatcherPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // exceptionListView
            // 
            resources.ApplyResources(this.exceptionListView, "exceptionListView");
            this.exceptionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.descriptionColumn});
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
            resources.ApplyResources(this.eyecatcherPictureBox, "eyecatcherPictureBox");
            this.eyecatcherPictureBox.Image = global::AntMe.Gui.Properties.Resources.error_48X48;
            this.eyecatcherPictureBox.Name = "eyecatcherPictureBox";
            this.eyecatcherPictureBox.TabStop = false;
            // 
            // descriptionLabel
            // 
            resources.ApplyResources(this.descriptionLabel, "descriptionLabel");
            this.descriptionLabel.Name = "descriptionLabel";
            // 
            // clipboardButton
            // 
            resources.ApplyResources(this.clipboardButton, "clipboardButton");
            this.clipboardButton.Name = "clipboardButton";
            this.clipboardButton.UseVisualStyleBackColor = true;
            this.clipboardButton.Click += new System.EventHandler(this.clipboardButton_Click);
            // 
            // mailButton
            // 
            resources.ApplyResources(this.mailButton, "mailButton");
            this.mailButton.Name = "mailButton";
            this.mailButton.UseVisualStyleBackColor = true;
            this.mailButton.Click += new System.EventHandler(this.mailButton_Click);
            // 
            // ExceptionViewer
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.okButton;
            this.Controls.Add(this.mailButton);
            this.Controls.Add(this.clipboardButton);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.eyecatcherPictureBox);
            this.Controls.Add(this.exceptionListView);
            this.Controls.Add(this.okButton);
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
        private System.Windows.Forms.Button mailButton;
    }
}