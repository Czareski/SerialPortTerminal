using CommunityToolkit.Mvvm.ComponentModel;

namespace SzeregowaAvalonia.Model
{
    public partial class Macro : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _command;

        public Macro(string title, string command)
        {
            _title = title;
            _command = command;
        }
    }
}
