namespace AntMe.Gui {
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.programMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group1MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedMaxMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed100fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed80fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed50fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed30fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed22fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed15fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed8fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed2fpmMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forumMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classDescriptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group2MenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.infoBoxMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.stateLabelBarItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarItem = new System.Windows.Forms.ToolStripProgressBar();
            this.stepCounterBarItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsBarItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.startToolItem = new System.Windows.Forms.ToolStripButton();
            this.stopToolItem = new System.Windows.Forms.ToolStripButton();
            this.pauseToolItem = new System.Windows.Forms.ToolStripButton();
            this.group1ToolItem = new System.Windows.Forms.ToolStripSeparator();
            this.sourceLabelToolItem = new System.Windows.Forms.ToolStripLabel();
            this.producerButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.group2ToolItem = new System.Windows.Forms.ToolStripSeparator();
            this.speedLabelToolItem = new System.Windows.Forms.ToolStripLabel();
            this.speedDropDownToolItem = new System.Windows.Forms.ToolStripDropDownButton();
            this.speedMaxToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed100fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed80fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed50fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed30fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed22fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed15fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed8fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speed2fpmToolItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slowerToolItem = new System.Windows.Forms.ToolStripButton();
            this.fasterToolItem = new System.Windows.Forms.ToolStripButton();
            this.onlineButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.profileButton = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.loginButton = new System.Windows.Forms.ToolStripButton();
            this.versionButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.welcomeTab = new System.Windows.Forms.TabPage();
            this.infoWebBrowser = new AntMe.Gui.WebBrowserEx();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.welcomeTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programMenuItem,
            this.settingsMenuItem,
            this.helpMenuItem});
            this.menuStrip.Name = "menuStrip";
            // 
            // programMenuItem
            // 
            resources.ApplyResources(this.programMenuItem, "programMenuItem");
            this.programMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startMenuItem,
            this.stopMenuItem,
            this.pauseMenuItem,
            this.group1MenuItem,
            this.closeMenuItem});
            this.programMenuItem.Name = "programMenuItem";
            // 
            // startMenuItem
            // 
            resources.ApplyResources(this.startMenuItem, "startMenuItem");
            this.startMenuItem.Image = global::AntMe.Gui.Properties.Resources.play;
            this.startMenuItem.Name = "startMenuItem";
            this.startMenuItem.Click += new System.EventHandler(this.start);
            // 
            // stopMenuItem
            // 
            resources.ApplyResources(this.stopMenuItem, "stopMenuItem");
            this.stopMenuItem.Image = global::AntMe.Gui.Properties.Resources.stop;
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Click += new System.EventHandler(this.stop);
            // 
            // pauseMenuItem
            // 
            resources.ApplyResources(this.pauseMenuItem, "pauseMenuItem");
            this.pauseMenuItem.Image = global::AntMe.Gui.Properties.Resources.pause;
            this.pauseMenuItem.Name = "pauseMenuItem";
            this.pauseMenuItem.Click += new System.EventHandler(this.pause);
            // 
            // group1MenuItem
            // 
            resources.ApplyResources(this.group1MenuItem, "group1MenuItem");
            this.group1MenuItem.Name = "group1MenuItem";
            // 
            // closeMenuItem
            // 
            resources.ApplyResources(this.closeMenuItem, "closeMenuItem");
            this.closeMenuItem.Name = "closeMenuItem";
            this.closeMenuItem.Click += new System.EventHandler(this.button_close);
            // 
            // settingsMenuItem
            // 
            resources.ApplyResources(this.settingsMenuItem, "settingsMenuItem");
            this.settingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginSettingsMenuItem,
            this.speedMenuItem,
            this.languageMenuItem,
            this.updateMenuItem});
            this.settingsMenuItem.Name = "settingsMenuItem";
            // 
            // pluginSettingsMenuItem
            // 
            resources.ApplyResources(this.pluginSettingsMenuItem, "pluginSettingsMenuItem");
            this.pluginSettingsMenuItem.Name = "pluginSettingsMenuItem";
            this.pluginSettingsMenuItem.Click += new System.EventHandler(this.button_plugins);
            // 
            // speedMenuItem
            // 
            resources.ApplyResources(this.speedMenuItem, "speedMenuItem");
            this.speedMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedMaxMenuItem,
            this.speed100fpmMenuItem,
            this.speed80fpmMenuItem,
            this.speed50fpmMenuItem,
            this.speed30fpmMenuItem,
            this.speed22fpmMenuItem,
            this.speed15fpmMenuItem,
            this.speed8fpmMenuItem,
            this.speed2fpmMenuItem});
            this.speedMenuItem.Image = global::AntMe.Gui.Properties.Resources.speed;
            this.speedMenuItem.Name = "speedMenuItem";
            // 
            // speedMaxMenuItem
            // 
            resources.ApplyResources(this.speedMaxMenuItem, "speedMaxMenuItem");
            this.speedMaxMenuItem.Name = "speedMaxMenuItem";
            this.speedMaxMenuItem.Click += new System.EventHandler(this.button_limitSetToMax);
            // 
            // speed100fpmMenuItem
            // 
            resources.ApplyResources(this.speed100fpmMenuItem, "speed100fpmMenuItem");
            this.speed100fpmMenuItem.Name = "speed100fpmMenuItem";
            this.speed100fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo100);
            // 
            // speed80fpmMenuItem
            // 
            resources.ApplyResources(this.speed80fpmMenuItem, "speed80fpmMenuItem");
            this.speed80fpmMenuItem.Name = "speed80fpmMenuItem";
            this.speed80fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo80);
            // 
            // speed50fpmMenuItem
            // 
            resources.ApplyResources(this.speed50fpmMenuItem, "speed50fpmMenuItem");
            this.speed50fpmMenuItem.Name = "speed50fpmMenuItem";
            this.speed50fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo50);
            // 
            // speed30fpmMenuItem
            // 
            resources.ApplyResources(this.speed30fpmMenuItem, "speed30fpmMenuItem");
            this.speed30fpmMenuItem.Name = "speed30fpmMenuItem";
            this.speed30fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo30);
            // 
            // speed22fpmMenuItem
            // 
            resources.ApplyResources(this.speed22fpmMenuItem, "speed22fpmMenuItem");
            this.speed22fpmMenuItem.Name = "speed22fpmMenuItem";
            this.speed22fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo22);
            // 
            // speed15fpmMenuItem
            // 
            resources.ApplyResources(this.speed15fpmMenuItem, "speed15fpmMenuItem");
            this.speed15fpmMenuItem.Name = "speed15fpmMenuItem";
            this.speed15fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo15);
            // 
            // speed8fpmMenuItem
            // 
            resources.ApplyResources(this.speed8fpmMenuItem, "speed8fpmMenuItem");
            this.speed8fpmMenuItem.Name = "speed8fpmMenuItem";
            this.speed8fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo8);
            // 
            // speed2fpmMenuItem
            // 
            resources.ApplyResources(this.speed2fpmMenuItem, "speed2fpmMenuItem");
            this.speed2fpmMenuItem.Name = "speed2fpmMenuItem";
            this.speed2fpmMenuItem.Click += new System.EventHandler(this.button_limitSetTo2);
            // 
            // languageMenuItem
            // 
            resources.ApplyResources(this.languageMenuItem, "languageMenuItem");
            this.languageMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.germanMenuItem,
            this.englishMenuItem});
            this.languageMenuItem.Name = "languageMenuItem";
            // 
            // germanMenuItem
            // 
            resources.ApplyResources(this.germanMenuItem, "germanMenuItem");
            this.germanMenuItem.Name = "germanMenuItem";
            this.germanMenuItem.Click += new System.EventHandler(this.button_german);
            // 
            // englishMenuItem
            // 
            resources.ApplyResources(this.englishMenuItem, "englishMenuItem");
            this.englishMenuItem.Name = "englishMenuItem";
            this.englishMenuItem.Click += new System.EventHandler(this.button_english);
            // 
            // updateMenuItem
            // 
            resources.ApplyResources(this.updateMenuItem, "updateMenuItem");
            this.updateMenuItem.Name = "updateMenuItem";
            this.updateMenuItem.Click += new System.EventHandler(this.button_switchAutoupdate);
            // 
            // helpMenuItem
            // 
            resources.ApplyResources(this.helpMenuItem, "helpMenuItem");
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tutorialsMenuItem,
            this.forumMenuItem,
            this.websiteMenuItem,
            this.classDescriptionMenuItem,
            this.group2MenuItem,
            this.infoBoxMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            // 
            // tutorialsMenuItem
            // 
            resources.ApplyResources(this.tutorialsMenuItem, "tutorialsMenuItem");
            this.tutorialsMenuItem.Name = "tutorialsMenuItem";
            this.tutorialsMenuItem.Click += new System.EventHandler(this.button_tutorials);
            // 
            // forumMenuItem
            // 
            resources.ApplyResources(this.forumMenuItem, "forumMenuItem");
            this.forumMenuItem.Name = "forumMenuItem";
            this.forumMenuItem.Click += new System.EventHandler(this.button_forum);
            // 
            // websiteMenuItem
            // 
            resources.ApplyResources(this.websiteMenuItem, "websiteMenuItem");
            this.websiteMenuItem.Name = "websiteMenuItem";
            this.websiteMenuItem.Click += new System.EventHandler(this.button_website);
            // 
            // classDescriptionMenuItem
            // 
            resources.ApplyResources(this.classDescriptionMenuItem, "classDescriptionMenuItem");
            this.classDescriptionMenuItem.Name = "classDescriptionMenuItem";
            this.classDescriptionMenuItem.Click += new System.EventHandler(this.button_classDescription);
            // 
            // group2MenuItem
            // 
            resources.ApplyResources(this.group2MenuItem, "group2MenuItem");
            this.group2MenuItem.Name = "group2MenuItem";
            // 
            // infoBoxMenuItem
            // 
            resources.ApplyResources(this.infoBoxMenuItem, "infoBoxMenuItem");
            this.infoBoxMenuItem.Name = "infoBoxMenuItem";
            this.infoBoxMenuItem.Click += new System.EventHandler(this.button_info);
            // 
            // statusStrip
            // 
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stateLabelBarItem,
            this.progressBarItem,
            this.stepCounterBarItem,
            this.fpsBarItem});
            this.statusStrip.Name = "statusStrip";
            // 
            // stateLabelBarItem
            // 
            resources.ApplyResources(this.stateLabelBarItem, "stateLabelBarItem");
            this.stateLabelBarItem.Name = "stateLabelBarItem";
            this.stateLabelBarItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            // 
            // progressBarItem
            // 
            resources.ApplyResources(this.progressBarItem, "progressBarItem");
            this.progressBarItem.Name = "progressBarItem";
            this.progressBarItem.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // stepCounterBarItem
            // 
            resources.ApplyResources(this.stepCounterBarItem, "stepCounterBarItem");
            this.stepCounterBarItem.Name = "stepCounterBarItem";
            // 
            // fpsBarItem
            // 
            resources.ApplyResources(this.fpsBarItem, "fpsBarItem");
            this.fpsBarItem.Name = "fpsBarItem";
            this.fpsBarItem.Spring = true;
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolItem,
            this.stopToolItem,
            this.pauseToolItem,
            this.group1ToolItem,
            this.sourceLabelToolItem,
            this.producerButton,
            this.group2ToolItem,
            this.speedLabelToolItem,
            this.speedDropDownToolItem,
            this.slowerToolItem,
            this.fasterToolItem,
            this.onlineButton,
            this.loginButton,
            this.versionButton});
            this.toolStrip.Name = "toolStrip";
            // 
            // startToolItem
            // 
            resources.ApplyResources(this.startToolItem, "startToolItem");
            this.startToolItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.startToolItem.Image = global::AntMe.Gui.Properties.Resources.play;
            this.startToolItem.Name = "startToolItem";
            this.startToolItem.Click += new System.EventHandler(this.start);
            // 
            // stopToolItem
            // 
            resources.ApplyResources(this.stopToolItem, "stopToolItem");
            this.stopToolItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopToolItem.Image = global::AntMe.Gui.Properties.Resources.stop;
            this.stopToolItem.Name = "stopToolItem";
            this.stopToolItem.Click += new System.EventHandler(this.stop);
            // 
            // pauseToolItem
            // 
            resources.ApplyResources(this.pauseToolItem, "pauseToolItem");
            this.pauseToolItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pauseToolItem.Image = global::AntMe.Gui.Properties.Resources.pause;
            this.pauseToolItem.Name = "pauseToolItem";
            this.pauseToolItem.Click += new System.EventHandler(this.pause);
            // 
            // group1ToolItem
            // 
            resources.ApplyResources(this.group1ToolItem, "group1ToolItem");
            this.group1ToolItem.Name = "group1ToolItem";
            // 
            // sourceLabelToolItem
            // 
            resources.ApplyResources(this.sourceLabelToolItem, "sourceLabelToolItem");
            this.sourceLabelToolItem.Name = "sourceLabelToolItem";
            // 
            // producerButton
            // 
            resources.ApplyResources(this.producerButton, "producerButton");
            this.producerButton.AutoToolTip = false;
            this.producerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.producerButton.Name = "producerButton";
            // 
            // group2ToolItem
            // 
            resources.ApplyResources(this.group2ToolItem, "group2ToolItem");
            this.group2ToolItem.Name = "group2ToolItem";
            // 
            // speedLabelToolItem
            // 
            resources.ApplyResources(this.speedLabelToolItem, "speedLabelToolItem");
            this.speedLabelToolItem.Name = "speedLabelToolItem";
            // 
            // speedDropDownToolItem
            // 
            resources.ApplyResources(this.speedDropDownToolItem, "speedDropDownToolItem");
            this.speedDropDownToolItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speedMaxToolItem,
            this.speed100fpmToolItem,
            this.speed80fpmToolItem,
            this.speed50fpmToolItem,
            this.speed30fpmToolItem,
            this.speed22fpmToolItem,
            this.speed15fpmToolItem,
            this.speed8fpmToolItem,
            this.speed2fpmToolItem});
            this.speedDropDownToolItem.Image = global::AntMe.Gui.Properties.Resources.speed;
            this.speedDropDownToolItem.Name = "speedDropDownToolItem";
            // 
            // speedMaxToolItem
            // 
            resources.ApplyResources(this.speedMaxToolItem, "speedMaxToolItem");
            this.speedMaxToolItem.Name = "speedMaxToolItem";
            this.speedMaxToolItem.Click += new System.EventHandler(this.button_limitSetToMax);
            // 
            // speed100fpmToolItem
            // 
            resources.ApplyResources(this.speed100fpmToolItem, "speed100fpmToolItem");
            this.speed100fpmToolItem.Name = "speed100fpmToolItem";
            this.speed100fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo100);
            // 
            // speed80fpmToolItem
            // 
            resources.ApplyResources(this.speed80fpmToolItem, "speed80fpmToolItem");
            this.speed80fpmToolItem.Name = "speed80fpmToolItem";
            this.speed80fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo80);
            // 
            // speed50fpmToolItem
            // 
            resources.ApplyResources(this.speed50fpmToolItem, "speed50fpmToolItem");
            this.speed50fpmToolItem.Name = "speed50fpmToolItem";
            this.speed50fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo50);
            // 
            // speed30fpmToolItem
            // 
            resources.ApplyResources(this.speed30fpmToolItem, "speed30fpmToolItem");
            this.speed30fpmToolItem.Name = "speed30fpmToolItem";
            this.speed30fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo30);
            // 
            // speed22fpmToolItem
            // 
            resources.ApplyResources(this.speed22fpmToolItem, "speed22fpmToolItem");
            this.speed22fpmToolItem.Name = "speed22fpmToolItem";
            this.speed22fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo22);
            // 
            // speed15fpmToolItem
            // 
            resources.ApplyResources(this.speed15fpmToolItem, "speed15fpmToolItem");
            this.speed15fpmToolItem.Name = "speed15fpmToolItem";
            this.speed15fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo15);
            // 
            // speed8fpmToolItem
            // 
            resources.ApplyResources(this.speed8fpmToolItem, "speed8fpmToolItem");
            this.speed8fpmToolItem.Name = "speed8fpmToolItem";
            this.speed8fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo8);
            // 
            // speed2fpmToolItem
            // 
            resources.ApplyResources(this.speed2fpmToolItem, "speed2fpmToolItem");
            this.speed2fpmToolItem.Name = "speed2fpmToolItem";
            this.speed2fpmToolItem.Click += new System.EventHandler(this.button_limitSetTo2);
            // 
            // slowerToolItem
            // 
            resources.ApplyResources(this.slowerToolItem, "slowerToolItem");
            this.slowerToolItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.slowerToolItem.Image = global::AntMe.Gui.Properties.Resources.downarrow;
            this.slowerToolItem.Name = "slowerToolItem";
            this.slowerToolItem.Click += new System.EventHandler(this.button_limitSlower);
            // 
            // fasterToolItem
            // 
            resources.ApplyResources(this.fasterToolItem, "fasterToolItem");
            this.fasterToolItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fasterToolItem.Image = global::AntMe.Gui.Properties.Resources.uparrow;
            this.fasterToolItem.Name = "fasterToolItem";
            this.fasterToolItem.Click += new System.EventHandler(this.button_limitFaster);
            // 
            // onlineButton
            // 
            resources.ApplyResources(this.onlineButton, "onlineButton");
            this.onlineButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.onlineButton.AutoToolTip = false;
            this.onlineButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileButton,
            this.logoutButton});
            this.onlineButton.Image = global::AntMe.Gui.Properties.Resources.online;
            this.onlineButton.Name = "onlineButton";
            // 
            // profileButton
            // 
            resources.ApplyResources(this.profileButton, "profileButton");
            this.profileButton.Name = "profileButton";
            this.profileButton.Click += new System.EventHandler(this.profileButton_Click);
            // 
            // logoutButton
            // 
            resources.ApplyResources(this.logoutButton, "logoutButton");
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // loginButton
            // 
            resources.ApplyResources(this.loginButton, "loginButton");
            this.loginButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.loginButton.Image = global::AntMe.Gui.Properties.Resources.offline;
            this.loginButton.Name = "loginButton";
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // versionButton
            // 
            resources.ApplyResources(this.versionButton, "versionButton");
            this.versionButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.versionButton.AutoToolTip = false;
            this.versionButton.Image = global::AntMe.Gui.Properties.Resources.warning;
            this.versionButton.Name = "versionButton";
            this.versionButton.Click += new System.EventHandler(this.versionButton_Click);
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Controls.Add(this.welcomeTab);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tab_select);
            // 
            // welcomeTab
            // 
            resources.ApplyResources(this.welcomeTab, "welcomeTab");
            this.welcomeTab.Controls.Add(this.infoWebBrowser);
            this.welcomeTab.Name = "welcomeTab";
            this.welcomeTab.UseVisualStyleBackColor = true;
            // 
            // infoWebBrowser
            // 
            resources.ApplyResources(this.infoWebBrowser, "infoWebBrowser");
            this.infoWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.infoWebBrowser.Name = "infoWebBrowser";
            this.infoWebBrowser.ScriptErrorsSuppressed = true;
            this.infoWebBrowser.WebBrowserShortcutsEnabled = false;
            this.infoWebBrowser.NavigateError += new AntMe.Gui.WebBrowserNavigateErrorEventHandler(this.infoWebBrowser_NavigateError);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_tick);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_close);
            this.Shown += new System.EventHandler(this.form_shown);
            this.Move += new System.EventHandler(this.form_resize);
            this.Resize += new System.EventHandler(this.form_resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.welcomeTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripMenuItem programMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripButton startToolItem;
        private System.Windows.Forms.ToolStripButton stopToolItem;
        private System.Windows.Forms.ToolStripButton pauseToolItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage welcomeTab;
        private System.Windows.Forms.ToolStripMenuItem startMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseMenuItem;
        private System.Windows.Forms.ToolStripSeparator group1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tutorialsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forumMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classDescriptionMenuItem;
        private System.Windows.Forms.ToolStripSeparator group2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoBoxMenuItem;
        private WebBrowserEx infoWebBrowser;
        private System.Windows.Forms.ToolStripLabel sourceLabelToolItem;
        private System.Windows.Forms.ToolStripSeparator group1ToolItem;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripStatusLabel stateLabelBarItem;
        private System.Windows.Forms.ToolStripProgressBar progressBarItem;
        private System.Windows.Forms.ToolStripStatusLabel stepCounterBarItem;
        private System.Windows.Forms.ToolStripStatusLabel fpsBarItem;
        private System.Windows.Forms.ToolStripSeparator group2ToolItem;
        private System.Windows.Forms.ToolStripMenuItem speedMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton speedDropDownToolItem;
        private System.Windows.Forms.ToolStripMenuItem speedMaxToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed100fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed15fpmToolItem;
        private System.Windows.Forms.ToolStripButton slowerToolItem;
        private System.Windows.Forms.ToolStripButton fasterToolItem;
        private System.Windows.Forms.ToolStripMenuItem speedMaxMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed100fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed80fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed50fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed80fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed50fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed30fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed22fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed8fpmToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed2fpmToolItem;
        private System.Windows.Forms.ToolStripLabel speedLabelToolItem;
        private System.Windows.Forms.ToolStripMenuItem speed30fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed22fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed15fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed8fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speed2fpmMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginSettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem languageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem germanMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton onlineButton;
        private System.Windows.Forms.ToolStripMenuItem profileButton;
        private System.Windows.Forms.ToolStripMenuItem logoutButton;
        private System.Windows.Forms.ToolStripButton loginButton;
        private System.Windows.Forms.ToolStripButton versionButton;
        private System.Windows.Forms.ToolStripDropDownButton producerButton;
    }
}