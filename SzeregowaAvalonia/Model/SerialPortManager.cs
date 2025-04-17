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
    }

    public string[] GetPortNames()
    {
        return SerialPort.GetPortNames();
    }


    public bool OpenPort(PortParameters param)
    {
        try
        {
            _serialPort = new SerialPort
            {
                PortName = param.portName,
                BaudRate = param.baudRate,
                DataBits = param.dataBits,
                Parity = param.parity,
                StopBits = param.stopBits
            };

            // Dodajemy obsługę zdarzenia DataReceived
            _serialPort.DataReceived += SerialPort_DataReceived;

            _serialPort.Open();
            Console.WriteLine("Połączono z portem " + param.portName);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return false;
        }
    }
    public void ChangeBaudRate(int newValue)
    {
        _serialPort.BaudRate = newValue;
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

}