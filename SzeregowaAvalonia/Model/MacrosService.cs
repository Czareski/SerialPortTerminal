using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    public class MacrosService
    {
        private ErrorHandler _errorService;
        public ObservableCollection<Macro> CurrentMacrosList;
        public event EventHandler OnMacrosUpdated;
        public MacrosService()
        {
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
        public async Task<Task> Import(IStorageFile file)
        {
            string path = file.Path.AbsolutePath;
            StreamReader reader = new StreamReader(path);
            for (int i = 0; i < 20 * 2; i++)
            {
                string lineContent = await reader.ReadLineAsync();
                if (lineContent == null)
                {
                    throw new Exception("ZA MAŁO LINI");
                }
                if (i % 2 == 0)
                {
                    CurrentMacrosList[i / 2].Title = lineContent;
                } else
                {
                    CurrentMacrosList[i / 2].Command = lineContent;
                }
            }
            return Task.CompletedTask;
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
