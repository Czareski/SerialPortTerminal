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
using System.IO;

namespace SzeregowaAvalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private ErrorHandler _errorHandler;
    private SerialPort _serialPort;

    [ObservableProperty]
    private string _connectButtonText = "Połącz";

    // wszystkie wartości -Info są wyświetlane w górnym pasku jako informacje o połączeniu
    [ObservableProperty]
    public string _portNameInfo = "Port: nie wybrano";
    [ObservableProperty]
    public string _baudRateInfo = "Prędkość transmisji: nie wybrano"; 
    [ObservableProperty]
    public string _dataBitsInfo = "Bity danych: nie wybrano";
    [ObservableProperty]
    public string _stopBitsInfo = "Bity stopu: nie wybrano";
    [ObservableProperty]
    public string _parityInfo = "Parzystość: nie wybrano";
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
        _errorHandler = errorHandler;
        _serialPort = serialPort;

        // ustanowienie domyślnych wartości
        _serialPort.BaudRate = 9600;
        _serialPort.DataBits = 8;
        _serialPort.Parity = Parity.None;
        _serialPort.StopBits = StopBits.One;

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
            // default jest nieosiągalny
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
            // nieosiągalne
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
            // nieosiągalne
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
            // nieosiągalne
        }
        if (_serialPort.IsOpen)
        {
            BaudRateInfo = "Prędkość: " + value;
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
        try { 
            _serialPort.PortName = SelectedPort;
        } catch (ArgumentNullException ex)
        {
            _errorHandler.ReportError("Należy wybrać port");
            return;
        }
        if (IsCustomInputEnabled)
        {
            bool result = SetCustomBaudRate();
            if (!result) return;
        }
        try
        {
            _serialPort.Open();
        } catch (UnauthorizedAccessException ex) 
        {
            _errorHandler.ReportError("Port " + _selectedPort + " jest już zajęty");
            return;
        } catch (IOException ex)
        {
            _errorHandler.ReportError("Port " + _selectedPort + " został nie znaleziony");
            return;
        }
        
        if (_serialPort.IsOpen)
        {
            UpdateConnectionInfo();
            SetConnectionDisplay();
        }
    }
    public bool SetCustomBaudRate()
    {
        if (CustomBaudRate != null)
        {
            if (int.TryParse(CustomBaudRate, out int baudRate))
            {
                _serialPort.BaudRate = baudRate;
            }
            else
            {
                _errorHandler.ReportError("Podano nieprawidłowy format prędkości danych");
                return false;
            }
        } else
        {
            _errorHandler.ReportError("Podano nieprawidłowy format prędkości danych");
            return false;
        }
        return true;
    }

    [RelayCommand]
    public void ScanPorts()
    {
        ComPorts.Clear();
        foreach (string com in SerialPort.GetPortNames())
        {
            ComPorts.Add(com);
        }
        if (ComPorts.Count == 0)
        {
            _errorHandler.ReportError("Nie znaleziono żadnego portu");
        }
    }
    private void UpdateConnectionInfo()
    {
        PortNameInfo = "Port: " + _serialPort.PortName;
        DataBitsInfo = "Bity danych: " + _serialPort.DataBits;
        BaudRateInfo = "Prędkość transmisji: " + _serialPort.BaudRate;
        ParityInfo = "Parzystość: " + _serialPort.Parity;
        double stopBitsToNumber = _serialPort.StopBits switch
        {
            StopBits.One => 1,
            StopBits.Two => 2,
            StopBits.OnePointFive => 1.5,
            _ => throw new NotImplementedException("Niepoprawny typ")
            // default jest nieosiągalny
        };
        StopBitsInfo = "Bity stopu: " + stopBitsToNumber;
    }
    
    private void SetConnectionDisplay()
    {
        ConnectButtonText = _serialPort.IsOpen ? "Rozłącz" : "Połącz";
        ConnectionStatusColor = _serialPort.IsOpen ? "#38CA53" : "#E02923";
    }

    


}



