using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SzeregowaAvalonia.ViewModels
{
    public partial class ScrollLogic : ObservableObject
    {
        private const int LINE_HEIGHT = 15;

        public bool autoScroll { get; set; } = true;

        [ObservableProperty]
        private Vector _offset;
        
        public ScrollLogic()
        {
            Offset = new Vector(0, 0);
        }
        public void ToggleAutoscroll()
        {
            autoScroll = !autoScroll;
        }
        public void ScrollToLine(int lineIndex)
        {
            Offset = new Vector(0, lineIndex * LINE_HEIGHT);
        }
        public void ScrollToTop()
        {
            Offset = new Vector(0, 0);
        }

    }
}
