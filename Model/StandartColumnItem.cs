namespace ColumnsGame.Model
{
    public class StandardColumnItem : ColumnItem
    {
        public StandardColumnItem(int startRow, int startColumn, int colorCount)
            : base(startRow, startColumn, colorCount)
        {
            
        }

        public override string ToString()
        {
            return "Standard";
        }
    }
}
