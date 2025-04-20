using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SzeregowaAvalonia.ViewModels;

namespace SzeregowaAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel VM)
    {
        InitializeComponent();
        DataContext = VM;
        Debug.WriteLine(DataContext.ToString());
    }
    //private async void ShowError(object sender, RoutedEventArgs args)
    //{
    //    Error.IsVisible = true;
    //    ErrorTextBox.Text = "You have to connect with serial port first...";
    //    Error.Classes.Add("shown");
    //    await Task.Delay(3000); // pokazuj przez 3 sekundy
    //    Error.Classes.Remove("shown");
    //    await Task.Delay(100);
    //    Error.IsVisible = false;
    //}
}
