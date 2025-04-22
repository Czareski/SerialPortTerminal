using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Diagnostics.Screenshots;
using Avalonia.Dialogs;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class RecieveViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public TerminalOutput Terminal { get; } = new TerminalOutput();
        private FileLogger? _fileLogger;

        private List<IDataReciever> _dataOutputs;
        
        [ObservableProperty]
        private string _loggingButtonContent = "Start Log";

        [ObservableProperty]
        private int _bytesRecieved;

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
                Debug.WriteLine(_encoding);
                foreach (var reciever in _dataOutputs)
                {
                    reciever.SetEncoding((EncodingType)_encoding);
                }
            }
        }

        public RecieveViewModel(ErrorHandler errorHandler, SerialPortDataReciever dataReciever)
        {
            _dataOutputs = [Terminal];
            dataReciever.DataRecieved += RecieveData;

        }

        private void RecieveData(object sender, byte data)
        {
            BytesRecieved += 1;
            
            foreach (var reciever in _dataOutputs)
            {
                reciever.RecieveData(data);
            }
        }

        [RelayCommand]
        public void Search()
        {
            Terminal.Search(InputText);
        }

        [RelayCommand]
        public void Clear()
        {
            Terminal.Clear();
            BytesRecieved = 0;
        }

        [RelayCommand]
        public void HandleLogButton() {
            if (_fileLogger != null)
            {
                _fileLogger.Dispose();
                _dataOutputs.Remove(_fileLogger);
                _fileLogger = null;
                LoggingButtonContent = "Start Log";
            } else
            {
                StartFileLog();
                
            }     
        }

        [RelayCommand]
        public void SearchNext()
        {
            Terminal.SearchForNextOccurance();
        }
        public async void StartFileLog() {
            IStorageFile selectedFile = await FileDialogHandler.OpenFileDialog();
            if (selectedFile == null)
            {
                return;
            }
            LoggingButtonContent = "Stop Log";
            _dataOutputs.Add(new FileLogger(selectedFile));
        }

    }
}
