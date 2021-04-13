using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.JiaJieMi
{
   public class MicroPayDataReq
    {
        private String version = "";

        private String mchnt_cd = "";

        private String random_str = "";

        private String order_type = "";

        private String order_amt = "";

        private String mchnt_order_no = "";

        private String txn_begin_ts = "";

        private String goods_des = "";

        private String goods_detail = "";

        private String goods_tag = "";

        private String term_id = "";

        private String term_ip = "";

        private String addn_inf = "";

        private String curr_type = "";

        private String auth_code = "";

        private String sence = "";

        private String sign;

        private String reserved_sub_appid;

        private String reserved_limit_pay;

        private String reserved_expire_minute = "0";

        private String reserved_fy_term_id;

        private String ins_cd = "";

        public string Version { get => version; set => version = value; }
        public string Mchnt_cd { get => mchnt_cd; set => mchnt_cd = value; }
        public string Random_str { get => random_str; set => random_str = value; }
        public string Order_type { get => order_type; set => order_type = value; }
        public string Order_amt { get => order_amt; set => order_amt = value; }
        public string Mchnt_order_no { get => mchnt_order_no; set => mchnt_order_no = value; }
        public string Txn_begin_ts { get => txn_begin_ts; set => txn_begin_ts = value; }
        public string Goods_des { get => goods_des; set => goods_des = value; }
        public string Goods_detail { get => goods_detail; set => goods_detail = value; }
        public string Goods_tag { get => goods_tag; set => goods_tag = value; }
        public string Term_id { get => term_id; set => term_id = value; }
        public string Term_ip { get => term_ip; set => term_ip = value; }
        public string Addn_inf { get => addn_inf; set => addn_inf = value; }
        public string Curr_type { get => curr_type; set => curr_type = value; }
        public string Auth_code { get => auth_code; set => auth_code = value; }
        public string Sence { get => sence; set => sence = value; }
        public string Sign { get => sign; set => sign = value; }
        public string Reserved_sub_appid { get => reserved_sub_appid; set => reserved_sub_appid = value; }
        public string Reserved_limit_pay { get => reserved_limit_pay; set => reserved_limit_pay = value; }
        public string Reserved_expire_minute { get => reserved_expire_minute; set => reserved_expire_minute = value; }
        public string Reserved_fy_term_id { get => reserved_fy_term_id; set => reserved_fy_term_id = value; }
        public string Ins_cd { get => ins_cd; set => ins_cd = value; }
    }
}
