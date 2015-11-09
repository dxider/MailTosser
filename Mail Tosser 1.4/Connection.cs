using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mail_Tosser_1._4
{

    class Connection
    {
        public MySqlConnection charconn= null;
        public MySqlConnection worldconn=null;
        public MySqlConnection authconn = null;

        public Connection()
        {
            charconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";uid=" + Properties.Settings.Default.User + "; pwd=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.CharDB + "; pooling=false;default command timeout=5;");
            worldconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";uid=" + Properties.Settings.Default.User + "; pwd=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.WorldDB + "; pooling=false;default command timeout=5;");
            authconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";uid=" + Properties.Settings.Default.User + "; pwd=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.AuthDB + "; pooling=false;default command timeout=5;");
        }
        
        
        //public MySqlConnection charconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";user id=" + Properties.Settings.Default.User + "; password=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.CharDB + "; pooling=false");
        //public MySqlConnection worldconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";user id=" + Properties.Settings.Default.User + "; password=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.WorldDB + "; pooling=false");
        //public MySqlConnection authconn = new MySqlConnection("server=" + Properties.Settings.Default.Host + ";user id=" + Properties.Settings.Default.User + "; password=" + Properties.Settings.Default.Pass + "; port=" + Properties.Settings.Default.Port + "; database=" + Properties.Settings.Default.AuthDB + "; pooling=false");
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader reader;
        //public Exception ExecuteMail(string sub, int style, string bod, List<int> recs, int sender, string money, int hasitems, int type, bool cod, string del, string exp)
        //{
        //    try
        //    {
        //        string icod = "0";
        //        if (cod) { icod = money; money = "0"; }
        //        if (charconn.State != System.Data.ConnectionState.Open) charconn.Open();
                
        //        reader.Close();
        //        for (int i = 0; i < recs.Count(); i++)
        //        {
        //            string bodyy = bod.Replace("$N", Form1.selnames[i]);
        //            cmd = new MySqlCommand("INSERT INTO `mail` (`id`, `messageType`, `stationery`, `mailTemplateId`, `sender`, `receiver`, `subject`, `body`, `has_items`, `expire_time`, `deliver_time`, `money`, `cod`) VALUES (" 
        //                + id.ToString()
        //                + ", "
        //                + type.ToString()
        //                + ", "
        //                + style.ToString()
        //                + ", 0, "
        //                + sender.ToString() 
        //                + ", " 
        //                + recs[i].ToString()
        //                + ", '"
        //                + sub
        //                + "', '" 
        //                + bodyy
        //                + "', "
        //                + hasitems.ToString()
        //                + ", "
        //                + exp
        //                + ", " 
        //                + del 
        //                + ", "
        //                + money.ToString()
        //                + ", "
        //                + icod.ToString() 
        //                + ");", charconn);
        //            cmd.ExecuteNonQuery();
        //            if (Form1.attitems.Count > 0) ExecuteItems(id, recs[i], Form1.attitems);
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex;
        //    }
        //    return null;
        //}
        //public Exception ExecuteItems(int mail_id, int receiver, List<string> items)
        //{
        //    int guid = 0;
        //    List<int> guids = new List<int>();
        //    if (charconn.State != System.Data.ConnectionState.Open) charconn.Open();
        //    cmd = new MySqlCommand("SELECT `guid` FROM `item_instance`", charconn);
        //    reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        int nguid = reader.GetInt32("guid");
        //        if (nguid > guid) guid = nguid;
        //    }
        //    reader.Close();
        //    guid += 2;
        //    for (int i = 0; i < items.Count; i++)
        //    {
        //        string count;
        //        count = items[i].Split(']')[0];
        //        count = count.Remove(0, 1);
        //        string id = items[i].Split('{')[1];
        //        id = id.Remove(id.Length - 1, 1);
        //        cmd = new MySqlCommand("INSERT INTO `item_instance` (`itemEntry`, `owner_guid`, `count`, `charges`, `enchantments`, `guid`, `durability`) VALUES (" + id + ", " + receiver + ", " + count + ", '1 0 0 0 0', '0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0', " + guid + ", 200);", charconn);
        //        cmd.ExecuteNonQuery();
        //        guids.Add(guid);
        //        guid += 2;
        //    }
        //    for (int i = 0; i < guids.Count; i++)
        //    {
        //        cmd = new MySqlCommand("INSERT INTO `mail_items` VALUES (" + mail_id + ", " + guids[i].ToString() + ", " + receiver.ToString() + ");", charconn);
        //        cmd.ExecuteNonQuery();
        //    }
        //    return null;
        //}
    
    }
    
}
