using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Reflection;
using Mail_Tosser_1._4.Classes;
/* Life runs on code */
namespace Mail_Tosser_1._4
{
    public partial class frmMain : Form
    {

        Settings frm = new Settings();
        Connection con = new Connection();
        Npcs npcsfrm = new Npcs();

        List<Mail_Tosser_1._4.Classes.Item> items = new List<Mail_Tosser_1._4.Classes.Item>();

        public static List<string> attitems = new List<string>();
        public static List<string> names = new List<string>();
        public static List<string> selnames = new List<string>();
        public static string npc;
        DateTime dat;
        public frmMain()
        {
            InitializeComponent();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void btnSett_Click(object sender, EventArgs e)
        {
            frm.Hide();
            frm = new Settings();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Visible = true;
                Form1_Load(null, null);
            }
        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            items.Clear();
            lblVersion.Text = "Version: " +Assembly.GetEntryAssembly().GetName().Version.ToString(4);
            Text = Assembly.GetEntryAssembly().GetName().Name.ToString();
            ckbInstant.Checked = false; ckbInstant.Checked = true;
            Form1_Activated(null, EventArgs.Empty);
            txtStyle.SelectedIndex = 0;
            dat = dtpExp.SelectionRange.Start;
            dat = dat.AddDays(30);

            dtpExp.SelectionRange.Start = dat;

            if (con.worldconn.State != ConnectionState.Open)
            {
                try
                {
                    con.worldconn.Open();
                    con.charconn.Open();
                }
                catch
                {
                    frm.ShowDialog();
                }
            }
            MySqlCommand cmd = new MySqlCommand("SELECT `name`, `entry` FROM `item_template`", con.worldconn);
            MySqlDataReader reader = cmd.ExecuteReader();
            lstSearchItems.Items.Clear();
            lstSearchItems.BeginUpdate();
            while (reader.Read())
            {
                Mail_Tosser_1._4.Classes.Item it = new Mail_Tosser_1._4.Classes.Item();
                it.Entry = reader.GetInt32("entry");
                it.Name = reader.GetString("name");
                items.Add(it);
                ListViewItem item = lstSearchItems.Items.Add(it.Entry.ToString());
                item.SubItems.Add(it.Name);
            }
            lstSearchItems.EndUpdate();
            reader.Close();

            cmd = new MySqlCommand("SELECT `name` FROM `characters`;", con.charconn);
            reader = cmd.ExecuteReader();
            txtSender.Items.Clear();
            while (reader.Read())
            {
                names.Add(reader.GetString("name"));
                txtSender.Items.Add(reader.GetString("name"));
            }
            reader.Close();

            if (con.worldconn.State != ConnectionState.Open) con.worldconn.Open();
            cmd = new MySqlCommand("SELECT `name`, `entry` FROM `creature_template`", con.worldconn);
            reader = cmd.ExecuteReader();
            while (reader.Read()) Npcs.lstnpcs.Add(reader.GetString("name") + " {" + reader.GetInt32("entry").ToString() + "}");
            reader.Close();

            txtSender.SelectedIndex = 0;
            txtStyle.SelectedIndex = 0;
            if (lstSearchItems.Items.Count > 0)
                lstSearchItems.Items[0].Selected = true;


        }

        public List<string> getnames() { return names; }

        private void Form1_Activated(object sender, EventArgs e)
        {
            try
            {
                
                if (con.charconn.State != ConnectionState.Open) con.charconn.Open();
                if (con.worldconn.State != ConnectionState.Open) con.worldconn.Open();
                //if (con.authconn.State != ConnectionState.Open) con.authconn.Open();
            }
            catch
            {
                btnSett_Click(btnSett, EventArgs.Empty);
                con = new Connection();
            }
            if (selnames.Count > 0) btnRec.Text = selnames.Count.ToString() + " receivers";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnAddSelected(object sender, EventArgs e)
        {
            if (lstSelectedItems.Items.Count >= 12) return;
            if (lstSelectedItems.Items.Count < 12)
            {
                if(lstSearchItems.SelectedItems.Count==1)
                {
                    ListViewItem item = (ListViewItem)lstSearchItems.SelectedItems[0].Clone();
                    item.SubItems.Add(numCount.Value.ToString());
                    lstSelectedItems.Items.Add(item);
                    UpdateCapacity();
                }
                numCount.Value = 1;
            }
            if (lstSelectedItems.Items.Count < 1) { rdbMoney.Checked = true; rdbCOD.Enabled = false; }
            else rdbCOD.Enabled = true;
        }

        private void UpdateCapacity()
        {
            lblCapacity.Text = "Attached items (" + lstSelectedItems.Items.Count.ToString() + " of 12)";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lstSelectedItems.SelectedItems.Count == 1)
                lstSelectedItems.Items.RemoveAt(lstSelectedItems.SelectedItems[0].Index);

            if (lstSelectedItems.Items.Count > 0)
            { 
                lstSelectedItems.Items[0].Selected = true;
                rdbMoney.Checked = true;
                rdbCOD.Enabled = false; 
            }
            else 
                rdbCOD.Enabled = true;
            UpdateCapacity();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstSelectedItems.Items.Clear();
        }

        private void btnRec_Click(object sender, EventArgs e)
        {
            Recs recs = new Recs();
            recs.ShowDialog();
            lstReceivers.Items.Clear();
            foreach (string receiver in selnames)
                lstReceivers.Items.Add(receiver);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
              
            var mail = new Mail(con.charconn);
            string exp = DateTimeToUnixTimestamp(dtpExp.SelectionRange.Start).ToString().Split(',')[0];
            string del = DateTimeToUnixTimestamp(dtpDel.SelectionRange.Start).ToString().Split(',')[0]; 
            mail.Delivery = del;
            mail.Expire = exp;
            mail.COD = rdbCOD.Checked;

            foreach (ListViewItem item in lstSelectedItems.Items)
            {
                var itemselected = new Item();
                itemselected.Entry = int.Parse(item.Text);
                itemselected.Quantity = int.Parse(item.SubItems[2].Text);
                mail.Items.Add(itemselected);
            }

            if (selnames.Count < 1)
            {
                    MessageBox.Show(this, "Please select at least one receiver.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
            }

            if (txtBody.Text == "")
            {
                MessageBox.Show(this, "Please enter a body message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
            }

            if (txtSubject.Text == "")
            {
                MessageBox.Show(this, "Please enter a subject.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = Cursors.Default;
                return;
            }

            List<int> guids = new List<int>();
            mail.Type = 0;
            mail.Sender = -1;
            int stationary = 0;
            int ssender = -1;
            Int64 money = 0;
            int hasitems = 0;
            int type = 0;

            MySqlCommand cmd0 = new MySqlCommand("SELECT `guid` FROM `characters` WHERE `name`='" + txtSender.Text + "' LIMIT 1;", con.charconn);
            MySqlDataReader reader0 = cmd0.ExecuteReader();
            while (reader0.Read()) mail.Sender = reader0.GetInt32("guid");
            reader0.Close();

            if (mail.Sender == -1)
            {
                mail.Type = 3;
                cmd0 = new MySqlCommand("SELECT `entry` FROM `creature_template` WHERE `name`='" + txtSender.Text + "';", con.worldconn);
                reader0 = cmd0.ExecuteReader();
                while (reader0.Read())
                {
                    mail.Sender = reader0.GetInt32("entry");
                }
                reader0.Close();
                if (mail.Sender == -1) 
                {
                    MessageBox.Show(this, "NPC not found, remember that the sender field is case sensetive!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Cursor = Cursors.Default;
                    return;
                }
            }
               
            for (int i = 0; i < selnames.Count; i++)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT `guid` FROM `characters` WHERE `name`='" + selnames[i] + "' LIMIT 1;", con.charconn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) mail.Receivers.Add(reader.GetInt32("guid"));
                reader.Close();
            }
            switch (txtStyle.SelectedIndex)
            {
                case 0:
                    mail.Style = 41;
                    break;
                case 1:
                    mail.Style = 61;
                    break;
                case 2:
                    mail.Style = 62;
                    break;
                case 3:
                    mail.Style = 64;
                    break;
                case 4:
                    mail.Style = 65;
                    break;
                case 5: 
                    mail.Style = 1;
                    break;
            }

            money = Convert.ToInt64(c.Value);
            money += Convert.ToInt64(s.Value * 100);
            money += Convert.ToInt64(g.Value * 10000);
            mail.Cash = money;
            if (lstSelectedItems.Items.Count > 0)
                hasitems = 1;

            mail.Body = txtBody.Text;
            mail.Subject = txtSubject.Text;

            DialogResult dr; 
            while (true)
            {

                //Exception ex = con.ExecuteMail(txtSubject.Text.Replace("'", "`"), stationary, txtBody.Text.Replace("'", "`"), guids, ssender, money.ToString(), hasitems, type, false, del.ToString(), exp.ToString());
                MySqlException ex = mail.Send();
                if (ex != null)
                {
                    dr = MessageBox.Show(this, "Something went wrong. Error message:\n" + ex.Message, "Error!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (dr == System.Windows.Forms.DialogResult.Cancel) break;
                }
                else
                {
                    MessageBox.Show(this, "Success! The mail will appear in the characters' inbox after one relog.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }

                
            }
            this.Cursor = Cursors.Default;
        }

        private void txtSQL_Click(object sender, EventArgs e)
        {
        //    List<string> lst = new List<string>();
        //    lst.Add("[20] BREAD {2000}");
        //    con.ExecuteItems(20, 4, lst);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ac-web.org/forums/showthread.php?198490-Release-Mail-Tosser-1-4&p=2034735#post2034735");
        }

        private void rdbMoney_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dtpExp.SelectionRange.Start = dat;
            dtpExp.SelectionRange.End = dat;
        }


        private void rdbCOD_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbInstant.Checked) {
                dtpDel.Enabled = false;
                dtpDel.SelectionRange.Start = DateTime.Today.AddDays(-1);
            } 
            else dtpDel.Enabled = true;
        }

        private void txtSender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            npcsfrm = new Npcs();
            Cursor = Cursors.WaitCursor;
            npcsfrm.ShowDialog();
            txtSender.Text = npc;
            Cursor = Cursors.Default;
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                lstSearchItems.Items.Clear();
                if (txtSearch.Text == "")
                {
                    foreach (Classes.Item it in items)
                    {
                        ListViewItem item = lstSearchItems.Items.Add(it.Entry.ToString());
                        item.SubItems.Add(it.Name);
                    }
                }
                else
                {
                    foreach (Classes.Item it in items)
                    {
                        if (it.Name.Contains(txtSearch.Text))
                        {
                            ListViewItem item = lstSearchItems.Items.Add(it.Entry.ToString());
                            item.SubItems.Add(it.Name);
                        }
                    }
                }
            }
        }
    }
}