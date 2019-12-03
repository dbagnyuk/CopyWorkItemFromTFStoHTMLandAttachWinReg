using System;
using System.Windows.Forms;
using mshtml;

namespace CopyWorkItemFromTFStoHTMLandAttachWinReg
{
    public partial class browserForm : Form
    {
        public int tempId;

        public browserForm()
        {
            InitializeComponent();
        }

        private void browserForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            tempId = TFStoHTML.workItem.Id;
            this.Text = TFStoHTML.workItem.Type.Name + " " + TFStoHTML.workItem.Id;
            webBrowser.DocumentText = TFStoHTML.temp;
        }

        private void webBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.IsInputKey = true;
                refreshStripMenuItem.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.C)
            {
                IHTMLDocument2 htmlDocument = webBrowser.Document.DomDocument as IHTMLDocument2;
                //Get selection object
                IHTMLSelectionObject currentSelection = htmlDocument.selection;
                if (currentSelection != null)
                {
                    IHTMLTxtRange range = currentSelection.createRange() as IHTMLTxtRange;
                    if (range != null && range.text != null)
                    {
                        string selectionText = range.text;
                        //Put text in Clipboard
                        Clipboard.SetText(selectionText);
                    }
                }
            }

            if (e.Control == true && e.KeyCode == Keys.A)
            {
                e.IsInputKey = true;
                copyAllStripMenuItem.PerformClick();
            }
        }

        private void refreshStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Enabled = false;
            Program.itemId = tempId;

            TFStoHTML.connectToTFS();
            TFStoHTML.readTFStoHTML();

            webBrowser.Navigate("about:blank");
            webBrowser.Document.OpenNew(false);
            webBrowser.Document.Write(TFStoHTML.temp);
            webBrowser.Refresh();

            TFStoHTML.temp = null;
            this.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void copyAllStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            webBrowser.Document.ExecCommand("SelectAll", true, null);
            webBrowser.Document.ExecCommand("Copy", true, null);
            //MessageBox.Show("Copied into Clipboard", "Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            webBrowser.Document.ExecCommand("Unselect", true, null);
            Cursor.Current = Cursors.Default;
        }
    }
}
