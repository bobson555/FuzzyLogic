using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace WeatherClothes
{
    public class TemperatureUpDown : IntegerUpDown
    {
        protected override string ConvertValueToText()
        {
           var S = base.ConvertValueToText();
           S += "°C";
           return S;
        }
       
    }
    public class SpeedUpDown : IntegerUpDown
    {
        public String Unit { get; set; }
        public SpeedUpDown()
            : base()
        {
            Unit = "m/s";
        }
        protected override string ConvertValueToText()
        {
            var S = base.ConvertValueToText();
            S += Unit;
            return S;
        }
    }
}
