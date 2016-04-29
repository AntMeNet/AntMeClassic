namespace AntMe.Plugin.Simulation
{
    partial class StatisticControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.loopsTreeView = new System.Windows.Forms.TreeView();
            this.treeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.summaryListView = new System.Windows.Forms.ListView();
            this.playerColumn = new System.Windows.Forms.ColumnHeader();
            this.collectedFoodColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns"));
            this.collectedFruitColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns1"));
            this.killedAntsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns2"));
            this.killedBugsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns3"));
            this.starvedAntsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns4"));
            this.beatenAntsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns5"));
            this.eatenAntsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns6"));
            this.pointsColumn = new System.Windows.Forms.ColumnHeader(resources.GetString("summaryListView.Columns7"));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.deleteButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.titelLabel = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.loopsTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.summaryListView);
            // 
            // loopsTreeView
            // 
            this.loopsTreeView.ContextMenuStrip = this.treeContext;
            resources.ApplyResources(this.loopsTreeView, "loopsTreeView");
            this.loopsTreeView.HideSelection = false;
            this.loopsTreeView.ImageList = this.imageList;
            this.loopsTreeView.Name = "loopsTreeView";
            this.loopsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_select);
            // 
            // treeContext
            // 
            this.treeContext.Name = "treeContext";
            resources.ApplyResources(this.treeContext, "treeContext");
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "loop_complete");
            this.imageList.Images.SetKeyName(1, "loop_incomplete");
            this.imageList.Images.SetKeyName(2, "loop_running");
            this.imageList.Images.SetKeyName(3, "simset_running");
            this.imageList.Images.SetKeyName(4, "simset_complete");
            this.imageList.Images.SetKeyName(5, "simset_incomplete");
            this.imageList.Images.SetKeyName(6, "beaten_ants");
            this.imageList.Images.SetKeyName(7, "collected_fruit");
            this.imageList.Images.SetKeyName(8, "collected_sugar");
            this.imageList.Images.SetKeyName(9, "colony");
            this.imageList.Images.SetKeyName(10, "eaten_ants");
            this.imageList.Images.SetKeyName(11, "killed_ants");
            this.imageList.Images.SetKeyName(12, "killed_bugs");
            this.imageList.Images.SetKeyName(13, "starved_ants");
            this.imageList.Images.SetKeyName(14, "total_points");
            // 
            // summaryListView
            // 
            this.summaryListView.AllowColumnReorder = true;
            this.summaryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playerColumn,
            this.collectedFoodColumn,
            this.collectedFruitColumn,
            this.killedAntsColumn,
            this.killedBugsColumn,
            this.starvedAntsColumn,
            this.beatenAntsColumn,
            this.eatenAntsColumn,
            this.pointsColumn});
            resources.ApplyResources(this.summaryListView, "summaryListView");
            this.summaryListView.GridLines = true;
            this.summaryListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups1"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups2"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups3"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups4"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups5"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups6"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("summaryListView.Groups7")))});
            this.summaryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.summaryListView.Name = "summaryListView";
            this.summaryListView.SmallImageList = this.imageList;
            this.summaryListView.UseCompatibleStateImageBehavior = false;
            this.summaryListView.View = System.Windows.Forms.View.Details;
            // 
            // playerColumn
            // 
            resources.ApplyResources(this.playerColumn, "playerColumn");
            // 
            // collectedFoodColumn
            // 
            resources.ApplyResources(this.collectedFoodColumn, "collectedFoodColumn");
            // 
            // collectedFruitColumn
            // 
            resources.ApplyResources(this.collectedFruitColumn, "collectedFruitColumn");
            // 
            // killedAntsColumn
            // 
            resources.ApplyResources(this.killedAntsColumn, "killedAntsColumn");
            // 
            // killedBugsColumn
            // 
            resources.ApplyResources(this.killedBugsColumn, "killedBugsColumn");
            // 
            // starvedAntsColumn
            // 
            resources.ApplyResources(this.starvedAntsColumn, "starvedAntsColumn");
            // 
            // beatenAntsColumn
            // 
            resources.ApplyResources(this.beatenAntsColumn, "beatenAntsColumn");
            // 
            // eatenAntsColumn
            // 
            resources.ApplyResources(this.eatenAntsColumn, "eatenAntsColumn");
            // 
            // pointsColumn
            // 
            resources.ApplyResources(this.pointsColumn, "pointsColumn");
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadButton,
            this.deleteButton,
            this.saveButton});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.load_16x16;
            resources.ApplyResources(this.loadButton, "loadButton");
            this.loadButton.Name = "loadButton";
            // 
            // deleteButton
            // 
            this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.delete_16x16;
            resources.ApplyResources(this.deleteButton, "deleteButton");
            this.deleteButton.Name = "deleteButton";
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = global::AntMe.Plugin.Simulation.Properties.Resources.save_16x16;
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            // 
            // titelLabel
            // 
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.titelLabel, "titelLabel");
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.Name = "titelLabel";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // StatisticControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.titelLabel);
            this.Name = "StatisticControl";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.TreeView loopsTreeView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView summaryListView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ColumnHeader playerColumn;
        private System.Windows.Forms.ColumnHeader collectedFoodColumn;
        private System.Windows.Forms.ColumnHeader collectedFruitColumn;
        private System.Windows.Forms.ColumnHeader killedAntsColumn;
        private System.Windows.Forms.ColumnHeader killedBugsColumn;
        private System.Windows.Forms.ColumnHeader starvedAntsColumn;
        private System.Windows.Forms.ColumnHeader beatenAntsColumn;
        private System.Windows.Forms.ColumnHeader eatenAntsColumn;
        private System.Windows.Forms.ColumnHeader pointsColumn;
        private System.Windows.Forms.ContextMenuStrip treeContext;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripButton loadButton;
        private System.Windows.Forms.ToolStripButton deleteButton;
        private System.Windows.Forms.ToolStripButton saveButton;
    }
}
