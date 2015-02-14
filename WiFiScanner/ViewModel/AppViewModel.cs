using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WiFiScanner.ViewModel
{
  /// <summary>
  /// Window command types (or state)
  /// </summary>
  public enum WindowCommandType
  {
    MinimizeWindow,
    MaximizeWindow,
    CloseWindow
  }

  /// <summary>
  /// Class that represents Network details
  /// </summary>
  public class NetworkDetails
  {
    /// <summary>
    /// Indicates wheter network should be shown on graph (canvas)
    /// </summary>
    public bool Show { get; set; }
    /// <summary>
    /// Security type of network (WEP, WPA2, Open etc.)
    /// </summary>
    public string Security { get; set; }
    /// <summary>
    /// MAC address of router (phisical address)
    /// </summary>
    public string NetworkBSSID { get; set; }
    /// <summary>
    /// Radio type of network (802.11g, 802.11n etc.)
    /// </summary>
    public string Mode { get; set; }            
    /// <summary>
    /// it's not a good idea, but it works (I don't know why, but with MultiBinding doesn't work!)
    /// Set of information (why? explained above):
    /// - NetworkName: SSID of network
    /// - Channel: channel that network is taking (1-13)
    /// - Signal: signal strength in percentage
    /// - Color: assigned color for displaying in DataGrid and on Graph (canvas)
    /// - CanvasWidth: actual canvas width (must be somehow sent, I am not proud of this solution)
    /// - CanvasHeight: actual canvas height
    /// </summary>
    public Tuple<string, int, int, string, double, double> NetworkNameChannelSignalColorCanvasWidthAndHeight { get; set; }    
  }

  /// <summary>
  /// Application ViewModel
  /// </summary>
  class AppViewModel : DependencyObject
  {
    /// <summary>
    /// Empty constructor
    /// </summary>
    public AppViewModel()
    {
   
    }

    #region Fields

    /// <summary>
    /// Array of colors for networks
    /// </summary>
    public string[] _colors = { "#FF3EF359", "#FFFF80AA", "#FFC6F33E", "#FF8A80FF", "#FFFFBF80", "#FF80FFF6", "#FF808080", "#FFDC73FF", "#FFCC546E", "#FFA0CC54", "#FFCC54CC", "#FFFF0000" };

    #endregion

    #region Properties    

    /// <summary>
    /// Graph Canvas width (set in MainWindow.xaml.cs)
    /// </summary>
    public double CanvasWidth
    {
      get { return (double)GetValue(CanvasWidthProperty); }
      set { SetValue(CanvasWidthProperty, value); }
    }
    
    public static readonly DependencyProperty CanvasWidthProperty =
        DependencyProperty.Register("CanvasWidth", typeof(double), typeof(AppViewModel), new PropertyMetadata(0.0, new PropertyChangedCallback(OnCanvasWidthChanged)));

    public static void OnCanvasWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      // when width of canvas change recalculate the paths
      // done in MainWindow.caml.cs... (workaround) I don't know how to do it from ViewModel
    }

    /// <summary>
    /// Graph Canvas height (set in MainWindow.xaml.cs)
    /// </summary>
    public double CanvasHeight
    {
      get { return (double)GetValue(CanvasHeightProperty); }
      set { SetValue(CanvasHeightProperty, value); }
    }
    
    public static readonly DependencyProperty CanvasHeightProperty =
        DependencyProperty.Register("CanvasHeight", typeof(double), typeof(AppViewModel), new PropertyMetadata(0.0, new PropertyChangedCallback(OnCanvasHeightChanged)));

    public static void OnCanvasHeightChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      // when height of canvas change recalculate the paths
      // done in MainWindow.caml.cs... (workaround) I don't know how to do it from ViewModel 
    }

    /// <summary>
    /// Information about Wi-Fi networks
    /// </summary>
    public List<NetworkDetails> NetworkValues
    {
      get { return (List<NetworkDetails>)GetValue(NetworkValuesProperty); }
      set { SetValue(NetworkValuesProperty, value); }
    }
    
    public static readonly DependencyProperty NetworkValuesProperty =
        DependencyProperty.Register("NetworkValues", typeof(List<NetworkDetails>), typeof(AppViewModel), new PropertyMetadata(null));

    #endregion

    #region Commands

    /// <summary>
    /// Close window command
    /// </summary>
    private ICommand _closeCommand;
    public ICommand CloseCommand
    {
        get
        {
            return _closeCommand ?? (_closeCommand = new WindowCommand(WindowCommandType.CloseWindow));
        }
    }

    /// <summary>
    /// Minimize window command
    /// </summary>
    private ICommand _minimizeCommand;
    public ICommand MinimizeCommand
    {
      get
      {
        return _minimizeCommand ?? (_minimizeCommand = new WindowCommand(WindowCommandType.MinimizeWindow));
      }
    }

    /// <summary>
    /// Maximize window command
    /// </summary>
    private ICommand _maximizeCommand;
    public ICommand MaximizeCommand
    {
      get
      {
        return _maximizeCommand ?? (_maximizeCommand = new WindowCommand(WindowCommandType.MaximizeWindow));
      }
    }

    /// <summary>
    /// Start scanning networks
    /// </summary>
    private ICommand _startScanningCommand;
    public ICommand StartScanningCommand
    {
      get
      {
        return _startScanningCommand ?? (_startScanningCommand = new ScanningCommand(() => MyAction()));
      }
    }

    /// <summary>
    /// Execute Netsh command, parse output, set NetworkValues property
    /// </summary>
    public void MyAction()
    {      
      NetworkValues = ParseNetshOutput(StartScanningNetsh());
    }
    #endregion

    #region Methods

    /// <summary>
    /// Using Netsh "wlan show networks mode=bssid" command, display Wi-Fi networks and their details
    /// </summary>
    /// <returns>Result of Netsh command - Wi-Fi networks</returns>
    public string StartScanningNetsh()
    {
      Process proc = new Process();
      proc.StartInfo.CreateNoWindow = true;
      proc.StartInfo.FileName = "netsh";
      proc.StartInfo.Arguments = "wlan show networks mode=bssid";
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.UseShellExecute = false;
      proc.Start();
      var output = proc.StandardOutput.ReadToEnd();
      proc.WaitForExit();

      return output;
    }

    /// <summary>
    /// Parsing Netsh output
    /// </summary>
    /// <param name="netshOutput">Output from netsh command</param>
    /// <returns>List of networks details</returns>
    public List<NetworkDetails> ParseNetshOutput(string netshOutput)
    {
      List<NetworkDetails> _returnList = new List<NetworkDetails>();
      string[] netshLines = Regex.Split(netshOutput, "\r\n");

      int j = 0; // color counter

      for (int i = 0; i < netshLines.Length; i++)
      {
        var currentLine = netshLines[i];

        if (currentLine.StartsWith("SSID")) // found new network
        {
          string networkSSID = currentLine.Split(new char[] { ':' }, 2)[1].Trim();         
          string authentication = netshLines[i + 2].Split(new char[] { ':' }, 2)[1].Trim();
          string encryption = netshLines[i + 3].Split(new char[] { ':' }, 2)[1].Trim();
          if (encryption == "WEP")
            authentication = "WEP";
          string networkBSSID = netshLines[i + 4].Split(new char[] { ':' }, 2)[1].Trim();
          string signalStrengthPercent = netshLines[i + 5].Split(new char[] { ':' }, 2)[1].Trim();
          int signalStrength = Int32.Parse(signalStrengthPercent.Substring(0, signalStrengthPercent.Length - 1));
          string radioType = netshLines[i + 6].Split(new char[] { ':' }, 2)[1].Trim();
          int channel = Int32.Parse(netshLines[i + 7].Split(new char[] { ':' }, 2)[1].Trim());          
          string colorString = _colors[j % _colors.Length];
          j++;

          _returnList.Add(new NetworkDetails() { Show = true,
                                            Security = authentication,
                                            NetworkBSSID = networkBSSID,                                            
                                            Mode = radioType,
                                            NetworkNameChannelSignalColorCanvasWidthAndHeight = new Tuple<string, int, int, string, double, double>(networkSSID, channel, signalStrength, colorString, CanvasWidth, CanvasHeight)
          });

          i += 10;          
          continue;
        }
      }

      return _returnList;
    }

    #endregion
  }

  public class WindowCommand : ICommand
  {
    WindowCommandType _commandType;

    public WindowCommand(WindowCommandType commandType)
    {
      _commandType = commandType;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
      // Logic goes here
      if (_commandType == WindowCommandType.CloseWindow)
        ((Window)parameter).Close();
      else if (_commandType == WindowCommandType.MinimizeWindow)
        ((Window)parameter).WindowState = WindowState.Minimized;
      else if (_commandType == WindowCommandType.MaximizeWindow)
      {
        if (((Window)parameter).WindowState == WindowState.Normal)
          ((Window)parameter).WindowState = WindowState.Maximized;
        else if (((Window)parameter).WindowState == WindowState.Maximized)
          ((Window)parameter).WindowState = WindowState.Normal;
      }
    }
  }

  public class ScanningCommand : ICommand
  {
    private Action _action;

    public ScanningCommand(Action action)
    {
      _action = action;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
      // Logic goes here
      _action();      
    }    
  }  
}
