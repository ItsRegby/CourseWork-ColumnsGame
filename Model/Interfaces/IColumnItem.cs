using ColumnsGame.Enums;

namespace ColumnsGame.Model
{
    public interface IColumnItem
    {
        //Coord
        public int Row { get; set; }
        public int Column { get; set; }
        //Colors
        public ItemColor TopColor { get; set; }
        public ItemColor MiddleColor { get; set; }
        public ItemColor BottonColor { get; set; }
        public void SwapColorDown();
        public void SwapColorUP();    
        
    }
}
