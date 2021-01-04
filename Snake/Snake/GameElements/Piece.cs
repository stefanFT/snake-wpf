
using System.Diagnostics;

namespace Snake.GameElements
{
    public class Piece
    {
        public Piece(int x, int y, int size)
        {
            X = x;
            Y = y;
            Size = size;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Size { get; set; }

        public int HalfSize => Size / 2;

        public int Left => LeftInternal();

        private int LeftInternal()
        {
            var x = X;
            var halfSize = HalfSize;
            Trace.WriteLine($"X: {x}, halfSize: {halfSize}, Size: {Size}");
            return X - HalfSize;
        }

        public int Right => X + HalfSize;

        public int Top => Y - HalfSize;

        public int Bottom => Y + HalfSize;

        public bool IsColliding(Piece other)
        {
            var isVertical = (other.Left <= this.Left && this.Left <= other.Right) ||
                             (this.Left <= other.Left && other.Left <= this.Right);

            var isHorizontal = (other.Top <= this.Top && this.Top <= other.Bottom) ||
                               (this.Top <= other.Top && other.Top <= this.Bottom);

            return isVertical && isHorizontal;
        }
    }
}
