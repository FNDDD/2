using Client.Tool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace Client
{
    /// <summary>
    /// HouMianWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HouMianWindow : Window
    {
        public HouMianWindow()
        {
            InitializeComponent();
            Screen[] _screens = Screen.AllScreens;
            Screen s;
            if (_screens.Length > 1)
            {
                s = Screen.AllScreens[1];
            }
            else
            {
                s = Screen.AllScreens[0];
            }
            System.Drawing.Rectangle rect = s.WorkingArea;
            this.Left = rect.Left;
            this.Top = rect.Top;
            MainWin.UpdateHM += MainWin_UpdateHM;
            timer = Convert.ToInt32(ConfigurationManager.AppSettings["Timer"]);
            t = new System.Timers.Timer(timer * 1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
           // thread = new Thread(Play);
            string mp3path = ConfigurationManager.AppSettings["MP3Path"];
            Play();
        }

        public void Play()
        {
            string mp3path = ConfigurationManager.AppSettings["MP3Path"];
            MP3Play.Play(mp3path);
        }

        static int timer;
        int Isleft;
        int IsDouble;
        static int oldtimer;
        int oldIsleft;
        int oldIsDouble;
        bool IsUpdate = false;
        string path = ConfigurationManager.AppSettings["ImgPath"];
        /// <summary>
        /// 更新时间
        /// </summary>
        System.Timers.Timer t ;
       public static string pathNow ="";
        private void MainWin_UpdateHM(List<Goodsroder> infoList, double tempPrice)
        {
            Goods_Info.Items.Clear();
            Num_XvHao.Items.Clear();
            Sum.Content = "￥" + tempPrice.ToString("0.00");
            for (int i=0;i< infoList.Count;i++)
            {
                ListViewItem listView1 = new ListViewItem();
                listView1.Height = 25;
                listView1.FontSize = 16;
                listView1.BorderBrush = Cope1.BorderBrush;
                listView1.Content = infoList[i].Name;
                Goods_Info.Items.Add(listView1);
                ListViewItem listView2 = new ListViewItem();
                listView2.Height = 25;
                listView2.FontSize = 16;
                listView2.BorderBrush = Cope2.BorderBrush;
                listView2.Content = "￥"+infoList[i].Sale_price.ToString("0.00")+ "           "+
                    infoList[i].Num.ToString()+ "          ￥"+ infoList[i].Sum.ToString("0.00");
                Goods_Info.Items.Add(listView2);
                ListViewItem listView3 = new ListViewItem();
                listView3.Content = "  " + (i+1).ToString();
                listView3.Height = 50;
                listView3.FontSize = 20;
                listView3.BorderBrush = Cope1.BorderBrush;
                Num_XvHao.Items.Add(listView3);
            }

        }
        int i = 1;

        //Thread thread;

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            
            path = ConfigurationManager.AppSettings["ImgPath"];
            Isleft= Convert.ToInt32(ConfigurationManager.AppSettings["IsLeft"]);
            IsDouble = Convert.ToInt32(ConfigurationManager.AppSettings["IsDouble"]);
            timer= Convert.ToInt32(ConfigurationManager.AppSettings["Timer"]);

            if (oldtimer!= timer|| oldIsleft != Isleft || oldIsDouble != IsDouble)
            {
                IsUpdate = true;
            }
            oldtimer = timer;
            oldIsleft = Isleft;
            oldIsDouble = IsDouble;
            UpdateShowNowTime();
        }

        JArray ja = new JArray(); //定义一个数组
        public string info = string.Empty;

        protected string GetPath()
        {
           
           // var path1 = System.AppDomain.CurrentDomain.BaseDirectory;//获取程序集目录
            //string path = System.IO.Path.Combine(path);//Path.Combine 将3个字符串组合成路径
            var images = Directory.GetFiles(path, ".", SearchOption.AllDirectories).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".gif"));
            //images = Directory.GetFiles(path, "*.png|*.jpg", SearchOption.AllDirectories);
            //Directory.GetFiles 返回指定目录的文件路径 SearchOption.AllDirectories 指定搜索当前目录及子目录
            if (i >= images.Count())
            {
                i = 1;
            }
            string str = "";
            int index = 0;
            //遍历string 型 images数组
            foreach (var j in images)
            {
                index++;
                
                //var str = j.Replace(path1, "");//获取相对路径
                var path2 = j.Replace("\\", "/");// 将字符“\\”转换为“/”
                if (index==i) {
                    str = path2.ToString();
                  
                    i++;
                    return str;
                }
                
               // ja.Add(path2);
            }
            return str;
           // info = Newtonsoft.Json.JsonConvert.SerializeObject(ja);//序列化为String  
        }

        private delegate void ShowNowTime();

        private void UpdateShowNowTime()
        {
            this.GuangGao.Dispatcher.BeginInvoke(new ShowNowTime(UpShowNowTime));
        }



        private void UpShowNowTime()
        {

            try
            {
                string sre = GetPath();

                BitmapImage imgSource = new BitmapImage(new Uri(sre));
                if (IsUpdate)
                {
                    // GetPath();
                    Play();

                if (Isleft == 0)
                {
                    GuangGao.Width = 976;
                    GuangGao.Margin = new Thickness(380, 0, 0, 0);
                }
                else
                {
                    GuangGao.Width = 1366;
                    GuangGao.Margin= new Thickness(0 ,0, 0, 0);
                }

                if (IsDouble == 1)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                   
                }
 
                t.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Timer"])*1000;
                    IsUpdate = false;
                }
                GuangGao.Source = imgSource;
            }
            catch (Exception ex)
            {

            }
           // i++;
        }
    }
}
