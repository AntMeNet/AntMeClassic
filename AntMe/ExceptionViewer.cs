using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace AntMe.Gui
{
    internal sealed partial class ExceptionViewer : Form
    {
        private readonly StringBuilder exceptionString = new StringBuilder();
        private readonly List<Exception> exceptions;

        /// <summary>
        /// Creates a new instance of ExceptionViewer with one single exception.
        /// </summary>
        /// <param name="exception">Single Exception</param>
        public ExceptionViewer(Exception exception)
        {
            exceptions = new List<Exception>(1);
            exceptions.Add(exception);
            fill();
        }

        /// <summary>
        /// Creates a new instance of ExceptionViewer with a list of exceptions.
        /// </summary>
        /// <param name="exceptions">List of Exceptions</param>
        public ExceptionViewer(ICollection<Exception> exceptions)
        {
            this.exceptions = new List<Exception>(exceptions.Count);
            this.exceptions.AddRange(exceptions);
            fill();
        }

        /// <summary>
        /// Fills the list with the exception-information.
        /// </summary>
        private void fill()
        {
            InitializeComponent();

            foreach (Exception ex in exceptions)
            {
                ListViewItem item = exceptionListView.Items.Add(ex.Message, "error");
                item.ToolTipText = ex.StackTrace;
                item.Tag = ex;

                pushString(ex, false);

                Exception inner = ex.InnerException;
                while (inner != null)
                {
                    ListViewItem sub = exceptionListView.Items.Add(" - " + inner.Message);
                    sub.ToolTipText = inner.StackTrace;
                    sub.Tag = inner;

                    pushString(inner, true);

                    inner = inner.InnerException;
                }
            }
        }

        private void pushString(Exception ex, bool inner)
        {
            exceptionString.Append((inner ? " -> " : "") + "=================" + Environment.NewLine);
            exceptionString.Append((inner ? " -> " : "") + "Message: " + ex.Message + Environment.NewLine);
            exceptionString.Append((inner ? " -> " : "") + "Date: " + DateTime.Now.ToString() + Environment.NewLine);
            exceptionString.Append((inner ? " -> " : "") + "Stack: " + Environment.NewLine + ex.StackTrace + Environment.NewLine + Environment.NewLine);
        }

        private void clipboardButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(exceptionString.ToString());
        }

        private void mailButton_Click(object sender, EventArgs e)
        {
            Process.Start("mailto:info@antme.net&subject=Bugreport&body=" +
                Uri.EscapeUriString(exceptionString.ToString()));
        }
    }
}