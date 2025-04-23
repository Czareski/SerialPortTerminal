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
            ToggleButton.Content = "Hide Configuration";
        }
        else
        {
            BottomPart.IsVisible = false; 
            ToggleButton.Content = "Show Configuration";
        }
        _isCollapsed = !_isCollapsed;
    }
}
