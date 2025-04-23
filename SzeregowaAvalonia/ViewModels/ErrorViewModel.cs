﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource? _errorCts;

        public ErrorViewModel(ErrorHandler errorService) {
            errorService.ErrorReceived += OnErrorReceived;
        }
        private async void OnErrorReceived(object sender, string errorMessage)
        {

            _errorCts?.Cancel();

            
            _errorCts = new CancellationTokenSource();

            try
            {
                await ShowError(errorMessage, _errorCts.Token);
            }
            catch (OperationCanceledException)
            {
                // Poprzedni błąd został przerwany
            }

        }
        private async Task ShowError(string errorMessage, CancellationToken token)
        {
            IsShown = true;
            ErrorMessage = errorMessage;

            await Task.Delay(3000, token);

            IsShown = false;
        }
    }
}
