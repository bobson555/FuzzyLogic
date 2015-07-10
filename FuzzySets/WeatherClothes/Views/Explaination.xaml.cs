using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using FuzzySets;
using OxyPlot;
using OxyPlot.Series;

namespace WeatherClothes.Views
{
    public class Plot
    {
        public PlotModel Model { get; set; }
        public string Title { get; set; }
        public Plot(string title, Attribute a, double x0, double x1, double[] crispValues, OxyColor[] colors, string[] labels = null)
        {
            Model = new PlotModel {Title = title};
            for (int i = 0; i < crispValues.Length; i++)
            {
                var d = new LineSeries();
               // D.Title = string.Format("Input{0}", (labels == null || labels.Length <= i) ? string.Empty : " " + labels[i]);
                d.Points.AddRange(new[] { new DataPoint(crispValues[i],0), new DataPoint(crispValues[i], 1)});


                d.Color = colors[3 + i];
                d.StrokeThickness = 2.05;
                Model.Series.Add(d);
            }
            for (int i = 0; i < crispValues.Length; i++)
            {
                var d = new LineSeries
                {
                    Title =
                        string.Format("Input{0}",
                            (labels == null || labels.Length <= i) ? string.Empty : " " + labels[i]),
                    Color = colors[3 + i],
                    StrokeThickness = 2.05
                };
                d.Points.AddRange(new[] { new DataPoint(crispValues[i], i / (double)crispValues.Length), new DataPoint(crispValues[i], (i + 1) / (double)crispValues.Length) });
                Model.Series.Add(d);
            }
            
            for (int i = 0; i < 3; i++)
            {
                var i1 = i;
                var f = new FunctionSeries(x => (a.GetFuzzySet(i1).Flv(x)), x0, x1, Math.Abs(x1 - x0)/1000,
                    a.GetLabel(i))
                {
                    Color = colors[i],
                    StrokeThickness = 0.75
                };
                Model.Series.Add(f);
            }

        }
        public Plot(string title, Attribute a, double x0, double x1, double crispValue, OxyColor[] colors, string[] labels = null)
        {
            Model = new PlotModel {Title = title};
            var d = new LineSeries
            {
                Title =
                    string.Format("Input{0}", (labels == null || labels.Length <= 0) ? string.Empty : " " + labels[0]),
                Color = colors[3],
                StrokeThickness = 2.05
            };
            d.Points.AddRange(new[] { new DataPoint(crispValue, 0), new DataPoint(crispValue, 1) });
            Model.Series.Add(d);

            for (int i = 0; i < 3; i++)
            {
                var i1 = i;
                var f = new FunctionSeries(x => (a.GetFuzzySet(i1).Flv(x)), x0, x1, Math.Abs(x1 - x0)/1000,
                    a.GetLabel(i))
                {
                    Color = colors[i],
                    StrokeThickness = 0.75
                };
                Model.Series.Add(f);
            }

        }
        public Plot(string title, SingleDimFuzzySet a, double x0, double x1, double crispValue, OxyColor[] colors)
        {
            Model = new PlotModel {Title = title};

            var d = new LineSeries
            {
                Title = "Crisp Value",
                Color = colors[3],
                StrokeThickness = 2.05
            };


            d.Points.AddRange(new[] { new DataPoint(crispValue, 0), new DataPoint(crispValue, 1) });
            Model.Series.Add(d);


            var f = new FunctionSeries(x => (a.Flv(x)), x0, x1, Math.Abs(x1 - x0)/100000, "FLV")
            {
                Color = colors[2],
                StrokeThickness = 0.75
            };
            Model.Series.Add(f);
            

        }
    }
    /// <summary>
    /// Interaction logic for Explaination.xaml
    /// </summary>
    public partial class Explaination
    {
        public Explaination(Attribute T, Attribute w, Attribute h, Attribute r, double[] values,SingleDimFuzzySet[] sets)
        {
            var colors = new[] { OxyColor.FromRgb(0, 0, 255), OxyColor.FromRgb(0, 255, 0), OxyColor.FromRgb(255, 0, 0), OxyColor.FromRgb(13, 34, 15) };
            Plot1 = new Plot("Temperature", T, Math.Min(-10,values[0]),Math.Max(values[0], 32), values[0], colors);
            Plot2 = new Plot("Wind Speed", w, 0, Math.Max(values[2],20), values[2], colors);
            Plot3 = new Plot("Humidity", h, 0, 1, values[1], colors);
            List<OxyColor> additionalColors = new List<OxyColor>();
            additionalColors.AddRange(colors);
            additionalColors.AddRange(new[] { OxyColor.FromRgb(255, 0, 255), OxyColor.FromRgb(0x99, 0x33, 0), OxyColor.FromRgb(0, 0x33, 0x69) });
            Plot4 = new Plot("Result", r, 0, 1, new[] { values[3], values[4], values[5], values[6] },  additionalColors.ToArray(), new[] { "Dad - Zadeh", "Mum - Algebraic", "Grandma - Lukasiewicz", "Grandad - Einstein" });
            PlotDad = new Plot("Dad- Zadeh Norm", sets[0], 0, 1, values[3], additionalColors.ToArray());
            PlotMum = new Plot("Mum- Algebraic Norm", sets[1], 0, 1, values[4], additionalColors.ToArray());
            PlotGMum = new Plot("Grandma- Lukasiewicz Norm", sets[2], 0, 1, values[5], additionalColors.ToArray());
            PlotGDad= new Plot("Grandad- Einstein Norm", sets[3], 0, 1, values[6], additionalColors.ToArray());
            DataContext = this;
            InitializeComponent();
          

        }
        public Window ParentView { get; set; }
        public Plot Plot1 { get; set; }
        public Plot Plot2 { get; set; }
        public Plot Plot3 { get; set; }
        public Plot Plot4 { get; set; }

        public Plot PlotDad { get; set; }
        public Plot PlotMum { get; set; }
        public Plot PlotGMum { get; set; }
        public Plot PlotGDad { get; set; }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ParentView != null)
            {
                ParentView.WindowState = WindowState.Maximized;
                ParentView.Visibility = Visibility.Visible;
                
              
            }

        }
    }
}
