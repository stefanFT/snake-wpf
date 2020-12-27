using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            //Canvas.SetLeft(head, 200);
            //Canvas.SetTop(head, 500);

            //var d = new GeometryDrawing();
            //d.Geometry.Transform = new TranslateTransform(offset x, offset y);
            
            _snake = new SnakeModel();

            var fps = 30d;
            var mainTimer = new Timer((1 / fps) * 1000);
            mainTimer.Elapsed += MainTimer_Elapsed;
            mainTimer.Start();

            //head = new Point(300, 300);

            //Board.Children.Add(testButton);
            Board.Children.Add(_head);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Draw()
        {
            Canvas.SetLeft(this._head, this._snake.Head.X);
            Canvas.SetTop(this._head, this._snake.Head.Y);
        }

        private void MainTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this._snake.UpdatePosition();

            if (Application.Current is Application application)
            {
                application.Dispatcher.Invoke(() => this.Draw());
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
