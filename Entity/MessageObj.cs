using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity
{
   public class MessageObj
    {
        private string messageId;
        private string deviceId;
        private string serviceType;
        private string sign;
        private string data;

        public string MessageId { get => messageId; set => messageId = value; }
        public string DeviceId { get => deviceId; set => deviceId = value; }
        public string ServiceType { get => serviceType; set => serviceType = value; }
        public string Sign { get => sign; set => sign = value; }
        public string Data { get => data; set => data = value; }
    }
}
