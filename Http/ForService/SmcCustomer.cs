using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
    public class SmcCustomer
    {
        private static long serialVersionUID = 1L;

        /** 主键id */
        private long id;

        /** 删除状态 1未删除 -1已删除 */
        private long status;

        /** 头像地址 */

        private String avatar;

        /** 性别 */

        private long gender;

        /** 昵称 */

        private String nickName;

        /** 微信openId */

        private String openId;

        /** 会员等级 */

        private long customerGrade;

        /** 密码 */
        private String password;

        /** 电话号码 */

        private String phoneNumber;

        /** token */

        private String token;

        /** 用户姓名 */

        private String customerName;

        /**接收参数 优惠券id*/
        private long couponsId;

        private long deptId;

        public long Id { get => id; set => id = value; }
        public long Status { get => status; set => status = value; }
        public string Avatar { get => avatar; set => avatar = value; }
        public long Gender { get => gender; set => gender = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public string OpenId { get => openId; set => openId = value; }
        public long CustomerGrade { get => customerGrade; set => customerGrade = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Token { get => token; set => token = value; }
        public string CustomerName { get => customerName; set => customerName = value; }
        public long CouponsId { get => couponsId; set => couponsId = value; }
        public long DeptId { get => deptId; set => deptId = value; }
    }
}
