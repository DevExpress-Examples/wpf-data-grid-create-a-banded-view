using System;
using System.Windows;
using System.Windows.Data;
namespace BandedViewExtension {
    public class GridLengthValueConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            double res = (int)value * (double)parameter;
            return new GridLength(res);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}