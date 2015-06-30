using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WiFiScanner.Converters
{
  public class BoolToOpacityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType,
       object parameter, CultureInfo culture)
    {
      // Do the conversion from bool to visibility
        return (bool)value ? 1 : 0;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
      // Do the conversion from visibility to bool
      return null;
    }
  }
}
