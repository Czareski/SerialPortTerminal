using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.Serialization;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SzeregowaAvalonia.Views;

public partial class SettingsView : UserControl
{
    private bool _isCollapsed = false;

    public SettingsView()
    {
        InitializeComponent();
    }

    private void TogglePanel(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_isCollapsed)
        {
            TopPanel.Height = double.NaN; // Przywraca Auto wysokość
            ToggleButton.Content = "▲";
        }
        else
        {
            TopPanel.Height = 50; // Wysokość dla zwiniętej wersji
            ToggleButton.Content = "▼";
        }
        _isCollapsed = !_isCollapsed;
    }

   
    

    public void SendMessage(object sender, RoutedEventArgs e)
    {
         //serialPortManager.SendMessage(Message.Text);
    }
}
public struct PortParameters
{
    public string portName;
    public int baudRate;
    public int dataBits;
    public Parity parity;
    public StopBits stopBits;
}