using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    internal class FileLogger : IDataReciever, IDisposable
    {
        private IStorageFile _file;
        private StreamWriter _writer;
        private bool _isDisposed;
        private EncodingType _encoding;
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

        public string EncodeData(byte data)
        {
            throw new NotImplementedException();
        }

        public void SetEncoding(EncodingType encoding)
        {
            _encoding = encoding;
        }
    }
}
