using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System;

namespace Snake
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Canvas _appleModel;
        private Canvas _snakeModel;
        private readonly SnakeGame _snakeGame;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            var boardHeight = 600;
            var boardWidth = 600;

            _snakeGame = new SnakeGame(width: boardWidth, height: boardHeight);

            var fps = 30d;
            var timer = new DispatcherTimer(DispatcherPriority.Render, Application.Current.Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(1 / fps);
            timer.Start();
            timer.Tick += MainEvent_Handler;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Points => _snakeGame.NumberOfHits;

        private void MainEvent_Handler(object sender, EventArgs e)
        {
            this._snakeGame.UpdatePosition();
            DrawSnake();
            DrawApple();
        }

        private void DrawSnake()
        {
            Board.Children.Remove(_snakeModel);
            _snakeModel = new Canvas();

            foreach (var piece in this._snakeGame.Pieces)
            {
                var canvas = new Canvas();
                canvas.Height = 20;
                canvas.Width = 20;
                canvas.Background = Brushes.Green;

                Canvas.SetLeft(canvas, piece.Left);
                Canvas.SetTop(canvas, piece.Top);

                _snakeModel.Children.Add(canvas);
            }
            
            Board.Children.Add(_snakeModel);
        }

        private void DrawApple()
        {
            Board.Children.Remove(_appleModel);

            _appleModel = new Canvas();
            _appleModel.Background = Brushes.Red;
            _appleModel.Width = _snakeGame.Apple.Size;
            _appleModel.Height = _snakeGame.Apple.Size;

            Canvas.SetLeft(_appleModel, _snakeGame.Apple.Left);
            Canvas.SetTop(_appleModel, _snakeGame.Apple.Top);

            Board.Children.Add(_appleModel);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Direction? direction = e.Key switch
            {
                Key.Up => Direction.Up,
                Key.Right => Direction.Right,
                Key.Down => Direction.Down,
                Key.Left => Direction.Left,
                _ => null,
            };

            if (direction.HasValue)
            {
                this._snakeGame.SetDirection(direction.Value);
            }

            if (e.Key == Key.P)
            {
                this._snakeGame.TogglePause();
            }
        }
    }
}
