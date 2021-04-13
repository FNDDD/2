using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
   public class MenDian
    {

        /** 部门ID */
        private long deptId;

        /** 父部门ID */
        private long parentId;

        /** 祖级列表 */
        private String ancestors;

        /** 店铺名称 */

    private String deptName;

        /** 店铺编号 */

    private String deptNumber;

        /** 支付商户号 */

    private String merchantCode;

        /** 店铺图标 */
        private String picture;





        /** 负责人 */
        private String leader;

        /** 店铺地址 */

    private String address;

        /** 联系电话 */
        /** 店铺热线 */

    private String phone;

        /** 邮箱 */

    private String email;

        /** 部门状态:0正常,1停用 */
        private String status;

        /** 删除标志（0代表存在 2代表删除） */
        private String delFlag;

        private string secret_key;

        public long DeptId { get => deptId; set => deptId = value; }
        public long ParentId { get => parentId; set => parentId = value; }
        public string Ancestors { get => ancestors; set => ancestors = value; }
        public string DeptName { get => deptName; set => deptName = value; }
        public string DeptNumber { get => deptNumber; set => deptNumber = value; }
        public string MerchantCode { get => merchantCode; set => merchantCode = value; }
        public string Picture { get => picture; set => picture = value; }
        public string Leader { get => leader; set => leader = value; }
        public string Address { get => address; set => address = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Status { get => status; set => status = value; }
        public string DelFlag { get => delFlag; set => delFlag = value; }
        public string Secret_key { get => secret_key; set => secret_key = value; }
    }
}
