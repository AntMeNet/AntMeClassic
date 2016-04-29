namespace AntMe.Plugin.Simulation {
    partial class TeamSetup
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeamSetup));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.playerListView = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.authorColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.chooseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTeamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam3MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam4MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam5MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam6MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam7MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chooseTeam8MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.teamListView = new System.Windows.Forms.ListView();
            this.playerColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.author2Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.teamContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveNewTeamMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam3MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam4MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam5MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam6MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam7MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveTeam8MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.createButton = new System.Windows.Forms.ToolStripButton();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.removeButton = new System.Windows.Forms.ToolStripButton();
            this.propertiesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.resetButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsLabelLabel = new System.Windows.Forms.ToolStripLabel();
            this.settingsLabel = new System.Windows.Forms.ToolStripLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.titelLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.playerContextMenu.SuspendLayout();
            this.teamContextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.playerListView);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.teamListView);
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
            this.playerListView.Name = "playerListView";
            this.playerListView.ShowItemToolTips = true;
            this.playerListView.SmallImageList = this.imageList;
            this.playerListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.playerListView.UseCompatibleStateImageBehavior = false;
            this.playerListView.View = System.Windows.Forms.View.Tile;
            this.playerListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.drag_playerList);
            this.playerListView.SelectedIndexChanged += new System.EventHandler(this.select_playerList);
            this.playerListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragDrop_playerList);
            this.playerListView.DragOver += new System.Windows.Forms.DragEventHandler(this.dragOver_playerList);
            this.playerListView.DoubleClick += new System.EventHandler(this.button_newTeam);
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
            this.removeMenuItem,
            this.propertiesMenuItem,
            this.toolStripSeparator2,
            this.chooseMenuItem});
            this.playerContextMenu.Name = "contextMenuStrip1";
            this.playerContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.context_open);
            // 
            // createMenuItem
            // 
            resources.ApplyResources(this.createMenuItem, "createMenuItem");
            this.createMenuItem.Name = "createMenuItem";
            this.createMenuItem.Click += new System.EventHandler(this.button_create);
            // 
            // loadMenuItem
            // 
            resources.ApplyResources(this.loadMenuItem, "loadMenuItem");
            this.loadMenuItem.Name = "loadMenuItem";
            this.loadMenuItem.Click += new System.EventHandler(this.button_load);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // removeMenuItem
            // 
            resources.ApplyResources(this.removeMenuItem, "removeMenuItem");
            this.removeMenuItem.Name = "removeMenuItem";
            this.removeMenuItem.Click += new System.EventHandler(this.button_remove);
            // 
            // propertiesMenuItem
            // 
            resources.ApplyResources(this.propertiesMenuItem, "propertiesMenuItem");
            this.propertiesMenuItem.Name = "propertiesMenuItem";
            this.propertiesMenuItem.Click += new System.EventHandler(this.button_antProperties);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // chooseMenuItem
            // 
            resources.ApplyResources(this.chooseMenuItem, "chooseMenuItem");
            this.chooseMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTeamMenuItem,
            this.chooseTeam1MenuItem,
            this.chooseTeam2MenuItem,
            this.chooseTeam3MenuItem,
            this.chooseTeam4MenuItem,
            this.chooseTeam5MenuItem,
            this.chooseTeam6MenuItem,
            this.chooseTeam7MenuItem,
            this.chooseTeam8MenuItem});
            this.chooseMenuItem.Name = "chooseMenuItem";
            // 
            // newTeamMenuItem
            // 
            resources.ApplyResources(this.newTeamMenuItem, "newTeamMenuItem");
            this.newTeamMenuItem.Name = "newTeamMenuItem";
            this.newTeamMenuItem.Click += new System.EventHandler(this.button_newTeam);
            // 
            // chooseTeam1MenuItem
            // 
            resources.ApplyResources(this.chooseTeam1MenuItem, "chooseTeam1MenuItem");
            this.chooseTeam1MenuItem.Name = "chooseTeam1MenuItem";
            this.chooseTeam1MenuItem.Click += new System.EventHandler(this.button_newTeam1);
            // 
            // chooseTeam2MenuItem
            // 
            resources.ApplyResources(this.chooseTeam2MenuItem, "chooseTeam2MenuItem");
            this.chooseTeam2MenuItem.Name = "chooseTeam2MenuItem";
            this.chooseTeam2MenuItem.Click += new System.EventHandler(this.button_newTeam2);
            // 
            // chooseTeam3MenuItem
            // 
            resources.ApplyResources(this.chooseTeam3MenuItem, "chooseTeam3MenuItem");
            this.chooseTeam3MenuItem.Name = "chooseTeam3MenuItem";
            this.chooseTeam3MenuItem.Click += new System.EventHandler(this.button_newTeam3);
            // 
            // chooseTeam4MenuItem
            // 
            resources.ApplyResources(this.chooseTeam4MenuItem, "chooseTeam4MenuItem");
            this.chooseTeam4MenuItem.Name = "chooseTeam4MenuItem";
            this.chooseTeam4MenuItem.Click += new System.EventHandler(this.button_newTeam4);
            // 
            // chooseTeam5MenuItem
            // 
            resources.ApplyResources(this.chooseTeam5MenuItem, "chooseTeam5MenuItem");
            this.chooseTeam5MenuItem.Name = "chooseTeam5MenuItem";
            this.chooseTeam5MenuItem.Click += new System.EventHandler(this.button_newTeam5);
            // 
            // chooseTeam6MenuItem
            // 
            resources.ApplyResources(this.chooseTeam6MenuItem, "chooseTeam6MenuItem");
            this.chooseTeam6MenuItem.Name = "chooseTeam6MenuItem";
            this.chooseTeam6MenuItem.Click += new System.EventHandler(this.button_newTeam6);
            // 
            // chooseTeam7MenuItem
            // 
            resources.ApplyResources(this.chooseTeam7MenuItem, "chooseTeam7MenuItem");
            this.chooseTeam7MenuItem.Name = "chooseTeam7MenuItem";
            this.chooseTeam7MenuItem.Click += new System.EventHandler(this.button_newTeam7);
            // 
            // chooseTeam8MenuItem
            // 
            resources.ApplyResources(this.chooseTeam8MenuItem, "chooseTeam8MenuItem");
            this.chooseTeam8MenuItem.Name = "chooseTeam8MenuItem";
            this.chooseTeam8MenuItem.Click += new System.EventHandler(this.button_newTeam8);
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
            // teamListView
            // 
            resources.ApplyResources(this.teamListView, "teamListView");
            this.teamListView.AllowDrop = true;
            this.teamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playerColumn,
            this.author2Column});
            this.teamListView.ContextMenuStrip = this.teamContextMenu;
            this.teamListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups1"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups2"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups3"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups4"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups5"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups6"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("teamListView.Groups7")))});
            this.teamListView.LargeImageList = this.imageList;
            this.teamListView.Name = "teamListView";
            this.teamListView.ShowItemToolTips = true;
            this.teamListView.SmallImageList = this.imageList;
            this.teamListView.UseCompatibleStateImageBehavior = false;
            this.teamListView.View = System.Windows.Forms.View.Tile;
            this.teamListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.drag_teamList);
            this.teamListView.SelectedIndexChanged += new System.EventHandler(this.select_teamList);
            this.teamListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dragDrop_teamList);
            this.teamListView.DragOver += new System.Windows.Forms.DragEventHandler(this.dragOver_teamList);
            // 
            // playerColumn
            // 
            resources.ApplyResources(this.playerColumn, "playerColumn");
            // 
            // author2Column
            // 
            resources.ApplyResources(this.author2Column, "author2Column");
            // 
            // teamContextMenu
            // 
            resources.ApplyResources(this.teamContextMenu, "teamContextMenu");
            this.teamContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kickMenuItem,
            this.moveMenuItem});
            this.teamContextMenu.Name = "teamContextMenu";
            // 
            // kickMenuItem
            // 
            resources.ApplyResources(this.kickMenuItem, "kickMenuItem");
            this.kickMenuItem.Name = "kickMenuItem";
            this.kickMenuItem.Click += new System.EventHandler(this.button_removeFromTeam);
            // 
            // moveMenuItem
            // 
            resources.ApplyResources(this.moveMenuItem, "moveMenuItem");
            this.moveMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveNewTeamMenuItem,
            this.moveTeam1MenuItem,
            this.moveTeam2MenuItem,
            this.moveTeam3MenuItem,
            this.moveTeam4MenuItem,
            this.moveTeam5MenuItem,
            this.moveTeam6MenuItem,
            this.moveTeam7MenuItem,
            this.moveTeam8MenuItem});
            this.moveMenuItem.Name = "moveMenuItem";
            // 
            // moveNewTeamMenuItem
            // 
            resources.ApplyResources(this.moveNewTeamMenuItem, "moveNewTeamMenuItem");
            this.moveNewTeamMenuItem.Name = "moveNewTeamMenuItem";
            this.moveNewTeamMenuItem.Click += new System.EventHandler(this.button_moveNewTeam);
            // 
            // moveTeam1MenuItem
            // 
            resources.ApplyResources(this.moveTeam1MenuItem, "moveTeam1MenuItem");
            this.moveTeam1MenuItem.Name = "moveTeam1MenuItem";
            this.moveTeam1MenuItem.Click += new System.EventHandler(this.button_moveTeam1);
            // 
            // moveTeam2MenuItem
            // 
            resources.ApplyResources(this.moveTeam2MenuItem, "moveTeam2MenuItem");
            this.moveTeam2MenuItem.Name = "moveTeam2MenuItem";
            this.moveTeam2MenuItem.Click += new System.EventHandler(this.button_moveTeam2);
            // 
            // moveTeam3MenuItem
            // 
            resources.ApplyResources(this.moveTeam3MenuItem, "moveTeam3MenuItem");
            this.moveTeam3MenuItem.Name = "moveTeam3MenuItem";
            this.moveTeam3MenuItem.Click += new System.EventHandler(this.button_moveTeam3);
            // 
            // moveTeam4MenuItem
            // 
            resources.ApplyResources(this.moveTeam4MenuItem, "moveTeam4MenuItem");
            this.moveTeam4MenuItem.Name = "moveTeam4MenuItem";
            this.moveTeam4MenuItem.Click += new System.EventHandler(this.button_moveTeam4);
            // 
            // moveTeam5MenuItem
            // 
            resources.ApplyResources(this.moveTeam5MenuItem, "moveTeam5MenuItem");
            this.moveTeam5MenuItem.Name = "moveTeam5MenuItem";
            this.moveTeam5MenuItem.Click += new System.EventHandler(this.button_moveTeam5);
            // 
            // moveTeam6MenuItem
            // 
            resources.ApplyResources(this.moveTeam6MenuItem, "moveTeam6MenuItem");
            this.moveTeam6MenuItem.Name = "moveTeam6MenuItem";
            this.moveTeam6MenuItem.Click += new System.EventHandler(this.button_moveTeam6);
            // 
            // moveTeam7MenuItem
            // 
            resources.ApplyResources(this.moveTeam7MenuItem, "moveTeam7MenuItem");
            this.moveTeam7MenuItem.Name = "moveTeam7MenuItem";
            this.moveTeam7MenuItem.Click += new System.EventHandler(this.button_moveTeam7);
            // 
            // moveTeam8MenuItem
            // 
            resources.ApplyResources(this.moveTeam8MenuItem, "moveTeam8MenuItem");
            this.moveTeam8MenuItem.Name = "moveTeam8MenuItem";
            this.moveTeam8MenuItem.Click += new System.EventHandler(this.button_moveTeam8);
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createButton,
            this.loadButton,
            this.removeButton,
            this.propertiesButton,
            this.toolStripSeparator1,
            this.settingsButton,
            this.toolStripSeparator3,
            this.resetButton,
            this.toolStripSeparator4,
            this.settingsLabelLabel,
            this.settingsLabel});
            this.toolStrip.Name = "toolStrip";
            // 
            // createButton
            // 
            resources.ApplyResources(this.createButton, "createButton");
            this.createButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.add;
            this.createButton.Name = "createButton";
            this.createButton.Click += new System.EventHandler(this.button_create);
            // 
            // loadButton
            // 
            resources.ApplyResources(this.loadButton, "loadButton");
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.load_16x16;
            this.loadButton.Name = "loadButton";
            this.loadButton.Click += new System.EventHandler(this.button_load);
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.removeButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.delete_16x16;
            this.removeButton.Name = "removeButton";
            this.removeButton.Click += new System.EventHandler(this.button_remove);
            // 
            // propertiesButton
            // 
            resources.ApplyResources(this.propertiesButton, "propertiesButton");
            this.propertiesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.propertiesButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.properties_16x16;
            this.propertiesButton.Name = "propertiesButton";
            this.propertiesButton.Click += new System.EventHandler(this.button_antProperties);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // settingsButton
            // 
            resources.ApplyResources(this.settingsButton, "settingsButton");
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.settings_16x16;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Click += new System.EventHandler(this.button_settings);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // resetButton
            // 
            resources.ApplyResources(this.resetButton, "resetButton");
            this.resetButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.resetButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.resetteams;
            this.resetButton.Name = "resetButton";
            this.resetButton.Click += new System.EventHandler(this.button_reset);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // settingsLabelLabel
            // 
            resources.ApplyResources(this.settingsLabelLabel, "settingsLabelLabel");
            this.settingsLabelLabel.Name = "settingsLabelLabel";
            // 
            // settingsLabel
            // 
            resources.ApplyResources(this.settingsLabel, "settingsLabel");
            this.settingsLabel.Name = "settingsLabel";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "*.dll";
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            this.openFileDialog.Multiselect = true;
            // 
            // titelLabel
            // 
            resources.ApplyResources(this.titelLabel, "titelLabel");
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.Name = "titelLabel";
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // TeamSetup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.titelLabel);
            this.Name = "TeamSetup";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.playerContextMenu.ResumeLayout(false);
            this.teamContextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip playerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView playerListView;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader authorColumn;
        private System.Windows.Forms.ListView teamListView;
        private System.Windows.Forms.ColumnHeader playerColumn;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripButton removeButton;
        private System.Windows.Forms.ToolStripButton propertiesButton;
        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton settingsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem chooseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTeamMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam3MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam4MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam5MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam6MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam7MenuItem;
        private System.Windows.Forms.ToolStripMenuItem chooseTeam8MenuItem;
        private System.Windows.Forms.ContextMenuStrip teamContextMenu;
        private System.Windows.Forms.ToolStripMenuItem kickMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveNewTeamMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam3MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam4MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam5MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam6MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam7MenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveTeam8MenuItem;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton resetButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ColumnHeader author2Column;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel settingsLabelLabel;
        private System.Windows.Forms.ToolStripLabel settingsLabel;
        private System.Windows.Forms.ToolStripButton createButton;
        private System.Windows.Forms.ToolStripMenuItem createMenuItem;
    }
}