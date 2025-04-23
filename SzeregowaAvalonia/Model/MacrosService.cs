using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            string path = file.Path.AbsolutePath;
            StreamWriter writer = new StreamWriter(path);
            foreach (var macro in CurrentMacrosList)
            {
                writer.WriteLine(macro.Title);
                writer.WriteLine(macro.Command);
            }
            writer.Dispose();
        }
        public async Task Import(IStorageFile file)
        {
            string path = file.Path.AbsolutePath;
            StreamReader reader = new StreamReader(path);
            for (int i = 0; i < 20 * 2; i++)
            {
                string lineContent = await reader.ReadLineAsync();
                if (lineContent == null)
                {
                    _errorHandler.ReportError("Niepoprawny plik makr (minimum 40linii)");
                    return;
                }
                if (i % 2 == 0)
                {
                    CurrentMacrosList[i / 2].Title = lineContent;
                } else
                {
                    CurrentMacrosList[i / 2].Command = lineContent;
                }
            }
            return;
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
