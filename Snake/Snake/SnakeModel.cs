using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

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
        private readonly Block _head = new Block();
        private readonly List<Block> _tail = new List<Block>();
        private readonly double _speed = 0.12;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Direction _direction = Direction.Down;

        public void SetDirection(Direction newDirection)
        {
            this._direction = newDirection;
        }

        public Block Head => this._head;

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

        public class Block
        {
            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}
