using System;

namespace Game.Core
{
    public struct IntPoint
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object other)
        {
            if (other is IntPoint otherPoint)
            {
                return X == otherPoint.X && Y == otherPoint.Y;
            }

            return base.Equals(other);
        }

        public static IntPoint operator +(IntPoint a, IntPoint b) => new IntPoint(a.X + b.X, a.Y + b.Y);
        public static IntPoint operator -(IntPoint a, IntPoint b) => new IntPoint(a.X - b.X, a.Y - b.Y);

        public override string ToString()
        {
            return string.Format("{0}, {1}", X, Y);
        }

        public int MaxDistanceTo(IntPoint other)
        {
            var direction = other - this;
            var x = Math.Abs(direction.X);
            var y = Math.Abs(direction.Y);
            return Math.Max(x, y);
        }

        public IntPoint UnitDirection()
        {
            if (Math.Abs(X) > Math.Abs(Y))
            {
                return new IntPoint(Math.Sign(X), 0);
            }
            return new IntPoint(0, Math.Sign(Y));
        }
    }
}