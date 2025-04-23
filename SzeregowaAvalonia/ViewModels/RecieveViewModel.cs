using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class RecieveViewModel : ViewModelBase, INotifyPropertyChanged
    {
        
        private IStorageFile _selectedFile;
        private ErrorHandler _errorHandler;
        [ObservableProperty]
        private SerialPortDataReciever _dataReciever;
        
        [ObservableProperty]
        private string _loggingButtonContent = "Zapisuj do pliku";

        [ObservableProperty]
        private int _bytesRecieved;

        [ObservableProperty]
        public string _selectedFileName = "";

        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }
        
        private int _encoding;
        public int Encoding {
            get => _encoding;
            set
            {
                _encoding = value;
                DataReciever.SetEncoding((EncodingType)_encoding);
            }
        }

        public RecieveViewModel(ErrorHandler errorHandler, SerialPortDataReciever dataReciever)
        {
            _errorHandler = errorHandler;
            _dataReciever = dataReciever;
            dataReciever.DataRecieved += IncreaseBytesRecieved;

        }

        private void IncreaseBytesRecieved(object sender, byte data)
        {
            BytesRecieved += 1;
        }

        [RelayCommand]
        public void Search()
        {
            DataReciever.Terminal.Search(InputText);
        }

        [RelayCommand]
        public void Clear()
        {
            DataReciever.Terminal.Clear();
            BytesRecieved = 0;
        }
        [RelayCommand]
        public async void SelectFile()
        {
            IStorageFile selectedFile = await FileDialogHandler.OpenFileDialog();
            if (selectedFile != null)
            {
                _selectedFile = selectedFile;
                SelectedFileName = _selectedFile.Name;
                return;
            }
            _errorHandler.ReportError("Żaden plik nie został wybrany");
        }

        [RelayCommand]
        public void HandleLogButton() {
            if (DataReciever.FileLogger != null)
            {
                LoggingButtonContent = "Zapisuj do pliku";
                DataReciever.StopFileLog();
            } else
            {
                if (_selectedFile == null)
                {
                    _errorHandler.ReportError("Nie wybrano pliku");
                    return;
                }

                LoggingButtonContent = "Zatrzymaj zapisywanie";

                DataReciever.StartFileLog(_selectedFile);
            }

        }

        [RelayCommand]
        public void SearchNext()
        {
            if (!DataReciever.Terminal.SearchForNextOccurance())
            {
                _errorHandler.ReportError("Nie znaleziono wystąpienia");
            }
        }

    }
}
