using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http.ForService
{
   public class AddSmcUser
    {
        private int id;
        private CashUser @object;
        private string serviceType;

        public int Id { get => id; set => id = value; }
        public CashUser Object { get => @object; set => @object = value; }
        public string ServiceType { get => serviceType; set => serviceType = value; }
    }
}
