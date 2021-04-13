using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
   public class SalesReport
    {
        private string name;
        private string className;
        private int num;
        private double sum;

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassName { get => className; set => className = value; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get => num; set => num = value; }

        /// <summary>
        /// 总价
        /// </summary>
        public double Sum { get => sum; set => sum = value; }
    }
}
