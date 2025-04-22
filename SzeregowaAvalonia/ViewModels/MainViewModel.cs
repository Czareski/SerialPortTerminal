using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainViewModel(ErrorHandler errorHandler, SerialPortHandler serialPortHandler)
        {
            SettingsVM = new SettingsViewModel(errorHandler, serialPortHandler);
            RecieveViewModel = new RecieveViewModel(errorHandler, serialPortHandler);
            SendViewModel = new SendViewModel(errorHandler, serialPortHandler);
            ErrorViewModel = new ErrorViewModel(errorHandler);
        }
    }
}
