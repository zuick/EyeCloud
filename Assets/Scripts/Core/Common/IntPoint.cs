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

        public static IntPoint operator +(IntPoint a, IntPoint b) => new IntPoint(a.X + b.Y, b.Y + b.Y);
        public static IntPoint operator -(IntPoint a, IntPoint b) => new IntPoint(a.X - b.Y, b.Y - b.Y);
    }
}