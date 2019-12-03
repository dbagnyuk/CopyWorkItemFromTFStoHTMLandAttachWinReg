using System;
using System.Windows.Forms;

namespace CopyWorkItemFromTFStoHTMLandAttachWinReg
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            if (!Config.readConfigFile())
            {
                MessageBox.Show("You need to create/edit config first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                configToolStripMenuItem_Click((object)sender, (EventArgs)e);
            }

            // Creating and setting the 
            // properties of the ToolTip 
            ToolTip t_Tip = new ToolTip();
            t_Tip.Active = true;
            t_Tip.AutoPopDelay = 4000;
            t_Tip.InitialDelay = 500;
            t_Tip.IsBalloon = true;
            t_Tip.ToolTipIcon = ToolTipIcon.Info;
            t_Tip.SetToolTip(textBoxTFSid, "TFS id contain only digits!");
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(textBoxTFSid.Text, out Program.itemId))
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Enabled = false;
                TFStoHTML.connectToTFS();

                if (checkBoxWindow.Checked || checkBoxFile.Checked)
                    TFStoHTML.readTFStoHTML();

                if (checkBoxFile.Checked)
                    TFStoHTML.writeTFStoHTML();

                if (checkBoxWindow.Checked)
                {
                    browserForm wbrDisplay = new browserForm();
                    wbrDisplay.Show();
                }

                // download the attachments from tfs item in new thread 
                if (checkBoxAttach.Checked)
                {
                    new System.Threading.Thread(delegate () {
                        if (!TFStoHTML.downloadAttach())
                            MessageBox.Show("For some reason, attachments cannot be downloaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }).Start();
                }

                //// download the attachments from tfs item
                //if (checkBoxAttach.Checked)
                //    if (!TFStoHTML.downloadAttach())
                //        MessageBox.Show("For some reason, attachments cannot be downloaded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                TFStoHTML.temp = null;

                this.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            else
            {
                textBoxTFSid.Clear();
                textBoxTFSid.Text = "Wrong input!";
            }
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBoxTFSid_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void textBoxTFSid_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonFind_Click((object)sender, (EventArgs)e);
            if (e.KeyCode == Keys.Escape)
                textBoxTFSid.Clear();
        }

        private void aboutStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
             MessageBox.Show("TFStoHTMLandAttach v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version + "\n\n"
                          + "Application for remote saving TFS Item information into HTML file,\n"
                          + "and download Attachments from the TFS Item into the selected folder.\n\n"
                          + "Copyright © Dim@ 2019", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBoxTFSid_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTFSid_Click(object sender, EventArgs e)
        {
            textBoxTFSid.Clear();
        }

        private void checkBoxAttach_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAttach.Checked)
                Program.downConfirm = true;
            else
                Program.downConfirm = false;
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Config Form class
            configForm confForm = new configForm(this);

            //foreach (Control item in this.Controls)
            //    item.Enabled = false;

            // Disable the Main Form and Show the Config Form
            this.Enabled = false;
            confForm.Show();
            confForm.Activate();
        }
        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you really want to exit?", "Close", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
