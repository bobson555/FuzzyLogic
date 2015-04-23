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
    public class Plot
    {
        public OxyPlot.PlotModel Model { get; set; }
        public string Title { get; set; }
        public Plot(string Title, Attribute A, double x0, double x1, double[] CrispValues, OxyPlot.OxyColor[] colors, string[] labels = null)
        {
            Model = new OxyPlot.PlotModel();
            Model.Title = Title;
            for (int i = 0; i < CrispValues.Length; i++)
            {
                var D = new OxyPlot.Series.LineSeries();
               // D.Title = string.Format("Input{0}", (labels == null || labels.Length <= i) ? string.Empty : " " + labels[i]);
                D.Points.AddRange(new[] { new OxyPlot.DataPoint(CrispValues[i],0), new OxyPlot.DataPoint(CrispValues[i], 1)});


                D.Color = colors[3 + i];
                D.StrokeThickness = 2.05;
                Model.Series.Add(D);
            }
            for (int i = 0; i < CrispValues.Length; i++)
            {
                var D = new OxyPlot.Series.LineSeries();
                D.Title = string.Format("Input{0}", (labels == null || labels.Length <= i) ? string.Empty : " " + labels[i]);
                D.Points.AddRange(new[] { new OxyPlot.DataPoint(CrispValues[i], (double)i/(double)CrispValues.Length), new OxyPlot.DataPoint(CrispValues[i], (double)(i+1)/(double)CrispValues.Length) });


                D.Color = colors[3 + i];
                D.StrokeThickness = 2.05;
                Model.Series.Add(D);
            }
            
            for (int i = 0; i < 3; i++)
            {
                var F = new OxyPlot.Series.FunctionSeries(x => (A.GetFuzzySet(i).FLV(x)), x0, x1, Math.Abs(x1 - x0) / 1000, A.GetLabel(i));
                F.Color = colors[i];
                F.StrokeThickness = 0.75;
                Model.Series.Add(F);
            }

        }
        public Plot(string Title, Attribute A, double x0, double x1, double CrispValue, OxyPlot.OxyColor[] colors, string[] labels = null)
        {
            Model = new OxyPlot.PlotModel();
            Model.Title = Title;
            var D = new OxyPlot.Series.LineSeries();
            D.Title = string.Format("Input{0}", (labels == null || labels.Length <= 0) ? string.Empty : " " + labels[0]);
            D.Points.AddRange(new[] { new OxyPlot.DataPoint(CrispValue, 0), new OxyPlot.DataPoint(CrispValue, 1) });

            D.Color = colors[3];
            D.StrokeThickness = 2.05;
            Model.Series.Add(D);

            for (int i = 0; i < 3; i++)
            {
                var F = new OxyPlot.Series.FunctionSeries(x => (A.GetFuzzySet(i).FLV(x)), x0, x1, Math.Abs(x1 - x0) / 1000, A.GetLabel(i));
                F.Color = colors[i];
                F.StrokeThickness = 0.75;
                Model.Series.Add(F);
            }

        }
        public Plot(string Title, SingleDimFuzzySet A, double x0, double x1, double CrispValue, OxyPlot.OxyColor[] colors)
        {
            Model = new OxyPlot.PlotModel();

            Model.Title = Title;
            var D = new OxyPlot.Series.LineSeries();
            D.Title = "Crisp Value";
            D.Points.AddRange(new[] { new OxyPlot.DataPoint(CrispValue, 0), new OxyPlot.DataPoint(CrispValue, 1) });

            D.Color = colors[3];
            D.StrokeThickness = 2.05;
            Model.Series.Add(D);

           
                var F = new OxyPlot.Series.FunctionSeries(x => (A.FLV(x)), x0, x1, Math.Abs(x1 - x0) / 1000,"FLV");
                F.Color = colors[2];
                F.StrokeThickness = 0.75;
                Model.Series.Add(F);
            

        }
    }
    /// <summary>
    /// Interaction logic for Explaination.xaml
    /// </summary>
    public partial class Explaination : Window
    {
        public Explaination(Attribute T, Attribute W, Attribute H, Attribute R, double[] Values,SingleDimFuzzySet[] Sets)
        {
            var Colors = new OxyPlot.OxyColor[] { OxyPlot.OxyColor.FromRgb(0, 0, 255), OxyPlot.OxyColor.FromRgb(0, 255, 0), OxyPlot.OxyColor.FromRgb(255, 0, 0), OxyPlot.OxyColor.FromRgb(13, 34, 15) };
            Plot1 = new Plot("Temperatura", T, -10, 32, Values[0], Colors);
            Plot2 = new Plot("Prędkość Wiatru", W, 0, 20, Values[2], Colors);
            Plot3 = new Plot("Wilgotność Powietrza", H, 0, 1, Values[1], Colors);
            List<OxyPlot.OxyColor> AdditionalColors = new List<OxyPlot.OxyColor>();
            AdditionalColors.AddRange(Colors);
            AdditionalColors.AddRange(new OxyPlot.OxyColor[] { OxyPlot.OxyColor.FromRgb(255, 0, 255), OxyPlot.OxyColor.FromRgb(0x99, 0x33, 0), OxyPlot.OxyColor.FromRgb(0, 0x33, 0x69) });
            Plot4 = new Plot("Wynik", R, 0, 1, new double[] { Values[3], Values[4], Values[5], Values[6] },  AdditionalColors.ToArray(), new[] { "Tata - Zadeh", "Mama - Algebraic", "Babcia - Lukasiewicz", "Dziadek - Einstein" });
            PlotDad = new Plot("Tata-Zadeh", Sets[0], 0, 1, Values[3], AdditionalColors.ToArray());
            PlotMum = new Plot("Mama-Algebraiczna", Sets[1], 0, 1, Values[4], AdditionalColors.ToArray());
            PlotGMum = new Plot("Babcia-Lukasiewicz", Sets[2], 0, 1, Values[5], AdditionalColors.ToArray());
            PlotGDad= new Plot("Dziadek-Einstein", Sets[3], 0, 1, Values[6], AdditionalColors.ToArray());
            DataContext = this;
            InitializeComponent();
            /*
             * 2 karty - 1. karta wykresy z dok 2. karta do wyboru zakładka z normą i tam wykres wynikowy wyliczony każdą normą
             * 
             */

        }
        public Plot Plot1 { get; set; }
        public Plot Plot2 { get; set; }
        public Plot Plot3 { get; set; }
        public Plot Plot4 { get; set; }

        public Plot PlotDad { get; set; }
        public Plot PlotMum { get; set; }
        public Plot PlotGMum { get; set; }
        public Plot PlotGDad { get; set; }
    }
}
