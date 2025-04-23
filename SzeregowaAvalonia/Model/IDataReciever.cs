using System;

namespace SzeregowaAvalonia.Model
{
    internal interface IDataReciever
    {
        public void RecieveData(byte data);
        public string EncodeData(byte data);
        public void SetEncoding(EncodingType encoding);
    }
}
