namespace E394KZ.Shapes
{
    internal abstract record BaseShape
    {
        public string name { get; init; }

        public uint x { get; init; }
        public uint y { get; init; }

        public ConsoleColor color { get; init; }

        public BaseShape(string name, uint x, uint y, ConsoleColor color)
        {
            this.name = name;
            this.x = x;
            this.y = y;
            this.color = color;
        }

        public abstract void Draw(ConsoleColor?[,] canvas);
    }
}
