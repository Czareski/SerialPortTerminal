﻿using System;
using System.IO.Ports;

namespace SzeregowaAvalonia.Model
{
    public class CommandProcessor
    {
        private SerialPort _serialPort;
        public CommandProcessor(SerialPort serialPort)
        {
            _serialPort = serialPort;
        }
        public void SendText(string text, LineEnding ending)
        {
            AddLineEnding(ref text, ending);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '$')
                {
                    if (i > 0 && text[i - 1] == '$')
                    {
                        _serialPort.Write("$");
                    }
                    if (i < text.Length - 2)
                    {

                        byte? hex = GetHexVariable(text.Substring(i + 1, 2));
                        if (hex == null)
                        {
                            continue;
                        }
                        else
                        {
                            _serialPort.Write("" + (char)hex);
                            i += 2;
                        }
                    }
                }
                else
                {
                    _serialPort.Write("" + text[i]);
                }
            }
        }
        public byte? GetHexVariable(string text)
        {
            if (text[0] == '$')
            {
                return null;
            }
            if (text.Length == 2)
            {
                try
                {
                    byte converted = Convert.ToByte(text, 16);
                    return converted;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private void AddLineEnding(ref string text, LineEnding ending)
        {
            switch (ending)
            {
                case LineEnding.CR:
                    text += "\r";
                    break;
                case LineEnding.NL:
                    text += "\n";
                    break;
                case LineEnding.CRNL:
                    text += "\r\n";
                    break;
                case LineEnding.NLCR:
                    text += "\n\r";
                    break;
            }
        }
    }
    public enum LineEnding
    {
        CR = 1,
        NL = 2,
        NLCR = 3,
        CRNL = 4,
        None = 5
    }
}
