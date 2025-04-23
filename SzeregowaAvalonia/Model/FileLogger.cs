using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    public class FileLogger : IDataReciever, IDisposable
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
        public void RecieveData(object sender,byte data)
        {
            string encoded = EncodeData(data);
            
            _writer.Write(encoded);
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
            string encodedData = "";
            if (_encoding == EncodingType.HEX)
            {
                encodedData = Convert.ToHexString(new byte[] { data }) + " ";
            }
            else
            {
                encodedData += (char)data;
            }
            return encodedData;
        }

        public void SetEncoding(EncodingType encoding)
        {
            _encoding = encoding;
        }
    }
}
