using FuzzySets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Game : Window, INotifyPropertyChanged
    {
        Attribute Temperature;
        Attribute Moisture;
        Attribute WindSpeed;
        Attribute Clothes;
        int[, ,] Rules;
        int[, ,] RulesR;
        #region DependecyProperties
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public double TemperatureDP
        {
            get
            {
                return (double)GetValue(TemperatureDPProperty);
            }
            set
            {
                SetValue(TemperatureDPProperty, value); OnPropertyChanged("TemperatureDP");
            }
        }

        // Using a DependencyProperty as the backing store for TemperatureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureDPProperty =
            DependencyProperty.Register("TemperatureDP", typeof(double), typeof(Game));

        public double MoistureDP
        {
            get { return (double)GetValue(MoistureDPProperty); }
            set { SetValue(MoistureDPProperty, value); OnPropertyChanged("MoistureDP"); }
        }

        // Using a DependencyProperty as the backing store for MoistureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoistureDPProperty =
            DependencyProperty.Register("MoistureDP", typeof(double), typeof(Game));

        public double WindSpeedDP
        {
            get { return (double)GetValue(WindSpeedDPProperty); }
            set { SetValue(WindSpeedDPProperty, value); OnPropertyChanged("WindSpeedDP"); }
        }

        // Using a DependencyProperty as the backing store for WindSpeedDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindSpeedDPProperty =
            DependencyProperty.Register("WindSpeedDP", typeof(double), typeof(Game));

        #endregion

        public Game()
        {
            TemperatureDP = 0;
            WindSpeedDP = 0;
            MoistureDP = 0;
            InitializeComponent();
            TextBlock1.Text = "Temperatura\notoczenia";
            TextBlock2.Text = "Prędkość\nwiatru";
            TextBlock3.Text = "Wilgotność\npowietrza";

            this.DataContext = this;

            InitializeAttributes();
        }

        /// <summary>
        /// Inicjalizacja zmiennych prywatnych
        /// </summary>
        private void InitializeAttributes()
        {
            var temperatureList = new List<String> { "Cold", "Warm", "Hot" };
            var windSpeedList = new List<String> { "None", "Mild", "Strong" };
            var moistureList = new List<String> { "Dry", "Wet", "Rain" };
            var clothesList = new List<String> { "Summer", "Spring", "Winter" };

            var coldFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 10, 1);
            var warmFSet = new TriangleFuzzySet(0, 10, 20, 1);
            var hotFSet = new RightShoulderFuzzySet(10, 20, double.PositiveInfinity, 1);
            var temperatureSets = new List<FuzzySet> { coldFSet.ToFuzzySet(), warmFSet.ToFuzzySet(), hotFSet.ToFuzzySet() };

            var noWindFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 10, 1);
            var mildWindFSet = new TriangleFuzzySet(0, 10, 15, 1);
            var strongWindFSet = new RightShoulderFuzzySet(10, 15, double.PositiveInfinity, 1);
            var windSets = new List<FuzzySet> { noWindFSet.ToFuzzySet(), mildWindFSet.ToFuzzySet(), strongWindFSet.ToFuzzySet() };

            var dryFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0.1, 0.4, 1);
            var moistFSet = new TriangleFuzzySet(0.1, 0.4, 0.75, 1);
            var rainFSet = new RightShoulderFuzzySet(0.4, 0.75, double.PositiveInfinity, 1);
            var moistureSets = new List<FuzzySet> { dryFSet.ToFuzzySet(), moistFSet.ToFuzzySet(), rainFSet.ToFuzzySet() };

            var summerFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 1F / 5F, 0.4, 1);
            var springFSet = new TriangleFuzzySet(1F / 5F, 0.4, 3F / 5F, 1);
            var winterFSet = new RightShoulderFuzzySet(0.4, 3F / 5F, double.PositiveInfinity, 1);
            var clothesSets = new List<FuzzySet> { summerFSet.ToFuzzySet(), springFSet.ToFuzzySet(), winterFSet.ToFuzzySet() };

            Temperature = new Attribute(temperatureList, temperatureSets);
            WindSpeed = new Attribute(windSpeedList, windSets);
            Moisture = new Attribute(moistureList, moistureSets);
            Clothes = new Attribute(clothesList, clothesSets);


            Rules = new int[,,]
            {
                {{2,0,0},{2,1,0},{2,1,1}},
                {{2,1,0},{2,1,0},{2,2,1}},
                {{2,1,0},{2,2,1},{2,2,1}}
            };

         }

        /// <summary>
        /// Przeprowadzenie Analizy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            var temperatureArr = Temperature[TemperatureDP];
            var moistureArr = Moisture[MoistureDP];
            var windSpeedArr = WindSpeed[WindSpeedDP];

            var input = new double[][] { temperatureArr, moistureArr, windSpeedArr };
            double[] Values = new double[4];
            SingleDimFuzzySet[] Sets = new SingleDimFuzzySet[4];
            var tata = GetOpinion(input, out Values[0], out Sets[0], Norm.Zadeh);
            var mama = GetOpinion(input, out Values[1], out Sets[1], Norm.Algebraic);
            var babcia = GetOpinion(input, out Values[2], out Sets[2], Norm.Lukasiewicz);
            var dziadek = GetOpinion(input, out Values[3], out Sets[3], Norm.Einstein);

            Result R = new Result(Temperature, WindSpeed, Moisture, Clothes, new[] { TemperatureDP, MoistureDP, WindSpeedDP, Values[0], Values[1], Values[2], Values[3] }, new[] { tata, mama, babcia, dziadek }, Sets);
            this.Visibility = System.Windows.Visibility.Collapsed;
            R.ParentView = this;
            R.Show();

        }
        public Window ParentView { get; set; }

        private String GetOpinion(double[][] input, out double crispValue, out SingleDimFuzzySet clothesResult, Norm norm = Norm.Zadeh)
        {
            var RuleValueSets = CalculateRuleValues(input, norm);
            var ClothesResults = CalculateResultFuzzySets(RuleValueSets, norm);
            var ClothesResult = ClothesResults[0].UnionWith(ClothesResults[1].UnionWith(ClothesResults[2], norm), norm);
            var r1 = MathNet.Numerics.Integration.NewtonCotesTrapeziumRule.IntegrateAdaptive(x => x * ClothesResult[x], 0, 1, 0.000001);
            var r2 = MathNet.Numerics.Integration.NewtonCotesTrapeziumRule.IntegrateAdaptive(x => ClothesResult[x], 0, 1, 0.000001);
            var r3 = MathNet.Numerics.Integration.SimpsonRule.IntegrateComposite(x => x * ClothesResult[x], 0, 1, 10);
            var r4 = MathNet.Numerics.Integration.SimpsonRule.IntegrateComposite(x => ClothesResult[x], 0, 1, 10);
                crispValue = (r1+r3) / (r2+r4); //Środek ciężkości wynikowego zbioru
            clothesResult = ClothesResult;
            return Clothes.GetMaxLabel(crispValue, new double[] { ClothesResults[0][crispValue], ClothesResults[1][crispValue], ClothesResults[2][crispValue] }, norm);
        }

        /// <summary>
        /// Obliczenie wynikowego zbioru rozmytego
        /// </summary>
        /// <param name="RuleValueSets"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        private FuzzySet[] CalculateResultFuzzySets(FuzzySet[, ,] RuleValueSets, Norm norm = Norm.Zadeh)
        {
            var rfs = new FuzzySet[] { new FuzzySet(Clothes.GetFuzzySet(0)), new FuzzySet(Clothes.GetFuzzySet(1)), new FuzzySet(Clothes.GetFuzzySet(2)) };
            bool[] Modified = new[] { false, false, false };
            for (int a = 0; a < 3; a++)
                for (int b = 0; b < 3; b++)
                    for (int c = 0; c < 3; c++)
                    {
                        if (RuleValueSets[a, b, c] == null) continue;
                        var ind = Rules[a, b, c]; //indeks ClothesSetu będącego po prawej stronie implikacji w Rules
                        rfs[ind] = rfs[ind].IntersectWith(RuleValueSets[a, b, c], norm);
                        Modified[ind] = true;
                    }
            for (int i = 0; i < 3; i++)
            {
                if (!Modified[i])
                {
                    rfs[i] = rfs[i].IntersectWith(new FuzzySet(0), norm);
                }
            }
            return rfs;
        }

        /// <summary>
        /// Obliczneie przynależności do zbiorów Clothes na podstawie Rules
        /// </summary>
        /// <param name="input"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        private FuzzySet[, ,] CalculateRuleValues(double[][] input, Norm norm = Norm.Zadeh)
        {
            var result = new FuzzySet[3, 3, 3];
            for (int tInd = 0; tInd < 3; tInd++)
            {
                 var tVal = input[0][tInd];
                 if (tVal == 0) continue;
                 for (int mInd = 0; mInd < 3; mInd++)
                 {
                     var mVal = input[1][mInd];
                     if (mVal == 0) continue;
                     for (int wInd = 0; wInd < 3; wInd++)
                     {
                         //var tVal = Temperature.GetAttributeValue(tInd, input[0][tInd]);
                         //var mVal = Moisture.GetAttributeValue(mInd, input[1][mInd]);
                         //var wVal = WindSpeed.GetAttributeValue(wInd, input[2][wInd]);


                         var wVal = input[2][wInd];
                         if (tVal == 0 || mVal == 0 || wVal == 0) continue;
                         //var t = tVal == 0 ? 1 : tVal;
                         //var m = mVal == 0 ? 1 : mVal;
                         //var w = wVal == 0 ? 1 : wVal;
                         var val = Norms.ApplyTNorm(Norms.ApplyTNorm(tVal, mVal, norm), wVal, norm);
                         result[wInd, mInd, tInd] = new FuzzySet(val);
                     }
                 }
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            if (ParentView != null) ParentView.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
