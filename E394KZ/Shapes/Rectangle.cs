namespace E394KZ.Shapes
{
    internal record Rectangle : BaseShape
    {
        public uint Width { get; init; }
        public uint Height { get; init; }

        public Rectangle(string name, uint x, uint y, uint width, uint height, ConsoleColor color) : base(name, x, y, color)
        {
            Width = width;
            Height = height;
        }
        public override void Draw(Canvas canvas)
        {
            for (uint x = X; x < X + Width && x < canvas.Width; x++)
            {
                for (uint y = Y; y < Y + Height && y < canvas.Height; y++)
                {
                    canvas[x, y] = Color;
                }
            }
        }
        public override string GetShapeName()
        {
            return "rectangle";
        }
    }
}
