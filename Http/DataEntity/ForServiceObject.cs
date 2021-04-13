using Client.Http.ForService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.DataEntity
{
   public class ForServiceObject
    {
        string msg;
        string code;
        List<SmcGoodsStore> data;

        public string Msg { get => msg; set => msg = value; }
        public string Code { get => code; set => code = value; }
        public List<SmcGoodsStore> Data { get => data; set => data = value; }
    }
}
