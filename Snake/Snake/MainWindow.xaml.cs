using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public readonly Canvas _head;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _head = new Canvas();
            Canvas.SetLeft(_head, 0);
            Canvas.SetTop(_head, 0);
            _head.Background = Brushes.SkyBlue;
            _head.Height = 100;
            _head.Width = 100;
            //Canvas.SetLeft(head, 200);
            //Canvas.SetTop(head, 500);

            var fps = 60d;
            var mainTimer = new Timer((1 / fps) * 1000);
            mainTimer.Elapsed += MainTimer_Elapsed;
            mainTimer.Start();

            //head = new Point(300, 300);

            //Board.Children.Add(testButton);
            Board.Children.Add(_head);
        }

        private void MainTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateHeadPosition();
        }

        public void UpdateHeadPosition()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var left = Canvas.GetLeft(this._head);
                Canvas.SetLeft(this._head, left + 1);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
