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

namespace WeatherClothes.Views
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Result : Window
    {
        Explaination E;
        Attribute T;
        Attribute W;
        Attribute H;
        Attribute R;
        double[] Values;
        public Result(Attribute T,Attribute W,Attribute H,Attribute R,double[] Values)
        {
            this.Values = Values;
            this.T = T;
            this.W = W;
            this.H = H;
            this.R = R;
            Initialize();
            InitializeComponent();
        }
        public void Initialize()
        {
            E = new Explaination(T, W, H, R, Values);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            E.ShowDialog();
            Initialize();

            
        }
    }
}
