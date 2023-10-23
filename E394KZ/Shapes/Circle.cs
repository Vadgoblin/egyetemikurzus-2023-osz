namespace E394KZ.Shapes
{
    internal record Circle : BaseShape
    {
        public uint radius { get; init; }
        public Circle(string name, uint x, uint y, ConsoleColor color, uint radius) : base(name, x, y, color)
        {
            this.radius = radius;
        }
        public override void Draw(ConsoleColor?[,] canvas)
        {
            canvas[x, y] = color;
        }
    }
}
