using ColumnsGame.Helpers;
using ColumnsGame.Enums;

namespace ColumnsGame.Model
{
    // Represents a falling Itemcolumns 
    public abstract class ColumnItem : IColumnItem
    {
        public ColumnItem(int startRow, int startColumn, int colorCount)
        {
            Row = startRow;
            Column = startColumn;
            TopColor = ColorHelper.GetRandomColor(colorCount);
            MiddleColor = ColorHelper.GetRandomColor(colorCount);
            BottonColor = ColorHelper.GetRandomColor(colorCount);
        }
        // Row index
        public int Row { get; set; }

        // Column index
        public int Column { get; set; }
        public ItemColor TopColor { get; set; }
        public ItemColor MiddleColor { get; set; }
        public ItemColor BottonColor { get; set; }

        // Swap colors
        public void SwapColorDown()
        {
            var pom = TopColor;
            TopColor = BottonColor;
            BottonColor = MiddleColor;
            MiddleColor = pom;
        }
        public void SwapColorUP()
        {
            var pom = TopColor;
            TopColor = MiddleColor;
            MiddleColor = BottonColor;
            BottonColor = pom;
        }
    }
}
