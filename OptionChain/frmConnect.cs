using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OptionChain
{
    public partial class frmConnect : Form
    {
        private int clientId;
        private int port;

        public frmConnect ()
        {
            InitializeComponent ();
            if (Properties.Settings.Default.IfUsingIBGateway)
            {
                rbIBGateway.Checked = true;
            }
            else
            {
                rbTWS.Checked = true;
            }
            tbClientId.Text = Properties.Settings.Default.ClientID.ToString ();
        }

        public int Port
        {
            get
            {
                return port;
            }
        }

        public int ClientId
        {
            get
            {
                return clientId;
            }
        }
        private void btnConnect_Click (object sender, EventArgs e)
        {
            port = 7496;
            if (!rbTWS.Checked)
            {
                port = 4001;
            }

            try
            {
                clientId = int.Parse (tbClientId.Text);
            }
            catch (Exception)
            {
                MessageBox.Show ("Invalid client id specified");
                btnConnect.DialogResult = DialogResult.Cancel;
            }
        }

        private void frmConnect_FormClosing (object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.IfUsingIBGateway = rbIBGateway.Checked;
            int ClientID = 5;
            if (int.TryParse (tbClientId.Text, out ClientID))
            {
                Properties.Settings.Default.ClientID = ClientID;
            }
            Properties.Settings.Default.Save ();
        }
    }
}
