using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.JiaJieMi
{
   public class MicroBackPayDataReq
    {
        private String version = "";

        private String mchnt_cd = "";

        private String term_id = "";

        private String random_str = "";

        private String mchnt_order_no = "";

        private String refund_order_no = "";

        private String order_type = "";

        private String total_amt = "";

        private String refund_amt = "";


        private String operator_id = "";

        private String reserved_fy_term_id = "";

        private String reserved_origi_dt = "";

        private String sign;

        public string Version { get => version; set => version = value; }
        public string Mchnt_cd { get => mchnt_cd; set => mchnt_cd = value; }
        public string Term_id { get => term_id; set => term_id = value; }
        public string Random_str { get => random_str; set => random_str = value; }
        public string Mchnt_order_no { get => mchnt_order_no; set => mchnt_order_no = value; }
        public string Refund_order_no { get => refund_order_no; set => refund_order_no = value; }
        public string Order_type { get => order_type; set => order_type = value; }

        public string Refund_amt { get => refund_amt; set => refund_amt = value; }
        public string Operator_id { get => operator_id; set => operator_id = value; }
        public string Reserved_fy_term_id { get => reserved_fy_term_id; set => reserved_fy_term_id = value; }
        public string Reserved_origi_dt { get => reserved_origi_dt; set => reserved_origi_dt = value; }
        public string Sign { get => sign; set => sign = value; }
        public string Total_amt { get => total_amt; set => total_amt = value; }
    }
}
