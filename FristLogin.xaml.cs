using Client.Http;
using Client.Http.ForService;
using Client.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// FristLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FristLogin : Window
    {
        public FristLogin()
        {
            InitializeComponent();
            DeviceID = ConfigurationManager.AppSettings["DeviceId"];
            if (!DeviceID.Equals("-1"))
            {
                

                fristLogin = new MainWindow();


                Application.Current.MainWindow = fristLogin;
                this.Close();
                fristLogin.Show();
            }
            else
            {

            }
        }

        MainWindow fristLogin;
        Loading loading;
        static string DeviceID;

        public class Device
        {
            public string deptId;
        }
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            string str = UserName.Text;

            Device device = new Device();
            device.deptId = str;

            if (GetUser(device))
            {
                loading = new Loading(device);
                loading.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("请核验门店Code，重新输入。", "错误！");
            }


        }
        public bool GetUser(Device device)
        {
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
                
                for (int i = 0; i < list.Count; i++)
                {
                    
                    try
                    {
                        if (!DataBaseControls.AddUser(list[i].UserId.ToString(), list[i].UserType, list[i].Dept_Id.ToString(), list[i].UserName, list[i].NickName, list[i].Phonenumber, list[i].Password, list[i].Status, list[i].Remark))
                        {
                            Log_Local.LOG("新增用户出错", 101, list[i].UserId.ToString());
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    re = true;
                }

               
            }
            catch (Exception ex)
            {

                return re;
            }
            return re;
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

        private void InTextBox(object sender, MouseEventArgs e)
        {
            if (UserName.Text.Equals("请输入门店编号"))
            {
                UserName.Text = "";
            } 
        }

        private void OutTextBox(object sender, MouseEventArgs e)
        {
            if (UserName.Text.Equals(""))
            {
                UserName.Text = "请输入门店编号";
            }
        }


        string StoreManage = ConfigurationManager.AppSettings["StoreManage"];

        MainWindow login;
        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            string un = UserName.Text;
            //string pw= Password_TextBox.Password;
            // loading = new Loading();
            this.Visibility = Visibility.Hidden;
            // loading.ShowDialog();
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

        private void Close_this(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
