using System;

namespace SzeregowaAvalonia.Model
{
    internal interface IDataReciever
    {
        public void RecieveData(object sender, byte data);
        public string EncodeData(byte data);
        public void SetEncoding(EncodingType encoding);
    }
}
