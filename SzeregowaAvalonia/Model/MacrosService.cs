using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    public class MacrosService
    {
        private ErrorHandler _errorHandler;
        public ObservableCollection<Macro> CurrentMacrosList;
        public event EventHandler OnMacrosUpdated;
        public MacrosService(ErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            CurrentMacrosList = new ObservableCollection<Macro>(
                Enumerable.Range(0, 20)
                .Select(i => new Macro($"M{i + 1}", ""))
            );
        }
        public void Export(IStorageFile file)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(CurrentMacrosList, options);
            File.WriteAllText(file.Path.AbsolutePath, json);
        }
        public async Task Import(IStorageFile file)
        {
            string jsonFromFile = File.ReadAllText(file.Path.AbsolutePath);
            try
            {
                CurrentMacrosList = JsonSerializer.Deserialize<ObservableCollection<Macro>>(jsonFromFile);
            }
            catch (Exception ex)
            {
                _errorHandler.ReportError("Niepoprawny plik");
            }
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
