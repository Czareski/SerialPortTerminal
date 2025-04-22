using CommunityToolkit.Mvvm.Input;
using SzeregowaAvalonia.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using SzeregowaAvalonia.Views;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class SendViewModel : ViewModelBase
    {
        private string _commandText = string.Empty;
        private CommandProcessor _commandProcessor;
        
        [ObservableProperty]
        public ObservableCollection<Macro> _macros = new ObservableCollection<Macro>();
        
        private MacrosService _macrosService = new MacrosService();
        public string CommandText
        {
            get => _commandText;
            set
            {
                _commandText = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private bool _isCRChecked;

        [ObservableProperty]
        private bool _isNLChecked;
        [ObservableProperty]
        private bool _isNLCRChecked;

        private LineEnding _selectedLineEnding;
        private bool AreOrderButtonsVisible => IsCRChecked && IsNLChecked;
        private ErrorHandler _errorHandler { get; }

        public SendViewModel(ErrorHandler errorHandler, SerialPort serialPort)
        {
            _errorHandler = errorHandler;
            _commandProcessor = new CommandProcessor(serialPort);
            _macrosService.OnMacrosUpdated += UpdateMacros;
            for (int i = 0; i < 20; i++)
            {
                Macros.Add(new Macro(_macrosService.CurrentMacrosList[i].Title, _macrosService.CurrentMacrosList[i].Command));
            }
        }


        partial void OnIsCRCheckedChanged(bool value)
        {
            OnPropertyChanged(nameof(AreOrderButtonsVisible));
            if (AreOrderButtonsVisible)
            {
                SetDefaultOrder();
                return;
            }
            if (value && !IsNLChecked)
            {
                _selectedLineEnding = LineEnding.CR;
                return;
            }
            if (!IsCRChecked && !IsNLChecked)
            {
                _selectedLineEnding = LineEnding.None;
            }
        }

        partial void OnIsNLCheckedChanged(bool value)
        {
            OnPropertyChanged(nameof(AreOrderButtonsVisible));
            if (AreOrderButtonsVisible)
            {
                SetDefaultOrder();
                return;
            }
            if (value && !IsCRChecked)
            {
                _selectedLineEnding = LineEnding.NL;
                return;
            }
            if (!IsCRChecked && !IsNLChecked)
            {
                _selectedLineEnding = LineEnding.None;
            }
        }
        private void SetDefaultOrder()
        {
            IsNLCRChecked = true;
            _selectedLineEnding = LineEnding.NLCR;
        }
        [RelayCommand]
        public void SetOrder(string order)
        {
            _selectedLineEnding = (LineEnding)Convert.ToByte(order);
        }


        [RelayCommand]
        public void SendCommand()
        {
            if (!AreOrderButtonsVisible)
            {
                if (IsCRChecked)
                {
                    _selectedLineEnding = LineEnding.CR;
                }
                else if (IsNLChecked)
                {
                    _selectedLineEnding = LineEnding.NL;
                }
            }

            _commandProcessor.SendText(CommandText, _selectedLineEnding);
        }
        [RelayCommand]
        public void OpenMacroWindow()
        {
            var window = new MacroWindow();
            window.DataContext = new MacroViewModel(_macrosService);
            window.Show();
        }
        private void UpdateMacros(object sender, EventArgs args)
        {
            for (int i = 0; i < 20; i++)
            {
                Macros[i].Title = _macrosService.CurrentMacrosList[i].Title;
                Macros[i].Command = _macrosService.CurrentMacrosList[i].Command;
            }
        }
        [RelayCommand]
        public void UseMacro(string param)
        {
            int index = Convert.ToInt32(param);
            CommandText = Macros[index].Command;
        }
    }
           
}
