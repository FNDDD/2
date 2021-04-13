using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    /// <summary>
    /// 商品类型
    /// </summary>
   public class Goods_Class
    {
        int id;
        string name;
        int parentId;

        /// <summary>
        /// 类型id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int ParentId { get => parentId; set => parentId = value; }
    }
}
