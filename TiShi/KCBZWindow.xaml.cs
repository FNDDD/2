using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.TiShi
{
    /// <summary>
    /// KCBZWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KCBZWindow : Window
    {
        public KCBZWindow()
        {
            InitializeComponent();
        }

        private void Close_Windows(object sender, MouseButtonEventArgs e)
        {
           this.Close();
        }
       public  delegate void NewDelegate();

       public event NewDelegate Update;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string str = "";
            Update();



        //switch (listView.Name)
        //{
        //    case "ListViewItem_KCYJ":
        //        ListViewItem_KCYJ.Background = ListViewItem_YES.Background;
        //        //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
        //        ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
        //        // ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
        //        XiaoXi_KuCunYuJing.Visibility = Visibility.Visible;
        //        XiaoXi_LinQi.Visibility = Visibility.Hidden;
        //        XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
        //        List<GoodsStore> ts = DataBaseControls.GetKunCunYuJing();
        //        XiaoXi_KuCunYuJing.ItemsSource = null;
        //        XiaoXi_KuCunYuJing.ItemsSource = ts;
        //        break;
        //    case "ListViewItem_LQYJ":
        //        ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
        //        //ListViewItem_LQYJ.Background = ListViewItem_YES.Background;
        //        ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
        //        //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
        //        XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
        //        XiaoXi_LinQi.Visibility = Visibility.Hidden;
        //        XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
        //        MessageBox.Show("敬请期待");
        //        break;
        //    case "ListViewItem_HLTZ":
        //        ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
        //        //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
        //        ListViewItem_HLTZ.Background = ListViewItem_YES.Background;
        //        //ListViewItem_HLiuTZ.Background = ListViewItem_NO.Background;
        //        XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
        //        XiaoXi_LinQi.Visibility = Visibility.Hidden;
        //        XiaoXi_HuoLiu.Visibility = Visibility.Visible;
        //        List<smc_order> list = DataBaseControls.GetOnlineOrder();
        //        XiaoXi_HuoLiu.ItemsSource = null;
        //        XiaoXi_HuoLiu.ItemsSource = list;
        //        break;
        //    case "ListViewItem_HLiuTZ":
        //        ListViewItem_KCYJ.Background = ListViewItem_NO.Background;
        //        //ListViewItem_LQYJ.Background = ListViewItem_NO.Background;
        //        ListViewItem_HLTZ.Background = ListViewItem_NO.Background;
        //        //ListViewItem_HLiuTZ.Background = ListViewItem_YES.Background;
        //        XiaoXi_KuCunYuJing.Visibility = Visibility.Hidden;
        //        XiaoXi_LinQi.Visibility = Visibility.Hidden;
        //        XiaoXi_HuoLiu.Visibility = Visibility.Hidden;
        //        MessageBox.Show("敬请期待");
        //        break;
        //}
        }
    }
}
