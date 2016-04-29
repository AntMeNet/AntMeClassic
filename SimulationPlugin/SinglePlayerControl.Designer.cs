namespace AntMe.Plugin.Simulation
{
    partial class SinglePlayerControl
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
            this.titelLabel = new System.Windows.Forms.Label();
            this.playerPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.removeLabel = new System.Windows.Forms.LinkLabel();
            this.propertyLabel = new System.Windows.Forms.LinkLabel();
            this.authorlabel = new System.Windows.Forms.Label();
            this.namelabel = new System.Windows.Forms.Label();
            this.leaderboardListView = new System.Windows.Forms.ListView();
            this.rankColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playerColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pointsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.leaderboardTitle = new System.Windows.Forms.Panel();
            this.staticLabel = new System.Windows.Forms.Label();
            this.orderComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.achievementsTitle = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.achievementPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.totalPagesLabel = new System.Windows.Forms.Label();
            this.pageTextBox = new System.Windows.Forms.NumericUpDown();
            this.homeButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.achievementsLoading = new AntMe.SharedComponents.Controls.ShutterControl();
            this.leaderboardLoading = new AntMe.SharedComponents.Controls.ShutterControl();
            this.playerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.leaderboardTitle.SuspendLayout();
            this.achievementsTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titelLabel
            // 
            this.titelLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(0)))), ((int)(((byte)(34)))));
            this.titelLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titelLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.titelLabel.Location = new System.Drawing.Point(0, 0);
            this.titelLabel.Name = "titelLabel";
            this.titelLabel.Padding = new System.Windows.Forms.Padding(20, 10, 0, 0);
            this.titelLabel.Size = new System.Drawing.Size(820, 35);
            this.titelLabel.TabIndex = 3;
            this.titelLabel.Text = "SINGLE PLAYER MODE (BETA)";
            // 
            // playerPanel
            // 
            this.playerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.playerPanel.Controls.Add(this.pictureBox1);
            this.playerPanel.Controls.Add(this.removeLabel);
            this.playerPanel.Controls.Add(this.propertyLabel);
            this.playerPanel.Controls.Add(this.authorlabel);
            this.playerPanel.Controls.Add(this.namelabel);
            this.playerPanel.Location = new System.Drawing.Point(0, 35);
            this.playerPanel.Name = "playerPanel";
            this.playerPanel.Size = new System.Drawing.Size(820, 110);
            this.playerPanel.TabIndex = 4;
            this.playerPanel.Click += new System.EventHandler(this.playerPanel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AntMe.Plugin.Simulation.Properties.Resources.Ai;
            this.pictureBox1.Location = new System.Drawing.Point(22, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(82, 82);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.playerPanel_Click);
            // 
            // removeLabel
            // 
            this.removeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeLabel.AutoSize = true;
            this.removeLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.removeLabel.Location = new System.Drawing.Point(756, 80);
            this.removeLabel.Name = "removeLabel";
            this.removeLabel.Padding = new System.Windows.Forms.Padding(5);
            this.removeLabel.Size = new System.Drawing.Size(57, 23);
            this.removeLabel.TabIndex = 4;
            this.removeLabel.TabStop = true;
            this.removeLabel.Text = "Remove";
            this.removeLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.removeLabel_LinkClicked);
            // 
            // propertyLabel
            // 
            this.propertyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyLabel.AutoSize = true;
            this.propertyLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.propertyLabel.Location = new System.Drawing.Point(687, 80);
            this.propertyLabel.Name = "propertyLabel";
            this.propertyLabel.Padding = new System.Windows.Forms.Padding(5);
            this.propertyLabel.Size = new System.Drawing.Size(64, 23);
            this.propertyLabel.TabIndex = 3;
            this.propertyLabel.TabStop = true;
            this.propertyLabel.Text = "Properties";
            this.propertyLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.propertyLabel_LinkClicked);
            // 
            // authorlabel
            // 
            this.authorlabel.AutoSize = true;
            this.authorlabel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authorlabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.authorlabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.authorlabel.Location = new System.Drawing.Point(112, 48);
            this.authorlabel.Name = "authorlabel";
            this.authorlabel.Size = new System.Drawing.Size(53, 20);
            this.authorlabel.TabIndex = 2;
            this.authorlabel.Text = "[author]";
            this.authorlabel.Click += new System.EventHandler(this.playerPanel_Click);
            // 
            // namelabel
            // 
            this.namelabel.AutoSize = true;
            this.namelabel.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namelabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.namelabel.Location = new System.Drawing.Point(110, 13);
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(106, 32);
            this.namelabel.TabIndex = 1;
            this.namelabel.Text = "[name]";
            this.namelabel.Click += new System.EventHandler(this.playerPanel_Click);
            // 
            // leaderboardListView
            // 
            this.leaderboardListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leaderboardListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.leaderboardListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.rankColumn,
            this.playerColumn,
            this.pointsColumn});
            this.leaderboardListView.Location = new System.Drawing.Point(421, 220);
            this.leaderboardListView.Name = "leaderboardListView";
            this.leaderboardListView.Size = new System.Drawing.Size(382, 210);
            this.leaderboardListView.TabIndex = 5;
            this.leaderboardListView.UseCompatibleStateImageBehavior = false;
            this.leaderboardListView.View = System.Windows.Forms.View.Details;
            // 
            // rankColumn
            // 
            this.rankColumn.Text = "Rank";
            // 
            // playerColumn
            // 
            this.playerColumn.Text = "Player";
            this.playerColumn.Width = 245;
            // 
            // pointsColumn
            // 
            this.pointsColumn.Text = "Points";
            // 
            // leaderboardTitle
            // 
            this.leaderboardTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.leaderboardTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(0)))), ((int)(((byte)(34)))));
            this.leaderboardTitle.Controls.Add(this.staticLabel);
            this.leaderboardTitle.Controls.Add(this.orderComboBox);
            this.leaderboardTitle.Controls.Add(this.label1);
            this.leaderboardTitle.Location = new System.Drawing.Point(421, 162);
            this.leaderboardTitle.Name = "leaderboardTitle";
            this.leaderboardTitle.Size = new System.Drawing.Size(382, 58);
            this.leaderboardTitle.TabIndex = 6;
            // 
            // staticLabel
            // 
            this.staticLabel.AutoSize = true;
            this.staticLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.staticLabel.ForeColor = System.Drawing.Color.White;
            this.staticLabel.Location = new System.Drawing.Point(150, 16);
            this.staticLabel.Name = "staticLabel";
            this.staticLabel.Size = new System.Drawing.Size(53, 17);
            this.staticLabel.TabIndex = 2;
            this.staticLabel.Text = "(static)";
            // 
            // orderComboBox
            // 
            this.orderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.orderComboBox.FormattingEnabled = true;
            this.orderComboBox.Location = new System.Drawing.Point(250, 16);
            this.orderComboBox.Name = "orderComboBox";
            this.orderComboBox.Size = new System.Drawing.Size(121, 21);
            this.orderComboBox.TabIndex = 1;
            this.orderComboBox.SelectedIndexChanged += new System.EventHandler(this.orderComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "LEADERBOARD";
            // 
            // achievementsTitle
            // 
            this.achievementsTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.achievementsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(0)))), ((int)(((byte)(34)))));
            this.achievementsTitle.Controls.Add(this.button1);
            this.achievementsTitle.Controls.Add(this.label2);
            this.achievementsTitle.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.achievementsTitle.Location = new System.Drawing.Point(22, 162);
            this.achievementsTitle.Name = "achievementsTitle";
            this.achievementsTitle.Size = new System.Drawing.Size(392, 58);
            this.achievementsTitle.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(303, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(23, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "ACHIEVEMENTS";
            // 
            // achievementPanel
            // 
            this.achievementPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.achievementPanel.AutoScroll = true;
            this.achievementPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.achievementPanel.Location = new System.Drawing.Point(22, 220);
            this.achievementPanel.Name = "achievementPanel";
            this.achievementPanel.Size = new System.Drawing.Size(392, 238);
            this.achievementPanel.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.totalPagesLabel);
            this.panel1.Controls.Add(this.pageTextBox);
            this.panel1.Controls.Add(this.homeButton);
            this.panel1.Controls.Add(this.nextButton);
            this.panel1.Controls.Add(this.backButton);
            this.panel1.Location = new System.Drawing.Point(421, 430);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 28);
            this.panel1.TabIndex = 9;
            // 
            // totalPagesLabel
            // 
            this.totalPagesLabel.AutoSize = true;
            this.totalPagesLabel.Location = new System.Drawing.Point(236, 7);
            this.totalPagesLabel.Name = "totalPagesLabel";
            this.totalPagesLabel.Size = new System.Drawing.Size(22, 13);
            this.totalPagesLabel.TabIndex = 4;
            this.totalPagesLabel.Text = "[...]";
            // 
            // pageTextBox
            // 
            this.pageTextBox.Location = new System.Drawing.Point(178, 5);
            this.pageTextBox.Name = "pageTextBox";
            this.pageTextBox.Size = new System.Drawing.Size(51, 20);
            this.pageTextBox.TabIndex = 3;
            // 
            // homeButton
            // 
            this.homeButton.Location = new System.Drawing.Point(101, 3);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(71, 23);
            this.homeButton.TabIndex = 2;
            this.homeButton.Text = "home";
            this.homeButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(342, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(37, 23);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = ">>";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(3, 3);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(37, 23);
            this.backButton.TabIndex = 0;
            this.backButton.Text = "<<";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // achievementsLoading
            // 
            this.achievementsLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.achievementsLoading.BackColor = System.Drawing.Color.White;
            this.achievementsLoading.ErrorMessage = null;
            this.achievementsLoading.InitMessage = "Not initialized yet";
            this.achievementsLoading.Location = new System.Drawing.Point(22, 219);
            this.achievementsLoading.Name = "achievementsLoading";
            this.achievementsLoading.ShutterState = AntMe.SharedComponents.Controls.ShutterState.Init;
            this.achievementsLoading.Size = new System.Drawing.Size(392, 239);
            this.achievementsLoading.TabIndex = 0;
            // 
            // leaderboardLoading
            // 
            this.leaderboardLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leaderboardLoading.BackColor = System.Drawing.Color.White;
            this.leaderboardLoading.ErrorMessage = null;
            this.leaderboardLoading.InitMessage = "Not initialized yet";
            this.leaderboardLoading.Location = new System.Drawing.Point(421, 219);
            this.leaderboardLoading.Name = "leaderboardLoading";
            this.leaderboardLoading.ShutterState = AntMe.SharedComponents.Controls.ShutterState.Init;
            this.leaderboardLoading.Size = new System.Drawing.Size(382, 239);
            this.leaderboardLoading.TabIndex = 10;
            // 
            // SinglePlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(139)))), ((int)(((byte)(43)))));
            this.Controls.Add(this.leaderboardLoading);
            this.Controls.Add(this.achievementsLoading);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.achievementPanel);
            this.Controls.Add(this.achievementsTitle);
            this.Controls.Add(this.leaderboardTitle);
            this.Controls.Add(this.leaderboardListView);
            this.Controls.Add(this.playerPanel);
            this.Controls.Add(this.titelLabel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SinglePlayerControl";
            this.Size = new System.Drawing.Size(820, 475);
            this.playerPanel.ResumeLayout(false);
            this.playerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.leaderboardTitle.ResumeLayout(false);
            this.leaderboardTitle.PerformLayout();
            this.achievementsTitle.ResumeLayout(false);
            this.achievementsTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageTextBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.Panel playerPanel;
        private System.Windows.Forms.Label authorlabel;
        private System.Windows.Forms.Label namelabel;
        private System.Windows.Forms.ListView leaderboardListView;
        private System.Windows.Forms.ColumnHeader rankColumn;
        private System.Windows.Forms.ColumnHeader playerColumn;
        private System.Windows.Forms.ColumnHeader pointsColumn;
        private System.Windows.Forms.Panel leaderboardTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox orderComboBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel removeLabel;
        private System.Windows.Forms.LinkLabel propertyLabel;
        private System.Windows.Forms.Panel achievementsTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel achievementPanel;
        private System.Windows.Forms.Label staticLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label totalPagesLabel;
        private System.Windows.Forms.NumericUpDown pageTextBox;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer;
        private SharedComponents.Controls.ShutterControl achievementsLoading;
        private SharedComponents.Controls.ShutterControl leaderboardLoading;
    }
}
