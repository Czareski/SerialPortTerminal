using System.ComponentModel;
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
        private IBrush _foreground = Brushes.White;
        public IBrush Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    OnPropertyChanged(nameof(Foreground));
                }
            }
        }

        public RecieveLine(IBrush background, IBrush foreground)
        {
            _originalBackground = background;
            Background = background;
            Foreground = foreground;
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
