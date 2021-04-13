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
    /// XXTZWindows.xaml 的交互逻辑
    /// </summary>
    public partial class XXTZWindows : Window
    {
        public XXTZWindows()
        {
            InitializeComponent();
        }
        public delegate void NewDelegate();

        public static event NewDelegate Update;

        private void Close_Windows(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ToXiaoXiTongZhi(object sender, MouseButtonEventArgs e)
        {
            Update();
            this.Close();
            
        }
    }
}
