﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace WeatherClothes
{
    public class ImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            d *= 0.75;
            d /= 689;
            d *= 585;
            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            d /= 0.75;
            d *= 689;
            d /= 585;
            return d;
        }
    }
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
           
            d *= 451;
            d /= 105;
            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
         
            var d = (double)value;
            d *= 0.75;
            d /= 689;
            d *= 585;
            return d;
        }
    }
    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            d *= 0.75;
            return d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            d /= 0.75;
            return d;
        }
    }
}
