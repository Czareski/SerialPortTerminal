using System;

namespace SzeregowaAvalonia.Model
{
    public class ErrorHandler
    {
        public event EventHandler<string> ErrorReceived;

        public void ReportError(string message)
        {
            ErrorReceived?.Invoke(this, message);
        }
    }
}
