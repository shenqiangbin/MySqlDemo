using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo02
{
    public class User
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }

        //为了应对数据库不能为空的情况，生成的SQL语句中字段为空则会报错，所以，只好在初始化时，就为其赋值。
        public User()
        {
            this.UserCode = "";
            this.UserName = "";
            this.Password = "";
            this.Phone = "";
            this.Email = "";
            this.Sex = -1;
        }
    }
}
