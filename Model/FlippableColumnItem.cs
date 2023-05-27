using ColumnsGame.Model.Interfaces;

namespace ColumnsGame.Model
{
    public class FlippableColumnItem : ColumnItem, IFlippableColumnItem
    {
        public FlippableColumnItem(int startRow, int startColumn, int colorCount)
            : base(startRow, startColumn, colorCount)
        {

        }

        public void FlipColors()
        {
            var temp = TopColor;
            TopColor = BottonColor;
            BottonColor = temp;
        }

        public override string ToString()
        {
            return "Flippable";
        }
    }
}
