using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzeregowaAvalonia.Model
{
    public class SerialPortDataReciever
    {
        private SerialPort _serialPort;
        
        public EventHandler<byte> DataRecieved;
        public SerialPortDataReciever(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _serialPort.DataReceived += OnDataRecieved;
        }

        public void OnDataRecieved(object sender, EventArgs args)
        {
            try
            {
                while (_serialPort.BytesToRead > 0)
                {
                    int data = _serialPort.ReadByte();
                    DataRecieved?.Invoke(this, (byte)data);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
    public enum EncodingType
    {
        ASCII = 0,
        HEX = 1
    }
}
