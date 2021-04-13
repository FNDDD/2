using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http
{
   public class ToServiceRequest
    {
        private string deviceId;
        private int type;
        private Object data;


        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get => type; set => type = value; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get => data; set => data = value; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string DeviceId { get => deviceId; set => deviceId = value; }
    }
}
