using Client.Entity;
using Client.Entity.FuYouZhiFu;
using Client.Http;
using Client.Http.ForService;
using Client.JiaJie;
using Client.JiaJieMi;
using Client.Tool;
using Client.用户;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WxPayAPI;
using static Client.FristLogin;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!ConfigurationManager.AppSettings["Old_Username"].Equals("-1"))
            {
                UserName_TextBox.Text = ConfigurationManager.AppSettings["Old_Username"];
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        User user;

        /// <summary>
        /// 主窗口
        /// </summary>
        MainWin mainWin;

        public class test{

           public string req;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region 新增订单
            //Order order = new Order();
            //order.Order_number = "DOGS"+DateTime.Now.ToString("yyyyMMddhhmmss");
            //order.Actual_payment = 88;
            //order.Order_status = 1;
            //order.Pay_ment = "微信支付";
            //order.Customer_id = "会员id";
            //order.Order_source = "2";
            //order.Dept_id = 2;
            //order.Return_reason = "退款原因";
            //order.Customer_id = "会员id";
            //order.Order_type = 2;
            //order.Coupons_price = 1;
            //order.Product_all_price = 87;
            //order.Cancel_people_name = "核销人";
            //order.Create_by = "创建人";
            //order.Remark = "备注";
            //order.User_id = 2222;
            //DataBaseControls.AddOrder(order); 
            #endregion

            #region 新增订单详情
            //List<GoodsStore> list = new List<GoodsStore>();
            //for (int i = 0; i < 3; i++)
            //{
            //    GoodsStore goodsStore = new GoodsStore();
            //    goodsStore.Goods_id = i;
            //    goodsStore.Goods_store_id = i;
            //    goodsStore.Goods_name = "商品名" + i;
            //    goodsStore.Inventory_now = (i + 3).ToString();
            //    goodsStore.Original_price = i + 4;
            //    list.Add(goodsStore);
            //}
            //DataBaseControls.AddOrderInfo("DOGS20210220111230", "1", "备注1", list); 
            #endregion

            #region 新增商品
            //GoodsStore goodsStore = new GoodsStore();
            //goodsStore.Classify_id = 1;
            //goodsStore.Supplier_id = 2;

            //goodsStore.Goods_name = "商品名" + 3;
            //goodsStore.Pinyin_code = "pinyinma";
            //goodsStore.Barcode = "tiaoxingma";

            //goodsStore.Cost_price = 55;
            //goodsStore.Sale_price = 88;
            //goodsStore.Inventory_now = "80";
            //goodsStore.Inventory_min = "10";

            //goodsStore.Make_date = DateTime.Now;
            //goodsStore.Shelf_life = "9";
            //goodsStore.Remark = "详情";
            //DataBaseControls.AddNewGoods("222222", goodsStore); 

            #endregion

            #region 测试
            //更新库存
            //DataBaseControls.UpdataStock(false,2,"1");

            //微信退款
           // Refund.Run("4200000886202102189850240682", "160493226420210218092041630","1","1");

            //微信支付
            //MicroPay.Run("TESTPAY2", "1", "134553051740940008"); 
            #endregion

            Log_Local.LOG("1",1,"123456789");

        }

        public static string ExceptBlanks( string str)
        {
            int _length = str.Length;
            if (_length > 0)
            {
                StringBuilder _builder = new StringBuilder(_length);
                for (int i = 0; i < str.Length; i++)
                {
                    char _c = str[i];
                    //switch (_c)
                    //{
                    //    case '\r':
                    //    case '\n':
                    //    case '\t':
                    //    case ' ':
                    //        continue;
                    //    default:
                    //        _builder.Append(_c);
                    //        break;
                    //}
                    if (!char.IsWhiteSpace(_c))
                        _builder.Append(_c);
                }
                return _builder.ToString();
            }
            return str;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            //MP3Play.Play(ConfigurationManager.AppSettings["MP3Path"]);
            #region 测试

            //FYZF.Pay(Tool_Somthing.GetOrderNumber(),"1", "ALIPAY", "282706625207026867","测试");

            // FYZF.testpay();

            #endregion

            try
            {
                if (UserName_TextBox.Text.Equals(""))
                {
                    MessageBox.Show("用户名不能为空！");
                    return;
                }
                if (Password_TextBox.Password.Equals(""))
                {
                    MessageBox.Show("密码不能为空！");
                    return;
                }
                user = DataBaseControls.GetUser(UserName_TextBox.Text);
                if (user.Password == null)
                {
                    Log_Local.LOG(UserName_TextBox.Text, 1, "Fail");
                    MessageBox.Show("请确认用户名和密码是否正确", "用户名或密码错误");
                    return;
                }
                if (user.Password.Equals(Password_TextBox.Password))
                {
                    Log_Local.LOG(user.User_id.ToString(), 1, "Success");
                    mainWin = new MainWin(user);
                    mainWin.Show();
                    this.Hide();
                    return;
                }
                else
                {
                    MessageBox.Show("请确认用户名和密码是否正确", "用户名或密码错误");
                    Log_Local.LOG(user.User_id.ToString(), 1, "Fail");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常：" + ex.ToString());

            }
        }

        private void Close_This(object sender, MouseButtonEventArgs e)
        {
          
            try
            {
                // App.Current.Shutdown();
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                System.Environment.Exit(0);
            }
        }


    }
}
