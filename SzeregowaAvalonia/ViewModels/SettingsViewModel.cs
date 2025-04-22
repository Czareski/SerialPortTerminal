using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO.Ports;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SzeregowaAvalonia.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SzeregowaAvalonia.Model;

namespace SzeregowaAvalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private ErrorHandler _errorHandler;
    private SerialPort _serialPort;

    [ObservableProperty]
    private string _connectButtonText = "Connect";
    [ObservableProperty]
    public string _portNameInfo = "COM: not selected";
    [ObservableProperty]
    public string _baudRateInfo = "Baud rate: not selected"; 
    [ObservableProperty]
    public string _dataBitsInfo = "Data bits: not selected";
    [ObservableProperty]
    public string _stopBitsInfo = "Stop bits: not selected";
    [ObservableProperty]
    public string _parityInfo = "Parity: not selected";
    [ObservableProperty]
    public string _connectionStatusColor = "#E02923";
    [ObservableProperty]
    private bool _isCustomInputEnabled;
    [ObservableProperty]
    private string _customBaudRate = "";
    [ObservableProperty]
    private string _selectedPort = "";
    public ObservableCollection<string> ComPorts { get; } = new ObservableCollection<string>();

    public SettingsViewModel(ErrorHandler errorHandler, SerialPort serialPort)
    {
        System.Diagnostics.Debug.WriteLine("SettingsViewModel constructor called");
        _errorHandler = errorHandler;
        _serialPort = serialPort;


        ScanPorts();

        if (ComPorts.Count > 0)
        {
            SelectedPort = ComPorts.First();
        }
    }

    [RelayCommand]
    public void ChangeStopBits(string value)
    {
        _serialPort.StopBits = value switch
        {
            "1" => StopBits.One,
            "2" => StopBits.Two,
            "1.5" => StopBits.OnePointFive,
            _ => throw new NotImplementedException("Niepoprawny typ")
        };
    }
    [RelayCommand]
    public void ChangeParity(string value)
    {
        if (Enum.TryParse(value, out Parity parity))
        {
            _serialPort.Parity = parity;
        }
        else
        {
            throw new NotImplementedException("Niepoprawny typ");
        }
    }
    [RelayCommand]
    public void ChangeDataBits(string value)
    {
        if (int.TryParse(value, out int dataBits))
        {
            _serialPort.DataBits = dataBits;
        }
        else
        {
            throw new NotImplementedException("Niepoprawny typ");
        }
    }
    [RelayCommand]
    public void ChangeBaudRate(string value)
    {
        if (value.Equals("custom")) {
            IsCustomInputEnabled = true;
            return;
        }

        IsCustomInputEnabled = false;
        if (int.TryParse(value, out int baudRate))
        {
            _serialPort.BaudRate = baudRate;
        }
        else
        {
            throw new NotImplementedException("Niepoprawny typ");
        }
        if (_serialPort.IsOpen)
        {
            BaudRateInfo = "Baud rate: " + value;
        }
        
    }
    [RelayCommand]
    public void ToggleConnectionState()
    {
        if (_serialPort.IsOpen)
        {
            _serialPort.Close();
            SetConnectionDisplay();
            return;
        }

        _serialPort.PortName = SelectedPort;
        if (IsCustomInputEnabled)
        {
            if (CustomBaudRate != null)
         
                if (int.TryParse(CustomBaudRate, out int baudRate))
                {
                    _serialPort.BaudRate = baudRate;
                }
                else
                {
                    throw new NotImplementedException("Niepoprawny typ");

                }
        }
        
        _serialPort.Open();
        
        if (_serialPort.IsOpen)
        {
            UpdateConnectionInfo();
            SetConnectionDisplay();
        }
    }
    
    [RelayCommand]
    public void ScanPorts()
    {
        ComPorts.Clear();
        foreach (string com in SerialPort.GetPortNames())
        {
            ComPorts.Add(com);
        }
    }
    private void UpdateConnectionInfo()
    {
        PortNameInfo = "COM: " + _serialPort.PortName;
        DataBitsInfo = "Data bits: " + _serialPort.DataBits;
        BaudRateInfo = "Baud rate: " + _serialPort.BaudRate;
        ParityInfo = "Parity: " + _serialPort.Parity;
        StopBitsInfo = "Stop bits: " + _serialPort.StopBits;
    }
    private void SetConnectionDisplay()
    {
        ConnectButtonText = _serialPort.IsOpen ? "Disconnect" : "Connect";
        ConnectionStatusColor = _serialPort.IsOpen ? "#38CA53" : "#E02923";
    }

    


}



