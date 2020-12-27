using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Collections.Generic;

namespace Snake
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public readonly Canvas _head;
        private readonly SnakeModel _snake;
        private readonly List<Canvas> _canvasCollection = new List<Canvas>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _snake = new SnakeModel();

            var fps = 30d;
            var timer = new DispatcherTimer(DispatcherPriority.Render, Application.Current.Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(1 / fps);
            timer.Start();
            timer.Tick += MainEvent_Handler;
        }

        private void MainEvent_Handler(object sender, EventArgs e)
        {
            this._snake.UpdatePosition();
            Draw();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void Draw()
        {
            // Clean up
            foreach (var canvas in this._canvasCollection)
            {
                Board.Children.Remove(canvas);
            }

            this._canvasCollection.Clear();

            foreach (var piece in this._snake.Pieces)
            {
                var canvas = new Canvas();
                canvas.Height = 20;
                canvas.Width = 20;
                canvas.Background = Brushes.Green;

                Canvas.SetLeft(canvas, piece.X);
                Canvas.SetTop(canvas, piece.Y);

                this._canvasCollection.Add(canvas);

                Board.Children.Add(canvas);
            }
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
                this._snake.SetDirection(direction.Value);
            }
        }
    }
}
