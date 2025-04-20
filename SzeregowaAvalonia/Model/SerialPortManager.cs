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

public delegate void DataRecieved(byte data);
class SerialPortManager
{
    public static SerialPortManager Instance;
    private SerialPort _serialPort;
    public event DataRecieved dataRecievedEvent;

    public SerialPortManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
    }
    public bool SetBaudRate(string value)
    {
        if (int.TryParse(value, out int baudRate)) { 
            _serialPort.BaudRate = baudRate;
            return true;
        }
        return false;
        
    }
    public void SetDataBits(string value)
    {
        if (int.TryParse(value, out int dataBits))
        {
            _serialPort.DataBits = dataBits;
        } else
        {

        }
        
    }
    public void SetStopBits(string value)
    {
        switch (value)
        {
            case "1":
                _serialPort.StopBits = StopBits.One;
                break;
            case "2":
                _serialPort.StopBits = StopBits.Two;
                break;
            case "1.5":
                _serialPort.StopBits = StopBits.OnePointFive;
                break;
            default:
                break;
        }
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
                
                dataRecievedEvent.Invoke((byte)indata);
            }
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
    public void SendMessage(string message)
    {
        _serialPort.Write(message);
    }
    public void SendMessage(byte hexValue)
    {
        _serialPort.Write("" + (char)hexValue);
    }
    public void SendMessage(char value)
    {
        _serialPort.Write("" + value);
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
                Console.WriteLine("Port został zamknięty.");
            }
            _serialPort.Dispose();
        }
    }
    public string GetPortName()
    {
        return _serialPort.PortName;
    }

    public Parity GetParity()
    {
        return _serialPort.Parity;
    }

    public int GetBaudRate()
    {
        return _serialPort.BaudRate;
    }

    public int GetDataBits()
    {
        return _serialPort.DataBits;
    }

    public StopBits GetStopBits()
    {
        return _serialPort.StopBits;
    }

}