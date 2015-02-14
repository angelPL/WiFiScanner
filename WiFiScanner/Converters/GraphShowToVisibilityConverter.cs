using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WiFiScanner.Converters
{
  /// <summary>
  /// If prop. Show set to true - show bezier curve and info for network on graph, oderwise hide
  /// </summary>
  public class GraphShowToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType,
       object parameter, CultureInfo culture)
    {
      if ((bool)value == true)
        return "Visible";
      else
        return "Hidden";
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
      // Do the conversion from visibility to bool
      return null;
    }
  }
}
