using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SzeregowaAvalonia.Model
{
    public class CommandProcessor
    {
        public CommandProcessor()
        {

        }
        public void SendText(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '$')
                {
                    if (i > 0 && text[i - 1] == '$')
                    {
                        SerialPortManager.Instance.SendMessage("$");
                    }
                    if (i < text.Length - 2) {
                        
                        byte? hex = GetHexVariable(text.Substring(i + 1, 2));
                        if (hex == null)
                        {
                            continue;
                        }
                        else
                        {
                            SerialPortManager.Instance.SendMessage((byte)hex);
                            i += 2;
                        }
                    }
                } else
                {
                    SerialPortManager.Instance.SendMessage(text[i]);
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
                } catch (Exception ex)
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
    }
}
