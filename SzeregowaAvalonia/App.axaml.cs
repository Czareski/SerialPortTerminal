using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DynamicData.Kernel;
using SzeregowaAvalonia.Model;
using SzeregowaAvalonia.ViewModels;
using SzeregowaAvalonia.Views;

namespace SzeregowaAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ErrorHandler errorService = new ErrorHandler();
            SerialPortHandler serialPortHandler = new SerialPortHandler();
            desktop.MainWindow = new MainWindow(new MainViewModel(errorService, serialPortHandler));


        }
        base.OnFrameworkInitializationCompleted();
    }
}   
