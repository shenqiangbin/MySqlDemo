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

            var success = AddUser("xiaoming4", "小明");
            if (success)
                Console.WriteLine("用户新增成功");
            else
                Console.WriteLine("用户新增失败");

            Console.WriteLine();
        }

        //service层的编写，考虑情况要周全

        #region Service

        public static bool AddUser(string userCode, string userName)
        {
            if (string.IsNullOrEmpty(userCode))
                throw new Exception("userCode不能为空");
            if (string.IsNullOrEmpty(userName))
                throw new Exception("userName不能为空");

            string sql = $"insert user (usercode,username,status) values('{userCode}','{userName}','1')";
            var returnVal = ExecuteNonQuery(sql);
            if (returnVal == 1)
                return true;
            else
                return false;
        }

        public class User
        {
            public string UserCode { get; set; }
            public string UserName { get; set; }
        }

        // 判断某个usercode是否存在 和 根据用户code获取用户的代码 可以合并的
        public static User GetByUserCode(string userCode)
        {
            if (string.IsNullOrEmpty(userCode))
                throw new Exception("userCode不能为空");

            DataTable dt = ExecuteDataTable("select * from user where usercode = " + userCode);
            if (dt != null && dt.Rows.Count > 0)
            {
                User user = new User();
                return user;
            }

            return null;
        }


        #endregion

        static object ExecuteScalar(string sql)
        {
            using (var conn = GetConn())
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    return cmd.ExecuteScalar();
                }
            }
        }

        static int ExecuteNonQuery(string sql)
        {
            using (var conn = GetConn())
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        static DataTable ExecuteDataTable(string sql)
        {
            DataTable dt = null;
            using (var conn = GetConn())
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        static object ExecuteScalar_Demo(string sql)
        {
            var conn = GetConn();
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                return cmd.ExecuteScalar();
            }
        }

        static MySqlConnection GetConn()
        {
            string str = "server=127.0.0.1;database=labdb;Uid=root;Pwd=123456;";
            //注意：链接字符串要小写。
            //str = "server=192.168.103.90;database=thesismgmt;Uid=thesismgmt;Pwd=123456;";

            MySqlConnection conn = new MySqlConnection(str);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            return conn;
        }
    }
}
