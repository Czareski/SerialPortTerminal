using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using SzeregowaAvalonia.Model;
using ReactiveUI;
using SzeregowaAvalonia.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class SendViewModel : ViewModelBase
    {
        private string _commandText = string.Empty;
        private CommandProcessor _commandProcessor = new CommandProcessor();
        public int SelectedLineEnding { get; set; }
        public string CommandText
        {
            get => _commandText;
            set
            {
                _commandText = value;
                OnPropertyChanged();
            }
        }
        //public ICommand OpenDialogCommand { get; }


        public SendViewModel()
        {
            //OpenDialogCommand = ReactiveCommand.CreateFromTask(async () =>
            //{
            //    var dialog = new MacroWindow();
            //    bool result = await dialog.ShowDialog<bool>((Window)App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime life ? life.MainWindow : null!);
            //});
        }

        [RelayCommand]
        public void SendCommand()
        {
            string lineEnding = "";
            // tu daje boxitem zamiast realnie item bruh
            System.Diagnostics.Debug.WriteLine(SelectedLineEnding);
            switch (SelectedLineEnding)
            {
                case 1:
                    lineEnding += "\r";
                    break;
                case 2:
                    lineEnding += "\n";
                    break;
                case 3:
                    lineEnding += "\r\n";
                    break;
                case 4:
                    lineEnding += "\n\r";
                    break;
                default:
                    break;
            }
            _commandProcessor.SendText(CommandText + lineEnding);
        }

    }
}
