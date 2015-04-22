using FuzzySets;
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
       
        SingleDimFuzzySet[] Sets;
        public Tuple<string, string> Tata { get; set; }
        public Tuple<string, string> Mama { get; set; }
        public Tuple<string, string> Babcia { get; set; }
        public Tuple<string, string> Dziadek { get; set; }
        double[] Values;
        public Result(Attribute T,Attribute W,Attribute H,Attribute R,double[] Values,string[] labels,SingleDimFuzzySet[] Sets)
        {
            this.Values = Values;
            this.T = T;
            this.W = W;
            this.H = H;
            this.R = R;
            this.Sets = Sets;
            DataContext = this;
            Tata = GetLabelDescription(labels[0]);
            Mama = GetLabelDescription(labels[1]);
            Babcia = GetLabelDescription(labels[2]);
            Dziadek = GetLabelDescription(labels[3]);
            Initialize();
            InitializeComponent();
        }
        private Tuple<string, string> GetLabelDescription(string label)
        {
            switch (label.ToLower())
            {
                case "spring":
                    return new Tuple<string, string>("/wiosna5.png", "Ubranie Wiosenne");
                case "winter":
                    return new Tuple<string, string>("/zima5.png", "Ubranie Zimowe");
                case "summer":
                    return new Tuple<string, string>("/lato5.png", "Ubranie Letne");
                default:
                    throw new ArgumentException("Unrecognised label");

            }
        }
        public void Initialize()
        {
            E = new Explaination(T, W, H, R, Values,Sets);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            E.ShowDialog();
            this.Visibility = System.Windows.Visibility.Visible;
            Initialize();

            
        }
        public Window ParentView { get; set; }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ParentView != null) ParentView.Visibility = System.Windows.Visibility.Visible;
        }
        
    }
}
