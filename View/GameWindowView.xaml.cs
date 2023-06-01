using ColumnsGame.ViewModel;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using ColumnsGame.Controls;

namespace ColumnsGame.View
{
    public partial class GameWindowView
    {
        private DateTime _lastMoveTime;
        private DateTime _lastMoveDownTime;

        public GameWindowView()
        {
            InitializeComponent();
            initView();
        }

        private void initView()
        {
            GameWindowModel = new GameWindowViewModel();
            _lastMoveTime = DateTime.Now;
            _lastMoveDownTime = DateTime.Now;

            GameWindowModel.NeedRefreshVisualControl += GameWindow_NeedRefreshVisualControl;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            KeyDown += GameWindowView_KeyDown;
        }


        private GameWindowViewModel GameWindowModel
        {
            get => DataContext as GameWindowViewModel;
            set => DataContext = value;
        }


        //  Repaint function
        private void GameWindow_NeedRefreshVisualControl(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => BoardControl.InvalidateVisual());
        }


        // CompositionTarget Rendering 
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!GameWindowModel.GameRunnig)
                return;

            var nowTime = DateTime.Now;
            var moved = false;

            // set delay move item down
            var gameDelayMove = 1000 - (GameWindowModel.SpeedGame * 50);

            // correct key repeat time for fast speed
            var repeatKeyTime = 100;
            if (gameDelayMove <= 150)
                repeatKeyTime = gameDelayMove / 2;

            // ESC - pause
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                GameWindowModel.PauseGameCommand.Execute();
            }


            // Move left
            if (Keyboard.IsKeyDown(Key.Left) && _lastMoveTime.AddMilliseconds(repeatKeyTime) < nowTime)
            {
                moved = true;
                GameWindowModel.MoveLeft();
            }


            // Move right
            if (Keyboard.IsKeyDown(Key.Right) && _lastMoveTime.AddMilliseconds(repeatKeyTime) < nowTime)
            {
                moved = true;
                GameWindowModel.MoveRight();
            }


            // Swap color UP
            if ((Keyboard.IsKeyDown(Key.Q)) && _lastMoveTime.AddMilliseconds(gameDelayMove / 5) < nowTime)
            {
                moved = true;
                GameWindowModel.SwapUP();
            }
            // Swap color Down
            if ((Keyboard.IsKeyDown(Key.W)) && _lastMoveTime.AddMilliseconds(gameDelayMove / 5) < nowTime)
            {
                moved = true;
                GameWindowModel.SwapDown();
            }
            // Flip color
            if ((Keyboard.IsKeyDown(Key.E)) && _lastMoveTime.AddMilliseconds(gameDelayMove / 5) < nowTime)
            {
                moved = true;
                GameWindowModel.FlipColor();
            }
            // Rotate figure
            if ((Keyboard.IsKeyDown(Key.R)) && _lastMoveTime.AddMilliseconds(gameDelayMove / 5) < nowTime)
            {
                moved = true;
                GameWindowModel.Rotate();
            }


            // move down key/timer
            if (_lastMoveDownTime.AddMilliseconds(Keyboard.IsKeyDown(Key.Down) ? gameDelayMove / 20 : gameDelayMove) < nowTime)
            {
                _lastMoveDownTime = DateTime.Now;
                GameWindowModel.MoveDown();
                BoardControl.InvalidateVisual();
            }

            
            // update last move time and repaint
            if (moved)
            {
                _lastMoveTime = DateTime.Now;
                BoardControl.InvalidateVisual();
            }
        }

        private void GameWindowView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
                setFullscreen();
        }


        private WindowState _lastWindowState;
        private void setFullscreen()
        {
            if (WindowStyle == WindowStyle.None)
            {
                WindowState = _lastWindowState;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                _lastWindowState = WindowState;
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
        }

    }
}
