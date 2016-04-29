using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AntMe.Gui {
    internal sealed partial class ExceptionViewer : Form {
        private readonly List<Exception> exceptions;

        /// <summary>
        /// Creates a new instance of ExceptionViewer with one single exception.
        /// </summary>
        /// <param name="exception">Single Exception</param>
        public ExceptionViewer(Exception exception) {
            exceptions = new List<Exception>(1);
            exceptions.Add(exception);
            fill();
        }

        /// <summary>
        /// Creates a new instance of ExceptionViewer with a list of exceptions.
        /// </summary>
        /// <param name="exceptions">List of Exceptions</param>
        public ExceptionViewer(ICollection<Exception> exceptions) {
            this.exceptions = new List<Exception>(exceptions.Count);
            this.exceptions.AddRange(exceptions);
            fill();
        }

        /// <summary>
        /// Fills the list with the exception-information.
        /// </summary>
        private void fill() {
            InitializeComponent();

            foreach (Exception ex in exceptions) {
                ListViewItem item = exceptionListView.Items.Add(ex.Message, "error");
                item.ToolTipText = ex.StackTrace;
                item.Tag = ex;

                Exception inner = ex.InnerException;
                while (inner != null) {
                    ListViewItem sub = exceptionListView.Items.Add(" - " + inner.Message);
                    sub.ToolTipText = inner.StackTrace;
                    sub.Tag = inner;

                    inner = inner.InnerException;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            // TODO: Need a good dump-method - XML is not the right way... :(
            //XmlSerializer serializer = new XmlSerializer(exceptions.GetType());
            //MemoryStream temp = new MemoryStream();
            //serializer.Serialize(temp, exceptions);

            //Clipboard.SetText(new StreamReader(temp).ReadToEnd());
        }
    }
}