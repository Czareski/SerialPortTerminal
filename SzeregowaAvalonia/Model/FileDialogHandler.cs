using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    internal class FileDialogHandler
    {
        private static Window GetMainWindow()
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow!;
            }
            return null!;
        }

        public static async Task<IStorageFile> OpenFileDialog()
        {
            IStorageProvider storageProvider = TopLevel.GetTopLevel(GetMainWindow()).StorageProvider;

            IReadOnlyList<IStorageFile> files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "",
                FileTypeFilter = [FilePickerFileTypes.TextPlain]
            });

            if (files.Count == 0)
            {
                return null!;
            }
            return files[0];
        }
    }
}
