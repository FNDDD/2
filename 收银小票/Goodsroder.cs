using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Goodsroder
    {
        /// <summary>
        /// 条形码
        /// </summary>
        private string barcode;

        /// <summary>
        /// 商品名称
        /// </summary>
        private string name;

        /// <summary>
        /// 原价
        /// </summary>
        private double original_price;

        /// <summary>
        /// 数量
        /// </summary>
        private int num;

        /// <summary>
        /// 销售价
        /// </summary>
        private double sale_price;

        /// <summary>
        /// 小计
        /// </summary>
        private double sum;

        private int goodsstoreid;
        public string Barcode { get => barcode; set => barcode = value; }
        public string Name { get => name; set => name = value; }
        public int Num { get => num; set => num = value; }
        public double Sale_price { get => sale_price; set => sale_price = value; }
        public double Original_price { get => original_price; set => original_price = value; }
        public double Sum { get => sum; set => sum = value; }
        //public int Goods_id { get => goods_id; set => goods_id = value; }
        public int GoodsStoreId { get => goodsstoreid; set => goodsstoreid = value; }
    }
}
