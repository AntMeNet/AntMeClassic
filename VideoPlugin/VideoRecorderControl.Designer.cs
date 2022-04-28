namespace AntMe.Plugin.Video
{
    partial class VideoRecorderControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoRecorderControl));
            this.titelLabel = new System.Windows.Forms.Label();
            this.simulationsListView = new System.Windows.Forms.ListView();
            this.dateColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playerColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.recorderLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // titelLabel
            // 
            resources.ApplyResources(this.titelLabel, "titelLabel");
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.Name = "titelLabel";
            // 
            // simulationsListView
            // 
            resources.ApplyResources(this.simulationsListView, "simulationsListView");
            this.simulationsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.dateColumn,
            this.playerColumn});
            this.simulationsListView.FullRowSelect = true;
            this.simulationsListView.HideSelection = false;
            this.simulationsListView.MultiSelect = false;
            this.simulationsListView.Name = "simulationsListView";
            this.simulationsListView.SmallImageList = this.imageList;
            this.simulationsListView.UseCompatibleStateImageBehavior = false;
            this.simulationsListView.View = System.Windows.Forms.View.Details;
            // 
            // dateColumn
            // 
            resources.ApplyResources(this.dateColumn, "dateColumn");
            // 
            // playerColumn
            // 
            resources.ApplyResources(this.playerColumn, "playerColumn");
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "video");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // saveButton
            // 
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // recorderLabel
            // 
            resources.ApplyResources(this.recorderLabel, "recorderLabel");
            this.recorderLabel.BackColor = System.Drawing.Color.Red;
            this.recorderLabel.ForeColor = System.Drawing.Color.White;
            this.recorderLabel.Name = "recorderLabel";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "antvi";
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // VideoRecorderControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.recorderLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.simulationsListView);
            this.Controls.Add(this.titelLabel);
            this.Name = "VideoRecorderControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.ListView simulationsListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader dateColumn;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label recorderLabel;
        private System.Windows.Forms.ColumnHeader playerColumn;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
