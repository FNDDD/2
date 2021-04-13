using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
    public class GuaDanObj
    {
        private List<Goodsroder> listGoodsGuaDan;
        private DateTime dateTime;
        private string code;
        public List<Goodsroder> ListGoodsGuaDan { get => listGoodsGuaDan; set => listGoodsGuaDan = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string Code { get => code; set => code = value; }
    }
}
