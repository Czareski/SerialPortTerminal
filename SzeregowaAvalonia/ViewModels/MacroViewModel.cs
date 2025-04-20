using System.Collections.ObjectModel;
using System.Diagnostics;
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
            MacrosList = new ObservableCollection<Macro>(macrosService.CurrentMacrosList);
            
        }
        [RelayCommand]
        public void SaveMacros()
        {
            
            service.SaveMacros(MacrosList);
        }
    }
}
