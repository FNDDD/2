using Client;
using Client.Entity;
using Client.收银小票;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Printer_1.打印机
{
    public class Printer
    {
        /// <summary>
        /// 打印机句柄
        /// </summary>
        public static int fs = -1;

        /// <summary>
        /// USB链接打印接
        /// </summary>
        /// <returns></returns>
        [DllImport("JsPrinterDll_64.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static int uniOpenUsb();

        /// <summary>
        /// 打印数据
        /// </summary>
        /// <param name="fs">链接句柄</param>
        /// <param name="SendBuf">数据</param>
        /// <param name="SendBufSize">数据大小</param>
        /// <returns></returns>
        [DllImport("JsPrinterDll_64.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static int uniWrite(int fs, byte[] SendBuf, int SendBufSize);

        /// <summary>
        /// 关闭链接
        /// </summary>
        /// <param name="fs">打印机句柄</param>
        /// <returns></returns>
        [DllImport("JsPrinterDll_64.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static bool uniClose(int fs);

        /// <summary>
        /// 打印图片
        /// </summary>
        /// <param name="fs">打印机句柄</param>
        /// <param name="ImagePath">图片地址</param>
        /// <returns></returns>
        [DllImport("JsPrinterDll_64.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static bool uniPrintImg1b2a(int fs, string ImagePath);

        [DllImport("JsPrinterDll_64.dll", CallingConvention = CallingConvention.Winapi)]
        public extern static bool uniselectAlignment(int fs, int n);
        //int _stdcall uniselectAlignment(int fs, int n)
        //{
        //    char data[] = { 0x1B, 0x61, (BYTE)n };
        //    return uniWrite(fs, data, sizeof(data));
        //}

        /// <summary>
        /// 设置居中
        /// </summary>
        /// <returns></returns>
        public static int SetCenter()
        {
            int ReNum = -1;           
            byte[] data = new byte[] { 0x1B, 0x61, 0x01 };
            int nret = uniWrite(fs, data, data.Length);
            return ReNum;
        }

        /// <summary>
        /// 设置左对齐
        /// </summary>
        /// <returns></returns>
        public static int SetLeft()
        {
            int ReNum = -1;
            byte[] data = new byte[] { 0x1B, 0x61, 0x00 };
            int nret = uniWrite(fs, data, data.Length);
            return ReNum;
        }


        public static int uniprintQRcode(String code)
        {
            
            byte[] b = System.Text.Encoding.Default.GetBytes(code);
            int a = b.Length;
            int nL = 0, nH = 0;
            if (a <= 255)
            {
                nL = a;
                nH = 0;
            }
            else
            {
                nH = a / 256;
                nL = a % 256;
            }
            byte[] data = { 0x1D, 0x28, 0x6B, 0x30, 0x67, (byte)4, 0x1D, 0x28, 0x6B, 0x30, 0x69, (byte)51, 0x1D, 0x28, 0x6B, 0x30, (byte)0x80, (byte)nL, (byte)nH };
            data = byteMerger(data, b);
            byte[] c = { 0x1D, 0x28, 0x6B, 0x30, (byte)0x81 };
            data = byteMerger(data, c);
            return uniWrite(fs, data, data.Length);


            
        }

        private static byte[] byteMerger(byte[] data, byte[] b)
        {
            byte[] ND = new byte[data.Length + b.Length];
            try
            {
                for (int i = 0; i < ND.Length; i++)
                {
                    if (i < data.Length)
                    {
                        ND[i] = data[i];
                    }
                    else
                    {
                        ND[i] = b[i - data.Length];
                    }


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            return ND;
        }

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <returns></returns>
        public static int SetHeightWidth(byte x)
        {
            int ReNum = -1;
            //设置高宽
            //byte[] data = new byte[] { 0x1D, 0x21 ,0xff};

            //设置加粗
            //byte[] data = new byte[] { 0x1B, 0x45, 0x00};


            //标准字体压缩字体 0x00，0x30标准字体；0x01，0x31压缩字体
            // byte[] data = new byte[] { 0x1B, 0x4D, 0x01 };

            //训责或取消汉字倍高或倍宽     可用
            byte[] data = new byte[] { 0x1C, 0x57, x };
      
            int nret = uniWrite(fs, data, data.Length);
            return ReNum;
        }

        /// <summary>
        /// 打印一行
        /// </summary>
        /// <param name="Str">要打印的内容</param>
        /// <returns>-1 为未连接到打印机  -2为打印失败</returns>
        public static int Write(string Str)
        {
            int ReNum = -1;
            if (fs > 0)
            {
                ReNum = 1;
                byte[] inStr = System.Text.Encoding.Default.GetBytes(Str);
                // inStr[inStr.Length - 1] = 0x0a;
                byte[] newArray = new byte[inStr.Length + 1];
                inStr.CopyTo(newArray, 0);
                newArray[newArray.Length - 1] = 0x0a;

                int nret = uniWrite(fs, newArray, newArray.Length);
                if (nret < 0)
                {
                    ReNum = -2;

                }
            }
            return ReNum;
        }

        /// <summary>
        /// 连接打印机
        /// </summary>
        /// <returns>大于0 连接成功   小于0连接失败</returns>
        public static int InitPrint()
        { 
            fs = uniOpenUsb();
            return fs;
        }

        /// <summary>
        /// 关闭打印机连接
        /// </summary>
        /// <returns></returns>
        public static bool Close()
        {
            return uniClose(fs);
        }

        /// <summary>
        /// 打印图片
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static bool PrintImg(string Path)
        {
            bool x = false;
            byte[] inStr = System.Text.Encoding.Default.GetBytes(Path);
            x = uniPrintImg1b2a(fs, Path);
            return x;
        }

        /// <summary>
        /// 便利店连锁名称
        /// </summary>
       public static String Name = "供销连锁便利店";

        /// <summary>
        /// 分店名称
        /// </summary>
       public static String BranchName = ConfigurationManager.AppSettings["StoreName"];

        /// <summary>
        /// 二维码地址
        /// </summary>
        public static String PicPath = @"D:\PIC\3.png";

        /// <summary>
        /// 打印收银小票
        /// </summary>
        /// <param name="list">商品列表</param>
        /// <param name="Cashier">收银员</param>
        /// <param name="SerialNumber">流水号</param>
        /// <param name="Sum">总金额</param>
        /// <param name="PayType">支付类型</param>
        /// <returns></returns>
        public static bool WriteObject(List<Goodsroder> list,String Cashier,String SerialNumber,double Sum,String PayType)
        {
            bool ReNum = true;
            try
            {
                Printer.SetHeightWidth(Convert.ToByte(1));
                SetCenter();
                Printer.Write(Name);
                Printer.Write( BranchName);
                SetLeft();
                Printer.SetHeightWidth(Convert.ToByte(0));

                Printer.Write("收银员:" + Cashier);
                Printer.Write("流水号:" + SerialNumber);
                Printer.Write("打印时间 ： " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                Printer.Write("商品名称   单价    数量    小计  ");       
                Printer.Write("------------------------------");
                foreach (Goodsroder orderGoods in list)
                {
                    //Goodsroder orderGoods = (Goodsroder)obj;
                    String Namer = orderGoods.Name;
                    while (Namer.Length > 0)
                    {
                        if (Namer.Length > 5)
                        {
                            Printer.Write(GetString(ref Namer));
                        }
                        else
                        {
                            Printer.Write(Namer + GetSpac(Namer.Length) + orderGoods.Sale_price+ "      " + orderGoods.Num + "      " + orderGoods.Sum);
                            Namer = "";
                            Printer.Write("-----");
                        }
                    }
                }
                Printer.Write("总价：  " + Sum.ToString());
                Printer.Write("应收：  " + Sum.ToString());
                Printer.Write("实收    " + Sum.ToString());
                Printer.Write("支付方式：  " + PayType);
                Printer.Write("      微信扫一扫关注我们");
               
                Printer.PrintImg(PicPath);
                Printer.Write("");
                Printer.Write("");
                Printer.Write("");
            }
            catch (Exception ex)
            {

                ReNum = false;
            }
            return ReNum;
        }

        public static string GetString(ref string RString)
        {
            string RE = string.Empty;
            string Temp = RString;
            if (RString.Length > 5)
            {
                RE = RString.Substring(0, 5);
                string temp = Temp.Substring(5);
                RString = temp;
                return RE;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 获取空格字符串
        /// </summary>
        /// <param name="index">空格数</param>
        /// <returns></returns>
        public static string GetSpac(int index)
        {
            try
            {
                string Re = "";
                for (int i = 0; i < 9 - index; i++)
                {
                    Re = Re + " ";
                }
                switch (index)
                {
                    case 1:
                        Re = Re + "    ";
                    break;
                    case 2:
                        Re = Re + "   ";
                        break;
                    case 3:
                        Re = Re + "  ";
                    break;
                    case 4:
                        Re = Re + " ";
                        break;
                    case 5:
                        Re = Re + "";
                        break;
                }
                return Re;
            }
            catch (Exception ex)
            {

                return "     ";
            }
        }

        /// <summary>
        /// 打印入库小票
        /// </summary>
        /// <param name="list">入库单</param>
        /// <param name="Cashier">入库人</param>
        /// <param name="SerialNumber">流水号</param>
        /// <param name="Sum">总金额</param>
        /// <param name="PayType">支付类型</param>
        /// <returns></returns>
        public static bool WriteRuKu(List<AddGoodsInfo> list, String Cashier, String SerialNumber, float Sum, String PayType)
        {
            bool ReNum = true;
            try
            {
                Printer.SetHeightWidth(Convert.ToByte(1));
                Printer.Write(Name);
                Printer.Write("--" + BranchName);
                Printer.SetHeightWidth(Convert.ToByte(0));
                Printer.Write("收货人 ： " + Cashier);
                Printer.Write("流水号 ： " + SerialNumber);
                Printer.Write("打印时间 ： " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                Printer.Write(" 商品名称   分类    售价    数量  ");
                Printer.Write("------------------------------");
                foreach (AddGoodsInfo orderGoods in list)
                {
                    //Goodsroder orderGoods = (Goodsroder)obj;
                    String Namer = orderGoods.Name;
                    while (Namer.Length > 0)
                    {
                        if (Namer.Length > 5)
                        {
                            Printer.Write(GetString(ref Namer));
                        }
                        else
                        {
                           Printer.Write(Namer + GetSpac(Namer.Length) + orderGoods.Class_gd.ToString() + "      " + orderGoods.Sale_price + "      " + orderGoods.Num);
                            Namer = "";
                            Printer.Write("-----");
                        }
                    }
                }
                Printer.Write(" 总售价：  " + Sum.ToString());
                Printer.Write(" 总数量：  " + Sum.ToString());
                Printer.Write(" 实收    " + Sum.ToString());
                Printer.Write("      微信扫一扫关注我们");

                Printer.PrintImg(PicPath);
                Printer.Write("");
                Printer.Write("");
                Printer.Write("");
            }
            catch (Exception ex)
            {

                ReNum = false;
            }
            return ReNum;
        }


        /// <summary>
        /// 打印交接班统计
        /// </summary>
        /// <param name="XSNum">销售笔数</param>
        /// <param name="XSSum">销售总额</param>
        /// <param name="XJNum">现金笔数</param>
        /// <param name="XJSum">现金总额</param>
        /// <param name="YHNum">优惠券笔数</param>
        /// <param name="YHSum">优惠券总额</param>
        /// <param name="VIPNum">会员优惠笔数</param>
        /// <param name="VIPSum">会员优惠总额</param>
        /// <param name="GDNum">挂单笔数（未完成）</param>
        /// <param name="GDSum">挂单总额（未完成）</param>
        /// <param name="GDWCNum">挂单笔数（完成）</param>
        /// <param name="GDWCSum">挂单总额（完成）</param>
        /// <returns></returns>
        public static bool WriteJiaoJieBan(String XSNum, String XSSum, String XJNum, String XJSum, String YHNum,String YHSum, String VIPNum, String VIPSum, String GDNum, String GDSum, String GDWCNum, String GDWCSum,String UserName)
        {
            bool ReNum = true;
            try
            {
                Printer.SetHeightWidth(Convert.ToByte(1));
                Printer.Write(Name);
                Printer.Write("--" + BranchName);
                Printer.SetHeightWidth(Convert.ToByte(0));

                Printer.Write("------------------------------");
                Printer.Write("打印时间 ： " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                Printer.Write("应收现金   "+ XJNum + "笔   共"+ XJSum + "元");
                Printer.Write("销售总额   " + XSNum + "笔   共" + XSSum + "元");
                Printer.Write("促销优惠   " + (Convert.ToInt32( YHNum)+ Convert.ToInt32(VIPNum)) + "笔   共" + (Convert.ToDouble(YHSum )+ Convert.ToDouble(VIPSum)) + "元");
                Printer.Write("   优惠券优惠" + YHNum + "笔   共" + YHSum + "元");
                Printer.Write("   会员优惠" + VIPNum + "笔   共" + VIPSum + "元");
                Printer.Write("挂单统计   " + (Convert.ToInt32(GDNum) + Convert.ToInt32(GDWCNum)) + "笔   共" + (Convert.ToDouble(GDSum)+ Convert.ToDouble(GDWCSum)) + "元");
                Printer.Write("   完成挂单" + GDWCNum + "笔   共" + GDWCSum + "元");
                Printer.Write("   未完成挂单" + GDNum + "笔   共" + GDSum + "元");
                Printer.Write("------------------------------");
                Printer.Write("营业员： "+UserName);
                Printer.Write("");
                Printer.Write("");
                Printer.Write("");
            }
            catch (Exception ex)
            {

                ReNum = false;
            }
            return ReNum;
        }

        /// <summary>
        /// 打印销售报表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Num"></param>
        /// <param name="Sum"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool WriteGoodsBaoBiao(List<SalesReport> list, String Num, String Sum,  String UserName)
        {
            bool ReNum = true;
            try
            {
                Printer.SetHeightWidth(Convert.ToByte(1));
                Printer.Write(Name);
                Printer.Write("--" + BranchName);
                Printer.SetHeightWidth(Convert.ToByte(0));
                Printer.Write("收银员 ： " + UserName);
                Printer.Write("打印时间 ： " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                Printer.Write(" 商品名称   分类    数量    小计  ");
                Printer.Write("------------------------------");
                foreach (SalesReport orderGoods in list)
                {
                    String Namer = orderGoods.Name;
                    while (Namer.Length > 0)
                    {
                        if (Namer.Length > 5)
                        {
                            Printer.Write(GetString(ref Namer));
                        }
                        else
                        {
                            Printer.Write(Namer + GetSpac(Namer.Length) + orderGoods.ClassName + "      " + orderGoods.Num + "      " + orderGoods.Sum);
                            Namer = "";
                            Printer.Write("-----");
                        }
                    }
                }
                Printer.Write(" 总数：  " + Sum.ToString());
                Printer.Write(" 总价：  " + Sum.ToString());
                Printer.Write("");
                Printer.Write("");
                Printer.Write("");
            }
            catch (Exception ex)
            {

                ReNum = false;
            }
            return ReNum;
        }


    }
}
