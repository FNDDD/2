using Senparc.Weixin.MP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPayAPI;

namespace Client.微信支付
{
    class WXZF
    {

        WxPayData REdata = new WxPayData();

        /// <summary>
        /// 直接调用支付
        /// </summary>
        public void Pay()
        {
            //string s= WxPayConfig.GetConfig().GetAppID();
            // microPay = new MicroPay();

            MicroPay.Run("测试付款","0.01","");
            //Register.RegisterMpAccount(Senparc.CO2NET.RegisterServices.IRegisterService);
            //WxPayApi.Refund();
        }


    }
}
