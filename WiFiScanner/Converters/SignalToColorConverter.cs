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
  public class SignalToColorConverter : IValueConverter
  {
    

    public object Convert(object value, Type targetType,
       object parameter, CultureInfo culture)
    {
      int signalInt = Int32.Parse(value.ToString());
      int signalColorRatio = (signalInt) * 255/100 * 2;
      int signalColorRatioReverse = (100 - signalInt) * 255 / 100 * 2;
      if (signalColorRatio > 255)
        signalColorRatio = 255;
      if (signalColorRatioReverse > 255)
        signalColorRatioReverse = 255;
      string signalColorRatioHex = signalColorRatio.ToString("X2");
      string signalColorRatioHexReverse = signalColorRatioReverse.ToString("X2");

      return "#" + signalColorRatioHexReverse + signalColorRatioHex + "00";
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
      // Do the conversion from visibility to bool
      return null;
    }
  }
}
