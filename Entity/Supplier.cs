using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    /// <summary>
    /// 供应商
    /// </summary>
   public class Supplier
    {
        int id;
        string name;
        string phone;
        string trademark;

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        public string Phone { get => phone; set => phone = value; }

        /// <summary>
        /// 供应商品牌
        /// </summary>
        public string Trademark { get => trademark; set => trademark = value; }
    }
}
