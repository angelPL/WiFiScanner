using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using WiFiScanner.ViewModel;
using WiFiScanner.View;

namespace WiFiScanner
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    /// <summary>
    /// Window constuctor
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      titleBar.MouseLeftButtonDown += (o, e) => DragMove();
      
      // to be honest, this is some example from Internet on how to implement custom window in WPF as a border
      new WindowResizer(this,
                new WindowBorder(BorderPosition.TopLeft, topLeft),
                new WindowBorder(BorderPosition.Top, top),
                new WindowBorder(BorderPosition.TopRight, topRight),
                new WindowBorder(BorderPosition.Right, right),
                new WindowBorder(BorderPosition.BottomRight, bottomRight),
                new WindowBorder(BorderPosition.Bottom, bottom),
                new WindowBorder(BorderPosition.BottomLeft, bottomLeft),
                new WindowBorder(BorderPosition.Left, left));        
    }

    /// <summary>
    /// Canvas size changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      // Inform ViewModel about changing the canvas size (recalculate paths)
      ((AppViewModel)this.DataContext).CanvasWidth = MainCanvas.ActualWidth;
      ((AppViewModel)this.DataContext).CanvasHeight = MainCanvas.ActualHeight;
      // now... I don't like this workaround
      List<NetworkDetails> newNetworkValues = new List<NetworkDetails>();
      var networkValues = ((AppViewModel)this.DataContext).NetworkValues;
      if (networkValues != null)
      {
        foreach (var networkValue in networkValues)
        {
          networkValue.NetworkNameChannelSignalColorCanvasWidthAndHeight = new Tuple<string, int, int, string, double, double>(
            networkValue.NetworkNameChannelSignalColorCanvasWidthAndHeight.Item1,
            networkValue.NetworkNameChannelSignalColorCanvasWidthAndHeight.Item2,
            networkValue.NetworkNameChannelSignalColorCanvasWidthAndHeight.Item3,
            networkValue.NetworkNameChannelSignalColorCanvasWidthAndHeight.Item4,            
            MainCanvas.ActualWidth, MainCanvas.ActualHeight);
          newNetworkValues.Add(networkValue);
        }
        ((AppViewModel)this.DataContext).NetworkValues = newNetworkValues;
      }
    }
  }
}
