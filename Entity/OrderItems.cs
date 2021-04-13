using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    public class OrderItems
    {
        private int id;
        private int goods_supply_id;
        private int goods_num;
        private int status;
        private int skill_item_id;
        private int class_id;
        private string goods_name;
        private string order_number;
        private string goods_store_id;
        private string create_by;
        private string update_by;
        private string remark;
        private string order_id;
        private string barcode;
        private double product_price;
        private DateTime create_time;
        private DateTime update_time;

        private string zhekou;

        public int Id { get => id; set => id = value; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int Goods_supply_id { get => goods_supply_id; set => goods_supply_id = value; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Goods_num { get => goods_num; set => goods_num = value; }

        /// <summary>
        /// 订单状态1 正常  2有退款
        /// </summary>
        public int Status { get => status; set => status = value; }

        /// <summary>
        /// 
        /// </summary>
        public int Skill_item_id { get => skill_item_id; set => skill_item_id = value; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public int Class_id { get => class_id; set => class_id = value; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Goods_name { get => goods_name; set => goods_name = value; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string Order_number { get => order_number; set => order_number = value; }

        /// <summary>
        /// 商超商品ID
        /// </summary>
        public string Goods_store_id { get => goods_store_id; set => goods_store_id = value; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Create_by { get => create_by; set => create_by = value; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string Update_by { get => update_by; set => update_by = value; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get => remark; set => remark = value; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string Order_id { get => order_id; set => order_id = value; }

        /// <summary>
        /// 条形码
        /// </summary>
        public string Barcode { get => barcode; set => barcode = value; }

        /// <summary>
        /// 生产价格
        /// </summary>
        public double Product_price { get => product_price; set => product_price = value; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create_time { get => create_time; set => create_time = value; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Update_time { get => update_time; set => update_time = value; }
        public string Zhekou { get => zhekou; set => zhekou = value; }
    }
}
