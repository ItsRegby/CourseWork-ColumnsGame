using ColumnsGame.Controls;
using ColumnsGame.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ColumnsGame.ViewModel
{
    public partial class GameWindowViewModel: INotifyPropertyChanged
    {
        public event EventHandler NeedRefreshVisualControl;

        private int _rowsCount;
        private int _columnsCount;
        private int _colorsCount;
        private bool _showAnimation;
        private int _speedGame;
        private bool _gameRunnig;
        private bool _gameOver;
        private GameBoard _board;
        MusicPlayer musicPlayer = new MusicPlayer();

        // Get or Set GameBoard
        public GameBoard Board
        {
            get => _board;
            private set
            {
                _board = value;
                RaisePropertyChanged();
            }
        }

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

        // Get or Set Colors count board
        public int ColorsCount
        {
            get => _colorsCount;
            set
            {
                _colorsCount = value;
                RaisePropertyChanged();
            }
        }

        // Get or Set whether an animation is showed
        public bool ShowAnimation
        { 
            get => _showAnimation;
            set 
            {
                _showAnimation = value;

                if (Board != null)
                    Board.ShowAnimation = value;

                RaisePropertyChanged();
            }
        }

        // Get or Set speed game, recommend 0(slow) to 10(fast)
        public int SpeedGame
        {
            get => _speedGame;
            set
            {
                _speedGame = value;
                RaisePropertyChanged();
            }
        }


        // Get or Set whether the game is running (Run/Pause)
        public bool GameRunnig
        {
            get => _gameRunnig;
            set
            {
                _gameRunnig = value;
                RaisePropertyChanged();
                updateButtonCanExecute();
            }
        }


        // Get or Set whether the game over
        public bool GameOver
        {
            get => _gameOver;
            set
            {
                _gameOver = value;

                if (_gameOver)
                {
                    GameRunnig = false;
                    musicPlayer.Play("Game_Over.mp3");
                }

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(GameRunnig));
                updateButtonCanExecute();
            }
        }


        // Get or Set start speed game
        public int StartSpeed { get; set; }



        public GameWindowViewModel()
        {
            RowsCount = GameBoard.DefaultRowsCount;
            ColumnsCount = GameBoard.DefaultColumnsCount;
            ColorsCount = GameBoard.DefaultColorsCount;
            ShowAnimation = true;
            
            createCommands();
            createNewGameBoard();
        }


        private void createNewGameBoard()
        {
            Board = new GameBoard(RowsCount, ColumnsCount, ColorsCount, ShowAnimation);

            Board.NeedRefreshVisualControl += (s, e) =>
            {
                NeedRefreshVisualControl?.Invoke(s,e);
            };
        }


        public void MoveLeft()
        {
            Board.MoveLeft();
        }

        public void MoveRight()
        {
            Board.MoveRight();
        }

        public void SwapUP()
        {
            Board.SwapItemUP();
        }
        public void SwapDown()
        {
            Board.SwapItemDown();
        }
        public void FlipColor()
        {
            Board.FlipItem();
        }
        public void Rotate()
        {
            Board.RotateItem();
        }

        public async void MoveDown()
        {
            if (!await Board.MoveDown())
                GameOver = true; 

            // speed-up game
            SpeedGame = StartSpeed + (Board.Score / 250);

        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
