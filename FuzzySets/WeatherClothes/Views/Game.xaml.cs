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
        Attribute Moisture;
        Attribute WindSpeed;
        Attribute Clothes;
        int[, ,] Rules;

        #region DependecyProperties
        public double TemperatureDP
        {
            get { return (double)GetValue(TemperatureDPProperty); }
            set { SetValue(TemperatureDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemperatureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureDPProperty =
            DependencyProperty.Register("TemperatureDP", typeof(double), typeof(Game));

        public double MoistureDP
        {
            get { return (double)GetValue(MoistureDPProperty); }
            set { SetValue(MoistureDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MoistureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoistureDPProperty =
            DependencyProperty.Register("MoistureDP", typeof(double), typeof(Game));

        public double WindSpeedDP
        {
            get { return (double)GetValue(WindSpeedDPProperty); }
            set { SetValue(WindSpeedDPProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindSpeedDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindSpeedDPProperty =
            DependencyProperty.Register("WindSpeedDP", typeof(double), typeof(Game));

        #endregion

        public Game()
        {
            InitializeComponent();
            TextBlock1.Text = "Temperatura\notoczenia";
            TextBlock2.Text = "Prędkość\nwiatru";
            TextBlock3.Text = "Wilgotność\npowietrza";
            InitializeAttributes();
        }

        private void InitializeAttributes()
        {
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

            Rules = new int[,,]
            {
                {{1,2,2},{0,1,2},{0,1,1}},
                {{1,2,2},{0,0,1},{0,0,1}},
                {{0,0,1},{0,1,1},{0,1,2}}
            };
        }

        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            var temperatureArr = Temperature[TemperatureDP];
            var moistureArr = Moisture[MoistureDP];
            var windSpeedArr = WindSpeed[WindSpeedDP];

            var input = new double[][] { temperatureArr, moistureArr, windSpeedArr };

            //TODO: wykonać następne kroki 4 razy, po jednym na każdą normę
            var RuleValueSets = CalculateRuleValues(input);
            /*
             * TODO: Wygeneruj zbiór wyjściowy:
             * 1. Oblicz przecięcie każdego zbioru w RuleValueSets w obrębie tej samej wartości w tablicy Rules
             * 2. Oblicz przecięcie powstałych trzech zbiorów z odpowiednimi zbiorami w obrębie atrybutu Clothes
             * 3. Zsumuj otrzymane trzy zbiory w jeden zbiór ClothesResult
             * 4. Zastosuj metodę centroidu do znalezienia pojedynczej wartości ClothesResult
             * 5. Zinterpretuj wynik (proponowana metoda: znajdź zbiór dla którego FLV(znaleziona wartość) przyjmuje max)
             * 6. Wyświetla wynik
             */
        }

        private FuzzySet[,,] CalculateRuleValues(double[][] input, Norm norm=Norm.Zadeh)
        {
            var result = new FuzzySet[3, 3, 3];
            for(int tInd=0; tInd<3; tInd++)
                for(int mInd=0; mInd<3; mInd++)
                    for (int wInd = 0; wInd < 3; wInd++)
                    {
                        result[wInd, mInd, tInd] = new FuzzySet(Norms.ApplyTNorm(Norms.ApplyTNorm(Temperature.GetAttributeValue(tInd, input[0][tInd]), Moisture.GetAttributeValue(mInd, input[1][mInd])), WindSpeed.GetAttributeValue(wInd, input[2][wInd]), norm));
                    }
            return result;
        }
    }
}
