using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
   public class CashUser
    {
        private String searchValue;
        
        private String userType;
        private String createBy;

        private String updateBy;

        private String remark;

        private String updateTime;

        private String createTime;

        /** 用户ID */
        private long userId;

        /** 店铺id,即deptId或deviceId */
      
        private long dept_id;

        /** 用户账号 */
      
        private String userName;

        /** 用户昵称 */
      
        private String nickName;

       /** 手机号码 */
       
        private String phonenumber;

        /** 密码 */
     
        private String password;

        /** 帐号状态（0正常 1停用） */
      
        private String status;

        /** token */

        private String token;

       
        public long UserId { get => userId; set => userId = value; }
        public long Dept_Id { get => dept_id; set => dept_id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public string Phonenumber { get => phonenumber; set => phonenumber = value; }
        public string Password { get => password; set => password = value; }
        public string Status { get => status; set => status = value; }
        public string Token { get => token; set => token = value; }
        public string SearchValue { get => searchValue; set => searchValue = value; }
        public string CreateBy { get => createBy; set => createBy = value; }
        public string UpdateBy { get => updateBy; set => updateBy = value; }
        public string Remark { get => remark; set => remark = value; }
        public string UpdateTime { get => updateTime; set => updateTime = value; }
        public string CreateTime { get => createTime; set => createTime = value; }
        public string UserType { get => userType; set => userType = value; }
    }
}
