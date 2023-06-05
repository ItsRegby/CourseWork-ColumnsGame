using ColumnsGame.Controls;
using ColumnsGame.Enums;
using ColumnsGame.Model;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;

namespace ColumnsGame.ViewModel
{
    public partial class GameWindowViewModel
    {
        public DelegateCommand StartGameCommand { get; set; }
        public DelegateCommand PauseGameCommand { get; set; }
        public DelegateCommand StopGameCommand { get; set; }
        public DelegateCommand<UpDownButtonChange?> RowsChangeCommand { get; set; }
        public DelegateCommand<UpDownButtonChange?> ColumnsChangeCommand { get; set; }
        public DelegateCommand<UpDownButtonChange?> ColorChangeCommand { get; set; }



        private void createCommands()
        {
            StartGameCommand = new DelegateCommand(startGame, startGameCanExecute);
            StopGameCommand = new DelegateCommand(stopGame, stopGameCanExecute);
            PauseGameCommand = new DelegateCommand(pauseGame, pauseGameCanExecute);

            RowsChangeCommand = new DelegateCommand<UpDownButtonChange?>(rowsChange, rowsChangeCanExecute);
            ColumnsChangeCommand = new DelegateCommand<UpDownButtonChange?>(columnsChange, columnsChangeCanExecute);
            ColorChangeCommand = new DelegateCommand<UpDownButtonChange?>(colorChange, colorChangeCanExecute);
        }



        private void startGame()
        {
            musicPlayer.Play("Title.mp3");
            createNewGameBoard();

            GameRunnig = true;
            GameOver = false;
            SpeedGame = StartSpeed;
            Board.StartGame();

            
            // test mode
            if (Keyboard.IsKeyDown(Key.LeftShift))
                Board.FillBoard_NOT_USE();

            updateButtonCanExecute();
        }
        

        private bool startGameCanExecute()
        {
            return !GameRunnig;
        }


        private void stopGame()
        {
            GameRunnig = false;
            musicPlayer.Play("Pause.mp3");
            if (MessageBox.Show("Finish the game?", "Stop game", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                GameOver = true;
                Board.StopGame();
                musicPlayer.Play("Game_Over.mp3");
            }
            else
            {
                GameRunnig = true;
                musicPlayer.Play("Title.mp3");
            }

            updateButtonCanExecute();
        }

        private bool stopGameCanExecute()
        {
            return GameRunnig;
        }


        private void pauseGame()
        {
            GameRunnig = false;
            musicPlayer.Play("Pause.mp3");
            MessageBox.Show("Continue the game?", "Pause game", MessageBoxButton.OK, MessageBoxImage.Question);
            GameRunnig = true;
            musicPlayer.Play("Title.mp3");
            updateButtonCanExecute();
        }

        private bool pauseGameCanExecute()
        {
            return GameRunnig;
        }


        private void updateButtonCanExecute()
        {
            StartGameCommand.RaiseCanExecuteChanged();
            StopGameCommand.RaiseCanExecuteChanged();
            PauseGameCommand.RaiseCanExecuteChanged();
        }


        private void rowsChange(UpDownButtonChange? change)
        {
            if (change == null)
                return;

            RowsCount += change == UpDownButtonChange.Up ? 1 : -1;

            RowsChangeCommand?.RaiseCanExecuteChanged();
            createNewGameBoard();
        }

        private bool rowsChangeCanExecute(UpDownButtonChange? change)
        {
            if (change == null)
                return false;

            return change == UpDownButtonChange.Up ? RowsCount < GameBoard.MaxRowsCount : RowsCount > GameBoard.MinRowsCount;
        }

        public void columnsChange(UpDownButtonChange? change)
        {
            if (change == null)
                return;

            ColumnsCount += change == UpDownButtonChange.Up ? 1 : -1;

            ColumnsChangeCommand?.RaiseCanExecuteChanged();
            createNewGameBoard();
        }

        public bool columnsChangeCanExecute(UpDownButtonChange? change)
        {
            if (change == null)
                return false;

            return change == UpDownButtonChange.Up ? ColumnsCount < GameBoard.MaxColumnsCount : ColumnsCount > GameBoard.MinColumnsCount;
        }

        public void colorChange(UpDownButtonChange? change)
        {
            if (change == null)
                return;

            ColorsCount += change == UpDownButtonChange.Up ? 1 : -1;

            ColorChangeCommand?.RaiseCanExecuteChanged();
            createNewGameBoard();
        }

        public bool colorChangeCanExecute(UpDownButtonChange? change)
        {
            if (change == null)
                return false;

            return change == UpDownButtonChange.Up ? ColorsCount < GameBoard.MaxColorsCount : ColorsCount > GameBoard.MinColorsCount;
        }
    }
}

