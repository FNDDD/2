using Client.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Http
{
   public class ToServiceOrder
    {

        public ToServiceOrder(string ordercode,int userId,DateTime dateTime,double price, double sum,string payment) {
            Order_code = ordercode;
            time = dateTime;
            user_id = userId;
            all_out_price = price;
            All_goods_sum = sum;
            Pay_ment = payment;
        }
        private string order_code;
        private DateTime time; 
        private int user_id; 
        private double all_out_price; 
        private double all_goods_sum;
        private string pay_ment;       
        private List<Goodsroder> list;

        public string Order_code { get => order_code; set => order_code = value; }
        public DateTime Time { get => time; set => time = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public double All_out_price { get => all_out_price; set => all_out_price = value; }
        public double All_goods_sum { get => all_goods_sum; set => all_goods_sum = value; }
        public string Pay_ment { get => pay_ment; set => pay_ment = value; }
        public List<Goodsroder> List { get => list; set => list = value; }
    }
    public class WaitPayOrder
    {
        private string scanResult;
        private string orderNo;
        private string orderAmount;

        public string ScanResult { get => scanResult; set => scanResult = value; }
        public string OrderNo { get => orderNo; set => orderNo = value; }
        public string OrderAmount { get => orderAmount; set => orderAmount = value; }
    }
}
