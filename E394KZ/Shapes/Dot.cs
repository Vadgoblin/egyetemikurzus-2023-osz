namespace E394KZ.Shapes
{
    internal record Dot : BaseShape
    {
        public Dot(string name, uint x, uint y, ConsoleColor color) : base(name, x, y,color)
        {
        }
        public override void Draw(ConsoleColor?[,] canvas)
        {
            canvas[x, y] = color;
        }
    }
}
