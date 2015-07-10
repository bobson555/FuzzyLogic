using System.ComponentModel;
using System.Windows;

namespace WeatherClothes.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game g = new Game();
            Visibility = Visibility.Collapsed;
            g.ParentView = this;
            g.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
