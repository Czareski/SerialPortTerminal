using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using Avalonia.Platform.Storage;

namespace SzeregowaAvalonia.Model
{
    public class SerialPortDataReciever
    {
        private List<IDataReciever> _dataOutputs;
        private SerialPort _serialPort;
        private EncodingType _currentEncoding = EncodingType.ASCII;
        public FileLogger? FileLogger;
        public TerminalOutput Terminal { get; } = new TerminalOutput();
        
        public EventHandler<byte> DataRecieved;
        public SerialPortDataReciever(SerialPort serialPort)
        {
            _serialPort = serialPort;
            _dataOutputs = [Terminal];
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
                    foreach (IDataReciever dataReciever in _dataOutputs)
                    {
                        dataReciever.RecieveData(sender, (byte)data);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        public void SetEncoding(EncodingType encoding)
        {
            _currentEncoding = encoding;
            foreach (IDataReciever dataReciever in _dataOutputs)
            {
                dataReciever.SetEncoding(encoding);
            }
        }
        public void StopFileLog()
        {
            FileLogger.Dispose();
            _dataOutputs.Remove(FileLogger);
            FileLogger = null;
        }

        public void StartFileLog(IStorageFile file)
        {
            FileLogger = new FileLogger(file);
            FileLogger.SetEncoding(_currentEncoding);
            _dataOutputs.Add(FileLogger);
        }
    }
    public enum EncodingType
    {
        ASCII = 0,
        HEX = 1
    }
}
