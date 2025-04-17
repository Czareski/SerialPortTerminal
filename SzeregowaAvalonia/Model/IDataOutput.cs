using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzeregowaAvalonia.Model
{
    internal interface IDataOutput
    {
        void RecieveData(byte data);
        void RecieveHexData(string data);
    }
}
