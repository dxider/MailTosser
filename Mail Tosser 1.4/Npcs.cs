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
    public partial class Npcs : Form
    {
        Connection con = new Connection();
        public static List<string> lstnpcs = new List<string>();
        public Npcs()
        {
            InitializeComponent();
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            lstNpcs.Items.Clear();
            if (txtSearch.Text == "") for (int i = 0; i < lstnpcs.Count; i++) lstNpcs.Items.Add(lstnpcs[i]);
            else
            {
                for (int i = 0; i < lstnpcs.Count; i++)
                {
                    if (lstnpcs[i].ToString().Contains(txtSearch.Text)) lstNpcs.Items.Add(lstnpcs[i]);
                    if (lstNpcs.Items.Count > 1000) break;
                }
            }
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            frmMain.npc = lstNpcs.SelectedItem.ToString().Remove(lstNpcs.SelectedItem.ToString().IndexOf('{'), lstNpcs.SelectedItem.ToString().Length - lstNpcs.SelectedItem.ToString().IndexOf('{'));
            frmMain.npc = frmMain.npc.Remove(frmMain.npc.ToString().Length - 1, 1);
            this.Close();
        }

        private void Npcs_Load(object sender, EventArgs e)
        
        {
            Cursor = Cursors.WaitCursor;
            lstNpcs.Items.Clear();
            for (int i = 0; i < lstnpcs.Count; i++) lstNpcs.Items.Add(lstnpcs[i]);
            Cursor = Cursors.Default;
        }
        
    }
}
