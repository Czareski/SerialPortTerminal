using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class ErrorViewModel : ViewModelBase
    {

        [ObservableProperty]
        private string _errorMessage = "";
        [ObservableProperty]
        public bool _isShown = false;
        public ErrorViewModel(ErrorHandler errorService) {
            Debug.WriteLine("Error View Model");
            errorService.ErrorReceived += OnErrorReceived;
        }
        private async void OnErrorReceived(object sender, string errorMessage)
        {
            Debug.WriteLine($"Error received ASDASd: {errorMessage}");

            IsShown = true;
            ErrorMessage = errorMessage;
            await Task.Delay(3000);
            IsShown = false;

        }

    }
}
