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
    private ErrorHandler _errorService;
    private bool isConnected = false;
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
    private string? _customBaudRate;
    [ObservableProperty]
    private string _selectedPort;
    public ObservableCollection<string> ComPorts { get; } = new ObservableCollection<string>();

    public SettingsViewModel(ErrorHandler errorService)
    {
        System.Diagnostics.Debug.WriteLine("SettingsViewModel constructor called");
        _errorService = errorService;
        
        if (SerialPortHandler.Instance == null)
        {
            new SerialPortHandler();
        }
        
        foreach (string com in SerialPortHandler.Instance.GetPortNames())
        {
            ComPorts.Add(com);
        }

        if (ComPorts.Count > 0)
        {
            SelectedPort = ComPorts.First();
        } else
        {
            SelectedPort = "No Selected Port";
            
        }
    }

    [RelayCommand]
    public void ChangeStopBits(string value)
    {
        SerialPortHandler.Instance.SetStopBits(value);
    }
    [RelayCommand]
    public void ChangeParity(string value)
    {
        SerialPortHandler.Instance.SetParity(value);
    }
    [RelayCommand]
    public void ChangeDataBits(string value)
    {
        SerialPortHandler.Instance.SetDataBits(value);
    }
    [RelayCommand]
    public void ChangeBaudRate(string value)
    {
        if (value.Equals("custom")) {
            IsCustomInputEnabled = true;
            return;
        }

        IsCustomInputEnabled = false;
        bool result = SerialPortHandler.Instance.SetBaudRate(value);
        if (result && isConnected)
        {
            BaudRateInfo = "Baud rate: " + value;
        }
        
    }
    [RelayCommand]
    public void ToggleConnectionState()
    {
        if (isConnected)
        {
            SerialPortHandler.Instance.ClosePort();
            isConnected = false;
            SetConnectionDisplay();
            return;
        }

        SerialPortHandler.Instance.SetPortName(SelectedPort);
        if (IsCustomInputEnabled)
        {
            SerialPortHandler.Instance.SetBaudRate(CustomBaudRate);
        }
        
        bool isSerialPortOpened = SerialPortHandler.Instance.OpenPort();
        
        if (isSerialPortOpened)
        {
            PortNameInfo = "COM: " + SerialPortHandler.Instance.GetPortName();
            DataBitsInfo = "Data bits: " + SerialPortHandler.Instance.GetDataBits();
            BaudRateInfo = "Baud rate: " + SerialPortHandler.Instance.GetBaudRate();
            ParityInfo = "Parity: " + SerialPortHandler.Instance.GetParity();
            StopBitsInfo = "Stop bits: " + SerialPortHandler.Instance.GetStopBits();

            isConnected = true;
            SetConnectionDisplay();
        }
    }
    
    [RelayCommand]
    public void ScanPorts()
    {
        System.Diagnostics.Debug.WriteLine("ScanPorts called");
        ComPorts.Clear();
        foreach (string com in SerialPortHandler.Instance.GetPortNames())
        {
            ComPorts.Add(com);
        }
    }
    private void SetConnectionDisplay()
    {
        ConnectButtonText = isConnected ? "Disconnect" : "Connect";
        ConnectionStatusColor = isConnected ? "#38CA53" : "#E02923";
    }

    


}



