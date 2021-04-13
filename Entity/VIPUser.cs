using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
   public class VIPUser
    {
        private int user_id;
        private int dept_id;
        private string user_name;
        private string nick_name;
        private string email;
        private string phonenumber;
        private string avatar;
        private string password;
        private string login_ip;
        private string create_by;
        private string update_by;
        private string remark;
        private string token;
        private string open_id;
        private DateTime create_time;
        private DateTime update_time;
        private DateTime login_date;
        private string user_type;
        private string sex;
        private string status;
        private string del_flag;
        private double integral;
        private double suplus;

        public int User_id { get => user_id; set => user_id = value; }
        public int Dept_id { get => dept_id; set => dept_id = value; }
        public string User_name { get => user_name; set => user_name = value; }
        public string Nick_name { get => nick_name; set => nick_name = value; }
        public string Email { get => email; set => email = value; }
        public string Phonenumber { get => phonenumber; set => phonenumber = value; }
        public string Avatar { get => avatar; set => avatar = value; }
        public string Password { get => password; set => password = value; }
        public string Login_ip { get => login_ip; set => login_ip = value; }
        public string Create_by { get => create_by; set => create_by = value; }
        public string Update_by { get => update_by; set => update_by = value; }
        public string Remark { get => remark; set => remark = value; }
        public string Token { get => token; set => token = value; }
        public string Open_id { get => open_id; set => open_id = value; }
        public DateTime Create_time { get => create_time; set => create_time = value; }
        public DateTime Update_time { get => update_time; set => update_time = value; }
        public DateTime Login_date { get => login_date; set => login_date = value; }
        public string User_type { get => user_type; set => user_type = value; }
        public string Sex { get => sex; set => sex = value; }
        public string Status { get => status; set => status = value; }
        public string Del_flag { get => del_flag; set => del_flag = value; }

        /// <summary>
        /// 积分
        /// </summary>
        public double Integral { get => integral; set => integral = value; }

        /// <summary>
        /// 盈余
        /// </summary>
        public double Suplus { get => suplus; set => suplus = value; }
    }
}
