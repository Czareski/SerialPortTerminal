using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzeregowaAvalonia.Model
{
    public class ErrorService
    {
        public event EventHandler<string> ErrorReceived;

        public void ReportError(string message)
        {
            ErrorReceived?.Invoke(this, message);
        }
    }
}
