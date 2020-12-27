using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using System.Windows.Shapes;

namespace Snake
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public readonly Canvas _head;
        private readonly SnakeModel _snake;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _head = new Canvas();
            Canvas.SetLeft(_head, 0);
            Canvas.SetTop(_head, 0);
            _head.Background = Brushes.Black;
            _head.Height = 30;
            _head.Width = 30;

            _snake = new SnakeModel();

            var fps = 30d;
            var timer = new DispatcherTimer(DispatcherPriority.Render, Application.Current.Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(1 / fps);
            timer.Start();
            timer.Tick += MainEvent_Handler;

            Board.Children.Add(_head);
        }

        private void MainEvent_Handler(object sender, EventArgs e)
        {
            this._snake.UpdatePosition();
            Draw();
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void Draw()
        {
            Canvas.SetLeft(this._head, this._snake.Head.X);
            Canvas.SetTop(this._head, this._snake.Head.Y);

            for (int i = 0; i < this._snake.Tail.Count; i++)
            {
                var piece = this._snake.Tail[i];
                var rect = new Rectangle();
                rect.Width = 20;
                rect.Height = 20;
                rect.Fill = Brushes.Green;

                Board.Children.Add(rect);
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
