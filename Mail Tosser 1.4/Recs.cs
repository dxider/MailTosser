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
    public partial class Recs : Form
    {
        
        public Recs()
        {
            InitializeComponent();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
         if (checkBox1.Checked) 
             for (int i = 0; i < lstNames.Items.Count; i++)
                 lstNames.SetItemChecked(i, true);
         else
             for (int i = 0; i < lstNames.Items.Count; i++)
                 lstNames.SetItemChecked(i, false);
        }

        private void Recs_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.selnames.Clear();
            for (int i = 0; i < lstNames.CheckedItems.Count; i++)
                frmMain.selnames.Add(lstNames.CheckedItems[i].ToString());
        }

        private void Recs_Shown(object sender, EventArgs e)
        {
            List<string> temp = frmMain.selnames;
            for (int i = 0; i < frmMain.names.Count(); i++) { lstNames.Items.Add(frmMain.names[i]); }
            for (int i = 0; i < lstNames.Items.Count; i++)
            {
                for (int j = 0; j < frmMain.selnames.Count; j++)
                {
                    //string tem = lstNames.Items[i].ToString();
                    //string te = Form1.selnames[j];
                   if (lstNames.Items[i].ToString() == frmMain.selnames[j].ToString()) lstNames.SetItemChecked(i, true);
                }
            }
        }
    }
}
