using Client.Entity;
using Client.JiaJie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ActiveMQ
{
   public static class MQTool
   {
        /// <summary>
        /// 拼接要发送的字符串
        /// </summary>
        /// <param name="messageId">消息唯一ID</param>
        /// <param name="deviceId">设备ID</param>
        /// <param name="serviceType">操作类型</param>
        /// <param name="sign">校验</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static string GetMQStr(string messageId,string deviceId,string serviceType,string sign,string data)
        {
            string re = string.Empty;
            string str = "messageId=" + messageId + "&deviceId=" + deviceId + "&serviceType=" + serviceType + "&data=" + data;
            string MD5Str = MakeSign.GetSign(str);
            re = str + "&sign=" + MD5Str;
            return re;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool CheckSign(MessageObj message)
        {
            bool ISOK = false;
            string str = "messageId=" + message.MessageId + "&deviceId=" + message.DeviceId + "&serviceType=" + message.ServiceType + "&data=" + message.Data;
            string MD5Str = MakeSign.GetSign(str);
            if (MD5Str.Equals(message.Sign))
            {
                ISOK = true;
            }
            return ISOK;
        }

        /// <summary>
        /// 解析出对象
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageObj GetObj(string message)
        {
            MessageObj messageObj = new MessageObj();
            try
            {
                string[] a_ = message.Split('&');
                for (int i = 0; i < a_.Length; i++)
                {
                    switch (a_[i])
                    {
                        case "messageId":
                            messageObj.MessageId = a_[i + 1];
                            i++;
                            break;
                        case "deviceId":
                            messageObj.DeviceId = a_[i + 1];
                            i++;
                            break;
                        case "serviceType":
                            messageObj.ServiceType = a_[i + 1];
                            i++;
                            break;
                        case "data":
                            messageObj.Data = a_[i + 1];
                            i++;
                            break;
                        case "sign":
                            messageObj.Sign = a_[i + 1];
                            i++;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;

            }
            return messageObj;
        }
    }
}
