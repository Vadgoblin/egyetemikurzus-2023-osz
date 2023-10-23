namespace E394KZ.Shapes
{
    internal abstract record BaseShape
    {
        public string Name { get; init; }

        public uint X { get; init; }
        public uint Y { get; init; }

        public ConsoleColor Color { get; init; }

        public BaseShape(string name, uint x, uint y, ConsoleColor color)
        {
            Name = name;
            X = x;
            Y = y;
            Color = color;
        }

        public abstract void Draw(Canvas canvas);
    }
}
