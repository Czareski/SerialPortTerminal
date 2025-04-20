using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class RecieveViewModel : ViewModelBase, INotifyPropertyChanged
    {

        // true - ASCII, false - HEX | Może osobna klasa ?
        private bool isASCIIencoded = true;

        private List<IDataOutput> _dataOutputs = [];

        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(); // lub OnPropertyChanged(nameof(InputText)) zależnie od bazy
            }
        }
        public TerminalOutput Terminal { get; } = new TerminalOutput();
        
        [ObservableProperty]
        private string _loggingButtonContent = "Start Log";

        [ObservableProperty]
        private int _bytesRecieved;

        [ObservableProperty]
        private int _encoding;


        public RecieveViewModel()
        {
            if (SerialPortManager.Instance == null)
            {
                new SerialPortManager();
            }
            _dataOutputs.Add(Terminal);
            
            SerialPortManager.Instance.dataRecievedEvent += RecieveData;
        }

        

        [RelayCommand]
        private void RecieveData(byte data)
        {
            BytesRecieved += 1;

            if (Encoding == 0)
            {
                foreach (IDataOutput output in _dataOutputs)
                {
                    output.RecieveData(data);
                }
            }
            else
            {
                string hexString = Convert.ToHexString(new byte[] { data }) + " ";
                foreach (IDataOutput output in _dataOutputs)
                {
                    output.RecieveHexData(hexString);
                }
               
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
        public void ChangeEncoding(bool value)
        {
            isASCIIencoded = value;
        }

        [RelayCommand]
        public void HandleLogButton() {
            foreach (IDataOutput output in _dataOutputs)
            {
                if (output is FileLogger)
                {
                    FileLogger fileLogger = (FileLogger)output;
                    
                    fileLogger.Dispose();
                    _dataOutputs.Remove(fileLogger);

                    LoggingButtonContent = "Start Log";
                    return;
                }
            }

            StartFileLog();
        }

        [RelayCommand]
        public void SearchNext()
        {
            Terminal.SearchNext();
        }


        // ???
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
