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
            //TestDapper();
            TestRepository();

            Console.ReadKey();
        }

        static void TestDapper()
        {
            var str = "server=127.0.0.1;database=thesisdb;Uid=root;Pwd=123456;";
            var conn = new MySqlConnection(str);

            //增
            string sql = "insert user(usercode,username,password) values(@usercode,@username,@password);SELECT LAST_INSERT_ID();";
            User user = new User { UserCode = "sa2", UserName = "管理员", Password = "123456" };
            //var id = conn.ExecuteScalar<int>(sql, user);

            //删

            //改
            sql = "update user set usercode = @usercode,username= @username,password=@password where userid = @userid";
            user = new User { Id = 2, UserCode = "stu2", UserName = "haha", Password = "123" };
            conn.Execute(sql, user);

            //查

            //查全部
            IEnumerable<User> list = conn.Query<User>("select * from user");
            //根据Id查
            var userid = 2;
            var selectUser = conn.QueryFirstOrDefault<User>("select * from user where userid = @userId", new { userid = userid });

            //分页查询

            //事务
            var tran = conn.BeginTransaction();
            try
            {
                sql = "update user set usercode = @usercode,username= @username,password=@password where userid = @userid";
                string sql1 = "update user set usercode = @usercode,username= @username,password=@password where userid = @userid";
                conn.Execute(sql, "", tran);
                conn.Execute(sql1, "", tran);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }

        }

        static void TestRepository()
        {
            UserRepository userRepository = new UserRepository();

            int userid = userRepository.Insert(new User { Id = 1, UserCode = "user004", UserName = "用户005", Password = "123" });
            Console.WriteLine(userid);

            User user = userRepository.SelectBy(1);
            //userRepository.SelectBy();
            //userRepository.SelectInfoBy<User>("");
        }
    }
}
