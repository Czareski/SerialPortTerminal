using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        public SettingsViewModel SettingsVM { get; }
        public RecieveViewModel RecieveViewModel { get; }
        public SendViewModel SendViewModel { get; }
        public ErrorViewModel ErrorViewModel { get; }
        public MainViewModel(Window mainWindow, ErrorHandler errorHandler, SerialPort serialPort)
        {
            SettingsVM = new SettingsViewModel(errorHandler, serialPort);

            SerialPortDataReciever dataReciever = new SerialPortDataReciever(serialPort);
            RecieveViewModel = new RecieveViewModel(errorHandler, dataReciever);
            
            SendViewModel = new SendViewModel(mainWindow, errorHandler, serialPort);
            ErrorViewModel = new ErrorViewModel(errorHandler);
        }
    }
}
