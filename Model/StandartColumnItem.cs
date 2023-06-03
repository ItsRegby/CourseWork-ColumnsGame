namespace ColumnsGame.Model
{
    public class StandardColumnItem : ColumnItem
    {
        public StandardColumnItem(int startRow, int startColumn, int colorCount)
            : base(startRow, startColumn, colorCount)
        {
            
        }
        public override void FlipColors()
        {
            // No color flipping needed for StandardColumnItem
        }

        public override void Rotate()
        {
            // No rotation needed for StandardColumnItem
        }

        public override void UnRotate()
        {
            // No rotation needed for StandardColumnItem
        }

        public override string ToString()
        {
            return "Standard";
        }
    }
}
