using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzeregowaAvalonia.Model
{
    public record Macro {
        public string Title { get; set; }
        public string Command { get; set; }
        public Macro(string title, string command)
        {
            Title = title;
            Command = command;
        }
    }
}
