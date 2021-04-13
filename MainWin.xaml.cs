using Apache.NMS;
using Client.ActiveMQ;
using Client.Entity;
using Client.Entity.FuYouZhiFu;
using Client.Http;
using Client.Http.DataEntity;
using Client.Http.ForService;
using Client.JiaJie;
using Client.SelfKJ;
using Client.TiShi;
using Client.Tool;
using Client.收银小票;
using Client.用户;
using Microsoft.Win32;
using Printer_1.打印机;
using Printer_1.扫码枪;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WxPayAPI;
using static Client.MainWindow;

namespace Client
{
    /// <summary>
    /// MainWin.xaml 的交互逻辑
    /// </summary>
    public partial class MainWin : Window
    {
        public MainWin(User _user)
        {
            InitializeComponent();
            Login_Time = DateTime.Now;
            user = _user;
            listener.ScanerEvent += Listener_ScanerEvent;
            listener.Start();
            Printer.InitPrint();
            mQ = new MQ();
            mQ.InitActiveMQ();
            DEVICE_ID = ConfigurationManager.AppSettings["DeviceId"];
            Main_ZH_Label.Content = user.User_name;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SetValue("Old_Username", _user.User_name);
            MenDianName = ConfigurationManager.AppSettings["StoreName"];
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；

            houMianWindow = new HouMianWindow();
            houMianWindow.Show();

            XXTZWindows.Update += XXTZWindows_Update;
        }

        private void XXTZWindows_Update()
        {
            string str = "ListViewItem_HLTZ";
            switch (str)
            {
                case "ListViewItem_KCYJ":
                    Grid_System_XiaoXi.Visibility = Visibility.Visible;
                    ListViewItem_KCYJ.Background = ListViewItem_YES.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    // ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Visible;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    List<GoodsStore> ts = DataBaseControls.GetKunCunYuJing();
                    XiaoXi_KuCunYuJing.ItemsSource = null;
                    XiaoXi_KuCunYuJing.ItemsSource = ts;
                    break;
                case "ListViewItem_LQYJ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_YES.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
                case "ListViewItem_HLTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_YES.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Visible;
                    List<smc_order> list = DataBaseControls.GetOnlineOrder();
                    XiaoXi_HuoLiu.ItemsSource = null;
                    XiaoXi_HuoLiu.ItemsSource = list;
                    break;
                case "ListViewItem_HLiuTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_YES.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
            }
        }

        public DateTime Login_Time;

        public delegate void HouMianUpdata(List<Goodsroder> infoList, double tempPrice);

        public static event HouMianUpdata UpdateHM;

        HouMianWindow houMianWindow;

        /// <summary>
        /// 更新时间
        /// </summary>
        System.Timers.Timer t = new System.Timers.Timer(1000);

        /// <summary>
        /// 消息中间件对象
        /// </summary>
        public MQ mQ;

        /// <summary>
        /// 设备ID
        /// </summary>
        public static string DEVICE_ID;

        /// <summary>
        /// 订单商品数量
        /// </summary>
        public static double GoodsAllNum = 0;

        /// <summary>
        /// 加盟门店Or自营
        /// </summary>
        public static string STORE_TYPE = "S";

        public static string MenDianName = "";

        /// <summary>
        /// 门店ID
        /// </summary>
        public static int DEPT_ID = 1;

        /// <summary>
        /// 当前用户
        /// </summary>
        public static User user;

        /// <summary>
        /// 会员信息
        /// </summary>
        public VIPUser vip;

        /// <summary>
        /// 监听扫码枪
        /// </summary>
        private ScanerHook listener = new ScanerHook();

        /// <summary>
        /// 扫码商品集合
        /// </summary>
        List<Goodsroder> infoList = new List<Goodsroder>();

        /// <summary>
        /// 进货集合
        /// </summary>
        List<AddGoodsInfo> addGoodsInfoslist = new List<AddGoodsInfo>();

        /// <summary>
        /// 收款金额
        /// </summary>
        public double PriceSum = 0;

        /// <summary>
        /// 是否准备收款
        /// </summary>
        public bool ISPAY = false;

        /// <summary>
        /// 是否是测试
        /// </summary>
        public bool ISDEBUG = Convert.ToBoolean(ConfigurationManager.AppSettings["ISDEBUG"]);

        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {
            try
            {
                if (codes.Result.Length == 18)//收款
                {
                    int PayType = 0;// 1 微信支付   2支付宝支付  3云闪付 4 其他
                                    //判断支付方式
                    string PAYTYPE = "";
                    Order order = new Order();
                    order.Order_number = Tool_Somthing.GetOrderNumber();
                    int ISOK = -1;
                    if (codes.Result.Length == 18)
                    {
                        if (ISPAY) { PriceSum = tempPriceSum; }
                        if (codes.Result.Substring(0, 2).Equals("28"))//支付宝
                        {
                            PAYTYPE = "支付宝支付";
                            PayType = 2;
                            ISOK = FYZF.Pay(order.Order_number, ((int)(PriceSum * 100)).ToString(), "ALIPAY", codes.Result, MenDianName);
                        }

                        else if (9 < Convert.ToInt16(codes.Result.Substring(0, 2)) && Convert.ToInt16(codes.Result.Substring(0, 2)) < 16)
                        {
                            PayType = 1;
                            PAYTYPE = "微信支付";
                            #region 富有支付
                            ISOK = FYZF.Pay(order.Order_number, ((int)(PriceSum * 100)).ToString(), "WECHAT", codes.Result, MenDianName);
                            #endregion

                            #region WXZF

                            //if (!MicroPay.Run("供销社测试", ((int)(PriceSum*100)).ToString(), codes.Result).Equals("SUCCESS"))
                            //    {
                            //        MessageBoxResult drs = MessageBox.Show("收款失败", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            //        if (drs == MessageBoxResult.OK)
                            //        {
                            //           // ShouKuan_Grid.Visibility = Visibility.Hidden;
                            //        }
                            //        Update("wx");
                            //        ShouKuan_Grid.Visibility = Visibility.Visible;
                            //        return;
                            //        //接口调用失败
                            //    }
                            //    Update("wx");
                            //    ShouKuan_Grid.Visibility = Visibility.Visible;

                            //MessageBoxResult dr = MessageBox.Show("收款成功", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            //if (dr == MessageBoxResult.OK)
                            //{
                            //    ShouKuan_Grid.Visibility = Visibility.Hidden;
                            //}
                            //ShouKuan_Grid.Visibility = Visibility.Hidden; 
                            #endregion
                        }

                        else
                        {
                            //其他付款方式
                        }


                        if (ISOK == 1)//支付成功
                        {
                            string typestr = "";
                            switch (PayType)
                            {
                                case 1://微信支付
                                    order.Pay_ment = "微信支付";
                                    typestr = "wx";
                                    break;
                                case 2://支付宝支付
                                    order.Pay_ment = "支付宝支付";
                                    typestr = "zfb";
                                    break;
                                case 3://云闪付
                                    order.Pay_ment = "云闪付";
                                    break;
                                case 4://
                                    break;
                                default://其他

                                    break;
                            }
                            Update(typestr);
                            ShouKuan_Grid.Visibility = Visibility.Visible;

                            MessageBoxResult dr = MessageBox.Show("收款成功", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            if (dr == MessageBoxResult.OK)
                            {
                                ShouKuan_Grid.Visibility = Visibility.Hidden;
                            }
                            ShouKuan_Grid.Visibility = Visibility.Hidden;
                            ISOK = -1;
                        }
                        else if (ISOK == 2)//等待支付
                        {

                        }
                        else//支付失败
                        {
                            MessageBoxResult drs = MessageBox.Show("收款失败", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            if (drs == MessageBoxResult.OK)
                            {
                                // ShouKuan_Grid.Visibility = Visibility.Hidden;
                            }
                            string typestr = "";
                            switch (PayType)
                            {
                                case 1://微信支付
                                    order.Pay_ment = "微信支付";
                                    typestr = "wx";
                                    break;
                                case 2://支付宝支付
                                    order.Pay_ment = "支付宝支付";
                                    typestr = "zfb";
                                    break;
                                case 3://云闪付
                                    order.Pay_ment = "云闪付";
                                    break;
                                case 4://
                                    break;
                                default://其他

                                    break;
                            }
                            Update(typestr);
                            ShouKuan_Grid.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    else
                    {
                        //付款码不正确
                        return;
                    }

                    //打印小票

                    if (!Printer.WriteObject(infoList, user.User_name, order.Order_number, PriceSum, PAYTYPE))
                    {
                        //打印小票出错支付已成功
                    }


                    //更新库存
                    foreach (Goodsroder ps12 in infoList)
                    {
                        DataBaseControls.UpdataStock(false, ps12.Num, ps12.Barcode.ToString());
                        if (DataBaseControls.GetKunCunYuJing(ps12.Barcode.ToString()))
                        {
                            KCBZWindow kCBZWindow = new KCBZWindow();
                            kCBZWindow.Update += UpdataXiao;
                            kCBZWindow.Show();
                            //MessageBox.Show(ps12.Name+" 库存不足！");
                        }
                    }

                    #region 新增订单

                    switch (PayType)
                    {
                        case 1://微信支付
                            order.Pay_ment = "微信支付";
                            break;
                        case 2://支付宝支付
                            order.Pay_ment = "支付宝支付";
                            break;
                        case 3://云闪付
                            order.Pay_ment = "云闪付";
                            break;
                        case 4://
                            break;
                        default://其他

                            break;
                    }
                    order.Product_all_price = PriceSum;
                    if (ISPAY)
                    {
                        order.Actual_payment = tempPriceSum;
                        order.Coupons_price = PriceSum - tempPriceSum;
                    }
                    else
                    {
                        order.Actual_payment = PriceSum;
                        order.Coupons_price = 0;
                    }

                    order.User_id = user.User_id;
                    order.Update_by = user.User_name;
                    order.Dept_id = DEPT_ID;
                    DataBaseControls.AddOrder(order);
                    #endregion

                    //新增订单详情
                    DataBaseControls.AddOrderInfo(order.Order_number, user.User_id.ToString(), "备注", infoList);

                    #region 更新服务端库存
                    try
                    {
                        ToServiceRequest toServiceRequest = new ToServiceRequest();
                        toServiceRequest.DeviceId = DEVICE_ID;
                        toServiceRequest.Type = 1;
                        ToServiceOrder toServiceOrder = new ToServiceOrder(order.Order_number, user.User_id, DateTime.Now, PriceSum, GoodsAllNum, PayType.ToString());
                        toServiceOrder.List = infoList;
                        toServiceRequest.Data = toServiceOrder;
                        HttpTool.doHttpPost("/cash/report/add_order", toServiceRequest.ToJSON());
                    }
                    catch (Exception ex)
                    {
                        Log_Local.LOG("更新服务端库存", 402, order.Order_number);
                    }
                    #endregion

                    ISPAY = false;
                    OrderGrid.ItemsSource = null;//清除界面商品信息
                    infoList.Clear();//清除商品信息集合
                    PriceSumLabel.Content = 0;
                    GoodsAllNum = 0;
                    GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                    Log_Local.LOG(user.User_id.ToString(), 3, order.Order_number);//记录日志 
                }
                else if (codes.Result.Length == 16)
                {
                    int PayType = 0;// 1 微信支付   2支付宝支付  3云闪付 4 其他
                                    //判断支付方式
                    string PAYTYPE = "会员卡支付";
                    Order order = new Order();
                    order.Order_number = Tool_Somthing.GetOrderNumber();
                    int ISOK = -1;


                    //支付
                    ToServiceRequest toServiceRequests = new ToServiceRequest();
                    toServiceRequests.DeviceId = DEVICE_ID;
                    toServiceRequests.Type = 2;//会员卡支付
                    WaitPayOrder waitPayOrder = new WaitPayOrder();
                    waitPayOrder.OrderNo = order.Order_number;
                    waitPayOrder.OrderAmount = PriceSum.ToString("0.00");
                    waitPayOrder.ScanResult = codes.Result;
                    toServiceRequests.Data = waitPayOrder;
                    JavaScriptSerializer Jss = new JavaScriptSerializer();
                    string rep1 = HttpTool.doHttpPost("/cash/report/wait_pay_order", toServiceRequests.ToJSON());
                    Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(rep1);
                    string code = string.Empty;
                    if (DicText.ContainsKey("code"))
                    {
                        code = DicText["code"].ToString();
                    }
                    if (true) {
                        if (!Printer.WriteObject(infoList, user.User_name, order.Order_number, PriceSum, PAYTYPE))
                        {
                            //打印小票出错支付已成功
                        }


                        //更新库存
                        foreach (Goodsroder ps12 in infoList)
                        {
                            DataBaseControls.UpdataStock(false, ps12.Num, ps12.Barcode.ToString());
                            if (DataBaseControls.GetKunCunYuJing(ps12.Barcode.ToString()))
                            {
                                KCBZWindow kCBZWindow = new KCBZWindow();
                                kCBZWindow.Update += UpdataXiao;
                                kCBZWindow.Show();
                                //MessageBox.Show(ps12.Name+" 库存不足！");
                            }
                        }

                        #region 新增订单

                        order.Pay_ment = "会员卡支付";
                        order.Product_all_price = PriceSum;
                        if (ISPAY)
                        {
                            order.Actual_payment = tempPriceSum;
                            order.Coupons_price = PriceSum - tempPriceSum;
                        }
                        else
                        {
                            order.Actual_payment = PriceSum;
                            order.Coupons_price = 0;
                        }

                        order.User_id = user.User_id;
                        order.Update_by = user.User_name;
                        order.Dept_id = DEPT_ID;
                        DataBaseControls.AddOrder(order);
                        #endregion

                        //新增订单详情
                        DataBaseControls.AddOrderInfo(order.Order_number, user.User_id.ToString(), "备注", infoList);

                        #region 更新服务端库存
                        try
                        {
                            ToServiceRequest toServiceRequest = new ToServiceRequest();
                            toServiceRequest.DeviceId = DEVICE_ID;
                            toServiceRequest.Type = 1;
                            ToServiceOrder toServiceOrder = new ToServiceOrder(order.Order_number, user.User_id, DateTime.Now, PriceSum, GoodsAllNum, PayType == 2 ? "微信支付" : "支付宝支付");
                            toServiceOrder.List = infoList;
                            toServiceRequest.Data = toServiceOrder;
                            HttpTool.doHttpPost("/cash/report/add_order", toServiceRequest.ToJSON());
                        }
                        catch (Exception ex)
                        {
                            Log_Local.LOG("更新服务端库存", 402, order.Order_number);
                        }
                        #endregion

                        ISPAY = false;
                        OrderGrid.ItemsSource = null;//清除界面商品信息
                        infoList.Clear();//清除商品信息集合
                        PriceSumLabel.Content = 0;
                        GoodsAllNum = 0;
                        GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                        Log_Local.LOG(user.User_id.ToString(), 3, order.Order_number);//记录日志 
                        ISGDPAY = false;
                    }
                }
                else//获取商品信息
                {
                    PriceSum = 0;
                    //商品表对象

                    GoodsStore ps1 = DataBaseControls.GetGoodsStoreByBarcode(codes.Result);
                    if (ps1.Barcode != null)
                    {
                        if (Goods_AddSum_Grid.Visibility == Visibility.Hidden)
                        {

                            //收银小票一行对象
                            Goodsroder goodsroder = new Goodsroder();

                            int j = 0;
                            bool remark = true;//是否加入集合
                            foreach (Goodsroder ps12 in infoList)
                            {

                                if (ps12.Barcode.Equals(ps1.Barcode))
                                {
                                    infoList[j].Num = infoList[j].Num + 1;
                                    infoList[j].Sum = infoList[j].Num * ps12.Sale_price;
                                    remark = false;
                                    break;
                                }
                                j++;
                            }
                            if (remark)
                            {

                                goodsroder.Num = 1;
                                goodsroder.GoodsStoreId = ps1.Goods_store_id;
                                goodsroder.Name = ps1.Goods_name;
                                goodsroder.Barcode = ps1.Barcode;
                                goodsroder.Original_price = ps1.Original_price;
                                goodsroder.Sale_price = ps1.Sale_price;
                                goodsroder.Sum = goodsroder.Num * goodsroder.Sale_price;
                                infoList.Add(goodsroder);
                            }
                            remark = true;
                            GoodsAllNum = 0;
                            foreach (Goodsroder ps12 in infoList)
                            {
                                PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                                GoodsAllNum = GoodsAllNum + ps12.Num;
                            }
                            OrderGrid.ItemsSource = null;
                            PriceSumLabel.Content = PriceSum.ToString();//总金额
                            OrderGrid.AutoGenerateColumns = false;
                            OrderGrid.ItemsSource = infoList;
                            GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                            UpdateHM(infoList, PriceSum);
                        }
                        else//进货页面
                        {
                            AddGoodsInfo good_add = new AddGoodsInfo();
                            All_IN_GOODS_SUM = 0;
                            ALL_OUT_PRICE = 0;
                            ALL_IN_PRICE = 0;
                            good_add.Num = 0;
                            good_add.Name = ps1.Goods_name;
                            good_add.Barcode = ps1.Barcode;
                            good_add.Guige = ps1.Unit;
                            good_add.Sale_price = ps1.Sale_price;
                            good_add.Supplier1 = ps1.Supplier_id;
                            good_add.GoodsId = ps1.Goods_id;
                            good_add.Cost_price = ps1.Cost_price;
                            InputWindows input = new InputWindows();
                            input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            input.ShowDialog();
                            if (input.Num != -1)
                            {
                                good_add.Num = (int)input.Num;
                            }
                            else
                            {
                                return;
                            }

                            bool remark = true;//是否加入集合
                            int i = 0;
                            foreach (AddGoodsInfo ps12 in addGoodsInfoslist)
                            {

                                if (ps12.Barcode.Equals(ps1.Barcode))
                                {
                                    addGoodsInfoslist[i].Num = addGoodsInfoslist[i].Num + good_add.Num;
                                    //addGoodsInfoslist[j].Sum = addGoodsInfoslist[j].Num * ps12.Sale_price;
                                    remark = false;
                                    break;
                                }
                                i++;
                            }
                            if (remark)
                            {
                                addGoodsInfoslist.Add(good_add);
                            }

                            AddGoods_DataGrid.ItemsSource = null;
                            AddGoods_DataGrid.AutoGenerateColumns = false;
                            AddGoods_DataGrid.ItemsSource = addGoodsInfoslist;
                            foreach (AddGoodsInfo obj in addGoodsInfoslist)
                            {
                                All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
                                ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
                                ALL_IN_PRICE = ALL_IN_PRICE + obj.Num * obj.Cost_price;
                            }
                            Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                            Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                            Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                        }
                    }
                    else
                    {
                        //未找到商品
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("2"+ ex.ToString());
            }
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateShowNowTime();
            // if (mQ.connection.) { }
        }

        private delegate void ShowNowTime();

        private void UpdateShowNowTime()
        {
            this.NowTime_Label.Dispatcher.BeginInvoke(new ShowNowTime(UpShowNowTime));
        }

        private void UpShowNowTime()
        {
            NowTime_Label.Content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        ///总进价 
        /// </summary>
        public static double ALL_IN_PRICE = 0;

        /// <summary>
        /// 总销售价
        /// </summary>
        public static double ALL_OUT_PRICE = 0;

        /// <summary>
        /// 总进货数
        /// </summary>
        public static Int32 All_IN_GOODS_SUM = 0;

        /// <summary>
        /// 挂单集合
        /// </summary>
        public List<object> ListOrder = new List<object>();

        /// <summary>
        /// 挂单商品信息
        /// </summary>
        public List<Goodsroder> ListGoodsGuaDan = new List<Goodsroder>();

        /// <summary>
        /// 挂单对象集合
        /// </summary>
        public List<GuaDanObj> ListGuaDanOBJ = new List<GuaDanObj>();

        /// <summary>
        /// 付款按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Console.WriteLine("1321123+++++++++");
        }

        private void MainWin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Space))
            {
                // Console.WriteLine("1321123");
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        #region 挂取单
        /// <summary>
        /// 挂单集合
        /// </summary>
        public GuaDanObj listGoodsGuaDan = null;

        /// <summary>
        /// 挂单付款
        /// </summary>
        public static bool ISGDPAY = false;

        /// <summary>
        /// 挂单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_HangingOrder(object sender, RoutedEventArgs e)
        {
            if (infoList.Count > 0)
            {
                GuaDanObj guaDanObj = new GuaDanObj();
                guaDanObj.DateTime = DateTime.Now;
                List<Goodsroder> infoListcopy = new List<Goodsroder>();

                foreach (Goodsroder goodsroder in infoList)
                {
                    if (goodsroder.Barcode != null && !goodsroder.Barcode.Equals(""))
                    {
                        infoListcopy.Add(goodsroder);
                    }
                }
                string code = DateTime.Now.ToString("yyyyMMddhhmmss");
                if (DataBaseControls.AddGuadan(code, PriceSum))
                {
                    DataBaseControls.AddGuadanInfo(code, infoListcopy);
                }

                //guaDanObj.ListGoodsGuaDan = infoListcopy;
                //ListGuaDanOBJ.Add(guaDanObj);
                infoList.Clear();
                OrderGrid.ItemsSource = null;
                OrderGrid.ItemsSource = infoList;
                UpdataNowOrderInfo();
            }
        }



        /// <summary>
        /// 取单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_WithdrawOrder(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            QD_ListView.Items.Clear();
            GetGuaDan();
            QvDan_Grid.Visibility = Visibility.Visible;
        }
        public void GetGuaDan()
        {


            int i = 0;
            ListGuaDanOBJ = DataBaseControls.GetGuaDan();
            GuaDan_DataGrid.ItemsSource = null;
            QvDan_TJ_Label.Content = "共0件 ， 总额 0 元";
            try
            {

                foreach (GuaDanObj gdo in ListGuaDanOBJ)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Content = gdo.Code;
                    listViewItem.FontSize = 28;
                    i++;
                    if (i == 1)
                    {
                        listViewItem.IsSelected = true;
                        GuaDanObj guaDanObj = DataBaseControls.GetGuaDanItem(gdo.Code);
                        listGoodsGuaDan = guaDanObj;
                        UpdataQvDanData(guaDanObj);
                    }
                    listViewItem.Selected += ListViewItem_Selected_QvDan;
                    QD_ListView.Items.Add(listViewItem);

                }
            }
            catch (Exception ex)
            {

            }
        }



        int QvDan_Num = 0;

        double QvDan_Sum = 0;

        private void ListViewItem_Selected_QvDan(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            QvDan_Num = 0;
            QvDan_Sum = 0;
            ListViewItem listViewItem = (ListViewItem)sender;
            GuaDanObj guaDanObj = DataBaseControls.GetGuaDanItem(listViewItem.Content.ToString());
            listGoodsGuaDan = guaDanObj;
            UpdataQvDanData(guaDanObj);
        }

        private void Button_Click_QD_ZBJ(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            if (listGoodsGuaDan != null)
            {

                infoList = listGoodsGuaDan.ListGoodsGuaDan;
                OrderGrid.ItemsSource = null;
                OrderGrid.ItemsSource = infoList;
                //ListGuaDanOBJ.Remove(listGoodsGuaDan);
                // GuaDan_DataGrid.ItemsSource = null;
                DataBaseControls.UpdataGuadan(listGoodsGuaDan.Code, 3);
                QvDan_Grid.Visibility = Visibility.Hidden;
                UpdataNowOrderInfo();

            }
            else
            {
                MessageBox.Show("请选择订单");
            }

        }

        /// <summary>
        /// 更新取单的数据
        /// </summary>
        public void UpdataQvDanData(GuaDanObj guaDanObj)
        {
            QvDan_Num = 0;
            QvDan_Sum = 0;

            GuaDan_DataGrid.ItemsSource = null;
            GuaDan_DataGrid.AutoGenerateColumns = false;
            GuaDan_DataGrid.ItemsSource = guaDanObj.ListGoodsGuaDan;
            foreach (Goodsroder goodsroder in guaDanObj.ListGoodsGuaDan)
            {
                QvDan_Num = QvDan_Num + goodsroder.Num;
                QvDan_Sum = QvDan_Sum + (goodsroder.Num * goodsroder.Sale_price);
            }
            QvDan_TJ_Label.Content = "共" + QvDan_Num.ToString() + "件 ， 总额 " + QvDan_Sum.ToString() + " 元";
        }

        private void Button_Click_QD_ZF(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (listGoodsGuaDan != null)
            {

                DataBaseControls.UpdataGuadan(listGoodsGuaDan.Code, 1);
                QvDan_Grid.Visibility = Visibility.Hidden;
                MessageBox.Show("已作废");
                UpdataNowOrderInfo();

            }
            else
            {
                MessageBox.Show("请选择订单");
            }
            // listGoodsGuaDan = null;    需要再编辑时 恢复删除的上一条  则放开
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Button_Click_QD_SY(object sender, RoutedEventArgs e)
        {
            if (listGoodsGuaDan != null)
            {

                infoList = listGoodsGuaDan.ListGoodsGuaDan;
                OrderGrid.ItemsSource = null;
                OrderGrid.ItemsSource = infoList;
                //ListGuaDanOBJ.Remove(listGoodsGuaDan);
                // GuaDan_DataGrid.ItemsSource = null;
                DataBaseControls.UpdataGuadan(listGoodsGuaDan.Code, 3);
                QvDan_Grid.Visibility = Visibility.Hidden;
                UpdataNowOrderInfo();

            }
            else
            {
                MessageBox.Show("请选择订单");
            }
            ShouKuan_Grid.Visibility = Visibility.Visible;
            PIC_Name = "xj";
            xj.Source = xj_.Source;
            if (PriceSum != 0)
            {
                ShouKan_ShangTextBox.Text = PriceSum.ToString();
                ShouKan_ZhongTextBox.Text = PriceSum.ToString();
                ShouKan_XiaTextBox.Text = "0";
                tempPriceSum = PriceSum;
            }
            else
            {
                ShouKan_ShangTextBox.Text = "";
                ShouKan_XiaTextBox.Text = "";
                ShouKan_ZhongTextBox.Text = "";
                tempPriceSum = PriceSum;
            }
            if (listGoodsGuaDan != null)
            {

                DataBaseControls.UpdataGuadan(listGoodsGuaDan.Code, 2);
            }
        }

        private void Button_Click_QD_DY(object sender, RoutedEventArgs e)
        {
            Printer.WriteObject(listGoodsGuaDan.ListGoodsGuaDan, "", "", (float)QvDan_Sum, "微信支付");
        }
        #endregion

        #region 增加新品
        /// <summary>
        /// 增加新品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewGoods_OK(object sender, RoutedEventArgs e)
        {
            GoodsStore goodsStore = new GoodsStore();
            goodsStore.Goods_name = Add_Good_Name.Text;
            goodsStore.Pinyin_code = Add_Good_PinYin.Text;
            goodsStore.Barcode = Add_Good_Bar_Code.Text;
            goodsStore.Sale_price = Convert.ToDouble(Add_Good_Sale_Price.Text);
            goodsStore.Cost_price = Convert.ToDouble(Add_Good_Cost_Price.Text);
            goodsStore.Inventory_now = Add_Good_Inventory_Now.Text;
            goodsStore.Inventory_min = Add_Good_Inventory_Min.Text;
            if (ComboBox_Goods_Class.SelectedItem != null)
            {
                ListBoxItem item = (ListBoxItem)ComboBox_Goods_Class.SelectedItem;
                Goods_Class goods_Class = (Goods_Class)item.Tag;
                goodsStore.Classify_id = goods_Class.Id;
            }

            if (ComboBox_Goods_Supplier.SelectedItem != null)
            {
                ListBoxItem item = (ListBoxItem)ComboBox_Goods_Supplier.SelectedItem;
                Supplier supplier = (Supplier)item.Tag;
                goodsStore.Supplier_id = supplier.Id;
            }
            goodsStore.Make_date = DateTime.Now;
            goodsStore.Shelf_life = Add_Good_Shelf_Life.Text;
            goodsStore.Remark = Add_Good_Remark.Text;
            goodsStore.Status = "1";
            //if (Add_Good_Status.IsChecked==true)
            //{
            //    goodsStore.Status = "1";
            //}
            //else
            //{
            //    goodsStore.Status = "2";
            //}
            if (DataBaseControls.AddNewGoods(user.User_id.ToString(), goodsStore))
            {
                MessageBox.Show("新增成功");
                Add_Goods_Grid.Visibility = Visibility.Hidden;
            }


        }

        /// <summary>
        /// 增加新品页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_AddNewGoods(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("敬请期待！");
            #region MyRegion
            //if (Add_Goods_Grid.Visibility == Visibility.Visible)
            //{
            //    Add_Goods_Grid.Visibility = Visibility.Hidden;
            //    OrderGrid.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ComboBox_Goods_Class.Items.Clear();
            //    ComboBox_Goods_Supplier.Items.Clear();
            //    List<Goods_Class> goods_Classes = DataBaseControls.GetGoodsClass();
            //    foreach (Goods_Class goods_Class in goods_Classes)
            //    {
            //        ListBoxItem item = new ListBoxItem();
            //        item.Content = goods_Class.Name;
            //        item.Tag = goods_Class;
            //        ComboBox_Goods_Class.Items.Add(item);
            //    }
            //    List<Supplier> goods_Suppliers = DataBaseControls.GetGoodsSupplier();
            //    foreach (Supplier goods_Supplier in goods_Suppliers)
            //    {
            //        ListBoxItem item = new ListBoxItem();
            //        item.Content = goods_Supplier.Name;
            //        item.Tag = goods_Supplier;
            //        ComboBox_Goods_Supplier.Items.Add(item);
            //    }
            //    Main_Border.Height = 766;
            //    Add_Goods_Grid.Visibility = Visibility.Visible;
            //    //OrderGrid.Visibility = Visibility.Hidden;
            //}
            #endregion
        }

        private void AddNewGoods_cancel(object sender, RoutedEventArgs e)
        {
            Main_Border.Height = 0;
            Add_Goods_Grid.Visibility = Visibility.Hidden;

        }

        #endregion

        /// <summary>
        /// 从订单中删除一个商品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {


            Goodsroder goodsroder = (Goodsroder)OrderGrid.SelectedItem;
            infoList.Remove(goodsroder);
            GoodsAllNum = 0;
            PriceSum = 0;
            foreach (Goodsroder ps12 in infoList)
            {
                PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                GoodsAllNum = GoodsAllNum + ps12.Num;
            }
            OrderGrid.ItemsSource = null;
            PriceSumLabel.Content = PriceSum.ToString();//总金额
            OrderGrid.AutoGenerateColumns = false;
            OrderGrid.ItemsSource = infoList;
            GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
        }

        /// <summary>
        /// 更新当前订单相关信息
        /// </summary>
        public void UpdataNowOrderInfo() {
            GoodsAllNum = 0;
            PriceSum = 0;
            foreach (Goodsroder ps12 in infoList)
            {
                PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                GoodsAllNum = GoodsAllNum + ps12.Num;
            }
            OrderGrid.ItemsSource = null;
            PriceSumLabel.Content = PriceSum.ToString();//总金额
            OrderGrid.AutoGenerateColumns = false;
            OrderGrid.ItemsSource = infoList;
            GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
        }

        /// <summary>
        ///当前时间 
        /// </summary>
        DateTime DateTimeNow = DateTime.Now;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Space))
            {
                DateTimeNow = DateTime.Now;
            }
            //if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            //{
            //    Goods_AddSumDateTimeNow = DateTime.Now;
            //    //Console.WriteLine(Goods_AddSumDateTimeNow.ToString());
            //}
        }

        private void MainWin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Space))
            {
                if ((((TimeSpan)(DateTime.Now - DateTimeNow)).TotalMilliseconds > 20) && (((TimeSpan)(DateTime.Now - DateTimeNow)).TotalMilliseconds < 1000))
                {
                    //ISPAY = !ISPAY;
                    if (ShouKuan_Grid.Visibility == Visibility.Hidden)
                    {

                        PIC_Name = "xj";
                        xj.Source = xj_.Source;
                        if (PriceSum != 0)
                        {
                            ShouKan_ShangTextBox.Text = PriceSum.ToString();
                            ShouKan_ZhongTextBox.Text = PriceSum.ToString();
                            ShouKan_XiaTextBox.Text = "0";
                            tempPriceSum = PriceSum;
                        }
                        else
                        {
                            ShouKan_ShangTextBox.Text = "";
                            ShouKan_XiaTextBox.Text = "";
                            ShouKan_ZhongTextBox.Text = "";
                            tempPriceSum = PriceSum;
                        }
                        ShouKuan_Grid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ShouKuan_Grid.Visibility = Visibility.Visible;
                    }

                }
            }
            //if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            //{
            //    Goods_AddSumDateTimeNow_End = DateTime.Now;
            //    //Console.WriteLine(((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds);
            //    if ((((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds > 20) && (((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds < 1000))
            //    {
            //        Console.WriteLine("1");
            //        //foreach (AddGoodsInfo obj in addGoodsInfoslist)
            //        //{
            //        //    All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
            //        //    ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
            //        //    ALL_IN_PRICE = 0;
            //        //}
            //        //Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
            //        //Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
            //        //Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
            //    }
            //}
        }

        #region 进货页面操作
        private void Image_JinHuo(object sender, MouseButtonEventArgs e)
        {
            Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
            Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
            Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
            Main_Border.Height = 766;
            Goods_AddSum_Grid.Visibility = Visibility.Visible;

        }

        private void JinHuoFanHui(object sender, MouseButtonEventArgs e)
        {
            Main_Border.Height = 0;
            Goods_AddSum_Grid.Visibility = Visibility.Hidden;
            All_IN_GOODS_SUM = 0;
            ALL_OUT_PRICE = 0;
            ALL_IN_PRICE = 0;
            AddGoods_DataGrid.ItemsSource = null;//清空进货界面数据
            addGoodsInfoslist.Clear();//清空进货列表
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (All_IN_GOODS_SUM != 0)
            {
                MessageBoxResult dr = MessageBox.Show("是否确认提交！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (dr == MessageBoxResult.OK)
                {
                    try
                    {
                        StockInOrder stockInOrder = new StockInOrder();
                        stockInOrder.All_goods_sum = All_IN_GOODS_SUM;
                        stockInOrder.All_in_price = All_IN_GOODS_SUM;
                        stockInOrder.All_out_price = ALL_OUT_PRICE;
                        stockInOrder.Delivery_by = "送货人";
                        stockInOrder.Delivery_phone = "送货人手机号";
                        stockInOrder.Order_code = "RK" + DateTime.Now.ToString("yyyyMMddhhmmss");
                        stockInOrder.User_id = user.User_id;
                        stockInOrder.Orther_order_code = "FWDOrderCode";

                        //保存入库单信息
                        DataBaseControls.AddStockInOrder(stockInOrder);

                        foreach (AddGoodsInfo ps12 in addGoodsInfoslist)
                        {
                            //更新库存
                            DataBaseControls.UpdataStock(true, ps12.Num, ps12.Barcode.ToString());
                            if (DataBaseControls.GetKunCunYuJing(ps12.Barcode.ToString()))
                            {
                                KCBZWindow kCBZWindow = new KCBZWindow();
                                kCBZWindow.Update += UpdataXiao;
                                kCBZWindow.Show();
                                //MessageBox.Show(ps12.Name + " 库存不足！");
                            }
                        }

                        //保存入库单详情
                        DataBaseControls.AddStockInOrderItem(stockInOrder.Order_code, addGoodsInfoslist);

                        try
                        {
                            ToServiceRequest toServiceRequest = new ToServiceRequest();
                            toServiceRequest.DeviceId = DEVICE_ID;
                            toServiceRequest.Type = 1;
                            ToServiceInStorage inStorage = new ToServiceInStorage(stockInOrder.Order_code, user.User_id, DateTime.Now, stockInOrder.All_in_price, stockInOrder.All_out_price, stockInOrder.All_goods_sum, stockInOrder.Delivery_by, stockInOrder.Delivery_phone, stockInOrder.Remark);
                            inStorage.List = addGoodsInfoslist;
                            toServiceRequest.Data = inStorage;
                            HttpTool.doHttpPost("", toServiceRequest.ToJSON());
                        }
                        catch (Exception ex)
                        {
                            Log_Local.LOG("更新服务端入库", 402, stockInOrder.Order_code);
                        }

                        MessageBoxResult drs = MessageBox.Show("入库成功，在查库存中查看入库数量。", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                        if (drs == MessageBoxResult.OK)
                        {
                            All_IN_GOODS_SUM = 0;
                            ALL_OUT_PRICE = 0;
                            ALL_IN_PRICE = 0;
                            Main_Border.Height = 0;
                            AddGoods_DataGrid.ItemsSource = null;//清空进货界面数据
                            //addGoodsInfoslist.Clear();//清空进货列表
                            addGoodsInfoslist.Clear();//清空进货列表
                            Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                            Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                            Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                        }
                        else
                        {
                            All_IN_GOODS_SUM = 0;
                            ALL_OUT_PRICE = 0;
                            ALL_IN_PRICE = 0;
                            AddGoods_DataGrid.ItemsSource = null;//清空进货界面数据
                            Main_Border.Height = 0;
                            addGoodsInfoslist.Clear();//清空进货列表
                            Goods_AddSum_Grid.Visibility = Visibility.Hidden;
                            Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                            Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                            Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                        Log_Local.LOG("入库错误代码", 101, ex.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("无进货商品！");
            }

        }

        DateTime Goods_AddSumDateTimeNow = DateTime.Now;

        DateTime Goods_AddSumDateTimeNow_End = DateTime.Now;

        private void Goods_AddSum_Grid_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            //{
            //    Goods_AddSumDateTimeNow = DateTime.Now;
            //    Console.WriteLine(Goods_AddSumDateTimeNow.ToString());
            //}
        }

        private void Goods_AddSum_Grid_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter))
            //{
            //    Goods_AddSumDateTimeNow_End = DateTime.Now;
            //    Console.WriteLine(((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds );
            //    if ((((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds > 20) && (((TimeSpan)(Goods_AddSumDateTimeNow_End - Goods_AddSumDateTimeNow)).TotalMilliseconds < 1000))
            //    {

            //        foreach (AddGoodsInfo obj in addGoodsInfoslist)
            //        {
            //            All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
            //            ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
            //            ALL_IN_PRICE = 0;
            //        }
            //        Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
            //        Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
            //        Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
            //    }
            //}
        }

        private void JinHuoShuLiang(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Enter)
                {
                    All_IN_GOODS_SUM = 0;
                    ALL_OUT_PRICE = 0;
                    ALL_IN_PRICE = 0;
                    AddGoods_DataGrid.ItemsSource = null;
                    AddGoods_DataGrid.AutoGenerateColumns = false;
                    AddGoods_DataGrid.ItemsSource = addGoodsInfoslist;
                    foreach (AddGoodsInfo obj in addGoodsInfoslist)
                    {
                        All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
                        ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
                        ALL_IN_PRICE = ALL_IN_PRICE + obj.Num * obj.Cost_price;
                    }
                    Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                    Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                    Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 交接班

        #region 统计数据
        /// <summary>
        /// 应收现金笔数
        /// </summary>
        public static int JJB_CASH_NUM = 0;

        /// <summary>
        /// 总销售笔数
        /// </summary>
        public static int JJB_ALL_ASLE_NUM = 0;

        /// <summary>
        /// 完成的挂单
        /// </summary>
        public static int JJB_GD_DONE_NUM = 0;

        /// <summary>
        /// 未完成的挂单
        /// </summary>
        public static int JJB_GD_NO_DONE_NUM = 0;

        /// <summary>
        /// 优惠券笔数
        /// </summary>
        public static int JJB_COUPON_NUM = 0;

        /// <summary>
        /// 会员优惠笔数
        /// </summary>
        public static int JJB_VIP_NUM = 0;

        /// <summary>
        /// 应收现金数
        /// </summary>
        public static double JJB_CASH_SUM = 0;

        /// <summary>
        /// 总销售数
        /// </summary>
        public static double JJB_ALL_ASLE_SUM = 0;

        /// <summary>
        /// 完成的挂单金额
        /// </summary>
        public static double JJB_GD_DONE_SUM = 0;

        /// <summary>
        /// 未完成的挂单金额
        /// </summary>
        public static double JJB_GD_NO_DONE_SUM = 0;

        /// <summary>
        /// 优惠券金额
        /// </summary>
        public static double JJB_COUPON_SUM = 0;

        /// <summary>
        /// 会员优惠金额
        /// </summary>
        public static double JJB_VIP_SUM = 0;
        #endregion

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear_JJE_Data()
        {
            JJB_CASH_NUM = 0;


            JJB_ALL_ASLE_NUM = 0;


            JJB_GD_DONE_NUM = 0;


            JJB_GD_NO_DONE_NUM = 0;


            JJB_COUPON_NUM = 0;


            JJB_VIP_NUM = 0;


            JJB_CASH_SUM = 0;


            JJB_ALL_ASLE_SUM = 0;


            JJB_GD_DONE_SUM = 0;


            JJB_GD_NO_DONE_SUM = 0;


            JJB_COUPON_SUM = 0;


            JJB_VIP_SUM = 0;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void Updata_JJE_Data()
        {
            if (user.User_type.Equals("1"))
            {
                JJB_CASH_NUM_Label.Content = "*** 笔";
                JJB_CASH_SUM_Label.Content = "共 *** 元";
            }
            else
            {
                JJB_CASH_NUM_Label.Content = JJB_CASH_NUM + "笔";
                JJB_CASH_SUM_Label.Content = "共" + JJB_CASH_SUM.ToString("0.00") + "元";
            }


            JJB_ALL_ASLE_NUM_Lable.Content = JJB_ALL_ASLE_NUM + "笔";


            JJB_GD_DONE_NUM_Lable.Content = JJB_GD_DONE_NUM + "笔";

            JJB_GD_NO_DONE_NUM_Lable.Content = JJB_GD_NO_DONE_NUM + "笔";

            JJB_COUPON_NUM_Label.Content = JJB_COUPON_NUM + "笔";

            JJB_VIP_NUM_Label.Content = JJB_VIP_NUM + "笔";

            JJB_GD_ALL_NUM_Lable.Content = (JJB_GD_NO_DONE_NUM + JJB_GD_DONE_NUM) + "笔";

            JJB_GD_ALL_SUM_Lable.Content = "共" + (JJB_GD_DONE_SUM + JJB_GD_NO_DONE_SUM).ToString("0.00") + "元";





            JJB_ALL_ASLE_SUM_Lable.Content = "共" + JJB_ALL_ASLE_SUM.ToString("0.00") + "元";


            JJB_GD_DONE_SUM_Lable.Content = "共" + JJB_GD_DONE_SUM.ToString("0.00") + "元";


            JJB_GD_NO_DONE_SUM_Lable.Content = "共" + JJB_GD_NO_DONE_SUM.ToString("0.00") + "元";


            JJB_COUPON_SUM_Label.Content = "共" + JJB_COUPON_SUM.ToString("0.00") + "元";


            JJB_VIP_SUM_Label.Content = "共" + JJB_VIP_SUM.ToString("0.00") + "元";

            JJB_YOUHUI_NUM_Label.Content = (JJB_VIP_NUM + JJB_COUPON_NUM) + "笔";

            JJB_YOUHUI_SUM_Label.Content = "共" + (JJB_COUPON_SUM + JJB_VIP_SUM).ToString("0.00") + "元";
        }

        /// <summary>
        /// 交接班
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_JiaoJieBan(object sender, MouseButtonEventArgs e)
        {
            bool mark = false;
            if (user.User_type.Equals("0"))
            {
                mark = true;
            }
            if (JiaoJieBan_Grid.Visibility == Visibility.Hidden)
            {
                JJB_DeptName.Content = ConfigurationManager.AppSettings["StoreName"];
                Main_Border.Height = 766;
                JJB_SYY_Label.Content = user.Nick_name;
                JJB_GH_Label.Content = user.User_name;
                JJB_Login_Time.Content = Login_Time.ToString("yyyy-MM-dd hh:mm:ss") + "----" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string myStr = DataBaseControls.GetProductAllPrice(user.User_id.ToString(), mark);
                char[] separator = { '_' };//'a' a为分隔符，此例为空格分隔符定义一个分隔符数组
                string[] mywords;
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_ALL_ASLE_SUM = Convert.ToDouble(mywords[0]);

                        JJB_ALL_ASLE_NUM = Convert.ToInt32(mywords[1]);
                    }
                }
                catch (Exception ex)
                {

                    JJB_ALL_ASLE_SUM = 0;

                    JJB_ALL_ASLE_NUM = 0;
                }

                myStr = DataBaseControls.GetVIPAllPrice(user.User_id.ToString(), mark);
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_VIP_SUM = Convert.ToDouble(mywords[0]);

                        JJB_VIP_NUM = Convert.ToInt32(mywords[1]);
                    }
                }
                catch (Exception ex)
                {
                    JJB_VIP_SUM = 0;

                    JJB_VIP_NUM = 0;

                }

                myStr = DataBaseControls.GetCouponAllPrice(user.User_id.ToString(), mark);
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_COUPON_SUM = Convert.ToDouble(mywords[0]);

                        JJB_COUPON_NUM = Convert.ToInt32(mywords[1]);
                    }
                }
                catch (Exception ex)
                {

                    JJB_COUPON_SUM = 0;

                    JJB_COUPON_NUM = 0;
                }

                myStr = DataBaseControls.GetGuaDanrealyNum();
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_GD_DONE_SUM = Convert.ToDouble(mywords[1]);

                        JJB_GD_DONE_NUM = Convert.ToInt32(mywords[0]);
                    }
                }
                catch (Exception ex)
                {

                    JJB_GD_DONE_SUM = 0;

                    JJB_GD_DONE_NUM = 0;
                }

                myStr = DataBaseControls.GetGuaDanNotrealyNum();
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_GD_NO_DONE_SUM = Convert.ToDouble(mywords[1]);

                        JJB_GD_NO_DONE_NUM = Convert.ToInt32(mywords[0]);
                    }
                }
                catch (Exception ex)
                {

                    JJB_GD_NO_DONE_SUM = 0;

                    JJB_GD_NO_DONE_NUM = 0;
                }

                myStr = DataBaseControls.GetCasheNum(user.User_id.ToString(), mark);
                try
                {
                    mywords = myStr.Split(separator);
                    if (mywords.Length == 2)
                    {
                        JJB_CASH_SUM = Convert.ToDouble(mywords[1]);

                        JJB_CASH_NUM = Convert.ToInt32(mywords[0]);
                    }
                }
                catch (Exception ex)
                {

                    JJB_CASH_SUM = 0;

                    JJB_CASH_NUM = 0;
                }

                Updata_JJE_Data();//更新

                JiaoJieBan_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                Main_Border.Height = 0;
                JiaoJieBan_Grid.Visibility = Visibility.Hidden;

            }
        }

        /// <summary>
        /// 关闭交接班页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Close_JiaoJieBan(object sender, MouseButtonEventArgs e)
        {
            Main_Border.Height = 0;
            JiaoJieBan_Grid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 打开日结界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lable_Open_RiJie(object sender, MouseButtonEventArgs e)
        {
            JiaoJieBan_Grid.Visibility = Visibility.Hidden;
            JiaoJieBan_RiJie_Grid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JiaoJieBan_Print_Click(object sender, RoutedEventArgs e)
        {
            Printer.WriteJiaoJieBan(JJB_ALL_ASLE_NUM.ToString(), JJB_ALL_ASLE_SUM.ToString(), JJB_CASH_NUM.ToString(), JJB_CASH_SUM.ToString(), JJB_COUPON_NUM.ToString(), JJB_COUPON_SUM.ToString(), JJB_VIP_NUM.ToString(), JJB_VIP_SUM.ToString(), JJB_GD_NO_DONE_NUM.ToString(), JJB_GD_NO_DONE_SUM.ToString(), JJB_GD_DONE_NUM.ToString(), JJB_GD_DONE_SUM.ToString(), user.User_name);
        }

        /// <summary>
        /// 打开销售报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JiaoJieBan_Statement_Click(object sender, RoutedEventArgs e)
        {
            salesReports.Clear();
            JiaoJieBan_Grid.Visibility = Visibility.Hidden;
            salesReports = DataBaseControls.GetSalesReport(user.User_id.ToString());
            Num_SalesReport = 0;
            Sum_SalesReport = 0;
            foreach (SalesReport salesReport in salesReports)
            {
                Num_SalesReport = Num_SalesReport + salesReport.Num;
                Sum_SalesReport = JJB_ALL_ASLE_SUM;
            }
            JJB_XHBB_DataGrid.ItemsSource = null;
            JJB_XHBB_DataGrid.AutoGenerateColumns = false;
            JJB_XHBB_DataGrid.ItemsSource = salesReports;
            JJB_Sales_Num_Label.Content = Num_SalesReport.ToString();
            JJB_Sales_Sum_Label.Content = Sum_SalesReport.ToString();
            JiaoJieBan_XSBB_Grid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 交班并退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_JiaoJieBan_Eixt(object sender, RoutedEventArgs e)
        {
            JiaoJieBan_Grid.Visibility = Visibility.Hidden;
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }

        /// <summary>
        /// 打开或折叠挂单统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JiaoJieBan_GuaDan_Img_Click(object sender, MouseButtonEventArgs e)
        {
            if (JiaoJieBan_GDTJ_Grid.Height == 50)
            {
                JiaoJieBan_GDTJ_Grid.Height = 150;
                JiaoJieBan_YHTJ_Grid.Margin = new System.Windows.Thickness(20, JiaoJieBan_YHTJ_Grid.Margin.Top + 100, 0, 0);
            }
            else
            {
                JiaoJieBan_GDTJ_Grid.Height = 50;
                JiaoJieBan_YHTJ_Grid.Margin = new System.Windows.Thickness(20, JiaoJieBan_YHTJ_Grid.Margin.Top - 100, 0, 0);
            }

        }

        /// <summary>
        /// 打开或折叠优惠统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JiaoJieBan_YouHui_Img_Click(object sender, MouseButtonEventArgs e)
        {
            if (JiaoJieBan_YHTJ_Grid.Height == 50)
            {
                JiaoJieBan_YHTJ_Grid.Height = 150;
            }
            else
            {
                JiaoJieBan_YHTJ_Grid.Height = 50;
            }
        }
        #endregion

        #region 日结界面

        /// <summary>
        /// 结束日期选择框的打开关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Calendar_End(object sender, MouseButtonEventArgs e)
        {
            if (End_Calender.Visibility == Visibility.Hidden)
            {
                Start_Calender.Visibility = Visibility.Hidden;
                End_Calender.Visibility = Visibility.Visible;
            }
            else
            {
                Start_Calender.Visibility = Visibility.Hidden;
                End_Calender.Visibility = Visibility.Hidden;
                if (End_Calender.SelectedDate.Value != null)
                {
                    RJ_EndTime_TextBox.Text = End_Calender.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                }
            }
        }

        /// <summary>
        /// 开始日期选择框的打开关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Calendar_Start(object sender, MouseButtonEventArgs e)
        {
            if (Start_Calender.Visibility == Visibility.Hidden)
            {
                End_Calender.Visibility = Visibility.Hidden;
                Start_Calender.Visibility = Visibility.Visible;
            }
            else
            {
                End_Calender.Visibility = Visibility.Hidden;
                Start_Calender.Visibility = Visibility.Hidden;
                if (Start_Calender.SelectedDate.Value != null) {
                    RJ_StarTime_TextBox.Text = Start_Calender.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                }
            }

        }

        /// <summary>
        /// 返回交接班
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_FanHuiJiaoBan(object sender, MouseButtonEventArgs e)
        {
            JiaoJieBan_RiJie_Grid.Visibility = Visibility.Hidden;
            JiaoJieBan_Grid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭日结
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Close_RiJie(object sender, MouseButtonEventArgs e)
        {
            //Main_Border.Height = 0;
            JiaoJieBan_RiJie_Grid.Visibility = Visibility.Hidden;
            JiaoJieBan_Grid.Visibility = Visibility.Visible;
        }

        #endregion

        #region 销售报表

        List<SalesReport> salesReports = new List<SalesReport>();

        int Num_SalesReport = 0;

        double Sum_SalesReport = 0;

        /// <summary>
        /// 打开销售报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_XIaoShouBaoBiao(object sender, MouseButtonEventArgs e)
        {
            if (JiaoJieBan_XSBB_Grid.Visibility == Visibility.Hidden)
            {
                salesReports = DataBaseControls.GetSalesReport(user.User_id.ToString());
                JJB_XHBB_DataGrid.ItemsSource = null;
                JJB_XHBB_DataGrid.ItemsSource = salesReports;
                JiaoJieBan_XSBB_Grid.Visibility = Visibility.Visible;
            }
        }

        private void Img_Close_XSBB(object sender, MouseButtonEventArgs e)
        {
            JiaoJieBan_XSBB_Grid.Visibility = Visibility.Hidden;
            JiaoJieBan_Grid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打印销售报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XSBB_Print(object sender, RoutedEventArgs e)
        {
            Printer.WriteGoodsBaoBiao(salesReports, Num_SalesReport.ToString(), Sum_SalesReport.ToString(), user.User_name);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XSBB_Cancel(object sender, RoutedEventArgs e)
        {
            JiaoJieBan_XSBB_Grid.Visibility = Visibility.Hidden;
            JiaoJieBan_Grid.Visibility = Visibility.Visible;
        }

        #endregion

        #region 系统设置

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = (ListViewItem)sender;
            switch (listViewItem.Name)
            {
                //通用设置
                case "ListViewItem_TYSS":
                    ListViewItem_TYSS.Background = ListViewItem_YES.Background;
                    ListViewItem_XPDY.Background = ListViewItem_NO.Background;
                    ListViewItem_GGSZ.Background = ListViewItem_NO.Background;
                    ListViewItem_YJGL.Background = ListViewItem_NO.Background;
                    ListViewItem_ZHGL.Background = ListViewItem_NO.Background;
                    Grid_TYSZ.Visibility = Visibility.Visible;
                    Grid_XPDY.Visibility = Visibility.Hidden;
                    Grid_GGSZ.Visibility = Visibility.Hidden;
                    Grid_YJGL.Visibility = Visibility.Hidden;
                    Grid_ZHGL.Visibility = Visibility.Hidden;

                    TUSZ_MDMC_TextBox.Text = ConfigurationManager.AppSettings["StoreName"];
                    TUSZ_MDDZ_TextBox.Text = ConfigurationManager.AppSettings["Address"];
                    TUSZ_KFDH_TextBox.Text = ConfigurationManager.AppSettings["Phone"];
                    break;
                //小票打印
                case "ListViewItem_XPDY":
                    ListViewItem_TYSS.Background = ListViewItem_NO.Background;
                    ListViewItem_XPDY.Background = ListViewItem_YES.Background;
                    ListViewItem_GGSZ.Background = ListViewItem_NO.Background;
                    ListViewItem_YJGL.Background = ListViewItem_NO.Background;
                    ListViewItem_ZHGL.Background = ListViewItem_NO.Background;
                    Grid_TYSZ.Visibility = Visibility.Hidden;
                    Grid_XPDY.Visibility = Visibility.Visible;
                    Grid_GGSZ.Visibility = Visibility.Hidden;
                    Grid_YJGL.Visibility = Visibility.Hidden;
                    Grid_ZHGL.Visibility = Visibility.Hidden;

                    XTSZ_XPDY_GG.Text = ConfigurationManager.AppSettings["XioaPiaoGG"];
                    XTSZ_XPDY_XPNum.Text = ConfigurationManager.AppSettings["XiaoPiaoNum"];
                    break;
                //广告设置
                case "ListViewItem_GGSZ":
                    ListViewItem_TYSS.Background = ListViewItem_NO.Background;
                    ListViewItem_XPDY.Background = ListViewItem_NO.Background;
                    ListViewItem_GGSZ.Background = ListViewItem_YES.Background;
                    ListViewItem_YJGL.Background = ListViewItem_NO.Background;
                    ListViewItem_ZHGL.Background = ListViewItem_NO.Background;
                    Grid_TYSZ.Visibility = Visibility.Hidden;
                    Grid_XPDY.Visibility = Visibility.Hidden;
                    Grid_GGSZ.Visibility = Visibility.Visible;
                    Grid_YJGL.Visibility = Visibility.Hidden;
                    Grid_ZHGL.Visibility = Visibility.Hidden;
                    try
                    {
                        ComboBox_Is_Left.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["IsLeft"]);
                        ComboBox_Is_Double.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["IsDouble"]);
                        ComboBox_Some_Time.SelectedIndex = Convert.ToInt32(ConfigurationManager.AppSettings["Timer"]) - 1;
                        TuiPian.Content = ConfigurationManager.AppSettings["ImgPath"];
                        ShengYin.Content = ConfigurationManager.AppSettings["MP3Path"];
                    }
                    catch (Exception ex)
                    {


                    }
                    break;
                //硬件管理
                case "ListViewItem_YJGL":
                    ListViewItem_TYSS.Background = ListViewItem_NO.Background;
                    ListViewItem_XPDY.Background = ListViewItem_NO.Background;
                    ListViewItem_GGSZ.Background = ListViewItem_NO.Background;
                    ListViewItem_YJGL.Background = ListViewItem_YES.Background;
                    ListViewItem_ZHGL.Background = ListViewItem_NO.Background;
                    Grid_TYSZ.Visibility = Visibility.Hidden;
                    Grid_XPDY.Visibility = Visibility.Hidden;
                    Grid_GGSZ.Visibility = Visibility.Hidden;
                    Grid_YJGL.Visibility = Visibility.Visible;
                    Grid_ZHGL.Visibility = Visibility.Hidden;
                    break;
                //账号管理
                case "ListViewItem_ZHGL":
                    XTSZ_ZHGL_TextBox.Text = user.User_name;
                    ListViewItem_TYSS.Background = ListViewItem_NO.Background;
                    ListViewItem_XPDY.Background = ListViewItem_NO.Background;
                    ListViewItem_GGSZ.Background = ListViewItem_NO.Background;
                    ListViewItem_YJGL.Background = ListViewItem_NO.Background;
                    ListViewItem_ZHGL.Background = ListViewItem_YES.Background;
                    Grid_TYSZ.Visibility = Visibility.Hidden;
                    Grid_XPDY.Visibility = Visibility.Hidden;
                    Grid_GGSZ.Visibility = Visibility.Hidden;
                    Grid_YJGL.Visibility = Visibility.Hidden;
                    Grid_ZHGL.Visibility = Visibility.Visible;
                    XTSZ_ZHGL_TextBox.Text = ConfigurationManager.AppSettings["StoreManage"];
                    XTSZ_LCIP_TextBox.Text = ConfigurationManager.AppSettings["IP"];
                    break;
            }
        }

        private void Main_Image_XTSZ(object sender, MouseButtonEventArgs e)
        {
            ListViewItem_TYSS.Background = ListViewItem_YES.Background;
            ListViewItem_XPDY.Background = ListViewItem_NO.Background;
            ListViewItem_GGSZ.Background = ListViewItem_NO.Background;
            ListViewItem_YJGL.Background = ListViewItem_NO.Background;
            ListViewItem_ZHGL.Background = ListViewItem_NO.Background;
            Grid_TYSZ.Visibility = Visibility.Visible;
            Grid_XPDY.Visibility = Visibility.Hidden;
            Grid_GGSZ.Visibility = Visibility.Hidden;
            Grid_YJGL.Visibility = Visibility.Hidden;
            Grid_ZHGL.Visibility = Visibility.Hidden;

            TUSZ_MDMC_TextBox.Text = ConfigurationManager.AppSettings["StoreName"];
            TUSZ_MDDZ_TextBox.Text = ConfigurationManager.AppSettings["Address"];
            TUSZ_KFDH_TextBox.Text = ConfigurationManager.AppSettings["Phone"];
            Grid_System_Set.Visibility = Visibility.Visible;
        }

        private void Grid_System_Set_Close(object sender, MouseButtonEventArgs e)
        {
            Grid_System_Set.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_ZhuXiao(object sender, RoutedEventArgs e)
        {
            Grid_System_Set.Visibility = Visibility.Hidden;

            if (DataBaseControls.DeleteGoods() && DataBaseControls.DeleteUser())
            {
                SetValue("DeviceId", "-1");
            }
            this.Close();
            FristLogin inputWindows = new FristLogin();
            inputWindows.ShowDialog();
        }

        #region 硬件管理
        string ImgFile = "";

        string VpiceFile = "";

        private void Button_Click_OpenImg(object sender, RoutedEventArgs e)
        {

            ImgFile = "";
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            openFileDialog.Description = "请选择文件路径";

            //openFileDialog.Filter = "所有文件(*.*)|*.*";
            //openFileDialog.FileName = string.Empty;
            //openFileDialog.FilterIndex = 1;
            //openFileDialog.Multiselect = false;
            //openFileDialog.RestoreDirectory = true;
            //openFileDialog.DefaultExt = "txt";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImgFile = openFileDialog.SelectedPath;
                TuiPian.Content = ImgFile;
               
            }
            
        }

        private void Button_Click_Voice(object sender, RoutedEventArgs e)
        {
            VpiceFile = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择数据源文件";
            openFileDialog.Filter = "mp3文件|*.mp3";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "mp3";
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }
            VpiceFile = openFileDialog.FileName;
            ShengYin.Content = VpiceFile;
        }

        private void Button_Click_XTSZ_YJGL_TJ(object sender, RoutedEventArgs e)
        {
           
           
            string ISLeft = ComboBox_Is_Left.SelectedIndex.ToString();
            string IsDouble = ComboBox_Is_Double.SelectedIndex.ToString();
            int TimeJianGe = (ComboBox_Some_Time.SelectedIndex+1);
            SetValue("IsLeft", ISLeft);
            SetValue("IsDouble", IsDouble);
            SetValue("Timer", TimeJianGe.ToString());
            if (ImgFile.Equals(""))
            {
               // SetValue("ImgPath", @"D:\GGPIC\");
            }
            else
            {
                SetValue("ImgPath", ImgFile);

            }
            if (VpiceFile.Equals(""))
            {
                //SetValue("VoicePath", @"D:\GGPIC\");
            }
            else
            {
                SetValue("MP3Path", VpiceFile);

            }
           // SetValue("VoicePath", VpiceFile);
            MessageBox.Show("设置成功！");
            TuiPian.Content = ConfigurationManager.AppSettings["ImgPath"];
            ShengYin.Content = ConfigurationManager.AppSettings["MP3Path"];
            Grid_System_Set.Visibility = Visibility.Visible;
        }

        private void Button_Click_XTSZ_YJGL_QX(object sender, RoutedEventArgs e)
        {
            Grid_System_Set.Visibility = Visibility.Hidden;
        }
        #endregion

        private void Button_Click_XTSZ_XPDY_QR(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_XTSZ_XPDY_QX(object sender, RoutedEventArgs e)
        {

        }

        public static void SetValue(string key, string value)
        {



            ConfigurationManager.AppSettings.Set(key, value);

            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }
        #endregion

        #region 消息
        private void ListViewItem_Selected_XiaoXi(object sender, RoutedEventArgs e)
        {
            ListViewItem listView = (ListViewItem)sender;

            switch (listView.Name)
            {
                case "ListViewItem_KCYJ":
                    ListViewItem_KCYJ.Background = ListViewItem_YES.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    // ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Visible;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    List<GoodsStore> ts = DataBaseControls.GetKunCunYuJing();
                    XiaoXi_KuCunYuJing.ItemsSource = null;
                    XiaoXi_KuCunYuJing.ItemsSource = ts;
                    break;
                case "ListViewItem_LQYJ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_YES.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
                case "ListViewItem_HLTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_YES.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Visible;
                    List<smc_order> list = DataBaseControls.GetOnlineOrder();
                    XiaoXi_HuoLiu.ItemsSource = null;
                    XiaoXi_HuoLiu.ItemsSource = list;
                    break;
                case "ListViewItem_HLiuTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_YES.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
            }
        }
        #endregion

        #region 销售单据

        private void Image_XIaoShouDanJv(object sender, MouseButtonEventArgs e)
        {
            Main_Border.Height = 766;
            XSDJ_SUM_Label.Content = "0";
            XSDJ_Num_Label.Content = "0";
            XSDV_DataGrid.ItemsSource = null;
            XSDV_Infos_DataGrid.ItemsSource = null;
            Start_Calender_XHDD.SelectedDate = DateTime.Now;
            End_Calender_XHDD.SelectedDate = DateTime.Now.AddDays(1);
            star_textbox_xsdj.Text = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd") + " 00:00:00";
            end_textbox_xsdj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string word = "";
            XSDJ_Orders = DataBaseControls.GetSalesReport(Start_Calender_XHDD.SelectedDate.Value, End_Calender_XHDD.SelectedDate.Value, word);
            XSDV_DataGrid.ItemsSource = null;
            XSDV_DataGrid.AutoGenerateColumns = false;
            XSDV_DataGrid.ItemsSource = XSDJ_Orders;
            XiaoShouDanJv_Grid.Visibility = Visibility.Visible;
        }

        private void Img_Close_XSDD(object sender, MouseButtonEventArgs e)
        {
            Main_Border.Height = 0;
            XiaoShouDanJv_Grid.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 订单对象集合
        /// </summary>
        public List<Order> XSDJ_Orders = new List<Order>();

        /// <summary>
        /// 订单对象明细
        /// </summary>
        public List<OrderItems> XSDJ_Orders_Items = new List<OrderItems>();

        //搜索
        private void Button_Click_SSOrder(object sender, RoutedEventArgs e)
        {

            string word = "";
            if (ordernumber_text.Text.Equals("") || ordernumber_text.Text.Equals("请输入订单号"))
            {
                word = "";
            }
            else
            {
                word = ordernumber_text.Text;
            }
            XSDJ_Orders = DataBaseControls.GetSalesReport(Start_Calender_XHDD.SelectedDate.Value, End_Calender_XHDD.SelectedDate.Value, word);
            XSDV_DataGrid.ItemsSource = null;
            XSDV_DataGrid.AutoGenerateColumns = false;
            XSDV_DataGrid.ItemsSource = XSDJ_Orders;
        }

        /// <summary>
        /// 订单商品数
        /// </summary>
        public int XSDJ_Num = 0;

        /// <summary>
        /// 订单总额
        /// </summary>
        public double XSDJ_Sum = 0;

        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order order = (Order)XSDV_DataGrid.SelectedItem;
            if (order != null) {
                // int ZK = (int)(order.Actual_payment / order.Product_all_price);
                XSDJ_Orders_Items = DataBaseControls.GetSalesReportItems(order.Order_number, (order.Actual_payment / order.Product_all_price).ToString("0.00") + "");
                XSDV_Infos_DataGrid.ItemsSource = null;
                XSDV_Infos_DataGrid.AutoGenerateColumns = false;
                XSDV_Infos_DataGrid.ItemsSource = XSDJ_Orders_Items;
                XSDJ_Num = 0;
                XSDJ_Sum = 0;
                foreach (OrderItems ob in XSDJ_Orders_Items)
                {
                    XSDJ_Num = XSDJ_Num + ob.Goods_num;
                    XSDJ_Sum = XSDJ_Sum + ob.Product_price * ob.Goods_num;
                }
                XSDJ_SUM_Label.Content = XSDJ_Sum.ToString();
                XSDJ_Num_Label.Content = XSDJ_Num.ToString();
            }
        }

        private void Img_Calendar_Start_XHDD(object sender, MouseButtonEventArgs e)
        {

            if (Start_Calender_XHDD.Visibility == Visibility.Hidden)
            {
                End_Calender_XHDD.Visibility = Visibility.Hidden;
                Start_Calender_XHDD.Visibility = Visibility.Visible;
            }
            else
            {
                if (Start_Calender_XHDD.SelectedDate.Value > End_Calender_XHDD.SelectedDate.Value)
                {
                    End_Calender_XHDD.SelectedDate = Start_Calender_XHDD.SelectedDate.Value;
                    MessageBox.Show("截止时间不能早于开始时间。");
                }
                End_Calender_XHDD.Visibility = Visibility.Hidden;
                Start_Calender_XHDD.Visibility = Visibility.Hidden;
                if (Start_Calender_XHDD.SelectedDate != null) {
                    star_textbox_xsdj.Text = Start_Calender_XHDD.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                }

            }
        }

        private void Img_Calendar_End_XHDD(object sender, MouseButtonEventArgs e)
        {

            if (End_Calender_XHDD.Visibility == Visibility.Hidden)
            {
                Start_Calender_XHDD.Visibility = Visibility.Hidden;
                End_Calender_XHDD.Visibility = Visibility.Visible;
            }
            else
            {
                if (Start_Calender_XHDD.SelectedDate.Value > End_Calender_XHDD.SelectedDate.Value)
                {
                    End_Calender_XHDD.SelectedDate = Start_Calender_XHDD.SelectedDate.Value;
                    MessageBox.Show("截止时间不能早于开始时间。");
                }
                Start_Calender_XHDD.Visibility = Visibility.Hidden;
                End_Calender_XHDD.Visibility = Visibility.Hidden;
                if (End_Calender_XHDD.SelectedDate != null)
                {
                    end_textbox_xsdj.Text = End_Calender_XHDD.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
                }
            }
        }

        public class TempOrdr
        {
           public string orderCode;
        }

        /// <summary>
        /// 退整单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XSDJ_TuiDan(object sender, MouseButtonEventArgs e)
        {
           
            bool mark = true;
            bool Marks = false;
            try
            {
                Image img = (Image)sender;
            }
            catch (Exception ex)
            {
                Marks = true;
                mark = false;
            }
            //if (Marks) { 
            //try
            //{
            //        Border lab = (Border)sender;
            //        mark = true;
            //    }
            //catch (Exception ex)
            //{

            //    mark = false;
            //}
            //}
            if (mark) {
                Order order = (Order)XSDV_DataGrid.SelectedItem;
                if (order != null)
                {
                    if (order.Order_status == 0 && DataBaseControls.TuiDan(order.Order_number))
                    {
                        try
                        {
                            TempOrdr tempOrdr = new TempOrdr();
                            tempOrdr.orderCode = order.Order_number;
                            HttpTool.doHttpPost("/cash/report/refundOrder", tempOrdr.ToJSON());
                            //HttpTool.TestdoHttpPost("http://10.0.0.18:8083/cash/report/refundOrder", tempOrdr.ToJSON());
                        }
                        catch (Exception ex)
                        {

                        }
                        //foreach (OrderItems orderItems in XSDJ_Orders_Items)
                        //{
                        //    //更新库存
                        //    //DataBaseControls.UpdataStock(true, orderItems.Goods_num, orderItems.Barcode);
                        //}
                        string type = GetPayType(order.Pay_ment);
                        if (!type.Equals(""))
                        {
                            if (!type.Equals("现金"))
                            {
                                int x = FYZF.BackPay(order.Order_number, (order.Actual_payment * 100).ToString("0"), type, (order.Actual_payment * 100).ToString("0"), order.Payment_time.ToString("yyyyMMdd"), user.User_id.ToString());
                                if (x == 1)
                                {
                                    TuiHuoTiShi_Label.Content = order.Order_number + "退单成功";
                                    //MessageBox.Show(order.Order_number + "退单成功", "退单成功");
                                    TuiHuoTiShi.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                TuiHuoTiShi_Label.Content = "退单成功,请用现金退款。";
                                //MessageBox.Show(order.Order_number + "退单成功", "退单成功");
                                TuiHuoTiShi.Visibility = Visibility.Visible;
                            }
                        }

                        Log_Local.LOG(user.User_id.ToString(), Log_Local.RETURN, order.Order_number);

                        Main_Border.Height = 0;
                        XiaoShouDanJv_Grid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        TuiHuoTiShi_Label.Content = "因为该订单存在退货记录，退货失败";
                        //MessageBox.Show(order.Order_number + " 因为该订单存在退货记录，退货失败", "退单失败");
                        TuiHuoTiShi.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void XSDJ_TuiDans(object sender, MouseButtonEventArgs e)
        {
            bool mark = true;
            bool Marks = false;
            try
            {
                Image img = (Image)sender;
            }
            catch (Exception ex)
            {
                Marks = true;
                mark = false;
            }
            //if (Marks) { 
            //try
            //{
            //        Border lab = (Border)sender;
            //        mark = true;
            //    }
            //catch (Exception ex)
            //{

            //    mark = false;
            //}
            //}
            if (mark)
            {
                Order order = (Order)XSDV_DataGrid.SelectedItem;
                if (order != null)
                {
                    if (order.Order_status == 0 && DataBaseControls.TuiDan(order.Order_number))
                    {
                        try
                        {
                            TempOrdr tempOrdr = new TempOrdr();
                            tempOrdr.orderCode = order.Order_number;
                            HttpTool.doHttpPost("/cash/report/refundOrder", tempOrdr.ToJSON());
                            //HttpTool.TestdoHttpPost("http://10.0.0.18:8083/cash/report/refundOrder", tempOrdr.ToJSON());
                        }
                        catch (Exception ex)
                        {

                        }
                        //foreach (OrderItems orderItems in XSDJ_Orders_Items)
                        //{
                        //    //更新库存
                        //    //DataBaseControls.UpdataStock(true, orderItems.Goods_num, orderItems.Barcode);
                        //}
                        string type = GetPayType(order.Pay_ment);
                        if (!type.Equals(""))
                        {
                            if (!type.Equals("现金"))
                            {
                                int x = FYZF.BackPay(order.Order_number, (order.Actual_payment * 100).ToString("0"), type, (order.Actual_payment * 100).ToString("0"), order.Payment_time.ToString("yyyyMMdd"), user.User_id.ToString());
                                if (x == 1)
                                {
                                    TuiHuoTiShi_Label.Content = order.Order_number + "退单成功";
                                    //MessageBox.Show(order.Order_number + "退单成功", "退单成功");
                                    TuiHuoTiShi.Visibility = Visibility.Visible;
                                }
                            }
                            else
                            {
                                TuiHuoTiShi_Label.Content = order.Order_number + "退单成功,请用现金退款。";
                                //MessageBox.Show(order.Order_number + "退单成功", "退单成功");
                                TuiHuoTiShi.Visibility = Visibility.Visible;
                            }
                        }

                        Log_Local.LOG(user.User_id.ToString(), Log_Local.RETURN, order.Order_number);

                        Main_Border.Height = 0;
                        XiaoShouDanJv_Grid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        TuiHuoTiShi_Label.Content = "因为该订单存在退货记录，退货失败";
                        //MessageBox.Show(order.Order_number + " 因为该订单存在退货记录，退货失败", "退单失败");
                        TuiHuoTiShi.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// 退单品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_TuiDanPin(object sender, RoutedEventArgs e)
        {
            Order order = (Order)XSDV_DataGrid.SelectedItem;
            if (order!=null) { 
            if (order.Order_status == 0 && DataBaseControls.TuiDan(order.Order_number))
            {
                OrderItems orderItems = (OrderItems)XSDV_Infos_DataGrid.SelectedItem;
                if (orderItems!=null) { 
                //更新库存
                DataBaseControls.UpdataStock(true, orderItems.Goods_num, orderItems.Barcode);
                        string type = GetPayType(order.Pay_ment);
                        if (!type.Equals(""))
                        {
                            double zk = 1;
                            if (orderItems.Zhekou.Equals(""))
                            {
                                zk = 1;
                            }
                            else
                            {
                                zk = Convert.ToDouble(orderItems.Zhekou);
                            }
                            int x = FYZF.BackPay(order.Order_number, (order.Actual_payment * 100).ToString("0"), type, (100*zk* orderItems.Goods_num* orderItems.Product_price).ToString("0"),  order.Payment_time.ToString("yyyyMMdd"), user.User_id.ToString());
                            if (x == 1) { MessageBox.Show(orderItems.Goods_name + "已退货成功", "退货成功"); }
                        }
                        Log_Local.LOG(user.User_id.ToString(), Log_Local.RETURN, order.Order_number + "&" + orderItems.Barcode);
               
                        Main_Border.Height = 0;
                        XiaoShouDanJv_Grid.Visibility = Visibility.Hidden;
                    }
            }
            else
            {
                MessageBox.Show( "因为该订单存在退货记录，退货失败", "退货失败");
            }
            }

        }

        private void Button_Click_XSDJ_Print(object sender, RoutedEventArgs e)
        {

        }

        private string GetPayType(string type)
        {
            string str = "";
            switch (type)
            {
                case "微信支付":
                    str = "WECHAT";
                    break;
                case "支付宝支付":
                    str = "ALIPAY";
                    break;
                case "现金":
                    str = "现金";
                    break;
                default:
                    str = "";
                    break;

            }
            return str;

        }
        #endregion

        #region 打开或关闭界面

     

        private void TuiChuJinHuo(object sender, MouseButtonEventArgs e)
        {
            ShouKuan_Grid.Visibility = Visibility.Hidden;
        }



        private void Close_QvDan(object sender, MouseButtonEventArgs e)
        {
            QvDan_Grid.Visibility = Visibility.Hidden;
        }

        private void Grid_System_XiaoXi_Close(object sender, MouseButtonEventArgs e)
        {
            Grid_System_XiaoXi.Visibility = Visibility.Hidden;
        }

        private void Image_OpenXiaoXi(object sender, MouseButtonEventArgs e)
        {
            Grid_System_XiaoXi.Visibility = Visibility.Visible;
        }

        #endregion

        #region 库存
        private void Img_Close_CKKC(object sender, MouseButtonEventArgs e)
        {
            KuCun_DataGrid.ItemsSource = null;
            Page_Num.Text = "1";
            KuCun_Grid.Visibility = Visibility.Hidden;
        }

        private void Image_CKKC(object sender, MouseButtonEventArgs e)
        {
            KuCun_TextBox.Text = "条码/拼音码/商品名"; 
            StartTime_KuCun.Text = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            EndTime_KuCun.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00";
            Page_Num.Text = "1";
            KCGoodsCount = DataBaseControls.GetGoodsCount();
            KC_GoodsStore = DataBaseControls.GetSomeKunCun(0);
            KuCun_DataGrid.ItemsSource = null;
            KuCun_DataGrid.AutoGenerateColumns = false;
            KuCun_DataGrid.ItemsSource = KC_GoodsStore;
            UpGoodsCount();
            KuCun_Grid.Visibility = Visibility.Visible;
            DatToMoon.Background = Today.Background;
        }

        private void Button_Click_BC(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(Page_Num.Text);
                
                if (num>1)
                {
                    num--;
                }
                KC_GoodsStore = DataBaseControls.GetSomeKunCun(num * 20);
                KuCun_DataGrid.ItemsSource = null;
                KuCun_DataGrid.AutoGenerateColumns = false;
                KuCun_DataGrid.ItemsSource = KC_GoodsStore;
                Page_Num.Text = num.ToString();
                UpGoodsCount();
            }
            catch (Exception ex)
            {

            }

        }

        private void Button_Click_GO(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(Page_Num.Text);
                num++;
                KC_GoodsStore = DataBaseControls.GetSomeKunCun((num-1)*20);
                KuCun_DataGrid.ItemsSource = null;
                KuCun_DataGrid.AutoGenerateColumns = false;
                KuCun_DataGrid.ItemsSource = KC_GoodsStore;
                Page_Num.Text = num.ToString();
                UpGoodsCount();
            }
            catch (Exception ex)
            {
            }
        }

        private void PageText_In(object sender, MouseEventArgs e)
        {
        }

        private void PageText_Out(object sender, MouseEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(Page_Num.Text);
                KC_GoodsStore = DataBaseControls.GetSomeKunCun(num * 10);
                KuCun_DataGrid.ItemsSource = null;
                KuCun_DataGrid.AutoGenerateColumns = false;
                KuCun_DataGrid.ItemsSource = KC_GoodsStore;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 库存数据集合
        /// </summary>
        public List<GoodsStore> KC_GoodsStore = new List<GoodsStore>();

        /// <summary>
        /// 查询商品总数
        /// </summary>
        public int KCGoodsCount = 0;

        /// <summary>
        /// 更新显示库存总数
        /// </summary>
        public void UpGoodsCount()
        {
            KC_Count_Label.Content="共"+KCGoodsCount+"条，"+ ((KCGoodsCount /20)+1)+ "页";
            KC_Count_Label.Visibility = Visibility.Visible;
        }

        private void Button_Click_KunCunSS(object sender, RoutedEventArgs e)
        {
            string word = "";
            if (!KuCun_TextBox.Text.Equals("") && !KuCun_TextBox.Text.Equals("条码/拼音码/商品名"))
            {
                word = KuCun_TextBox.Text;
                KC_GoodsStore = DataBaseControls.GetKunCun(word);
            }
            else
            {
                KC_GoodsStore = DataBaseControls.GetKunCun(word);
            }
           // else
            //if (Start_Calender_KuCun.SelectedDate == null || End_Calender_KuCun.SelectedDate == null)
            //{
            //    KC_GoodsStore = DataBaseControls.GetKunCun(Start_Calender_KuCun.DisplayDate.AddDays(-1), End_Calender_KuCun.DisplayDate.AddDays(1), word);

            //}
            //else {
            //    KC_GoodsStore = DataBaseControls.GetKunCun(Start_Calender_KuCun.SelectedDate.Value, End_Calender_KuCun.SelectedDate.Value, word);
            //}
            KuCun_DataGrid.ItemsSource = null;
            KuCun_DataGrid.AutoGenerateColumns = false;
            KuCun_DataGrid.ItemsSource = KC_GoodsStore;
            KC_Count_Label.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 结束日期选择框的打开关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Calendar_End_KuCun(object sender, MouseButtonEventArgs e)
        {
            //StartTime_KuCun.Text = Start_Calender_KuCun.DisplayDate.ToString("yyyy-MM-dd") + " 00:00:00";
            
            if (End_Calender_KuCun.Visibility == Visibility.Hidden)
            {
                Start_Calender_KuCun.Visibility = Visibility.Hidden;
                End_Calender_KuCun.Visibility = Visibility.Visible;
                
            }
            else
            {
                Start_Calender_KuCun.Visibility = Visibility.Hidden;
                End_Calender_KuCun.Visibility = Visibility.Hidden;
                if (End_Calender_KuCun.SelectedDate==null) { return; }
                EndTime_KuCun.Text = End_Calender_KuCun.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            }
        }

        /// <summary>
        /// 开始日期选择框的打开关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Img_Calendar_Start_KuCun(object sender, MouseButtonEventArgs e)
        {
            
            //EndTime_KuCun.Text = End_Calender_KuCun.DisplayDate.ToString("yyyy-MM-dd") + " 00:00:00";
            if (Start_Calender_KuCun.Visibility == Visibility.Hidden)
            {
                End_Calender_KuCun.Visibility = Visibility.Hidden;
                Start_Calender_KuCun.Visibility = Visibility.Visible;
               
            }
            else
            {
                End_Calender_KuCun.Visibility = Visibility.Hidden;
                Start_Calender_KuCun.Visibility = Visibility.Hidden;
                if (Start_Calender_KuCun.SelectedDate == null) { return; }
                StartTime_KuCun.Text = Start_Calender_KuCun.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            }
            
        }
        Button DatToMoon=new Button();
        private void Button_Click_Today(object sender, RoutedEventArgs e)
        {
            Today.Background= DatToMoon.Background;
            OneMoon.Background = new SolidColorBrush(Colors.WhiteSmoke); 
            //End_Calender_KuCun.DisplayDate = DateTime.Now.AddDays(1);
            //Start_Calender_KuCun.DisplayDate = DateTime.Now;
            //EndTime_KuCun.Text = End_Calender_KuCun.DisplayDate.ToString("yyyy-MM-dd") + " 00:00:00";
            //StartTime_KuCun.Text = Start_Calender_KuCun.DisplayDate.ToString("yyyy-MM-dd") + " 00:00:00";
            KC_GoodsStore = DataBaseControls.GetKunCun(DateTime.Now, DateTime.Now.AddDays(1), "");
            KuCun_DataGrid.ItemsSource = null;
            KuCun_DataGrid.AutoGenerateColumns = false;
            KuCun_DataGrid.ItemsSource = KC_GoodsStore;
            KC_Count_Label.Visibility = Visibility.Hidden;
        }

        private void Button_Click_TheMoon(object sender, RoutedEventArgs e)
        {
            Button bnt = (Button)sender;
            //End_Calender_KuCun.SelectedDate = DateTime.Now.AddDays(1);
            //Start_Calender_KuCun.SelectedDate = DateTime.Now.AddDays(-30);
            //EndTime_KuCun.Text = End_Calender_KuCun.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            //StartTime_KuCun.Text = Start_Calender_KuCun.SelectedDate.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            OneMoon.Background = DatToMoon.Background;
            Today.Background = new SolidColorBrush(Colors.WhiteSmoke);
            KC_GoodsStore = DataBaseControls.GetKunCun(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(1), "");
            KuCun_DataGrid.ItemsSource = null;
            KuCun_DataGrid.AutoGenerateColumns = false;
            KuCun_DataGrid.ItemsSource = KC_GoodsStore;
            KC_Count_Label.Visibility = Visibility.Hidden;
        }

        #endregion

        #region 键盘

        public int JP_Type = -1;

        private void Button_Click_JP_OK(object sender, RoutedEventArgs e)
        {
            switch (JP_Type)
            {
                case 2://条码拼音商品名
                    if (!JP_TextBox.Text.Equals(""))
                    {
                        Main_TMPYSPM_TextBox.Text = JP_TextBox.Text;
                    }
                    else
                    {
                        Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                    }
                    PriceSum = 0;
                    //商品表对象

                    GoodsStore ps1 = DataBaseControls.GetGoodsStoreByBarcode(Main_TMPYSPM_TextBox.Text);
                    if (ps1.Barcode != null)
                    {
                        if (Goods_AddSum_Grid.Visibility == Visibility.Hidden)
                        {

                            //收银小票一行对象
                            Goodsroder goodsroder = new Goodsroder();

                            int j = 0;
                            bool remark = true;//是否加入集合
                            foreach (Goodsroder ps12 in infoList)
                            {

                                if (ps12.Barcode.Equals(ps1.Barcode))
                                {
                                    infoList[j].Num = infoList[j].Num + 1;
                                    infoList[j].Sum = infoList[j].Num * ps12.Sale_price;
                                    remark = false;
                                    break;
                                }
                                j++;
                            }
                            if (remark)
                            {
                                goodsroder.Num = 1;
                                goodsroder.Name = ps1.Goods_name;
                                goodsroder.Barcode = ps1.Barcode;
                                goodsroder.Original_price = ps1.Original_price;
                                goodsroder.Sale_price = ps1.Sale_price;
                                goodsroder.Sum = goodsroder.Num * goodsroder.Sale_price;
                                infoList.Add(goodsroder);
                            }
                            remark = true;
                            GoodsAllNum = 0;
                            foreach (Goodsroder ps12 in infoList)
                            {
                                PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                                GoodsAllNum = GoodsAllNum + ps12.Num;
                            }
                            OrderGrid.ItemsSource = null;
                            PriceSumLabel.Content = PriceSum.ToString();//总金额
                            OrderGrid.AutoGenerateColumns = false;
                            OrderGrid.ItemsSource = infoList;
                            GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                            Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                        }
                        else//进货页面
                        {
                            AddGoodsInfo good_add = new AddGoodsInfo();
                            All_IN_GOODS_SUM = 0;
                            ALL_OUT_PRICE = 0;
                            ALL_IN_PRICE = 0;

                            good_add.Num = 0;
                            good_add.Name = ps1.Goods_name;
                            good_add.Barcode = ps1.Barcode;
                            good_add.Guige = ps1.Unit;
                            good_add.Sale_price = ps1.Sale_price;
                            good_add.Supplier1 = ps1.Supplier_id;
                            good_add.GoodsId = ps1.Goods_id;
                            InputWindows input = new InputWindows();
                            input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            input.ShowDialog();
                            good_add.Num = (int)input.Num;
                            addGoodsInfoslist.Add(good_add);
                            AddGoods_DataGrid.ItemsSource = null;
                            AddGoods_DataGrid.AutoGenerateColumns = false;
                            AddGoods_DataGrid.ItemsSource = addGoodsInfoslist;
                            foreach (AddGoodsInfo obj in addGoodsInfoslist)
                            {
                                All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
                                ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
                                ALL_IN_PRICE = 0;
                            }
                            Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                            Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                            Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                        }
                    }
                    else
                    {
                        //未找到商品
                        MessageBox.Show("未找到商品！");
                    }
                    break;
                case 1://无码商品
                    Main_WMSP_TextBox.Text = JP_TextBox.Text;
                    Goodsroder goodsroders = new Goodsroder();
                    goodsroders.Num = 1;
                    goodsroders.Name = "无码商品";
                    goodsroders.Barcode ="0000000";
                    goodsroders.Original_price =0;

                    try
                    {
                        goodsroders.Sum = Convert.ToDouble(JP_TextBox.Text);
                        goodsroders.Sale_price = Convert.ToDouble(JP_TextBox.Text);
                        infoList.Add(goodsroders);
                        GoodsAllNum = 0;
                        foreach (Goodsroder ps12 in infoList)
                        {
                            PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                            GoodsAllNum = GoodsAllNum + ps12.Num;
                        }
                        OrderGrid.ItemsSource = null;
                        PriceSumLabel.Content = PriceSum.ToString();//总金额
                        OrderGrid.AutoGenerateColumns = false;
                        OrderGrid.ItemsSource = infoList;
                        GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                    }
                    catch (Exception ex)
                    {
                        Main_WMSP_TextBox.Text = "";
                        break;
                    }
                    
                    break;
                case 3://会员号
                    Main_HYHSJH_TextBox.Text = JP_TextBox.Text;
                    vip= DataBaseControls.GetVIPUser(Main_HYHSJH_TextBox.Text);
                    Vip_Name.Content = vip.User_name;
                    Vip_JF.Content = vip.Integral.ToString();
                    Vip_YY.Content = vip.Suplus.ToString();
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
                    
            }
            JianPan_Grid.Visibility = Visibility.Hidden;
            JP_TextBox.Text = "";
        }

        private void Button_Click_JP_QX(object sender, RoutedEventArgs e)
        {
            JP_Type = -1;
            JianPan_Grid.Visibility = Visibility.Hidden;
            JP_TextBox.Text = "";
        }

        private void Button_Click_JP_Num(object sender, RoutedEventArgs e)
        {
            Button bnt = (Button)sender;
            if (!bnt.Content.Equals(""))
            {
                JP_TextBox.Text = JP_TextBox.Text + bnt.Content;
            }
            else
            {
               

            }
           
        }

        private void JP_WMSP_Img_Button(object sender, MouseButtonEventArgs e)
        {
            JP_Type = 1;
            JianPan_Grid.Visibility = Visibility.Visible;
        }

        private void Button_Click_JP_Delete(object sender, MouseButtonEventArgs e)
        {
            if (JP_TextBox.Text.Length > 0)
            {
                JP_TextBox.Text = JP_TextBox.Text.Remove(JP_TextBox.Text.Length - 1);
            }
        }

        private void JP_TMPYMMC_Img_Button(object sender, MouseButtonEventArgs e)
        {
            JP_Type = 2;
            if (!Main_TMPYSPM_TextBox.Text.Equals("条码/拼音码/商品名称"))
            {
                JP_TextBox.Text = Main_TMPYSPM_TextBox.Text;
            }
            else
            {
                JP_TextBox.Text = "";
            }
           
            JianPan_Grid.Visibility = Visibility.Visible;
        }

        private void JP_VIP_Img_Button(object sender, MouseButtonEventArgs e)
        {
            JP_Type = 3;
            JianPan_Grid.Visibility = Visibility.Visible;
        }



        #endregion

        #region 收款

        private void Image_ShouKuan(object sender, MouseButtonEventArgs e)
        {
            ShouKuan_Grid.Visibility = Visibility.Visible;
            PIC_Name = "xj";
            xj.Source = xj_.Source;
            if (PriceSum != 0)
            {
                ShouKan_ShangTextBox.Text = PriceSum.ToString();
                ShouKan_ZhongTextBox.Text = PriceSum.ToString();
                ShouKan_XiaTextBox.Text = "0";
                tempPriceSum = PriceSum;
            }
            else
            {
                ShouKan_ShangTextBox.Text = "";
                ShouKan_XiaTextBox.Text = "";
                ShouKan_ZhongTextBox.Text = "";
                tempPriceSum = PriceSum;
            }
        }

        private void Image_ShouKuan(object sender, RoutedEventArgs e)
        {
            ShouKuan_Grid.Visibility = Visibility.Visible;
            PIC_Name = "xj";
            xj.Source = xj_.Source;
            if (PriceSum != 0)
            {
                ShouKan_ShangTextBox.Text = PriceSum.ToString();
                ShouKan_ZhongTextBox.Text = PriceSum.ToString();
                ShouKan_XiaTextBox.Text = "0";
                tempPriceSum = PriceSum;
            }
            else
            {
                ShouKan_ShangTextBox.Text = "";
                ShouKan_XiaTextBox.Text = "";
                ShouKan_ZhongTextBox.Text = "";
                tempPriceSum = PriceSum;
            }
        }

        private void Button_Click_AddCart(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_ShouKuanAnNiu(object sender, RoutedEventArgs e)
        {
            try
            {
                double Cash_Sum = 0;
                Button bnt = (Button)sender;
                ShouKan_ZhongTextBox.Text = ShouKan_ZhongTextBox.Text + bnt.Content;
                double Sum = Convert.ToDouble(ShouKan_ShangTextBox.Text);
                try
                {
                    Cash_Sum = Convert.ToDouble(ShouKan_ZhongTextBox.Text);
                }
                catch (Exception ex)
                {
                    ShouKan_ZhongTextBox.Text = ShouKan_ZhongTextBox.Text.ToString().Remove(ShouKan_ZhongTextBox.Text.ToString().Length - 1);
                    Cash_Sum = Convert.ToDouble(ShouKan_ZhongTextBox.Text);
                }
                if ((Cash_Sum - Sum) > 0)
                {
                    ShouKan_XiaTextBox.Text = (Cash_Sum - Sum).ToString();
                }
                else
                {
                    ShouKan_XiaTextBox.Text = "0";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click_ShouKuanZheng(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bnt = (Button)sender;
                ShouKan_ZhongTextBox.Text = bnt.Content.ToString();
                double Sum = Convert.ToDouble(ShouKan_ShangTextBox.Text);
                double Cash_Sum = Convert.ToDouble(ShouKan_ZhongTextBox.Text);
                if ((Cash_Sum - Sum) > 0)
                {
                    ShouKan_XiaTextBox.Text = (Cash_Sum - Sum).ToString();
                }
                else
                {
                    ShouKan_XiaTextBox.Text = "0";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click_Delete_ShouKuan(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShouKan_ZhongTextBox.Text.Length > 0)
                {
                    ShouKan_ZhongTextBox.Text = ShouKan_ZhongTextBox.Text.ToString().Remove(ShouKan_ZhongTextBox.Text.ToString().Length - 1);
                    double Cash_Sum = 0;
                    if (ShouKan_ZhongTextBox.Text.ToString().Length < 1)
                    {
                        Cash_Sum = 0;
                    }
                    else
                    {
                        Cash_Sum = Convert.ToDouble(ShouKan_ZhongTextBox.Text);
                    }
                    double Sum = Convert.ToDouble(ShouKan_ShangTextBox.Text);

                    if ((Cash_Sum - Sum) > 0)
                    {
                        ShouKan_XiaTextBox.Text = (Cash_Sum - Sum).ToString();
                    }
                    else
                    {
                        ShouKan_XiaTextBox.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Click_ZheKou(object sender, RoutedEventArgs e)
        {
            try
            {
                tempPriceSum = PriceSum;
                ShouKan_ShangTextBox.Text = PriceSum.ToString();
                ShouKan_ZhongTextBox.Text = PriceSum.ToString();
                ShouKan_XiaTextBox.Text = "0";
                Button bnt = (Button)sender;
                switch (bnt.Content.ToString())
                {
                    case "免单":
                        tempPriceSum = 0;
                        break;
                    case "抹零":
                        break;
                    case "95折":
                        tempPriceSum = tempPriceSum * 0.95;
                        break;
                    case "9折":
                        tempPriceSum = tempPriceSum * 0.9;
                        break;
                    case "85折":
                        tempPriceSum = tempPriceSum * 0.85;
                        break;
                    case "8折":
                        tempPriceSum = tempPriceSum * 0.8;
                        break;
                    case "75折":
                        tempPriceSum = tempPriceSum * 0.75;
                        break;
                    case "7折":
                        tempPriceSum = tempPriceSum * 0.7;
                        break;
                    default:
                        break;
                }
                ShouKan_ShangTextBox.Text = tempPriceSum.ToString();
                ShouKan_ZhongTextBox.Text = tempPriceSum.ToString();
                ShouKan_XiaTextBox.Text = "0";
                UpdateHM(infoList, tempPriceSum);
            }
            catch (Exception ex)
            {

            }
        }

        private void XJTextBoxSR(object sender, KeyEventArgs e)
        {
            if (!ShouKan_ShangTextBox.Text.Equals("")&& !ShouKan_ZhongTextBox.Text.Equals("")) { 
            double Cash_Sum = 0;
            double Sum = Convert.ToDouble(ShouKan_ShangTextBox.Text);
            Cash_Sum = Convert.ToDouble(ShouKan_ZhongTextBox.Text);
            if ((Cash_Sum - Sum) > 0)
            {
                ShouKan_XiaTextBox.Text = (Cash_Sum - Sum).ToString();
            }
            else
            {
                ShouKan_XiaTextBox.Text = "0";
            }
            }
        }

        /// <summary>
        /// 折后金额
        /// </summary>
        double tempPriceSum = 0;

        private void Button_Click_QRSK(object sender, RoutedEventArgs e)
        {
            int PayType = 4;// 1 微信支付   2支付宝支付  3云闪付 4 其他
                            //判断支付方式
            UpdateHM(infoList, tempPriceSum);
            switch (PIC_Name)
            {
                case "zfb":
                    zfb.Source = zfb_.Source;
                    break;
                case "wx":
                    ISPAY = true;
                    break;
                case "czk":
                    czk.Source = czk_.Source;
                    break;
                case "xj":
                    #region 现金
                    if (infoList.Count > 0)
                    {

                        //打印小票
                        Order order = new Order();
                        order.Order_number = Tool_Somthing.GetOrderNumber();
                        if (!Printer.WriteObject(infoList, user.User_name, order.Order_number, PriceSum, "现金支付"))
                        {
                            //打印小票出错支付已成功
                        }


                        //更新库存
                        foreach (Goodsroder ps12 in infoList)
                        {
                            DataBaseControls.UpdataStock(false, ps12.Num, ps12.Barcode.ToString());
                            if (DataBaseControls.GetKunCunYuJing(ps12.Barcode.ToString()))
                            {
                                KCBZWindow kCBZWindow = new KCBZWindow();
                                kCBZWindow.Update += UpdataXiao;
                                kCBZWindow.Show();                              
                                //return;
                            }
                            
                        }

                        #region 新增订单


                       order.Pay_ment = "现金";
                        order.Coupons_price = PriceSum - tempPriceSum;
                        order.Product_all_price = PriceSum;
                        order.Actual_payment = tempPriceSum;
                        order.User_id = user.User_id;
                        order.Update_by = user.User_name;
                        order.Dept_id = DEPT_ID;
                        DataBaseControls.AddOrder(order);
                        #endregion

                        //新增订单详情
                        DataBaseControls.AddOrderInfo(order.Order_number, user.User_id.ToString(), "备注", infoList);

                        #region 更新服务端库存
                        try
                        {
                            ToServiceRequest toServiceRequest = new ToServiceRequest();
                            toServiceRequest.DeviceId = DEVICE_ID;
                            toServiceRequest.Type = 1;
                            ToServiceOrder toServiceOrder = new ToServiceOrder(order.Order_number, user.User_id, DateTime.Now, PriceSum, GoodsAllNum, "3");
                            toServiceOrder.List = infoList;
                            toServiceRequest.Data = toServiceOrder;
                            HttpTool.doHttpPost("/cash/report/add_order", toServiceRequest.ToJSON());
                            //HttpTool.TestdoHttpPost("http://10.0.0.18:8083/cash/report/add_order", toServiceRequest.ToJSON());
                        }
                        catch (Exception ex)
                        {
                            Log_Local.LOG("更新服务端库存", 402, order.Order_number);
                        }
                        #endregion

                        ISPAY = false;
                        OrderGrid.ItemsSource = null;//清除界面商品信息
                        infoList.Clear();//清除商品信息集合
                        PriceSumLabel.Content = 0;
                        GoodsAllNum = 0;
                        GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                        Log_Local.LOG(user.User_id.ToString(), 3, order.Order_number);//记录日志 
                        ShouKuan_Grid.Visibility = Visibility.Hidden;
                    } 
                    #endregion
                    break;
                default:
                    break;
            } 
        }

        private void PIC_In(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Opacity = 0.6;
        }

        private void PIC_Out(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            if (img.Name != PIC_Name) { img.Opacity = 0.8; }

        }
        string PIC_Name = string.Empty;
        private void Click_PIC(object sender, MouseButtonEventArgs e)
        {
            zfb.Source = _zfb_.Source;
            wx.Source = _wx_.Source;
            czk.Source = _czk_.Source;
            xj.Source = _xj_.Source;
            Image img = (Image)sender;
            switch (img.Name)
            {
                case "zfb":
                    zfb.Source = zfb_.Source;
                    break;
                case "wx":
                    wx.Source = wx_.Source;
                    break;
                case "czk":
                    czk.Source = czk_.Source;
                    break;
                case "xj":
                    xj.Source = xj_.Source;
                    break;
                default:
                    break;
            }
            PIC_Name = img.Name;
        }

        public void Update(string name)
        {
            try
            {
                zfb.Source = _zfb_.Source;
                wx.Source = _wx_.Source;
                czk.Source = _czk_.Source;
                xj.Source = _xj_.Source;
                switch (name)
                {
                    case "zfb":
                        zfb.Source = zfb_.Source;
                        break;
                    case "wx":
                        wx.Source = wx_.Source;
                        break;
                    case "czk":
                        czk.Source = czk_.Source;
                        break;
                    case "xj":
                        xj.Source = xj_.Source;
                        break;
                    default:
                        break;
                }
                PIC_Name = name;
                ShouKan_ShangTextBox.Text = PriceSum.ToString();
                ShouKan_ZhongTextBox.Text = PriceSum.ToString();
            }
            catch (Exception ex)
            {

            }
        }



        #endregion

        private void Button_Click_LiShi(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_Selected_QvDan(object sender, MouseButtonEventArgs e)
        {

        }

        private void InTextBox(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Name.Equals("ordernumber_text"))
            {
                if (ordernumber_text.Text.Equals("请输入订单号"))
                {
                    ordernumber_text.Text = "";
                }

            }
            else
            {
                if (textBox.Text.Equals("条码/拼音码/商品名"))
                {
                    textBox.Text = "";
                }
            }
        }

        private void OutTextBox(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            

            if (textBox.Name.Equals("ordernumber_text"))
            {
                if (ordernumber_text.Text.Equals(""))
                {
                    ordernumber_text.Text = "请输入订单号";
                }

            }
            else
            {
                if (textBox.Text.Equals(""))
                {
                    textBox.Text = "条码/拼音码/商品名";
                }
            }
        }

        private void WMSKJG(object sender, KeyEventArgs e)
        {
            TextBox textBox=(TextBox)sender;
            if (e.Key == Key.Enter)
            {
                switch (textBox.Name)
                {
                    case "Main_TMPYSPM_TextBox":


                        //商品表对象
                        List<GoodsStore> GoodsStore = DataBaseControls.GetGoodsStore(textBox.Text);
                        SS_OrderGrid.ItemsSource = null;
                        SS_OrderGrid.ItemsSource = GoodsStore;

                        NowSSLabel.Content = "当前匹配数："+ GoodsStore.Count;
                        if (GoodsStore.Count > 10)
                        {
                            NowSSLabelTS.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            NowSSLabelTS.Visibility = Visibility.Visible;
                        }
                        TCK_MHSS.Visibility = Visibility.Visible;

                        //textBox.Text = "";

                        #region MyRegion
                       
                        #endregion

                        break;
                    case "Main_WMSP_TextBox":
                        //Main_WMSP_TextBox.Text = JP_TextBox.Text;
                        Goodsroder goodsroders = new Goodsroder();
                        goodsroders.Num = 1;
                        goodsroders.Name = "无码商品";
                        goodsroders.Barcode = "0000000";
                        goodsroders.Original_price = 0;

                        try
                        {
                            goodsroders.Sum = Convert.ToDouble(Main_WMSP_TextBox.Text);
                            goodsroders.Sale_price = Convert.ToDouble(Main_WMSP_TextBox.Text);
                            infoList.Add(goodsroders);
                            GoodsAllNum = 0;
                            PriceSum = 0;
                            foreach (Goodsroder ps12 in infoList)
                            {
                                PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                                GoodsAllNum = GoodsAllNum + ps12.Num;
                            }
                            OrderGrid.ItemsSource = null;
                            PriceSumLabel.Content = PriceSum.ToString();//总金额
                            OrderGrid.AutoGenerateColumns = false;
                            OrderGrid.ItemsSource = infoList;
                            GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                        }
                        catch (Exception ex)
                        {
                            Main_WMSP_TextBox.Text = "";
                            break;
                        }
                        Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                        break;
                    case "Main_HYHSJH_TextBox":

                        Main_HYHSJH_TextBox.Text = JP_TextBox.Text;
                        vip = DataBaseControls.GetVIPUser(Main_HYHSJH_TextBox.Text);
                        Vip_Name.Content = vip.User_name;
                        Vip_JF.Content = vip.Integral.ToString();
                        Vip_YY.Content = vip.Suplus.ToString();
                        break;
                }
                //回车
            }
        }

        private void WMSKJGIn(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Focusable = true;
            switch (textBox.Name)
                {
                    case "Main_TMPYSPM_TextBox":
                    if (Main_TMPYSPM_TextBox.Text.Equals("条码/拼音码/商品名称"))
                    { 
                    Main_TMPYSPM_TextBox.Text = "";
                    }
                    break;
                    case "Main_WMSP_TextBox":
                    if (Main_WMSP_TextBox.Text.Equals("输入价格无码收款"))
                    {
                        Main_WMSP_TextBox.Text = "";
                    }
                    break;
                case "Main_HYHSJH_TextBox":
                    if (Main_HYHSJH_TextBox.Text.Equals("会员号/手机号"))
                    {
                        Main_HYHSJH_TextBox.Text = "";
                    }
                    break;
            }
                //回车
            
        }

        private void WMSKJGOut(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            switch (textBox.Name)
            {
                case "Main_TMPYSPM_TextBox":
                    if (Main_TMPYSPM_TextBox.Text.Equals(""))
                    {
                        Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                        
                    }
                    break;
                case "Main_WMSP_TextBox":
                    if (Main_WMSP_TextBox.Text.Equals(""))
                    {
                        Main_WMSP_TextBox.Text = "输入价格无码收款";
                    }
                    break;
                case "Main_HYHSJH_TextBox":
                    if (Main_HYHSJH_TextBox.Text.Equals(""))
                    {
                        Main_HYHSJH_TextBox.Text = "会员号/手机号";
                    }
                    break;
            }
            textBox.Focusable = false;
        }

        public void XianShi() { }
        public void BuXianShi() { }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GoodsStore ps1 = (GoodsStore)SS_OrderGrid.SelectedItem;
            if (ps1 != null)
            {
                if (Goods_AddSum_Grid.Visibility == Visibility.Hidden)
                {
                    //收银小票一行对象
                    Goodsroder goodsroder = new Goodsroder();

                    int j = 0;
                    bool remark = true;//是否加入集合
                    foreach (Goodsroder ps12 in infoList)
                    {

                        if (ps12.Barcode.Equals(ps1.Barcode))
                        {
                            infoList[j].Num = infoList[j].Num + 1;
                            infoList[j].Sum = infoList[j].Num * ps12.Sale_price;
                            remark = false;
                            break;
                        }
                        j++;
                    }
                    if (remark)
                    {
                        goodsroder.Num = 1;
                        goodsroder.Name = ps1.Goods_name;
                        goodsroder.Barcode = ps1.Barcode;
                        goodsroder.Original_price = ps1.Original_price;
                        goodsroder.Sale_price = ps1.Sale_price;
                        goodsroder.Sum = goodsroder.Num * goodsroder.Sale_price;
                        infoList.Add(goodsroder);
                    }
                    remark = true;
                    GoodsAllNum = 0;
                    PriceSum = 0;
                    foreach (Goodsroder ps12 in infoList)
                    {
                        PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                        GoodsAllNum = GoodsAllNum + ps12.Num;
                    }
                    OrderGrid.ItemsSource = null;
                    PriceSumLabel.Content = PriceSum.ToString();//总金额
                    OrderGrid.AutoGenerateColumns = false;
                    OrderGrid.ItemsSource = infoList;
                    GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                    TCK_MHSS.Visibility = Visibility.Hidden;
                    Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                }
            }
            else
            {
               
                AddGoodsInfo good_add = new AddGoodsInfo();
                All_IN_GOODS_SUM = 0;
                ALL_OUT_PRICE = 0;
                ALL_IN_PRICE = 0;
                good_add.Num = 0;
                good_add.Name = ps1.Goods_name;
                good_add.Barcode = ps1.Barcode;
                good_add.Guige = ps1.Unit;
                good_add.Sale_price = ps1.Sale_price;
                good_add.Supplier1 = ps1.Supplier_id;
                good_add.GoodsId = ps1.Goods_id;
                good_add.Cost_price = ps1.Cost_price;
                InputWindows input = new InputWindows();
                input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                input.ShowDialog();
                if (input.Num != -1)
                {
                    good_add.Num = (int)input.Num;
                }
                else
                {
                    return;
                }
                addGoodsInfoslist.Add(good_add);
                AddGoods_DataGrid.ItemsSource = null;
                AddGoods_DataGrid.AutoGenerateColumns = false;
                AddGoods_DataGrid.ItemsSource = addGoodsInfoslist;
                foreach (AddGoodsInfo obj in addGoodsInfoslist)
                {
                    All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
                    ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
                    ALL_IN_PRICE = ALL_IN_PRICE + obj.Num * obj.Cost_price;
                }
                Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
            }
        }

        private void Button_Click_2(object sender, MouseButtonEventArgs e)
        {
            Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
            RuKu_TextBox.Text = "条码/拼音/商品名";
            TCK_MHSS.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<GoodsStore> GoodsStore = DataBaseControls.GetGoodsStore(TCK_TextBox.Text);
            SS_OrderGrid.ItemsSource = null;
            SS_OrderGrid.ItemsSource = GoodsStore;
            NowSSLabel.Content = "当前匹配数：" + GoodsStore.Count;
            if (GoodsStore.Count > 10)
            {
                NowSSLabelTS.Visibility = Visibility.Visible;
            }
            else
            {
                NowSSLabelTS.Visibility = Visibility.Visible;
            }
        }

        private void RuKuText_keyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.Key == Key.Enter)
            {

                List<GoodsStore> GoodsStore = DataBaseControls.GetGoodsStore(textBox.Text);
                SS_OrderGrid.ItemsSource = null;
                SS_OrderGrid.ItemsSource = GoodsStore;

                NowSSLabel.Content = "当前匹配数：" + GoodsStore.Count;
                if (GoodsStore.Count > 10)
                {
                    NowSSLabelTS.Visibility = Visibility.Visible;
                }
                else
                {
                    NowSSLabelTS.Visibility = Visibility.Hidden;
                }
                TCK_MHSS.Visibility = Visibility.Visible;


               

            }
        }

        private void RuKu_textIn(object sender, MouseEventArgs e)
        {
            RuKu_TextBox.Focusable = true;
            if (RuKu_TextBox.Text.Equals("条码/拼音/商品名"))
            {
                RuKu_TextBox.Text = "";
            }


        }

        private void Ruku_textOut(object sender, MouseEventArgs e)
        {
            RuKu_TextBox.Focusable = false;

            if (RuKu_TextBox.Text.Equals(""))
            {
                RuKu_TextBox.Text = "条码/拼音/商品名";
            }


        }

        private void Button_Click_1(object sender, MouseButtonEventArgs e)
        {
            GoodsStore ps1 = (GoodsStore)SS_OrderGrid.SelectedItem;
            if (ps1 != null)
            {
                if (Goods_AddSum_Grid.Visibility == Visibility.Hidden)
                {
                    //收银小票一行对象
                    Goodsroder goodsroder = new Goodsroder();

                    int j = 0;
                    bool remark = true;//是否加入集合
                    foreach (Goodsroder ps12 in infoList)
                    {

                        if (ps12.Barcode.Equals(ps1.Barcode))
                        {
                            infoList[j].Num = infoList[j].Num + 1;
                            infoList[j].Sum = infoList[j].Num * ps12.Sale_price;
                            remark = false;
                            break;
                        }
                        j++;
                    }
                    if (remark)
                    {
                        goodsroder.Num = 1;
                        goodsroder.Name = ps1.Goods_name;
                        goodsroder.Barcode = ps1.Barcode;
                        goodsroder.Original_price = ps1.Original_price;
                        goodsroder.Sale_price = ps1.Sale_price;
                        goodsroder.Sum = goodsroder.Num * goodsroder.Sale_price;
                        infoList.Add(goodsroder);
                    }
                    remark = true;
                    GoodsAllNum = 0;
                    PriceSum = 0;
                    foreach (Goodsroder ps12 in infoList)
                    {
                        PriceSum = PriceSum + (ps12.Sale_price * ps12.Num);
                        GoodsAllNum = GoodsAllNum + ps12.Num;
                    }
                    OrderGrid.ItemsSource = null;
                    PriceSumLabel.Content = PriceSum.ToString();//总金额
                    OrderGrid.AutoGenerateColumns = false;
                    OrderGrid.ItemsSource = infoList;
                    GoodsInfo_Label.Content = "共" + infoList.Count + "行，" + GoodsAllNum + "件商品";
                    TCK_MHSS.Visibility = Visibility.Hidden;
                    Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
                    UpdateHM(infoList, PriceSum);
                }
                else
                {

                    AddGoodsInfo good_add = new AddGoodsInfo();
                    All_IN_GOODS_SUM = 0;
                    ALL_OUT_PRICE = 0;
                    ALL_IN_PRICE = 0;
                    good_add.Num = 0;
                    good_add.Name = ps1.Goods_name;
                    good_add.Barcode = ps1.Barcode;
                    good_add.Guige = ps1.Unit;
                    good_add.Sale_price = ps1.Sale_price;
                    good_add.Supplier1 = ps1.Supplier_id;
                    good_add.GoodsId = ps1.Goods_id;
                    good_add.Cost_price = ps1.Cost_price;



                    InputWindows input = new InputWindows();
                    input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    input.ShowDialog();
                    if (input.Num != -1)
                    {
                        good_add.Num = (int)input.Num;
                    }
                    else
                    {
                        return;
                    }

                    bool remark = true;//是否加入集合
                    int j = 0;
                    foreach (AddGoodsInfo ps12 in addGoodsInfoslist)
                    {

                        if (ps12.Barcode.Equals(ps1.Barcode))
                        {
                            addGoodsInfoslist[j].Num = addGoodsInfoslist[j].Num + good_add.Num;
                            //addGoodsInfoslist[j].Sum = addGoodsInfoslist[j].Num * ps12.Sale_price;
                            remark = false;
                            break;
                        }
                        j++;
                    }
                    if (remark)
                    {
                        addGoodsInfoslist.Add(good_add);
                    }
                    AddGoods_DataGrid.ItemsSource = null;
                    AddGoods_DataGrid.AutoGenerateColumns = false;
                    AddGoods_DataGrid.ItemsSource = addGoodsInfoslist;
                    foreach (AddGoodsInfo obj in addGoodsInfoslist)
                    {
                        All_IN_GOODS_SUM = All_IN_GOODS_SUM + obj.Num;
                        ALL_OUT_PRICE = ALL_OUT_PRICE + obj.Num * obj.Sale_price;
                        ALL_IN_PRICE = ALL_IN_PRICE + obj.Num * obj.Cost_price;
                    }
                    Goods_AddSum_Grid_SumGoods.Content = All_IN_GOODS_SUM.ToString();
                    Goods_AddSum_Grid_SumOutPrice.Content = ALL_OUT_PRICE.ToString();
                    Goods_AddSum_Grid_SumInPrice.Content = ALL_IN_PRICE.ToString();
                    TCK_MHSS.Visibility = Visibility.Hidden;
                }
            }
           
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Main_TMPYSPM_TextBox.Text = "条码/拼音码/商品名称";
            RuKu_TextBox.Text = "条码/拼音/商品名";
            TCK_MHSS.Visibility = Visibility.Hidden;
        }

        public void UpdataXiao()
        {
            string str = "ListViewItem_KCYJ";
            switch (str)
            {
                case "ListViewItem_KCYJ":
                    Grid_System_XiaoXi.Visibility = Visibility.Visible;
                    ListViewItem_KCYJ.Background = ListViewItem_YES.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    // ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Visible;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    List<GoodsStore> ts = DataBaseControls.GetKunCunYuJing();
                    XiaoXi_KuCunYuJing.ItemsSource = null;
                    XiaoXi_KuCunYuJing.ItemsSource = ts;
                    break;
                case "ListViewItem_LQYJ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_YES.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
                case "ListViewItem_HLTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_YES.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Visible;
                    List<smc_order> list = DataBaseControls.GetOnlineOrder();
                    XiaoXi_HuoLiu.ItemsSource = null;
                    XiaoXi_HuoLiu.ItemsSource = list;
                    break;
                case "ListViewItem_HLiuTZ":
                    ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
                    //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
                    ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
                    //ListViewItem_HLiuTZ.Background = ListViewItem_YES.Background;
                    XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
                    XiaoXi_LinQi.Visibility = Visibility.Hidden;
                    XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
                    MessageBox.Show("敬请期待");
                    break;
            }
        }

        /// <summary>
        /// 退货提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
           
            TuiHuoTiShi.Visibility = Visibility.Hidden;
            //tuihuo
        }

        private void Close_TuiHuoTiShi(object sender, MouseButtonEventArgs e)
        {
            TuiHuoTiShi.Visibility = Visibility.Hidden;
        }

        private void Close_TuiHuoTiShi(object sender, RoutedEventArgs e)
        {
            TuiHuoTiShi.Visibility = Visibility.Hidden;
        }
    }
}
