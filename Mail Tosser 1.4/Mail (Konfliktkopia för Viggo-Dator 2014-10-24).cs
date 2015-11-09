using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mail_Tosser_1._4
{
    class Mail
    {
        public string Subject { get; set; }
        public int Style { get; set; }
        public string Body { get; set; }
        public string Delivery { get; set; }
        public string Expire { get; set; }
        public List<Item> Items { get; set; }
        public int MyProperty { get; set; }
        public List<int> Receivers { get; set; }
        public int Sender { get; set; }
        public bool COD { get; set; }
        public long Cash { get; set; }
        public MySqlConnection CharConn { get; set; }

        public Mail(MySqlConnection con)
        {
            CharConn = con;
            Items = new List<Item>();
            Receivers = new List<int>();
        }

        public MySqlException Send()
        {
            
            for (int i = 0; i < Receivers.Count; i++)
            {
                try
                {
                    string ID = GetNewID().ToString();
                    string codcash = "0";
                    if (COD) { codcash = Cash.ToString(); Cash = 0; }
                    if (Body != null) Body = Body.Replace("$N", Form1.selnames[i]);
                    string query = String.Format("INSERT INTO `mail` (`id`, `messageType`, `stationery`, `mailTemplateId`, `sender`, `receiver`, `subject`, `body`, `has_items`, `expire_time`, `deliver_time`, `money`, `cod`) VALUES ({0}, {1}, {2}, 0, {3}, {4}, '{5}', '{6}', {7}, {8}, {9}, {10}, {11});",
                        ID,
                        "3",
                        Style.ToString(),
                        Sender.ToString(),
                        Receivers[i].ToString(),
                        Subject,
                        Body,
                        Convert.ToInt32(Items.Count >= 1).ToString(),
                        Expire,
                        Delivery,
                        Cash.ToString(),
                        codcash);
                    var cmd = new MySqlCommand(query, CharConn);
                    cmd.ExecuteNonQuery();
                    MySqlDataReader reader;

                    // ITEMS:
                    int guid = 0;
                    var guids = new List<int>();
                    cmd = new MySqlCommand("SELECT `guid` FROM `item_instance`", CharConn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int nguid = reader.GetInt32("guid");
                        if (nguid > guid) guid = nguid;
                    }
                    reader.Close();
                    guid += 2;
                    for (int j = 0; j < Items.Count; j++)
                    {
                        cmd = new MySqlCommand("INSERT INTO `item_instance` (`itemEntry`, `owner_guid`, `count`, `charges`, `enchantments`, `guid`, `durability`) VALUES (" + Items[j].Entry.ToString() + ", " + Receivers[i].ToString() + ", " + Items[j].Quantity.ToString() + ", '1 0 0 0 0', '0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0', " + guid + ", 200);", CharConn);
                        cmd.ExecuteNonQuery();
                        guids.Add(guid);
                        guid += 2;
                    }
                    for (int j = 0; j < guids.Count; j++)
                    {
                        cmd = new MySqlCommand("INSERT INTO `mail_items` VALUES (" + ID + ", " + guids[j].ToString() + ", " + Receivers[i].ToString() + ");", CharConn);
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (MySqlException ex)
                {
                    return ex;
                }
            }
            return null;
        }

        private int GetNewID()
        {
            MySqlCommand cmd = new MySqlCommand("SELECT `id` FROM `mail`;", CharConn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int hid = 0;
            while (reader.Read())
            {
                int nid = reader.GetInt32("id");
                if (nid > hid) hid = nid;
            }
            reader.Close();
            return hid + 1;
        }
    }

    class Item
    {
        public int Quantity { get; set; }
        public int Entry { get; set; }
    }
}
