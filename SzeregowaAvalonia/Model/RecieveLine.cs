using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace SzeregowaAvalonia.Model
{
    public class RecieveLine : INotifyPropertyChanged
    {
        private string _text = "";
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }

        private IBrush _originalBackground;

        private IBrush _background = Brushes.Black;
        public IBrush Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged(nameof(Background));
                }
            }
        }

        public RecieveLine(IBrush background)
        {
            _originalBackground = background;
            Background = background;
            Text = string.Empty;
        }
        public void ResetBackground()
        {
            Background = _originalBackground;
        }
        public void MarkGreen()
        {
            Background = Brush.Parse("#38CA53");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
