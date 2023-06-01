namespace ColumnsGame.Model.Interfaces
{
    public interface IRotatableColumnItem : IColumnItem
    {
        public bool IsHorizontal { get; set; }
        public void Rotate();
        public void UnRotate();
    }
}
