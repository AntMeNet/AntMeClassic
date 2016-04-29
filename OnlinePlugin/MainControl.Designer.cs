namespace AntMe.Plugin.Online
{
    partial class MainControl
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
            this.titelLabel = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // titelLabel
            // 
            this.titelLabel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.titelLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.titelLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titelLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.titelLabel.Location = new System.Drawing.Point(0, 0);
            this.titelLabel.Name = "titelLabel";
            this.titelLabel.Padding = new System.Windows.Forms.Padding(4);
            this.titelLabel.Size = new System.Drawing.Size(709, 27);
            this.titelLabel.TabIndex = 1;
            this.titelLabel.Text = "AntMe! Online";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 27);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(709, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.titelLabel);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(709, 403);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titelLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}
