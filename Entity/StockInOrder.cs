using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{

    /// <summary>
    /// 入库单对象
    /// </summary>
    public class StockInOrder
    {
        private string order_code;
        private DateTime time;
        private int user_id;
        private double all_in_price;
        private double all_out_price;
        private int all_goods_sum;
        private string delivery_by;
        private string delivery_phone;
        private string remark;
        private string orther_order_code;
        



        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime Time { get => time; set => time = value; }

        /// <summary>
        /// 入库人id
        /// </summary>
        public int User_id { get => user_id; set => user_id = value; }

        /// <summary>
        /// 总进价
        /// </summary>
        public double All_in_price { get => all_in_price; set => all_in_price = value; }

        /// <summary>
        /// 总售价
        /// </summary>
        public double All_out_price { get => all_out_price; set => all_out_price = value; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int All_goods_sum { get => all_goods_sum; set => all_goods_sum = value; }

        /// <summary>
        /// 送货人
        /// </summary>
        public string Delivery_by { get => delivery_by; set => delivery_by = value; }

        /// <summary>
        /// 送货人电话
        /// </summary>
        public string Delivery_phone { get => delivery_phone; set => delivery_phone = value; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get => remark; set => remark = value; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string Order_code { get => order_code; set => order_code = value; }

        /// <summary>
        /// 订单编码（服务端的）
        /// </summary>
        public string Orther_order_code { get => orther_order_code; set => orther_order_code = value; }
    }
}
