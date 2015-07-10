using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using FuzzySets;
using MathNet.Numerics.Integration;

namespace WeatherClothes.Views
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : INotifyPropertyChanged
    {
        Attribute _temperature;
        Attribute _humidity;
        Attribute _windSpeed;
        Attribute _clothes;
        int[, ,] _rules;

        #region DependecyProperties
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public double TemperatureDp
        {
            get
            {
                return (double)GetValue(TemperatureDpProperty);
            }
            set
            {
                SetValue(TemperatureDpProperty, value); OnPropertyChanged("TemperatureDp");
            }
        }

        // Using a DependencyProperty as the backing store for TemperatureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureDpProperty =
            DependencyProperty.Register("TemperatureDp", typeof(double), typeof(Game));

        public double MoistureDp
        {
            get { return (double)GetValue(MoistureDpProperty); }
            set { SetValue(MoistureDpProperty, value); OnPropertyChanged("MoistureDp"); }
        }

        // Using a DependencyProperty as the backing store for MoistureDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MoistureDpProperty =
            DependencyProperty.Register("MoistureDp", typeof(double), typeof(Game));

        public double WindSpeedDp
        {
            get { return (double)GetValue(WindSpeedDpProperty); }
            set { SetValue(WindSpeedDpProperty, value); OnPropertyChanged("WindSpeedDp"); }
        }

        // Using a DependencyProperty as the backing store for WindSpeedDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindSpeedDpProperty =
            DependencyProperty.Register("WindSpeedDp", typeof(double), typeof(Game));

        #endregion

        public Game()
        {
            TemperatureDp = 0;
            WindSpeedDp = 0;
            MoistureDp = 0;
            InitializeComponent();
            TextBlock1.Text = "Outside\nTemperature";
            TextBlock2.Text = "Wind\nSpeed";
            TextBlock3.Text = "Air\nHumidity";

            DataContext = this;

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

            _temperature = new Attribute(temperatureList, temperatureSets);
            _windSpeed = new Attribute(windSpeedList, windSets);
            _humidity = new Attribute(moistureList, moistureSets);
            _clothes = new Attribute(clothesList, clothesSets);


            _rules = new[,,]
            {
                {{2,0,0},{2,1,0},{2,1,1}},
                {{2,1,0},{2,1,0},{2,2,1}},
                {{2,1,0},{2,2,1},{2,2,1}}
            };

         }

        /// <summary>
        /// Performs an analysis.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            var temperatureArr = _temperature[TemperatureDp];
            var moistureArr = _humidity[MoistureDp];
            var windSpeedArr = _windSpeed[WindSpeedDp];

            var input = new[] { temperatureArr, moistureArr, windSpeedArr };
            double[] values = new double[4];
            SingleDimFuzzySet[] sets = new SingleDimFuzzySet[4];
            var tata = GetOpinion(input, out values[0], out sets[0]);
            var mama = GetOpinion(input, out values[1], out sets[1], Norm.Algebraic);
            var babcia = GetOpinion(input, out values[2], out sets[2], Norm.Lukasiewicz);
            var dziadek = GetOpinion(input, out values[3], out sets[3], Norm.Einstein);

            Result r = new Result(_temperature, _windSpeed, _humidity, _clothes, new[] { TemperatureDp, MoistureDp, WindSpeedDp, values[0], values[1], values[2], values[3] }, new[] { tata, mama, babcia, dziadek }, sets);
            Visibility = Visibility.Collapsed;
            r.ParentView = this;
            r.Show();

        }
        public Window ParentView { get; set; }

        /// <summary>
        /// Calculates crisp values of an analysis and interprets it.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="crispValue"></param>
        /// <param name="clothesResult"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        private String GetOpinion(double[][] input, out double crispValue, out SingleDimFuzzySet clothesResult, Norm norm = Norm.Zadeh)
        {
            var ruleValueSets = CalculateRuleValues(input, norm);
            var clothesResults = CalculateResultFuzzySets(ruleValueSets, norm);
            var integratedClothesResult = clothesResults[0].UnionWith(clothesResults[1].UnionWith(clothesResults[2], norm), norm);
            var r1 = SimpsonRule.IntegrateComposite(x => x * integratedClothesResult[x], 0, 1, 42);
            var r2 = SimpsonRule.IntegrateComposite(x => integratedClothesResult[x], 0, 1, 42);
            crispValue = r2.CompareTo(0)==0 ? 0.5 : r1 / r2; //Środek ciężkości wynikowego zbioru
            clothesResult = integratedClothesResult;
            return _clothes.GetMaxLabel(crispValue, new[] { clothesResults[0][crispValue], clothesResults[1][crispValue], clothesResults[2][crispValue] }, norm);
        }

        /// <summary>
        /// Calculate output fuzzy set.
        /// </summary>
        /// <param name="ruleValueSets"></param>
        /// <param name="norm"></param>
        /// <returns></returns>
        private FuzzySet[] CalculateResultFuzzySets(FuzzySet[, ,] ruleValueSets, Norm norm = Norm.Zadeh)
        {
            var rfs = new[] { new FuzzySet(_clothes.GetFuzzySet(0)), new FuzzySet(_clothes.GetFuzzySet(1)), new FuzzySet(_clothes.GetFuzzySet(2)) };
            bool[] modified = { false, false, false };
            for (int a = 0; a < 3; a++)
                for (int b = 0; b < 3; b++)
                    for (int c = 0; c < 3; c++)
                    {
                        if (ruleValueSets[a, b, c] == null) continue;
                        var ind = _rules[a, b, c]; //indeks ClothesSetu będącego po prawej stronie implikacji w Rules
                        rfs[ind] = rfs[ind].IntersectWith(ruleValueSets[a, b, c], norm).ToFuzzySet();
                        modified[ind] = true;
                    }
            for (int i = 0; i < 3; i++)
            {
                if (!modified[i])
                {
                    rfs[i] = rfs[i].IntersectWith(new FuzzySet(), norm).ToFuzzySet();
                }
            }
            return rfs;
        }

        /// <summary>
        /// Calculating membership to "Clothes" fuzzy sets based on Rules array.
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
                 if (tVal.CompareTo(0)==0) continue;
                 for (int mInd = 0; mInd < 3; mInd++)
                 {
                     var mVal = input[1][mInd];
                     if (mVal.CompareTo(0) == 0) continue;
                     for (int wInd = 0; wInd < 3; wInd++)
                     {
                         //var tVal = Temperature.GetAttributeValue(tInd, input[0][tInd]);
                         //var mVal = Moisture.GetAttributeValue(mInd, input[1][mInd]);
                         //var wVal = WindSpeed.GetAttributeValue(wInd, input[2][wInd]);


                         var wVal = input[2][wInd];
                         if (wVal.CompareTo(0) == 0) continue;
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
            if (ParentView != null) ParentView.Visibility = Visibility.Visible;
        }
    }
}
