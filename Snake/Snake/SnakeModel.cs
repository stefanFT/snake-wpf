using Snake.GameElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Snake
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    public class SnakeModel
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Random _random = new Random();

        private readonly Piece _head;
        private readonly List<Piece> _tail = new List<Piece>();
        private readonly double _speed = 0.125;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _isPaused;
        private Direction _direction = Direction.Down;

        public SnakeModel(int width, int height)
        {
            _width = width;
            _height = height;

            var blockSize = 20;

            _head = new Piece(x: 0, y: 0, blockSize);

            for (int i = 1; i < 10; i++) 
            {
                _tail.Add(new Piece(x: blockSize, y: blockSize * i, size: blockSize));
            }

            this.Apple = new Piece(300, 300, blockSize * 2);
        }

        public Piece Apple { get; }

        public void SetDirection(Direction newDirection)
        {
            var oppositeDirection = newDirection switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new NotSupportedException(),
            };

            if (_direction != oppositeDirection)
            {
                _direction = newDirection;
            }
        }

        public int NumberOfHits { get; private set; }

        public IEnumerable<Piece> Pieces => new List<Piece> { _head }.Concat(_tail);

        public void UpdatePosition()
        {
            if (_isPaused)
            {
                return;
            }

            UpdateTail();
            UpdateHead();

            if (_head.IsColliding(Apple))
            {
                NumberOfHits++;
                MoveAppleToRandomPosition();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (!_isPaused)
            {
                _stopwatch.Restart();
            }
        }

        private void MoveAppleToRandomPosition()
        {
            Apple.X = _random.Next(0, _width + 1); ;
            Apple.Y = _random.Next(0, _height + 1); ;
        }

        private void UpdateHead()
        {
            var delta = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();

            var displacement = (int)(_speed * delta);

            switch (_direction)
            {
                case Direction.Up:
                    _head.Y -= displacement;
                    break;

                case Direction.Right:
                    _head.X += displacement;
                    break;

                case Direction.Down:
                    _head.Y += displacement;
                    break;

                case Direction.Left:
                    _head.X -= displacement;
                    break;

                default:
                    throw new NotSupportedException($"The direction: {_direction} is not supported.");
            }
        }

        private void UpdateTail()
        {
            for (int i = _tail.Count - 1; i > 0; i--)
            {
                var piece = _tail[i];
                var next = _tail[i - 1];
                piece.X = next.X;
                piece.Y = next.Y;
            }

            if (_tail.Any())
            {
                _tail[0].X = _head.X;
                _tail[0].Y = _head.Y;
            }
        }
    }
}
