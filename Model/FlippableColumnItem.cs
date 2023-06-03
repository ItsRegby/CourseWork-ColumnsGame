using ColumnsGame.Model.Interfaces;

namespace ColumnsGame.Model
{
    public class FlippableColumnItem : ColumnItem, IFlippableColumnItem
    {
        public FlippableColumnItem(int startRow, int startColumn, int colorCount)
            : base(startRow, startColumn, colorCount)
        {

        }

        public override void FlipColors()
        {
            var temp = TopColor;
            TopColor = BottonColor;
            BottonColor = temp;
        }
        public override void Rotate()
        {
            // No rotation needed for FlippableColumnItem
        }

        public override void UnRotate()
        {
            // No rotation needed for FlippableColumnItem
        }

        public override string ToString()
        {
            return "Flippable";
        }
    }
}
