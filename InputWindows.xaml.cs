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

namespace Client
{
    /// <summary>
    /// InputWindows.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindows : Window
    {
        public InputWindows()
        {
            InitializeComponent();
        }
       public double Num = -1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
             Num = -1;
            try
            {
                 Num = Convert.ToDouble(Num_TextBox.Text);
            }
            catch (Exception ex)
            { 
            }
            // GetNum(Num);
            this.Close();
        }
        public delegate void UpHandler(double Num);
        public event UpHandler GetNum;
    }
}
