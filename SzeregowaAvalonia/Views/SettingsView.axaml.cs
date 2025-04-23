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

    private void TogglePanel(object sender, RoutedEventArgs e)
    {
        if (_isCollapsed)
        {
            BottomPart.IsVisible = true; 
            ToggleButton.Content = "Ukryj konfiguracje";
        }
        else
        {
            BottomPart.IsVisible = false; 
            ToggleButton.Content = "Pokaż konfiguracje";
        }
        _isCollapsed = !_isCollapsed;
    }
}
