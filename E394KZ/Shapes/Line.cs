namespace E394KZ.Shapes
{
    internal record Line : BaseShape
    {
        public uint EndX { get; init; }
        public uint EndY { get; init; }

        public Line(string name, uint x, uint y, uint endX, uint endY, ConsoleColor color) : base(name, x, y, color)
        {
            EndX = endX;
            EndY = endY;
        }

        public override void Draw(Canvas canvas)
        {
            //lehet jobban jartam volna sima int hasznalataval?
            int x1 = (int)X;
            int x2 = (int)EndX;
            int y1 = (int)Y;
            int y2 = (int)EndY;


            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                canvas[(uint)x1, (uint)y1] = Color;
                if (x1 == x2 && y1 == y2)
                    break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }
        public override string GetShapeName()
        {
            return "line";
        }
    }
}
