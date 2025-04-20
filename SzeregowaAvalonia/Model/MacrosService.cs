using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    public class MacrosService
    {
        private ErrorService _errorService;
        public ObservableCollection<Macro> CurrentMacrosList;
        public event EventHandler OnMacrosUpdated;
        public MacrosService()
        {
            CurrentMacrosList = new ObservableCollection<Macro>(
                Enumerable.Range(0, 20)
                .Select(i => new Macro("Titlexd", "Command"))
            );
        }
        public List<Macro> ExportFromFile(IStorageFile file)
        {
            return null;
        }
        public List<Macro> ImportFromFile(IStorageFile file)
        {
            return null;
        }
        public Macro GetMacro(int i)
        {
            return CurrentMacrosList[i];
        }
        public void SaveMacros(ObservableCollection<Macro> macros)
        {
            CurrentMacrosList = macros;
            
            OnMacrosUpdated?.Invoke(this, EventArgs.Empty);
        }

    }
}
