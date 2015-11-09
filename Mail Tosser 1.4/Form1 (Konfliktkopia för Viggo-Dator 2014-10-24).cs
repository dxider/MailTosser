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
/* Life runs on code */
namespace Mail_Tosser_1._4
{
    public partial class Form1 : Form
    {

        Settings frm = new Settings();
        Connection con = new Connection();
        Npcs npcsfrm = new Npcs();

        List<string> items = new List<string>();

        public static List<string> attitems = new List<string>();
        public static List<string> names = new List<string>();
        public static List<string> selnames = new List<string>();
        public static string npc;
        DateTime dat;
        public Form1()
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
            frm.ShowDialog();
        }
        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            Form1_Activated(null, EventArgs.Empty);
            txtStyle.SelectedIndex = 0;
            dat = dtpExp.Value;
            dat = dat.AddDays(30);
            dtpDel.Value = DateTime.Today.AddDays(-1);
            dtpExp.Value = dat;

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
            while (reader.Read())
            {
                items.Add(reader.GetString("name") + " {" + reader.GetInt32("entry") + "}");
                lstItems.Items.Add(items[items.Count() - 1]);
            }
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
            lstItems.SelectedIndex = 0;



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
            }
            if (selnames.Count > 0) btnRec.Text = selnames.Count.ToString() + " receivers";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lstItems.Items.Clear();
            if (txtSearch.Text == "")
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    lstItems.Items.Add(items[i]);
                }
            }
            else
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    if (items[i].Contains(txtSearch.Text)) lstItems.Items.Add(items[i]);
                    if (lstItems.Items.Count > 100) break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lstAtt.Items.Count >= 12) return;
            if (lstAtt.Items.Count < 12)
            {
                lstAtt.Items.Add("[" + numCount.Value.ToString() + "] " + lstItems.SelectedItem);
                numCount.Value = 1;
            }
            if (lstAtt.Items.Count < 1) { rdbMoney.Checked = true; rdbCOD.Enabled = false; }
            else rdbCOD.Enabled = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lstAtt.SelectedIndex >= 0) lstAtt.Items.RemoveAt(lstAtt.SelectedIndex);
            if (lstAtt.Items.Count > 0) lstAtt.SelectedIndex = 0;
            if (lstAtt.Items.Count < 1) { rdbMoney.Checked = true; rdbCOD.Enabled = false; }
            else rdbCOD.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstAtt.Items.Clear();
        }

        private void btnRec_Click(object sender, EventArgs e)
        {
            Recs recs = new Recs();
            recs.ShowDialog();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
              
            var mail = new Mail(con.charconn);
            string exp = DateTimeToUnixTimestamp(dtpExp.Value).ToString().Split(',')[0];
            string del = DateTimeToUnixTimestamp(dtpDel.Value).ToString().Split(',')[0]; 
            mail.Delivery = del;
            mail.Expire = exp;
            mail.COD = rdbCOD.Checked;

            for (int i = 0; i < lstAtt.Items.Count; i++)
            {
                var item = new Item();
                item.Entry = int.Parse(lstAtt.Items[i].ToString().Split('{', '}')[1]);
                item.Quantity = int.Parse(lstAtt.Items[i].ToString().Split('[', ']')[1]);
                mail.Items.Add(item);
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
            
            int stationary = 0;
            int ssender = -1;
            Int64 money = 0;
            int hasitems = 0;
            int type = 0;
            mail.Sender = -1;

            MySqlCommand cmd0 = new MySqlCommand("SELECT `guid` FROM `characters` WHERE `name`='" + txtSender.Text + "' LIMIT 1;", con.charconn);
            MySqlDataReader reader0 = cmd0.ExecuteReader();
            while (reader0.Read()) mail.Sender = reader0.GetInt32("guid");
            reader0.Close();

            if (mail.Sender == -1)
            {
                type = 3;
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
            if (lstAtt.Items.Count > 0) hasitems = 1;

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
            dtpExp.Value = dat;
        }


        private void rdbCOD_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbInstant.Checked)
            {
                dtpDel.Enabled = false;
                dtpDel.Value = DateTime.Today.AddDays(-1);
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

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }
    }
}