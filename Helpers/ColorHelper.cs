using ColumnsGame.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ColumnsGame.Helpers
{
    public static class ColorHelper
    {
        private static readonly Random _rnd = new Random();

        // Background board
        public static SolidColorBrush BackgroundBoard { get; set; } = new SolidColorBrush(Colors.LightGray);

        // Background desk
        public static SolidColorBrush BackgroundDesk { get; set; } = new SolidColorBrush(Colors.White);

        // Line
        public static Pen Line { get; set; } = new Pen(new SolidColorBrush(Colors.LightGray), 2);


        static ColorHelper()
        {
            foreach (var color in _colorList)
            {
                color.Freeze();
            }

            BackgroundBoard.Freeze();
            BackgroundDesk.Freeze();
            Line.Freeze();

        }

        // List used colors
        private static readonly List<SolidColorBrush> _colorList = new List<SolidColorBrush>()
                    {
                        new SolidColorBrush(Color.FromRgb(0x24, 0x33, 0x9d)),
                        new SolidColorBrush(Color.FromRgb(0xec, 0x2f, 0x2f)),
                        new SolidColorBrush(Color.FromRgb(0x0f, 0xbc, 0x47)),
                        new SolidColorBrush(Color.FromRgb(0xff, 0xe2, 0x80)),
                        new SolidColorBrush(Color.FromRgb(0x3a, 0xe6, 0xE6)),
                        new SolidColorBrush(Color.FromRgb(0x92, 0x4c, 0x9a)),
                        new SolidColorBrush(Color.FromRgb(0xe0, 0x7a, 0x13)),
                        new SolidColorBrush(Color.FromRgb(0xff, 0x77, 0xa3)),
                        new SolidColorBrush(Color.FromRgb(0x53, 0x19, 0x14)),
                        new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0x00)),
                        new SolidColorBrush(Colors.Transparent)
                    };
  
        
        // Get random color from from a list
        public static ItemColor GetRandomColor(int colorCount)
        {
            return  (ItemColor)_rnd.Next(0, colorCount);
        }


        // Get color by index
        public static SolidColorBrush GetColor(int colorIndex)
        {
            if (colorIndex < 0 || colorIndex >= _colorList.Count)
                throw new ArgumentOutOfRangeException(nameof(colorIndex));

            return _colorList[colorIndex];
        }

    }
}
