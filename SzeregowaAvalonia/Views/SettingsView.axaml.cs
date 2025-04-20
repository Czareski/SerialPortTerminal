using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.Serialization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SzeregowaAvalonia.ViewModels;

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
            BottomPart.IsVisible = true; // Przywraca Auto wysokość
            ToggleButton.Content = "Hide Config";
        }
        else
        {
            BottomPart.IsVisible = false; // Wysokość dla zwiniętej wersji
            ToggleButton.Content = "Show Config";
        }
        _isCollapsed = !_isCollapsed;
    }
}
