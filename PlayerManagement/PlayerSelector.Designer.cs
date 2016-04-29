namespace AntMe.PlayerManagement
{
    partial class PlayerSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerSelector));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.createButton = new System.Windows.Forms.ToolStripButton();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.propertiesButton = new System.Windows.Forms.ToolStripButton();
            this.playerListView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.authorColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.toolStrip.SuspendLayout();
            this.playerContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createButton,
            this.loadButton,
            this.propertiesButton});
            this.toolStrip.Name = "toolStrip";
            // 
            // createButton
            // 
            resources.ApplyResources(this.createButton, "createButton");
            this.createButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createButton.Image = global::AntMe.PlayerManagement.Properties.Resources.add;
            this.createButton.Name = "createButton";
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // loadButton
            // 
            resources.ApplyResources(this.loadButton, "loadButton");
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadButton.Image = global::AntMe.PlayerManagement.Properties.Resources.load_16x16;
            this.loadButton.Name = "loadButton";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // propertiesButton
            // 
            resources.ApplyResources(this.propertiesButton, "propertiesButton");
            this.propertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.propertiesButton.Image = global::AntMe.PlayerManagement.Properties.Resources.properties_16x16;
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Click += new System.EventHandler(this.propertiesButton_Click);
            // 
            // playerListView
            // 
            resources.ApplyResources(this.playerListView, "playerListView");
            this.playerListView.AllowDrop = true;
            this.playerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.authorColumn});
            this.playerListView.ContextMenuStrip = this.playerContextMenu;
            this.playerListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("playerListView.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("playerListView.Groups1")))});
            this.playerListView.LargeImageList = this.imageList;
            this.playerListView.MultiSelect = false;
            this.playerListView.Name = "playerListView";
            this.playerListView.ShowItemToolTips = true;
            this.playerListView.SmallImageList = this.imageList;
            this.playerListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.playerListView.UseCompatibleStateImageBehavior = false;
            this.playerListView.View = System.Windows.Forms.View.Tile;
            this.playerListView.SelectedIndexChanged += new System.EventHandler(this.playerListView_SelectedIndexChanged);
            this.playerListView.DoubleClick += new System.EventHandler(this.playerListView_DoubleClick);
            // 
            // nameColumn
            // 
            resources.ApplyResources(this.nameColumn, "nameColumn");
            // 
            // authorColumn
            // 
            resources.ApplyResources(this.authorColumn, "authorColumn");
            // 
            // playerContextMenu
            // 
            resources.ApplyResources(this.playerContextMenu, "playerContextMenu");
            this.playerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createMenuItem,
            this.loadMenuItem,
            this.toolStripMenuItem1,
            this.propertiesMenuItem});
            this.playerContextMenu.Name = "contextMenuStrip1";
            // 
            // createMenuItem
            // 
            resources.ApplyResources(this.createMenuItem, "createMenuItem");
            this.createMenuItem.Name = "createMenuItem";
            this.createMenuItem.Click += new System.EventHandler(this.createButton_Click);
            // 
            // loadMenuItem
            // 
            resources.ApplyResources(this.loadMenuItem, "loadMenuItem");
            this.loadMenuItem.Name = "loadMenuItem";
            this.loadMenuItem.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // propertiesMenuItem
            // 
            resources.ApplyResources(this.propertiesMenuItem, "propertiesMenuItem");
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Click += new System.EventHandler(this.propertiesButton_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "nonstatic");
            this.imageList.Images.SetKeyName(1, "nonstatic_disabled");
            this.imageList.Images.SetKeyName(2, "nonstatic_disabled_secure");
            this.imageList.Images.SetKeyName(3, "nonstatic_secure");
            this.imageList.Images.SetKeyName(4, "static");
            this.imageList.Images.SetKeyName(5, "static_disabled");
            this.imageList.Images.SetKeyName(6, "static_secure");
            this.imageList.Images.SetKeyName(7, "static_disabled_secure");
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.dll";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            this.openFileDialog.Multiselect = true;
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // OkButton
            // 
            resources.ApplyResources(this.OkButton, "OkButton");
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Name = "OkButton";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // PlayerSelector
            // 
            this.AcceptButton = this.OkButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.playerListView);
            this.Controls.Add(this.toolStrip);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlayerSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.playerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton createButton;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripButton propertiesButton;
        private System.Windows.Forms.ListView playerListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader authorColumn;
        private System.Windows.Forms.ContextMenuStrip playerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem createMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.ImageList imageList;
    }
}