using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    internal class FileLogger : IDataOutput, IDisposable
    {
        private IStorageFile _file;
        private StreamWriter _writer;
        private bool _isDisposed;
        
        public FileLogger(IStorageFile file)
        {
            _file = file;
            _writer = new StreamWriter(_file.Path.AbsolutePath, append: true);

        }
        public void RecieveData(byte data)
        {
            System.Diagnostics.Debug.WriteLine("File Logger otrzymał: " + data);
            // tu kiedys wyskoczyl blad
            _writer.Write((char)data);
            _writer.Flush();
        }
        public void RecieveHexData(string data)
        {
            _writer.Write(data);
            _writer.Flush();
        }
        ~FileLogger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _writer.Close();

            _isDisposed = true;
        }

        
    }
}
