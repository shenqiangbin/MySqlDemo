using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySqlDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //使用MySQL.data访问数据库(注意释放资源)
            //_01Demo.Run();
            //添加一些业务后，防止SQL注入
            _02Demo.Run();


            //using (var conn = GetConn())
            //{
            //    using (MySqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "select getchildlist(1)";

            //        MySqlDataAdapter adapter = new MySqlDataAdapter();
            //        adapter.SelectCommand = cmd;
            //        DataSet ds = new DataSet();
            //        adapter.Fill(ds);
            //        Console.WriteLine();
            //    }
            //}

            //验证不释放连接的后果
            //string countSql = "select count(0) from user";
            //for (int i = 0; i < 1000; i++)
            //{
            //    object returnVal = ExecuteScalar(countSql);
            //    //object returnVal = ExecuteScalar_Demo(countSql);
            //    Console.WriteLine(returnVal);
            //}

            //string sql = "insert user (usercode,username,status) values('xiaoming4','小明','1')";
            //var returnVal = ExecuteNonQuery(sql);
            //if (returnVal == 1)
            //    Console.WriteLine("用户新增成功");
            //else
            //    Console.WriteLine("用户新增失败");

            

            Console.WriteLine();
        }

       
    }
}
