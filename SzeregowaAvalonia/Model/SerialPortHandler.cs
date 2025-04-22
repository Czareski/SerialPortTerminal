using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SzeregowaAvalonia.ViewModels;
using SzeregowaAvalonia.Views;

namespace SzeregowaAvalonia.Model;

class SerialPortHandler
{
    private SerialPort _serialPort;
    public EventHandler<byte> dataRecievedEvent;

    public SerialPortHandler()
    {
        _serialPort = new SerialPort();
    }

    public string[] GetPortNames()
    {
        return SerialPort.GetPortNames();
    }

    public void SetPortName(string value)
    {
        _serialPort.PortName = value;
    }
    public void SetParity(string value)
    {
        if (Enum.TryParse(value, out Parity parity))
        {
            _serialPort.Parity = parity;
        }
        throw new NotImplementedException("Niepoprawny typ");
    }
    public bool SetBaudRate(string value)
    {
        if (int.TryParse(value, out int baudRate)) { 
            _serialPort.BaudRate = baudRate;
            return true;
        }
        throw new NotImplementedException("Niepoprawny typ");        
    }
    public void SetDataBits(string value)
    {
        if (int.TryParse(value, out int dataBits))
        {
            _serialPort.DataBits = dataBits;
        } else
        {
            throw new NotImplementedException("Niepoprawny typ")
        }
        
    }
    public void SetStopBits(string value)
    {
        _serialPort.StopBits = value switch
        {
            "1" => StopBits.One,
            "2" => StopBits.Two,
            "1.5" => StopBits.OnePointFive,
            _ => throw new NotImplementedException("Niepoprawny typ")
        };
    }


    public bool OpenPort()
    {
        try
        {
            // Dodajemy obsługę zdarzenia DataReceived
            _serialPort.DataReceived += SerialPort_DataReceived;
            _serialPort.Open();
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return false;
        }
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        try
        {
            SerialPort sp = (SerialPort)sender;
            while (sp.BytesToRead > 0)
            {
                int indata = sp.ReadByte();

                dataRecievedEvent.Invoke(this, (byte)indata);
            }
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    public void ClosePort()
    {
        if (_serialPort != null)
        {
            if (_serialPort.IsOpen)
            {
                // Odłączamy zdarzenie przed zamknięciem
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Close();
            }
            _serialPort.Dispose();
        }
    }

    public void SendMessage(string message) => _serialPort.Write(message);
    public void SendMessage(byte value) => _serialPort.Write(((char)value).ToString());
    public void SendMessage(char value) => _serialPort.Write(value.ToString());

    // Accessors   
    public string GetPortName() => _serialPort.PortName;
    public Parity GetParity() => _serialPort.Parity;
    public int GetBaudRate() => _serialPort.BaudRate;
    public int GetDataBits() => _serialPort.DataBits;
    public StopBits GetStopBits() => _serialPort.StopBits;

}