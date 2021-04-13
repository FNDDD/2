using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.用户
{
    /// <summary>
    /// 商品信息
    /// </summary>
   public class GoodsStore
    {
        private int goods_store_id;
        private int goods_id;
        private int classify_id;
        private int shop_id;
        private string goods_name;
        private string pinyin_code;
        private string barcode;
        private string show_img;
        private string img_url;
        private string main_img;
        private double cost_price;
        private double original_price;
        private double sale_price;
        private double vip_price;
        private string spec_data;
        private string unit;
        private string inventory_now;
        private string inventory_max;
        private string inventory_min;
        private string is_discount;
        private string integral_goods;
        private DateTime make_date;
        private string shelf_life;
        private string status;
        private string create_by;
        private DateTime create_time;
        private string update_by;
        private DateTime update_time;
        private string remark;
        private int sales;
        private int virtual_sales;
        private int supplier_id;
        private string supplier;
        private double wholesale_price;
        /// <summary>
        /// 商超商品Id
        /// </summary>
        public int Goods_store_id { get => goods_store_id; set => goods_store_id = value; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int Goods_id { get => goods_id; set => goods_id = value; }

        /// <summary>
        /// 商品类型ID
        /// </summary>
        public int Classify_id { get => classify_id; set => classify_id = value; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public int Shop_id { get => shop_id; set => shop_id = value; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Goods_name { get => goods_name; set => goods_name = value; }

        /// <summary>
        /// 商品拼音码
        /// </summary>
        public string Pinyin_code { get => pinyin_code; set => pinyin_code = value; }

        /// <summary>
        /// 商品条形码
        /// </summary>
        public string Barcode { get => barcode; set => barcode = value; }

        /// <summary>
        /// 显示图片
        /// </summary>
        public string Show_img { get => show_img; set => show_img = value; }

        /// <summary>
        /// 详情图片
        /// </summary>
        public string Img_url { get => img_url; set => img_url = value; }

        /// <summary>
        /// 主图
        /// </summary>
        public string Main_img { get => main_img; set => main_img = value; }

        /// <summary>
        /// 进货价
        /// </summary>
        public double Cost_price { get => cost_price; set => cost_price = value; }

        /// <summary>
        /// 原价
        /// </summary>
        public double Original_price { get => original_price; set => original_price = value; }

        /// <summary>
        /// 销售价
        /// </summary>
        public double Sale_price { get => sale_price; set => sale_price = value; }

        /// <summary>
        /// 会员价
        /// </summary>
        public double Vip_price { get => vip_price; set => vip_price = value; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec_data { get => spec_data; set => spec_data = value; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get => unit; set => unit = value; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public string Inventory_now { get => inventory_now; set => inventory_now = value; }

        /// <summary>
        /// 最大库存
        /// </summary>
        public string Inventory_max { get => inventory_max; set => inventory_max = value; }

        /// <summary>
        /// 最小库存
        /// </summary>
        public string Inventory_min { get => inventory_min; set => inventory_min = value; }

        /// <summary>
        /// 是否优惠0不是   1是 
        /// </summary>
        public string Is_discount { get => is_discount; set => is_discount = value; }

        /// <summary>
        /// 积分商品  0不是   1是  
        /// </summary>
        public string Integral_goods { get => integral_goods; set => integral_goods = value; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime Make_date { get => make_date; set => make_date = value; }

        /// <summary>
        /// 保质期
        /// </summary>
        public string Shelf_life { get => shelf_life; set => shelf_life = value; }

        /// <summary>
        /// 状态默认0（0=未启用，1=启用）
        /// </summary>
        public string Status { get => status; set => status = value; }

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
        /// 销量
        /// </summary>
        public int Sales { get => sales; set => sales = value; }

        /// <summary>
        /// 虚拟销量
        /// </summary>
        public int Virtual_sales { get => virtual_sales; set => virtual_sales = value; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public int Supplier_id { get => supplier_id; set => supplier_id = value; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Supplier { get => supplier; set => supplier = value; }

        /// <summary>
        /// 批发价
        /// </summary>
        public double Wholesale_price { get => wholesale_price; set => wholesale_price = value; }
    }
}
