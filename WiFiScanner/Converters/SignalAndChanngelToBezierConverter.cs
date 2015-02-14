using System;
using System.Globalization;
using System.Windows.Data;

namespace WiFiScanner.Converters
{
  public class SignalAndChanngelToBezierConverter : IValueConverter
  {
    public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
    {      
      var channel = ((Tuple<string, int, int, string, double, double>)values).Item2;
      var signal = ((Tuple<string, int, int, string, double, double>)values).Item3;
      var canvasWidth = ((Tuple<string, int, int, string, double, double>)values).Item5;
      var canvasHeight = ((Tuple<string, int, int, string, double, double>)values).Item6;
                       
      // Width
      int channelOne = Int32.Parse(Math.Round(canvasWidth / 25.83006535947712f).ToString());
      int channelWidth = Int32.Parse(Math.Round(canvasWidth / 3.753086419753086f).ToString());
      int channelValue = channelOne + ((int)channel - 1) * Int32.Parse(Math.Round(canvasWidth / 15.14176245210728f).ToString());
      // Height
      var signalFull = Int32.Parse(Math.Round(canvasHeight / -7.922705314009661f).ToString()); // -34.5
      var signalStrength = signalFull + Int32.Parse(Math.Round(canvasHeight / 8.96174863387978f).ToString()) * (100.0f - (int)signal) / 10.0f; // 36      
      var signalOnCanvas = Int32.Parse(Math.Round(signalStrength).ToString());
      var signalBottom = Int32.Parse(Math.Round(canvasHeight / 0.9712301587301586f).ToString()); // 281      
      // Calculations
      var startPoint = channelValue - (channelWidth / 2) + "," + signalBottom;
      var startPointBezier = channelValue - (channelWidth / 2) + "," + signalOnCanvas;
      var endPointBezier = channelValue + (channelWidth / 2) + "," + signalOnCanvas;
      var endPoint = channelValue + (channelWidth / 2) + "," + signalBottom;

      var bezierCurve = "M " + startPoint + " C " + startPointBezier + " " + endPointBezier + " " + endPoint;

      return bezierCurve;             
    }

    public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
    {
      return null;
    }
  }
}
