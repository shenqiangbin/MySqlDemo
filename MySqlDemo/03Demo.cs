using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlDemo
{
    //防SQL注入
    public class _03Demo
    {
        public static void Run()
        {
            //var success = AddUser("xiaoming4", "小明");
            //if (success)
            //    Console.WriteLine("用户新增成功");
            //else
            //    Console.WriteLine("用户新增失败");

            //sql 注入攻击
            var users = GetByUserCode("admin' or '1=1");

        }

        //界面层
        public static void Add()
        {
            Console.WriteLine("这里是新增用户");
            Console.WriteLine("请输入账号");
            string userCode = Console.ReadLine();
            Console.WriteLine("请输入名字");
            string userName = Console.ReadLine();

            try
            {
                bool success = AddUser(userCode, userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //service层的编写，考虑情况要周全

        #region Service

        public static bool AddUser(string userCode, string userName)
        {
            if (string.IsNullOrEmpty(userCode))
                throw new Exception("userCode不能为空");
            if (string.IsNullOrEmpty(userName))
                throw new Exception("userName不能为空");

            var users = GetByUserCode(userCode);
            if (users.Count() != 0)
                throw new Exception("userCode已存在");

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
        public static IEnumerable<User> GetByUserCode(string userCode)
        {
            if (string.IsNullOrEmpty(userCode))
                throw new Exception("userCode不能为空");

            List<User> users = new List<User>();

            DataTable dt = ExecuteDataTable($"select * from user where usercode = '{userCode}'");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    User user = new User();
                    user.UserCode = row["usercode"] == DBNull.Value ? "" : row["usercode"].ToString();
                    user.UserName = row["username"] == DBNull.Value ? "" : row["username"].ToString();
                    users.Add(user);
                }
            }

            return users;
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
            DataTable dt = new DataTable();
            using (var conn = GetConn())
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    MySqlParameterCollection paras = null;
                    cmd.Parameters.AddWithValue("", "");
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
