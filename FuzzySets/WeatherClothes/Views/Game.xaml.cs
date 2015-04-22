﻿using FuzzySets;
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

        #region DependecyProperties
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public double TemperatureDP
        {
            get { 
                return (double)GetValue(TemperatureDPProperty); }
            set { 
                SetValue(TemperatureDPProperty, value); OnPropertyChanged("TemperatureDP"); }
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
            var clothesList = new List<String> { "Winter", "Spring", "Summer" };

            var coldFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 18, 1);
            var warmFSet = new TriangleFuzzySet(10, 18, 25, 1);
            var hotFSet = new RightShoulderFuzzySet(18, 25, double.PositiveInfinity, 1);
            var temperatureSets = new List<FuzzySet> { coldFSet.ToFuzzySet(), warmFSet.ToFuzzySet(), hotFSet.ToFuzzySet() };

            var noWindFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 10, 1);
            var mildWindFSet = new TriangleFuzzySet(5, 10, 15, 1);
            var strongWindFSet = new RightShoulderFuzzySet(10, 15, double.PositiveInfinity, 1);
            var windSets = new List<FuzzySet> { noWindFSet.ToFuzzySet(), mildWindFSet.ToFuzzySet(), strongWindFSet.ToFuzzySet() };

            var dryFSet = new LeftShoulderFuzzySet(double.NegativeInfinity, 0, 0.5, 1);
            var moistFSet = new TriangleFuzzySet(0.25, 0.5, 0.75, 1);
            var rainFSet = new RightShoulderFuzzySet(0.5, 0.75, double.PositiveInfinity, 1);
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
            var tata = GetOpinion(input, out Values[0],Norm.Zadeh);
            var mama = GetOpinion(input, out Values[1],Norm.Algebraic);
            var babcia = GetOpinion(input,out Values[2],Norm.Lukasiewicz);
            var dziadek = GetOpinion(input,out Values[3],Norm.Einstein);
            MessageBox.Show(String.Format("tata: {0} \n mama: {1} \n babcia: {2} \n dziadek: {3}",tata,mama,babcia,dziadek));
            Result R = new Result(Temperature, WindSpeed, Moisture, Clothes, new[] { TemperatureDP, MoistureDP, WindSpeedDP, Values[0], Values[1], Values[2], Values[3] });
            R.ShowDialog();
        }

        private String GetOpinion(double[][] input,out double crispValue, Norm norm = Norm.Zadeh)
        {
            var RuleValueSets = CalculateRuleValues(input, norm);
            var ClothesResults = CalculateResultFuzzySets(RuleValueSets, norm);
            var ClothesResult = ClothesResults[0].UnionWith(ClothesResults[1].UnionWith(ClothesResults[2], norm),norm);
            var r1 = MathNet.Numerics.Integration.NewtonCotesTrapeziumRule.IntegrateAdaptive(x => x * ClothesResult[x], 0, 100, 0.0001);
            var r2 = MathNet.Numerics.Integration.NewtonCotesTrapeziumRule.IntegrateAdaptive(x => ClothesResult[x], 0, 100, 0.0001);
            crispValue = r1 / r2; //Środek ciężkości wynikowego zbioru
            return Clothes.GetMaxLabel(crispValue, new double[]{ClothesResults[0][crispValue],ClothesResults[1][crispValue],ClothesResults[2][crispValue]}, norm);
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
            for (int a = 0; a < 3; a++)
                for (int b = 0; b < 3; b++)
                    for (int c = 0; c < 3; c++)
                    {
                        if (RuleValueSets[a, b, c] == null) continue;
                        var ind = Rules[a, b, c]; //indeks ClothesSetu będącego po prawej stronie implikacji w Rules
                        rfs[ind] = rfs[ind].IntersectWith(RuleValueSets[a, b, c], norm);
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
            for(int tInd=0; tInd<3; tInd++)
                for(int mInd=0; mInd<3; mInd++)
                    for (int wInd = 0; wInd < 3; wInd++)
                    {
                        var val = Norms.ApplyTNorm(Norms.ApplyTNorm(Temperature.GetAttributeValue(tInd, input[0][tInd]), Moisture.GetAttributeValue(mInd, input[1][mInd])), WindSpeed.GetAttributeValue(wInd, input[2][wInd]), norm);
                        if(val != 0)
                        result[wInd, mInd, tInd] = new FuzzySet(val);
                    }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
