using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "server=127.0.0.1;database=ThesisMgmt;Uid=root;Pwd=123456;";//
            str = "server=192.168.103.90;database=ThesisMgmt;Uid=root;Pwd=123456;";
            using (MySqlConnection conn = new MySqlConnection(str))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select getchildlist(1)";

                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    Console.WriteLine();
                }              
            }

            Console.WriteLine();
        }
    }
}
