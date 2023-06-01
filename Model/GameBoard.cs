using ColumnsGame.Controls;
using ColumnsGame.Enums;
using ColumnsGame.Helpers;
using ColumnsGame.Model.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ColumnsGame.Model;

namespace ColumnsGame.Model
{
    public class GameBoard : INotifyPropertyChanged
    {

        public static readonly int MaxRowsCount = 30;
        public static readonly int MinRowsCount = 10;
        public static readonly int DefaultRowsCount = 13;

        public static readonly int MaxColumnsCount = 30;
        public static readonly int MinColumnsCount = 5;
        public static readonly int DefaultColumnsCount = 6;

        public static readonly int MaxColorsCount = 10;
        public static readonly int MinColorsCount = 3;
        public static readonly int DefaultColorsCount = 5;

        MusicPlayer musicPlayer = new MusicPlayer();


        //  The event is invoked when it is necessary to refresh the UI
        public event EventHandler NeedRefreshVisualControl;


        private StorageItems _storageItems;
        private int _rowsCount;
        private int _columnsCount;
        private int _colorsCount;

        // Get or Set rows count board
        public int RowsCount
        {
            get => _rowsCount;
            set
            {
                _rowsCount = value;
                RaisePropertyChanged();
            }
        }

        // Get or Set columns count board
        public int ColumnsCount
        {
            get => _columnsCount;
            set
            {
                _columnsCount = value;
                RaisePropertyChanged();
            }
        }

        // Get or Set colors count board
        public int ColorsCount
        {
            get => _colorsCount;
            set
            {
                _colorsCount = value;
                RaisePropertyChanged();
            }
        }

        public ConcurrentDictionary<int, ItemColor> Cells => _storageItems.GetCells();

        // Get or Set whether an animation is showed
        public bool ShowAnimation { get; set; }

        // Get or Set game score
        public int Score { get; set; }


        // Get or Set falling column item
        public IColumnItem ColumnItem { get; set; }
        public IColumnItem NextColumnItem { get; set; }

        // Create a game board with parameter 
        public GameBoard(int rowsCount, int columnsCount, int colorCount, bool animation)
        {
            if (rowsCount < MinRowsCount || rowsCount > MaxRowsCount)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));

            if (columnsCount < MinColumnsCount || columnsCount > MaxColumnsCount)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));

            if (colorCount < MinColorsCount || colorCount > MaxColorsCount)
                throw new ArgumentOutOfRangeException(nameof(rowsCount));


            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            ColorsCount = colorCount;
            ShowAnimation = animation;

            _storageItems = new StorageItems();
        }

        // Initializing and start game
        public void StartGame()
        {
            Score = 0;
            _storageItems.Clear();
            createNewColumnItems();
        }

        // Stop game
        public void StopGame()
        {
            ColumnItem = null;
            NextColumnItem = null;
            refreshUI();
        }

        //  Move item left
        public void MoveLeft()
        {
            if (ColumnItem == null)
                return;

            ColumnItem.Column--;

            if (!validateMove())
            {
                ColumnItem.Column++;
            }
        }

        // Move item right
        public void MoveRight()
        {
            if (ColumnItem == null)
                return;
            ColumnItem.Column++;

            if (!validateMove())
            {
                ColumnItem.Column--;
            }
        }

        // Rotate item color
        public void SwapItemUP()
        {
            ColumnItem?.SwapColorUP();
        }
        public void SwapItemDown()
        {
            ColumnItem?.SwapColorDown();
        }
        public void FlipItem()
        {
            if(ColumnItem is IFlippableColumnItem)
            {
                (ColumnItem as IFlippableColumnItem)?.FlipColors();
            }
        }
        public void RotateItem()
        {
            if (ColumnItem is IRotatableColumnItem rotatable)
            {
                rotatable.Rotate();
                if(!validateMove())
                {
                    rotatable.UnRotate();
                }
            }
        }

        // Move down falling column item and check valid item
        public async Task<bool> MoveDown()
        {
            if (ColumnItem == null)
                return true;
            ColumnItem.Row--;
            if (!validateMove())
            {
                musicPlayer.Play("BEEP.mp3");
                ColumnItem.Row++;

                // check game over
                if (ColumnItem.Row >= RowsCount - 2)
                {
                    return false;
                }
                _storageItems.Add(ColumnItem);
                bool isHorizonat = (ColumnItem is IRotatableColumnItem rotatable) ? rotatable.IsHorizontal : false; 
                ColumnItem = null;
                refreshUI();

                // check valid items, delete, and fall down
                await Task.Run(() =>
                {
                    //anime here
                    if (isHorizonat)
                    {
                        animateDeleteItems(_storageItems.GetCells().ToDictionary(kv => kv.Key, kv => kv.Value));
                        _storageItems.FallDownItems();
                    }
                    while (checkItemsForRemove())
                    {
                        refreshUI();
                        _storageItems.FallDownItems();
                    }
                });

                switchAndCreateColumnItems();
                refreshUI();
            }

            return true;
        }

        private bool Next = false;    

        // Switch next column item to falling item and create new next item
        public void switchAndCreateColumnItems()
        {
            ColumnItem = NextColumnItem;
            NextColumnItem = GenerateRandomColumnItem();
        }
        private IColumnItem GenerateRandomColumnItem()
        {
            var random = new Random();
            var itemType = (ColumnItemType)random.Next(3);

            switch (itemType)
            {
                case ColumnItemType.Standard:                  
                    if (!Next)
                    {
                        return new StandardColumnItem(RowsCount - 2, ColumnsCount / 2, ColorsCount);
                    }
                    else
                    {
                        return new StandardColumnItem(RowsCount, ColumnsCount / 2, ColorsCount);
                    }
                case ColumnItemType.Rotatable:                  
                    if (!Next)
                    {
                        return new RotatableColumnItem(RowsCount - 2, ColumnsCount / 2, ColorsCount);
                    }
                    else
                    {
                        return new RotatableColumnItem(RowsCount, ColumnsCount / 2, ColorsCount);
                    }
                case ColumnItemType.Flippable:                  
                    if (!Next)
                    {
                        return new FlippableColumnItem(RowsCount - 2, ColumnsCount / 2, ColorsCount);
                    }
                    else
                    {
                        return new FlippableColumnItem(RowsCount, ColumnsCount / 2, ColorsCount);
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        // Create next column item and falling column item
        private void createNewColumnItems()
        {
            ColumnItem = GenerateRandomColumnItem();
            Next = true;
            NextColumnItem = GenerateRandomColumnItem();
            Next = false;
        }

        // Validate the column item move 
        private bool validateMove()
        {
            if(ColumnItem is IRotatableColumnItem rotatebleColum && rotatebleColum.IsHorizontal)            
                if (ColumnItem.Row < 0 || (ColumnItem.Column + 1) > (ColumnsCount - 1) || (ColumnItem.Column - 1 ) < 0)
                    return false;
            

            if (ColumnItem.Row < 0 || ColumnItem.Column > (ColumnsCount - 1) || ColumnItem.Column < 0)
                return false;


            return !_storageItems.Contains(ColumnItem.Row, ColumnItem.Column, ColumnItem);
        }

        // check items for remove from board
        private bool checkItemsForRemove()
        {
            var dicDelete = _storageItems.GetItemsForDelete();

            Score += dicDelete.Count * 10;

            var ret = dicDelete.Count > 0;

            if (ret && ShowAnimation)
            {
                animateDeleteItems(dicDelete);
            }

            _storageItems.Remove(dicDelete);

            return ret;
        }


        // Animate deleted item (change color to transparent and back )
        private void animateDeleteItems(Dictionary<int, ItemColor> dicDelete)
        {
            musicPlayer.Play("StageClear.wav");
            for (int i = 0; i < 4; i++)
            {
                _storageItems.SetItemsTransparentColor(dicDelete);
                refreshUI();
                Thread.Sleep(100);

                _storageItems.SetItemsColor(dicDelete);
                refreshUI();
                Thread.Sleep(100);
            }
        }

        // test method for random fill board
        public void FillBoard_NOT_USE()
        {
            for (int row = 0; row < RowsCount - 6; row++)
            {
                for (int col = 0; col < ColumnsCount; col ++)
                {
                    _storageItems.AddItem(row, col, ColorHelper.GetRandomColor(ColorsCount));
                }
            }
        }
        
        // UI refresh request 
        private void refreshUI()
        {
            NeedRefreshVisualControl?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
