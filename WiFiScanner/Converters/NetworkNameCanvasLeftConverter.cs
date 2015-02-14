using System;
using System.Globalization;
using System.Windows.Data;

namespace WiFiScanner.Converters
{
  /// <summary>
  /// Sets TextBlock that indicates NetworkName on canvas Left property based on signal strength and canvas width
  /// </summary>
  public class NetworkNameCanvasLeftConverter : IValueConverter
  {
    /// <summary>
    /// Converts channel no. and canvas width to Canvas.Left property (double)
    /// </summary>
    /// <param name="value">NetworkNameChannelSignalColorCanvasWidthAndHeight passed, used information about SignalStrength and CanvasHeight</param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType,
       object parameter, CultureInfo culture)
    {
      // Passing value = NetworkNameChannelSignalColorCanvasWidthAndHeight
      var channel = ((Tuple<string, int, int, string, double, double>)value).Item2;
      var canvasWidth = ((Tuple<string, int, int, string, double, double>)value).Item5;
      
      int channelOne = Int32.Parse(Math.Round(canvasWidth / 43.91111111111111f).ToString()); // 20 - for initial size of canvas
      int channelWidth = Int32.Parse(Math.Round(canvasWidth / 15.40740740740741f).ToString()); // 57 - for initial size of canvas
      int channelValue = channelOne + ((int)channel - 1) * Int32.Parse(Math.Round(canvasWidth / 15.14176245210728f).ToString());

      return channelValue;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {      
      return null;
    }
  }
}
