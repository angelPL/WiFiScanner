using System;
using System.Globalization;
using System.Windows.Data;

namespace WiFiScanner.Converters
{
  /// <summary>
  /// Sets TextBlock that indicates NetworkName on canvas Top property based on signal strength and canvas height
  /// </summary>
  public class NetworkNameCanvasTopConverter : IValueConverter
  {
    /// <summary>
    /// Converts signal strength and canvas height to Canvas.Top property (double)
    /// </summary>
    /// <param name="value">NetworkNameChannelSignalColorCanvasWidthAndHeight passed, used information about SignalStrength and CanvasHeight</param>
    /// <param name="targetType">double (Canvas.Top property)</param>
    /// <param name="parameter">No parameters</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType,
       object parameter, CultureInfo culture)
    {
      // Passing value = NetworkNameChannelSignalColorCanvasWidthAndHeight
      var signal = ((Tuple<string, int, int, string, double, double>)value).Item3; // Signal strength
      var canvasHeight = ((Tuple<string, int, int, string, double, double>)value).Item6; // Canvas actual height

      var signalFull = Int32.Parse(Math.Round(canvasHeight / 10.93333333333333f).ToString()); // set value for 100% signal
      var signalStrength = signalFull + Int32.Parse(Math.Round(canvasHeight / 12.14814814814815f).ToString()) * (100.0f - (int)signal) / 10.0f; // adjust to signal strength
      var signalOnCanvas = Int32.Parse(Math.Round(signalStrength).ToString());

      return signalOnCanvas;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {      
      return null;
    }
  }
}
