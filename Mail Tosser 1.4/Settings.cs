using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Mail_Tosser_1._4
{
    public partial class Settings : Form
    {
        Connection con = new Connection();
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Properties.Settings.Default.Host = host.Text;
            Properties.Settings.Default.User = user.Text;
            Properties.Settings.Default.Pass = pass.Text;
            Properties.Settings.Default.CharDB = chardb.Text;
            Properties.Settings.Default.WorldDB = worlddb.Text;
            Properties.Settings.Default.Port = port.Text;
            Properties.Settings.Default.AuthDB = txtAuth.Text;
            while (true)
            {
                try
                {
                    con = new Connection();
                    con.charconn.Close();
                    con.worldconn.Close();
                    //con.authconn.Close();
                    con.charconn.Open();
                    con.worldconn.Open();
                    con.authconn.Open();
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    break;
                }
                catch (Exception ex)
                {
                        DialogResult dr;
                        dr = MessageBox.Show(this, "Something went wrong. Exception message:\n" + ex.Message, "Error!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (dr == System.Windows.Forms.DialogResult.Cancel) break;
                }
            }
        }
        private void host_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Host = host.Text;
            ReOpenConnections();
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            host.Text = Properties.Settings.Default.Host;
            user.Text = Properties.Settings.Default.User;
            pass.Text = Properties.Settings.Default.Pass;
            port.Text = Properties.Settings.Default.Port;
            chardb.Text = Properties.Settings.Default.CharDB;
            worlddb.Text = Properties.Settings.Default.WorldDB;
            txtAuth.Text = Properties.Settings.Default.AuthDB;
        }

        private void user_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.User = user.Text;
            ReOpenConnections();
        }

        private void pass_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Pass = pass.Text;
            ReOpenConnections();
        }

        private void port_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Port = port.Text;
            ReOpenConnections();
        }

        private void txtAuth_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AuthDB = txtAuth.Text;
            ReOpenConnections();
        }

        private void worlddb_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.WorldDB = worlddb.Text;
            ReOpenConnections();
        }

        private void ReOpenConnections()
        {
            con = new Connection();
        }

    }
}
