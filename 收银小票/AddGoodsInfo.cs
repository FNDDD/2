using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.收银小票
{
   public class AddGoodsInfo
    {
        /// <summary>
        /// 条形码
        /// </summary>
        private string barcode;

        private int goodsId;

        /// <summary>
        /// 商品名称
        /// </summary>
        private string name;

        /// <summary>
        /// 规格
        /// </summary>
        private string guige;

        /// <summary>
        /// 数量
        /// </summary>
        private int num;

        /// <summary>
        /// 销售价
        /// </summary>
        private double sale_price;

        /// <summary>
        /// 分类
        /// </summary>
        private int class_gd;

        /// <summary>
        /// 供应商
        /// </summary>
        private int Supplier;

        private double cost_price;

        /// <summary>
        /// 条形码
        /// </summary>
        public string Barcode { get => barcode; set => barcode = value; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get => num; set => num = value; }

        /// <summary>
        /// 销售价
        /// </summary>
        public double Sale_price { get => sale_price; set => sale_price = value; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Guige { get => guige; set => guige = value; }

        /// <summary>
        /// 供应商
        /// </summary>
        public int Supplier1 { get => Supplier; set => Supplier = value; }

        /// <summary>
        /// 分类
        /// </summary>
        public int Class_gd { get => class_gd; set => class_gd = value; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int GoodsId { get => goodsId; set => goodsId = value; }
        public double Cost_price { get => cost_price; set => cost_price = value; }
    }
}
