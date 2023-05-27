using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ColumnsGame.Enums;
using ColumnsGame.Model.Interfaces;

namespace ColumnsGame.Model
{
    // Storage for item colors
    public class StorageItems
    {
        private readonly ConcurrentDictionary<int, ItemColor> _cells = new ConcurrentDictionary<int, ItemColor>();
        private const int columnMultiple = 10000;

        // Convert dictionary key to row index
        public static int GetRowFromKey(int key)
        {
            return key % columnMultiple;
        }

        // Convert dictionary key to column index
        public static int GetColumnFromKey(int key)
        {
            return key / columnMultiple;
        }

        // Get reference to items dictionary
        public ConcurrentDictionary<int, ItemColor> GetCells()
        {
            return _cells;
        }

        // Clear storage
        public void Clear()
        {
            _cells.Clear();
        }


        // Determines whether the storage contains item
        public bool Contains(int row, int column, ColumnItem columnItem)
        {
            if (columnItem is RotatableColumnItem rotatebleColum && rotatebleColum.IsHorizontal)
            {
                bool middle = _cells.ContainsKey(column * columnMultiple + row);
                bool right = _cells.ContainsKey((column - 1) * columnMultiple + row);
                bool left = _cells.ContainsKey((column + 1) * columnMultiple + row);
                               
                return middle || right || left;
            }

            return _cells.ContainsKey(column * columnMultiple + row);
        }


        // Add column item to storage
        public void Add(ColumnItem item)
        {
            if (item is RotatableColumnItem rotatableItem && rotatableItem.IsHorizontal)
            {
                _cells.TryAdd(((rotatableItem.Column - 1) * columnMultiple) + rotatableItem.Row, item.TopColor);
                _cells.TryAdd((rotatableItem.Column * columnMultiple) + rotatableItem.Row, item.MiddleColor);
                _cells.TryAdd(((rotatableItem.Column + 1) * columnMultiple) + rotatableItem.Row, item.BottonColor);
            }
            else
            {
                _cells.TryAdd(item.Column * columnMultiple + item.Row, item.BottonColor);
                _cells.TryAdd(item.Column * columnMultiple + item.Row + 1, item.MiddleColor);
                _cells.TryAdd(item.Column * columnMultiple + item.Row + 2, item.TopColor);
            }
        }


        //  Add one item to storage
        public void AddItem(int row, int column, ItemColor color)
        {
            _cells.TryAdd(column * columnMultiple + row, color);
        }

        // Delete items from storage
        public void Remove(Dictionary<int, ItemColor> items)
        {
            foreach (var item in items)
            {
                _cells.TryRemove(item.Key, out _);
            }
        }

        // Find and get all item for delete
        public Dictionary<int, ItemColor> GetItemsForDelete()
        {
            Dictionary<int, ItemColor> deleteItems = new Dictionary<int, ItemColor>();

            ItemColor color1;
            ItemColor color2;

            // find all item to delete
            foreach (var item in _cells)
            {

                // horizontal
                if (_cells.TryGetValue(item.Key + 1, out color1) && _cells.TryGetValue(item.Key - 1, out color2))
                {
                    if (color1 == color2 && color2 == item.Value)
                    {
                        deleteItems.TryAdd(item.Key, item.Value);
                        deleteItems.TryAdd(item.Key + 1, item.Value);
                        deleteItems.TryAdd(item.Key - 1, item.Value);
                    }
                }

                // vertical
                if (_cells.TryGetValue(item.Key + columnMultiple, out color1) && _cells.TryGetValue(item.Key - columnMultiple, out color2))
                {
                    if (color1 == color2 && color2 == item.Value)
                    {
                        deleteItems.TryAdd(item.Key, item.Value);
                        deleteItems.TryAdd(item.Key + columnMultiple, item.Value);
                        deleteItems.TryAdd(item.Key - columnMultiple, item.Value);
                    }
                }

                // diagonal 1
                if (_cells.TryGetValue(item.Key + columnMultiple + 1, out color1) && _cells.TryGetValue(item.Key - columnMultiple - 1, out color2))
                {
                    if (color1 == color2 && color2 == item.Value)
                    {
                        deleteItems.TryAdd(item.Key, item.Value);
                        deleteItems.TryAdd(item.Key + columnMultiple + 1, item.Value);
                        deleteItems.TryAdd(item.Key - columnMultiple - 1, item.Value);
                    }
                }

                // diagonal 2
                if (_cells.TryGetValue(item.Key + columnMultiple - 1, out color1) && _cells.TryGetValue(item.Key - columnMultiple + 1, out color2))
                {
                    if (color1 == color2 && color2 == item.Value)
                    {
                        deleteItems.TryAdd(item.Key, item.Value);
                        deleteItems.TryAdd(item.Key + columnMultiple - 1, item.Value);
                        deleteItems.TryAdd(item.Key - columnMultiple + 1, item.Value);
                    }
                }
            }

            return deleteItems;
        }


        // Fall down items in storage
        public void FallDownItems()
        {
            var change = true;
            var listFallDown = new List<int>();

            while (change)
            {
                change = false;
                listFallDown.Clear();

                foreach (var item in _cells.OrderBy(a => a.Key).ToList())
                {
                    // is not bottom && not exit item row - 1 -> add to list
                    if (item.Key % columnMultiple > 0 && !_cells.ContainsKey(item.Key - 1))
                        listFallDown.Add(item.Key);
                }

                foreach (var item in listFallDown)
                {
                    change = true;
                    if (_cells.TryRemove(item, out ItemColor valColor))
                    {
                        _cells.TryAdd(item - 1, valColor);
                    }
                }
            }
        }


        // Set items transparent color, by input key value
        public void SetItemsTransparentColor(Dictionary<int, ItemColor> items)
        {
            foreach (var item in items)
                _cells.TryUpdate(item.Key, ItemColor.Transparent, item.Value);
        }

        // Set items color, by input key value
        public void SetItemsColor(Dictionary<int, ItemColor> items)
        {
            foreach (var item in items)
                _cells.TryUpdate(item.Key, item.Value, ItemColor.Transparent);
        }
    }
}
