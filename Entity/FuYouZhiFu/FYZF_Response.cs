using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entity.FuYouZhiFu
{
   public class FYZF_Response
    {
        //        result_code 必填  String	16	错误代码, 000000 成功,其他详细参见错误列表
        //2	result _msg 必填 String	128	错误代码描述
        //3	ins_cd 必填  String	20	机构号,接入机构在富友的唯一代码
        //4	mchnt_cd 必填  String	15	商户号, 富友分配的商户号
        //5	term_id 非必填 String	8	终端号(没有真实终端号统一填88888888)
        //6	random_str 必填  String	32	随机字符串
        //7	sign 必填  String	512	签名, 详见签名生成算法
        //8	order_type 必填  String	20	订单类型:ALIPAY, WECHAT,UNIONPAY(银联二维码）, BESTPAY(翼支付)
        //9	total_amount 必填  Number  16	订单金额，分为单位的整数
        //10	buyer_id 非必填 String  128	买家在渠道账号
        //11	transaction_id 必填  String  64	渠道交易流水号


        //12	addn_inf 非必填 String  50	附加数据
        //13	reserved_fy_order_no 非必填 String  30	富友生成的订单号, 需要商户与商户订单号进行关联
        //14	reserved_mchnt_order_no 必填  String  30	商户订单号, 商户系统内部的订单号
        //15	reserved_fy_settle_dt 必填  String  8	富友交易日期
        //16	reserved_coupon_fee 非必填 String  10	优惠金额（分）
        //17	reserved_buyer_logon_id 非必填 String  128	买家在渠道登录账号
        //18	reserved_fund_bill_list 非必填 String 不定长 支付宝交易资金渠道, 详细渠道



        //19	reserved_fy_trace_no 必填  String  12	富友系统内部追踪号
        //20	reserved_channel_order_id 非必填 String  64	条码流水号，用户账单二维码对应的流水
        //21	reserved_is_credit 非必填 String  8	1：表示信用卡或者花呗
        //0：表示其他(非信用方式)
        //不填，表示未知
        //22	reserved_txn_fin_ts 非必填 String  14	用户支付时间yyyyMMddHHmmss
        //23	reserved_settlement_amt 非必填 Number  16	应结算订单金额，以分为单位的整数
        //只有成功交易才会返回


        //如果使用了商户免充值优惠券，该值为订单金额-商户免充值
        //如果没有使用商户免充值，该值等于订单金额
        //24	reserved_bank_type 非必填 String  16	付款方式
        //25	reserved_promotion_detail 非必填 String  6000	微信营销详情，见文档中reserved_promotion_detail说明字段



        private string result_code;
        private string result_msg;
        private string ins_cd;
        private string mchnt_cd;
        private string random_str;
        private string sign;
        private string order_type;
        private string total_amount;
        private string transaction_id;
        private string reserved_mchnt_order_no;
        private string reserved_fy_settle_dt;
        private string reserved_fy_trace_no;





        private string term_id;
        private string buyer_id;
        private string addn_inf;
        private string reserved_fy_order_no;
        private string reserved_coupon_fee;
        private string reserved_buyer_logon_id;
        private string reserved_fund_bill_list;
        private string reserved_channel_order_id;
        private string reserved_is_credit;
        private string reserved_txn_fin_ts;
        private string reserved_settlement_amt;
        private string reserved_bank_type;
        private string reserved_promotion_detail;

        /// <summary>
        /// 错误代码, 000000 成功,其他详细参见错误列表
        /// </summary>
        public string Result_code { get => result_code; set => result_code = value; }

        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string Result_msg { get => result_msg; set => result_msg = value; }

        /// <summary>
        /// 机构号,接入机构在富友的唯一代码
        /// </summary>
        public string Ins_cd { get => ins_cd; set => ins_cd = value; }

        /// <summary>
        /// 商户号, 富友分配的商户号
        /// </summary>
        public string Mchnt_cd { get => mchnt_cd; set => mchnt_cd = value; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string Random_str { get => random_str; set => random_str = value; }

        /// <summary>
        /// 签名, 详见签名生成算法
        /// </summary>
        public string Sign { get => sign; set => sign = value; }

        /// <summary>
        /// 订单类型:ALIPAY, WECHAT,UNIONPAY(银联二维码）, BESTPAY(翼支付)
        /// </summary>
        public string Order_type { get => order_type; set => order_type = value; }

        /// <summary>
        /// 订单金额，分为单位的整数
        /// </summary>
        public string Total_amount { get => total_amount; set => total_amount = value; }

        /// <summary>
        /// 渠道交易流水号
        /// </summary>
        public string Transaction_id { get => transaction_id; set => transaction_id = value; }

        /// <summary>
        /// 商户订单号, 商户系统内部的订单号
        /// </summary>
        public string Reserved_mchnt_order_no { get => reserved_mchnt_order_no; set => reserved_mchnt_order_no = value; }

        /// <summary>
        /// 富友交易日期
        /// </summary>
        public string Reserved_fy_settle_dt { get => reserved_fy_settle_dt; set => reserved_fy_settle_dt = value; }

        /// <summary>
        /// 富友系统内部追踪号
        /// </summary>
        public string Reserved_fy_trace_no { get => reserved_fy_trace_no; set => reserved_fy_trace_no = value; }

        /// <summary>
        /// 终端号(没有真实终端号统一填88888888)
        /// </summary>
        public string Term_id { get => term_id; set => term_id = value; }

        /// <summary>
        /// 买家在渠道账号
        /// </summary>
        public string Buyer_id { get => buyer_id; set => buyer_id = value; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string Addn_inf { get => addn_inf; set => addn_inf = value; }

        /// <summary>
        /// 富友生成的订单号, 需要商户与商户订单号进行关联
        /// </summary>
        public string Reserved_fy_order_no { get => reserved_fy_order_no; set => reserved_fy_order_no = value; }

        /// <summary>
        /// 优惠金额（分）
        /// </summary>
        public string Reserved_coupon_fee { get => reserved_coupon_fee; set => reserved_coupon_fee = value; }

        /// <summary>
        /// 买家在渠道登录账号
        /// </summary>
        public string Reserved_buyer_logon_id { get => reserved_buyer_logon_id; set => reserved_buyer_logon_id = value; }

        /// <summary>
        /// 不定长 支付宝交易资金渠道, 详细渠道
        /// </summary>
        public string Reserved_fund_bill_list { get => reserved_fund_bill_list; set => reserved_fund_bill_list = value; }

        /// <summary>
        /// 条码流水号，用户账单二维码对应的流水
        /// </summary>
        public string Reserved_channel_order_id { get => reserved_channel_order_id; set => reserved_channel_order_id = value; }

        /// <summary>
        /// 表示信用卡或者花呗  0：表示其他(非信用方式)
        /// </summary>
        public string Reserved_is_credit { get => reserved_is_credit; set => reserved_is_credit = value; }

        /// <summary>
        /// 用户支付时间yyyyMMddHHmmss
        /// </summary>
        public string Reserved_txn_fin_ts { get => reserved_txn_fin_ts; set => reserved_txn_fin_ts = value; }

        /// <summary>
        /// 应结算订单金额，以分为单位的整数  只有成功交易才会返回
        /// </summary>
        public string Reserved_settlement_amt { get => reserved_settlement_amt; set => reserved_settlement_amt = value; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string Reserved_bank_type { get => reserved_bank_type; set => reserved_bank_type = value; }

        /// <summary>
        /// 微信营销详情，见文档中reserved_promotion_detail说明字段
        /// </summary>
        public string Reserved_promotion_detail { get => reserved_promotion_detail; set => reserved_promotion_detail = value; }
    }
}
