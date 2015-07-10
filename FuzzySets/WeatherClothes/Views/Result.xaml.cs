using System;
using System.ComponentModel;
using System.Windows;
using FuzzySets;

namespace WeatherClothes.Views
{
    
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    
    public partial class Result
    {
        Explaination _e;
        readonly Attribute _t;
        readonly Attribute _w;
        readonly Attribute _h;
        readonly Attribute _r;

        readonly SingleDimFuzzySet[] _sets;
        public Tuple<string, string> Tata { get; set; }
        public Tuple<string, string> Mama { get; set; }
        public Tuple<string, string> Babcia { get; set; }
        public Tuple<string, string> Dziadek { get; set; }
        readonly double[] _values;
        public Result(Attribute T,Attribute w,Attribute h,Attribute r,double[] values,string[] labels,SingleDimFuzzySet[] sets)
        {
            _values = values;
            _t = T;
            _w = w;
            _h = h;
            _r = r;
            _sets = sets;
            DataContext = this;
            Tata = GetLabelDescription(labels[0]);
            Mama = GetLabelDescription(labels[1]);
            Babcia = GetLabelDescription(labels[2]);
            Dziadek = GetLabelDescription(labels[3]);
         
            InitializeComponent();
        }
        private Tuple<string, string> GetLabelDescription(string label)
        {
            switch (label.ToLower())
            {
                case "spring":
                    return new Tuple<string, string>("/wiosna5.png", "Spring Clothing");
                case "winter":
                    return new Tuple<string, string>("/zima5.png", "Winter Clothing");
                case "summer":
                    return new Tuple<string, string>("/lato5.png", "Summer Clothing");
                default:
                    throw new ArgumentException("Unrecognised label");

            }
        }
        public void Initialize()
        {
            _e = new Explaination(_t, _w, _h, _r, _values, _sets) {ParentView = this};
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Initialize();
            Visibility = Visibility.Collapsed;
            _e.Show();

        

            
        }
        public Window ParentView { get; set; }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ParentView != null)
            {
                ParentView.Visibility = Visibility.Visible;
                ParentView.WindowState = WindowState.Normal;
            }
        }
        
    }
}
