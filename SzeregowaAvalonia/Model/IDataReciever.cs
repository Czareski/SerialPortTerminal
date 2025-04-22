using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzeregowaAvalonia.Model
{
    internal interface IDataReciever
    {
        public void RecieveData(byte data);
        public string EncodeData(byte data);
        public void SetEncoding(EncodingType encoding);
    }
}
