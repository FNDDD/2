using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Tool
{
   public class Tool_Somthing
    {
      /// <summary>
      /// 门店设备ID
      /// </summary>
      public static string DeviceId = ConfigurationManager.AppSettings["DeviceId"];
        public static string QianZhui = ConfigurationManager.AppSettings["DHQZ"];
        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
           
            string Restr = string.Empty;
            Restr = QianZhui+ "DOGS" + MainWin.DEVICE_ID;
            Restr = Restr + DateTime.Now.ToString("yyMMddhhmmss");
            return Restr;
        }

        /// <summary>
        /// 获取消息唯一id
        /// </summary>
        /// <returns></returns>
        public static string GetMessageId()
        {
            string Restr = string.Empty;
            Restr = DeviceId;
            Restr = Restr + DateTime.Now.ToString("yyMMddhhmmss");
            return Restr;
        }


    }
}
