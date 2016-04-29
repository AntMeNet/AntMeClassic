namespace AntMe.Plugin.Simulation
{
    partial class ChallengeButton
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
            this.label1 = new System.Windows.Forms.Label();
            this.star3 = new System.Windows.Forms.PictureBox();
            this.star2 = new System.Windows.Forms.PictureBox();
            this.star1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.star3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sugar run!";
            // 
            // star3
            // 
            this.star3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.star3.Image = global::AntMe.Plugin.Simulation.Properties.Resources.star1;
            this.star3.Location = new System.Drawing.Point(209, 49);
            this.star3.Name = "star3";
            this.star3.Size = new System.Drawing.Size(32, 32);
            this.star3.TabIndex = 1;
            this.star3.TabStop = false;
            // 
            // star2
            // 
            this.star2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.star2.Image = global::AntMe.Plugin.Simulation.Properties.Resources.star1;
            this.star2.Location = new System.Drawing.Point(171, 49);
            this.star2.Name = "star2";
            this.star2.Size = new System.Drawing.Size(32, 32);
            this.star2.TabIndex = 2;
            this.star2.TabStop = false;
            // 
            // star1
            // 
            this.star1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.star1.Image = global::AntMe.Plugin.Simulation.Properties.Resources.star1;
            this.star1.Location = new System.Drawing.Point(133, 49);
            this.star1.Name = "star1";
            this.star1.Size = new System.Drawing.Size(32, 32);
            this.star1.TabIndex = 3;
            this.star1.TabStop = false;
            // 
            // ChallengeButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.star1);
            this.Controls.Add(this.star2);
            this.Controls.Add(this.star3);
            this.Controls.Add(this.label1);
            this.Name = "ChallengeButton";
            this.Size = new System.Drawing.Size(244, 84);
            ((System.ComponentModel.ISupportInitialize)(this.star3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox star3;
        private System.Windows.Forms.PictureBox star2;
        private System.Windows.Forms.PictureBox star1;
    }
}
