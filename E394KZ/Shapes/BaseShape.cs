namespace E394KZ.Shapes
{
    internal abstract class BaseShape
    {
        public required string Name { get; init; }

        public uint x { get; private set; }
        public uint y { get; private set; }

        public ConsoleColor color { get; private set; }

        public BaseShape(string name, uint x, uint y)
        {
            Name = name;
            this.x = x;
            this.y = y;
        }

        public abstract ConsoleColor?[,] Draw(ConsoleColor?[,] canvas);
    }
}
