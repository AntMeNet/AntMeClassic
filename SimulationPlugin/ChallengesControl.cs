using System;
using System.Windows.Forms;

namespace AntMe.Plugin.Simulation
{
    public partial class ChallengesControl : UserControl
    {
        public ChallengesControl()
        {
            InitializeComponent();
        }

        private void ChallengesControl_Paint(object sender, PaintEventArgs e)
        {
            //using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.ClientRectangle.Height), Color.LightBlue, Color.CornflowerBlue))
            //{
            //    e.Graphics.FillRectangle(brush, e.ClipRectangle);
            //}
        }

        private void ChallengesControl_Resize(object sender, EventArgs e)
        {
            mainPanel.Left = (ClientRectangle.Width - mainPanel.Width) / 2;
            mainPanel.Top = (ClientRectangle.Height - mainPanel.Height) / 2;
        }
    }
}
