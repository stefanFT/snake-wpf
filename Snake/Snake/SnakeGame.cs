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

    public class SnakeGame
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Random _random = new Random();

        private readonly int _snakePieceSize = 20;
        private readonly Piece _head;
        private readonly List<Piece> _tail = new List<Piece>();
        private readonly double _speed = 0.125;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _isPaused = true;

        public SnakeGame(int width, int height)
        {
            _width = width;
            _height = height;

            
            _head = new Piece(x: 0, y: 0, _snakePieceSize);
            this.Apple = new Piece(300, 300, _snakePieceSize * 2);
        }

        public Piece Apple { get; }

        public int NumberOfHits { get; private set; }

        public IEnumerable<Piece> Pieces => new List<Piece> { _head }.Concat(_tail);

        public void Start()
        {
            _isPaused = false;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (!_isPaused)
            {
                _stopwatch.Restart();
            }
        }

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

            if (_head.Direction != oppositeDirection)
            {
                _head.Direction = newDirection;
            }
        }

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
                AddPiece();
            }
        }

        private void MoveAppleToRandomPosition()
        {
            Apple.X = _random.Next(0 + Apple.HalfSize, _width - Apple.HalfSize + 1);
            Apple.Y = _random.Next(0 + Apple.HalfSize, _height - Apple.HalfSize + 1);
        }

        private void AddPiece()
        {
            var currentLast = _tail.Any() ? _tail.Last() : _head;
            var newLast = new Piece(currentLast.X, currentLast.Y, _snakePieceSize);

            switch (currentLast.Direction)
            {
                case Direction.Up:
                    newLast.Y += _snakePieceSize;
                    break;

                case Direction.Down:
                    newLast.Y -= _snakePieceSize;
                    break;

                case Direction.Right:
                    newLast.X += _snakePieceSize;
                    break;

                case Direction.Left:
                    newLast.X -= _snakePieceSize;
                    break;

                default:
                    throw new NotSupportedException($"Direction: {currentLast} is not supported!");
            }

            _tail.Add(newLast);
        }

        private void UpdateHead()
        {
            var delta = _stopwatch.ElapsedMilliseconds;
            _stopwatch.Restart();

            var displacement = (int)(_speed * delta);

            switch (_head.Direction)
            {
                case Direction.Up:
                    _head.Y -= displacement;
                    break;

                case Direction.Down:
                    _head.Y += displacement;
                    break;

                case Direction.Right:
                    _head.X += displacement;
                    break;

                case Direction.Left:
                    _head.X -= displacement;
                    break;

                default:
                    throw new NotSupportedException($"The direction: {_head.Direction} is not supported.");
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
                piece.Direction = next.Direction;
            }

            if (_tail.Any())
            {
                _tail[0].X = _head.X;
                _tail[0].Y = _head.Y;
            }
        }
    }
}
