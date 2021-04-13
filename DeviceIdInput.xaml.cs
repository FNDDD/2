using Client.Http;
using Client.Http.DataEntity;
using Client.Http.ForService;
using Client.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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

namespace Client
{
    /// <summary>
    /// DeviceIdInput.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceIdInput : Window
    {
        public DeviceIdInput()
        {
            InitializeComponent();
            DeviceID = ConfigurationManager.AppSettings["DeviceId"];
            if (!DeviceID.Equals("-1"))
            {


                fristLogin = new MainWindow();

               
                Application.Current.MainWindow = fristLogin;
                this.Close();
                fristLogin.Show();
                //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

                //t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

                //t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
                //i = 0;

               
            }
            else
            {
               
            }
        }
        int i = 0;
        System.Timers.Timer t = new System.Timers.Timer(110);//实例化Timer类，设置间隔时间为10000毫秒；

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            i++;
            if (i == 5)
            {
                try
                {
                    UpdateShowWindow();
                }
                catch (Exception ex)
                {

                }
                t.AutoReset = false;
                t.Enabled = false;
                
               

            }


        }

        MainWindow fristLogin;
        Loading loading;
        static string DeviceID;

        public class Device
        {
            public string deptId;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          string str=  ID_TextBox.Text;
           
            Device device = new Device();
            device.deptId = str;
            //loading = new Loading(device);
            loading.Show();
            this.Close();
            #region 获取用户
            //string objs = HttpTool.doHttpPost("/cash/user/list", device.ToJSON());
            //JavaScriptSerializer Jss = new JavaScriptSerializer();
            //Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(objs);
            //string objstr = string.Empty;
            //if (DicText.ContainsKey("data"))
            //{
            //    object obj = DicText["data"];
            //    objstr = obj.ToJSON();
            //}
            //List<CashUser> list = JsonTool.FromJSON<List<CashUser>>(objstr);
            //foreach (CashUser cashUser in list)
            //{
            //    try
            //    {
            //        if (!DataBaseControls.AddUser(cashUser.UserId.ToString(), cashUser.ShopId.ToString(), cashUser.UserName, cashUser.NickName, cashUser.Phonenumber, cashUser.Password, cashUser.Status, cashUser.Remark))
            //        {
            //            Log_Local.LOG("新增用户出错", 101, cashUser.UserId.ToString());
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //} 
            #endregion



            #region 获取商品
            //string Goods = HttpTool.doHttpPost("/cash/goods/list", device.ToJSON());
            //ForServiceObject OBJ = JsonTool.FromJSON<ForServiceObject>(Goods);
            //foreach (SmcGoodsStore goods in OBJ.Data)
            //{
            //    try
            //    {
            //        if (!DataBaseControls.InsertGoods(goods))
            //        {
            //            Log_Local.LOG("新增商品出错", 101, goods.GoodsId.ToString());
            //        }
            //        Thread.Sleep(50);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //} 
            #endregion

        }


        private delegate void ShowWindow();

        private void UpdateShowWindow()
        {
            this.fristLogin.Dispatcher.BeginInvoke(new ShowWindow(ShowWindows));
        }

        private void ShowWindows()
        {
            fristLogin.Show();
            this.Hide();
        }
    }
}
