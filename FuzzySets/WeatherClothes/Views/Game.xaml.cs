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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        Attribute Temperature;
        Attribute WindSpeed;
        Attribute Moisture;
        Attribute Clothes;

        public Game()
        {
            InitializeComponent();
            TextBlock1.Text = "Temperatura\notoczenia";
            TextBlock2.Text = "Prędkość\nwiatru";
            TextBlock3.Text = "Wilgotność\npowietrza";

            var temperatureList = new List<String> { "Cold", "Warm", "Hot" };
            var windSpeedList = new List<String> { "None", "Mild", "Strong" };
            var moistureList = new List<String> { "Dry", "Wet", "Rain" };
            var clothesList = new List<String> { "Winter", "Spring", "Summer" };

            var coldFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 18, 1);
            var warmFSet = new TriangleFuzzySet(10, 18, 25, 1);
            var hotFSet = new RightShoulderFuzzySet(18, 25, double.PositiveInfinity, 1);
            var temperatureSets = new List<FuzzySet> { coldFSet.ToFuzzySet(), warmFSet.ToFuzzySet(), hotFSet.ToFuzzySet() };

            var noWindFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 10, 1);
            var mildWindFSet = new TriangleFuzzySet(5, 10, 15, 1);
            var strongWindFSet = new RightShoulderFuzzySet(10, 15, double.PositiveInfinity, 1);
            var windSets = new List<FuzzySet> { noWindFSet.ToFuzzySet(), mildWindFSet.ToFuzzySet(), strongWindFSet.ToFuzzySet() };

            var dryFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 10, 1);
            var moistFSet = new TriangleFuzzySet(5, 10, 15, 1);
            var rainFSet = new RightShoulderFuzzySet(10, 15, double.PositiveInfinity, 1);
            var moistureSets = new List<FuzzySet> { dryFSet.ToFuzzySet(), moistFSet.ToFuzzySet(), rainFSet.ToFuzzySet() };

            var winterFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 50, 1);
            var springFSet = new TriangleFuzzySet(0, 50, 100, 1);
            var summerFSet = new RightShoulderFuzzySet(50, 100, double.PositiveInfinity, 1);
            var clothesSets = new List<FuzzySet> { winterFSet.ToFuzzySet(), springFSet.ToFuzzySet(), summerFSet.ToFuzzySet() };

            Temperature = new Attribute(temperatureList, temperatureSets);
            WindSpeed = new Attribute(windSpeedList, windSets);
            Moisture = new Attribute(moistureList, moistureSets);
            Clothes = new Attribute(clothesList, clothesSets);
        }
    }
}
