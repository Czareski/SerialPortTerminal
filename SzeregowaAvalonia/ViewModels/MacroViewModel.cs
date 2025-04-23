using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class MacroViewModel : ViewModelBase
    {
        private MacrosService service;
        private Window _window;
        [ObservableProperty]
        public ObservableCollection<Macro> _macrosList = new ObservableCollection<Macro>();
        public MacroViewModel(MacrosService macrosService, Window window)
        {
            _window = window;
            service = macrosService;
            // kopia makr z _macrosService do lokalnej listy (nie może być referencji do listy z service, ponieważ wymagane jest zatwiedzenie na przycisku Zapisz)
            for (int i = 0; i < 20; i++)
            {
                MacrosList.Add(new Macro(service.CurrentMacrosList[i].Title, service.CurrentMacrosList[i].Command));
            }

        }
        [RelayCommand]
        public void SaveMacros()
        {
            service.SaveMacros(MacrosList);
            _window.Close();
        }
        [RelayCommand]
        public async void Import()
        {
            IStorageFile selectedFile = await FileDialogHandler.OpenFileDialog();
            if (selectedFile == null)
            {
                return;
            }
            await service.Import(selectedFile);
            for (int i = 0; i < 20; i++)
            {
                MacrosList[i].Title = service.CurrentMacrosList[i].Title;
                MacrosList[i].Command = service.CurrentMacrosList[i].Command;
            }
        }

        [RelayCommand]
        public async void Export()
        {
            IStorageFile selectedFile = await FileDialogHandler.OpenFileDialog();
            if (selectedFile == null)
            {
                return;
            }
            service.Export(selectedFile);
        }

    }
}
