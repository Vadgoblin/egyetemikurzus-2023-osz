namespace E394KZ.Shapes
{
    internal record Triangle : BaseShape
    {
        public uint V1X => X;
        public uint V1Y => Y;

        public uint V2X { get; init; }
        public uint V2Y { get; init; }

        public uint V3X { get; init; }
        public uint V3Y { get; init; }

        public Triangle(string name, uint v1x, uint v1y, uint v2x, uint v2y, uint v3x, uint v3y, ConsoleColor color) : base(name, v1x, v1y, color)
        {
            V2X = v2x;
            V2Y = v2y;
            V3X = v3x;
            V3Y = v3y;
        }

        public override void Draw(Canvas canvas)
        {
            var grid = new bool[canvas.Width, canvas.Height];

            DrawLine(grid, (int)V1X, (int)V1Y, (int)V2X, (int)V2Y);
            DrawLine(grid, (int)V1X, (int)V1Y, (int)V3X, (int)V3Y);
            DrawLine(grid, (int)V2X, (int)V2Y, (int)V3X, (int)V3Y);

            for(int y = 0; y < canvas.Height; y++)
            {
                int first = FindFirstTrueIndex(grid, y);
                int last = FindLastTrueIndex(grid, y);

                if (first != last)
                {
                    for(int x = first; x < last; x++)
                    {
                        grid[x,y] = true;
                    }
                }
            }

            for (int x = 0; x < canvas.Width; x++)
            {
                for (int y = 0; y < canvas.Height; y++)
                {
                    if (grid[x, y])
                    {
                        canvas[(uint)x, (uint)y] = Color;
                    }
                }
            }
        }

        private static void DrawLine(bool[,] grid, int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;

            int err = dx - dy;
            int currentX = x1;
            int currentY = y1;

            while (true)
            {
                grid[currentX, currentY] = true;

                if (currentX == x2 && currentY == y2)
                    break;

                int err2 = 2 * err;
                if (err2 > -dy)
                {
                    err -= dy;
                    currentX += sx;
                }
                if (err2 < dx)
                {
                    err += dx;
                    currentY += sy;
                }
            }
        }
        private static int FindFirstTrueIndex(bool[,] grid,int y)
        {
            for(int i = 0; i < grid.GetLength(1); i++)
            {
                if (grid[i, y] == true) return i;
            }
            return -1;
        }
        private static int FindLastTrueIndex(bool[,] grid, int y)
        {
            for (int i = grid.GetLength(1)-1; i >=1 ; i--)
            {
                if (grid[i, y] == true) return i;
            }
            return -1;
        }

        public override string GetShapeName()
        {
            return "triangle";
        }
    }
}
