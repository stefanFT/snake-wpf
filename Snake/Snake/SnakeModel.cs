using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            this._tail.Add(new SnakePiece(blockSize, 0, blockSize));
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

        public SnakePiece Head => this._head;

        public List<SnakePiece> Tail => this._tail;

        public void UpdatePosition()
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

        public class SnakePiece
        {
            public SnakePiece(
                int length,
                int x,
                int y)
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
