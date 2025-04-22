using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using SzeregowaAvalonia.ViewModels;

namespace SzeregowaAvalonia.Model
{
    public class TerminalOutput : IDataReciever
    {
        private double CHARACTER_PER_WIDTH = 8.79;
        private const int MAX_LINES = 1000;
        private double _lineWidth;
        private int _lineAlternateIndex = 0;
        private string _searchedPhrase = "";
        private List<RecieveLine> searchedLines = [];
        private int currentSearchIndex = 0;
        int allLines = 0;
        public ScrollLogic Scroll { get; } = new();
        public ObservableCollection<RecieveLine> Lines { get; } = [new RecieveLine(Brushes.Black)];
        private EncodingType _encoding = EncodingType.ASCII;

        public void UpdateLineWidth(double value)
        {
            _lineWidth = Convert.ToInt32(value / CHARACTER_PER_WIDTH) - 1;
        }
        public void RecieveData(byte data) {
            if (data == 0x0D)
            {
                AddNewLine();
                return;
            }
            if (data == 0x0A)
            {
                return;
            }

            Lines[Lines.Count - 1].Text += EncodeData(data);

            if (Lines[Lines.Count - 1].Text.Length >= _lineWidth)
            {
                AddNewLine();
            }
        }

        public void AddNewLine()
        {
            allLines += 1;
            _lineAlternateIndex += 1;
            if (Lines.Count == MAX_LINES)
            {
                Lines.RemoveAt(0);
            }
            if (_lineAlternateIndex == 2) _lineAlternateIndex = 0;

            IBrush background = _lineAlternateIndex % 2 == 0 ? Brushes.Black : Brush.Parse("#0f0f0f");
            RecieveLine newLine = new RecieveLine(background);
            
            Lines.Add(newLine);
            newLine.Text = allLines + "> ";
            SearchInLine(newLine);

            if (Scroll.autoScroll)
            {
                Scroll.ScrollToLine(Lines.Count - 1);
            }
        }
        public void Clear()
        {
            Lines.Clear();
            Lines.Add(new RecieveLine(Brushes.Black));
            Scroll.ScrollToTop();
        }
        public void Search(string input)
        {
            ClearSearched();
            if (input == "") return;
            _searchedPhrase = input;
            foreach (RecieveLine line in Lines)
            {
                SearchInLine(line);
            }
            if (searchedLines.Count > 0)
            {
                // ??
                Scroll.ScrollToLine(Lines.IndexOf(searchedLines[0]));
                Scroll.autoScroll = false;
            }
        }
        private void SearchInLine(RecieveLine line)
        {
            if (_searchedPhrase == "") return;
            int foundedIndex = line.Text.IndexOf(_searchedPhrase);
            if (foundedIndex > -1)
            {
                searchedLines.Add(line);
                line.MarkGreen();
            }
        }
        private void ClearSearched()
        {
            foreach (RecieveLine line in searchedLines)
            {
                if (line != null)
                {
                    line.ResetBackground();
                }
            }
            searchedLines.Clear();
            currentSearchIndex = 0;
        }
        public void SearchForNextOccurance()
        {
            if (searchedLines.Count == 0) return;
            currentSearchIndex += 1;
            if (currentSearchIndex >= searchedLines.Count)
            {
                currentSearchIndex = 0;
            }
            // ???
            Scroll.ScrollToLine(Lines.IndexOf(searchedLines[currentSearchIndex]));
        }

        public string EncodeData(byte data)
        {
            string encodedData = "";
            if (_encoding == EncodingType.HEX)
            {
                encodedData = Convert.ToHexString(new byte[] { data }) + " ";
            }
            else
            {
                encodedData += (char)data;
            }
            return encodedData;
        }

        public void SetEncoding(EncodingType encoding)
        {
            _encoding = encoding;
        }
    }
}
