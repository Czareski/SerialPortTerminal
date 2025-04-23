using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private EncodingType _encoding = EncodingType.ASCII;
        public ScrollLogic Scroll { get; } = new();
        public ObservableCollection<RecieveLine> Lines { get; } = [];

        public void UpdateLineWidth(double value)
        {
            _lineWidth = Convert.ToInt32(value / CHARACTER_PER_WIDTH) - 1;
        }
        public void RecieveData(object sender, byte data) {
            if (data == 0x0D)
            {
                AddNewLine();
                return;
            }
            if (data == 0x0A)
            {
                return;
            }
            if (Lines.Count == 0)
            {
                AddNewLine();
            }
            Lines[Lines.Count - 1].Text += EncodeData(data);

            if (Lines[Lines.Count - 1].Text.Length >= _lineWidth)
            {
                AddNewLine();
            }
        }

        public void AddNewLine()
        {
            _lineAlternateIndex += 1;
            if (Lines.Count == MAX_LINES)
            {
                Lines.RemoveAt(0);
            }
            if (_lineAlternateIndex == 2) _lineAlternateIndex = 0;

            IBrush background = _lineAlternateIndex % 2 == 0 ? Brushes.Black : Brush.Parse("#0f0f0f");
            IBrush foreground = _encoding == EncodingType.ASCII ? Brushes.White : Brushes.Wheat;
            RecieveLine newLine = new RecieveLine(background, foreground);
            
            Lines.Add(newLine);
            SearchInLine(newLine);

            if (Scroll.AutoScroll)
            {
                Scroll.ScrollToLine(Lines.Count - 1);
            }
        }
        public void Clear()
        {
            Lines.Clear();
            
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
                Scroll.ScrollToLine(Lines.IndexOf(searchedLines[0]));
                Scroll.AutoScroll = false;
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
        public bool SearchForNextOccurance()
        {
            if (searchedLines.Count == 0) return false;
            currentSearchIndex += 1;
            if (currentSearchIndex >= searchedLines.Count)
            {
                currentSearchIndex = 0;
            }
            
            Scroll.ScrollToLine(Lines.IndexOf(searchedLines[currentSearchIndex]));
            return true;
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
