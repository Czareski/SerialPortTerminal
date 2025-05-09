﻿using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.Serialization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SzeregowaAvalonia.ViewModels;

namespace SzeregowaAvalonia.Views;

public partial class RecieveView : UserControl
{

    public RecieveView()
    {
        InitializeComponent();
    }
    private void OnLayoutUpdated(object? sender, EventArgs e)
    {
        if (DataContext is RecieveViewModel vm && sender is Control control)
        {
            vm.DataReciever.Terminal.UpdateLineWidth(control.Bounds.Width);
        }
    }
    private void SearchFieldUpdated(object? sender, TextChangedEventArgs e)
    {
        
        if (DataContext is RecieveViewModel vm && sender is Control control)
        {
            string input = ((TextBox)sender).Text;
            vm.DataReciever.Terminal.Search(input);
        }
    }

}
