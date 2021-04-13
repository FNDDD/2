using Apache.NMS;
using Client.ActiveMQ;
using Client.Entity;
using Client.Http;
using Client.Http.DataEntity;
using Client.Http.ForService;
using Client.Tool;
using System;
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
using static Client.FristLogin;

namespace Client
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : Window
    {
        public Loading(Device dvc)
        {
            InitializeComponent();
            device = dvc;
            fristLogin = new MainWindow();
            fristLogin2 = new FristLogin();
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            i = 0;
        }
        System.Timers.Timer t = new System.Timers.Timer(1000);//实例化Timer类，设置间隔时间为10000毫秒；
        int i = 0;
        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            i++;
            if (i==5)
            {
                t.AutoReset = false;
                t.Enabled = false;
                GetDeptInfor();

                GetGoods();
                MessageBox.Show("店员信息和商品信息已经同步完成，接下来可以正常收银了。", "加载完成!");
                SetValue("DeviceId", device.deptId);
                SetValue("ActiveMQ_Subject", "queue_"+ device.deptId);
                UpdateShowWindow();

            }
           

        }

        FristLogin fristLogin2;

        MainWindow fristLogin;

        Device device;

        public bool GetUser(){
            bool re = false;
            string objs = HttpTool.doHttpPost("/cash/user/list", device.ToJSON());
            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(objs);
                string objstr = string.Empty;
                if (DicText.ContainsKey("data"))
                {
                    object obj = DicText["data"];
                    objstr = obj.ToJSON();
                }
                List<CashUser> list = JsonTool.FromJSON<List<CashUser>>(objstr);
                Update1Info(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        if (!DataBaseControls.AddUser(list[i].UserId.ToString(), list[i].UserType, list[i].Dept_Id.ToString(), list[i].UserName, list[i].NickName, list[i].Phonenumber, list[i].Password, list[i].Status, list[i].Remark))
                        {
                            Log_Local.LOG("新增用户出错", 101, list[i].UserId.ToString());
                        }
                        ProgressBarLoad(i);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                ProgressBarLoad(0);
                re = true;
            }
            catch (Exception ex)
            {

                return re;
            }
            return re;
        } 

        public bool GetGoods()
        {
            bool re = false;
            string Goods = HttpTool.doHttpPost("/cash/goods/list", device.ToJSON());
            ForServiceObject OBJ = JsonTool.FromJSON<ForServiceObject>(Goods);
            ProgressBarLoad(0);
            Update1Info(OBJ.Data.Count);
            for (int i=0;i< OBJ.Data.Count;i++)
            {
                try
                {
                    if (OBJ.Data[i].ShopId.Equals(device.deptId)) {
                    if (!DataBaseControls.InsertGoods(OBJ.Data[i]))
                    {
                        Log_Local.LOG("新增商品出错", 101, OBJ.Data[i].GoodsId.ToString());
                    }
                    Thread.Sleep(10);
                    }
                    ProgressBarLoad(i);
                }
                catch (Exception ex)
                {
                }
            }
            ProgressBarLoad(0);

            return re;
        }

        public void GetDeptInfor()
        {
            string objs = HttpTool.doHttpPost("/cash/shop/getInfo", device.ToJSON());
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(objs);
            string objstr = string.Empty;
            if (DicText.ContainsKey("data"))
            {
                object obj = DicText["data"];
                objstr = obj.ToJSON();
            }
            MenDian list = JsonTool.FromJSON<MenDian>(objstr);
            SetValue("StoreName", list.DeptName);
            SetValue("Phone", list.DeptNumber);
            SetValue("Address", list.Address);
            SetValue("PRIVATE_KEY", list.Secret_key);
        }

        /// <summary>
        /// 消息中间件对象
        /// </summary>
        public MQ mQ;

        public void ProgressBarLoad(int i)
        {

                double value = i;
                ProgressBar_Load.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(ProgressBar_Load.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, value);

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mQ = new MQ();
            mQ.InitActiveMQ();
            mQ.Consumer.Listener += new MessageListener(consumer_Listener);
        }

        public static void consumer_Listener(IMessage message)
        {
            try
            {
               ITextMessage msg = (ITextMessage)message;
               MessageObj messageObj= MQTool.GetObj(msg.Text);
                if (MQTool.CheckSign(messageObj))
                {
                    switch (messageObj.ServiceType)
                    {
                        case "1"://同步会员信息
                          //获取集合

                            //遍历

                                //插入会员表
                                    //成功
                                    //失败   记录

                            //插入服务端消息表  标识消费成功   回复服务端消费成功


                            //异常   插入服务端消息表  标识消费失败   回复服务端消费失败
                            break;
                        case "2"://同步用户信息
                            //获取集合

                            //遍历

                                //插入用户表
                                    //成功
                                    //失败   记录

                            //插入服务端消息表  标识消费成功   回复服务端消费成功

                            //异常   插入服务端消息表  标识消费失败   回复服务端消费失败
                            break;
                        case "3"://同步供应商
                            break;
                        case "4"://同步商品类型
                            break;
                        case "5"://同步商品
                            break;
                        case "6"://更新供应商
                            break;
                        case "7"://更新商品类型
                            break;
                        case "8"://跟更新商品信息
                            break;
                        case "9"://线上订单
                            break;
                        default:
                            break;

                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

        private delegate void DispList2Data(int data);
        //线程调用方法
        private void Update1Info(int data)
        {
            this.ProgressBar_Load.Dispatcher.BeginInvoke(new DispList2Data(Update1Action), data);
        }
        //定义委托指向的方法
        private void Update1Action(int data)
        {
            ProgressBar_Load.Maximum = data;
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


        private delegate void ShowWindow2();

        private void UpdateShowWindow2()
        {
            this.fristLogin2.Dispatcher.BeginInvoke(new ShowWindow(ShowWindows2));
        }

        private void ShowWindows2()
        {
            fristLogin2.Show();
            this.Hide();
        }
    }
}
