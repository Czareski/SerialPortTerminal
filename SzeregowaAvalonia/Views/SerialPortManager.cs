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

namespace SzeregowaAvalonia.Views;
class SerialPortManager
{
    private SerialPort _serialPort;
    private ListBox dataRecievedList;


    public SerialPortManager(ListBox _dataRecievedList)
    {
        dataRecievedList = _dataRecievedList;

    }

    public string[] GetPortNames()
    {
        return SerialPort.GetPortNames();
    }


    public bool OpenPort(PortParameters param)
    {
        dataRecievedList.Items.Add(param.portName);
        dataRecievedList.Items.Add(param.baudRate);
        dataRecievedList.Items.Add(param.dataBits);
        dataRecievedList.Items.Add(param.parity);
        dataRecievedList.Items.Add(param.stopBits);

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
            string indata = sp.ReadLine();

            // Wyświetlamy otrzymane dane wraz z czasem
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                MainViewModel.Instance.AppendReceivedLine(indata);
            });
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
    public void SendMessage(string message)
    {
        _serialPort.Write(message);
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