namespace E394KZ.Shapes
{
    internal record Rectangle : BaseShape
    {
        public uint Width { get; init; }
        public uint Height { get; init; }

        public Rectangle(string name, uint x, uint y, ConsoleColor color, uint width, uint height) : base(name, x, y, color)
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
    }
}
