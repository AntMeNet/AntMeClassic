using System;
using System.Windows.Forms;

namespace AntMe.SharedComponents.Controls
{
    public partial class ShutterControl : UserControl
    {
        private ShutterState shutterState = ShutterState.Init;

        public ShutterControl()
        {
            InitializeComponent();

            ErrorMessage = "There was an error";
            InitMessage = "Not initialized yet";
        }

        public string ErrorMessage { get; set; }

        public string InitMessage { get; set; }

        public ShutterState ShutterState
        {
            get { return shutterState; }
            set
            {
                shutterState = value;
                SetShutterState(shutterState);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            loadingLabel.Top = (ClientSize.Height - loadingLabel.Height) / 2;
            loadingLabel.Left = (ClientSize.Width - loadingLabel.Width) / 2;
        }

        private void SetShutterState(ShutterState state)
        {
            switch (state)
            {
                case ShutterState.Init:
                    this.Visible = true;
                    loadingLabel.Text = InitMessage;
                    break;
                case ShutterState.Error:
                    this.Visible = true;
                    loadingLabel.Text = ErrorMessage;
                    break;
                case ShutterState.Loading:
                    this.Visible = true;
                    loadingLabel.Text = "Loading...";
                    break;
                case ShutterState.Open:
                    this.Visible = false;
                    break;
            }
        }
    }

    public enum ShutterState
    {
        Open,
        Error,
        Loading,
        Init
    }
}
