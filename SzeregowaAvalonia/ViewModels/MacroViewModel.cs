using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class MacroViewModel : ViewModelBase
    {
        [ObservableProperty]
        public ObservableCollection<Macro> _macrosList;
        private MacrosService service;
        public MacroViewModel(MacrosService macrosService)
        {
            service = macrosService;
            MacrosList = new ObservableCollection<Macro>(
                Enumerable.Range(0, 20)
                .Select(i => new Macro($"M{i + 1}", ""))
            );

        }
        [RelayCommand]
        public void SaveMacros()
        {
            Debug.WriteLine("SaveMacros");
            service.SaveMacros(MacrosList);
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
