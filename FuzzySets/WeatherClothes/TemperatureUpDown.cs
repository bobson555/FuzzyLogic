using System;
using Xceed.Wpf.Toolkit;

namespace WeatherClothes
{
    public class TemperatureUpDown : IntegerUpDown
    {
        protected override string ConvertValueToText()
        {
           var s = base.ConvertValueToText();
           s += "°C";
           return s;
        }
       
    }
    public class SpeedUpDown : IntegerUpDown
    {
        public String Unit { get; set; }
        public SpeedUpDown()
        {
            Unit = "m/s";
        }
        protected override string ConvertValueToText()
        {
            var s = base.ConvertValueToText();
            s += Unit;
            return s;
        }
    }
}
