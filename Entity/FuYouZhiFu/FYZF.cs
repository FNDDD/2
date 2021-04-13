using Client.Http;
using Client.JiaJie;
using Client.JiaJieMi;
using Client.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity.FuYouZhiFu
{
    public class FYZF
    {
        /// <summary>
        /// 密匙
        /// </summary>
        public static String privateKey = ConfigurationManager.AppSettings["PRIVATE_KEY"];// "";

        /// <summary>
        /// 商户号
        /// </summary>
        public static String SHH = ConfigurationManager.AppSettings["FYSHH"];// "";

        /// <summary>
        /// IP
        /// </summary>
        public static String IP = ConfigurationManager.AppSettings["IP"];// "10.0.0.30";

        /// <summary>
        /// URL
        /// </summary>
        public static String URL = ConfigurationManager.AppSettings["FYJKDZ"];// "https://aipaytest.fuioupay.com/aggregatePay/micropay";

        /// <summary>
        /// 退款URL
        /// </summary>
        public static String ReURL = ConfigurationManager.AppSettings["TKFYJKDZ"];// "https://aipaytest.fuioupay.com/aggregatePay/micropay";


        private static String[] hexDigits = { "g", "h", "i", "j", "k", "l",
            "m", "n", "o", "p", "a", "b", "c", "d", "e", "f" };

        public FYZF()
        {

            //MicroPayDataReq req = new MicroPayDataReq();
            //req.setVersion("1.0");
            //req.setMchnt_cd(SHH);
            //req.setRandom_str("201707140000015");
            //req.setOrder_type("WECHAT");
            //req.setOrder_amt("1");
            //req.setMchnt_order_no("200000000000007");//不能重复
            //req.setTxn_begin_ts(DateTime.Now.ToString("yyyyMMddHHmmss"));
            ////req.setTxn_begin_ts("20210331135418");
            //req.setGoods_des("测试");
            //req.setTerm_id("88888888");
            //req.setTerm_ip("10.0.0.30");
            //req.setAuth_code("136446403167183421");
            //req.setAddn_inf("hyjfservice18217072673");
            //StringBuilder sb = new StringBuilder();
            //sb.Append(req.getMchnt_cd()).Append("|").Append(req.getOrder_type()).Append("|")
            //        .Append(req.getOrder_amt()).Append("|").Append(req.getMchnt_order_no()).Append("|")
            //        .Append(req.getTxn_begin_ts()).Append("|").Append(req.getGoods_des()).Append("|").Append(req.getTerm_id())
            //        .Append("|").Append(req.getTerm_ip()).Append("|" + req.getAuth_code()).Append("|").Append(req.getRandom_str())
            //        .Append("|").Append(req.getVersion()).Append("|").Append(privateKey);
            ////System.out.println("请求sign：" + sb.toString());
            //req.setSign(MakeSign.GetSign(sb.ToString()).ToLower());
            //string str = HttpTool.FYdoHttpPost("https://aipay.fuioupay.com/aggregatePay/micropay", req.ToJSON());

            MicroPayDataReq req = new MicroPayDataReq();
            req.Version = "1.0";//版本号
            req.Mchnt_cd = "0002900F0313432";//商户号
            req.Random_str = "201707140000015";//随机字符
            req.Order_type = "ALIPAY";//支付方式
            req.Order_amt = "1";//支付金额
            req.Mchnt_order_no = "200000000000006";//不能重复
            req.Txn_begin_ts = "20210331135418";
            //req.Txn_begin_ts("20210331135418");
            req.Goods_des = "测试";//商品或支付单简要描述
            req.Term_id = "88888888";
            req.Term_ip = "192.168.8.8";//IP地址
            req.Auth_code = "4545465465";//条码
            req.Addn_inf = "hyjfservice18217072673";
            StringBuilder sb = new StringBuilder();
            sb.Append(req.Mchnt_cd).Append("|").Append(req.Order_type).Append("|")
                    .Append(req.Order_amt).Append("|").Append(req.Mchnt_order_no).Append("|")
                    .Append(req.Txn_begin_ts).Append("|").Append(req.Goods_des).Append("|").Append(req.Term_id)
                    .Append("|").Append(req.Term_ip).Append("|" + req.Auth_code).Append("|").Append(req.Random_str)
                    .Append("|").Append(req.Version).Append("|").Append("f00dac5077ea11e754e14c9541bc0170");
            //System.out.println("请求sign：" + sb.toString());
            req.Sign = MakeSign.GetSign(sb.ToString()).ToLower();
            string ss = req.ToJSON();
            string str = HttpTool.FYdoHttpPost("https://aipaytest.fuioupay.com/aggregatePay/micropay", req.ToJSON());
        }

        public static void testpay()
        {
            MicroPayDataReq req = new MicroPayDataReq();
            req.Version = "1.0";//版本号
            req.Mchnt_cd = "0002900F0313432";//商户号
            req.Random_str = "201707140000015";//随机字符
            req.Order_type = "ALIPAY";//支付方式
            req.Order_amt = "1";//支付金额
            req.Mchnt_order_no = Tool_Somthing.GetOrderNumber();//不能重复
            req.Txn_begin_ts = DateTime.Now.ToString("yyyyMMddHHmmss");
            //req.Txn_begin_ts("20210331135418");
            req.Goods_des = "测试";//商品或支付单简要描述
            req.Term_id = "88888888";
            req.Term_ip = "192.168.8.8";//IP地址
            req.Auth_code = "4545465465";//条码
            req.Addn_inf = "hyjfservice18217072673";
            StringBuilder sb = new StringBuilder();
            sb.Append(req.Mchnt_cd).Append("|").Append(req.Order_type).Append("|")
                    .Append(req.Order_amt).Append("|").Append(req.Mchnt_order_no).Append("|")
                    .Append(req.Txn_begin_ts).Append("|").Append(req.Goods_des).Append("|").Append(req.Term_id)
                    .Append("|").Append(req.Term_ip).Append("|" + req.Auth_code).Append("|").Append(req.Random_str)
                    .Append("|").Append(req.Version).Append("|").Append("f00dac5077ea11e754e14c9541bc0170");
            //System.out.println("请求sign：" + sb.toString());
            req.Sign = MakeSign.GetSign(sb.ToString()).ToLower();
            string ss = req.ToJSON();
            string str = HttpTool.FYdoHttpPost("https://aipaytest.fuioupay.com/aggregatePay/micropay", req.ToJSON());
            string sss = "";
        }


        public static int Pay(string ordercode,string price,string paytype,string paycode,string des)
        {
            int renum = -1;

            MicroPayDataReq req = new MicroPayDataReq();
            req.Version="1.0";//版本号
            req.Mchnt_cd = SHH ;//商户号
            req.Random_str=GetRandom();//随机字符
            req.Order_type=paytype;//支付方式
            req.Order_amt=price;//支付金额
            req.Mchnt_order_no= ordercode;//不能重复
            req.Txn_begin_ts=DateTime.Now.ToString("yyyyMMddHHmmss");
            //req.Txn_begin_ts("20210331135418");
            req.Goods_des=des;//商品或支付单简要描述
            req.Term_id=GetNumZMRandom();
            req.Term_ip=IP;//IP地址
            req.Auth_code=paycode;//条码
            req.Addn_inf="";
            StringBuilder sb = new StringBuilder();
            sb.Append(req.Mchnt_cd).Append("|").Append(req.Order_type).Append("|")
                    .Append(req.Order_amt).Append("|").Append(req.Mchnt_order_no).Append("|")
                    .Append(req.Txn_begin_ts).Append("|").Append(req.Goods_des).Append("|").Append(req.Term_id)
                    .Append("|").Append(req.Term_ip).Append("|" + req.Auth_code).Append("|").Append(req.Random_str)
                    .Append("|").Append(req.Version).Append("|").Append(privateKey);
            req.Sign=MakeSign.GetSign(sb.ToString()).ToLower();
           string json = req.ToJSON();
           string str = HttpTool.FYdoHttpPost(URL, json);
            FYZF_Response resp = JsonTool.FromJSON<FYZF_Response>(str);
            try
            {
                Log_Local.LOG_FYZF(MainWin.user.User_id.ToString(), json, str, resp.Result_code);
            }
            catch (Exception ex)
            {

            }
            switch (resp.Result_code)
            {
                case "000000"://支付成功
                    renum = 1;
                    break;
                case "010001"://支付中
                    renum = 2;
                    break;
                case "030010"://支付中
                    renum = 2;
                    break;
                case "010002"://支付中
                    renum = 2;
                    break;
                case "9999"://支付中
                    renum = 2;
                    break;
                default://其他
                    renum = -2;
                    break;
            }
            return renum;
        }
        public static Random random = new Random();


        public static string GetRandom()
        {
            string re = string.Empty;
            
            for (int i=0;i<16;i++) { re = re+ random.Next(0, 9).ToString(); }
            
            return re;
        }

        public static string GetNumZMRandom()
        {
            string re = string.Empty;

            for (int i = 0; i < 4; i++) {
                re = re + random.Next(0, 9).ToString();
                re = re + hexDigits[random.Next(0, hexDigits.Length - 1)];
            }

            return re;
        }


        public static int BackPay(string ordercode, string price, string paytype, string Backpaytype, string time,string userid)
        {
            int renum = -1;

            MicroBackPayDataReq req = new MicroBackPayDataReq();
            req.Version = "1.0";//版本号
            req.Mchnt_cd = SHH;//商户号
            req.Term_id = GetNumZMRandom();
            req.Random_str = GetRandom();//随机字符
            req.Mchnt_order_no = ordercode;//不能重复
            req.Refund_order_no = ordercode + "R";
            req.Order_type = paytype;//支付方式
            req.Total_amt = price;//支付金额
            req.Refund_amt = Backpaytype;
            req.Operator_id = userid;
            req.Reserved_fy_term_id = "";
            req.Reserved_origi_dt = time;
            //操作员
            //富有终端号
            //原交易日期

            StringBuilder sb = new StringBuilder();
            sb.Append(req.Mchnt_cd).Append("|").Append(req.Order_type).Append("|")
                    .Append(req.Mchnt_order_no).Append("|").Append(req.Refund_order_no).Append("|")
                    .Append(req.Total_amt).Append("|").Append(req.Refund_amt).Append("|").Append(req.Term_id)
                    .Append("|").Append(req.Random_str).Append("|" + req.Version).Append("|").Append(privateKey)
                    ;
            req.Sign = MakeSign.GetSign(sb.ToString()).ToLower();
            string json = req.ToJSON();
            string str = HttpTool.FYdoHttpPost(ReURL, json);
            FYZF_Response resp = JsonTool.FromJSON<FYZF_Response>(str);
            try
            {
                Log_Local.LOG_FYZF(MainWin.user.User_id.ToString(), json, str, resp.Result_code);
            }
            catch (Exception ex)
            {

            }
            switch (resp.Result_code)
            {
                case "000000"://支付成功
                    renum = 1;
                    break;
                case "030015"://余额不足	
                    renum = 2;
                    break;
                case "030010"://支付中
                    renum = 2;
                    break;
                case "010002"://支付中
                    renum = 2;
                    break;
                case "9999"://支付中
                    renum = 2;
                    break;
                default://其他
                    renum = -2;
                    break;
            }
            return renum;
        }

    }
}
