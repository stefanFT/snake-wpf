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
        private readonly SnakePiece _head = new SnakePiece(20, 0, 0);
        private readonly List<SnakePiece> _tail = new List<SnakePiece>();
        private readonly double _speed = 0.125;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Direction _direction = Direction.Down;

        public SnakeModel()
        {
            var blockSize = 20;
            for (int i = 1; i < 10; i++) 
            {
                this._tail.Add(new SnakePiece(blockSize, 0, blockSize * i));
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
            };

            if (_direction != oppositeDirection)
            {
                this._direction = newDirection;
            }
        }

        public IEnumerable<SnakePiece> Pieces => new List<SnakePiece> { this._head }.Concat(this._tail); 

        public void UpdatePosition()
        {
            this.UpdateTail();
            this.UpdateHead();
        }

        private void UpdateHead()
        {
            var delta = this._stopwatch.ElapsedMilliseconds;
            this._stopwatch.Restart();

            var displacement = (int)(this._speed * delta);

            switch (_direction)
            {
                case Direction.Up:
                    this._head.Y -= displacement;
                    break;

                case Direction.Right:
                    this._head.X += displacement;
                    break;

                case Direction.Down:
                    this._head.Y += displacement;
                    break;

                case Direction.Left:
                    this._head.X -= displacement;
                    break;

                default:
                    throw new NotSupportedException($"The direction: {this._direction} is not supported.");
            }
        }

        private void UpdateTail()
        {
            for (int i = this._tail.Count - 1; i > 0; i--)
            {
                var piece = this._tail[i];
                var next = this._tail[i - 1];
                piece.X = next.X;
                piece.Y = next.Y;
            }

            if (_tail.Any())
            {
                this._tail[0].X = this._head.X;
                this._tail[0].Y = this._head.Y;
            }
        }

        public class SnakePiece
        {
            public SnakePiece(int length, int x, int y)
            {
                this.Length = length;
                this.X = x;
                this.Y = y;
            }

            public int Length { get; }

            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}
