using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AntMe.Online.Client;

namespace AntMe.Plugin.Simulation
{
    public partial class AchievementControl : UserControl
    {
        public AchievementControl()
        {
            InitializeComponent();
        }

        public void SetAchievement(Achievement achievement)
        {
            string root = "https://antmeservice.blob.core.windows.net/achievements/{0}_{1}_small.png";

            titleLabel.Text = achievement.Title;
            valueLabel.Text = achievement.Value.ToString();
            descriptionLabel.Text = achievement.Description;

            if (achievement.Achieved.HasValue)
            {
                pictureBox.ImageLocation = string.Format(root, achievement.PictureId, "enabled");
            }
            else
            {
                pictureBox.ImageLocation = string.Format(root, achievement.PictureId, "disabled");
            }
            
        }
    }
}
