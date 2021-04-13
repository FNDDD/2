using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    /// <summary>
    /// 入库单明细
    /// </summary>
   public class StockInOrderItem
    {
        private string ordercode;
        private string goodsname;
        private string barcode;
        private string unit;
        private string type;
        private string supplier;
        private double price_in;
        private double price_out;
        private int number;

        /// <summary>
        /// 入库单Id
        /// </summary>
        public string Ordercode { get => ordercode; set => ordercode = value; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Goodsname { get => goodsname; set => goodsname = value; }

        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get => barcode; set => barcode = value; }

        /// <summary>
        /// 商品规格
        /// </summary>
        public string Unit { get => unit; set => unit = value; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string Type { get => type; set => type = value; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get => supplier; set => supplier = value; }

        /// <summary>
        /// 进价
        /// </summary>
        public double Price_in { get => price_in; set => price_in = value; }

        /// <summary>
        /// 售价
        /// </summary>
        public double Price_out { get => price_out; set => price_out = value; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get => number; set => number = value; }
    }
}
