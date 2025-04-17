using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.Serialization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SzeregowaAvalonia.ViewModels;

namespace SzeregowaAvalonia.Views;

public partial class SendView : UserControl
{

    public SendView()
    {
        InitializeComponent();
        this.DataContext = new SendViewModel();
    }

}
