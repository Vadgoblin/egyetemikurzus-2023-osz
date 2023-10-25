using System.Drawing;
using System;

namespace E394KZ.Shapes
{
    internal record Triangle : BaseShape
    {
        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(uint x, uint y)
            {
                X = (int)x;
                Y = (int)y;
            }
        }

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
        //public Triangle(string name, uint v1x, uint v1y, uint v2x, uint v2y, uint v3x, uint v3y, ConsoleColor color) : this(name, v1x, v1y, color, v2x, v2y, v3x, v3y) { }

        public override void Draw(Canvas canvas)
        {
            Point p1 = new Point(V1X, V1Y);
            Point p2 = new Point(V2X, V2Y);
            Point p3 = new Point(V3X, V3Y);

            // Sort the vertices by y-coordinate to ensure p1 is the top vertex, p2 is the middle, and p3 is the bottom
            if (p2.Y < p1.Y)
                Swap(ref p1, ref p2);
            if (p3.Y < p1.Y)
                Swap(ref p1, ref p3);
            if (p3.Y < p2.Y)
                Swap(ref p2, ref p3);

            // Calculate the slopes of the two lines
            float invSlope1 = (float)(p2.X - p1.X) / (p2.Y - p1.Y);
            float invSlope2 = (float)(p3.X - p1.X) / (p3.Y - p1.Y);

            // Initialize the starting and ending x-coordinates for the two lines
            float curX1 = p1.X;
            float curX2 = p1.X;

            for (int scanlineY = p1.Y; scanlineY <= p2.Y; scanlineY++)
            {
                DrawHorizontalLine(canvas, (int)curX1, (int)curX2, scanlineY, Color);
                curX1 += invSlope1;
                curX2 += invSlope2;
            }

            // Calculate the new slope for the bottom line
            invSlope1 = (float)(p3.X - p2.X) / (p3.Y - p2.Y);
            curX1 = p2.X;

            for (int scanlineY = p2.Y + 1; scanlineY <= p3.Y; scanlineY++)
            {
                DrawHorizontalLine(canvas, (int)curX1, (int)curX2, scanlineY, Color);
                curX1 += invSlope1;
                curX2 += invSlope2;
            }
        }
        private void DrawHorizontalLine(Canvas canvas, int x1, int x2, int y, ConsoleColor color)
        {
            for (int x = x1; x <= x2; x++)
            {
                if (x >= 0 && x < canvas.Width && y >= 0 && y < canvas.Height)
                {
                    canvas[(uint)x, (uint)y] = color;
                }
            }
        }
        static void Swap(ref Point p1, ref Point p2)
        {
            Point temp = p1;
            p1 = p2;
            p2 = temp;
        }
        public override string GetShapeName()
        {
            return "triangle";
        }
    }
}
