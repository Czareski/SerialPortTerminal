using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO.Ports;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SzeregowaAvalonia.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private PortParameters portParameters;
    private SerialPortManager serialPortManager;
    private bool isConnected = false;

    [ObservableProperty]
    private string _connectButtonText = "Connect";
    [ObservableProperty]
    public string _portNameInfo = "COM: ";
    [ObservableProperty]
    public string _baudRateInfo = "Baud rate: "; 
    [ObservableProperty]
    public string _dataBitsInfo = "Data bits: ";
    [ObservableProperty]
    public string _stopBitsInfo = "Stop bits: ";
    [ObservableProperty]
    public string _parityInfo = "Parity: ";
    [ObservableProperty]
    public string _connectionStatusColor = "#ff0000";
    [ObservableProperty]
    private string _receivedText;

    public ObservableCollection<string> ComPorts { get; } = new ObservableCollection<string>();
    public string SelectedPort { get; set; }

    public IRelayCommand<string> BaudRateCommand { get; }
    public IRelayCommand<string> StopBitsCommand { get; }
    public IRelayCommand<string> ParityCommand { get; }
    public IRelayCommand<string> DataBitsCommand { get; }
    public IRelayCommand ConnectCommand { get; }
    public IRelayCommand ScanPortsCommand { get; }
    public MainViewModel()
    {
        BaudRateCommand = new RelayCommand<string>(BaudRate_Changed);
        StopBitsCommand = new RelayCommand<string>(StopBits_Changed);
        ParityCommand = new RelayCommand<string>(Parity_Changed);
        DataBitsCommand = new RelayCommand<string>(DataBits_Changed);
        ConnectCommand = new RelayCommand(ConnectWithSerialPort);
        ScanPortsCommand = new RelayCommand(ScanPorts);

        
        foreach (string com in SerialPortManager.Instance.GetPortNames())
        {
            ComPorts.Add(com);
        }
    }
    public void StopBits_Changed(string value)
    {
        switch (value)
        {
            case "1":
                portParameters.stopBits = StopBits.One;
                break;
            case "2":
                portParameters.stopBits = StopBits.Two;
                break;
            case "1.5":
                portParameters.stopBits = StopBits.OnePointFive;
                break;
        }

        
    }

    public void Parity_Changed(string value)
    {
        if (Enum.TryParse<Parity>(value, out Parity parity))
        {
            portParameters.parity = parity;
            
        }
    }

    public void DataBits_Changed(string value)
    {
        if (int.TryParse(value, out int bits))
        {
            portParameters.dataBits = bits;
            
        }
    }
    public void BaudRate_Changed(string content)
    {
        portParameters.baudRate = Convert.ToInt32(content);
        if (isConnected)
        {
            SerialPortManager.Instance.ChangeBaudRate(portParameters.baudRate);
            BaudRateInfo = "Baud rate: " + portParameters.baudRate;
        }
        
    }
    public void ConnectWithSerialPort()
    {
        if (isConnected)
        {
            SerialPortManager.Instance.ClosePort();
            ConnectButtonText = "Connect";
            ConnectionStatusColor = "#ff0000";
            isConnected = false;
            return;
        }

        portParameters.portName = SelectedPort;
        bool isSerialPortOpened = SerialPortManager.Instance.OpenPort(portParameters);
        if (isSerialPortOpened)
        {
            ConnectionStatusColor = "#00ff00";
            PortNameInfo = "COM: " + portParameters.portName;
            DataBitsInfo = "Data bits: " + portParameters.dataBits;
            BaudRateInfo = "Baud rate: " + portParameters.baudRate;
            ParityInfo = "Parity: " + portParameters.parity;
            StopBitsInfo = "Stop bits: " + portParameters.stopBits;
            
            ConnectButtonText = "Disconnect";
                isConnected = true;
        }
    }
    public void ScanPorts()
    {
        ComPorts.Clear();
        foreach (string com in SerialPortManager.Instance.GetPortNames())
        {
            ComPorts.Add(com);
        }
    }




}



