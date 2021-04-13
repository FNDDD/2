using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    public class Order
    {
        public Order()
        {
            Dept_id = 1;
            Order_status = 0;
            Order_source = "1";
            Return_reason = "未退款";
            Order_type = 1;
            Remark = "无备注";
            Coupons_id = "无优惠券";
            Coupons_price = 0;
        }
        private int id;
        private string order_number;
        private double actual_payment;
        private int order_status;
        private string pay_ment;
        private string customer_id;
        private string order_source;
        private int dept_id;
        private string return_reason;
        private string coupons_id;
        private int order_type;
        private double coupons_price;
        private double product_all_price;
        private string cancel_people_name;
        private DateTime payment_time;
        private int status;
        private string create_by;
        private DateTime create_time;
        private string update_by;
        private DateTime update_time;
        private string remark;
        private DateTime refund_time;
        private int user_id;

        /// <summary>
        /// 订单id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Order_number { get => order_number; set => order_number = value; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public double Actual_payment { get => actual_payment; set => actual_payment = value; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Order_status { get => order_status; set => order_status = value; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string Pay_ment { get => pay_ment; set => pay_ment = value; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string Customer_id { get => customer_id; set => customer_id = value; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string Order_source { get => order_source; set => order_source = value; }

        /// <summary>
        /// 门店id
        /// </summary>
        public int Dept_id { get => dept_id; set => dept_id = value; }

        /// <summary>
        /// 退款原因
        /// </summary>
        public string Return_reason { get => return_reason; set => return_reason = value; }

        /// <summary>
        /// 优惠券id
        /// </summary>
        public string Coupons_id { get => coupons_id; set => coupons_id = value; }

        /// <summary>
        /// 订单类型  1供销社  2商超
        /// </summary>
        public int Order_type { get => order_type; set => order_type = value; }

        /// <summary>
        /// 优惠券减免金额
        /// </summary>
        public double Coupons_price { get => coupons_price; set => coupons_price = value; }

        /// <summary>
        /// 商品总价
        /// </summary>
        public double Product_all_price { get => product_all_price; set => product_all_price = value; }

        /// <summary>
        /// 核销人
        /// </summary>
        public string Cancel_people_name { get => cancel_people_name; set => cancel_people_name = value; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime Payment_time { get => payment_time; set => payment_time = value; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get => status; set => status = value; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Create_by { get => create_by; set => create_by = value; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_time { get => create_time; set => create_time = value; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string Update_by { get => update_by; set => update_by = value; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Update_time { get => update_time; set => update_time = value; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get => remark; set => remark = value; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime Refund_time { get => refund_time; set => refund_time = value; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int User_id { get => user_id; set => user_id = value; }
    }
}
