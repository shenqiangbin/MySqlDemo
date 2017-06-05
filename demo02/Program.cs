using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace demo02
{
    class Program
    {
        // 参考地址：http://www.cnblogs.com/Sinte-Beuve/p/4231053.html
        static void Main(string[] args)
        {
            var str = "server=192.168.103.90;database=thesismgmt;Uid=thesismgmt;Pwd=123456;";
            var conn = new MySqlConnection(str);
            var list = conn.Query("SELECT * FROM thesismgmt.user;");

            foreach (var item in list)
            {
                var s = item;
            }

            Console.ReadKey();
        }
    }
}
