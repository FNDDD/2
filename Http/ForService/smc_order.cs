using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
   public class smc_order
    {
        private string strType;
        /** 主键id */
    private String id;

    private String deptName;

        /** 实际付款 */
    private double actualPayment;

        /** 订单状态 */
    private int orderStatus;

        /** 支付方式 */      
    private String payMent;

        /** 支付来源 */      
    private int orderSource;

        /**会员id*/
   private long customerId;

   /** 用户电话 */
    
    private String customerPhone;

        /** 订单编号 */
    private String orderNumber;
        /** 订单类型 */
    private int orderType;

        /** 收货地址Id */
    private String addressId;

        /** 商家留言 */
    private String businessesLeaveMessage;

        /** 配送日期 */
    private String deliveryDate;

        /** 核销人 */

    private String cancelPeopleName;

        /** $column.columnComment */
        private string status;

        /**会员昵称*/
        private String nickName;
        private String couponsName;

        public string Id { get => id; set => id = value; }
        public string DeptName { get => deptName; set => deptName = value; }
        public double ActualPayment { get => actualPayment; set => actualPayment = value; }
        public int OrderStatus { get => orderStatus; set => orderStatus = value; }
        public string PayMent { get => payMent; set => payMent = value; }
        public int OrderSource { get => orderSource; set => orderSource = value; }
        public long CustomerId { get => customerId; set => customerId = value; }
        public string CustomerPhone { get => customerPhone; set => customerPhone = value; }
        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public int OrderType { get => orderType; set => orderType = value; }
        public string AddressId { get => addressId; set => addressId = value; }
        public string BusinessesLeaveMessage { get => businessesLeaveMessage; set => businessesLeaveMessage = value; }
        public string DeliveryDate { get => deliveryDate; set => deliveryDate = value; }
        public string CancelPeopleName { get => cancelPeopleName; set => cancelPeopleName = value; }
        public string Status { get => status; set => status = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public string CouponsName { get => couponsName; set => couponsName = value; }
        public string StrType { get => strType; set => strType = value; }
    }
}
