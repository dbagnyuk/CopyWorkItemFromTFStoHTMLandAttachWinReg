using System;
using System.Windows.Forms;

namespace CopyWorkItemFromTFStoHTMLandAttachWinReg
{
    public partial class browserForm : Form
    {
        public browserForm()
        {
            InitializeComponent();
        }
        private void browserForm_Load(object sender, EventArgs e)
        {
            webBrowser.DocumentText = TFStoHTML.temp;
            this.Text = TFStoHTML.workItem.Type.Name + " " + TFStoHTML.workItem.Id;
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
