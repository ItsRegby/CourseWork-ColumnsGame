using ColumnsGame.Enums;
using ColumnsGame.Model.Interfaces;
using System;

namespace ColumnsGame.Model
{
    public class RotatableColumnItem : ColumnItem, IRotatableColumnItem
    {
        public bool IsHorizontal { get; set; }
        public RotatableColumnItem(int startRow, int startColumn, int colorCount)
            : base(startRow, startColumn, colorCount)
        {
            IsHorizontal = false;
        }
        // 0001 - north
        // 0010 - west
        // 0100 - south
        // 1000 - east
        private int position = 1;
        private readonly int NORTH = 1;
        private readonly int WEST = 1 << 1;
        private readonly int SOUTH = 1 << 2;
        private readonly int EAST = 1 << 3;
        public void UnRotate()
        {
            if (IsHorizontal)
            { 
                // Change to vertical position
                IsHorizontal = false;
                Row--;
            }
            else
            {
                // Change to horizontal position
                IsHorizontal = true;
                Row++;
            }
        }
        public void Rotate()
        {
            if (IsHorizontal)
            {
                // Change to vertical position
                IsHorizontal = false;
                Row--;

                if ((position & WEST) == WEST)
                {
                    ItemColor temp = TopColor;
                    TopColor = BottonColor;
                    BottonColor = temp;
                    position = SOUTH;
                }
                else
                {
                    ItemColor temp = TopColor;
                    TopColor = BottonColor;
                    BottonColor = temp;
                    position = NORTH;
                }
            }
            else
            {
                // Change to horizontal position
                IsHorizontal = true;
                Row++;

                if((position & SOUTH) == SOUTH)
                {                   
                    position = EAST;
                } else
                {
                    position = WEST;
                }
            }
        }

        public override string ToString()
        {
            return "Rotatable";
        }
    }
}
