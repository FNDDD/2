using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Client.Entity;
using Client.Http;
using Client.Http.ForService;
using Client.TiShi;
using Client.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Client.ActiveMQ
{
    public class MQ
    {
        public IConnectionFactory factorys;

        public IConnection connection;

        /// <summary>
        /// 生产者
        /// </summary>
        public IMessageProducer Producer;

        /// <summary>
        /// 消费者
        /// </summary>
        public IMessageConsumer Consumer;

        /// <summary>
        /// 初始化消息中间件
        /// </summary>
        /// <returns></returns>
        public bool InitActiveMQ()
        {
            bool re = false;
            try
            {
                factorys = new ConnectionFactory("tcp://" + ConfigurationManager.AppSettings["ActiveMQ_Path"] + "/");
                connection = factorys.CreateConnection(ConfigurationManager.AppSettings["ActiveMQ_UserName"], ConfigurationManager.AppSettings["ActiveMQ_PassWord"]);
                
                connection.Start();

                //创建会话
                ISession session = connection.CreateSession();
                    
                        //创建消费者
                        Consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(ConfigurationManager.AppSettings["ActiveMQ_Subject"]));

                        Consumer.Listener += new MessageListener(consumer_Listener);

                    

                //创建会话   
                ISession sessions = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                    
                        //创建生产者 
                        Producer = sessions.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(ConfigurationManager.AppSettings["ActiveMQ_Subject"]));
                    


            }
            catch (Exception ex)
            {
                Log_Local.LOG("初始化消息中间件", 200, "初始化消息中间件异常");
            }
            return re;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            bool re = false;
            try
            {
                connection.Stop();
                connection.Close();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("关闭连接", 201, ex.ToString());
            }
            return re;
        }

        public static void consumer_Listener(IMessage message)
        {
            try
            {
                ITextMessage msg = (ITextMessage)message;
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(msg.Text);
                string objstr = string.Empty;
                int id = 0;
                string type = string.Empty;
                if (DicText.ContainsKey("object"))
                {
                    object obj = DicText["object"];
                    objstr = obj.ToJSON();
                }

                if (DicText.ContainsKey("id"))
                {
                     id = (int)DicText["id"];
                   
                }

                if (DicText.ContainsKey("serviceType"))
                {
                     type = DicText["serviceType"].ToString();
                    
                }

                

                switch (type)
                {
                    case "1-1":
                        CashUser messageObj1 = JsonTool.FromJSON<CashUser>(objstr);
                        if (!DataBaseControls.AddUser(messageObj1.UserId.ToString(), messageObj1.UserType, messageObj1.Dept_Id.ToString(), messageObj1.UserName, messageObj1.NickName, messageObj1.Phonenumber, messageObj1.Password, messageObj1.Status, messageObj1.Remark))
                        {
                            Log_Local.LOG("新增用户出错", 101, messageObj1.UserId.ToString());
                        }
                        break;
                    case "1-2":
                       
                        if (!DataBaseControls.DeleteUser(objstr))
                        {
                            Log_Local.LOG("删除用户出错", 101, objstr);
                        }

                        break;
                    case "1-3":
                        CashUser messageObj3 = JsonTool.FromJSON<CashUser>(objstr);

                        if (!DataBaseControls.UpdataUser(messageObj3.UserId.ToString(), messageObj3.UserType, messageObj3.Dept_Id.ToString(), messageObj3.UserName, messageObj3.NickName, messageObj3.Phonenumber, messageObj3.Password, messageObj3.Status, messageObj3.Remark))
                        {
                            Log_Local.LOG("修改用户出错", 101, messageObj3.UserId.ToString());
                        }
                        break;
                    case "2-1":
                        SmcGoodsStore smcGoodsStore= JsonTool.FromJSON<SmcGoodsStore>(objstr);

                        if (!DataBaseControls.InsertGoods(smcGoodsStore))
                        {
                            Log_Local.LOG("增加商品出错", 101, smcGoodsStore.GoodsId.ToString());
                        }
                        break;
                    case "2-2":
                       

                        if (!DataBaseControls.DeleteGoods(objstr))
                        {
                            Log_Local.LOG("删除商品出错", 101, objstr);
                        }
                        break;
                    case "2-3":
                        SmcGoodsStore smcGoodsStore3 = JsonTool.FromJSON<SmcGoodsStore>(objstr);

                        if (!DataBaseControls.UpdateGoods(smcGoodsStore3))
                        {
                            Log_Local.LOG("修改商品出错", 101, smcGoodsStore3.GoodsId.ToString());
                        }
                        break;
                    case "3-1":

                        //线上订单
                        smc_order smcorder = JsonTool.FromJSON<smc_order>(objstr);
                        //订单信息入库
                        if (!DataBaseControls.AddOnlineOrder(smcorder.Id, smcorder.OrderType==1?"供销优选":"商超便利", smcorder.OrderNumber, smcorder.ActualPayment.ToString(), smcorder.CustomerId.ToString(), smcorder.NickName, smcorder.CustomerPhone, smcorder.DeliveryDate))
                        {
                            Log_Local.LOG("新增线上订单出错", 101, smcorder.OrderNumber.ToString());
                        }
                        TestMP3 testMP3 = new TestMP3();
                        testMP3.Play();
                        //播放声音
                       // MP3Play.Play();
                        xXTZWindows = new XXTZWindows();
                        //弹出窗口
                        UpdateShowWindow();
                        break;

                    case "3-2"://删除

                        //线上订单
                       
                        //订单信息入库
                        if (!DataBaseControls.UpdataOnlineOrder(objstr, "2",""))
                        {
                            Log_Local.LOG("删除线上订单出错", 101, objstr);
                        }
                        //订单信息入库

                        //播放声音
                       // MP3Play.Play();
                        //弹出窗口
                       // UpdateShowWindow();
                        break;
                    case "3-3"://修改

                        //线上订单

                        //订单信息入库

                        //播放声音
                        MP3Play.Play();
                        //弹出窗口
                        UpdateShowWindow();
                        break;
                    case "4-3"://修改店铺
                        MenDian list = JsonTool.FromJSON<MenDian>(objstr);
                        SetValue("StoreName", list.DeptName);
                        SetValue("Phone", list.DeptNumber);
                        SetValue("Address", list.Address);
                        SetValue("PRIVATE_KEY", list.Secret_key);
                        break;

                    case "5-1"://修改店铺
                        


                        break;
                }
                Receive receive = new Receive();
                receive.id = id;
                HttpTool.doHttpPost("/cash/mq/receive", receive.ToJSON());
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void UpDataMQ(AddSmcUser messageObj)
        {
            switch (messageObj.ServiceType)
            {
                case "1-1":
                    if (!DataBaseControls.AddUser(messageObj.Object.UserId.ToString(), messageObj.Object.UserType, messageObj.Object.Dept_Id.ToString(), messageObj.Object.UserName, messageObj.Object.NickName, messageObj.Object.Phonenumber, messageObj.Object.Password, messageObj.Object.Status, messageObj.Object.Remark))
                    {
                        Log_Local.LOG("新增用户出错", 101, messageObj.Object.UserId.ToString());
                    }
                    break;
                case "1-2":
                    if (!DataBaseControls.DeleteUser(messageObj.Object.UserId.ToString()))
                    {
                        Log_Local.LOG("删除用户出错", 101, messageObj.Object.UserId.ToString());
                    }
                    Console.Write("^^^^^^^^^^^^^^^^删除#############");
                    break;
                case "1-3":
                    Console.Write("^^^^^^^^^^^^^^^^修改#############");
                    if (!DataBaseControls.UpdataUser(messageObj.Object.UserId.ToString(), messageObj.Object.UserType, messageObj.Object.Dept_Id.ToString(), messageObj.Object.UserName, messageObj.Object.NickName, messageObj.Object.Phonenumber, messageObj.Object.Password, messageObj.Object.Status, messageObj.Object.Remark))
                    {
                        Log_Local.LOG("修改用户出错", 101, messageObj.Object.UserId.ToString());
                    }
                    break;
            }
            Receive receive = new Receive();
            receive.id = messageObj.Id;

            HttpTool.doHttpPost("/cash/user/receive", receive.ToJSON());
        }

        public class Receive
            {
           public long id;

        }

        private delegate void ShowWindow();

        public static XXTZWindows xXTZWindows =new XXTZWindows();

        private static void UpdateShowWindow()
        {
            xXTZWindows.Dispatcher.BeginInvoke(new ShowWindow(ShowWindows));
        }

        private static void ShowWindows()
        {
            xXTZWindows.ShowDialog();//弹出框 
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
    }
}
